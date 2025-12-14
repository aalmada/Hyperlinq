using System;
using NetFabric.Assertive;
using NetFabric.Hyperlinq;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests.Span;

public class SpanSingleOrDefaultTests
{
    [Test]
    public void Array_SingleOrDefault_SingleElement_ShouldReturnElement()
    {
        var array = new int[] { 42 };
        var result = array.SingleOrDefault();

        _ = result.Must().BeEqualTo(42);
    }

    [Test]
    public void Array_SingleOrDefault_Empty_ShouldReturnDefault()
    {
        var memory = Array.Empty<int>().AsMemory();
        var result = memory.Span.SingleOrDefault();

        _ = result.Must().BeEqualTo(0);
    }

    [Test]
    public void Array_SingleOrDefault_Empty_WithDefault_ShouldReturnProvidedDefault()
    {
        var array = Array.Empty<int>();
        var result = array.SingleOrDefault(99);

        _ = result.Must().BeEqualTo(99);
    }

    [Test]
    public void Array_SingleOrDefault_MultipleElements_ShouldThrow()
    {
        var array = new int[] { 1, 2 };

        Action action = () => array.SingleOrDefault();
        _ = action.Must().Throw<InvalidOperationException>();
    }

    [Test]
    public void List_SingleOrDefault_SingleElement_ShouldReturnElement()
    {
        var list = new System.Collections.Generic.List<int> { 99 };
        var result = list.SingleOrDefault();

        _ = result.Must().BeEqualTo(99);
    }

    [Test]
    public void Memory_SingleOrDefault_SingleElement_ShouldReturnElement()
    {
        var memory = new int[] { 5 }.AsMemory();
        var result = memory.Span.SingleOrDefault();

        _ = result.Must().BeEqualTo(5);
    }
}
