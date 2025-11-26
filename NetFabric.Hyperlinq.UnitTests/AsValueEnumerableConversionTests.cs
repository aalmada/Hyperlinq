using System;
using System.Collections.Generic;
using System.Linq;
using TUnit.Core;
using NetFabric.Assertive;

namespace NetFabric.Hyperlinq.UnitTests;

public class AsValueEnumerableConversionTests
{
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
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetEnumerableSources))]
    public void IEnumerable_AsValueEnumerable_ShouldReturnEnumerableValueEnumerable((Func<IEnumerable<int>> enumerableFactory, string description) testCase)
    {
        IEnumerable<int> enumerable = testCase.enumerableFactory();
        var valueEnum = enumerable.AsValueEnumerable();
        valueEnum.Must().BeOfType<EnumerableValueEnumerable<int>>();
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetEnumerableSources))]
    public void IEnumerable_AsValueEnumerable_ShouldBeEnumerableOf((Func<IEnumerable<int>> enumerableFactory, string description) testCase)
    {
        IEnumerable<int> enumerable = testCase.enumerableFactory();
        var valueEnum = enumerable.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(enumerable);
    }

    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetCollectionSources))]
    public void ICollection_AsValueEnumerable_ShouldBeEnumerableOf((Func<ICollection<int>> collectionFactory, string description) testCase)
    {
        ICollection<int> collection = testCase.collectionFactory();
        var valueEnum = collection.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(collection);
    }
    
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
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetEnumerableSources))]
    public void IEnumerable_AsValueEnumerable_ShouldSupportMultipleEnumerations((Func<IEnumerable<int>> enumerableFactory, string description) testCase)
    {
        IEnumerable<int> enumerable = testCase.enumerableFactory();
        var valueEnum = enumerable.AsValueEnumerable();
        
        // NetFabric.Assertive will enumerate multiple times to verify behavior
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(enumerable);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetCollectionSources))]
    public void ICollection_AsValueEnumerable_ShouldSupportMultipleEnumerations((Func<ICollection<int>> collectionFactory, string description) testCase)
    {
        ICollection<int> collection = testCase.collectionFactory();
        var valueEnum = collection.AsValueEnumerable();
        
        // NetFabric.Assertive will enumerate multiple times to verify behavior
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(collection);
    }
}
