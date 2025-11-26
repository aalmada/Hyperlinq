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
    public void List_AsValueEnumerable_ShouldReturnListValueEnumerable()
    {
        var list = new List<int> { 1, 2, 3 };
        var valueEnum = list.AsValueEnumerable();
        valueEnum.Must().BeOfType<ListValueEnumerable<int>>();
    }
    
    [Test]
    public void List_AsValueEnumerable_ShouldBeEnumerableOf()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };
        var valueEnum = list.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(list);
    }
    
    [Test]
    public void List_AsValueEnumerable_Count_ShouldMatchLinq()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };
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
    public void List_AsValueEnumerable_Any_ShouldMatchLinq()
    {
        var list = new List<int> { 1, 2, 3 };
        var valueEnum = list.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(list)
            .EvaluateTrue(e => e.Any() == list.Any());
    }
    
    [Test]
    public void List_Empty_Any_ShouldMatchLinq()
    {
        var list = new List<int>();
        var valueEnum = list.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(list)
            .EvaluateTrue(e => e.Any() == false);
    }
    
    [Test]
    public void List_AsValueEnumerable_First_ShouldMatchLinq()
    {
        var list = new List<int> { 10, 20, 30 };
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
    public void List_AsValueEnumerable_Sum_ShouldMatchLinq()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };
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
    public void Array_AsValueEnumerable_ShouldReturnArrayValueEnumerable()
    {
        var array = new int[] { 1, 2, 3 };
        var valueEnum = array.AsValueEnumerable();
        valueEnum.Must().BeOfType<ArrayValueEnumerable<int>>();
    }
    
    [Test]
    public void Array_AsValueEnumerable_ShouldBeEnumerableOf()
    {
        var array = new int[] { 1, 2, 3, 4, 5 };
        var valueEnum = array.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(array);
    }
    
    [Test]
    public void Array_AsValueEnumerable_Count_ShouldMatchLinq()
    {
        var array = new int[] { 1, 2, 3, 4, 5 };
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
    public void Array_AsValueEnumerable_Sum_ShouldMatchLinq()
    {
        var array = new int[] { 10, 20, 30 };
        var valueEnum = array.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(array)
            .EvaluateTrue(e => e.Sum() == array.Sum());
    }
    
    [Test]
    public void Array_AsValueEnumerable_Any_ShouldMatchLinq()
    {
        var array = new int[] { 1, 2, 3 };
        var valueEnum = array.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(array)
            .EvaluateTrue(e => e.Any() == array.Any());
    }
    
    // ===== Chained Operations Tests =====
    
    [Test]
    public void List_WhereSelect_ShouldMatchLinq()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };
        
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
    public void List_WhereSelectSum_ShouldMatchLinq()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };
        
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
    public void Array_WhereSelectCount_ShouldMatchLinq()
    {
        var array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        
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
    public void List_MultipleWhere_ShouldMatchLinq()
    {
        var list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        
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
    
    [Test]
    public void List_Empty_Count_ShouldMatchLinq()
    {
        var list = new List<int>();
        var valueEnum = list.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEmpty()
            .EvaluateTrue(e => e.Count() == 0);
    }
    
    [Test]
    public void Array_Empty_Sum_ShouldMatchLinq()
    {
        var array = new int[] { };
        var valueEnum = array.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEmpty()
            .EvaluateTrue(e => e.Sum() == 0);
    }
    
    [Test]
    public void List_SingleElement_First_ShouldMatchLinq()
    {
        var list = new List<int> { 99 };
        var valueEnum = list.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(list)
            .EvaluateTrue(e => e.First() == 99);
    }
    
    [Test]
    public void List_LargeCollection_Sum_ShouldMatchLinq()
    {
        var list = Enumerable.Range(1, 1000).ToList();
        var valueEnum = list.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(list)
            .EvaluateTrue(e => e.Sum() == list.Sum());
    }
    
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
    public void List_AsValueEnumerable_ShouldSupportMultipleEnumerations()
    {
        var list = new List<int> { 1, 2, 3 };
        var valueEnum = list.AsValueEnumerable();
        
        // NetFabric.Assertive will enumerate multiple times to verify behavior
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(list);
    }
    
    [Test]
    public void Array_AsValueEnumerable_ShouldSupportMultipleEnumerations()
    {
        var array = new int[] { 1, 2, 3 };
        var valueEnum = array.AsValueEnumerable();
        
        // NetFabric.Assertive will enumerate multiple times to verify behavior
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(array);
    }
}
