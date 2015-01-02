namespace ReactiveUI.FSharp

open System
open Microsoft.FSharp.Quotations
open ReactiveUI

[<Interface>]
type ICreatesObservableForProperty =
    inherit Splat.IEnableLogger
    abstract member GetAffinityForObject : ``type``:Type * propertyName:string * beforeChanged:bool -> int
    abstract member GetNotificationForProperty : sender:obj * expression:Expr * beforeChanged:bool -> IObservable<FSObservedChange<obj,obj>>


