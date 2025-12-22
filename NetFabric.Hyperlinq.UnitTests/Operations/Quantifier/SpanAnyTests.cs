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

    // Direct predicate tests for optimized Any(predicate) method
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Span_Any_WithPredicate_ShouldMatchLinq(TestCase<int[]> testCase)
    {
        var array = testCase.Factory();
        var span = array.AsSpan();

        var hyperlinqResult = span.Any(x => x % 2 == 0);
        var linqResult = Enumerable.Any(array, x => x % 2 == 0);

        _ = hyperlinqResult.Must().BeEqualTo(linqResult);
    }

    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Span_Any_WithValueDelegate_ShouldMatchLinq(TestCase<int[]> testCase)
    {
        var array = testCase.Factory();
        var span = array.AsSpan();

        var hyperlinqResult = span.Any(new IsEvenPredicate());
        var linqResult = Enumerable.Any(array, x => x % 2 == 0);

        _ = hyperlinqResult.Must().BeEqualTo(linqResult);
    }

    [Test]
    public void Span_Any_WithPredicate_EarlyExit_ShouldReturnTrue()
    {
        ReadOnlySpan<int> span = stackalloc int[] { 1, 3, 5, 6, 7, 9 };

        var result = span.Any(x => x % 2 == 0);

        _ = result.Must().BeTrue();
    }

    [Test]
    public void Span_Any_WithPredicate_NoMatch_ShouldReturnFalse()
    {
        ReadOnlySpan<int> span = stackalloc int[] { 1, 3, 5, 7, 9 };

        var result = span.Any(x => x % 2 == 0);

        _ = result.Must().BeFalse();
    }
}
