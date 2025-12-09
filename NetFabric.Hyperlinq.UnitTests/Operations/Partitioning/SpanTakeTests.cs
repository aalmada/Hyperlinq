using System;
using System.Collections.Generic;
using System.Linq;
using TUnit.Core;
using NetFabric.Assertive;

namespace NetFabric.Hyperlinq.UnitTests.Partitioning;

public class SpanTakeTests
{
    [Test]
    public void Array_Take_Zero_ShouldReturnEmpty()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var result = array.AsSpan().Take(0);
        result.ToArray().Must().BeEqualTo(Array.Empty<int>());
    }

    [Test]
    public void Array_Take_Negative_ShouldReturnEmpty()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var result = array.AsSpan().Take(-5);
        result.ToArray().Must().BeEqualTo(Array.Empty<int>());
    }

    [Test]
    public void Array_Take_PartialElements_ShouldTakeCorrectly()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var result = array.AsSpan().Take(3);
        result.ToArray().Must().BeEqualTo(new[] { 1, 2, 3 });
    }

    [Test]
    public void Array_Take_AllElements_ShouldReturnAll()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var result = array.AsSpan().Take(5);
        result.ToArray().Must().BeEqualTo(array);
    }

    [Test]
    public void Array_Take_MoreThanLength_ShouldReturnAll()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var result = array.AsSpan().Take(10);
        result.ToArray().Must().BeEqualTo(array);
    }

    [Test]
    public void List_Take_ShouldMatchLinq()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };
        var hyperlinqResult = list.Take(3).ToArray();
        var linqResult = Enumerable.Take(list, 3).ToArray();
        hyperlinqResult.Must().BeEqualTo(linqResult);
    }

    [Test]
    public void Memory_Take_ShouldWork()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        ReadOnlyMemory<int> memory = array.AsMemory();
        var result = memory.Take(3);
        result.ToArray().Must().BeEqualTo(new[] { 1, 2, 3 });
    }

    [Test]
    public void ArraySegment_Take_ShouldWork()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var segment = new ArraySegment<int>(array, 1, 3); // { 2, 3, 4 }
        var result = segment.Take(2);
        result.ToArray().Must().BeEqualTo(new[] { 2, 3 });
    }

    [Test]
    public void Take_Chained_ShouldWork()
    {
        var array = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };
        var result = array.AsSpan().Take(6).Take(4);
        result.ToArray().Must().BeEqualTo(new[] { 1, 2, 3, 4 });
    }

    [Test]
    public void Skip_Then_Take_Pagination_ShouldWork()
    {
        var array = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        // Page 2, size 3 (skip 3, take 3)
        var result = array.AsSpan().Skip(3).Take(3);
        result.ToArray().Must().BeEqualTo(new[] { 4, 5, 6 });
    }
}
