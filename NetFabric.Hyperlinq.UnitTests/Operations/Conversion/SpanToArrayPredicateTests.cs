using System;
using System.Linq;
using NetFabric.Assertive;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests.Span;

public class SpanToArrayPredicateTests
{
    // Direct predicate tests for optimized ToArray(predicate) method
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Span_ToArray_WithPredicate_ShouldMatchLinq(TestCase<int[]> testCase)
    {
        var array = testCase.Factory();
        var span = array.AsSpan();

        var hyperlinqResult = span.ToArray(x => x % 2 == 0);
        var linqResult = Enumerable.ToArray(Enumerable.Where(array, x => x % 2 == 0));

        _ = hyperlinqResult.Must().BeEnumerableOf<int>().BeEqualTo(linqResult);
    }

    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Span_ToArray_WithValueDelegate_ShouldMatchLinq(TestCase<int[]> testCase)
    {
        var array = testCase.Factory();
        var span = array.AsSpan();

        var hyperlinqResult = span.ToArray(new IsEvenPredicate());
        var linqResult = Enumerable.ToArray(Enumerable.Where(array, x => x % 2 == 0));

        _ = hyperlinqResult.Must().BeEnumerableOf<int>().BeEqualTo(linqResult);
    }

    [Test]
    public void Span_ToArray_WithPredicate_EmptyResult_ShouldReturnEmptyArray()
    {
        ReadOnlySpan<int> span = stackalloc int[] { 1, 3, 5, 7, 9 };

        var result = span.ToArray(x => x % 2 == 0);

        _ = result.Must().BeEnumerableOf<int>().BeEmpty();
    }

    [Test]
    public void Span_ToArray_WithPredicate_AllMatch_ShouldReturnAllElements()
    {
        ReadOnlySpan<int> span = stackalloc int[] { 2, 4, 6, 8 };

        var result = span.ToArray(x => x % 2 == 0);

        _ = result.Must().BeEnumerableOf<int>().BeEqualTo(new[] { 2, 4, 6, 8 });
    }

    [Test]
    public void Span_ToArray_WithPredicate_SomeMatch_ShouldReturnFilteredElements()
    {
        ReadOnlySpan<int> span = stackalloc int[] { 1, 2, 3, 4, 5, 6 };

        var result = span.ToArray(x => x % 2 == 0);

        _ = result.Must().BeEnumerableOf<int>().BeEqualTo(new[] { 2, 4, 6 });
    }
}

