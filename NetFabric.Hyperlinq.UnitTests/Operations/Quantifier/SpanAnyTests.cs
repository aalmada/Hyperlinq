using System;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Assertive;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests.Span;

public class SpanAnyTests
{
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_Any_ShouldMatchLinq(TestCase<int[]> testCase)
    {
        var array = testCase.Factory();

        var hyperlinqResult = array.AsSpan().Any();
        var linqResult = Enumerable.Any(array);

        _ = hyperlinqResult.Must().BeEqualTo(linqResult);
    }

    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_Any_ShouldMatchLinq(TestCase<int[]> testCase)
    {
        var list = new List<int>(testCase.Factory());

        var hyperlinqResult = list.Any();
        var linqResult = Enumerable.Any(list);

        _ = hyperlinqResult.Must().BeEqualTo(linqResult);
    }

    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetNonEmptyIntArraySources))]
    public void List_Where_Any_ShouldMatchLinq(TestCase<int[]> testCase)
    {
        var list = new List<int>(testCase.Factory());

        var hyperlinqResult = list.AsValueEnumerable().Where(x => x % 2 == 0).Any();
        var linqResult = list.AsValueEnumerable().Where(x => x % 2 == 0).Any();

        _ = hyperlinqResult.Must().BeEqualTo(linqResult);
    }
}
