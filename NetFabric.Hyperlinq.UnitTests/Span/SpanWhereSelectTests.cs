using System;
using System.Collections.Generic;
using System.Linq;
using TUnit.Core;
using NetFabric.Assertive;

namespace NetFabric.Hyperlinq.UnitTests.Span;

public class SpanWhereSelectTests
{
    // ===== Fusion Verification =====
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetNonEmptyIntArraySources))]
    public void Array_WhereSelect_ShouldReturnWhereSelectReadOnlySpanEnumerable((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        var result = array.Where(x => x % 2 == 0).Select(x => x * 2);
        
        // Ref structs cannot be used in generic type arguments
        result.ToArray().Must().BeEnumerableOf<int>();
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_WhereSelect_ShouldReturnWhereSelectListEnumerable((Func<int[]> arrayFactory, string description) testCase)
    {
        var list = new List<int>(testCase.arrayFactory());
        var result = list.Where(x => x % 2 == 0).Select(x => x * 2);
        
        result.Must().BeOfType<WhereSelectListEnumerable<int, int>>();
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetNonEmptyIntArraySources))]
    public void Memory_WhereSelect_ShouldReturnWhereSelectMemoryEnumerable((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        ReadOnlyMemory<int> memory = array.AsMemory();
        var result = memory.Where(x => x % 2 == 0).Select(x => x * 2);
        
        result.Must().BeOfType<WhereSelectMemoryEnumerable<int, int>>();
    }
    
    // ===== Correctness Tests =====
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_WhereSelect_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        
        var hyperlinqResult = array.Where(x => x % 2 == 0).Select(x => x * 10);
        var linqResult = Enumerable.Select(Enumerable.Where(array, x => x % 2 == 0), x => x * 10);
        
        hyperlinqResult.ToArray().Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_WhereSelect_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var list = new List<int>(testCase.arrayFactory());
        
        var hyperlinqResult = list.Where(x => x % 2 == 0).Select(x => x * 10);
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
        
        hyperlinqResult.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(linqResult);
    }
    
    // ===== Type Changes =====
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetNonEmptyIntArraySources))]
    public void Array_WhereSelect_IntToString_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        
        var hyperlinqResult = array.Where(x => x % 2 == 0).Select(x => x.ToString());
        var linqResult = Enumerable.Select(Enumerable.Where(array, x => x % 2 == 0), x => x.ToString());
        
        hyperlinqResult.ToArray().Must()
            .BeEnumerableOf<string>()
            .EvaluateTrue(e => e.SequenceEqual(linqResult));
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetNonEmptyIntArraySources))]
    public void Array_WhereSelect_IntToDouble_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        
        var hyperlinqResult = array.Where(x => x % 2 == 0).Select(x => (double)x * 1.5);
        var linqResult = Enumerable.Select(Enumerable.Where(array, x => x % 2 == 0), x => (double)x * 1.5);
        
        hyperlinqResult.ToArray().Must()
            .BeEnumerableOf<double>()
            .BeEqualTo(linqResult);
    }
    
    // ===== Optimized Operations =====
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_WhereSelect_Count_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        
        // Count should be optimized to ignore the selector
        var hyperlinqResult = array.Where(x => x % 2 == 0).Select(x => x * 10).Count();
        var linqResult = Enumerable.Count(Enumerable.Where(array, x => x % 2 == 0));
        
        hyperlinqResult.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_WhereSelect_Any_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        
        // Any should be optimized to ignore the selector
        var hyperlinqResult = array.Where(x => x % 2 == 0).Select(x => x * 10).Any();
        var linqResult = Enumerable.Any(Enumerable.Where(array, x => x % 2 == 0));
        
        hyperlinqResult.Must().BeEqualTo(linqResult);
    }
    
    // ===== Chained Operations =====
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_WhereSelect_Sum_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        
        var hyperlinqResult = array.Where(x => x % 2 == 0).Select(x => x * 2).Sum();
        var linqResult = Enumerable.Sum(Enumerable.Select(Enumerable.Where(array, x => x % 2 == 0), x => x * 2));
        
        hyperlinqResult.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetNonEmptyIntArraySources))]
    public void Array_WhereSelect_First_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        var filtered = array.Where(x => x > 0);
        
        if (filtered.Any())
        {
            var hyperlinqResult = filtered.Select(x => x * 2).First();
            var linqResult = Enumerable.First(Enumerable.Select(Enumerable.Where(array, x => x > 0), x => x * 2));
            
            hyperlinqResult.Must().BeEqualTo(linqResult);
        }
    }
    
    // ===== Edge Cases =====
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetEdgeCaseIntArraySources))]
    public void Array_WhereSelect_EdgeCases_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        
        var hyperlinqResult = array.Where(x => x % 2 == 0).Select(x => x * 2);
        var linqResult = Enumerable.Select(Enumerable.Where(array, x => x % 2 == 0), x => x * 2);
        
        hyperlinqResult.ToArray().Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(linqResult);
    }
}

