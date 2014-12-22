namespace ReactiveUI.FSharp

open ReactiveUI
open System.Runtime.CompilerServices
open System.Reactive.Linq
open Microsoft.FSharp.Quotations
open System
open System.Windows.Input
open System.Collections.Generic

// TODO - tests

[<Extension>]
type ReactiveObjectExtensions =
    /// <summary>
    /// RaiseAndSetIfChanged fully implements a Setter for a read-write
    /// property on a ReactiveObject, using CallerMemberName to raise the notification
    /// and the ref to the backing field to set the property.
    /// </summary>
    /// <typeparam name="TObj">The type of the This.</typeparam>
    /// <typeparam name="TRet">The type of the return value.</typeparam>
    /// <param name="This">The <see cref="ReactiveObject"/> raising the notification.</param>
    /// <param name="backingField">A Reference to the backing field for this
    /// property.</param>
    /// <param name="newValue">The new value.</param>
    /// <param name="property">An expression referencing the property</param>
    /// <returns>The newly set value, normally discarded.</returns>
    static member RaiseAndSetIfChanged<'TObj, 'TRet when 'TObj :> IReactiveObject>(this : 'TObj, backingField : byref<'TRet>, newValue : 'TRet, property : Expr<'TObj -> 'TRet>) : 'TRet =
        let propertyName = property |> Expression.getName

        if not (EqualityComparer<'TRet>.Default.Equals(backingField, newValue)) 
            then this.RaisePropertyChanging(propertyName)
                 backingField <- newValue
                 this.RaisePropertyChanged(propertyName)
        newValue


