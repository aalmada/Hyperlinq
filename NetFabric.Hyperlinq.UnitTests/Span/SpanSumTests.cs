using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TUnit.Core;
using NetFabric.Assertive;

namespace NetFabric.Hyperlinq.UnitTests.Span;

public class SpanSumTests
{
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_Sum_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        
        var spanResult = array.Sum();  // SpanExtensions.Sum
        var linqResult = Enumerable.Sum(array);
        
        spanResult.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_Sum_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var list = new List<int>(testCase.arrayFactory());
        
        var spanResult = list.Sum();  // SpanExtensions.Sum
        var linqResult = Enumerable.Sum(list);
        
        spanResult.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void ReadOnlyMemory_Sum_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        ReadOnlyMemory<int> memory = array.AsMemory();
        
        var result = memory.Sum();
        var linqResult = Enumerable.Sum(array);
        
        result.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Memory_Sum_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        Memory<int> memory = array.AsMemory();
        
        var result = memory.Sum();
        var linqResult = Enumerable.Sum(array);
        
        result.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void ArraySegment_Sum_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
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
        var array = new long[] { 1L, 2L, 3L, 4L, 5L };
        
        var result = array.Sum();
        
        result.Must().BeEqualTo(15L);
    }
}
