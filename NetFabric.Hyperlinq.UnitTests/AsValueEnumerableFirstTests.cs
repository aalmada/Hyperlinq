using System;
using System.Collections.Generic;
using System.Linq;
using TUnit.Core;
using NetFabric.Assertive;

namespace NetFabric.Hyperlinq.UnitTests;

public class AsValueEnumerableFirstTests
{
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetNonEmptyIntArraySources))]
    public void List_AsValueEnumerable_First_ShouldMatchLinq(TestCase<int[]> testCase)
    {
        var list = new List<int>(testCase.Factory());
        var valueEnum = list.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(list)
            .EvaluateTrue(e => e.First() == list.First());
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetNonEmptyEnumerableSources))]
    public void IEnumerable_AsValueEnumerable_First_ShouldMatchLinq(TestCase<IEnumerable<int>> testCase)
    {
        IEnumerable<int> enumerable = testCase.Factory();
        var valueEnum = enumerable.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(enumerable)
            .EvaluateTrue(e => e.First() == enumerable.First());
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetNonEmptyCollectionSources))]
    public void ICollection_AsValueEnumerable_First_ShouldMatchLinq(TestCase<ICollection<int>> testCase)
    {
        ICollection<int> collection = testCase.Factory();
        var valueEnum = collection.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(collection)
            .EvaluateTrue(e => e.First() == collection.First());
    }
    
    [Test]
    public void IList_AsValueEnumerable_First_ShouldUseOptimization()
    {
        // Create an IList<int> to verify the optimization path is taken
        IList<int> list = new List<int> { 1, 2, 3, 4, 5 };
        var valueEnum = list.AsValueEnumerable();
        
        // The optimization should return the first element without enumeration
        var result = valueEnum.First();
        result.Must().BeEqualTo(1);
    }
    
    [Test]
    public void IList_AsValueEnumerable_First_EmptyList_ShouldThrow()
    {
        // Verify the optimization correctly throws for empty IList
        IList<int> list = new List<int>();
        var valueEnum = list.AsValueEnumerable();
        
        Action action = () => valueEnum.First();
        action.Must().Throw<InvalidOperationException>();
    }
}
