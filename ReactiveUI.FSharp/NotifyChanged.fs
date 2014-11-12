namespace ReactiveUI.FSharp

open System
open System.Reactive.Linq
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Quotations.Patterns
open ReactiveUI
open Splat

// TODO - split up file
// TODO - fewer explicit types
// TODO - merge ICreatesObservableForProperty?

// TODO - these Expression and Reflection bits seem overly complicated

module Expression =
    let getName (expr : Expr) = 
        match expr with
            | PropertyGet(_, prop, _) -> prop.Name
            | FieldGet(_, field)      -> field.Name
            | _ -> raise (NotSupportedException(sprintf "Unsupported expression type: '%A'" (expr.Type)))

    let constantArg = function | Value(v, _) -> Some v | _ -> None 

    let getArgumentsArray (expr : Expr) =
        match expr with
            | PropertyGet(_, _, args) -> args |> List.choose constantArg
                                              |> Array.ofList 
                                              |> Some
            | _ -> None

    let rec getExpressionChain (expr : Expr) = 
        match expr with
            | PropertyGet(Some item, prop, _) -> expr :: (getExpressionChain item)
            | FieldGet(Some item, field)      -> expr :: (getExpressionChain item)
            | _                               -> [expr]


    let rewrite (expr : Expr) =
        let rec check expr =
            match expr with
                | PropertyGet(Some item, _, args) -> args |> List.map constantArg |> List.iter(function | Some _ -> () | None -> raise (NotSupportedException("Indexers must have constant indexes"))); check item
                | PropertyGet(None, _, args)      -> args |> List.map constantArg |> List.iter(function | Some _ -> () | None -> raise (NotSupportedException("Indexers must have constant indexes"))); ()
                | FieldGet(Some item, field)      -> check item
                | FieldGet(None, _)               -> ()
                | Var(_)                          -> ()
                | _ -> raise (NotSupportedException(sprintf "Unsupported expression type: '%A'" (expr.Type)))
        check expr
        expr

module Reflection = 
    let getValueFetcherOrThrow (memb : Expr) : (obj -> obj[] option -> obj) =
        match memb with
            | PropertyGet(_, prop, _) -> (fun o a -> prop.GetValue(o, defaultArg a null))
            | FieldGet(_, field)      -> (fun o _ -> field.GetValue(o))
            | _ -> raise (ArgumentException(sprintf "Type must have a property '%A'" memb))

    let rec tryGetValueForPropertyChain(current : obj, expressionChain : Expr list) : (bool * 'TValue) =
        match expressionChain with
        | expression :: tail when current <> null -> let args  = expression |> Expression.getArgumentsArray
                                                     tryGetValueForPropertyChain((getValueFetcherOrThrow(expression) current args), tail)
        | [lastExpression]                        -> let args  = lastExpression |> Expression.getArgumentsArray 
                                                     let value = getValueFetcherOrThrow (lastExpression) current args
                                                     (true, value :?> 'TValue)
        | _                                       -> (false, Unchecked.defaultof<'TValue>)

[<RequireQualifiedAccess>]
type NotifyChanged() =
    static let notifyFactoryCache =
        let calcFunc (t, s, b) _ = 
            let services = Locator.Current.GetServices<ReactiveUI.FSharp.ICreatesObservableForProperty>() 
            if (services |> Seq.isEmpty) then failwith ("Couldn't find a ICreatesObservableForProperty. This should never happen, your service locator is probably broken.")
            services |> Seq.maxBy(fun x -> x.GetAffinityForObject(t, s, b))
        new MemoizingMRUCache<_,_>(Func<_,_,_>(calcFunc), RxApp.BigCacheLimit)

    static let notifyForProperty(sender : obj, expression : Expr, beforeChange : bool) : IObservable<IObservedChange<obj, obj>> =
        let result = lock notifyFactoryCache (fun () -> notifyFactoryCache.Get((sender.GetType(), (expression |> Expression.getName), beforeChange)))
        result.GetNotificationForProperty(sender, expression, beforeChange);

    static let observedChangeFor(expression : Expr, sourceChange : IObservedChange<obj, obj>) : IObservedChange<obj, obj> =
        if (sourceChange.Value = null) then new ReactiveUI.FSharp.ObservedChange<obj, obj>(sourceChange.Value, expression) :> IObservedChange<obj, obj>
                                       else let value = Reflection.tryGetValueForPropertyChain(sourceChange.Value, [expression])
                                            new ReactiveUI.FSharp.ObservedChange<obj, obj>(sourceChange.Value, expression, value) :> IObservedChange<obj, obj>

    static let nestedObservedChanges(expression : Expr, sourceChange : IObservedChange<obj, obj>, beforeChange : bool) : IObservable<IObservedChange<obj, obj>> =
        // Make sure a change at a root node propogates events down
        let kicker = observedChangeFor(expression, sourceChange)

        // Handle null values in the chain
        if (sourceChange.Value = null) then Observable.Return(kicker)
                                       else notifyForProperty(sourceChange.Value, expression, beforeChange).
                                                Select(fun x -> ReactiveUI.FSharp.ObservedChange(x.Sender, expression, x.GetValue()) :> IObservedChange<_,_>).
                                                StartWith(kicker)

    static do
        // TODO - Hacky
        let registrations = ReactiveUI.FSharp.Registrations() :> IWantsToRegisterStuff
        registrations.Register(Action<Func<obj>,Type>(fun (f : Func<obj>) t -> Locator.CurrentMutable.RegisterConstant(f.Invoke(), t)))

    static member subscribeToExpressionChain(source : 'TSender, expression : Expr, ?beforeChange (* = false *), ?skipInitial (* = true *)) : IObservable<IObservedChange<'TSender, 'TValue>> =
        let beforeChange = defaultArg beforeChange false
        let skipInitial =  defaultArg skipInitial  true
        let (chain : Expr list) = expression |> Expression.rewrite |> Expression.getExpressionChain

        let mutable notifier = Observable.Return(new ReactiveUI.FSharp.ObservedChange<obj, obj>(null, expression, source) :> IObservedChange<_,_>)
        notifier <- chain |> Seq.fold (fun n expr -> n.Select(fun y -> nestedObservedChanges(expr, y, beforeChange)).Switch()) notifier 
        if skipInitial then notifier <- notifier.Skip(1)
        notifier <- notifier.Where(fun x -> x.Sender <> null)

        let r = notifier.Select(fun x -> match x.GetValue() with
                                         | :? 'TValue as value -> new ReactiveUI.FSharp.ObservedChange<'TSender, 'TValue>(source, expression, value) :> IObservedChange<'TSender, 'TValue> 
                                         | null                -> new ReactiveUI.FSharp.ObservedChange<'TSender, 'TValue>(source, expression, null)  :> IObservedChange<'TSender, 'TValue>
                                         | x -> raise (InvalidCastException(String.Format("Unable to cast from {0} to {1}.", x.GetType(), typeof<'TValue>))))

        r.DistinctUntilChanged(fun x -> x.Value)

    static member forProperty (this : 'TSender, property : Expr<'TSender->'TValue>, ?beforeChange (* = false *), ?skipInitial (* = true *)) =
        if (this = null) then raise (ArgumentNullException("Sender"))
            
        (* x => x.Foo.Bar.Baz;
         * 
         * Subscribe to This, look for Foo
         * Subscribe to Foo, look for Bar
         * Subscribe to Bar, look for Baz
         * Subscribe to Baz, publish to Subject
         * Return Subject
         * 
         * If Bar changes (notification fires on Foo), resubscribe to new Bar
         *  Resubscribe to new Baz, publish to Subject
         * 
         * If Baz changes (notification fires on Bar),
         *  Resubscribe to new Baz, publish to Subject
         *)

        match property with 
        | Lambda(_, expr) -> NotifyChanged.subscribeToExpressionChain<'TSender, 'TValue>(
                                this,
                                expr,
                                defaultArg beforeChange false,
                                defaultArg skipInitial true)
        | _               -> raise (ArgumentException(sprintf "Unsupported expression type %A" property.Type))

#if false
    public static class ReactiveNotifyPropertyChangedMixin
    {
        static ReactiveNotifyPropertyChangedMixin()
        {
            RxApp.EnsureInitialized();
        }

        /// <summary>
        /// ObservableForProperty returns an Observable representing the
        /// property change notifications for a specific property on a
        /// ReactiveObject. This method (unlike other Observables that return
        /// IObservedChange) guarantees that the Value property of
        /// the IObservedChange is set.
        /// </summary>
        /// <param name="property">An Expression representing the property (i.e.
        /// 'x => x.SomeProperty.SomeOtherProperty'</param>
        /// <param name="beforeChange">If True, the Observable will notify
        /// immediately before a property is going to change.</param>
        /// <returns>An Observable representing the property change
        /// notifications for the given property.</returns>
        public static IObservable<IObservedChange<TSender, TValue>> ObservableForProperty<TSender, TValue>(
                this TSender This,
                Expression<Func<TSender, TValue>> property,
                bool beforeChange = false,
                bool skipInitial = true)
        {
            if (This == null) {
                throw new ArgumentNullException("Sender");
            }
            
            /* x => x.Foo.Bar.Baz;
             * 
             * Subscribe to This, look for Foo
             * Subscribe to Foo, look for Bar
             * Subscribe to Bar, look for Baz
             * Subscribe to Baz, publish to Subject
             * Return Subject
             * 
             * If Bar changes (notification fires on Foo), resubscribe to new Bar
             *  Resubscribe to new Baz, publish to Subject
             * 
             * If Baz changes (notification fires on Bar),
             *  Resubscribe to new Baz, publish to Subject
             */

            return SubscribeToExpressionChain<TSender, TValue>(
                This,
                property.Body,
                beforeChange,
                skipInitial);
        }

        /// <summary>
        /// ObservableForProperty returns an Observable representing the
        /// property change notifications for a specific property on a
        /// ReactiveObject, running the IObservedChange through a Selector
        /// function.
        /// </summary>
        /// <param name="property">An Expression representing the property (i.e.
        /// 'x => x.SomeProperty'</param>
        /// <param name="selector">A Select function that will be run on each
        /// item.</param>
        /// <param name="beforeChange">If True, the Observable will notify
        /// immediately before a property is going to change.</param>
        /// <returns>An Observable representing the property change
        /// notifications for the given property.</returns>
        public static IObservable<TRet> ObservableForProperty<TSender, TValue, TRet>(
                this TSender This,
                Expression<Func<TSender, TValue>> property,
                Func<TValue, TRet> selector,
                bool beforeChange = false)
            where TSender : class
        {
            Contract.Requires(selector != null);
            return This.ObservableForProperty(property, beforeChange).Select(x => selector(x.Value));
        }

        public static IObservable<IObservedChange<TSender, TValue>> SubscribeToExpressionChain<TSender, TValue> ( 
            this TSender source,
            Expression expression, 
            bool beforeChange = false,
            bool skipInitial = true)
        {
            IObservable<IObservedChange<object, object>> notifier = 
                Observable.Return(new ObservedChange<object, object>(null, null, source));

            IEnumerable<Expression> chain = Reflection.Rewrite(expression).GetExpressionChain();
            notifier = chain.Aggregate(notifier, (n, expr) => n
                .Select(y => nestedObservedChanges(expr, y, beforeChange))
                .Switch());
            
            if (skipInitial) {
                notifier = notifier.Skip(1);
            }

            notifier = notifier.Where(x => x.Sender != null);

            var r = notifier.Select(x => {
                // ensure cast to TValue will succeed, throw useful exception otherwise
                var val = x.GetValue();
                if (val != null && ! (val is TValue)) {
                    throw new InvalidCastException(string.Format("Unable to cast from {0} to {1}.", val.GetType(), typeof(TValue)));
                }

                return new ObservedChange<TSender, TValue>(source, expression, (TValue) val);
            });

            return r.DistinctUntilChanged(x=>x.Value);
        }

        static IObservedChange<object, object> observedChangeFor(Expression expression, IObservedChange<object, object> sourceChange)
        {
            var propertyName = expression.GetMemberInfo().Name;
            if (sourceChange.Value == null) {
                return new ObservedChange<object, object>(sourceChange.Value, expression); ;
            } else {
                object value;
                // expression is always a simple expression
                Reflection.TryGetValueForPropertyChain(out value, sourceChange.Value, new[] { expression });
                return new ObservedChange<object, object>(sourceChange.Value, expression, value);
            }
        }

        static IObservable<IObservedChange<object, object>> nestedObservedChanges(Expression expression, IObservedChange<object, object> sourceChange, bool beforeChange)
        {
            // Make sure a change at a root node propogates events down
            var kicker = observedChangeFor(expression, sourceChange);

            // Handle null values in the chain
            if (sourceChange.Value == null) {
                return Observable.Return(kicker);
            }

            // Handle non null values in the chain
            return notifyForProperty(sourceChange.Value, expression, beforeChange)
                .Select(x => new ObservedChange<object, object>(x.Sender, expression, x.GetValue()))
                .StartWith(kicker);
        }

        static readonly MemoizingMRUCache<Tuple<Type, string, bool>, ICreatesObservableForProperty> notifyFactoryCache =
            new MemoizingMRUCache<Tuple<Type, string, bool>, ICreatesObservableForProperty>((t, _) => {
                return Locator.Current.GetServices<ICreatesObservableForProperty>()
                    .Aggregate(Tuple.Create(0, (ICreatesObservableForProperty)null), (acc, x) => {
                        int score = x.GetAffinityForObject(t.Item1, t.Item2, t.Item3);
                        return (score > acc.Item1) ? Tuple.Create(score, x) : acc;
                    }).Item2;
            }, RxApp.BigCacheLimit);

        static IObservable<IObservedChange<object, object>> notifyForProperty(object sender, Expression expression, bool beforeChange)
        {
            var result = default(ICreatesObservableForProperty);
            lock (notifyFactoryCache) {
                result = notifyFactoryCache.Get(Tuple.Create(sender.GetType(), expression.GetMemberInfo().Name, beforeChange));
            }

            if (result == null) {
                throw new Exception(
                    String.Format("Couldn't find a ICreatesObservableForProperty for {0}. This should never happen, your service locator is probably broken.", 
                    sender.GetType()));
            }
            
            return result.GetNotificationForProperty(sender, expression, beforeChange);
        }
    }
#endif