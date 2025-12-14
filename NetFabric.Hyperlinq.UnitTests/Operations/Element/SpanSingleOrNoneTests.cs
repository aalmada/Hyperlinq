using System;
using NetFabric.Assertive;
using NetFabric.Hyperlinq;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests.Span;

public class SpanSingleOrNoneTests
{
    [Test]
    public void Array_SingleOrNone_SingleElement_ShouldReturnSome()
    {
        var array = new int[] { 42 };
        var option = array.SingleOrNone();

        _ = option.HasValue.Must().BeTrue();
        _ = option.Value.Must().BeEqualTo(42);
    }

    [Test]
    public void Array_SingleOrNone_Empty_ShouldReturnNone()
    {
        var array = Array.Empty<int>();
        var option = array.SingleOrNone();

        _ = option.HasValue.Must().BeFalse();
    }

    [Test]
    public void Array_SingleOrNone_MultipleElements_ShouldThrow()
    {
        var array = new int[] { 1, 2, 3 };

        Action action = () => array.SingleOrNone();
        _ = action.Must().Throw<InvalidOperationException>();
    }

    [Test]
    public void List_SingleOrNone_SingleElement_ShouldReturnSome()
    {
        var list = new System.Collections.Generic.List<int> { 99 };
        var option = list.SingleOrNone();

        _ = option.HasValue.Must().BeTrue();
        _ = option.Value.Must().BeEqualTo(99);
    }

    [Test]
    public void Memory_SingleOrNone_SingleElement_ShouldReturnSome()
    {
        var memory = new int[] { 5 }.AsMemory();
        var option = memory.Span.SingleOrNone();

        _ = option.HasValue.Must().BeTrue();
        _ = option.Value.Must().BeEqualTo(5);
    }

    [Test]
    public void Memory_SingleOrNone_Empty_ShouldReturnNone()
    {
        var memory = Array.Empty<int>().AsMemory();
        var option = memory.Span.SingleOrNone();

        _ = option.HasValue.Must().BeFalse();
    }
}
