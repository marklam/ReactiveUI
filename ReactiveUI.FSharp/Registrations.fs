namespace ReactiveUI.FSharp

open System
open ReactiveUI

type Registrations() =
    interface IWantsToRegisterStuff with
        member this.Register(registerFunction : Action<Func<obj>, Type>) =
            let registerFunction f t = registerFunction.Invoke(Func<obj>(f), t)
            registerFunction (fun () -> ReactiveUI.FSharp.INPCObservableForProperty() :> obj) typeof<ReactiveUI.FSharp.ICreatesObservableForProperty>
            registerFunction (fun () -> ReactiveUI.FSharp.IROObservableForProperty()  :> obj) typeof<ReactiveUI.FSharp.ICreatesObservableForProperty>
            registerFunction (fun () -> ReactiveUI.FSharp.POCOObservableForProperty() :> obj) typeof<ReactiveUI.FSharp.ICreatesObservableForProperty>

