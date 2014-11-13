namespace ReactiveUI.FSharp

open System
open System.Reflection
open System.Reactive.Linq
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Quotations.Patterns
open ReactiveUI

// TODO - use observable stuff from FSharp.Control

/// <summary>
/// Generates Observables based on observing Reactive objects
/// </summary>
type IROObservableForProperty() =
    interface ReactiveUI.FSharp.ICreatesObservableForProperty with
        member this.GetAffinityForObject(``type`` : Type, propertyName : string, beforeChanged : bool) =
            // NB: Since every IReactiveObject is also an INPC, we need to bind more 
            // tightly than INPCObservableForProperty, so we return 10 here 
            // instead of one
            if typeof<IReactiveObject>.GetTypeInfo().IsAssignableFrom(``type``.GetTypeInfo()) then 10 else 0

        member this.GetNotificationForProperty(sender : obj, expression : Expr, beforeChanged : bool) : IObservable<IObservedChange<obj, obj>> =
            match sender with
            | :? IReactiveObject as iro -> 
                let obs = if beforeChanged then IReactiveObjectExtensions.getChangingObservable(iro) else IReactiveObjectExtensions.getChangedObservable(iro)
                let name = match expression with
                           | PropertyGet(_, info, []) -> info.Name
                           | PropertyGet(_, info, xs) -> info.Name + "[]"
                           | _ -> raise (ArgumentException(sprintf "Unsupported expression type: '%A'" (expression.Type)))
                obs.Where(fun x -> x.PropertyName.Equals(name)).
                    Select(fun x -> FSObservedChange<obj, obj>(sender, expression) :> IObservedChange<obj, obj>)
            | _ -> raise (ArgumentException("Sender doesn't implement IReactiveObject"))
