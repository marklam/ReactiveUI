namespace ReactiveUI.FSharp
open System.Runtime.CompilerServices
open ReactiveUI
open System
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Quotations.Patterns
open System.Reactive.Concurrency
open System.Runtime.InteropServices

// TODO - tests

[<Extension>]
type OAPHCreationHelperMixin =
    [<Extension>]
    static member private observableToProperty<'TObj, 'TRet when 'TObj :> ReactiveObject>(this : 'TObj, observable : IObservable<'TRet>, property : Expr<'TObj -> 'TRet>, initialValue : 'TRet, scheduler : IScheduler) : ObservableAsPropertyHelper<'TRet> =
        let expression = property |> Expression.rewrite

        match (expression |> Expression.getParent) with
        | Var(_) -> let name = expression |> Expression.getName
                    new ObservableAsPropertyHelper<'TRet>(observable, (fun _ -> this.RaisePropertyChanged(name)), (fun _ -> this.RaisePropertyChanging(name)), initialValue, scheduler)
        | _ -> raise (ArgumentException("Property expression must be of the form 'x => x.SomeProperty'"))

    /// <summary>
    /// Converts an Observable to an ObservableAsPropertyHelper and
    /// automatically provides the onChanged method to raise the property
    /// changed notification.         
    /// </summary>
    /// <param name="source">The ReactiveObject that has the property</param>
    /// <param name="property">An Expression representing the property (i.e.
    /// 'x => x.SomeProperty'</param>
    /// <param name="initialValue">The initial value of the property.</param>
    /// <param name="scheduler">The scheduler that the notifications will be
    /// provided on - this should normally be a Dispatcher-based scheduler
    /// (and is by default)</param>
    /// <returns>An initialized ObservableAsPropertyHelper; use this as the
    /// backing field for your property.</returns>
    [<Extension>]
    static member ToProperty<'TObj, 'TRet when 'TObj :> ReactiveObject>(this : IObservable<'TRet>, source : 'TObj , property : Expr<'TObj -> 'TRet>, ?initialValue : 'TRet, ?scheduler : IScheduler) : ObservableAsPropertyHelper<'TRet> =
        let initialValue = defaultArg initialValue Unchecked.defaultof<'TRet>
        let scheduler    = defaultArg scheduler null
        OAPHCreationHelperMixin.observableToProperty(source, this, property, initialValue, scheduler)


    /// <summary>
    /// Converts an Observable to an ObservableAsPropertyHelper and
    /// automatically provides the onChanged method to raise the property
    /// changed notification.         
    /// </summary>
    /// <param name="source">The ReactiveObject that has the property</param>
    /// <param name="property">An Expression representing the property (i.e.
    /// 'x => x.SomeProperty'</param>
    /// <param name="initialValue">The initial value of the property.</param>
    /// <param name="scheduler">The scheduler that the notifications will be
    /// provided on - this should normally be a Dispatcher-based scheduler
    /// (and is by default)</param>
    /// <returns>An initialized ObservableAsPropertyHelper; use this as the
    /// backing field for your property.</returns>
    [<Extension>]
    static member ToProperty<'TObj, 'TRet when 'TObj :> ReactiveObject>(this : IObservable<'TRet>, source : 'TObj , property : Expr<'TObj -> 'TRet>, [<Out>] result : byref<ObservableAsPropertyHelper<'TRet>>, ?initialValue : 'TRet, ?scheduler : IScheduler) : ObservableAsPropertyHelper<'TRet> =
        let initialValue = defaultArg initialValue Unchecked.defaultof<'TRet>
        let scheduler    = defaultArg scheduler null
        let ret = OAPHCreationHelperMixin.observableToProperty(source, this, property, initialValue, scheduler)
        result <- ret
        ret
