using System;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Assertive;
using NetFabric.Hyperlinq;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests.Operations.Aggregation;

public class MinMaxByTests
{
    [Test]
    public void ValueEnumerable_MinBy_Should_Succeed()
    {
        var list = Enumerable.Range(1, 10).ToList();
        var result = list.AsValueEnumerable().MinBy(item => -item);

        _ = result.Must().BeEqualTo(10);
    }

    [Test]
    public void ValueEnumerable_MaxBy_Should_Succeed()
    {
        var list = Enumerable.Range(1, 10).ToList();
        var result = list.AsValueEnumerable().MaxBy(item => -item);

        _ = result.Must().BeEqualTo(1);
    }

    // [Test]
    // public void ValueEnumerable_MinMaxBy_Should_Succeed()
    // {
    //     var list = Enumerable.Range(1, 10).ToList();
    //     var result = list.AsValueEnumerable().MinMaxBy(new FunctionWrapper<int, int>(item => -item));
    //     
    //     result.Min.Must().BeEqualTo(10);
    //     result.Max.Must().BeEqualTo(1);
    // }

    [Test]
    public void ReadOnlySpan_MinBy_Should_Succeed()
    {
        ReadOnlySpan<int> span = new int[] { 1, 2, 3, 4, 5 };
        var result = span.MinBy(item => -item);

        _ = result.Must().BeEqualTo(5);
    }

    [Test]
    public void ReadOnlySpan_MaxBy_Should_Succeed()
    {
        ReadOnlySpan<int> span = new int[] { 1, 2, 3, 4, 5 };
        var result = span.MaxBy(item => -item);

        _ = result.Must().BeEqualTo(1);
    }

    [Test]
    public void ReadOnlySpan_MinMaxBy_Should_Succeed()
    {
        ReadOnlySpan<int> span = new int[] { 1, 2, 3, 4, 5 };
        var result = span.MinMaxBy(item => -item);

        _ = result.Min.Must().BeEqualTo(5);
        _ = result.Max.Must().BeEqualTo(1);
    }

    [Test]
    public void List_MinBy_Should_Succeed()
    {
        var list = Enumerable.Range(1, 10).ToList();
        var result = list.MinBy(item => -item);

        _ = result.Must().BeEqualTo(10);
    }

    [Test]
    public void List_MaxBy_Should_Succeed()
    {
        var list = Enumerable.Range(1, 10).ToList();
        var result = list.MaxBy(item => -item);

        _ = result.Must().BeEqualTo(1);
    }

    [Test]
    public void List_MinMaxBy_Should_Succeed()
    {
        var list = Enumerable.Range(1, 10).ToList();
        var result = list.MinMaxBy(item => -item);

        _ = result.Min.Must().BeEqualTo(10);
        _ = result.Max.Must().BeEqualTo(1);
    }

    [Test]
    public void Array_MinBy_Should_Succeed()
    {
        var array = Enumerable.Range(1, 10).ToArray();
        var result = array.AsValueEnumerable().MinBy(item => -item);

        _ = result.Must().BeEqualTo(10);
    }

    [Test]
    public void Array_MaxBy_Should_Succeed()
    {
        var array = Enumerable.Range(1, 10).ToArray();
        var result = array.AsValueEnumerable().MaxBy(item => -item);

        _ = result.Must().BeEqualTo(1);
    }

    [Test]
    public void Array_MinMaxBy_Should_Succeed()
    {
        var array = Enumerable.Range(1, 10).ToArray();
        var result = array.AsValueEnumerable().MinMaxBy(item => -item);

        _ = result.Min.Must().BeEqualTo(10);
        _ = result.Max.Must().BeEqualTo(1);
    }

    // [Test]
    // public void MinBy_Empty_Should_Return_None()
    // {
    //     var list = new List<int>();
    //     var result = list.AsValueEnumerable().MinByOrNone(new FunctionWrapper<int, int>(item => item));
    //     
    //     result.HasValue.Must().BeFalse();
    // }

    // [Test]
    // public void MaxBy_Empty_Should_Return_None()
    // {
    //     var list = new List<int>();
    //     var result = list.AsValueEnumerable().MaxByOrNone(new FunctionWrapper<int, int>(item => item));
    //     
    //     result.HasValue.Must().BeFalse();
    // }

    // [Test]
    // public void MinMaxBy_Empty_Should_Return_None()
    // {
    //     var list = new List<int>();
    //     var result = list.AsValueEnumerable().MinMaxByOrNone(new FunctionWrapper<int, int>(item => item));
    //     
    //     result.HasValue.Must().BeFalse();
    // }
}
