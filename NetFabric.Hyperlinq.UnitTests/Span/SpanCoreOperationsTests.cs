using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TUnit.Core;
using NetFabric.Assertive;

namespace NetFabric.Hyperlinq.UnitTests.Span;

public class SpanCoreOperationsTests
{
    // ===== Data Sources =====
    
    public static IEnumerable<(Func<int[]> arrayFactory, string description)> GetIntArraySources()
    {
        yield return (() => new int[] { 1, 2, 3, 4, 5 }, "Array with 5 elements");
        yield return (() => new int[] { 10, 20, 30 }, "Array with 3 elements");
        yield return (() => Array.Empty<int>(), "Empty array");
        yield return (() => new int[] { 42 }, "Single element");
    }
    
    public static IEnumerable<(Func<List<int>> listFactory, string description)> GetIntListSources()
    {
        yield return (() => new List<int> { 1, 2, 3, 4, 5 }, "List with 5 elements");
        yield return (() => new List<int> { 10, 20, 30 }, "List with 3 elements");
        yield return (() => new List<int>(), "Empty list");
        yield return (() => new List<int> { 99 }, "Single element");
    }
    
    public static IEnumerable<(Func<int[]> arrayFactory, string description)> GetNonEmptyIntArraySources()
    {
        yield return (() => new int[] { 1, 2, 3, 4, 5 }, "Array with 5 elements");
        yield return (() => new int[] { 10, 20, 30 }, "Array with 3 elements");
        yield return (() => new int[] { 42 }, "Single element");
    }
    
    // ===== Count Tests =====
    
    [Test]
    [MethodDataSource(nameof(GetIntArraySources))]
    public void Array_Count_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        
        var hyperlinqResult = array.Count();
        var linqResult = Enumerable.Count(array);
        
        hyperlinqResult.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(nameof(GetIntListSources))]
    public void List_Count_ShouldMatchLinq((Func<List<int>> listFactory, string description) testCase)
    {
        var list = testCase.listFactory();
        
        var hyperlinqResult = list.Count();
        var linqResult = Enumerable.Count(list);
        
        hyperlinqResult.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    public void Memory_Count_ShouldWork()
    {
        ReadOnlyMemory<int> memory = new int[] { 1, 2, 3 }.AsMemory();
        memory.Count().Must().BeEqualTo(3);
    }
    
    // ===== Any Tests =====
    
    [Test]
    [MethodDataSource(nameof(GetIntArraySources))]
    public void Array_Any_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        
        var hyperlinqResult = array.Any();
        var linqResult = Enumerable.Any(array);
        
        hyperlinqResult.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(nameof(GetIntListSources))]
    public void List_Any_ShouldMatchLinq((Func<List<int>> listFactory, string description) testCase)
    {
        var list = testCase.listFactory();
        
        var hyperlinqResult = list.Any();
        var linqResult = Enumerable.Any(list);
        
        hyperlinqResult.Must().BeEqualTo(linqResult);
    }
    
    // ===== First Tests =====
    
    [Test]
    [MethodDataSource(nameof(GetNonEmptyIntArraySources))]
    public void Array_First_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        
        var hyperlinqResult = array.First();
        var linqResult = Enumerable.First(array);
        
        hyperlinqResult.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(nameof(GetNonEmptyIntArraySources))]
    public void List_First_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var list = new List<int>(testCase.arrayFactory());
        
        var hyperlinqResult = list.First();
        var linqResult = Enumerable.First(list);
        
        hyperlinqResult.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    public void Memory_First_ShouldWork()
    {
        ReadOnlyMemory<int> memory = new int[] { 100, 200 }.AsMemory();
        memory.First().Must().BeEqualTo(100);
    }
    
    [Test]
    public void Array_First_Empty_ShouldThrow()
    {
        var array = Array.Empty<int>();
        Action action = () => array.First();
        action.Must().Throw<InvalidOperationException>();
    }
    
    // ===== Last Tests =====
    
    [Test]
    [MethodDataSource(nameof(GetNonEmptyIntArraySources))]
    public void Array_Last_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        
        var hyperlinqResult = array.Last();
        var linqResult = Enumerable.Last(array);
        
        hyperlinqResult.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(nameof(GetNonEmptyIntArraySources))]
    public void List_Last_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var list = new List<int>(testCase.arrayFactory());
        
        var hyperlinqResult = list.Last();
        var linqResult = Enumerable.Last(list);
        
        hyperlinqResult.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    public void Memory_Last_ShouldWork()
    {
        ReadOnlyMemory<int> memory = new int[] { 100, 200, 300 }.AsMemory();
        memory.Last().Must().BeEqualTo(300);
    }
    
    [Test]
    public void Array_Last_Empty_ShouldThrow()
    {
        var array = Array.Empty<int>();
        Action action = () => array.Last();
        action.Must().Throw<InvalidOperationException>();
    }
    
    // ===== Combined Operations =====
    
    [Test]
    [MethodDataSource(nameof(GetIntArraySources))]
    public void Array_Where_Count_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        
        var hyperlinqResult = array.Where(x => x % 2 == 0).Count();
        var linqResult = array.Where(x => x % 2 == 0).Count();
        
        hyperlinqResult.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(nameof(GetIntListSources))]
    public void List_Where_Any_ShouldMatchLinq((Func<List<int>> listFactory, string description) testCase)
    {
        var list = testCase.listFactory();
        
        var hyperlinqResult = list.Where(x => x % 2 == 0).Any();
        var linqResult = list.Where(x => x % 2 == 0).Any();
        
        hyperlinqResult.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    public void Array_Where_First_ShouldWork()
    {
        var array = new int[] { 1, 2, 3, 4, 5 };
        var result = array.Where(x => x > 3).First();
        result.Must().BeEqualTo(4);
    }
    
    [Test]
    public void Array_Where_Last_ShouldWork()
    {
        var array = new int[] { 1, 2, 3, 4, 5 };
        var result = array.Where(x => x > 2).Last();
        result.Must().BeEqualTo(5);
    }
}
