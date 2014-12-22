namespace ReactiveUI.FSharp

open System.Reactive.Linq
open Microsoft.FSharp.Quotations
open System.Runtime.CompilerServices

[<Extension>]
type ReactiveNotifyPropertyChangedMixin() =
    [<Extension>]
    static member inline ObservableForProperty(this : 'TSender, property : Expr<'TSender->'TValue>, ?beforeChange (* = false *), ?skipInitial (* = true *)) =
            NotifyChanged.forProperty<'TSender, 'TValue>(this, property, (defaultArg beforeChange false), (defaultArg skipInitial true))

    [<Extension>]
    static member inline ObservableForProperty(this : 'TSender, property : Expr<'TSender->'TValue>, selector : 'TValue -> 'Ret, ?beforeChange (* = false *)) =
            NotifyChanged.forProperty<'TSender, 'TValue>(this, property, (defaultArg beforeChange false)).Select(fun x -> selector x.Value)
