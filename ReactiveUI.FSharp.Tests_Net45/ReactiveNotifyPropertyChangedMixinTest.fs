namespace ReactiveUI.FSharp.Tests

open System
open System.Reflection
open System.Linq
open Microsoft.Reactive.Testing
open ReactiveUI.Tests
open ReactiveUI.Testing
open Xunit
open ReactiveUI
open ReactiveUI.FSharp
open Microsoft.FSharp.Quotations

// TODO - version of these tests with F# syntax

type ReactiveNotifyPropertyChangedMixinTest() =
    [<Fact>]
    let OFPSimplePropertyTest() =
        (new TestScheduler()).With(fun sched -> 
            let fixture = new TestFixture()
            let changes = fixture.ObservableForProperty(<@ fun (x : TestFixture) -> x.IsOnlyOneWord @>).CreateCollection()

            fixture.IsOnlyOneWord <- "Foo"
            sched.Start()
            Assert.Equal(1, changes.Count)

            fixture.IsOnlyOneWord <- "Bar"
            sched.Start()
            Assert.Equal(2, changes.Count)

            fixture.IsOnlyOneWord <- "Baz"
            sched.Start()
            Assert.Equal(3, changes.Count)

            fixture.IsOnlyOneWord <- "Baz"
            sched.Start()
            Assert.Equal(3, changes.Count)

            Assert.True(changes.All(fun x -> x.Sender = fixture))
            Assert.True(changes.All(fun x -> x.GetPropertyName() = "IsOnlyOneWord"))
            changes.Select(fun x -> x.Value).AssertAreEqual([| "Foo"; "Bar"; "Baz" |])
        )

    [<Fact>]
    let OFPSimpleChildPropertyTest() =
        (new TestScheduler()).With(fun sched ->
            let fixture = new HostTestFixture(Child = new TestFixture())
            let changes = fixture.ObservableForProperty(<@ fun (x : HostTestFixture) -> x.Child.IsOnlyOneWord @>).CreateCollection()

            fixture.Child.IsOnlyOneWord <- "Foo"
            sched.Start()
            Assert.Equal(1, changes.Count)

            fixture.Child.IsOnlyOneWord <- "Bar"
            sched.Start()
            Assert.Equal(2, changes.Count)

            fixture.Child.IsOnlyOneWord <- "Baz"
            sched.Start()
            Assert.Equal(3, changes.Count)

            fixture.Child.IsOnlyOneWord <- "Baz"
            sched.Start()
            Assert.Equal(3, changes.Count)

            Assert.True(changes.All(fun x -> x.Sender = fixture))
            Assert.True(changes.All(fun x -> x.GetPropertyName() = "Child.IsOnlyOneWord"))
            changes.Select(fun x -> x.Value).AssertAreEqual([| "Foo"; "Bar"; "Baz" |])
        )

    [<Fact>]
    let OFPReplacingTheHostShouldResubscribeTheObservable() =
        (new TestScheduler()).With(fun sched ->
            let fixture = new HostTestFixture(Child = new TestFixture())
            let changes = fixture.ObservableForProperty(<@ fun (x : HostTestFixture) -> x.Child.IsOnlyOneWord @>).CreateCollection()

            fixture.Child.IsOnlyOneWord <- "Foo"
            sched.Start()
            Assert.Equal(1, changes.Count)

            fixture.Child.IsOnlyOneWord <- "Bar"
            sched.Start()
            Assert.Equal(2, changes.Count)

            // Tricky! This is a change too, because from the perspective 
            // of the binding, we've went from "Bar" to null
            fixture.Child <- new TestFixture()
            sched.Start()
            Assert.Equal(3, changes.Count)

            // Here we've set the value but it shouldn't change
            fixture.Child.IsOnlyOneWord <- null
            sched.Start()
            Assert.Equal(3, changes.Count)

            fixture.Child.IsOnlyOneWord <- "Baz"
            sched.Start()
            Assert.Equal(4, changes.Count)

            fixture.Child.IsOnlyOneWord <- "Baz"
            sched.Start()
            Assert.Equal(4, changes.Count)

            Assert.True(changes.All(fun x -> x.Sender = fixture))
            Assert.True(changes.All(fun x -> x.GetPropertyName() = "Child.IsOnlyOneWord"))
            changes.Select(fun x -> x.Value).AssertAreEqual([| "Foo"; "Bar"; null; "Baz" |])
        )

    [<Fact>]
    let OFPReplacingTheHostWithNullThenSettingItBackShouldResubscribeTheObservable() =
        (new TestScheduler()).With(fun sched ->
            let fixture = new HostTestFixture(Child = new TestFixture())
            let changes = fixture.ObservableForProperty(<@ fun (x : HostTestFixture) -> x.Child.IsOnlyOneWord @>).CreateCollection()

            fixture.Child.IsOnlyOneWord <- "Foo"
            sched.Start()
            Assert.Equal(1, changes.Count)

            fixture.Child.IsOnlyOneWord <- "Bar"
            sched.Start()
            Assert.Equal(2, changes.Count)

            // Oops, now the child is Null, we may now blow up
            fixture.Child <- null
            sched.Start()
            Assert.Equal(2, changes.Count)

            // Tricky! This is a change too, because from the perspective 
            // of the binding, we've went from "Bar" to null
            fixture.Child <- new TestFixture()
            sched.Start()
            Assert.Equal(3, changes.Count)

            Assert.True(changes.All(fun x -> x.Sender = fixture))
            Assert.True(changes.All(fun x -> x.GetPropertyName() = "Child.IsOnlyOneWord"))
            changes.Select(fun x -> x.Value).AssertAreEqual([| "Foo"; "Bar"; null |])
        )

    [<Fact>]
    let OFPChangingTheHostPropertyShouldFireAChildChangeNotificationOnlyIfThePreviousChildIsDifferent() =
        (new TestScheduler()).With(fun sched ->
            let fixture = new HostTestFixture(Child = new TestFixture())
            let changes = fixture.ObservableForProperty(<@ fun (x : HostTestFixture) -> x.Child.IsOnlyOneWord @>).CreateCollection()

            fixture.Child.IsOnlyOneWord <- "Foo"
            sched.Start()
            Assert.Equal(1, changes.Count)

            fixture.Child.IsOnlyOneWord <- "Bar"
            sched.Start()
            Assert.Equal(2, changes.Count)

            fixture.Child <- new TestFixture(IsOnlyOneWord = "Bar")
            sched.Start()
            Assert.Equal(2, changes.Count)
        )

    [<Fact>]
    let OFPShouldWorkWithINPCObjectsToo() =
        (new TestScheduler()).With(fun sched ->
            let fixture = new NonReactiveINPCObject(InpcProperty = null)

            let changes = fixture.ObservableForProperty(<@ fun (x : NonReactiveINPCObject) -> x.InpcProperty.IsOnlyOneWord @>).CreateCollection()

            fixture.InpcProperty <- new TestFixture() 
            sched.Start()
            Assert.Equal(1, changes.Count)

            fixture.InpcProperty.IsOnlyOneWord <- "Foo"
            sched.Start()
            Assert.Equal(2, changes.Count)

            fixture.InpcProperty.IsOnlyOneWord <- "Bar"
            sched.Start()
            Assert.Equal(3, changes.Count)
        )

    [<Fact>]
    let AnyChangeInExpressionListTriggersUpdate() =
        let obj = new ObjChain1()
        let obsUpdated = ref false

        obj.ObservableForProperty(<@ fun (x : ObjChain1) -> x.Model.Model.Model.SomeOtherParam @>).Subscribe(fun _ -> obsUpdated := true) |> ignore
           
        obsUpdated := false
        obj.Model.Model.Model.SomeOtherParam <- 42
        Assert.True(!obsUpdated)
 
        obsUpdated := false
        obj.Model.Model.Model <- new HostTestFixture()
        Assert.True(!obsUpdated)
 
        obsUpdated := false
        obj.Model.Model <- new ObjChain3(Model = new HostTestFixture(SomeOtherParam = 10))
        Assert.True(!obsUpdated)
 
        obsUpdated := false
        obj.Model <- new ObjChain2()
        Assert.True(!obsUpdated)

    [<Fact>]
    let SubscriptionToWhenAnyShouldReturnCurrentValue() =
        let obj = new HostTestFixture()
        let observedValue = ref 1
        obj.WhenAnyValue(fun x -> x.SomeOtherParam)
           .Subscribe(fun x -> observedValue := x)
           |> ignore

        obj.SomeOtherParam <- 42
            
        Assert.True(!observedValue = obj.SomeOtherParam)

    [<Fact>]
    let MultiPropertyExpressionsShouldBeProperlyResolved() =
        let checkChain expr (properties : string list) (types : Type list) =
            let chain = expr |> Reflection.Rewrite |> Expression.getExpressionChain
            properties.AssertAreEqual(chain |> List.map(Expression.getName))
            types.AssertAreEqual(chain |> List.map(fun y -> y.Type))
        checkChain <@ fun (x : HostTestFixture) -> x.Child.IsOnlyOneWord.Length @> [ "Child"; "IsOnlyOneWord"; "Length" ] [ typeof<TestFixture>; typeof<string>; typeof<int> ]
        checkChain <@ fun (x : HostTestFixture) -> x.SomeOtherParam @>             [ "SomeOtherParam" ]                   [ typeof<int> ]
        checkChain <@ fun (x : HostTestFixture) -> x.Child.IsNotNullString @>      [ "Child"; "IsNotNullString" ]         [ typeof<TestFixture>; typeof<string> ]
        checkChain <@ fun (x : HostTestFixture) -> x.Child.Changed @>              [ "Child"; "Changed" ]                 [ typeof<TestFixture>; typeof<IObservable<IReactivePropertyChangedEventArgs<IReactiveObject>>> ]

    [<Fact>]
    let WhenAnySmokeTest() =
        (new TestScheduler()).With(fun sched ->
            let fixture = new HostTestFixture(Child = new TestFixture())
            fixture.SomeOtherParam <- 5
            fixture.Child.IsNotNullString <- "Foo"

            let output1 = ResizeArray<IObservedChange<HostTestFixture, int>>()
            let output2 = ResizeArray<IObservedChange<HostTestFixture, string>>()
            fixture.WhenAny(<@ fun (x : HostTestFixture) -> x.SomeOtherParam @>, <@ fun (x : HostTestFixture) -> x.Child.IsNotNullString @>, fun x y -> (x,y)).Subscribe(fun (sop, nns) -> output1.Add(sop); output2.Add(nns)) |> ignore

            sched.Start()
            Assert.Equal(1, output1.Count)
            Assert.Equal(1, output2.Count)
            Assert.Equal(fixture, output1.[0].Sender)
            Assert.Equal(fixture, output2.[0].Sender)
            Assert.Equal(5, output1.[0].Value)
            Assert.Equal<string>("Foo", output2.[0].Value)

            fixture.SomeOtherParam <- 10
            sched.Start()
            Assert.Equal(2, output1.Count)
            Assert.Equal(2, output2.Count)
            Assert.Equal(fixture, output1.[1].Sender)
            Assert.Equal(fixture, output2.[1].Sender)
            Assert.Equal(10, output1.[1].Value)
            Assert.Equal<string>("Foo", output2.[1].Value)

            fixture.Child.IsNotNullString <- "Bar"
            sched.Start()
            Assert.Equal(3, output1.Count)
            Assert.Equal(3, output2.Count)
            Assert.Equal(fixture, output1.[2].Sender)
            Assert.Equal(fixture, output2.[2].Sender)
            Assert.Equal(10, output1.[2].Value)
            Assert.Equal<string>("Bar", output2.[2].Value)
        )

    [<Fact>]
    let WhenAnyShouldWorkEvenWithNormalProperties() =
        let fixture = new TestFixture(IsNotNullString = "Foo", IsOnlyOneWord = "Baz", PocoProperty = "Bamf")

        let output = new ResizeArray<IObservedChange<TestFixture, string>>()
        fixture.WhenAny(<@ fun (x : TestFixture) -> x.PocoProperty @>, fun x -> x).Subscribe(output.Add) |> ignore
        let output2 = new ResizeArray<string>()
        fixture.WhenAnyValue(<@ fun (x : TestFixture) -> x.PocoProperty @>).Subscribe(output2.Add) |> ignore
        let output3 = new ResizeArray<IObservedChange<TestFixture, Nullable<int>>>()
        fixture.WhenAny(<@ fun (x : TestFixture) -> x.NullableInt @>, fun x -> x).Subscribe(output3.Add) |> ignore

        let output4 = new ResizeArray<Nullable<int>>()
        fixture.WhenAnyValue(<@ fun (x : TestFixture) -> x.NullableInt @>).Subscribe(output4.Add) |> ignore
           
        Assert.Equal(1, output.Count)
        Assert.Equal(fixture, output.[0].Sender)
        Assert.Equal<string>("PocoProperty", output.[0].GetPropertyName())
        Assert.Equal<string>("Bamf", output.[0].Value)

        Assert.Equal(1, output2.Count)
        Assert.Equal<string>("Bamf", output2.[0])

        Assert.Equal(1, output3.Count)
        Assert.Equal(fixture, output3.[0].Sender)
        Assert.Equal<string>("NullableInt", output3.[0].GetPropertyName())
        Assert.Null(output3.[0].Value)

        Assert.Equal(1, output4.Count)
        Assert.Null(output4.[0])

    [<Fact>]
    let WhenAnyValueSmokeTest() =
        (new TestScheduler()).With(fun sched ->
            let fixture = new HostTestFixture(Child = new TestFixture())
            fixture.SomeOtherParam <- 5
            fixture.Child.IsNotNullString <- "Foo"

            let output1 = new ResizeArray<int>()
            let output2 = new ResizeArray<string>()
            fixture.WhenAnyValue(<@ fun (x : HostTestFixture) -> x.SomeOtherParam @>, 
                                 <@ fun (x : HostTestFixture) -> x.Child.IsNotNullString @>, 
                                 fun sop nns -> (sop, nns))
                   .Subscribe(fun (sop, nns) -> output1.Add(sop); output2.Add(nns))
                   |> ignore 
                          
            sched.Start()
            Assert.Equal(1, output1.Count)
            Assert.Equal(1, output2.Count)
            Assert.Equal(5, output1.[0])
            Assert.Equal<string>("Foo", output2.[0])

            fixture.SomeOtherParam <- 10
            sched.Start()
            Assert.Equal(2, output1.Count)
            Assert.Equal(2, output2.Count)
            Assert.Equal(10, output1.[1])
            Assert.Equal<string>("Foo", output2.[1])

            fixture.Child.IsNotNullString <- "Bar"
            sched.Start()
            Assert.Equal(3, output1.Count)
            Assert.Equal(3, output2.Count)
            Assert.Equal(10, output1.[2])
            Assert.Equal<string>("Bar", output2.[2])
        )

    // TODO - get rid of type specifiers in expression

    [<Fact>]
    let WhenAnyValueShouldWorkEvenWithNormalProperties() =
        let fixture = new TestFixture(IsNotNullString = "Foo", IsOnlyOneWord = "Baz", PocoProperty = "Bamf")

        let output1 = new ResizeArray<string>()
        let output2 = new ResizeArray<int>()
        fixture.WhenAnyValue(<@ fun (x : TestFixture) -> x.PocoProperty @>).Subscribe(output1.Add) |> ignore
        fixture.WhenAnyValue(<@ fun (x : TestFixture) -> x.IsOnlyOneWord @>, 
                             fun (x : string) -> x.Length).Subscribe(output2.Add) |> ignore

        Assert.Equal(1, output1.Count)
        Assert.Equal<string>("Bamf", output1.[0])
        Assert.Equal(1, output2.Count)
        Assert.Equal(3, output2.[0])

    [<Fact>]
    let WhenAnyShouldRunInContext() =
        let tid = Thread.CurrentThread.ManagedThreadId

        (Scheduler.TaskPool).With(fun sched ->
            let mutable whenAnyTid = 0
            let fixture = new TestFixture(IsNotNullString = "Foo", IsOnlyOneWord = "Baz", PocoProperty = "Bamf")

            fixture.WhenAnyValue(fun x -> x.IsNotNullString).Subscribe(fun x -> 
                whenAnyTid := Thread.CurrentThread.ManagedThreadId
            )

            let mutable timeout = 10
            fixture.IsNotNullString <- "Bar"
            while (Interlocked.Decrement(timeout) > 0 && whenAnyTid = 0) do Thread.Sleep(250)

            Assert.Equal(tid, whenAnyTid)
        )

    [<Fact>]
    let OFPNamedPropertyTest() =
        (new TestScheduler()).With(fun sched ->
            let fixture = new TestFixture()
            let changes = fixture.ObservableForProperty(<@ fun (x : TestFixture) -> x.IsOnlyOneWord @>).CreateCollection()

            fixture.IsOnlyOneWord <- "Foo"
            sched.Start()
            Assert.Equal(1, changes.Count)

            fixture.IsOnlyOneWord <- "Bar"
            sched.Start()
            Assert.Equal(2, changes.Count)

            fixture.IsOnlyOneWord <- "Baz"
            sched.Start()
            Assert.Equal(3, changes.Count)

            fixture.IsOnlyOneWord <- "Baz"
            sched.Start()
            Assert.Equal(3, changes.Count)

            Assert.True(changes.All(fun x -> x.Sender = fixture))
            Assert.True(changes.All(fun x -> x.GetPropertyName() = "IsOnlyOneWord"))
            changes.Select(fun x -> x.Value).AssertAreEqual([| "Foo"; "Bar"; "Baz" |])
        )

    [<Fact>]
    let OFPNamedPropertyTestNoSkipInitial() =
        (new TestScheduler()).With(fun sched ->
            let fixture = new TestFixture(IsOnlyOneWord = "Pre")
            let changes = fixture.ObservableForProperty(<@ fun (x : TestFixture) -> x.IsOnlyOneWord @>, skipInitial = false).CreateCollection()

            sched.Start()
            Assert.Equal(1, changes.Count)

            fixture.IsOnlyOneWord <- "Foo"
            sched.Start()
            Assert.Equal(2, changes.Count)

            Assert.True(changes.All(fun x -> x.Sender = fixture))
            Assert.True(changes.All(fun x -> x.GetPropertyName() = "IsOnlyOneWord"))
            changes.Select(fun x -> x.Value).AssertAreEqual([| "Pre"; "Foo" |])
        )

    [<Fact>]
    let OFPNamedPropertyTestBeforeChange() =
        (new TestScheduler()).With(fun sched ->
            let fixture = new TestFixture(IsOnlyOneWord = "Pre")
            let changes = fixture.ObservableForProperty(<@ fun (x : TestFixture) -> x.IsOnlyOneWord @>, beforeChange = true).CreateCollection()

            sched.Start()
            Assert.Equal(0, changes.Count)

            fixture.IsOnlyOneWord <- "Foo"
            sched.Start()
            Assert.Equal(1, changes.Count)

            fixture.IsOnlyOneWord <- "Bar"
            sched.Start()
            Assert.Equal(2, changes.Count)

            Assert.True(changes.All(fun x -> x.Sender = fixture))
            Assert.True(changes.All(fun x -> x.GetPropertyName() = "IsOnlyOneWord"))
            changes.Select(fun x -> x.Value).AssertAreEqual([|  "Pre"; "Foo" |])
        )

    [<Fact>]
    let OFPNamedPropertyTestRepeats() =
        (new TestScheduler()).With(fun sched ->
            let fixture = new TestFixture()
            let changes = fixture.ObservableForProperty(<@ fun (x : TestFixture) -> x.IsOnlyOneWord @>).CreateCollection()

            fixture.IsOnlyOneWord <- "Foo"
            sched.Start()
            Assert.Equal(1, changes.Count)

            fixture.IsOnlyOneWord <- "Bar"
            sched.Start()
            Assert.Equal(2, changes.Count)

            fixture.IsOnlyOneWord <- "Bar"
            sched.Start()
            Assert.Equal(2, changes.Count)

            fixture.IsOnlyOneWord <- "Foo"
            sched.Start()
            Assert.Equal(3, changes.Count)

            Assert.True(changes.All(fun x -> x.Sender = fixture))
            Assert.True(changes.All(fun x -> x.GetPropertyName() = "IsOnlyOneWord"))
            changes.Select(fun x -> x.Value).AssertAreEqual([|  "Foo"; "Bar"; "Foo" |])
        )

#if false
public class WhenAnyObservableTests
{
    [<Fact>]
    public async Task WhenAnyObservableSmokeTest() =
    {
        let fixture = new TestWhenAnyObsViewModel()

        let list = new List<int>()
        fixture.WhenAnyObservable(fun x -> x.Command1, fun x -> x.Command2)
                .Subscribe(fun x -> list.Add((int)x))

        Assert.Equal(0, list.Count)

        await fixture.Command1.ExecuteAsync(1)
        Assert.Equal(1, list.Count)

        await fixture.Command2.ExecuteAsync(2)
        Assert.Equal(2, list.Count)

        await fixture.Command1.ExecuteAsync(1)
        Assert.Equal(3, list.Count)

        Assert.True(
            [| 1, 2, 1,}.Zip(list, (expected, actual) => new {expected, actual})
                            .All(fun x -> x.expected = x.actual))
    }

    [<Fact>]
    let WhenAnyWithNullObjectShouldUpdateWhenObjectIsntNullAnymore() =
    {
        let fixture = new TestWhenAnyObsViewModel()
        let output = fixture.WhenAnyObservable(fun x -> x.MyListOfInts.CountChanged).CreateCollection()

        Assert.Equal(0, output.Count)

        fixture.MyListOfInts = new ReactiveList<int>()
        Assert.Equal(0, output.Count)

        fixture.MyListOfInts.Add(1)
        Assert.Equal(1, output.Count)

        fixture.MyListOfInts = null
        Assert.Equal(1, output.Count)
    }
}

//#if !MONO
public class HostTestView : Control, IViewFor<HostTestFixture>
{
    public HostTestFixture ViewModel {
        get { return (HostTestFixture)GetValue(ViewModelProperty); }
        set { SetValue(ViewModelProperty, value); }
    }
    public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.Register("ViewModel", typeof(HostTestFixture), typeof(HostTestView), new PropertyMetadata(null))

    object IViewFor.ViewModel {
        get { return ViewModel; }
        set { ViewModel = (HostTestFixture) value; }
    }
}

public class WhenAnyThroughDependencyObjectTests
{
    [<Fact>]
    let WhenAnyThroughAViewShouldntGiveNullValues() =
    {
        let vm = new HostTestFixture() {
            Child = new TestFixture() {IsNotNullString <- "Foo", IsOnlyOneWord <- "Baz", PocoProperty <- "Bamf"},
        }

        let fixture = new HostTestView()

        let output = new List<string>()

        Assert.Equal(0, output.Count)
        Assert.Null(fixture.ViewModel)

        fixture.WhenAnyValue(fun x -> x.ViewModel.Child.IsNotNullString).Subscribe(output.Add)

        fixture.ViewModel = vm
        Assert.Equal(1, output.Count)

        fixture.ViewModel.Child.IsNotNullString <- "Bar"
        Assert.Equal(2, output.Count)
        [|  "Foo", "Bar" }.AssertAreEqual(output)
    }
}
#endif

