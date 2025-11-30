using System;
using System.Collections.Generic;
using System.Linq;
using TUnit.Core;
using NetFabric.Assertive;

namespace NetFabric.Hyperlinq.UnitTests;

public class AsValueEnumerableSumTests
{
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
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetEnumerableSources))]
    public void IEnumerable_AsValueEnumerable_Sum_ShouldMatchLinq((Func<IEnumerable<int>> enumerableFactory, string description) testCase)
    {
        IEnumerable<int> enumerable = testCase.enumerableFactory();
        var valueEnum = enumerable.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(enumerable)
            .EvaluateTrue(e => e.Sum() == enumerable.Sum());
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetCollectionSources))]
    public void ICollection_AsValueEnumerable_Sum_ShouldMatchLinq((Func<ICollection<int>> collectionFactory, string description) testCase)
    {
        ICollection<int> collection = testCase.collectionFactory();
        var valueEnum = collection.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(collection)
            .EvaluateTrue(e => e.Sum() == collection.Sum());
    }
    
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
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_WhereSelectSum_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var list = new List<int>(testCase.arrayFactory());
        
        var hyperlinqResult = list.AsValueEnumerable()
                                  .Where(x => x % 2 == 0)
                                  .Select(x => x * 10)
                                  .Sum();
        
        var linqResult = list.AsValueEnumerable().Where(x => x % 2 == 0)
                              .Select(x => x * 10)
                              .Sum();
        
        hyperlinqResult.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetEnumerableSources))]
    public void IEnumerable_WhereSelectSum_ShouldMatchLinq((Func<IEnumerable<int>> enumerableFactory, string description) testCase)
    {
        IEnumerable<int> enumerable = testCase.enumerableFactory();
        
        var hyperlinqResult = enumerable.AsValueEnumerable()
                                  .Where(x => x % 2 == 0)
                                  .Select(x => x * 10)
                                  .Sum();
        
        var linqResult = enumerable.Where(x => x % 2 == 0)
                            .Select(x => x * 10)
                            .Sum();
        
        hyperlinqResult.Must().BeEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetCollectionSources))]
    public void ICollection_WhereSelectSum_ShouldMatchLinq((Func<ICollection<int>> collectionFactory, string description) testCase)
    {
        ICollection<int> collection = testCase.collectionFactory();
        
        var hyperlinqResult = collection.AsValueEnumerable()
                                  .Where(x => x % 2 == 0)
                                  .Select(x => x * 10)
                                  .Sum();
        
        var linqResult = collection.Where(x => x % 2 == 0)
                            .Select(x => x * 10)
                            .Sum();
        
        hyperlinqResult.Must().BeEqualTo(linqResult);
    }
}
