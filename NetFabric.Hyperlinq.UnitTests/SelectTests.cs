using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TUnit.Core;
using NetFabric.Hyperlinq.LinqHelper;

namespace NetFabric.Hyperlinq.UnitTests;

public class SelectTests
{
    [Test]
    [Arguments(2, "Multiply by 2")]
    [Arguments(10, "Multiply by 10")]
    [Arguments(-1, "Multiply by -1")]
    public async Task Select_WithDifferentMultipliers_ShouldMatchLinq(int multiplier, string description)
    {
        IEnumerable<int> source = new List<int> { 1, 2, 3, 4, 5 };
        
        var hyperlinqResult = source.Select(x => x * multiplier).ToList();
        var linqResult = StandardLinq.Select(source, x => x * multiplier).ToList();
        
        await Assert.That(hyperlinqResult).IsEquivalentTo(linqResult);
    }
    
    [Test]
    [MethodDataSource(nameof(GetTransformations))]
    public async Task Select_WithVariousTransformations_ShouldMatchLinq(
        Func<IEnumerable<int>> sourceFactory, 
        Func<int, int> selector,
        string description)
    {
        var source = sourceFactory();
        var hyperlinqResult = source.Select(selector).ToList();
        var linqResult = StandardLinq.Select(source, selector).ToList();
        
        await Assert.That(hyperlinqResult).IsEquivalentTo(linqResult);
    }
    
    public static IEnumerable<(Func<IEnumerable<int>> sourceFactory, Func<int, int> selector, string description)> GetTransformations()
    {
        yield return (() => new List<int> { 1, 2, 3 }, x => x * 2, "Double values");
        yield return (() => new int[] { 1, 2, 3 }, x => x + 10, "Add 10");
        yield return (() => new List<int> { 1, 2, 3 }, x => x * x, "Square values");
        yield return (() => new int[] { 5, 10, 15 }, x => x / 5, "Divide by 5");
    }
}
