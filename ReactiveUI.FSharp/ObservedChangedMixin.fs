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
            | :? FSObservedChange<'TSender,'TValue> as change -> let (ret, value) = (change |> FSObservedChange.tryGetValue)
                                                                 changeValue <- value
                                                                 ret
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
  

#if false
    public static class ObservedChangedMixin
    {

        /// <summary>
        /// Given a fully filled-out IObservedChange object, SetValueToProperty
        /// will apply it to the specified object (i.e. it will ensure that
        /// target.property == This.GetValue() and "replay" the observed change
        /// onto another object)
        /// </summary>
        /// <param name="target">The target object to apply the change to.</param>
        /// <param name="property">The target property to apply the change to.</param>
        internal static void SetValueToProperty<TSender, TValue, TTarget>(
            this IObservedChange<TSender, TValue> This, 
            TTarget target,
            Expression<Func<TTarget, TValue>> property)
        {
            Reflection.TrySetValueToPropertyChain(target, Reflection.Rewrite(property.Body).GetExpressionChain(), This.GetValue());
        }

        /// <summary>
        /// Given a stream of notification changes, this method will convert 
        /// the property changes to the current value of the property.
        /// </summary>
        /// <returns>An Observable representing the stream of current values of
        /// the given change notification stream.</returns>
        public static IObservable<TValue> Value<TSender, TValue>(
		    this IObservable<IObservedChange<TSender, TValue>> This)
        {
            return This.Select(GetValue);
        }
    }


#endif
