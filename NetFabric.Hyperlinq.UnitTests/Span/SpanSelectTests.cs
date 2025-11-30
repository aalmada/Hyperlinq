using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TUnit.Core;
using NetFabric.Assertive;

namespace NetFabric.Hyperlinq.UnitTests.Span;

public class SpanSelectTests
{
    // ===== Array Select Tests =====
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_Select_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        
        var hyperlinqResult = array.Select(x => x * 2);
        var linqResult = Enumerable.Select(array, x => x * 2);
        
        hyperlinqResult.ToArray().Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_Select_Count_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        var result = array.Select(x => x * 2);
        
        result.Count().Must().BeEqualTo(array.Length);
    }
    
    
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_Select_TypeChange_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        
        var hyperlinqResult = array.Select(x => x.ToString());
        var linqResult = Enumerable.Select(array, x => x.ToString());
        
        hyperlinqResult.ToArray().Must()
            .BeEnumerableOf<string>()
            .EvaluateTrue(e => e.SequenceEqual(linqResult));
    }
    
    // ===== List Select Tests =====
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_Select_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var list = new List<int>(testCase.arrayFactory());
        
        var hyperlinqResult = list.Select(x => x * 3);
        var linqResult = Enumerable.Select(list, x => x * 3);
        
        hyperlinqResult.ToArray().Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_Select_Count_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var list = new List<int>(testCase.arrayFactory());
        var result = list.Select(x => x / 10);
        
        result.Count().Must().BeEqualTo(list.Count);
    }
    

    
    // ===== Memory Select Tests =====
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Memory_Select_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        ReadOnlyMemory<int> memory = array.AsMemory();
        
        var hyperlinqResult = memory.Select(x => x + 10);
        var linqResult = Enumerable.Select(array, x => x + 10);
        
        hyperlinqResult.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(linqResult);
    }
    

    

    
    // ===== Type-Specific Tests =====
    
    [Test]
    public void Array_Select_Double_ShouldMatchLinq()
    {
        var array = new double[] { 1.5, 2.5, 3.5 };
        
        var hyperlinqResult = array.Select(x => x * 2.0);
        var linqResult = Enumerable.Select(array, x => x * 2.0);
        
        hyperlinqResult.ToArray().Must()
            .BeEnumerableOf<double>()
            .BeEqualTo(linqResult);
    }
    
    [Test]
    public void List_Select_String_ShouldMatchLinq()
    {
        var list = new List<string> { "a", "b", "c" };
        
        var hyperlinqResult = list.Select(x => x.ToUpper());
        var linqResult = Enumerable.Select(list, x => x.ToUpper());
        
        hyperlinqResult.ToArray().Must()
            .BeEnumerableOf<string>()
            .EvaluateTrue(e => e.SequenceEqual(linqResult));
    }
    
    // ===== Complex Projection Tests =====
    
    [Test]
    public void Array_Select_ComplexProjection_ShouldMatchLinq()
    {
        var array = new int[] { 1, 2, 3, 4, 5 };
        
        var hyperlinqResult = array.Select(x => new { Value = x, Squared = x * x });
        var linqResult = Enumerable.Select(array, x => new { Value = x, Squared = x * x });
        
        hyperlinqResult.Count().Must().BeEqualTo(linqResult.Count());
        
        hyperlinqResult.ToArray().Must()
            .BeEnumerableOf<object>()
            .EvaluateTrue(e => e.Zip(linqResult).All(pair => 
                pair.First.Value == pair.Second.Value && 
                pair.First.Squared == pair.Second.Squared));
    }
}
