namespace ReactiveUI.FSharp

open System
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Quotations.Patterns

// TODO - these Expression and Reflection bits seem overly complicated

module Reflection = 
#if false
        public static Func<object, object[], object> GetValueFetcherForProperty(MemberInfo member)
        {
            Contract.Requires(member != null);
            
            FieldInfo field = member as FieldInfo;
            if (field != null) {
                return (obj, args) => field.GetValue(obj);
            }

            PropertyInfo property = member as PropertyInfo;
            if (property != null) {
                return property.GetValue;
            }

            return null;
        }
#endif

    let GetValueFetcherOrThrow (memb : Expr) : (obj -> obj[] option -> obj) =
        match memb with
            | PropertyGet(_, prop, _) -> (fun o a -> prop.GetValue(o, defaultArg a null))
            | FieldGet(_, field)      -> (fun o _ -> field.GetValue(o))
            | _ -> raise (ArgumentException(sprintf "Type must have a property '%A'" memb))
    
    let getValueFetcherOrThrow = GetValueFetcherOrThrow

#if false
        public static Action<object, object, object[]> GetValueSetterForProperty(MemberInfo member)
        {
            Contract.Requires(member != null);

            FieldInfo field = member as FieldInfo;
            if(field != null) {
                return (obj, val, args) => field.SetValue(obj, val);
            }

            PropertyInfo property = member as PropertyInfo;
            if (property != null) {
                return property.SetValue;
            }

            return null;
        }

        public static Action<object, object, object[]> GetValueSetterOrThrow(MemberInfo member)
        {
            var ret = GetValueSetterForProperty(member);

            if (ret == null) {
                throw new ArgumentException(String.Format("Type '{0}' must have a property '{1}'", member.DeclaringType, member.Name));
            }
            return ret;
        }
#endif

    let rec tryGetValueForPropertyChain(current : obj, expressionChain : Expr list) : 'TValue option =
        match expressionChain with
        | [lastExpression]                        -> let args  = lastExpression |> Expression.getArgumentsArray 
                                                     let value = getValueFetcherOrThrow (lastExpression) current args
                                                     value :?> 'TValue |> Some
        | expression :: tail when current <> null -> let args  = expression |> Expression.getArgumentsArray
                                                     tryGetValueForPropertyChain((getValueFetcherOrThrow(expression) current args), tail)
        | _                                       -> None

    let TryGetValueForPropertyChain<'TValue>(changeValue : 'TValue byref, current : obj, expressionChain : Expr list) =
        match tryGetValueForPropertyChain(current, expressionChain) with
        | None       -> false
        | Some value -> changeValue <- value; true

    let ExpressionToPropertyNames(expr : Expr) =
        expr |> Expression.getExpressionChain
             |> List.map(fun e -> match e with
                                  | PropertyGet(_, prop, [])   -> prop.Name
                                  | PropertyGet(_, prop, args) -> prop.Name + "[" +  "TODO" + "]"   // TODO
                                  | _ -> failwith "TODO"                                            // TODO
                         )
             |> List.reduce(fun a b -> a + "." + b)

    let expressionToPropertyNames = ExpressionToPropertyNames

    let Rewrite(expr : Expr) =
        expr |> Expression.rewrite

#if false
        public static bool TryGetAllValuesForPropertyChain(out IObservedChange<object, object>[] changeValues, object current, IEnumerable<Expression> expressionChain)
        {
            int currentIndex = 0;
            changeValues = new IObservedChange<object,object>[expressionChain.Count()];

            foreach (var expression in expressionChain.SkipLast(1)) {
                if (current == null) {
                    changeValues[currentIndex] = null;
                    return false;
                }

                var sender = current;
                current = GetValueFetcherOrThrow(expression.GetMemberInfo())(current, expression.GetArgumentsArray());
                var box = new ObservedChange<object, object>(sender, expression, current);

                changeValues[currentIndex] = box;
                currentIndex++;
            }

            if (current == null) {
                changeValues[currentIndex] = null;
                return false;
            }

            Expression lastExpression = expressionChain.Last();
            changeValues[currentIndex] = new ObservedChange<object, object>(current, lastExpression, GetValueFetcherOrThrow(lastExpression.GetMemberInfo())(current, lastExpression.GetArgumentsArray()));

            return true;
        }

        public static bool TrySetValueToPropertyChain<TValue>(object target, IEnumerable<Expression> expressionChain, TValue value, bool shouldThrow = true)
        {
            foreach (var expression in expressionChain.SkipLast(1)) {
                var getter = shouldThrow ?
                    GetValueFetcherOrThrow(expression.GetMemberInfo()) :
                    GetValueFetcherForProperty(expression.GetMemberInfo());

                target = getter(target, expression.GetArgumentsArray());
            }

            if (target == null) return false;

            Expression lastExpression = expressionChain.Last();
            var setter = shouldThrow ?
                GetValueSetterOrThrow(lastExpression.GetMemberInfo()) :
                GetValueSetterForProperty(lastExpression.GetMemberInfo());

            if (setter == null) return false;
            setter(target, value, lastExpression.GetArgumentsArray());
            return true;
        }

        static readonly MemoizingMRUCache<string, Type> typeCache = new MemoizingMRUCache<string, Type>((type,_) => {
            return Type.GetType(type, false);
        }, 20);

        public static Type ReallyFindType(string type, bool throwOnFailure) 
        {
            lock (typeCache) {
                var ret = typeCache.Get(type);
                if (ret != null || !throwOnFailure) return ret;
                throw new TypeLoadException();
            }
        }
    
        public static Type GetEventArgsTypeForEvent(Type type, string eventName)
        {
            var ti = type;
            var ei = ti.GetRuntimeEvent(eventName);
            if (ei == null) {
                throw new Exception(String.Format("Couldn't find {0}.{1}", type.FullName, eventName));
            }
    
            // Find the EventArgs type parameter of the event via digging around via reflection
            var eventArgsType = ei.EventHandlerType.GetRuntimeMethods().First(x => x.Name == "Invoke").GetParameters()[1].ParameterType;
            return eventArgsType;
        }

        public static void ThrowIfMethodsNotOverloaded(string callingTypeName, object targetObject, params string[] methodsToCheck)
        {
            var missingMethod = methodsToCheck
                .Select(x => {
                    var methods = targetObject.GetType().GetTypeInfo().DeclaredMethods;
                    return Tuple.Create(x, methods.FirstOrDefault(y => y.Name == x));
                })
                .FirstOrDefault(x => x.Item2 == null);

            if (missingMethod != null) {
                throw new Exception(String.Format("Your class must implement {0} and call {1}.{0}", missingMethod.Item1, callingTypeName));
            }
        }
#endif
