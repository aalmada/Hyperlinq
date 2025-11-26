using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TUnit.Core;
using NetFabric.Assertive;

namespace NetFabric.Hyperlinq.UnitTests;

public class AsValueEnumerableTests
{
    // ===== List<T> Tests =====
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_AsValueEnumerable_ShouldReturnListValueEnumerable((Func<int[]> arrayFactory, string description) testCase)
    {
        var list = new List<int>(testCase.arrayFactory());
        var valueEnum = list.AsValueEnumerable();
        valueEnum.Must().BeOfType<ListValueEnumerable<int>>();
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_AsValueEnumerable_ShouldBeEnumerableOf((Func<int[]> arrayFactory, string description) testCase)
    {
        var list = new List<int>(testCase.arrayFactory());
        var valueEnum = list.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(list);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_AsValueEnumerable_Count_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var list = new List<int>(testCase.arrayFactory());
        var valueEnum = list.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(list)
            .EvaluateTrue(e => e.Count() == list.Count());
    }
    
    [Test]
    public void List_AsValueEnumerable_Indexer_ShouldWork()
    {
        var list = new List<int> { 10, 20, 30, 40, 50 };
        var valueEnum = list.AsValueEnumerable();
        
        // Test indexer access
        valueEnum[0].Must().BeEqualTo(10);
        valueEnum[2].Must().BeEqualTo(30);
        valueEnum[4].Must().BeEqualTo(50);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_AsValueEnumerable_Any_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var list = new List<int>(testCase.arrayFactory());
        var valueEnum = list.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(list)
            .EvaluateTrue(e => e.Any() == list.Any());
    }
    

    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetNonEmptyIntArraySources))]
    public void List_AsValueEnumerable_First_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var list = new List<int>(testCase.arrayFactory());
        var valueEnum = list.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(list)
            .EvaluateTrue(e => e.First() == list.First());
    }
    
    [Test]
    public void List_AsValueEnumerable_Single_ShouldMatchLinq()
    {
        var list = new List<int> { 42 };
        var valueEnum = list.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(list)
            .EvaluateTrue(e => e.Single() == list.Single());
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_AsValueEnumerable_Sum_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var list = new List<int>(testCase.arrayFactory());
        var valueEnum = list.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(list)
            .EvaluateTrue(e => e.Sum() == list.Sum());
    }
    
    [Test]
    public void List_AsValueEnumerable_Sum_Double_ShouldMatchLinq()
    {
        var list = new List<double> { 1.5, 2.5, 3.5 };
        var valueEnum = list.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<double>()
            .BeEqualTo(list)
            .EvaluateTrue(e => Math.Abs(e.Sum() - list.Sum()) < 1e-10);
    }
    
    // ===== Array Tests =====
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_AsValueEnumerable_ShouldReturnArrayValueEnumerable((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        var valueEnum = array.AsValueEnumerable();
        valueEnum.Must().BeOfType<ArrayValueEnumerable<int>>();
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_AsValueEnumerable_ShouldBeEnumerableOf((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        var valueEnum = array.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(array);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_AsValueEnumerable_Count_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        var valueEnum = array.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(array)
            .EvaluateTrue(e => e.Count() == array.Count());
    }
    
    [Test]
    public void Array_AsValueEnumerable_Indexer_ShouldWork()
    {
        var array = new int[] { 10, 20, 30, 40, 50 };
        var valueEnum = array.AsValueEnumerable();
        
        valueEnum[0].Must().BeEqualTo(10);
        valueEnum[2].Must().BeEqualTo(30);
        valueEnum[4].Must().BeEqualTo(50);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_AsValueEnumerable_Sum_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        var valueEnum = array.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(array)
            .EvaluateTrue(e => e.Sum() == array.Sum());
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_AsValueEnumerable_Any_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        var valueEnum = array.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(array)
            .EvaluateTrue(e => e.Any() == array.Any());
    }
    
    // ===== Chained Operations Tests =====
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_WhereSelect_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var list = new List<int>(testCase.arrayFactory());
        
        var hyperlinqResult = list.AsValueEnumerable()
                                  .Where(x => x % 2 == 0)
                                  .Select(x => x * 10);
        
        var linqResult = list.Where(x => x % 2 == 0)
                            .Select(x => x * 10);
        
        hyperlinqResult.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_WhereSelectSum_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var list = new List<int>(testCase.arrayFactory());
        
        var hyperlinqResult = list.AsValueEnumerable()
                                  .Where(x => x % 2 == 0)
                                  .Select(x => x * 10)
                                  .Sum();
        
        var linqResult = list.Where(x => x % 2 == 0)
                            .Select(x => x * 10)
                            .Sum();
        
        hyperlinqResult.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_WhereSelectCount_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        
        var hyperlinqResult = array.AsValueEnumerable()
                                   .Where(x => x > 5)
                                   .Select(x => x * 2);
        
        var linqResult = array.Where(x => x > 5)
                             .Select(x => x * 2);
        
        hyperlinqResult.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(linqResult)
            .EvaluateTrue(e => e.Count() == linqResult.Count());
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_MultipleWhere_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var list = new List<int>(testCase.arrayFactory());
        
        var hyperlinqResult = list.AsValueEnumerable()
                                  .Where(x => x > 3)
                                  .Where(x => x % 2 == 0);
        
        var linqResult = list.Where(x => x > 3)
                            .Where(x => x % 2 == 0);
        
        hyperlinqResult.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(linqResult);
    }
    
    // ===== Edge Cases =====
    

    

    

    

    
    // ===== IEnumerable<T> Fallback Tests =====
    
    [Test]
    public void IEnumerable_AsValueEnumerable_ShouldReturnEnumerableValueEnumerable()
    {
        IEnumerable<int> enumerable = Enumerable.Range(1, 5);
        var valueEnum = enumerable.AsValueEnumerable();
        valueEnum.Must().BeOfType<EnumerableValueEnumerable<int>>();
    }
    
    [Test]
    public void IEnumerable_AsValueEnumerable_Sum_ShouldMatchLinq()
    {
        IEnumerable<int> enumerable = Enumerable.Range(1, 10);
        var valueEnum = enumerable.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(enumerable)
            .EvaluateTrue(e => e.Sum() == enumerable.Sum());
    }
    
    // ===== Type-Specific Tests =====
    
    [Test]
    public void List_Long_Sum_ShouldMatchLinq()
    {
        var list = new List<long> { 1L, 2L, 3L, 4L, 5L };
        var valueEnum = list.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<long>()
            .BeEqualTo(list)
            .EvaluateTrue(e => e.Sum() == list.Sum());
    }
    
    [Test]
    public void Array_Float_Sum_ShouldMatchLinq()
    {
        var array = new float[] { 1.5f, 2.5f, 3.5f };
        var valueEnum = array.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<float>()
            .BeEqualTo(array)
            .EvaluateTrue(e => Math.Abs(e.Sum() - array.Sum()) < 1e-6f);
    }
    
    // ===== Enumerator Reusability Tests =====
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_AsValueEnumerable_ShouldSupportMultipleEnumerations((Func<int[]> arrayFactory, string description) testCase)
    {
        var list = new List<int>(testCase.arrayFactory());
        var valueEnum = list.AsValueEnumerable();
        
        // NetFabric.Assertive will enumerate multiple times to verify behavior
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(list);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_AsValueEnumerable_ShouldSupportMultipleEnumerations((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        var valueEnum = array.AsValueEnumerable();
        
        // NetFabric.Assertive will enumerate multiple times to verify behavior
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(array);
    }
}
