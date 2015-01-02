namespace ReactiveUI.FSharp

open System
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Quotations.Patterns

module Reflection = 
    [<CompiledName("GetValueFetcherOrThrow")>]
    let getValueFetcherOrThrow (memb : Expr) : (obj -> obj[] option -> obj) =
        match memb with
            | PropertyGet(_, prop, _) -> (fun o a -> prop.GetValue(o, defaultArg a null))
            | FieldGet(_, field)      -> (fun o _ -> field.GetValue(o))
            | _ -> raise (ArgumentException(sprintf "Type must have a property '%A'" memb))
    
    let rec tryGetValueForPropertyChain(current : obj, expressionChain : Expr list) : 'TValue option =
        if (null = current) 
            then None
            else match expressionChain with
                 | [lastExpression]   -> let args  = lastExpression |> Expression.getArgumentsArray 
                                         let value = getValueFetcherOrThrow (lastExpression) current args
                                         value :?> 'TValue |> Some
                 | expression :: tail -> let args  = expression |> Expression.getArgumentsArray
                                         tryGetValueForPropertyChain((getValueFetcherOrThrow(expression) current args), tail)
                 | _                  -> None

    [<CompiledName("ExpressionToPropertyNames")>]
    let expressionToPropertyNames(expr : Expr) =
        expr |> Expression.getExpressionChain
             |> List.map(fun e -> match e with
                                  | PropertyGet(_, prop, [])   -> prop.Name
                                  | PropertyGet(_, prop, args) -> prop.Name + "[" +  "TODO" + "]"   // TODO
                                  | _ -> failwith "TODO"                                            // TODO
                         )
             |> List.reduce(fun a b -> a + "." + b)
