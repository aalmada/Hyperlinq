using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TUnit.Core;
using NetFabric.Assertive;

namespace NetFabric.Hyperlinq.UnitTests.Span;

public class SpanSelectTests
{
    // ===== Data Sources =====
    
    public static IEnumerable<(Func<int[]> arrayFactory, string description)> GetIntArraySources()
    {
        yield return (() => new int[] { 1, 2, 3, 4, 5 }, "Array with 5 elements");
        yield return (() => new int[] { 10, 20, 30 }, "Array with 3 elements");
        yield return (() => Array.Empty<int>(), "Empty array");
        yield return (() => new int[] { 42 }, "Single element");
        yield return (() => Enumerable.Range(1, 100).ToArray(), "Large array (100 elements)");
    }
    
    public static IEnumerable<(Func<List<int>> listFactory, string description)> GetIntListSources()
    {
        yield return (() => new List<int> { 1, 2, 3, 4, 5 }, "List with 5 elements");
        yield return (() => new List<int> { 10, 20, 30 }, "List with 3 elements");
        yield return (() => new List<int>(), "Empty list");
        yield return (() => new List<int> { 99 }, "Single element");
        yield return (() => Enumerable.Range(1, 100).ToList(), "Large list (100 elements)");
    }
    
    // ===== Array Select Tests =====
    
    [Test]
    [MethodDataSource(nameof(GetIntArraySources))]
    public void Array_Select_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        
        var hyperlinqResult = array.Select(x => x * 2);
        var linqResult = array.Select(x => x * 2);
        
        hyperlinqResult.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(nameof(GetIntArraySources))]
    public void Array_Select_Count_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        var result = array.Select(x => x * 2);
        
        result.Count.Must().BeEqualTo(array.Length);
    }
    

    
    [Test]
    [MethodDataSource(nameof(GetIntArraySources))]
    public void Array_Select_TypeChange_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        
        var hyperlinqResult = array.Select(x => x.ToString());
        var linqResult = array.Select(x => x.ToString());
        
        hyperlinqResult.Must()
            .BeEnumerableOf<string>()
            .EvaluateTrue(e => e.SequenceEqual(linqResult));
    }
    
    // ===== List Select Tests =====
    
    [Test]
    [MethodDataSource(nameof(GetIntListSources))]
    public void List_Select_ShouldMatchLinq((Func<List<int>> listFactory, string description) testCase)
    {
        var list = testCase.listFactory();
        
        var hyperlinqResult = list.Select(x => x * 3);
        var linqResult = list.Select(x => x * 3);
        
        hyperlinqResult.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(nameof(GetIntListSources))]
    public void List_Select_Count_ShouldMatchLinq((Func<List<int>> listFactory, string description) testCase)
    {
        var list = testCase.listFactory();
        var result = list.Select(x => x / 10);
        
        result.Count.Must().BeEqualTo(list.Count);
    }
    

    
    // ===== Memory Select Tests =====
    
    [Test]
    [MethodDataSource(nameof(GetIntArraySources))]
    public void Memory_Select_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        ReadOnlyMemory<int> memory = array.AsMemory();
        
        var hyperlinqResult = memory.Select(x => x + 10);
        var linqResult = array.Select(x => x + 10);
        
        hyperlinqResult.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(linqResult);
    }
    
    // ===== WhereSelect Fusion Tests =====
    
    [Test]
    [MethodDataSource(nameof(GetIntArraySources))]
    public void Array_WhereSelect_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        
        var hyperlinqResult = array
            .Where(x => x % 2 == 0)
            .Select(x => x * 10);
        
        var linqResult = array
            .Where(x => x % 2 == 0)
            .Select(x => x * 10);
        
        hyperlinqResult.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(nameof(GetIntListSources))]
    public void List_WhereSelect_ShouldMatchLinq((Func<List<int>> listFactory, string description) testCase)
    {
        var list = testCase.listFactory();
        
        var hyperlinqResult = list
            .Where(x => x > 4)
            .Select(x => x * 2);
        
        var linqResult = list
            .Where(x => x > 4)
            .Select(x => x * 2);
        
        hyperlinqResult.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(nameof(GetIntArraySources))]
    public void Array_WhereSelect_Count_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        
        var hyperlinqResult = array
            .Where(x => x > 10)
            .Select(x => x * 2);
        
        var linqResult = array
            .Where(x => x > 10)
            .Select(x => x * 2);
        
        hyperlinqResult.Count().Must().BeEqualTo(linqResult.Count());
    }
    
    [Test]
    [MethodDataSource(nameof(GetIntArraySources))]
    public void Array_WhereSelect_Any_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        
        var hyperlinqResult = array
            .Where(x => x > 1000)
            .Select(x => x * 2);
        
        var linqResult = array
            .Where(x => x > 1000)
            .Select(x => x * 2);
        
        hyperlinqResult.Any().Must().BeEqualTo(linqResult.Any());
    }
    
    // ===== Type-Specific Tests =====
    
    [Test]
    public void Array_Select_Double_ShouldMatchLinq()
    {
        var array = new double[] { 1.5, 2.5, 3.5 };
        
        var hyperlinqResult = array.Select(x => x * 2.0);
        var linqResult = array.Select(x => x * 2.0);
        
        hyperlinqResult.Must()
            .BeEnumerableOf<double>()
            .BeEqualTo(linqResult);
    }
    
    [Test]
    public void List_Select_String_ShouldMatchLinq()
    {
        var list = new List<string> { "a", "b", "c" };
        
        var hyperlinqResult = list.Select(x => x.ToUpper());
        var linqResult = list.Select(x => x.ToUpper());
        
        hyperlinqResult.Must()
            .BeEnumerableOf<string>()
            .EvaluateTrue(e => e.SequenceEqual(linqResult));
    }
    
    // ===== Complex Projection Tests =====
    
    [Test]
    public void Array_Select_ComplexProjection_ShouldMatchLinq()
    {
        var array = new int[] { 1, 2, 3, 4, 5 };
        
        var hyperlinqResult = array.Select(x => new { Value = x, Squared = x * x });
        var linqResult = array.Select(x => new { Value = x, Squared = x * x });
        
        hyperlinqResult.Count().Must().BeEqualTo(linqResult.Count());
        
        hyperlinqResult.Must()
            .BeEnumerableOf<object>()
            .EvaluateTrue(e => e.Zip(linqResult).All(pair => 
                pair.First.Value == pair.Second.Value && 
                pair.First.Squared == pair.Second.Squared));
    }
}
