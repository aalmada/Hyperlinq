using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests.Span;

public class SpanSumTests
{
    [Test]
    public async Task Array_Sum_ShouldMatchLinq()
    {
        var array = new int[] { 1, 2, 3, 4, 5 };
        
        var spanResult = array.Sum();  // SpanExtensions.Sum
        var linqResult = Enumerable.Sum(array);
        
        await Assert.That(spanResult).IsEqualTo(linqResult);
    }
    
    [Test]
    public async Task List_Sum_ShouldMatchLinq()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };
        
        var spanResult = list.Sum();  // SpanExtensions.Sum
        var linqResult = Enumerable.Sum(list);
        
        await Assert.That(spanResult).IsEqualTo(linqResult);
    }
    
    [Test]
    public async Task ReadOnlyMemory_Sum_ShouldWork()
    {
        var array = new int[] { 1, 2, 3, 4, 5 };
        ReadOnlyMemory<int> memory = array.AsMemory();
        
        var result = memory.Sum();
        
        await Assert.That(result).IsEqualTo(15);
    }
    
    [Test]
    public async Task Memory_Sum_ShouldWork()
    {
        var array = new int[] { 1, 2, 3, 4, 5 };
        Memory<int> memory = array.AsMemory();
        
        var result = memory.Sum();
        
        await Assert.That(result).IsEqualTo(15);
    }
    
    [Test]
    public async Task ArraySegment_Sum_ShouldWork()
    {
        var array = new int[] { 1, 2, 3, 4, 5 };
        var segment = new ArraySegment<int>(array, 1, 3);  // {2, 3, 4}
        
        var result = segment.Sum();
        
        await Assert.That(result).IsEqualTo(9);
    }
    
    [Test]
    public async Task ReadOnlySpan_Sum_ShouldWork()
    {
        ReadOnlySpan<int> span = stackalloc int[] { 1, 2, 3, 4, 5 };
        
        var result = span.Sum();
        
        await Assert.That(result).IsEqualTo(15);
    }
    
    [Test]
    public async Task Span_Sum_ShouldWork()
    {
        Span<int> span = stackalloc int[] { 1, 2, 3, 4, 5 };
        
        var result = span.Sum();
        
        await Assert.That(result).IsEqualTo(15);
    }
    
    [Test]
    public async Task Empty_Array_Sum_ShouldReturnZero()
    {
        var array = Array.Empty<int>();
        
        var result = array.Sum();
        
        await Assert.That(result).IsEqualTo(0);
    }
    
    [Test]
    public async Task Double_Sum_ShouldWork()
    {
        var array = new double[] { 1.5, 2.5, 3.5 };
        
        var result = array.Sum();
        
        await Assert.That(Math.Abs(result - 7.5)).IsLessThan(1e-10);
    }
    
    [Test]
    public async Task Long_Sum_ShouldWork()
    {
        var array = new long[] { 1L, 2L, 3L, 4L, 5L };
        
        var result = array.Sum();
        
        await Assert.That(result).IsEqualTo(15L);
    }
}
