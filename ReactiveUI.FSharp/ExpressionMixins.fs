namespace ReactiveUI.FSharp

open System
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Quotations.Patterns

// TODO - these Expression and Reflection bits seem overly complicated

module Expression =
    let getName (expr : Expr) = 
        match expr with
            | PropertyGet(_, prop, _) -> prop.Name
            | FieldGet(_, field)      -> field.Name
            | _ -> raise (NotSupportedException(sprintf "Unsupported expression type: '%A'" (expr.Type)))

    let constantArg = function | Value(v, _) -> Some v | _ -> None 

    let getArgumentsArray (expr : Expr) =
        match expr with
            | PropertyGet(_, _, args) -> args |> List.choose constantArg
                                              |> Array.ofList 
                                              |> Some
            | _ -> None

    let getExpressionChain expr = 
        let rec buildChain (expr : Expr) =
            match expr with
                | PropertyGet(Some item, _, _) -> expr :: (buildChain item)
                | FieldGet(Some item, _)       -> expr :: (buildChain item)
                | _                            -> [expr]
        
        expr |> buildChain 
             |> List.rev 
             |> List.tail

    let rewrite (expr : Expr) =
        let rec check expr =
            match expr with
                | PropertyGet(Some item, _, args) -> args |> List.map constantArg |> List.iter(function | Some _ -> () | None -> raise (NotSupportedException("Indexers must have constant indexes"))); check item
                | PropertyGet(None, _, args)      -> args |> List.map constantArg |> List.iter(function | Some _ -> () | None -> raise (NotSupportedException("Indexers must have constant indexes"))); ()
                | FieldGet(Some item, field)      -> check item
                | FieldGet(None, _)               -> ()
                | Var(_)                          -> ()
                | _ -> raise (NotSupportedException(sprintf "Unsupported expression type: '%A'" (expr.Type)))
        check expr
        expr


