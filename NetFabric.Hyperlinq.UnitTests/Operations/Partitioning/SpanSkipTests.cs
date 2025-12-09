using System;
using System.Collections.Generic;
using System.Linq;
using TUnit.Core;
using NetFabric.Assertive;

namespace NetFabric.Hyperlinq.UnitTests.Partitioning;

public class SpanSkipTests
{
    [Test]
    public void Array_Skip_Zero_ShouldReturnAll()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var result = array.AsSpan().Skip(0);
        result.ToArray().Must().BeEqualTo(array);
    }

    [Test]
    public void Array_Skip_Negative_ShouldReturnAll()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var result = array.AsSpan().Skip(-5);
        result.ToArray().Must().BeEqualTo(array);
    }

    [Test]
    public void Array_Skip_PartialElements_ShouldSkipCorrectly()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var result = array.AsSpan().Skip(2);
        result.ToArray().Must().BeEqualTo(new[] { 3, 4, 5 });
    }

    [Test]
    public void Array_Skip_AllElements_ShouldReturnEmpty()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var result = array.AsSpan().Skip(5);
        result.ToArray().Must().BeEqualTo(Array.Empty<int>());
    }

    [Test]
    public void Array_Skip_MoreThanLength_ShouldReturnEmpty()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var result = array.AsSpan().Skip(10);
        result.ToArray().Must().BeEqualTo(Array.Empty<int>());
    }

    [Test]
    public void List_Skip_ShouldMatchLinq()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };
        var hyperlinqResult = list.Skip(2).ToArray();
        var linqResult = Enumerable.Skip(list, 2).ToArray();
        hyperlinqResult.Must().BeEqualTo(linqResult);
    }

    [Test]
    public void Memory_Skip_ShouldWork()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        ReadOnlyMemory<int> memory = array.AsMemory();
        var result = memory.Skip(2);
        result.ToArray().Must().BeEqualTo(new[] { 3, 4, 5 });
    }

    [Test]
    public void ArraySegment_Skip_ShouldWork()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var segment = new ArraySegment<int>(array, 1, 3); // { 2, 3, 4 }
        var result = segment.Skip(1);
        result.ToArray().Must().BeEqualTo(new[] { 3, 4 });
    }

    [Test]
    public void Skip_Chained_ShouldWork()
    {
        var array = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };
        var result = array.AsSpan().Skip(2).Skip(2);
        result.ToArray().Must().BeEqualTo(new[] { 5, 6, 7, 8 });
    }
}
