
namespace ReactiveUI.FSharp

open System
open System.Reactive.Linq
open Microsoft.FSharp.Quotations
open System.Runtime.CompilerServices
open ReactiveUI
[<Extension>]
type WhenAnyMixin() = 

    /// <summary>
    /// WhenAny allows you to observe whenever one or more properties on an
    /// object have changed, providing an initial value when the Observable
    /// is set up, unlike ObservableForProperty(). Use this method in
    /// constructors to set up bindings between properties that also need an
    /// initial setup.
    /// </summary>

    [<Extension>]
    static member inline WhenAny(this : 'TSender, property1 : Expr<'TSender -> 'T1>, selector : IObservedChange<'TSender, 'T1> -> 'TRet) =
        this.ObservableForProperty(property1, false, false).Select(selector) 

    /// <summary>
    /// WhenAny allows you to observe whenever one or more properties on an
    /// object have changed, providing an initial value when the Observable
    /// is set up, unlike ObservableForProperty(). Use this method in
    /// constructors to set up bindings between properties that also need an
    /// initial setup.
    /// </summary>
    [<Extension>]
    static member inline WhenAny(this : 'TSender, property1 : Expr<'TSender -> 'T1>, property2 : Expr<'TSender -> 'T2>, selector : IObservedChange<'TSender, 'T1> -> IObservedChange<'TSender, 'T2> -> 'TRet) =
        System.Reactive.Linq.Observable.CombineLatest(this.ObservableForProperty(property1, false, false), this.ObservableForProperty(property2, false, false), Func<_,_,_>(selector))


    /// <summary>
    /// WhenAny allows you to observe whenever one or more properties on an
    /// object have changed, providing an initial value when the Observable
    /// is set up, unlike ObservableForProperty(). Use this method in
    /// constructors to set up bindings between properties that also need an
    /// initial setup.
    /// </summary>
    [<Extension>]
    static member inline WhenAny(this : 'TSender, property1 : Expr<'TSender -> 'T1>, property2 : Expr<'TSender -> 'T2>, property3 : Expr<'TSender -> 'T3>, selector : IObservedChange<'TSender, 'T1> -> IObservedChange<'TSender, 'T2> -> IObservedChange<'TSender, 'T3> -> 'TRet) =
        System.Reactive.Linq.Observable.CombineLatest(this.ObservableForProperty(property1, false, false), this.ObservableForProperty(property2, false, false), this.ObservableForProperty(property3, false, false), Func<_,_,_,_>(selector))


    /// <summary>
    /// WhenAny allows you to observe whenever one or more properties on an
    /// object have changed, providing an initial value when the Observable
    /// is set up, unlike ObservableForProperty(). Use this method in
    /// constructors to set up bindings between properties that also need an
    /// initial setup.
    /// </summary>
    [<Extension>]
    static member inline WhenAny(this : 'TSender, property1 : Expr<'TSender -> 'T1>, property2 : Expr<'TSender -> 'T2>, property3 : Expr<'TSender -> 'T3>, property4 : Expr<'TSender -> 'T4>, selector : IObservedChange<'TSender, 'T1> -> IObservedChange<'TSender, 'T2> -> IObservedChange<'TSender, 'T3> -> IObservedChange<'TSender, 'T4> -> 'TRet) =
        System.Reactive.Linq.Observable.CombineLatest(this.ObservableForProperty(property1, false, false), this.ObservableForProperty(property2, false, false), this.ObservableForProperty(property3, false, false), this.ObservableForProperty(property4, false, false), Func<_,_,_,_,_>(selector))


    /// <summary>
    /// WhenAny allows you to observe whenever one or more properties on an
    /// object have changed, providing an initial value when the Observable
    /// is set up, unlike ObservableForProperty(). Use this method in
    /// constructors to set up bindings between properties that also need an
    /// initial setup.
    /// </summary>
    [<Extension>]
    static member inline WhenAny(this : 'TSender, property1 : Expr<'TSender -> 'T1>, property2 : Expr<'TSender -> 'T2>, property3 : Expr<'TSender -> 'T3>, property4 : Expr<'TSender -> 'T4>, property5 : Expr<'TSender -> 'T5>, selector : IObservedChange<'TSender, 'T1> -> IObservedChange<'TSender, 'T2> -> IObservedChange<'TSender, 'T3> -> IObservedChange<'TSender, 'T4> -> IObservedChange<'TSender, 'T5> -> 'TRet) =
        System.Reactive.Linq.Observable.CombineLatest(this.ObservableForProperty(property1, false, false), this.ObservableForProperty(property2, false, false), this.ObservableForProperty(property3, false, false), this.ObservableForProperty(property4, false, false), this.ObservableForProperty(property5, false, false), Func<_,_,_,_,_,_>(selector))


    /// <summary>
    /// WhenAny allows you to observe whenever one or more properties on an
    /// object have changed, providing an initial value when the Observable
    /// is set up, unlike ObservableForProperty(). Use this method in
    /// constructors to set up bindings between properties that also need an
    /// initial setup.
    /// </summary>
    [<Extension>]
    static member inline WhenAny(this : 'TSender, property1 : Expr<'TSender -> 'T1>, property2 : Expr<'TSender -> 'T2>, property3 : Expr<'TSender -> 'T3>, property4 : Expr<'TSender -> 'T4>, property5 : Expr<'TSender -> 'T5>, property6 : Expr<'TSender -> 'T6>, selector : IObservedChange<'TSender, 'T1> -> IObservedChange<'TSender, 'T2> -> IObservedChange<'TSender, 'T3> -> IObservedChange<'TSender, 'T4> -> IObservedChange<'TSender, 'T5> -> IObservedChange<'TSender, 'T6> -> 'TRet) =
        System.Reactive.Linq.Observable.CombineLatest(this.ObservableForProperty(property1, false, false), this.ObservableForProperty(property2, false, false), this.ObservableForProperty(property3, false, false), this.ObservableForProperty(property4, false, false), this.ObservableForProperty(property5, false, false), this.ObservableForProperty(property6, false, false), Func<_,_,_,_,_,_,_>(selector))


    /// <summary>
    /// WhenAny allows you to observe whenever one or more properties on an
    /// object have changed, providing an initial value when the Observable
    /// is set up, unlike ObservableForProperty(). Use this method in
    /// constructors to set up bindings between properties that also need an
    /// initial setup.
    /// </summary>
    [<Extension>]
    static member inline WhenAny(this : 'TSender, property1 : Expr<'TSender -> 'T1>, property2 : Expr<'TSender -> 'T2>, property3 : Expr<'TSender -> 'T3>, property4 : Expr<'TSender -> 'T4>, property5 : Expr<'TSender -> 'T5>, property6 : Expr<'TSender -> 'T6>, property7 : Expr<'TSender -> 'T7>, selector : IObservedChange<'TSender, 'T1> -> IObservedChange<'TSender, 'T2> -> IObservedChange<'TSender, 'T3> -> IObservedChange<'TSender, 'T4> -> IObservedChange<'TSender, 'T5> -> IObservedChange<'TSender, 'T6> -> IObservedChange<'TSender, 'T7> -> 'TRet) =
        System.Reactive.Linq.Observable.CombineLatest(this.ObservableForProperty(property1, false, false), this.ObservableForProperty(property2, false, false), this.ObservableForProperty(property3, false, false), this.ObservableForProperty(property4, false, false), this.ObservableForProperty(property5, false, false), this.ObservableForProperty(property6, false, false), this.ObservableForProperty(property7, false, false), Func<_,_,_,_,_,_,_,_>(selector))


    /// <summary>
    /// WhenAny allows you to observe whenever one or more properties on an
    /// object have changed, providing an initial value when the Observable
    /// is set up, unlike ObservableForProperty(). Use this method in
    /// constructors to set up bindings between properties that also need an
    /// initial setup.
    /// </summary>
    [<Extension>]
    static member inline WhenAny(this : 'TSender, property1 : Expr<'TSender -> 'T1>, property2 : Expr<'TSender -> 'T2>, property3 : Expr<'TSender -> 'T3>, property4 : Expr<'TSender -> 'T4>, property5 : Expr<'TSender -> 'T5>, property6 : Expr<'TSender -> 'T6>, property7 : Expr<'TSender -> 'T7>, property8 : Expr<'TSender -> 'T8>, selector : IObservedChange<'TSender, 'T1> -> IObservedChange<'TSender, 'T2> -> IObservedChange<'TSender, 'T3> -> IObservedChange<'TSender, 'T4> -> IObservedChange<'TSender, 'T5> -> IObservedChange<'TSender, 'T6> -> IObservedChange<'TSender, 'T7> -> IObservedChange<'TSender, 'T8> -> 'TRet) =
        System.Reactive.Linq.Observable.CombineLatest(this.ObservableForProperty(property1, false, false), this.ObservableForProperty(property2, false, false), this.ObservableForProperty(property3, false, false), this.ObservableForProperty(property4, false, false), this.ObservableForProperty(property5, false, false), this.ObservableForProperty(property6, false, false), this.ObservableForProperty(property7, false, false), this.ObservableForProperty(property8, false, false), Func<_,_,_,_,_,_,_,_,_>(selector))


    /// <summary>
    /// WhenAny allows you to observe whenever one or more properties on an
    /// object have changed, providing an initial value when the Observable
    /// is set up, unlike ObservableForProperty(). Use this method in
    /// constructors to set up bindings between properties that also need an
    /// initial setup.
    /// </summary>
    [<Extension>]
    static member inline WhenAny(this : 'TSender, property1 : Expr<'TSender -> 'T1>, property2 : Expr<'TSender -> 'T2>, property3 : Expr<'TSender -> 'T3>, property4 : Expr<'TSender -> 'T4>, property5 : Expr<'TSender -> 'T5>, property6 : Expr<'TSender -> 'T6>, property7 : Expr<'TSender -> 'T7>, property8 : Expr<'TSender -> 'T8>, property9 : Expr<'TSender -> 'T9>, selector : IObservedChange<'TSender, 'T1> -> IObservedChange<'TSender, 'T2> -> IObservedChange<'TSender, 'T3> -> IObservedChange<'TSender, 'T4> -> IObservedChange<'TSender, 'T5> -> IObservedChange<'TSender, 'T6> -> IObservedChange<'TSender, 'T7> -> IObservedChange<'TSender, 'T8> -> IObservedChange<'TSender, 'T9> -> 'TRet) =
        System.Reactive.Linq.Observable.CombineLatest(this.ObservableForProperty(property1, false, false), this.ObservableForProperty(property2, false, false), this.ObservableForProperty(property3, false, false), this.ObservableForProperty(property4, false, false), this.ObservableForProperty(property5, false, false), this.ObservableForProperty(property6, false, false), this.ObservableForProperty(property7, false, false), this.ObservableForProperty(property8, false, false), this.ObservableForProperty(property9, false, false), Func<_,_,_,_,_,_,_,_,_,_>(selector))


    /// <summary>
    /// WhenAny allows you to observe whenever one or more properties on an
    /// object have changed, providing an initial value when the Observable
    /// is set up, unlike ObservableForProperty(). Use this method in
    /// constructors to set up bindings between properties that also need an
    /// initial setup.
    /// </summary>
    [<Extension>]
    static member inline WhenAny(this : 'TSender, property1 : Expr<'TSender -> 'T1>, property2 : Expr<'TSender -> 'T2>, property3 : Expr<'TSender -> 'T3>, property4 : Expr<'TSender -> 'T4>, property5 : Expr<'TSender -> 'T5>, property6 : Expr<'TSender -> 'T6>, property7 : Expr<'TSender -> 'T7>, property8 : Expr<'TSender -> 'T8>, property9 : Expr<'TSender -> 'T9>, property10 : Expr<'TSender -> 'T10>, selector : IObservedChange<'TSender, 'T1> -> IObservedChange<'TSender, 'T2> -> IObservedChange<'TSender, 'T3> -> IObservedChange<'TSender, 'T4> -> IObservedChange<'TSender, 'T5> -> IObservedChange<'TSender, 'T6> -> IObservedChange<'TSender, 'T7> -> IObservedChange<'TSender, 'T8> -> IObservedChange<'TSender, 'T9> -> IObservedChange<'TSender, 'T10> -> 'TRet) =
        System.Reactive.Linq.Observable.CombineLatest(this.ObservableForProperty(property1, false, false), this.ObservableForProperty(property2, false, false), this.ObservableForProperty(property3, false, false), this.ObservableForProperty(property4, false, false), this.ObservableForProperty(property5, false, false), this.ObservableForProperty(property6, false, false), this.ObservableForProperty(property7, false, false), this.ObservableForProperty(property8, false, false), this.ObservableForProperty(property9, false, false), this.ObservableForProperty(property10, false, false), Func<_,_,_,_,_,_,_,_,_,_,_>(selector))


    /// <summary>
    /// WhenAny allows you to observe whenever one or more properties on an
    /// object have changed, providing an initial value when the Observable
    /// is set up, unlike ObservableForProperty(). Use this method in
    /// constructors to set up bindings between properties that also need an
    /// initial setup.
    /// </summary>
    [<Extension>]
    static member inline WhenAny(this : 'TSender, property1 : Expr<'TSender -> 'T1>, property2 : Expr<'TSender -> 'T2>, property3 : Expr<'TSender -> 'T3>, property4 : Expr<'TSender -> 'T4>, property5 : Expr<'TSender -> 'T5>, property6 : Expr<'TSender -> 'T6>, property7 : Expr<'TSender -> 'T7>, property8 : Expr<'TSender -> 'T8>, property9 : Expr<'TSender -> 'T9>, property10 : Expr<'TSender -> 'T10>, property11 : Expr<'TSender -> 'T11>, selector : IObservedChange<'TSender, 'T1> -> IObservedChange<'TSender, 'T2> -> IObservedChange<'TSender, 'T3> -> IObservedChange<'TSender, 'T4> -> IObservedChange<'TSender, 'T5> -> IObservedChange<'TSender, 'T6> -> IObservedChange<'TSender, 'T7> -> IObservedChange<'TSender, 'T8> -> IObservedChange<'TSender, 'T9> -> IObservedChange<'TSender, 'T10> -> IObservedChange<'TSender, 'T11> -> 'TRet) =
        System.Reactive.Linq.Observable.CombineLatest(this.ObservableForProperty(property1, false, false), this.ObservableForProperty(property2, false, false), this.ObservableForProperty(property3, false, false), this.ObservableForProperty(property4, false, false), this.ObservableForProperty(property5, false, false), this.ObservableForProperty(property6, false, false), this.ObservableForProperty(property7, false, false), this.ObservableForProperty(property8, false, false), this.ObservableForProperty(property9, false, false), this.ObservableForProperty(property10, false, false), this.ObservableForProperty(property11, false, false), Func<_,_,_,_,_,_,_,_,_,_,_,_>(selector))


    /// <summary>
    /// WhenAny allows you to observe whenever one or more properties on an
    /// object have changed, providing an initial value when the Observable
    /// is set up, unlike ObservableForProperty(). Use this method in
    /// constructors to set up bindings between properties that also need an
    /// initial setup.
    /// </summary>
    [<Extension>]
    static member inline WhenAny(this : 'TSender, property1 : Expr<'TSender -> 'T1>, property2 : Expr<'TSender -> 'T2>, property3 : Expr<'TSender -> 'T3>, property4 : Expr<'TSender -> 'T4>, property5 : Expr<'TSender -> 'T5>, property6 : Expr<'TSender -> 'T6>, property7 : Expr<'TSender -> 'T7>, property8 : Expr<'TSender -> 'T8>, property9 : Expr<'TSender -> 'T9>, property10 : Expr<'TSender -> 'T10>, property11 : Expr<'TSender -> 'T11>, property12 : Expr<'TSender -> 'T12>, selector : IObservedChange<'TSender, 'T1> -> IObservedChange<'TSender, 'T2> -> IObservedChange<'TSender, 'T3> -> IObservedChange<'TSender, 'T4> -> IObservedChange<'TSender, 'T5> -> IObservedChange<'TSender, 'T6> -> IObservedChange<'TSender, 'T7> -> IObservedChange<'TSender, 'T8> -> IObservedChange<'TSender, 'T9> -> IObservedChange<'TSender, 'T10> -> IObservedChange<'TSender, 'T11> -> IObservedChange<'TSender, 'T12> -> 'TRet) =
        System.Reactive.Linq.Observable.CombineLatest(this.ObservableForProperty(property1, false, false), this.ObservableForProperty(property2, false, false), this.ObservableForProperty(property3, false, false), this.ObservableForProperty(property4, false, false), this.ObservableForProperty(property5, false, false), this.ObservableForProperty(property6, false, false), this.ObservableForProperty(property7, false, false), this.ObservableForProperty(property8, false, false), this.ObservableForProperty(property9, false, false), this.ObservableForProperty(property10, false, false), this.ObservableForProperty(property11, false, false), this.ObservableForProperty(property12, false, false), Func<_,_,_,_,_,_,_,_,_,_,_,_,_>(selector))


    /// <summary>
    /// WhenAnyValue allows you to observe whenever the value of a
    /// property on an object has changed, providing an initial value when
    /// the Observable is set up, unlike ObservableForProperty(). Use this
    /// method in constructors to set up bindings between properties that also
    /// need an initial setup.
    /// </summary>
    [<Extension>]
    static member inline WhenAnyValue(this : 'TSender, property1 : Expr<'TSender -> 'T1>) =
        WhenAnyMixin.WhenAny(this, property1, fun (c1 : IObservedChange<'TSender, 'T1>) -> (c1.Value))


    /// <summary>
    /// WhenAnyValue allows you to observe whenever the value of a
    /// property on an object has changed, providing an initial value when
    /// the Observable is set up, unlike ObservableForProperty(). Use this
    /// method in constructors to set up bindings between properties that also
    /// need an initial setup.
    /// </summary>
    [<Extension>]
    static member inline WhenAnyValue(this : 'TSender, property1 : Expr<'TSender -> 'T1>, property2 : Expr<'TSender -> 'T2>) =
        WhenAnyMixin.WhenAny(this, property1, property2, fun (c1 : IObservedChange<'TSender, 'T1>) (c2 : IObservedChange<'TSender, 'T2>) -> (c1.Value, c2.Value))


    /// <summary>
    /// WhenAnyValue allows you to observe whenever the value of a
    /// property on an object has changed, providing an initial value when
    /// the Observable is set up, unlike ObservableForProperty(). Use this
    /// method in constructors to set up bindings between properties that also
    /// need an initial setup.
    /// </summary>
    [<Extension>]
    static member inline WhenAnyValue(this : 'TSender, property1 : Expr<'TSender -> 'T1>, property2 : Expr<'TSender -> 'T2>, property3 : Expr<'TSender -> 'T3>) =
        WhenAnyMixin.WhenAny(this, property1, property2, property3, fun (c1 : IObservedChange<'TSender, 'T1>) (c2 : IObservedChange<'TSender, 'T2>) (c3 : IObservedChange<'TSender, 'T3>) -> (c1.Value, c2.Value, c3.Value))


    /// <summary>
    /// WhenAnyValue allows you to observe whenever the value of a
    /// property on an object has changed, providing an initial value when
    /// the Observable is set up, unlike ObservableForProperty(). Use this
    /// method in constructors to set up bindings between properties that also
    /// need an initial setup.
    /// </summary>
    [<Extension>]
    static member inline WhenAnyValue(this : 'TSender, property1 : Expr<'TSender -> 'T1>, property2 : Expr<'TSender -> 'T2>, property3 : Expr<'TSender -> 'T3>, property4 : Expr<'TSender -> 'T4>) =
        WhenAnyMixin.WhenAny(this, property1, property2, property3, property4, fun (c1 : IObservedChange<'TSender, 'T1>) (c2 : IObservedChange<'TSender, 'T2>) (c3 : IObservedChange<'TSender, 'T3>) (c4 : IObservedChange<'TSender, 'T4>) -> (c1.Value, c2.Value, c3.Value, c4.Value))


    /// <summary>
    /// WhenAnyValue allows you to observe whenever the value of a
    /// property on an object has changed, providing an initial value when
    /// the Observable is set up, unlike ObservableForProperty(). Use this
    /// method in constructors to set up bindings between properties that also
    /// need an initial setup.
    /// </summary>
    [<Extension>]
    static member inline WhenAnyValue(this : 'TSender, property1 : Expr<'TSender -> 'T1>, property2 : Expr<'TSender -> 'T2>, property3 : Expr<'TSender -> 'T3>, property4 : Expr<'TSender -> 'T4>, property5 : Expr<'TSender -> 'T5>) =
        WhenAnyMixin.WhenAny(this, property1, property2, property3, property4, property5, fun (c1 : IObservedChange<'TSender, 'T1>) (c2 : IObservedChange<'TSender, 'T2>) (c3 : IObservedChange<'TSender, 'T3>) (c4 : IObservedChange<'TSender, 'T4>) (c5 : IObservedChange<'TSender, 'T5>) -> (c1.Value, c2.Value, c3.Value, c4.Value, c5.Value))


    /// <summary>
    /// WhenAnyValue allows you to observe whenever the value of a
    /// property on an object has changed, providing an initial value when
    /// the Observable is set up, unlike ObservableForProperty(). Use this
    /// method in constructors to set up bindings between properties that also
    /// need an initial setup.
    /// </summary>
    [<Extension>]
    static member inline WhenAnyValue(this : 'TSender, property1 : Expr<'TSender -> 'T1>, property2 : Expr<'TSender -> 'T2>, property3 : Expr<'TSender -> 'T3>, property4 : Expr<'TSender -> 'T4>, property5 : Expr<'TSender -> 'T5>, property6 : Expr<'TSender -> 'T6>) =
        WhenAnyMixin.WhenAny(this, property1, property2, property3, property4, property5, property6, fun (c1 : IObservedChange<'TSender, 'T1>) (c2 : IObservedChange<'TSender, 'T2>) (c3 : IObservedChange<'TSender, 'T3>) (c4 : IObservedChange<'TSender, 'T4>) (c5 : IObservedChange<'TSender, 'T5>) (c6 : IObservedChange<'TSender, 'T6>) -> (c1.Value, c2.Value, c3.Value, c4.Value, c5.Value, c6.Value))


    /// <summary>
    /// WhenAnyValue allows you to observe whenever the value of a
    /// property on an object has changed, providing an initial value when
    /// the Observable is set up, unlike ObservableForProperty(). Use this
    /// method in constructors to set up bindings between properties that also
    /// need an initial setup.
    /// </summary>
    [<Extension>]
    static member inline WhenAnyValue(this : 'TSender, property1 : Expr<'TSender -> 'T1>, property2 : Expr<'TSender -> 'T2>, property3 : Expr<'TSender -> 'T3>, property4 : Expr<'TSender -> 'T4>, property5 : Expr<'TSender -> 'T5>, property6 : Expr<'TSender -> 'T6>, property7 : Expr<'TSender -> 'T7>) =
        WhenAnyMixin.WhenAny(this, property1, property2, property3, property4, property5, property6, property7, fun (c1 : IObservedChange<'TSender, 'T1>) (c2 : IObservedChange<'TSender, 'T2>) (c3 : IObservedChange<'TSender, 'T3>) (c4 : IObservedChange<'TSender, 'T4>) (c5 : IObservedChange<'TSender, 'T5>) (c6 : IObservedChange<'TSender, 'T6>) (c7 : IObservedChange<'TSender, 'T7>) -> (c1.Value, c2.Value, c3.Value, c4.Value, c5.Value, c6.Value, c7.Value))


    /// <summary>
    /// WhenAnyValue allows you to observe whenever the value of a
    /// property on an object has changed, providing an initial value when
    /// the Observable is set up, unlike ObservableForProperty(). Use this
    /// method in constructors to set up bindings between properties that also
    /// need an initial setup.
    /// </summary>
    [<Extension>]
    static member inline WhenAnyValue(this : 'TSender, property1 : Expr<'TSender -> 'T1>, property2 : Expr<'TSender -> 'T2>, property3 : Expr<'TSender -> 'T3>, property4 : Expr<'TSender -> 'T4>, property5 : Expr<'TSender -> 'T5>, property6 : Expr<'TSender -> 'T6>, property7 : Expr<'TSender -> 'T7>, property8 : Expr<'TSender -> 'T8>) =
        WhenAnyMixin.WhenAny(this, property1, property2, property3, property4, property5, property6, property7, property8, fun (c1 : IObservedChange<'TSender, 'T1>) (c2 : IObservedChange<'TSender, 'T2>) (c3 : IObservedChange<'TSender, 'T3>) (c4 : IObservedChange<'TSender, 'T4>) (c5 : IObservedChange<'TSender, 'T5>) (c6 : IObservedChange<'TSender, 'T6>) (c7 : IObservedChange<'TSender, 'T7>) (c8 : IObservedChange<'TSender, 'T8>) -> (c1.Value, c2.Value, c3.Value, c4.Value, c5.Value, c6.Value, c7.Value, c8.Value))


    /// <summary>
    /// WhenAnyValue allows you to observe whenever the value of a
    /// property on an object has changed, providing an initial value when
    /// the Observable is set up, unlike ObservableForProperty(). Use this
    /// method in constructors to set up bindings between properties that also
    /// need an initial setup.
    /// </summary>
    [<Extension>]
    static member inline WhenAnyValue(this : 'TSender, property1 : Expr<'TSender -> 'T1>, property2 : Expr<'TSender -> 'T2>, property3 : Expr<'TSender -> 'T3>, property4 : Expr<'TSender -> 'T4>, property5 : Expr<'TSender -> 'T5>, property6 : Expr<'TSender -> 'T6>, property7 : Expr<'TSender -> 'T7>, property8 : Expr<'TSender -> 'T8>, property9 : Expr<'TSender -> 'T9>) =
        WhenAnyMixin.WhenAny(this, property1, property2, property3, property4, property5, property6, property7, property8, property9, fun (c1 : IObservedChange<'TSender, 'T1>) (c2 : IObservedChange<'TSender, 'T2>) (c3 : IObservedChange<'TSender, 'T3>) (c4 : IObservedChange<'TSender, 'T4>) (c5 : IObservedChange<'TSender, 'T5>) (c6 : IObservedChange<'TSender, 'T6>) (c7 : IObservedChange<'TSender, 'T7>) (c8 : IObservedChange<'TSender, 'T8>) (c9 : IObservedChange<'TSender, 'T9>) -> (c1.Value, c2.Value, c3.Value, c4.Value, c5.Value, c6.Value, c7.Value, c8.Value, c9.Value))


    /// <summary>
    /// WhenAnyValue allows you to observe whenever the value of a
    /// property on an object has changed, providing an initial value when
    /// the Observable is set up, unlike ObservableForProperty(). Use this
    /// method in constructors to set up bindings between properties that also
    /// need an initial setup.
    /// </summary>
    [<Extension>]
    static member inline WhenAnyValue(this : 'TSender, property1 : Expr<'TSender -> 'T1>, property2 : Expr<'TSender -> 'T2>, property3 : Expr<'TSender -> 'T3>, property4 : Expr<'TSender -> 'T4>, property5 : Expr<'TSender -> 'T5>, property6 : Expr<'TSender -> 'T6>, property7 : Expr<'TSender -> 'T7>, property8 : Expr<'TSender -> 'T8>, property9 : Expr<'TSender -> 'T9>, property10 : Expr<'TSender -> 'T10>) =
        WhenAnyMixin.WhenAny(this, property1, property2, property3, property4, property5, property6, property7, property8, property9, property10, fun (c1 : IObservedChange<'TSender, 'T1>) (c2 : IObservedChange<'TSender, 'T2>) (c3 : IObservedChange<'TSender, 'T3>) (c4 : IObservedChange<'TSender, 'T4>) (c5 : IObservedChange<'TSender, 'T5>) (c6 : IObservedChange<'TSender, 'T6>) (c7 : IObservedChange<'TSender, 'T7>) (c8 : IObservedChange<'TSender, 'T8>) (c9 : IObservedChange<'TSender, 'T9>) (c10 : IObservedChange<'TSender, 'T10>) -> (c1.Value, c2.Value, c3.Value, c4.Value, c5.Value, c6.Value, c7.Value, c8.Value, c9.Value, c10.Value))


    /// <summary>
    /// WhenAnyValue allows you to observe whenever the value of a
    /// property on an object has changed, providing an initial value when
    /// the Observable is set up, unlike ObservableForProperty(). Use this
    /// method in constructors to set up bindings between properties that also
    /// need an initial setup.
    /// </summary>
    [<Extension>]
    static member inline WhenAnyValue(this : 'TSender, property1 : Expr<'TSender -> 'T1>, property2 : Expr<'TSender -> 'T2>, property3 : Expr<'TSender -> 'T3>, property4 : Expr<'TSender -> 'T4>, property5 : Expr<'TSender -> 'T5>, property6 : Expr<'TSender -> 'T6>, property7 : Expr<'TSender -> 'T7>, property8 : Expr<'TSender -> 'T8>, property9 : Expr<'TSender -> 'T9>, property10 : Expr<'TSender -> 'T10>, property11 : Expr<'TSender -> 'T11>) =
        WhenAnyMixin.WhenAny(this, property1, property2, property3, property4, property5, property6, property7, property8, property9, property10, property11, fun (c1 : IObservedChange<'TSender, 'T1>) (c2 : IObservedChange<'TSender, 'T2>) (c3 : IObservedChange<'TSender, 'T3>) (c4 : IObservedChange<'TSender, 'T4>) (c5 : IObservedChange<'TSender, 'T5>) (c6 : IObservedChange<'TSender, 'T6>) (c7 : IObservedChange<'TSender, 'T7>) (c8 : IObservedChange<'TSender, 'T8>) (c9 : IObservedChange<'TSender, 'T9>) (c10 : IObservedChange<'TSender, 'T10>) (c11 : IObservedChange<'TSender, 'T11>) -> (c1.Value, c2.Value, c3.Value, c4.Value, c5.Value, c6.Value, c7.Value, c8.Value, c9.Value, c10.Value, c11.Value))


    /// <summary>
    /// WhenAnyValue allows you to observe whenever the value of a
    /// property on an object has changed, providing an initial value when
    /// the Observable is set up, unlike ObservableForProperty(). Use this
    /// method in constructors to set up bindings between properties that also
    /// need an initial setup.
    /// </summary>
    [<Extension>]
    static member inline WhenAnyValue(this : 'TSender, property1 : Expr<'TSender -> 'T1>, property2 : Expr<'TSender -> 'T2>, property3 : Expr<'TSender -> 'T3>, property4 : Expr<'TSender -> 'T4>, property5 : Expr<'TSender -> 'T5>, property6 : Expr<'TSender -> 'T6>, property7 : Expr<'TSender -> 'T7>, property8 : Expr<'TSender -> 'T8>, property9 : Expr<'TSender -> 'T9>, property10 : Expr<'TSender -> 'T10>, property11 : Expr<'TSender -> 'T11>, property12 : Expr<'TSender -> 'T12>) =
        WhenAnyMixin.WhenAny(this, property1, property2, property3, property4, property5, property6, property7, property8, property9, property10, property11, property12, fun (c1 : IObservedChange<'TSender, 'T1>) (c2 : IObservedChange<'TSender, 'T2>) (c3 : IObservedChange<'TSender, 'T3>) (c4 : IObservedChange<'TSender, 'T4>) (c5 : IObservedChange<'TSender, 'T5>) (c6 : IObservedChange<'TSender, 'T6>) (c7 : IObservedChange<'TSender, 'T7>) (c8 : IObservedChange<'TSender, 'T8>) (c9 : IObservedChange<'TSender, 'T9>) (c10 : IObservedChange<'TSender, 'T10>) (c11 : IObservedChange<'TSender, 'T11>) (c12 : IObservedChange<'TSender, 'T12>) -> (c1.Value, c2.Value, c3.Value, c4.Value, c5.Value, c6.Value, c7.Value, c8.Value, c9.Value, c10.Value, c11.Value, c12.Value))


    /// <summary>
    /// WhenAnyValue allows you to observe whenever the value of a
    /// property on an object has changed, providing an initial value when
    /// the Observable is set up, unlike ObservableForProperty(). Use this
    /// method in constructors to set up bindings between properties that also
    /// need an initial setup.
    /// </summary>
    [<Extension>]
    static member inline WhenAnyValue(this : 'TSender, property1 : Expr<'TSender -> 'T1>, selector : 'T1 -> 'TRet) =
        WhenAnyMixin.WhenAny(this, property1, fun c1 -> selector (c1.Value))


    /// <summary>
    /// WhenAnyValue allows you to observe whenever the value of a
    /// property on an object has changed, providing an initial value when
    /// the Observable is set up, unlike ObservableForProperty(). Use this
    /// method in constructors to set up bindings between properties that also
    /// need an initial setup.
    /// </summary>
    [<Extension>]
    static member inline WhenAnyValue(this : 'TSender, property1 : Expr<'TSender -> 'T1>, property2 : Expr<'TSender -> 'T2>, selector : 'T1 -> 'T2 -> 'TRet) =
        WhenAnyMixin.WhenAny(this, property1, property2, fun c1 c2 -> selector (c1.Value) (c2.Value))


    /// <summary>
    /// WhenAnyValue allows you to observe whenever the value of a
    /// property on an object has changed, providing an initial value when
    /// the Observable is set up, unlike ObservableForProperty(). Use this
    /// method in constructors to set up bindings between properties that also
    /// need an initial setup.
    /// </summary>
    [<Extension>]
    static member inline WhenAnyValue(this : 'TSender, property1 : Expr<'TSender -> 'T1>, property2 : Expr<'TSender -> 'T2>, property3 : Expr<'TSender -> 'T3>, selector : 'T1 -> 'T2 -> 'T3 -> 'TRet) =
        WhenAnyMixin.WhenAny(this, property1, property2, property3, fun c1 c2 c3 -> selector (c1.Value) (c2.Value) (c3.Value))


    /// <summary>
    /// WhenAnyValue allows you to observe whenever the value of a
    /// property on an object has changed, providing an initial value when
    /// the Observable is set up, unlike ObservableForProperty(). Use this
    /// method in constructors to set up bindings between properties that also
    /// need an initial setup.
    /// </summary>
    [<Extension>]
    static member inline WhenAnyValue(this : 'TSender, property1 : Expr<'TSender -> 'T1>, property2 : Expr<'TSender -> 'T2>, property3 : Expr<'TSender -> 'T3>, property4 : Expr<'TSender -> 'T4>, selector : 'T1 -> 'T2 -> 'T3 -> 'T4 -> 'TRet) =
        WhenAnyMixin.WhenAny(this, property1, property2, property3, property4, fun c1 c2 c3 c4 -> selector (c1.Value) (c2.Value) (c3.Value) (c4.Value))


    /// <summary>
    /// WhenAnyValue allows you to observe whenever the value of a
    /// property on an object has changed, providing an initial value when
    /// the Observable is set up, unlike ObservableForProperty(). Use this
    /// method in constructors to set up bindings between properties that also
    /// need an initial setup.
    /// </summary>
    [<Extension>]
    static member inline WhenAnyValue(this : 'TSender, property1 : Expr<'TSender -> 'T1>, property2 : Expr<'TSender -> 'T2>, property3 : Expr<'TSender -> 'T3>, property4 : Expr<'TSender -> 'T4>, property5 : Expr<'TSender -> 'T5>, selector : 'T1 -> 'T2 -> 'T3 -> 'T4 -> 'T5 -> 'TRet) =
        WhenAnyMixin.WhenAny(this, property1, property2, property3, property4, property5, fun c1 c2 c3 c4 c5 -> selector (c1.Value) (c2.Value) (c3.Value) (c4.Value) (c5.Value))


    /// <summary>
    /// WhenAnyValue allows you to observe whenever the value of a
    /// property on an object has changed, providing an initial value when
    /// the Observable is set up, unlike ObservableForProperty(). Use this
    /// method in constructors to set up bindings between properties that also
    /// need an initial setup.
    /// </summary>
    [<Extension>]
    static member inline WhenAnyValue(this : 'TSender, property1 : Expr<'TSender -> 'T1>, property2 : Expr<'TSender -> 'T2>, property3 : Expr<'TSender -> 'T3>, property4 : Expr<'TSender -> 'T4>, property5 : Expr<'TSender -> 'T5>, property6 : Expr<'TSender -> 'T6>, selector : 'T1 -> 'T2 -> 'T3 -> 'T4 -> 'T5 -> 'T6 -> 'TRet) =
        WhenAnyMixin.WhenAny(this, property1, property2, property3, property4, property5, property6, fun c1 c2 c3 c4 c5 c6 -> selector (c1.Value) (c2.Value) (c3.Value) (c4.Value) (c5.Value) (c6.Value))


    /// <summary>
    /// WhenAnyValue allows you to observe whenever the value of a
    /// property on an object has changed, providing an initial value when
    /// the Observable is set up, unlike ObservableForProperty(). Use this
    /// method in constructors to set up bindings between properties that also
    /// need an initial setup.
    /// </summary>
    [<Extension>]
    static member inline WhenAnyValue(this : 'TSender, property1 : Expr<'TSender -> 'T1>, property2 : Expr<'TSender -> 'T2>, property3 : Expr<'TSender -> 'T3>, property4 : Expr<'TSender -> 'T4>, property5 : Expr<'TSender -> 'T5>, property6 : Expr<'TSender -> 'T6>, property7 : Expr<'TSender -> 'T7>, selector : 'T1 -> 'T2 -> 'T3 -> 'T4 -> 'T5 -> 'T6 -> 'T7 -> 'TRet) =
        WhenAnyMixin.WhenAny(this, property1, property2, property3, property4, property5, property6, property7, fun c1 c2 c3 c4 c5 c6 c7 -> selector (c1.Value) (c2.Value) (c3.Value) (c4.Value) (c5.Value) (c6.Value) (c7.Value))


    /// <summary>
    /// WhenAnyValue allows you to observe whenever the value of a
    /// property on an object has changed, providing an initial value when
    /// the Observable is set up, unlike ObservableForProperty(). Use this
    /// method in constructors to set up bindings between properties that also
    /// need an initial setup.
    /// </summary>
    [<Extension>]
    static member inline WhenAnyValue(this : 'TSender, property1 : Expr<'TSender -> 'T1>, property2 : Expr<'TSender -> 'T2>, property3 : Expr<'TSender -> 'T3>, property4 : Expr<'TSender -> 'T4>, property5 : Expr<'TSender -> 'T5>, property6 : Expr<'TSender -> 'T6>, property7 : Expr<'TSender -> 'T7>, property8 : Expr<'TSender -> 'T8>, selector : 'T1 -> 'T2 -> 'T3 -> 'T4 -> 'T5 -> 'T6 -> 'T7 -> 'T8 -> 'TRet) =
        WhenAnyMixin.WhenAny(this, property1, property2, property3, property4, property5, property6, property7, property8, fun c1 c2 c3 c4 c5 c6 c7 c8 -> selector (c1.Value) (c2.Value) (c3.Value) (c4.Value) (c5.Value) (c6.Value) (c7.Value) (c8.Value))


    /// <summary>
    /// WhenAnyValue allows you to observe whenever the value of a
    /// property on an object has changed, providing an initial value when
    /// the Observable is set up, unlike ObservableForProperty(). Use this
    /// method in constructors to set up bindings between properties that also
    /// need an initial setup.
    /// </summary>
    [<Extension>]
    static member inline WhenAnyValue(this : 'TSender, property1 : Expr<'TSender -> 'T1>, property2 : Expr<'TSender -> 'T2>, property3 : Expr<'TSender -> 'T3>, property4 : Expr<'TSender -> 'T4>, property5 : Expr<'TSender -> 'T5>, property6 : Expr<'TSender -> 'T6>, property7 : Expr<'TSender -> 'T7>, property8 : Expr<'TSender -> 'T8>, property9 : Expr<'TSender -> 'T9>, selector : 'T1 -> 'T2 -> 'T3 -> 'T4 -> 'T5 -> 'T6 -> 'T7 -> 'T8 -> 'T9 -> 'TRet) =
        WhenAnyMixin.WhenAny(this, property1, property2, property3, property4, property5, property6, property7, property8, property9, fun c1 c2 c3 c4 c5 c6 c7 c8 c9 -> selector (c1.Value) (c2.Value) (c3.Value) (c4.Value) (c5.Value) (c6.Value) (c7.Value) (c8.Value) (c9.Value))


    /// <summary>
    /// WhenAnyValue allows you to observe whenever the value of a
    /// property on an object has changed, providing an initial value when
    /// the Observable is set up, unlike ObservableForProperty(). Use this
    /// method in constructors to set up bindings between properties that also
    /// need an initial setup.
    /// </summary>
    [<Extension>]
    static member inline WhenAnyValue(this : 'TSender, property1 : Expr<'TSender -> 'T1>, property2 : Expr<'TSender -> 'T2>, property3 : Expr<'TSender -> 'T3>, property4 : Expr<'TSender -> 'T4>, property5 : Expr<'TSender -> 'T5>, property6 : Expr<'TSender -> 'T6>, property7 : Expr<'TSender -> 'T7>, property8 : Expr<'TSender -> 'T8>, property9 : Expr<'TSender -> 'T9>, property10 : Expr<'TSender -> 'T10>, selector : 'T1 -> 'T2 -> 'T3 -> 'T4 -> 'T5 -> 'T6 -> 'T7 -> 'T8 -> 'T9 -> 'T10 -> 'TRet) =
        WhenAnyMixin.WhenAny(this, property1, property2, property3, property4, property5, property6, property7, property8, property9, property10, fun c1 c2 c3 c4 c5 c6 c7 c8 c9 c10 -> selector (c1.Value) (c2.Value) (c3.Value) (c4.Value) (c5.Value) (c6.Value) (c7.Value) (c8.Value) (c9.Value) (c10.Value))


    /// <summary>
    /// WhenAnyValue allows you to observe whenever the value of a
    /// property on an object has changed, providing an initial value when
    /// the Observable is set up, unlike ObservableForProperty(). Use this
    /// method in constructors to set up bindings between properties that also
    /// need an initial setup.
    /// </summary>
    [<Extension>]
    static member inline WhenAnyValue(this : 'TSender, property1 : Expr<'TSender -> 'T1>, property2 : Expr<'TSender -> 'T2>, property3 : Expr<'TSender -> 'T3>, property4 : Expr<'TSender -> 'T4>, property5 : Expr<'TSender -> 'T5>, property6 : Expr<'TSender -> 'T6>, property7 : Expr<'TSender -> 'T7>, property8 : Expr<'TSender -> 'T8>, property9 : Expr<'TSender -> 'T9>, property10 : Expr<'TSender -> 'T10>, property11 : Expr<'TSender -> 'T11>, selector : 'T1 -> 'T2 -> 'T3 -> 'T4 -> 'T5 -> 'T6 -> 'T7 -> 'T8 -> 'T9 -> 'T10 -> 'T11 -> 'TRet) =
        WhenAnyMixin.WhenAny(this, property1, property2, property3, property4, property5, property6, property7, property8, property9, property10, property11, fun c1 c2 c3 c4 c5 c6 c7 c8 c9 c10 c11 -> selector (c1.Value) (c2.Value) (c3.Value) (c4.Value) (c5.Value) (c6.Value) (c7.Value) (c8.Value) (c9.Value) (c10.Value) (c11.Value))


    /// <summary>
    /// WhenAnyValue allows you to observe whenever the value of a
    /// property on an object has changed, providing an initial value when
    /// the Observable is set up, unlike ObservableForProperty(). Use this
    /// method in constructors to set up bindings between properties that also
    /// need an initial setup.
    /// </summary>
    [<Extension>]
    static member inline WhenAnyValue(this : 'TSender, property1 : Expr<'TSender -> 'T1>, property2 : Expr<'TSender -> 'T2>, property3 : Expr<'TSender -> 'T3>, property4 : Expr<'TSender -> 'T4>, property5 : Expr<'TSender -> 'T5>, property6 : Expr<'TSender -> 'T6>, property7 : Expr<'TSender -> 'T7>, property8 : Expr<'TSender -> 'T8>, property9 : Expr<'TSender -> 'T9>, property10 : Expr<'TSender -> 'T10>, property11 : Expr<'TSender -> 'T11>, property12 : Expr<'TSender -> 'T12>, selector : 'T1 -> 'T2 -> 'T3 -> 'T4 -> 'T5 -> 'T6 -> 'T7 -> 'T8 -> 'T9 -> 'T10 -> 'T11 -> 'T12 -> 'TRet) =
        WhenAnyMixin.WhenAny(this, property1, property2, property3, property4, property5, property6, property7, property8, property9, property10, property11, property12, fun c1 c2 c3 c4 c5 c6 c7 c8 c9 c10 c11 c12 -> selector (c1.Value) (c2.Value) (c3.Value) (c4.Value) (c5.Value) (c6.Value) (c7.Value) (c8.Value) (c9.Value) (c10.Value) (c11.Value) (c12.Value))

[<Extension>]
type WhenAnyObservableMixin() =

    [<Extension>]
    static member inline WhenAnyObservable(this : 'TSender, obs1 : Expr<'TSender -> IObservable<'TRet>>) =
        WhenAnyMixin.WhenAny(this, obs1, fun x -> x.Value).Switch()
    [<Extension>]
    static member inline WhenAnyObservable(this : 'TSender, obs1 : Expr<'TSender -> IObservable<'TRet>>, obs2 : Expr<'TSender -> IObservable<'TRet>>) =
        WhenAnyMixin.WhenAny(this, obs1, obs2, fun o1 o2 -> [| o1.Value; o2.Value |]).Select(fun x -> x.Merge()).Switch()

    [<Extension>]
    static member inline WhenAnyObservable(this : 'TSender, obs1 : Expr<'TSender -> IObservable<'TRet>>, obs2 : Expr<'TSender -> IObservable<'TRet>>, obs3 : Expr<'TSender -> IObservable<'TRet>>) =
        WhenAnyMixin.WhenAny(this, obs1, obs2, obs3, fun o1 o2 o3 -> [| o1.Value; o2.Value; o3.Value |]).Select(fun x -> x.Merge()).Switch()

    [<Extension>]
    static member inline WhenAnyObservable(this : 'TSender, obs1 : Expr<'TSender -> IObservable<'TRet>>, obs2 : Expr<'TSender -> IObservable<'TRet>>, obs3 : Expr<'TSender -> IObservable<'TRet>>, obs4 : Expr<'TSender -> IObservable<'TRet>>) =
        WhenAnyMixin.WhenAny(this, obs1, obs2, obs3, obs4, fun o1 o2 o3 o4 -> [| o1.Value; o2.Value; o3.Value; o4.Value |]).Select(fun x -> x.Merge()).Switch()

    [<Extension>]
    static member inline WhenAnyObservable(this : 'TSender, obs1 : Expr<'TSender -> IObservable<'TRet>>, obs2 : Expr<'TSender -> IObservable<'TRet>>, obs3 : Expr<'TSender -> IObservable<'TRet>>, obs4 : Expr<'TSender -> IObservable<'TRet>>, obs5 : Expr<'TSender -> IObservable<'TRet>>) =
        WhenAnyMixin.WhenAny(this, obs1, obs2, obs3, obs4, obs5, fun o1 o2 o3 o4 o5 -> [| o1.Value; o2.Value; o3.Value; o4.Value; o5.Value |]).Select(fun x -> x.Merge()).Switch()

    [<Extension>]
    static member inline WhenAnyObservable(this : 'TSender, obs1 : Expr<'TSender -> IObservable<'TRet>>, obs2 : Expr<'TSender -> IObservable<'TRet>>, obs3 : Expr<'TSender -> IObservable<'TRet>>, obs4 : Expr<'TSender -> IObservable<'TRet>>, obs5 : Expr<'TSender -> IObservable<'TRet>>, obs6 : Expr<'TSender -> IObservable<'TRet>>) =
        WhenAnyMixin.WhenAny(this, obs1, obs2, obs3, obs4, obs5, obs6, fun o1 o2 o3 o4 o5 o6 -> [| o1.Value; o2.Value; o3.Value; o4.Value; o5.Value; o6.Value |]).Select(fun x -> x.Merge()).Switch()

    [<Extension>]
    static member inline WhenAnyObservable(this : 'TSender, obs1 : Expr<'TSender -> IObservable<'TRet>>, obs2 : Expr<'TSender -> IObservable<'TRet>>, obs3 : Expr<'TSender -> IObservable<'TRet>>, obs4 : Expr<'TSender -> IObservable<'TRet>>, obs5 : Expr<'TSender -> IObservable<'TRet>>, obs6 : Expr<'TSender -> IObservable<'TRet>>, obs7 : Expr<'TSender -> IObservable<'TRet>>) =
        WhenAnyMixin.WhenAny(this, obs1, obs2, obs3, obs4, obs5, obs6, obs7, fun o1 o2 o3 o4 o5 o6 o7 -> [| o1.Value; o2.Value; o3.Value; o4.Value; o5.Value; o6.Value; o7.Value |]).Select(fun x -> x.Merge()).Switch()

    [<Extension>]
    static member inline WhenAnyObservable(this : 'TSender, obs1 : Expr<'TSender -> IObservable<'TRet>>, obs2 : Expr<'TSender -> IObservable<'TRet>>, obs3 : Expr<'TSender -> IObservable<'TRet>>, obs4 : Expr<'TSender -> IObservable<'TRet>>, obs5 : Expr<'TSender -> IObservable<'TRet>>, obs6 : Expr<'TSender -> IObservable<'TRet>>, obs7 : Expr<'TSender -> IObservable<'TRet>>, obs8 : Expr<'TSender -> IObservable<'TRet>>) =
        WhenAnyMixin.WhenAny(this, obs1, obs2, obs3, obs4, obs5, obs6, obs7, obs8, fun o1 o2 o3 o4 o5 o6 o7 o8 -> [| o1.Value; o2.Value; o3.Value; o4.Value; o5.Value; o6.Value; o7.Value; o8.Value |]).Select(fun x -> x.Merge()).Switch()

    [<Extension>]
    static member inline WhenAnyObservable(this : 'TSender, obs1 : Expr<'TSender -> IObservable<'TRet>>, obs2 : Expr<'TSender -> IObservable<'TRet>>, obs3 : Expr<'TSender -> IObservable<'TRet>>, obs4 : Expr<'TSender -> IObservable<'TRet>>, obs5 : Expr<'TSender -> IObservable<'TRet>>, obs6 : Expr<'TSender -> IObservable<'TRet>>, obs7 : Expr<'TSender -> IObservable<'TRet>>, obs8 : Expr<'TSender -> IObservable<'TRet>>, obs9 : Expr<'TSender -> IObservable<'TRet>>) =
        WhenAnyMixin.WhenAny(this, obs1, obs2, obs3, obs4, obs5, obs6, obs7, obs8, obs9, fun o1 o2 o3 o4 o5 o6 o7 o8 o9 -> [| o1.Value; o2.Value; o3.Value; o4.Value; o5.Value; o6.Value; o7.Value; o8.Value; o9.Value |]).Select(fun x -> x.Merge()).Switch()

    [<Extension>]
    static member inline WhenAnyObservable(this : 'TSender, obs1 : Expr<'TSender -> IObservable<'TRet>>, obs2 : Expr<'TSender -> IObservable<'TRet>>, obs3 : Expr<'TSender -> IObservable<'TRet>>, obs4 : Expr<'TSender -> IObservable<'TRet>>, obs5 : Expr<'TSender -> IObservable<'TRet>>, obs6 : Expr<'TSender -> IObservable<'TRet>>, obs7 : Expr<'TSender -> IObservable<'TRet>>, obs8 : Expr<'TSender -> IObservable<'TRet>>, obs9 : Expr<'TSender -> IObservable<'TRet>>, obs10 : Expr<'TSender -> IObservable<'TRet>>) =
        WhenAnyMixin.WhenAny(this, obs1, obs2, obs3, obs4, obs5, obs6, obs7, obs8, obs9, obs10, fun o1 o2 o3 o4 o5 o6 o7 o8 o9 o10 -> [| o1.Value; o2.Value; o3.Value; o4.Value; o5.Value; o6.Value; o7.Value; o8.Value; o9.Value; o10.Value |]).Select(fun x -> x.Merge()).Switch()

    [<Extension>]
    static member inline WhenAnyObservable(this : 'TSender, obs1 : Expr<'TSender -> IObservable<'TRet>>, obs2 : Expr<'TSender -> IObservable<'TRet>>, obs3 : Expr<'TSender -> IObservable<'TRet>>, obs4 : Expr<'TSender -> IObservable<'TRet>>, obs5 : Expr<'TSender -> IObservable<'TRet>>, obs6 : Expr<'TSender -> IObservable<'TRet>>, obs7 : Expr<'TSender -> IObservable<'TRet>>, obs8 : Expr<'TSender -> IObservable<'TRet>>, obs9 : Expr<'TSender -> IObservable<'TRet>>, obs10 : Expr<'TSender -> IObservable<'TRet>>, obs11 : Expr<'TSender -> IObservable<'TRet>>) =
        WhenAnyMixin.WhenAny(this, obs1, obs2, obs3, obs4, obs5, obs6, obs7, obs8, obs9, obs10, obs11, fun o1 o2 o3 o4 o5 o6 o7 o8 o9 o10 o11 -> [| o1.Value; o2.Value; o3.Value; o4.Value; o5.Value; o6.Value; o7.Value; o8.Value; o9.Value; o10.Value; o11.Value |]).Select(fun x -> x.Merge()).Switch()

    [<Extension>]
    static member inline WhenAnyObservable(this : 'TSender, obs1 : Expr<'TSender -> IObservable<'TRet>>, obs2 : Expr<'TSender -> IObservable<'TRet>>, obs3 : Expr<'TSender -> IObservable<'TRet>>, obs4 : Expr<'TSender -> IObservable<'TRet>>, obs5 : Expr<'TSender -> IObservable<'TRet>>, obs6 : Expr<'TSender -> IObservable<'TRet>>, obs7 : Expr<'TSender -> IObservable<'TRet>>, obs8 : Expr<'TSender -> IObservable<'TRet>>, obs9 : Expr<'TSender -> IObservable<'TRet>>, obs10 : Expr<'TSender -> IObservable<'TRet>>, obs11 : Expr<'TSender -> IObservable<'TRet>>, obs12 : Expr<'TSender -> IObservable<'TRet>>) =
        WhenAnyMixin.WhenAny(this, obs1, obs2, obs3, obs4, obs5, obs6, obs7, obs8, obs9, obs10, obs11, obs12, fun o1 o2 o3 o4 o5 o6 o7 o8 o9 o10 o11 o12 -> [| o1.Value; o2.Value; o3.Value; o4.Value; o5.Value; o6.Value; o7.Value; o8.Value; o9.Value; o10.Value; o11.Value; o12.Value |]).Select(fun x -> x.Merge()).Switch()

