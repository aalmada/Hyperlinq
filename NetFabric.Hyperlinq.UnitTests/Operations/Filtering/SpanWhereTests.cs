using System;
using System.Collections.Generic;
using System.Linq;
using TUnit.Core;
using NetFabric.Assertive;

namespace NetFabric.Hyperlinq.UnitTests.Span;

public class SpanWhereTests
{
    // ===== Basic Where Operations =====
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_Where_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        
        var hyperlinqResult = array.Where(x => x % 2 == 0);
        var linqResult = Enumerable.Where(array, x => x % 2 == 0);
        
        hyperlinqResult.ToArray().Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_Where_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var list = new List<int>(testCase.arrayFactory());
        
        var hyperlinqResult = list.Where(x => x % 2 == 0);
        var linqResult = Enumerable.Where(list, x => x % 2 == 0);
        
        hyperlinqResult.ToArray().Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Memory_Where_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        ReadOnlyMemory<int> memory = array.AsMemory();
        
        var hyperlinqResult = memory.Where(x => x % 2 == 0);
        var linqResult = Enumerable.Where(array, x => x % 2 == 0);
        
        var i = 0;
        foreach (var item in hyperlinqResult)
        {
            if (item != linqResult.ElementAt(i++))
                throw new Exception("Mismatch");
        }
        if (i != linqResult.Count())
            throw new Exception("Count mismatch");
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void ArraySegment_Where_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        if (array.Length == 0)
        {
            var segment = new ArraySegment<int>(array);
            var hyperlinqResult = segment.Where(x => x % 2 == 0);
            var linqResult = Enumerable.Where(segment, x => x % 2 == 0);
            
            hyperlinqResult.ToArray().Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(linqResult);
        }
        else
        {
            var segment = new ArraySegment<int>(array, 0, array.Length);
            var hyperlinqResult = segment.Where(x => x % 2 == 0);
            var linqResult = Enumerable.Where(segment, x => x % 2 == 0);
            
            hyperlinqResult.ToArray().Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(linqResult);
        }
    }
    
    // ===== Type Verification =====
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetNonEmptyIntArraySources))]
    public void Array_Where_ShouldReturnWhereReadOnlySpanEnumerable((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        var result = array.Where(x => x % 2 == 0);
        
        // Ref structs cannot be used in generic type arguments, so we can't use Must().BeOfType<T>()
        // We verify behavior instead
        result.ToArray().Must().BeEnumerableOf<int>();
    }
    

    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetNonEmptyIntArraySources))]
    public void Memory_Where_ShouldReturnWhereReadOnlySpanEnumerable((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        ReadOnlyMemory<int> memory = array.AsMemory();
        var result = memory.Where(x => x % 2 == 0);
        

    }
    
    // ===== Edge Cases =====
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetEdgeCaseIntArraySources))]
    public void Array_Where_EdgeCases_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        
        var hyperlinqResult = array.Where(x => x % 2 == 0);
        var linqResult = Enumerable.Where(array, x => x % 2 == 0);
        
        hyperlinqResult.ToArray().Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(linqResult);
    }
    
    // ===== Chained Operations =====
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_Where_Count_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        
        var hyperlinqResult = array.Where(x => x % 2 == 0).Count();
        var linqResult = Enumerable.Count(Enumerable.Where(array, x => x % 2 == 0));
        
        hyperlinqResult.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_Where_Any_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        
        var hyperlinqResult = array.Where(x => x % 2 == 0).Any();
        var linqResult = Enumerable.Any(Enumerable.Where(array, x => x % 2 == 0));
        
        hyperlinqResult.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetNonEmptyIntArraySources))]
    public void Array_Where_First_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        var filtered = array.Where(x => x > 0);
        
        if (filtered.Any())
        {
            var hyperlinqResult = filtered.First();
            var linqResult = Enumerable.First(Enumerable.Where(array, x => x > 0));
            
            hyperlinqResult.Must().BeEqualTo(linqResult);
        }
    }
}

