using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TUnit.Core;
using NetFabric.Hyperlinq.LinqHelper;

namespace NetFabric.Hyperlinq.UnitTests;

public class CountTests
{
    [Test]
    [MethodDataSource(nameof(GetIEnumerableTestData))]
    public async Task Count_IEnumerable_WithVariousSources_ShouldMatchLinq(Func<IEnumerable<int>> sourceFactory, int expectedCount, string description)
    {
        var source = sourceFactory();
        var hyperlinqResult = source.Count();
        var linqResult = StandardLinq.Count(source);
        
        await Assert.That(hyperlinqResult).IsEqualTo(linqResult);
        await Assert.That(hyperlinqResult).IsEqualTo(expectedCount);
    }
    
    [Test]
    [MethodDataSource(nameof(GetICollectionTestData))]
    public async Task Count_ICollection_DirectCall_ShouldMatchLinq(Func<ICollection<int>> sourceFactory, int expectedCount, string description)
    {
        var source = sourceFactory();
        // Explicitly call the ICollection<T> overload
        var hyperlinqResult = NetFabric.Hyperlinq.Optimized.Count(source);
        var linqResult = StandardLinq.Count(source);
        
        await Assert.That(hyperlinqResult).IsEqualTo(linqResult);
        await Assert.That(hyperlinqResult).IsEqualTo(expectedCount);
    }
    
    public static IEnumerable<(Func<IEnumerable<int>> sourceFactory, int expectedCount, string description)> GetIEnumerableTestData()
    {
        foreach (var (sourceFactory, description) in TestDataSources.GetIntEnumerableSources())
        {
            var source = sourceFactory();
            yield return (sourceFactory, source.Count(), description);
        }
    }
    
    public static IEnumerable<(Func<ICollection<int>> sourceFactory, int expectedCount, string description)> GetICollectionTestData()
    {
        foreach (var (sourceFactory, description) in TestDataSources.GetIntCollectionSources())
        {
            var source = sourceFactory();
            yield return (sourceFactory, source.Count, description);
        }
    }
}
