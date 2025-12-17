using System;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Assertive;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests.Operations.Filtering;

public class EnumerableWhereTests
{
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetEnumerableSources))]
    public void Enumerable_Where_ToList_ShouldMatchLinq(TestCase<IEnumerable<int>> testCase)
    {
        var enumerable = testCase.Factory();
        var expected = Enumerable.ToList(Enumerable.Where(enumerable, x => x % 2 == 0));

        var result = enumerable.AsValueEnumerable() // This wraps it in ValueEnumerable wrapper
            .Where(x => x % 2 == 0) // This creates WhereEnumerable
            .ToList(); // This uses DynamicArrayBuilder optimization

        _ = result.Must().BeEnumerableOf<int>().BeEqualTo(expected);
    }

    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetEnumerableSources))]
    public void Enumerable_Where_Select_ToList_ShouldMatchLinq(TestCase<IEnumerable<int>> testCase)
    {
        var enumerable = testCase.Factory();
        var expected = Enumerable.ToList(Enumerable.Select(Enumerable.Where(enumerable, x => x % 2 == 0), x => x * 2));

        var result = enumerable.AsValueEnumerable()
            .Where(x => x % 2 == 0)
            .Select(x => x * 2) // This creates WhereSelectEnumerable
            .ToList(); // This uses DynamicArrayBuilder optimization

        _ = result.Must().BeEnumerableOf<int>().BeEqualTo(expected);
    }
}
