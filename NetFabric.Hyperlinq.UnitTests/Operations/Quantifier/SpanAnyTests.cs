using System;
using System.Collections.Generic;
using System.Linq;
using TUnit.Core;
using NetFabric.Assertive;

namespace NetFabric.Hyperlinq.UnitTests.Span;

public class SpanAnyTests
{
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_Any_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        
        var hyperlinqResult = array.AsSpan().Any();
        var linqResult = Enumerable.Any(array);
        
        hyperlinqResult.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_Any_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var list = new List<int>(testCase.arrayFactory());
        
        var hyperlinqResult = list.Any();
        var linqResult = Enumerable.Any(list);
        
        hyperlinqResult.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetNonEmptyIntArraySources))]
    public void List_Where_Any_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var list = new List<int>(testCase.arrayFactory());
        
        var hyperlinqResult = list.AsValueEnumerable().Where(x => x % 2 == 0).Any();
        var linqResult = list.AsValueEnumerable().Where(x => x % 2 == 0).Any();
        
        hyperlinqResult.Must().BeEqualTo(linqResult);
    }
}
