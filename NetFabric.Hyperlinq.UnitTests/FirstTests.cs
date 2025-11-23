using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUnit.Core;
using NetFabric.Hyperlinq.LinqHelper;

namespace NetFabric.Hyperlinq.UnitTests;

public class FirstTests
{
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetNonEmptyIntEnumerableSources))]
    public async Task First_WithVariousSources_ShouldMatchLinq(Func<IEnumerable<int>> sourceFactory, string description)
    {
        var source = sourceFactory();
        var hyperlinqResult = source.First();
        var linqResult = StandardLinq.First(source);
        
        await Assert.That(hyperlinqResult).IsEqualTo(linqResult);
    }
}
