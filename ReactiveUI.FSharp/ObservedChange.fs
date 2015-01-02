namespace ReactiveUI.FSharp

open Microsoft.FSharp.Quotations
open ReactiveUI
open System

type FSObservedChange<'TSender, 'TValue>(sender:'TSender, expression:Expr, ?value : 'TValue option) =
    let mutable value = defaultArg value None
    let getCachedValue() = if value.IsNone then value <- Reflection.tryGetValueForPropertyChain(sender, expression |> Expression.getExpressionChain)
                           value

    /// <summary>
    /// Attempts to return the current value of a property given a 
    /// notification that it has changed. If any property in the
    /// property expression is null, false is returned.
    /// </summary>
    /// <param name="changeValue">The value of the property
    /// expression.</param>
    /// <returns>True if the entire expression was able to be followed,
    /// false otherwise</returns>
    static member tryGetValue (this : FSObservedChange<'TSender, 'TValue>) = Reflection.tryGetValueForPropertyChain(this.Sender, this.Expression |> Expression.getExpressionChain)

    /// <summary>
    /// Returns the current value of a property given a notification that
    /// it has changed.
    /// </summary>
    /// <returns>The current value of the property</returns>
    static member getValue (this : FSObservedChange<'TSender, 'TValue>) =
        match (this |> FSObservedChange.tryGetValue) with
        | None     -> failwith (sprintf "One of the properties in the expression '%s' was null" (FSObservedChange.getPropertyName this))
        | Some ret -> ret

    static member getPropertyName(this : FSObservedChange<'TSender, 'TValue>) =
        this.Expression |> Reflection.expressionToPropertyNames

    member this.Expression = expression
    member this.Sender     = sender
    member this.Value      = match getCachedValue() with
                             | Some v -> v
                             | None   -> Unchecked.defaultof<'TValue>

//    interface IObservedChange<'TSender,'TValue> with
//        member this.Expression = raise (NotSupportedException("Linq Expressions not supported by FSObservedChange (for FSharp Expr types)"))
//        member this.Sender     = this.Sender
//        member this.Value      = this.Value 

