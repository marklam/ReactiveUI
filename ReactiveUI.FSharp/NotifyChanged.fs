﻿namespace ReactiveUI.FSharp

open System
open System.Reactive.Linq
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Quotations.Patterns
open ReactiveUI
open Splat

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
        result.GetNotificationForProperty(sender, expression, beforeChange)

    static let observedChangeFor(expression : Expr, sourceChange : IObservedChange<obj, obj>) : IObservedChange<obj, obj> =
        if (sourceChange.Value = null) then FSObservedChange<obj, obj>(sourceChange.Value, expression) :> IObservedChange<obj, obj>
                                       else match Reflection.tryGetValueForPropertyChain(sourceChange.Value, [expression]) with
                                            | Some value -> FSObservedChange<obj, obj>(sourceChange.Value, expression, value) :> IObservedChange<obj, obj>
                                            | None       -> failwith "TODO"

    static let nestedObservedChanges(expression : Expr, sourceChange : IObservedChange<obj, obj>, beforeChange : bool) : IObservable<IObservedChange<obj, obj>> =
        // Make sure a change at a root node propogates events down
        let kicker = observedChangeFor(expression, sourceChange)

        // Handle null values in the chain
        if (sourceChange.Value = null) then Observable.Return(kicker)
                                       else notifyForProperty(sourceChange.Value, expression, beforeChange).
                                                Select(fun x -> FSObservedChange(x.Sender, expression, x.GetValue()) :> IObservedChange<_,_>).
                                                StartWith(kicker)

    static do
        RxApp.EnsureInitialized()
        let registrations = ReactiveUI.FSharp.Registrations() :> IWantsToRegisterStuff
        registrations.Register(Action<Func<obj>,Type>(fun (f : Func<obj>) t -> Locator.CurrentMutable.RegisterConstant(f.Invoke(), t)))

    static member subscribeToExpressionChain(source : 'TSender, expression : Expr, ?beforeChange (* = false *), ?skipInitial (* = true *)) : IObservable<IObservedChange<'TSender, 'TValue>> =
        let beforeChange = defaultArg beforeChange false
        let skipInitial =  defaultArg skipInitial  true
        let (chain : Expr list) = expression |> Expression.rewrite |> Expression.getExpressionChain

        let mutable notifier = Observable.Return(FSObservedChange<obj, obj>(null, expression, source) :> IObservedChange<_,_>)
        notifier <- chain |> Seq.fold (fun n expr -> n.Select(fun y -> nestedObservedChanges(expr, y, beforeChange)).Switch()) notifier 
        if skipInitial then notifier <- notifier.Skip(1)
        notifier <- notifier.Where(fun x -> x.Sender <> null)

        let r = notifier.Select(fun x -> match x.GetValue() with
                                         | :? 'TValue as value -> FSObservedChange<'TSender, 'TValue>(source, expression, value)
                                         | x when box x = null -> FSObservedChange<'TSender, 'TValue>(source, expression, Unchecked.defaultof<'TValue>)
                                         | x -> raise (InvalidCastException(String.Format("Unable to cast from {0} to {1}.", x.GetType(), typeof<'TValue>)))
                                         :> IObservedChange<'TSender, 'TValue>)

        r.DistinctUntilChanged(fun x -> x.Value)

    static member forProperty(this : 'TSender, property : Expr<'TSender->'TValue>, ?beforeChange (* = false *), ?skipInitial (* = true *)) =
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
