using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TUnit.Core;
using NetFabric.Assertive;
using NetFabric.Hyperlinq;

namespace NetFabric.Hyperlinq.UnitTests.Span;

public class SpanSumTests
{
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_Sum_ShouldMatchLinq(TestCase<int[]> testCase)
    {
        var array = testCase.Factory();
        
        var spanResult = array.Sum();  // SpanExtensions.Sum
        var linqResult = Enumerable.Sum(array);
        
        spanResult.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_Sum_ShouldMatchLinq(TestCase<int[]> testCase)
    {
        var list = new List<int>(testCase.Factory());
        
        var spanResult = list.Sum();  // SpanExtensions.Sum
        var linqResult = Enumerable.Sum(list);
        
        spanResult.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void ReadOnlyMemory_Sum_ShouldMatchLinq(TestCase<int[]> testCase)
    {
        var array = testCase.Factory();
        ReadOnlyMemory<int> memory = array.AsMemory();
        
        var result = memory.Sum();
        var linqResult = Enumerable.Sum(array);
        
        result.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Memory_Sum_ShouldMatchLinq(TestCase<int[]> testCase)
    {
        var array = testCase.Factory();
        Memory<int> memory = array.AsMemory();
        
        var result = memory.Span.Sum();
        var linqResult = Enumerable.Sum(array);
        
        result.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void ArraySegment_Sum_ShouldMatchLinq(TestCase<int[]> testCase)
    {
        var array = testCase.Factory();
        var segment = new ArraySegment<int>(array);
        
        var result = segment.Sum();
        var linqResult = Enumerable.Sum(array);
        
        result.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    public void ReadOnlySpan_Sum_ShouldWork()
    {
        ReadOnlySpan<int> span = stackalloc int[] { 1, 2, 3, 4, 5 };
        
        var result = span.Sum();
        
        result.Must().BeEqualTo(15);
    }
    
    [Test]
    public void Span_Sum_ShouldWork()
    {
        Span<int> span = stackalloc int[] { 1, 2, 3, 4, 5 };
        
        var result = span.Sum();
        
        result.Must().BeEqualTo(15);
    }
    

    
    [Test]
    public async Task Double_Sum_ShouldWork()
    {
        var array = new double[] { 1.5, 2.5, 3.5 };
        
        var result = array.Sum();
        
        await Assert.That(Math.Abs(result - 7.5)).IsLessThan(1e-10);
    }
    
    [Test]
    public void Long_Sum_ShouldWork()
    {
        var memory = new long[] { 1L, 2L, 3L, 4L, 5L }.AsMemory();
        var result = memory.Span.Sum();
        
        result.Must().BeEqualTo(15L);
    }
}
