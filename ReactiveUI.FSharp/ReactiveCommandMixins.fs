namespace ReactiveUI.FSharp

open ReactiveUI
open System.Runtime.CompilerServices
open System.Reactive.Linq
open Microsoft.FSharp.Quotations
open System
open System.Windows.Input

// TODO - tests

[<Extension>]
type ReactiveCommandMixins =
    /// <summary>
    /// A utility method that will pipe an Observable to an ICommand (i.e.
    /// it will first call its CanExecute with the provided value, then if
    /// the command can be executed, Execute() will be called)
    /// </summary>
    /// <param name="target">The root object which has the Command.</param>
    /// <param name="commandProperty">The expression to reference the Command.</param>
    /// <returns>An object that when disposes, disconnects the Observable
    /// from the command.</returns>
    static member InvokeCommand<'T, 'TTarget>(this : IObservable<'T>, target : 'TTarget, commandProperty : Expr<'TTarget -> ICommand>) : IDisposable =
        this.CombineLatest(target.WhenAnyValue(commandProperty), fun v cmd -> (v, cmd))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(fun (v, cmd) -> if cmd.CanExecute(v) then cmd.Execute(v))
