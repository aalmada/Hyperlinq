using System;
using System.Collections.Generic;
using System.Linq;
using TUnit.Core;
using NetFabric.Assertive;

namespace NetFabric.Hyperlinq.UnitTests;

public class AsValueEnumerableCountTests
{
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_AsValueEnumerable_Count_ShouldMatchLinq(TestCase<int[]> testCase)
    {
        var list = new List<int>(testCase.Factory());
        var valueEnum = list.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(list)
            .EvaluateTrue(e => e.Count() == list.Count());
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_AsValueEnumerable_Count_ShouldMatchLinq(TestCase<int[]> testCase)
    {
        var array = testCase.Factory();
        var valueEnum = array.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(array)
            .EvaluateTrue(e => e.Count() == array.Count());
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetEnumerableSources))]
    public void IEnumerable_AsValueEnumerable_Count_ShouldMatchLinq(TestCase<IEnumerable<int>> testCase)
    {
        IEnumerable<int> enumerable = testCase.Factory();
        var valueEnum = enumerable.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(enumerable)
            .EvaluateTrue(e => e.Count() == enumerable.Count());
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetCollectionSources))]
    public void ICollection_AsValueEnumerable_Count_ShouldMatchLinq(TestCase<ICollection<int>> testCase)
    {
        ICollection<int> collection = testCase.Factory();
        var valueEnum = collection.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(collection)
            .EvaluateTrue(e => e.Count() == collection.Count);
    }
}
