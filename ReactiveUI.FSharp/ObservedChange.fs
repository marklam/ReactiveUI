namespace ReactiveUI.FSharp

open Microsoft.FSharp.Quotations
open ReactiveUI

type ObservedChange<'TSender, 'TValue when 'TValue:null>(sender:'TSender, expression:Expr, ?value:'TValue) =
    member this.Expression     = expression

    interface IObservedChange<'TSender,'TValue> with
        member this.Expression = null
        member this.Sender     = sender
        member this.Value      = defaultArg value null



