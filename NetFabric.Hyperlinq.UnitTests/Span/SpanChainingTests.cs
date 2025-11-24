using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests.Span;

public class SpanChainingTests
{
    [Test]
    public async Task Array_Where_Sum_ShouldWork()
    {
        var array = new int[] { 1, 2, 3, 4, 5, 6 };
        
        var result = array.Where(x => x % 2 == 0).Sum();
        var expected = Enumerable.Where(array, x => x % 2 == 0).Sum();
        
        await Assert.That(result).IsEqualTo(expected);
        await Assert.That(result).IsEqualTo(12);  // 2 + 4 + 6
    }
    
    [Test]
    public async Task List_Where_Sum_ShouldWork()
    {
        var list = new List<int> { 1, 2, 3, 4, 5, 6 };
        
        var result = list.Where(x => x % 2 == 0).Sum();
        var expected = Enumerable.Where(list, x => x % 2 == 0).Sum();
        
        await Assert.That(result).IsEqualTo(expected);
        await Assert.That(result).IsEqualTo(12);
    }
    
    [Test]
    public async Task Memory_Where_Sum_ShouldWork()
    {
        var array = new int[] { 1, 2, 3, 4, 5, 6 };
        ReadOnlyMemory<int> memory = array.AsMemory();
        
        var result = memory.Where(x => x % 2 == 0).Sum();
        
        await Assert.That(result).IsEqualTo(12);
    }
    
    [Test]
    public async Task Array_WhereSelect_Sum_ShouldWork()
    {
        var array = new int[] { 1, 2, 3, 4, 5 };
        
        var result = array
            .Where(x => x % 2 == 0)
            .Select(x => x * 2)
            .Sum();
        
        var expected = array
            .Where(x => x % 2 == 0)
            .Select(x => x * 2)
            .Sum();
        
        await Assert.That(result).IsEqualTo(expected);
        await Assert.That(result).IsEqualTo(12);  // (2*2) + (4*2) = 4 + 8
    }
    
    [Test]
    public async Task List_WhereSelect_Sum_ShouldWork()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };
        
        var result = list
            .Where(x => x % 2 == 0)
            .Select(x => x * 2)
            .Sum();
        
        await Assert.That(result).IsEqualTo(12);
    }
    
    [Test]
    public async Task ArraySegment_Where_Sum_ShouldWork()
    {
        var array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
        var segment = new ArraySegment<int>(array, 2, 4);  // {3, 4, 5, 6}
        
        var result = segment.Where(x => x % 2 == 0).Sum();
        
        await Assert.That(result).IsEqualTo(10);  // 4 + 6
    }
    
    [Test]
    public async Task EmptyArray_Where_Sum_ShouldReturnZero()
    {
        var array = Array.Empty<int>();
        
        var result = array.Where(x => x % 2 == 0).Sum();
        
        await Assert.That(result).IsEqualTo(0);
    }
    
    [Test]
    public async Task Array_Where_NoMatches_Sum_ShouldReturnZero()
    {
        var array = new int[] { 1, 3, 5, 7, 9 };
        
        var result = array.Where(x => x % 2 == 0).Sum();
        
        await Assert.That(result).IsEqualTo(0);
    }
}
