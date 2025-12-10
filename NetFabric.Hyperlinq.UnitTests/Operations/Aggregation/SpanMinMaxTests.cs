using System;
using System.Collections.Generic;
using System.Linq;
using TUnit.Core;
using NetFabric.Assertive;
using NetFabric.Hyperlinq;

namespace NetFabric.Hyperlinq.UnitTests.Span;

public class SpanMinMaxTests
{
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_Min_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        if (array.Length == 0) return; // Skip empty
        
        var spanResult = array.Min();  // SpanExtensions.Min
        var linqResult = Enumerable.Min(array);
        
        spanResult.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_Max_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        if (array.Length == 0) return;
        
        var spanResult = array.Max();
        var linqResult = Enumerable.Max(array);
        
        spanResult.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_Min_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var list = new List<int>(testCase.arrayFactory());
        if (list.Count == 0) return;
        
        var spanResult = list.Min();
        var linqResult = Enumerable.Min(list);
        
        spanResult.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_Max_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var list = new List<int>(testCase.arrayFactory());
        if (list.Count == 0) return;
        
        var spanResult = list.Max();
        var linqResult = Enumerable.Max(list);
        
        spanResult.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void ReadOnlyMemory_Min_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        if (array.Length == 0) return;
        
        ReadOnlyMemory<int> memory = array.AsMemory();
        
        var result = memory.Min();
        var linqResult = Enumerable.Min(array);
        
        result.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void ReadOnlyMemory_Max_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        if (array.Length == 0) return;
        
        ReadOnlyMemory<int> memory = array.AsMemory();
        
        var result = memory.Max();
        var linqResult = Enumerable.Max(array);
        
        result.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    public void ReadOnlySpan_Min_ShouldWork()
    {
        ReadOnlySpan<int> span = stackalloc int[] { 5, 2, 8, 1, 9 };
        
        var result = span.Min();
        
        result.Must().BeEqualTo(1);
    }
    
    [Test]
    public void ReadOnlySpan_Max_ShouldWork()
    {
        ReadOnlySpan<int> span = stackalloc int[] { 5, 2, 8, 1, 9 };
        
        var result = span.Max();
        
        result.Must().BeEqualTo(9);
    }
    
    [Test]
    public void Span_Min_ShouldWork()
    {
        Span<int> span = stackalloc int[] { 5, 2, 8, 1, 9 };
        
        var result = span.Min();
        
        result.Must().BeEqualTo(1);
    }
    
    [Test]
    public void Span_Max_ShouldWork()
    {
        Span<int> span = stackalloc int[] { 5, 2, 8, 1, 9 };
        
        var result = span.Max();
        
        result.Must().BeEqualTo(9);
    }
    
    [Test]
    public void Double_Min_ShouldWork()
    {
        var array = new double[] { 1.5, 2.5, 0.5, 3.5 };
        
        var result = array.Min();
        
        result.Must().BeEqualTo(0.5);
    }
    
    [Test]
    public void Double_Max_ShouldWork()
    {
        var array = new double[] { 1.5, 2.5, 0.5, 3.5 };
        
        var result = array.Max();
        
        result.Must().BeEqualTo(3.5);
    }
    
    [Test]
    public void Long_Min_ShouldWork()
    {
        var memory = new long[] { 5L, 2L, 8L, 1L, 9L }.AsMemory();
        var result = memory.Span.Min();
        
        result.Must().BeEqualTo(1L);
    }
    
    [Test]
    public void Long_Max_ShouldWork()
    {
        var memory = new long[] { 5L, 2L, 8L, 1L, 9L }.AsMemory();
        var result = memory.Span.Max();
        
        result.Must().BeEqualTo(9L);
    }

    // Predicate tests
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_Min_WithPredicate_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        if (array.Length == 0) return;
        
        var predicate = (int x) => x > 0;
        var filtered = array.Where(predicate).ToArray();
        
        if (filtered.Length > 0)
        {
            var spanResult = array.Min(predicate);
            var linqResult = filtered.Min();
            
            spanResult.Must().BeEqualTo(linqResult);
        }
        else
        {
            Assert.Throws<InvalidOperationException>(() => array.Min(predicate));
        }
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_Max_WithPredicate_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        if (array.Length == 0) return;
        
        var predicate = (int x) => x > 0;
        var filtered = array.Where(predicate).ToArray();
        
        if (filtered.Length > 0)
        {
            var spanResult = array.Max(predicate);
            var linqResult = filtered.Max();
            
            spanResult.Must().BeEqualTo(linqResult);
        }
        else
        {
            Assert.Throws<InvalidOperationException>(() => array.Max(predicate));
        }
    }
    
    [Test]
    public void ReadOnlySpan_Min_WithPredicate_ShouldWork()
    {
        ReadOnlySpan<int> span = stackalloc int[] { -5, 2, -8, 1, 9 };
        
        var result = span.Min(x => x > 0);
        
        result.Must().BeEqualTo(1);
    }
    
    [Test]
    public void ReadOnlySpan_Max_WithPredicate_ShouldWork()
    {
        ReadOnlySpan<int> span = stackalloc int[] { -5, 2, -8, 1, 9 };
        
        var result = span.Max(x => x < 5);
        
        result.Must().BeEqualTo(2);
    }
    
    [Test]
    public void List_Min_WithPredicate_NoMatch_ShouldThrow()
    {
        var list = new List<int> { -5, -3, -1 };
        
        Assert.Throws<InvalidOperationException>(() => list.Min(x => x > 0));
    }
    
    [Test]
    public void Array_Max_WithPredicate_NoMatch_ShouldThrow()
    {
        var array = new[] { -5, -3, -1 };
        
        Assert.Throws<InvalidOperationException>(() => array.Max(x => x > 0));
    }
    
    [Test]
    public void ReadOnlyMemory_Min_WithPredicate_ShouldWork()
    {
        ReadOnlyMemory<int> memory = new int[] { -5, 2, -8, 1, 9 }.AsMemory();
        
        var result = memory.Min(x => x > 0);
        
        result.Must().BeEqualTo(1);
    }
    
    [Test]
    public void ReadOnlyMemory_Max_WithPredicate_ShouldWork()
    {
        ReadOnlyMemory<int> memory = new int[] { -5, 2, -8, 1, 9 }.AsMemory();
        
        var result = memory.Max(x => x < 5);
        
        result.Must().BeEqualTo(2);
    }
    
    // ArraySegment tests
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void ArraySegment_Min_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        if (array.Length == 0) return;
        
        var segment = new ArraySegment<int>(array);
        
        var result = segment.Min();
        var linqResult = Enumerable.Min(array);
        
        result.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void ArraySegment_Max_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        if (array.Length == 0) return;
        
        var segment = new ArraySegment<int>(array);
        
        var result = segment.Max();
        var linqResult = Enumerable.Max(array);
        
        result.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    public void ArraySegment_Min_WithPredicate_ShouldReturnMinimumOfMatchingElements()
    {
        var segment = new ArraySegment<int>(new[] { 1, 2, 3, 4, 5, 6 });
        
        var result = segment.Min(x => x % 2 == 0); // Even numbers only
        
        result.Must().BeEqualTo(2);
    }
    
    [Test]
    public void ArraySegment_Max_WithPredicate_ShouldReturnMaximumOfMatchingElements()
    {
        var segment = new ArraySegment<int>(new[] { 1, 2, 3, 4, 5, 6 });
        
        var result = segment.Max(x => x % 2 == 0); // Even numbers only
        
        result.Must().BeEqualTo(6);
    }
    
    [Test]
    public void ArraySegment_Min_WithPredicate_NoMatch_ShouldThrow()
    {
        var segment = new ArraySegment<int>(new[] { -5, -3, -1 });
        
        Assert.Throws<InvalidOperationException>(() => segment.Min(x => x > 0));
    }
    
    [Test]
    public void ArraySegment_Max_WithPredicate_NoMatch_ShouldThrow()
    {
        var segment = new ArraySegment<int>(new[] { -5, -3, -1 });
        
        Assert.Throws<InvalidOperationException>(() => segment.Max(x => x > 0));
    }
    
    [Test]
    public void ArraySegment_Min_WithOffset_ShouldWork()
    {
        var array = new[] { 10, 20, 5, 2, 8, 1, 9 };
        var segment = new ArraySegment<int>(array, 2, 5); // { 5, 2, 8, 1, 9 }
        
        var result = segment.Min();
        
        result.Must().BeEqualTo(1);
    }
    
    [Test]
    public void ArraySegment_Max_WithOffset_ShouldWork()
    {
        var array = new[] { 1, 2, 5, 2, 8, 1, 9 };
        var segment = new ArraySegment<int>(array, 2, 5); // { 5, 2, 8, 1, 9 }
        
        var result = segment.Max();
        
        result.Must().BeEqualTo(9);
    }

    // MinMax tests
    [Test]
    public void Array_MinMax_ShouldWork()
    {
        var array = new[] { 5, 2, 8, 1, 9, 3 };
        
        var result = array.MinMax();
        
        result.Min.Must().BeEqualTo(1);
        result.Max.Must().BeEqualTo(9);
    }
    
    [Test]
    public void List_MinMax_ShouldWork()
    {
        var list = new List<int> { 10, 20, 5, 30, 15 };
        
        var result = list.MinMax();
        
        result.Min.Must().BeEqualTo(5);
        result.Max.Must().BeEqualTo(30);
    }
    
    [Test]
    public void ReadOnlyMemory_MinMax_ShouldWork()
    {
        ReadOnlyMemory<int> memory = new[] { 7, 3, 9, 1, 5 }.AsMemory();
        
        var result = memory.MinMax();
        
        result.Min.Must().BeEqualTo(1);
        result.Max.Must().BeEqualTo(9);
    }
    
    [Test]
    public void ArraySegment_MinMax_ShouldWork()
    {
        var segment = new ArraySegment<int>(new[] { 4, 8, 2, 6, 1 });
        
        var result = segment.MinMax();
        
        result.Min.Must().BeEqualTo(1);
        result.Max.Must().BeEqualTo(8);
    }
    
    [Test]
    public void ReadOnlySpan_MinMax_ShouldWork()
    {
        ReadOnlySpan<int> span = stackalloc int[] { 15, 3, 22, 8, 11 };
        
        var result = span.MinMax();
        
        result.Min.Must().BeEqualTo(3);
        result.Max.Must().BeEqualTo(22);
    }
    
    [Test]
    public void Double_MinMax_ShouldWork()
    {
        var array = new double[] { 2.5, 1.1, 3.7, 0.5, 2.0 };
        
        var result = array.MinMax();
        
        result.Min.Must().BeEqualTo(0.5);
        result.Max.Must().BeEqualTo(3.7);
    }
    
    [Test]
    public void Array_MinMax_WithPredicate_ShouldWork()
    {
        var array = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };
        
        var result = array.MinMax(x => x % 2 == 0); // Even numbers: 2, 4, 6, 8
        
        result.Min.Must().BeEqualTo(2);
        result.Max.Must().BeEqualTo(8);
    }
    
    [Test]
    public void List_MinMax_WithPredicate_ShouldWork()
    {
        var list = new List<int> { 10, 15, 20, 25, 30 };
        
        var result = list.MinMax(x => x > 15); // 20, 25, 30
        
        result.Min.Must().BeEqualTo(20);
        result.Max.Must().BeEqualTo(30);
    }
    
    [Test]
    public void ArraySegment_MinMax_WithOffset_ShouldWork()
    {
        var array = new[] { 100, 200, 5, 10, 15, 20 };
        var segment = new ArraySegment<int>(array, 2, 4); // { 5, 10, 15, 20 }
        
        var result = segment.MinMax();
        
        result.Min.Must().BeEqualTo(5);
        result.Max.Must().BeEqualTo(20);
    }
    
    [Test]
    public void Array_MinMax_Empty_ShouldThrow()
    {
        var array = Array.Empty<int>();
        
        Assert.Throws<InvalidOperationException>(() => array.MinMax());
    }
    
    [Test]
    public void Array_MinMax_WithPredicate_NoMatch_ShouldThrow()
    {
        var array = new[] { 1, 3, 5, 7 };
        
        Assert.Throws<InvalidOperationException>(() => array.MinMax(x => x % 2 == 0));
    }
    
    [Test]
    public void Array_MinMax_SingleElement_ShouldReturnSame()
    {
        var array = new[] { 42 };
        
        var result = array.MinMax();
        
        result.Min.Must().BeEqualTo(42);
        result.Max.Must().BeEqualTo(42);
    }
}
