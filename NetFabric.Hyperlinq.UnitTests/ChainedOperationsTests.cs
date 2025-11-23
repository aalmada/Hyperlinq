using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TUnit.Core;
using NetFabric.Hyperlinq.LinqHelper;

namespace NetFabric.Hyperlinq.UnitTests;

public class ChainedOperationsTests
{
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntEnumerableSources))]
    public async Task WhereSelect_Chained_ShouldMatchLinq(Func<IEnumerable<int>> sourceFactory, string description)
    {
        var source = sourceFactory();
        var hyperlinqResult = source.Where(x => x % 2 == 0).Select(x => x * 10).ToList();
        var linqResult = StandardLinq.Select(StandardLinq.Where(source, x => x % 2 == 0), x => x * 10).ToList();
        
        await Assert.That(hyperlinqResult).IsEquivalentTo(linqResult);
    }
}
