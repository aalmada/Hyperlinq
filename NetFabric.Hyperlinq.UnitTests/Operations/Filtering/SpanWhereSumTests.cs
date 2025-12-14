using System;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Assertive;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests.Span;

public class SpanWhereSumTests
{

    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_Where_Sum_ShouldMatchLinq(TestCase<int[]> testCase)
    {
        var list = new List<int>(testCase.Factory());

        var result = list.AsValueEnumerable().Where(x => x % 2 == 0).Sum();
        var expected = Enumerable.Where(list, x => x % 2 == 0).Sum();

        _ = result.Must().BeEqualTo(expected);
    }

    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Memory_Where_Sum_ShouldMatchLinq(TestCase<int[]> testCase)
    {
        var array = testCase.Factory();
        ReadOnlyMemory<int> memory = array.AsMemory();

        var result = memory.Where(x => x % 2 == 0).Sum();
        var expected = Enumerable.Where(array, x => x % 2 == 0).Sum();

        _ = result.Must().BeEqualTo(expected);
    }

    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void ArraySegment_Where_Sum_ShouldMatchLinq(TestCase<int[]> testCase)
    {
        var array = testCase.Factory();
        var segment = new ArraySegment<int>(array);

        var result = segment.Where(x => x % 2 == 0).Sum();
        var expected = Enumerable.Where(array, x => x % 2 == 0).Sum();

        _ = result.Must().BeEqualTo(expected);
    }
}
