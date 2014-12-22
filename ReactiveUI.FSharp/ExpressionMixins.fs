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

    let GetArgumentsArray (expr : Expr) =
        match expr with
            | PropertyGet(_, _, args) -> args |> List.choose constantArg
                                              |> Array.ofList 
                                              |> Some
            | _ -> None

    let getArgumentsArray = GetArgumentsArray

    let GetExpressionChain expr = 
        let rec buildChain (expr : Expr) =
            match expr with
                | PropertyGet(Some item, prop, args) -> (match item with | Var(_) -> expr 
                                                                         | _      -> Expr.PropertyGet(Var("_", prop.DeclaringType) |> Expr.Var, prop, args))
                                                        :: (buildChain item)
                | Var(_) -> []
                | _ -> failwith "TODO"

        expr |> buildChain 
             |> List.rev 

    let getExpressionChain = GetExpressionChain

#if false 
        /// <summary>
        ///
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MemberInfo GetMemberInfo(this Expression expression)
        {
            MemberInfo info = null;
            switch (expression.NodeType) {
            case ExpressionType.Index:
                info = ((IndexExpression)expression).Indexer;
                break;
            case ExpressionType.MemberAccess:
                info = ((MemberExpression)expression).Member;
                break;
            case ExpressionType.Convert:
            case ExpressionType.ConvertChecked:
                return GetMemberInfo(((UnaryExpression)expression).Operand);
            default:
                throw new NotSupportedException(string.Format("Unsupported expression type: '{0}'", expression.NodeType));
            }

            return info;
        }


#endif
    /// <summary>
    ///
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    let GetParent(expression : Expr) =
        match expression with 
        | PropertyGet(Some parent, _, _)  -> parent
        | _ -> raise (NotSupportedException(sprintf "Unsupported expression type: '%A'" expression.Type))

    // TODO - there's an attribute for this
    let getParent = GetParent

    let rec rewrite (expr : Expr) =
        match expr with
            | Var(_)                             -> expr
            | Lambda(_, expr)                    -> rewrite expr
            | PropertyGet(Some item, prop, args) -> if (args |> List.map constantArg |> List.exists(Option.isNone)) then raise (NotSupportedException("Indexers must have constant indexes")) 
                                                    Expr.PropertyGet(rewrite item, prop, args)
            | _                                  -> raise (NotSupportedException(sprintf "Unsupported expression type: '%A'" (expr.Type)))

