namespace ReactiveUI.FSharp

open System
open System.Reactive.Linq
open Microsoft.FSharp.Quotations
open ReactiveUI
open Splat

/// <summary>
/// This class is the final fallback for WhenAny, and will simply immediately
/// return the value of the type at the time it was created. It will also 
/// warn the user that this is probably not what they want to do
/// </summary>
type POCOObservableForProperty() =
    let hasWarned = new System.Collections.Generic.HashSet<Type>()

    interface ReactiveUI.FSharp.ICreatesObservableForProperty with
        member this.GetAffinityForObject(``type`` : Type, propertyName : string, beforeChanged : bool) = 1

        member this.GetNotificationForProperty(sender : obj, expression : Expr, beforeChanged : bool) : IObservable<IObservedChange<obj, obj>> =
            let ``type`` = sender.GetType()
            if not (hasWarned.Contains(``type``)) then this.Log().Warn(sprintf "%s is a POCO type and won't send change notifications, WhenAny will only return a single value!" (``type``.FullName))
                                                       hasWarned.Add(``type``) |> ignore

            Observable.Return(FSObservedChange<obj, obj>(sender, expression) :> IObservedChange<obj, obj>, RxApp.MainThreadScheduler)
                .Concat(Observable.Never<IObservedChange<obj, obj>>())
