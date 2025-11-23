using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using TUnit.Core;
using NetFabric.Hyperlinq.LinqHelper;

namespace NetFabric.Hyperlinq.UnitTests;

public class AnyTests
{
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntEnumerableSources))]
    public async Task Any_IEnumerable_WithVariousSources_ShouldMatchLinq(Func<IEnumerable<int>> sourceFactory, string description)
    {
        var source = sourceFactory();
        var hyperlinqResult = source.Any();
        var linqResult = StandardLinq.Any(source);
        
        await Assert.That(hyperlinqResult).IsEqualTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntCollectionSources))]
    public async Task Any_ICollection_DirectCall_ShouldMatchLinq(Func<ICollection<int>> sourceFactory, string description)
    {
        var source = sourceFactory();
        // Explicitly call the ICollection<T> overload
        var hyperlinqResult = NetFabric.Hyperlinq.Optimized.Any(source);
        var linqResult = StandardLinq.Any(source);
        
        await Assert.That(hyperlinqResult).IsEqualTo(linqResult);
    }
}
