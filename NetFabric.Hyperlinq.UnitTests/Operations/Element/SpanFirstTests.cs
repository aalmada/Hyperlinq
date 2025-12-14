using System;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Assertive;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests.Span;

public class SpanFirstTests
{
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetNonEmptyIntArraySources))]
    public void Array_First_ShouldMatchLinq(TestCase<int[]> testCase)
    {
        var array = testCase.Factory();

        var hyperlinqResult = array.First();
        var linqResult = Enumerable.First(array);

        _ = hyperlinqResult.Must().BeEqualTo(linqResult);
    }

    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetNonEmptyIntArraySources))]
    public void List_First_ShouldMatchLinq(TestCase<int[]> testCase)
    {
        var list = new List<int>(testCase.Factory());

        var hyperlinqResult = list.First();
        var linqResult = Enumerable.First(list);

        _ = hyperlinqResult.Must().BeEqualTo(linqResult);
    }

    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetNonEmptyIntArraySources))]
    public void Memory_First_ShouldMatchLinq(TestCase<int[]> testCase)
    {
        var array = testCase.Factory();
        ReadOnlyMemory<int> memory = array.AsMemory();

        var hyperlinqResult = memory.First();
        var linqResult = Enumerable.First(array);

        _ = hyperlinqResult.Must().BeEqualTo(linqResult);
    }

    [Test]
    public void Array_First_Empty_ShouldThrow()
    {
        var array = Array.Empty<int>();
        Action action = () => array.First();
        _ = action.Must().Throw<InvalidOperationException>();
    }

    [Test]
    public void Array_Where_First_ShouldWork()
    {
        var array = new int[] { 1, 2, 3, 4, 5 };
        var result = array.Where(x => x > 3).First();
        _ = result.Must().BeEqualTo(4);
    }
}
