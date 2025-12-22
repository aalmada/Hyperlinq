using System;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Assertive;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests.Span;

public class SpanCountTests
{
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_Count_ShouldMatchLinq(TestCase<int[]> testCase)
    {
        var array = testCase.Factory();

        var hyperlinqResult = array.Count();
        var linqResult = Enumerable.Count(array);

        _ = hyperlinqResult.Must().BeEqualTo(linqResult);
    }

    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_Count_ShouldMatchLinq(TestCase<int[]> testCase)
    {
        var list = new List<int>(testCase.Factory());

        var hyperlinqResult = list.Count();
        var linqResult = Enumerable.Count(list);

        _ = hyperlinqResult.Must().BeEqualTo(linqResult);
    }

    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Memory_Count_ShouldMatchLinq(TestCase<int[]> testCase)
    {
        var array = testCase.Factory();
        ReadOnlyMemory<int> memory = array.AsMemory();

        var hyperlinqResult = memory.Span.Count();
        var linqResult = Enumerable.Count(array);

        _ = hyperlinqResult.Must().BeEqualTo(linqResult);
    }

    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_Where_Count_ShouldMatchLinq(TestCase<int[]> testCase)
    {
        var array = testCase.Factory();

        var hyperlinqResult = array.Where(x => x % 2 == 0).Count();
        var linqResult = array.Where(x => x % 2 == 0).Count();

        _ = hyperlinqResult.Must().BeEqualTo(linqResult);
    }

    // Direct predicate tests for optimized Count(predicate) method
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Span_Count_WithPredicate_ShouldMatchLinq(TestCase<int[]> testCase)
    {
        var array = testCase.Factory();
        var span = array.AsSpan();

        var hyperlinqResult = span.Count(x => x % 2 == 0);
        var linqResult = Enumerable.Count(array, x => x % 2 == 0);

        _ = hyperlinqResult.Must().BeEqualTo(linqResult);
    }

    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Span_Count_WithValueDelegate_ShouldMatchLinq(TestCase<int[]> testCase)
    {
        var array = testCase.Factory();
        var span = array.AsSpan();

        var hyperlinqResult = span.Count(new IsEvenPredicate());
        var linqResult = Enumerable.Count(array, x => x % 2 == 0);

        _ = hyperlinqResult.Must().BeEqualTo(linqResult);
    }
}

readonly struct IsEvenPredicate : IFunction<int, bool>
{
    public bool Invoke(int element) => element % 2 == 0;
}
