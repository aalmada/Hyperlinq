using System;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Assertive;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests.Span;

public class SpanWhereSelectSumTests
{
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Memory_WhereSelect_Sum_ShouldMatchLinq(TestCase<int[]> testCase)
    {
        var array = testCase.Factory();
        ReadOnlyMemory<int> memory = array.AsMemory();

        var result = memory
            .Where(x => x % 2 == 0)
            .Select(x => x * 2)
            .Sum();

        var expected = Enumerable.Sum(Enumerable.Select(Enumerable.Where(array, x => x % 2 == 0), x => x * 2));

        _ = result.Must().BeEqualTo(expected);
    }
}
