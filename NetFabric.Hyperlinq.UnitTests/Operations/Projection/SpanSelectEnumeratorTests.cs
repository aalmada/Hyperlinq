using System;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Assertive;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests.Span;

public class SpanSelectEnumeratorTests
{
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void SelectReadOnlySpanEnumerable_Enumerator_ShouldEnumerateCorrectly(TestCase<int[]> testCase)
    {
        var array = testCase.Factory();
        var span = array.AsSpan();
        var enumerable = span.Select(x => x * 2);
        
        var result = new List<int>();
        foreach (var item in enumerable)
        {
            result.Add(item);
        }
        
        var expected = Enumerable.Select(array, x => x * 2);
        _ = result.Must().BeEnumerableOf<int>().BeEqualTo(expected);
    }

    [Test]
    public void SelectReadOnlySpanEnumerable_Enumerator_Empty_ShouldNotEnumerate()
    {
        ReadOnlySpan<int> span = Array.Empty<int>();
        var enumerable = span.Select(x => x * 2);
        
        var result = new List<int>();
        foreach (var item in enumerable)
        {
            result.Add(item);
        }
        
        _ = result.Must().BeEnumerableOf<int>().BeEmpty();
    }

    [Test]
    public void SelectReadOnlySpanEnumerable_Enumerator_SingleElement_ShouldWork()
    {
        ReadOnlySpan<int> span = stackalloc int[] { 5 };
        var enumerable = span.Select(x => x * 2);
        
        var result = new List<int>();
        foreach (var item in enumerable)
        {
            result.Add(item);
        }
        
        _ = result.Must().BeEnumerableOf<int>().BeEqualTo(new[] { 10 });
    }

    [Test]
    public void SelectReadOnlySpanEnumerable_Enumerator_LargeArray_ShouldWork()
    {
        var array = Enumerable.Range(1, 100).ToArray();
        var span = array.AsSpan();
        var enumerable = span.Select(x => x * 2);
        
        var result = new List<int>();
        foreach (var item in enumerable)
        {
            result.Add(item);
        }
        
        var expected = Enumerable.Select(array, x => x * 2);
        _ = result.Must().BeEnumerableOf<int>().BeEqualTo(expected);
    }

    [Test]
    public void SelectReadOnlySpanEnumerable_GetEnumerator_MultipleTimes_ShouldWork()
    {
        var array = new[] { 1, 2, 3 };
        var span = array.AsSpan();
        var enumerable = span.Select(x => x * 2);
        
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
        
        _ = result1.Must().BeEnumerableOf<int>().BeEqualTo(new[] { 2, 4, 6 });
        _ = result2.Must().BeEnumerableOf<int>().BeEqualTo(new[] { 2, 4, 6 });
    }
}
