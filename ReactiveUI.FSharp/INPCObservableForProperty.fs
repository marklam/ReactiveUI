namespace ReactiveUI.FSharp

open System
open System.Reflection
open System.Reactive.Linq
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Quotations.Patterns
open ReactiveUI
open Splat
open System.ComponentModel

/// <summary>
/// Generates Observables based on observing INotifyPropertyChanged objects
/// </summary>
type INPCObservableForProperty() =
    interface ReactiveUI.FSharp.ICreatesObservableForProperty with
        member this.GetAffinityForObject(``type`` : Type, propertyName : string, beforeChanged : bool) =
            let target = if beforeChanged then  typeof<INotifyPropertyChanging> else typeof<INotifyPropertyChanged>
            if target.GetTypeInfo().IsAssignableFrom(``type``.GetTypeInfo()) then 5 else 0

        member this.GetNotificationForProperty(sender : obj, expression : Expr, beforeChanged : bool) : IObservable<FSObservedChange<obj, obj>> =
            let before = match sender with | :? INotifyPropertyChanging as before -> Some before | _ -> None
            let after  = match sender with | :? INotifyPropertyChanged  as after  -> Some after | _ -> None

            let name = match expression with
                        | PropertyGet(_, info, []) -> info.Name
                        | PropertyGet(_, info, xs) -> info.Name + "[]"
                        | _ -> raise (ArgumentException(sprintf "Unsupported expression type: '%A'" (expression.Type)))

            match beforeChanged, before, after with
            | true, None, _
            | false, _, None -> Observable.Never<FSObservedChange<obj, obj>>()
            | true, Some before, _ ->
                let obs = Observable.FromEventPattern<PropertyChangingEventHandler, PropertyChangingEventArgs>(before.PropertyChanging.AddHandler, before.PropertyChanging.RemoveHandler)
                obs.Where(fun x -> x.EventArgs.PropertyName.Equals(name)).
                    Select(fun _ -> FSObservedChange<obj, obj>(sender, expression))
            | false, _, Some after ->
                let obs = Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(after.PropertyChanged.AddHandler, after.PropertyChanged.RemoveHandler)
                obs.Where(fun x -> x.EventArgs.PropertyName.Equals(name)).
                    Select(fun _ -> FSObservedChange<obj, obj>(sender, expression))
