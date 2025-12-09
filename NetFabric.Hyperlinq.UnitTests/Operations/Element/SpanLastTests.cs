using System;
using System.Collections.Generic;
using System.Linq;
using TUnit.Core;
using NetFabric.Assertive;

namespace NetFabric.Hyperlinq.UnitTests.Span;

public class SpanLastTests
{
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetNonEmptyIntArraySources))]
    public void Array_Last_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        
        var hyperlinqResult = array.Last();
        var linqResult = Enumerable.Last(array);
        
        hyperlinqResult.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetNonEmptyIntArraySources))]
    public void List_Last_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var list = new List<int>(testCase.arrayFactory());
        
        var hyperlinqResult = list.Last();
        var linqResult = Enumerable.Last(list);
        
        hyperlinqResult.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetNonEmptyIntArraySources))]
    public void Memory_Last_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        ReadOnlyMemory<int> memory = array.AsMemory();
        
        var hyperlinqResult = memory.Last();
        var linqResult = Enumerable.Last(array);
        
        hyperlinqResult.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    public void Array_Last_Empty_ShouldThrow()
    {
        var array = Array.Empty<int>();
        Action action = () => array.Last();
        action.Must().Throw<InvalidOperationException>();
    }
    
    [Test]
    public void Array_Where_Last_ShouldWork()
    {
        var array = new int[] { 1, 2, 3, 4, 5 };
        var result = array.Where(x => x > 2).Last();
        result.Must().BeEqualTo(5);
    }
}
