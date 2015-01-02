namespace ReactiveUI.FSharp

open System
open System.Reflection
open System.Reactive.Linq
open System.Reactive.Disposables
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Quotations.Patterns
open System.Windows
open System.ComponentModel
open ReactiveUI

type DependencyObjectObservableForProperty() =
    let getDependencyProperty(``type`` : Type, propertyName : string) =
        ``type``.GetTypeInfo().GetFields(BindingFlags.FlattenHierarchy ||| BindingFlags.Static ||| BindingFlags.Public)
        |> Seq.tryFind(fun x -> x.Name = propertyName + "Property" && x.IsStatic)
        |> Option.bind(fun fi -> match fi.GetValue(null) with
                                 | :? DependencyProperty as dep -> Some dep
                                 | _ -> None)

    interface ReactiveUI.FSharp.ICreatesObservableForProperty with
        member this.GetAffinityForObject(``type`` : Type, propertyName : string, beforeChanged : bool) =
            if not (typeof<DependencyObject>.GetTypeInfo().IsAssignableFrom(``type``.GetTypeInfo()))
                then 0
                else match getDependencyProperty(``type``, propertyName) with
                     | None   -> 0
                     | Some _ -> 4

        member this.GetNotificationForProperty(sender : obj, expression : Expr, beforeChanged : bool) : IObservable<FSObservedChange<obj, obj>> =
            let ``type`` = sender.GetType()
            let propertyName = expression |> Expression.getName
            match getDependencyProperty(``type``, propertyName) with
            | None -> failwith "TODO"
            | Some dep -> let dpd = DependencyPropertyDescriptor.FromProperty(dep, ``type``)
                          Observable.Create(fun (subj : IObserver<_>) -> let handler = EventHandler(fun _ _ -> subj.OnNext(FSObservedChange(sender, expression)))
                                                                         dpd.AddValueChanged(sender, handler)
                                                                         Disposable.Create(fun () -> dpd.RemoveValueChanged(sender, handler)))
