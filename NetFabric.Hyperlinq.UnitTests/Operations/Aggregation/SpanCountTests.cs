using System;
using System.Collections.Generic;
using System.Linq;
using TUnit.Core;
using NetFabric.Assertive;

namespace NetFabric.Hyperlinq.UnitTests.Span;

public class SpanCountTests
{
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_Count_ShouldMatchLinq(TestCase<int[]> testCase)
    {
        var array = testCase.Factory();
        
        var hyperlinqResult = array.Count();
        var linqResult = Enumerable.Count(array);
        
        hyperlinqResult.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_Count_ShouldMatchLinq(TestCase<int[]> testCase)
    {
        var list = new List<int>(testCase.Factory());
        
        var hyperlinqResult = list.Count();
        var linqResult = Enumerable.Count(list);
        
        hyperlinqResult.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Memory_Count_ShouldMatchLinq(TestCase<int[]> testCase)
    {
        var array = testCase.Factory();
        ReadOnlyMemory<int> memory = array.AsMemory();
        
        var hyperlinqResult = memory.Span.Count();
        var linqResult = Enumerable.Count(array);
        
        hyperlinqResult.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_Where_Count_ShouldMatchLinq(TestCase<int[]> testCase)
    {
        var array = testCase.Factory();
        
        var hyperlinqResult = array.Where(x => x % 2 == 0).Count();
        var linqResult = array.Where(x => x % 2 == 0).Count();
        
        hyperlinqResult.Must().BeEqualTo(linqResult);
    }
}
