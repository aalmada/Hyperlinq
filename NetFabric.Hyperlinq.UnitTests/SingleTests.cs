using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUnit.Core;
using NetFabric.Hyperlinq.LinqHelper;

namespace NetFabric.Hyperlinq.UnitTests;

public class SingleTests
{
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetSingleElementSources))]
    public async Task Single_WithSingleElement_ShouldMatchLinq(Func<IEnumerable<int>> sourceFactory, string description)
    {
        var source = sourceFactory();
        var hyperlinqResult = source.Single();
        var linqResult = StandardLinq.Single(source);
        
        await Assert.That(hyperlinqResult).IsEqualTo(linqResult);
    }
}
