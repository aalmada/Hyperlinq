using System;
using System.Collections.Generic;
using System.Linq;
using TUnit.Core;
using NetFabric.Assertive;
using NetFabric.Hyperlinq;

namespace NetFabric.Hyperlinq.UnitTests.Span;

public class SpanAverageTests
{
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_Average_ShouldWork((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        if (array.Length == 0) return; // Skip empty
        
        var result = array.Average();
        var expected = array.Sum() / array.Length; // Integer division
        
        result.Must().BeEqualTo(expected);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_Average_ShouldWork((Func<int[]> arrayFactory, string description) testCase)
    {
        var list = new List<int>(testCase.arrayFactory());
        if (list.Count == 0) return;
        
        var result = list.Average();
        var expected = list.Sum() / list.Count; // Integer division
        
        result.Must().BeEqualTo(expected);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void ReadOnlyMemory_Average_ShouldWork((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        if (array.Length == 0) return;
        
        ReadOnlyMemory<int> memory = array.AsMemory();
        
        var result = memory.Average();
        var expected = array.Sum() / array.Length; // Integer division
        
        result.Must().BeEqualTo(expected);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void ArraySegment_Average_ShouldWork((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        if (array.Length == 0) return;
        
        var segment = new ArraySegment<int>(array);
        
        var result = segment.Average();
        var expected = array.Sum() / array.Length; // Integer division
        
        result.Must().BeEqualTo(expected);
    }
    
    [Test]
    public void ReadOnlySpan_Average_ShouldWork()
    {
        ReadOnlySpan<int> span = stackalloc int[] { 5, 2, 8, 1, 9 };
        
        var result = span.Average();
        
        result.Must().BeEqualTo(5); // (5+2+8+1+9)/5 = 25/5 = 5
    }
    
    [Test]
    public void Span_Average_ShouldWork()
    {
        Span<int> span = stackalloc int[] { 10, 20, 30 };
        
        var result = span.Average();
        
        result.Must().BeEqualTo(20); // (10+20+30)/3 = 60/3 = 20
    }
    
    [Test]
    public void Double_Average_ShouldWork()
    {
        var array = new double[] { 1.5, 2.5, 3.5, 4.5 };
        
        var result = array.Average();
        
        result.Must().BeEqualTo(3.0); // (1.5+2.5+3.5+4.5)/4 = 12/4 = 3.0
    }
    
    [Test]
    public void Long_Average_ShouldWork()
    {
        var memory = new long[] { 5L, 10L, 15L, 20L }.AsMemory();
        var result = memory.Span.Average();
        
        result.Must().BeEqualTo(12L); // (5+10+15+20)/4 = 50/4 = 12 (integer division)
    }

    // Predicate tests
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_Average_WithPredicate_ShouldWork((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        if (array.Length == 0) return;
        
        var predicate = (int x) => x > 0;
        var filtered = array.Where(predicate).ToArray();
        
        if (filtered.Length > 0)
        {
            var result = array.Average(predicate);
            var expected = filtered.Sum() / filtered.Length; // Integer division
            
            result.Must().BeEqualTo(expected);
        }
        else
        {
            Assert.Throws<InvalidOperationException>(() => array.Average(predicate));
        }
    }
    
    [Test]
    public void ReadOnlySpan_Average_WithPredicate_ShouldWork()
    {
        ReadOnlySpan<int> span = stackalloc int[] { 1, 2, 3, 4, 5, 6 };
        
        var result = span.Average(x => x % 2 == 0); // Even numbers: 2, 4, 6
        
        result.Must().BeEqualTo(4); // (2+4+6)/3 = 12/3 = 4
    }
    
    [Test]
    public void List_Average_WithPredicate_ShouldWork()
    {
        var list = new List<int> { 10, 20, 30, 40, 50 };
        
        var result = list.Average(x => x > 20); // 30, 40, 50
        
        result.Must().BeEqualTo(40); // (30+40+50)/3 = 120/3 = 40
    }
    
    [Test]
    public void Array_Average_Empty_ShouldThrow()
    {
        var array = Array.Empty<int>();
        
        Assert.Throws<InvalidOperationException>(() => array.Average());
    }
    
    [Test]
    public void ReadOnlyMemory_Average_Empty_ShouldThrow()
    {
        ReadOnlyMemory<int> memory = Array.Empty<int>().AsMemory();
        
        Assert.Throws<InvalidOperationException>(() => memory.Average());
    }
    
    [Test]
    public void List_Average_WithPredicate_NoMatch_ShouldThrow()
    {
        var list = new List<int> { -5, -3, -1 };
        
        Assert.Throws<InvalidOperationException>(() => list.Average(x => x > 0));
    }
    
    [Test]
    public void Array_Average_WithPredicate_NoMatch_ShouldThrow()
    {
        var array = new[] { -5, -3, -1 };
        
        Assert.Throws<InvalidOperationException>(() => array.Average(x => x > 0));
    }
    
    [Test]
    public void ArraySegment_Average_WithPredicate_ShouldWork()
    {
        var segment = new ArraySegment<int>(new[] { 1, 2, 3, 4, 5, 6 });
        
        var result = segment.Average(x => x % 2 == 0); // Even numbers: 2, 4, 6
        
        result.Must().BeEqualTo(4); // (2+4+6)/3 = 12/3 = 4
    }
    
    [Test]
    public void ArraySegment_Average_WithOffset_ShouldWork()
    {
        var array = new[] { 100, 200, 10, 20, 30, 40 };
        var segment = new ArraySegment<int>(array, 2, 4); // { 10, 20, 30, 40 }
        
        var result = segment.Average();
        
        result.Must().BeEqualTo(25); // (10+20+30+40)/4 = 100/4 = 25
    }
    
    [Test]
    public void ReadOnlyMemory_Average_WithPredicate_ShouldWork()
    {
        ReadOnlyMemory<int> memory = new int[] { 1, 2, 3, 4, 5, 6 }.AsMemory();
        
        var result = memory.Average(x => x % 2 != 0); // Odd numbers: 1, 3, 5
        
        result.Must().BeEqualTo(3); // (1+3+5)/3 = 9/3 = 3
    }

    // AverageOrNone tests
    [Test]
    public void Array_AverageOrNone_Empty_ShouldReturnNone()
    {
        var array = Array.Empty<int>();
        
        var result = array.AverageOrNone();
        
        result.HasValue.Must().BeFalse();
    }
    
    [Test]
    public void ReadOnlyMemory_AverageOrNone_Empty_ShouldReturnNone()
    {
        ReadOnlyMemory<int> memory = Array.Empty<int>().AsMemory();
        
        var result = memory.AverageOrNone();
        
        result.HasValue.Must().BeFalse();
    }
    
    [Test]
    public void List_AverageOrNone_WithData_ShouldReturnSome()
    {
        var list = new List<int> { 10, 20, 30 };
        
        var result = list.AverageOrNone();
        
        result.HasValue.Must().BeTrue();
        result.Value.Must().BeEqualTo(20); // (10+20+30)/3 = 20
    }
    
    [Test]
    public void ArraySegment_AverageOrNone_WithPredicate_NoMatch_ShouldReturnNone()
    {
        var segment = new ArraySegment<int>(new[] { -5, -3, -1 });
        
        var result = segment.AverageOrNone(x => x > 0);
        
        result.HasValue.Must().BeFalse();
    }
    
    [Test]
    public void ReadOnlySpan_AverageOrNone_WithPredicate_ShouldReturnSome()
    {
        ReadOnlySpan<int> span = stackalloc int[] { 1, 2, 3, 4, 5, 6 };
        
        var result = span.AverageOrNone(x => x % 2 == 0); // Even: 2, 4, 6
        
        result.HasValue.Must().BeTrue();
        result.Value.Must().BeEqualTo(4); // (2+4+6)/3 = 4
    }
    
    [Test]
    public void Double_AverageOrNone_ShouldWork()
    {
        var array = new double[] { 2.0, 4.0, 6.0 };
        
        var result = array.AverageOrNone();
        
        result.HasValue.Must().BeTrue();
        result.Value.Must().BeEqualTo(4.0); // (2+4+6)/3 = 4.0
    }
}
