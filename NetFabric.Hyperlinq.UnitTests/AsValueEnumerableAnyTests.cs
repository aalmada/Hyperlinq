using System;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Assertive;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests;

public class AsValueEnumerableAnyTests
{
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_AsValueEnumerable_Any_ShouldMatchLinq(TestCase<int[]> testCase)
    {
        var list = new List<int>(testCase.Factory());
        var valueEnum = list.AsValueEnumerable();

        _ = valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(list)
            .EvaluateTrue(e => e.Any() == list.Any());
    }

    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_AsValueEnumerable_Any_ShouldMatchLinq(TestCase<int[]> testCase)
    {
        var array = testCase.Factory();
        var valueEnum = array.AsValueEnumerable();

        _ = valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(array)
            .EvaluateTrue(e => e.Any() == array.Any());
    }

    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetEnumerableSources))]
    public void IEnumerable_AsValueEnumerable_Any_ShouldMatchLinq(TestCase<IEnumerable<int>> testCase)
    {
        IEnumerable<int> enumerable = testCase.Factory();
        var valueEnum = enumerable.AsValueEnumerable();

        _ = valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(enumerable)
            .EvaluateTrue(e => e.Any() == enumerable.Any());
    }

    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetCollectionSources))]
    public void ICollection_AsValueEnumerable_Any_ShouldMatchLinq(TestCase<ICollection<int>> testCase)
    {
        ICollection<int> collection = testCase.Factory();
        var valueEnum = collection.AsValueEnumerable();

        _ = valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(collection)
            .EvaluateTrue(e => e.Any() == collection.Any());
    }
}
