using System;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Assertive;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests;

public class AsValueEnumerableChainingTests
{
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_WhereSelect_ShouldMatchLinq(TestCase<int[]> testCase)
    {
        var list = new List<int>(testCase.Factory());

        var hyperlinqResult = list.AsValueEnumerable()
                                  .Where(x => x % 2 == 0)
                                  .Select(x => x * 10);

        var linqResult = list.AsValueEnumerable().Where(x => x % 2 == 0)
                             .Select(x => x * 10);

        _ = hyperlinqResult.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(linqResult);
    }

    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetEnumerableSources))]
    public void IEnumerable_WhereSelect_ShouldMatchLinq(TestCase<IEnumerable<int>> testCase)
    {
        IEnumerable<int> enumerable = testCase.Factory();

        var hyperlinqResult = enumerable.AsValueEnumerable()
                                  .Where(x => x % 2 == 0)
                                  .Select(x => x * 10);

        var linqResult = enumerable.Where(x => x % 2 == 0)
                             .Select(x => x * 10);

        _ = hyperlinqResult.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(linqResult);
    }

    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetCollectionSources))]
    public void ICollection_WhereSelect_ShouldMatchLinq(TestCase<ICollection<int>> testCase)
    {
        ICollection<int> collection = testCase.Factory();

        var hyperlinqResult = collection.AsValueEnumerable()
                                  .Where(x => x % 2 == 0)
                                  .Select(x => x * 10);

        var linqResult = collection.Where(x => x % 2 == 0)
                             .Select(x => x * 10);

        _ = hyperlinqResult.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(linqResult);
    }
}
