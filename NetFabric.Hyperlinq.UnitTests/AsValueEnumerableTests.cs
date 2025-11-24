using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests;

public class AsValueEnumerableTests
{
    // ===== List<T> Tests =====
    
    [Test]
    public async Task List_AsValueEnumerable_ShouldReturnListValueEnumerable()
    {
        var list = new List<int> { 1, 2, 3 };
        var valueEnum = list.AsValueEnumerable();
        await Assert.That(valueEnum.GetType().Name).IsEqualTo("ListValueEnumerable`1");
    }
    
    [Test]
    public async Task List_AsValueEnumerable_Count_ShouldMatchLinq()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };
        var hyperlinqResult = list.AsValueEnumerable().Count();
        var linqResult = list.Count();
        await Assert.That(hyperlinqResult).IsEqualTo(linqResult);
    }
    
    [Test]
    public async Task List_AsValueEnumerable_Indexer_ShouldWork()
    {
        var list = new List<int> { 10, 20, 30, 40, 50 };
        var valueEnum = list.AsValueEnumerable();
        
        await Assert.That(valueEnum[0]).IsEqualTo(10);
        await Assert.That(valueEnum[2]).IsEqualTo(30);
        await Assert.That(valueEnum[4]).IsEqualTo(50);
    }
    
    [Test]
    public async Task List_AsValueEnumerable_Any_ShouldMatchLinq()
    {
        var list = new List<int> { 1, 2, 3 };
        var hyperlinqResult = list.AsValueEnumerable().Any();
        var linqResult = list.Any();
        await Assert.That(hyperlinqResult).IsEqualTo(linqResult);
    }
    
    [Test]
    public async Task List_Empty_Any_ShouldMatchLinq()
    {
        var list = new List<int>();
        var hyperlinqResult = list.AsValueEnumerable().Any();
        var linqResult = list.Any();
        await Assert.That(hyperlinqResult).IsEqualTo(linqResult);
        await Assert.That(hyperlinqResult).IsFalse();
    }
    
    [Test]
    public async Task List_AsValueEnumerable_First_ShouldMatchLinq()
    {
        var list = new List<int> { 10, 20, 30 };
        var hyperlinqResult = list.AsValueEnumerable().First();
        var linqResult = list.First();
        await Assert.That(hyperlinqResult).IsEqualTo(linqResult);
    }
    
    [Test]
    public async Task List_AsValueEnumerable_Single_ShouldMatchLinq()
    {
        var list = new List<int> { 42 };
        var hyperlinqResult = list.AsValueEnumerable().Single();
        var linqResult = list.Single();
        await Assert.That(hyperlinqResult).IsEqualTo(linqResult);
    }
    
    [Test]
    public async Task List_AsValueEnumerable_Sum_ShouldMatchLinq()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };
        var hyperlinqResult = list.AsValueEnumerable().Sum();
        var linqResult = list.Sum();
        await Assert.That(hyperlinqResult).IsEqualTo(linqResult);
    }
    
    [Test]
    public async Task List_AsValueEnumerable_Sum_Double_ShouldMatchLinq()
    {
        var list = new List<double> { 1.5, 2.5, 3.5 };
        var hyperlinqResult = list.AsValueEnumerable().Sum();
        var linqResult = list.Sum();
        await Assert.That(Math.Abs(hyperlinqResult - linqResult)).IsLessThan(1e-10);
    }
    
    // ===== Array Tests =====
    
    [Test]
    public async Task Array_AsValueEnumerable_ShouldReturnArrayValueEnumerable()
    {
        var array = new int[] { 1, 2, 3 };
        var valueEnum = array.AsValueEnumerable();
        await Assert.That(valueEnum.GetType().Name).IsEqualTo("ArrayValueEnumerable`1");
    }
    
    [Test]
    public async Task Array_AsValueEnumerable_Count_ShouldMatchLinq()
    {
        var array = new int[] { 1, 2, 3, 4, 5 };
        var hyperlinqResult = array.AsValueEnumerable().Count();
        var linqResult = array.Count();
        await Assert.That(hyperlinqResult).IsEqualTo(linqResult);
    }
    
    [Test]
    public async Task Array_AsValueEnumerable_Indexer_ShouldWork()
    {
        var array = new int[] { 10, 20, 30, 40, 50 };
        var valueEnum = array.AsValueEnumerable();
        
        await Assert.That(valueEnum[0]).IsEqualTo(10);
        await Assert.That(valueEnum[2]).IsEqualTo(30);
        await Assert.That(valueEnum[4]).IsEqualTo(50);
    }
    
    [Test]
    public async Task Array_AsValueEnumerable_Sum_ShouldMatchLinq()
    {
        var array = new int[] { 10, 20, 30 };
        var hyperlinqResult = array.AsValueEnumerable().Sum();
        var linqResult = array.Sum();
        await Assert.That(hyperlinqResult).IsEqualTo(linqResult);
    }
    
    [Test]
    public async Task Array_AsValueEnumerable_Any_ShouldMatchLinq()
    {
        var array = new int[] { 1, 2, 3 };
        var hyperlinqResult = array.AsValueEnumerable().Any();
        var linqResult = array.Any();
        await Assert.That(hyperlinqResult).IsEqualTo(linqResult);
    }
    
    // ===== Chained Operations Tests =====
    
    [Test]
    public async Task List_WhereSelect_ShouldMatchLinq()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };
        
        var hyperlinqResult = list.AsValueEnumerable()
                                  .Where(x => x % 2 == 0)
                                  .Select(x => x * 10)
                                  .ToList();
        
        var linqResult = list.Where(x => x % 2 == 0)
                            .Select(x => x * 10)
                            .ToList();
        
        await Assert.That(hyperlinqResult).IsEquivalentTo(linqResult);
    }
    
    [Test]
    public async Task List_WhereSelectSum_ShouldMatchLinq()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };
        
        var hyperlinqResult = list.AsValueEnumerable()
                                  .Where(x => x % 2 == 0)
                                  .Select(x => x * 10)
                                  .Sum();
        
        var linqResult = list.Where(x => x % 2 == 0)
                            .Select(x => x * 10)
                            .Sum();
        
        await Assert.That(hyperlinqResult).IsEqualTo(linqResult);
    }
    
    [Test]
    public async Task Array_WhereSelectCount_ShouldMatchLinq()
    {
        var array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        
        var hyperlinqResult = array.AsValueEnumerable()
                                   .Where(x => x > 5)
                                   .Select(x => x * 2)
                                   .Count();
        
        var linqResult = array.Where(x => x > 5)
                             .Select(x => x * 2)
                             .Count();
        
        await Assert.That(hyperlinqResult).IsEqualTo(linqResult);
    }
    
    [Test]
    public async Task List_MultipleWhere_ShouldMatchLinq()
    {
        var list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        
        var hyperlinqResult = list.AsValueEnumerable()
                                  .Where(x => x > 3)
                                  .Where(x => x % 2 == 0)
                                  .ToList();
        
        var linqResult = list.Where(x => x > 3)
                            .Where(x => x % 2 == 0)
                            .ToList();
        
        await Assert.That(hyperlinqResult).IsEquivalentTo(linqResult);
    }
    
    // ===== Edge Cases =====
    
    [Test]
    public async Task List_Empty_Count_ShouldMatchLinq()
    {
        var list = new List<int>();
        var hyperlinqResult = list.AsValueEnumerable().Count();
        var linqResult = list.Count();
        await Assert.That(hyperlinqResult).IsEqualTo(linqResult);
        await Assert.That(hyperlinqResult).IsEqualTo(0);
    }
    
    [Test]
    public async Task Array_Empty_Sum_ShouldMatchLinq()
    {
        var array = new int[] { };
        var hyperlinqResult = array.AsValueEnumerable().Sum();
        var linqResult = array.Sum();
        await Assert.That(hyperlinqResult).IsEqualTo(linqResult);
        await Assert.That(hyperlinqResult).IsEqualTo(0);
    }
    
    [Test]
    public async Task List_SingleElement_First_ShouldMatchLinq()
    {
        var list = new List<int> { 99 };
        var hyperlinqResult = list.AsValueEnumerable().First();
        var linqResult = list.First();
        await Assert.That(hyperlinqResult).IsEqualTo(linqResult);
    }
    
    [Test]
    public async Task List_LargeCollection_Sum_ShouldMatchLinq()
    {
        var list = Enumerable.Range(1, 1000).ToList();
        var hyperlinqResult = list.AsValueEnumerable().Sum();
        var linqResult = list.Sum();
        await Assert.That(hyperlinqResult).IsEqualTo(linqResult);
    }
    
    // ===== IEnumerable<T> Fallback Tests =====
    
    [Test]
    public async Task IEnumerable_AsValueEnumerable_ShouldReturnEnumerableValueEnumerable()
    {
        IEnumerable<int> enumerable = Enumerable.Range(1, 5);
        var valueEnum = enumerable.AsValueEnumerable();
        await Assert.That(valueEnum.GetType().Name).IsEqualTo("EnumerableValueEnumerable`1");
    }
    
    [Test]
    public async Task IEnumerable_AsValueEnumerable_Sum_ShouldMatchLinq()
    {
        IEnumerable<int> enumerable = Enumerable.Range(1, 10);
        var hyperlinqResult = enumerable.AsValueEnumerable().Sum();
        var linqResult = enumerable.Sum();
        await Assert.That(hyperlinqResult).IsEqualTo(linqResult);
    }
    
    // ===== Type-Specific Tests =====
    
    [Test]
    public async Task List_Long_Sum_ShouldMatchLinq()
    {
        var list = new List<long> { 1L, 2L, 3L, 4L, 5L };
        var hyperlinqResult = list.AsValueEnumerable().Sum();
        var linqResult = list.Sum();
        await Assert.That(hyperlinqResult).IsEqualTo(linqResult);
    }
    
    [Test]
    public async Task Array_Float_Sum_ShouldMatchLinq()
    {
        var array = new float[] { 1.5f, 2.5f, 3.5f };
        var hyperlinqResult = array.AsValueEnumerable().Sum();
        var linqResult = array.Sum();
        await Assert.That(Math.Abs(hyperlinqResult - linqResult)).IsLessThan(1e-6f);
    }
}
