﻿open System

let spaces     a b = a + " " + b
let commas     a b = a + ", " + b
let semicolons a b = a + "; " + b
let arrows     a b = a + " -> " + b

let maxFuncLength = 12

let intro = """
namespace ReactiveUI.FSharp

open System
open System.Reactive.Linq
open Microsoft.FSharp.Quotations
open System.Runtime.CompilerServices
open ReactiveUI
"""

let whenAnyComment = """
    /// <summary>
    /// WhenAny allows you to observe whenever one or more properties on an
    /// object have changed, providing an initial value when the Observable
    /// is set up, unlike ObservableForProperty(). Use this method in
    /// constructors to set up bindings between properties that also need an
    /// initial setup.
    /// </summary>
"""

let whenAnyValueComment = """
    /// <summary>
    /// WhenAnyValue allows you to observe whenever the value of a
    /// property on an object has changed, providing an initial value when
    /// the Observable is set up, unlike ObservableForProperty(). Use this
    /// method in constructors to set up bindings between properties that also
    /// need an initial setup.
    /// </summary>
"""

let typeDefinition = "[<Extension>]\r\ntype WhenAnyMixin() = \r\n" + whenAnyComment + """
    [<Extension>]
    static member inline WhenAny(this : 'TSender, property1 : Expr<'TSender -> 'T1>, selector : IObservedChange<'TSender, 'T1> -> 'TRet) =
        this.ObservableForProperty(property1, false, false).Select(selector) 
"""

let typeDefinition2 = "[<Extension>]\r\ntype WhenAnyObservableMixin() =\r\n" + """
    [<Extension>]
    static member inline WhenAnyObservable(this : 'TSender, obs1 : Expr<'TSender -> IObservable<'TRet>>) =
        WhenAnyMixin.WhenAny(this, obs1, fun x -> x.Value).Switch()
"""

let whenAnyDeclaration length =
    let paramIndex = seq { 1 .. length }
    let propertyParams = paramIndex |> Seq.map (fun x -> String.Format("property{0} : Expr<'TSender -> 'T{0}>", x))
                                    |> Seq.reduce commas
    let selectorType   =(paramIndex |> Seq.map (fun x -> String.Format("IObservedChange<'TSender, 'T{0}>", x))
                                    |> Seq.reduce arrows)
                                    + " -> 'TRet"
    let observables    = paramIndex |> Seq.map (fun x -> String.Format("this.ObservableForProperty(property{0}, false, false)", x))
                                    |> Seq.reduce commas
    let selectorCall   = "Func<" + (String.replicate length "_,") + "_>(selector)"
    
    let declaration    = "    [<Extension>]\r\n    static member inline WhenAny(this : 'TSender, " + propertyParams + ", selector : " + selectorType + ") =\r\n"
    let body =           "        System.Reactive.Linq.Observable.CombineLatest(" + observables + ", " + selectorCall + ")\r\n\r\n"
    whenAnyComment + declaration + body

let whenAnyValueTuple length =
    let paramIndex = seq { 1 .. length }
    let propertyParams = paramIndex |> Seq.map (fun x -> String.Format("property{0} : Expr<'TSender -> 'T{0}>", x))
                                    |> Seq.reduce commas
    let properties     = paramIndex |> Seq.map (fun x -> String.Format("property{0}", x))   
                                    |> Seq.reduce commas
    let selectorParams = paramIndex |> Seq.map (fun x -> String.Format("(c{0} : IObservedChange<'TSender, 'T{0}>)", x)) 
                                    |> Seq.reduce spaces
    let selectorTuple  = paramIndex |> Seq.map (fun x -> String.Format("c{0}.Value", x))
                                    |> Seq.reduce commas
    let declaration    = "    [<Extension>]\r\n    static member inline WhenAnyValue(this : 'TSender, " + propertyParams + ") =\r\n"
    let body =           "        WhenAnyMixin.WhenAny(this, " + properties + ", fun " + selectorParams + " -> (" + selectorTuple + "))\r\n\r\n"
    whenAnyValueComment + declaration + body

let whenAnyValueSelector length =
    let paramIndex = seq { 1 .. length }
    let propertyParams = paramIndex |> Seq.map (fun x -> String.Format("property{0} : Expr<'TSender -> 'T{0}>", x))
                                    |> Seq.reduce commas
    let properties     = paramIndex |> Seq.map (fun x -> String.Format("property{0}", x))   
                                    |> Seq.reduce commas
    let selectorType   =(paramIndex |> Seq.map (fun x -> String.Format("'T{0}", x))
                                    |> Seq.reduce arrows)
                                    + " -> 'TRet"
    let selectorInParams  = paramIndex |> Seq.map (fun x -> String.Format("c{0}", x))
                                       |> Seq.reduce spaces
    let selectorOutParams = paramIndex |> Seq.map (fun x -> String.Format("(c{0}.Value)", x))
                                       |> Seq.reduce spaces
    let declaration    = "    [<Extension>]\r\n    static member inline WhenAnyValue(this : 'TSender, " + propertyParams + ", selector : " + selectorType + ") =\r\n"
    let body =           "        WhenAnyMixin.WhenAny(this, " + properties + ", fun " + selectorInParams + " -> selector " + selectorOutParams + ")\r\n\r\n"
    whenAnyValueComment + declaration + body

let whenAnyObsDeclaration length =
    let paramIndex = seq { 1 .. length }
    let observableParams = paramIndex |> Seq.map (fun x -> String.Format("obs{0} : Expr<'TSender -> #IObservable<'TRet>>", x))
                                      |> Seq.reduce commas
    let observables      = paramIndex |> Seq.map (fun x -> String.Format("obs{0}", x))
                                      |> Seq.reduce commas
    let buildArrayParams = paramIndex |> Seq.map (fun x -> String.Format("o{0}", x))
                                      |> Seq.reduce spaces
    let arrayValues      = paramIndex |> Seq.map (fun x -> String.Format("o{0}.Value :> IObservable<'TRet>", x))
                                      |> Seq.reduce semicolons
                                      
    let declaration =    "    [<Extension>]\r\n    static member inline WhenAnyObservable(this : 'TSender, " + observableParams + ") =\r\n"
    let body =           "        WhenAnyMixin.WhenAny(this, " + observables + ", fun " + buildArrayParams + " -> [| " + arrayValues + " |]).Select(fun x -> x.Merge()).Switch()\r\n\r\n"
    declaration + body



let generatedFile = __SOURCE_DIRECTORY__ + "\\WhenAnyMixin.fs"
let output = new System.IO.StreamWriter(generatedFile)
output.Write intro
output.Write typeDefinition
seq { 2 .. maxFuncLength } |> Seq.map whenAnyDeclaration    |> Seq.iter output.Write
seq { 1 .. maxFuncLength } |> Seq.map whenAnyValueTuple     |> Seq.iter output.Write
seq { 1 .. maxFuncLength } |> Seq.map whenAnyValueSelector  |> Seq.iter output.Write
output.Write typeDefinition2
seq { 2 .. maxFuncLength } |> Seq.map whenAnyObsDeclaration |> Seq.iter output.Write

output.Close()