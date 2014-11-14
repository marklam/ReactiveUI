namespace ReactiveUI.FSharp

open ReactiveUI

// TODO - test all public API
// TODO - check extensions vs modules vs static members
// TODO - FSObservedChange internal?

[<AutoOpen>]
module ObservedChangedMixin =

    type IObservedChange<'TSender,'TValue> with
        /// <summary>
        /// Returns the name of a property which has been changed.
        /// </summary>
        /// <returns>The name of the property which has change</returns>
        member this.GetPropertyName() =
            match this with 
            | :? FSObservedChange<'TSender,'TValue> as change -> change |> FSObservedChange.getPropertyName
            | _ -> ReactiveUI.ObservedChangedMixin.GetPropertyName(this)

        /// <summary>
        /// Attempts to return the current value of a property given a 
        /// notification that it has changed. If any property in the
        /// property expression is null, false is returned.
        /// </summary>
        /// <param name="changeValue">The value of the property
        /// expression.</param>
        /// <returns>True if the entire expression was able to be followed,
        /// false otherwise</returns>
        member this.TryGetValue(changeValue : 'TValue byref) = 
            match this with 
            | :? FSObservedChange<'TSender,'TValue> as change -> match (change |> FSObservedChange.tryGetValue) with
                                                                 | Some value -> changeValue <- value;                        true
                                                                 | None       -> changeValue <- Unchecked.defaultof<'TValue>; false
            | _ -> ReactiveUI.ObservedChangedMixin.TryGetValue(this, &changeValue)

        /// <summary>
        /// Returns the current value of a property given a notification that
        /// it has changed.
        /// </summary>
        /// <returns>The current value of the property</returns>
        member this.GetValue() = 
            match this with 
            | :? FSObservedChange<'TSender,'TValue> as change -> change |> FSObservedChange.getValue
            | _ -> ReactiveUI.ObservedChangedMixin.GetValue(this)
