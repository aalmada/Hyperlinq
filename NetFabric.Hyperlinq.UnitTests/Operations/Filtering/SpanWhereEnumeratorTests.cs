using System;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Assertive;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests.Span;

public class SpanWhereEnumeratorTests
{
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void WhereReadOnlySpanEnumerable_Enumerator_ShouldEnumerateCorrectly(TestCase<int[]> testCase)
    {
        var array = testCase.Factory();
        var span = array.AsSpan();
        var enumerable = span.Where(x => x % 2 == 0);
        
        var result = new List<int>();
        foreach (var item in enumerable)
        {
            result.Add(item);
        }
        
        var expected = Enumerable.Where(array, x => x % 2 == 0);
        _ = result.Must().BeEnumerableOf<int>().BeEqualTo(expected);
    }

    [Test]
    public void WhereReadOnlySpanEnumerable_Enumerator_Empty_ShouldNotEnumerate()
    {
        ReadOnlySpan<int> span = Array.Empty<int>();
        var enumerable = span.Where(x => x % 2 == 0);
        
        var result = new List<int>();
        foreach (var item in enumerable)
        {
            result.Add(item);
        }
        
        _ = result.Must().BeEnumerableOf<int>().BeEmpty();
    }

    [Test]
    public void WhereReadOnlySpanEnumerable_Enumerator_NoMatches_ShouldReturnEmpty()
    {
        ReadOnlySpan<int> span = stackalloc int[] { 1, 3, 5, 7, 9 };
        var enumerable = span.Where(x => x % 2 == 0);
        
        var result = new List<int>();
        foreach (var item in enumerable)
        {
            result.Add(item);
        }
        
        _ = result.Must().BeEnumerableOf<int>().BeEmpty();
    }

    [Test]
    public void WhereReadOnlySpanEnumerable_Enumerator_AllMatch_ShouldReturnAll()
    {
        ReadOnlySpan<int> span = stackalloc int[] { 2, 4, 6, 8 };
        var enumerable = span.Where(x => x % 2 == 0);
        
        var result = new List<int>();
        foreach (var item in enumerable)
        {
            result.Add(item);
        }
        
        _ = result.Must().BeEnumerableOf<int>().BeEqualTo(new[] { 2, 4, 6, 8 });
    }

    [Test]
    public void WhereReadOnlySpanEnumerable_Enumerator_WithValueDelegate_ShouldWork()
    {
        var array = new[] { 1, 2, 3, 4, 5, 6 };
        var span = array.AsSpan();
        var enumerable = span.Where(new IsEvenPredicate());
        
        var result = new List<int>();
        foreach (var item in enumerable)
        {
            result.Add(item);
        }
        
        _ = result.Must().BeEnumerableOf<int>().BeEqualTo(new[] { 2, 4, 6 });
    }

    [Test]
    public void WhereReadOnlySpanEnumerable_GetEnumerator_MultipleTimes_ShouldWork()
    {
        var array = new[] { 1, 2, 3, 4 };
        var span = array.AsSpan();
        var enumerable = span.Where(x => x % 2 == 0);
        
        // First enumeration
        var result1 = new List<int>();
        foreach (var item in enumerable)
        {
            result1.Add(item);
        }
        
        // Second enumeration
        var result2 = new List<int>();
        foreach (var item in enumerable)
        {
            result2.Add(item);
        }
        
        _ = result1.Must().BeEnumerableOf<int>().BeEqualTo(new[] { 2, 4 });
        _ = result2.Must().BeEnumerableOf<int>().BeEqualTo(new[] { 2, 4 });
    }
}
