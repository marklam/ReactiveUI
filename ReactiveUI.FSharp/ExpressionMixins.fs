namespace ReactiveUI.FSharp

open System
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Quotations.Patterns

module Expression =
    let private constantArg = function | Value(v, _) -> Some v | _ -> None 

    let getName (expr : Expr) = 
        match expr with
            | PropertyGet(_, prop, _) -> prop.Name
            | _ -> raise (NotSupportedException(sprintf "Unsupported expression type: '%A'" (expr.Type)))

    [<CompiledName("GetArgumentsArray")>]
    let getArgumentsArray (expr : Expr) =
        match expr with
            | PropertyGet(_, _, args) -> args |> List.choose constantArg
                                              |> Array.ofList 
                                              |> Some
            | _ -> None

    [<CompiledName("GetExpressionChain")>]
    let getExpressionChain expr = 
        let rec buildChain (expr : Expr) =
            match expr with
                | PropertyGet(Some item, prop, args) -> (match item with | Var(_) -> expr 
                                                                         | _      -> Expr.PropertyGet(Var("_", prop.DeclaringType) |> Expr.Var, prop, args))
                                                        :: (buildChain item)
                | Var(_) -> []
                | _ -> failwith "TODO" // TODO

        expr |> buildChain 
             |> List.rev 

    /// <summary>
    ///
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    [<CompiledName("GetParent")>]
    let getParent(expression : Expr) =
        match expression with 
        | PropertyGet(Some parent, _, _)  -> parent
        | _ -> raise (NotSupportedException(sprintf "Unsupported expression type: '%A'" expression.Type))

    let rec rewrite (expr : Expr) =
        match expr with
            | Var(_)                             -> expr
            | Lambda(_, expr)                    -> rewrite expr
            | PropertyGet(Some item, prop, args) -> if (args |> List.map constantArg |> List.exists(Option.isNone)) then raise (NotSupportedException("Indexers must have constant indexes")) 
                                                    Expr.PropertyGet(rewrite item, prop, args)
            | _                                  -> raise (NotSupportedException(sprintf "Unsupported expression type: '%A'" (expr.Type)))

