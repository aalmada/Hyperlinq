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
    public void List_AsValueEnumerable_ShouldReturnListValueEnumerable(TestCase<int[]> testCase)
    {
        var list = new List<int>(testCase.Factory());
        var valueEnum = list.AsValueEnumerable();
        valueEnum.Must().BeOfType<ListValueEnumerable<int>>();
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_AsValueEnumerable_ShouldBeEnumerableOf(TestCase<int[]> testCase)
    {
        var list = new List<int>(testCase.Factory());
        var valueEnum = list.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(list);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_AsValueEnumerable_ShouldReturnArrayValueEnumerable(TestCase<int[]> testCase)
    {
        var array = testCase.Factory();
        var valueEnum = array.AsValueEnumerable();
        valueEnum.Must().BeOfType<ArrayValueEnumerable<int>>();
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_AsValueEnumerable_ShouldBeEnumerableOf(TestCase<int[]> testCase)
    {
        var array = testCase.Factory();
        var valueEnum = array.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(array);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetEnumerableSources))]
    public void IEnumerable_AsValueEnumerable_ShouldReturnEnumerableValueEnumerable(TestCase<IEnumerable<int>> testCase)
    {
        IEnumerable<int> enumerable = testCase.Factory();
        var valueEnum = enumerable.AsValueEnumerable();
        valueEnum.Must().BeOfType<EnumerableValueEnumerable<int>>();
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetEnumerableSources))]
    public void IEnumerable_AsValueEnumerable_ShouldBeEnumerableOf(TestCase<IEnumerable<int>> testCase)
    {
        IEnumerable<int> enumerable = testCase.Factory();
        var valueEnum = enumerable.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(enumerable);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetCollectionSources))]
    public void ICollection_AsValueEnumerable_ShouldBeEnumerableOf(TestCase<ICollection<int>> testCase)
    {
        ICollection<int> collection = testCase.Factory();
        var valueEnum = collection.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(collection);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_AsValueEnumerable_ShouldSupportMultipleEnumerations(TestCase<int[]> testCase)
    {
        var list = new List<int>(testCase.Factory());
        var valueEnum = list.AsValueEnumerable();
        
        // NetFabric.Assertive will enumerate multiple times to verify behavior
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(list);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetEnumerableSources))]
    public void IEnumerable_AsValueEnumerable_ShouldSupportMultipleEnumerations(TestCase<IEnumerable<int>> testCase)
    {
        IEnumerable<int> enumerable = testCase.Factory();
        var valueEnum = enumerable.AsValueEnumerable();
        
        // NetFabric.Assertive will enumerate multiple times to verify behavior
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(enumerable);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetCollectionSources))]
    public void ICollection_AsValueEnumerable_ShouldSupportMultipleEnumerations(TestCase<ICollection<int>> testCase)
    {
        ICollection<int> collection = testCase.Factory();
        var valueEnum = collection.AsValueEnumerable();
        
        // NetFabric.Assertive will enumerate multiple times to verify behavior
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(collection);
    }
}
