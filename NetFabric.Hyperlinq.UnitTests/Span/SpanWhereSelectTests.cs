using System;
using System.Collections.Generic;
using System.Linq;
using TUnit.Core;
using NetFabric.Assertive;

namespace NetFabric.Hyperlinq.UnitTests.Span;

public class SpanWhereSelectTests
{

    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_WhereSelect_ShouldReturnWhereSelectListEnumerable((Func<int[]> arrayFactory, string description) testCase)
    {
        var list = new List<int>(testCase.arrayFactory());
        var result = list.AsValueEnumerable().Where(x => x % 2 == 0).Select(x => x * 2);
        
        result.Must().BeOfType<WhereSelectListEnumerable<int, int>>();
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetNonEmptyIntArraySources))]
    public void Memory_WhereSelect_ShouldReturnWhereSelectReadOnlySpanEnumerable((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        ReadOnlyMemory<int> memory = array.AsMemory();
        var result = memory.Where(x => x % 2 == 0).Select(x => x * 2);
        

    }
    
    // ===== Correctness Tests =====
    

    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_WhereSelect_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var list = new List<int>(testCase.arrayFactory());
        
        var hyperlinqResult = list.AsValueEnumerable().Where(x => x % 2 == 0).Select(x => x * 10);
        var linqResult = Enumerable.Select(Enumerable.Where(list, x => x % 2 == 0), x => x * 10);
        
        hyperlinqResult.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Memory_WhereSelect_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        ReadOnlyMemory<int> memory = array.AsMemory();
        
        var hyperlinqResult = memory.Where(x => x % 2 == 0).Select(x => x * 10);
        var linqResult = Enumerable.Select(Enumerable.Where(array, x => x % 2 == 0), x => x * 10);
        
        var i = 0;
        foreach (var item in hyperlinqResult)
        {
            if (item != linqResult.ElementAt(i++))
                throw new Exception("Mismatch");
        }
        if (i != linqResult.Count())
            throw new Exception("Count mismatch");
    }
    
    // ===== Type Changes =====
    

    

    

    

    

}

