using System;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Assertive;
using NetFabric.Hyperlinq;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests.Operations.Aggregation;

public class MinMaxTests
{
    [Test]
    public void WhereListEnumerable_MinMax_Should_Succeed()
    {
        var list = Enumerable.Range(1, 10).ToList();
        var result = list.AsValueEnumerable().Where(item => item % 2 == 0).MinMax();

        _ = result.Min.Must().BeEqualTo(2);
        _ = result.Max.Must().BeEqualTo(10);
    }

    [Test]
    public void WhereReadOnlySpanEnumerable_MinMax_Should_Succeed()
    {
        var array = Enumerable.Range(1, 10).ToArray();
        var result = array.AsValueEnumerable().Where(item => item % 2 == 0).MinMax();

        _ = result.Min.Must().BeEqualTo(2);
        _ = result.Max.Must().BeEqualTo(10);
    }

    [Test]
    public void WhereSelectListEnumerable_MinMax_Should_Succeed()
    {
        var list = Enumerable.Range(1, 10).ToList();
        var result = list.AsValueEnumerable().Where(item => item % 2 == 0).Select(item => item * 2).MinMax();

        _ = result.Min.Must().BeEqualTo(4);
        _ = result.Max.Must().BeEqualTo(20);
    }

    [Test]
    public void WhereSelectReadOnlySpanEnumerable_MinMax_Should_Succeed()
    {
        var array = Enumerable.Range(1, 10).ToArray();
        var result = array.AsValueEnumerable().Where(item => item % 2 == 0).Select(item => item * 2).MinMax();

        _ = result.Min.Must().BeEqualTo(4);
        _ = result.Max.Must().BeEqualTo(20);
    }

    [Test]
    public void SelectReadOnlySpanEnumerable_MinMax_Should_Succeed()
    {
        ReadOnlySpan<int> span = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        var result = span.Select(item => item * 2).MinMax();

        _ = result.Min.Must().BeEqualTo(2);
        _ = result.Max.Must().BeEqualTo(20);
    }

    [Test]
    public void SelectListEnumerable_MinMax_Should_Succeed()
    {
        var list = Enumerable.Range(1, 10).ToList();
        var result = list.AsValueEnumerable().Select(item => item * 2).MinMax();

        _ = result.Min.Must().BeEqualTo(2);
        _ = result.Max.Must().BeEqualTo(20);
    }
}
