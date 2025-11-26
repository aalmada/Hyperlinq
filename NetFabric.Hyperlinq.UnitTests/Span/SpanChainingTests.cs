using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TUnit.Core;
using NetFabric.Assertive;

namespace NetFabric.Hyperlinq.UnitTests.Span;

public class SpanChainingTests
{
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_Where_Sum_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        
        var result = array.Where(x => x % 2 == 0).Sum();
        var expected = Enumerable.Where(array, x => x % 2 == 0).Sum();
        
        result.Must().BeEqualTo(expected);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_Where_Sum_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var list = new List<int>(testCase.arrayFactory());
        
        var result = list.Where(x => x % 2 == 0).Sum();
        var expected = Enumerable.Where(list, x => x % 2 == 0).Sum();
        
        result.Must().BeEqualTo(expected);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Memory_Where_Sum_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        ReadOnlyMemory<int> memory = array.AsMemory();
        
        var result = memory.Where(x => x % 2 == 0).Sum();
        var expected = Enumerable.Where(array, x => x % 2 == 0).Sum();
        
        result.Must().BeEqualTo(expected);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_WhereSelect_Sum_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        
        var result = array
            .Where(x => x % 2 == 0)
            .Select(x => x * 2)
            .Sum();
        
        var expected = array
            .Where(x => x % 2 == 0)
            .Select(x => x * 2)
            .Sum();
        
        result.Must().BeEqualTo(expected);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_WhereSelect_Sum_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var list = new List<int>(testCase.arrayFactory());
        
        var result = list
            .Where(x => x % 2 == 0)
            .Select(x => x * 2)
            .Sum();
            
        var expected = list
            .Where(x => x % 2 == 0)
            .Select(x => x * 2)
            .Sum();
        
        result.Must().BeEqualTo(expected);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void ArraySegment_Where_Sum_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        var segment = new ArraySegment<int>(array);
        
        var result = segment.Where(x => x % 2 == 0).Sum();
        var expected = Enumerable.Where(array, x => x % 2 == 0).Sum();
        
        result.Must().BeEqualTo(expected);
    }
    

}
