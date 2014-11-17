open System

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

let typeDefinition = "[<Extension>]\r\ntype WhenAnyMixin() = \r\n" + whenAnyComment + """
        [<Extension>]
        static member inline WhenAny(this : 'TSender, property1 : Expr<'TSender -> 'T1>, selector : IObservedChange<'TSender, 'T1> -> 'TRet) =
            this.ObservableForProperty(property1, false, false).Select(selector) 
"""

let maxFuncLength = 12

for length = 2 to maxFuncLength do
    let paramIndex = seq { 1 .. length }
    let propertyParams = String.Join(", ",   paramIndex |> Seq.map (fun x -> String.Format("property{0} : Expr<'TSender -> 'T{0}>", x)))
    let selectorType   = String.Join(" -> ", paramIndex |> Seq.map (fun x -> String.Format("IObservedChange<'TSender, 'T{0}>", x))) + " -> 'TRet"
    let observables    = String.Join(", ", paramIndex |> Seq.map (fun x -> String.Format("this.ObservableForProperty(property{0}, false, false)", x)))
    let selectorCall   = "Func<" + String.Join(",",  paramIndex |> Seq.map (fun x -> "_")) + ",_>(selector)"
    
    let declaration    = "    [<Extension>]\r\n    static member inline WhenAny(this : 'TSender, " + propertyParams + ", selector : " + selectorType + ") =\r\n"
    let body =           "        System.Reactive.Linq.Observable.CombineLatest(" + observables + ", " + selectorCall + ")\r\n\r\n"
    printf "%s" (whenAnyComment + declaration + body)

// WhenAnyValue (tuple)
// WhenAnyValue (selector)
// WhenAnyDynamic
