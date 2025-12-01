using System;
using System.Collections.Generic;
using System.Linq;
using TUnit.Core;
using NetFabric.Assertive;

namespace NetFabric.Hyperlinq.UnitTests;

public class AsValueEnumerableMinMaxTests
{
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_AsValueEnumerable_Min_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var list = new List<int>(testCase.arrayFactory());
        if (list.Count == 0) return; // Skip empty for now, testing separately

        var valueEnum = list.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(list)
            .EvaluateTrue(e => e.Min() == list.Min());
    }

    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_AsValueEnumerable_Max_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var list = new List<int>(testCase.arrayFactory());
        if (list.Count == 0) return;

        var valueEnum = list.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(list)
            .EvaluateTrue(e => e.Max() == list.Max());
    }

    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_AsValueEnumerable_Min_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        if (array.Length == 0) return;

        var valueEnum = array.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(array)
            .EvaluateTrue(e => e.Min() == array.Min());
    }

    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_AsValueEnumerable_Max_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        if (array.Length == 0) return;

        var valueEnum = array.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(array)
            .EvaluateTrue(e => e.Max() == array.Max());
    }

    [Test]
    public void List_Empty_Min_ShouldThrow()
    {
        var list = new List<int>();
        var valueEnum = list.AsValueEnumerable();

        Assert.Throws<InvalidOperationException>(() => valueEnum.Min());
    }

    [Test]
    public void Array_Empty_Min_ShouldThrow()
    {
        var array = Array.Empty<int>();
        var valueEnum = array.AsValueEnumerable();

        Assert.Throws<InvalidOperationException>(() => valueEnum.Min());
    }
}
