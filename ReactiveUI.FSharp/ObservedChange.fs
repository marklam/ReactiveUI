namespace ReactiveUI.FSharp

open Microsoft.FSharp.Quotations
open ReactiveUI

type FSObservedChange<'TSender, 'TValue>(sender:'TSender, expression:Expr, ?value:'TValue) =
    let value = lazy(defaultArg value (match Reflection.tryGetValueForPropertyChain(sender, expression |> Expression.getExpressionChain) with Some v -> v | None -> Unchecked.defaultof<'TValue>))

    do 
        if obj.Equals(sender, Unchecked.defaultof<'TSender>) then failwith "WTF"

    /// <summary>
    /// Attempts to return the current value of a property given a 
    /// notification that it has changed. If any property in the
    /// property expression is null, false is returned.
    /// </summary>
    /// <param name="changeValue">The value of the property
    /// expression.</param>
    /// <returns>True if the entire expression was able to be followed,
    /// false otherwise</returns>
    static member tryGetValue (this : FSObservedChange<'TSender, 'TValue>) =
        if not (obj.Equals(this.Value, Unchecked.defaultof<'TValue>))
            then this.Value |> Some
            else Reflection.tryGetValueForPropertyChain(this.Sender, this.Expression |> Expression.getExpressionChain)

    /// <summary>
    /// Returns the current value of a property given a notification that
    /// it has changed.
    /// </summary>
    /// <returns>The current value of the property</returns>
    static member getValue (this : FSObservedChange<'TSender, 'TValue>) =
        match (this |> FSObservedChange.tryGetValue) with
        | None     -> failwith (sprintf "One of the properties in the expression '%s' was null" (this.GetPropertyName()))
        | Some ret -> ret

    static member getPropertyName(this : FSObservedChange<'TSender, 'TValue>) =
        this.Expression |> Reflection.expressionToPropertyNames

    member this.Expression = expression
    member this.Sender     = sender
    member this.Value      = value.Force()

    interface IObservedChange<'TSender,'TValue> with
        member this.Expression = null
        member this.Sender     = this.Sender
        member this.Value      = this.Value 

