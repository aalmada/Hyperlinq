using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TUnit.Core;
using NetFabric.Hyperlinq.LinqHelper;

namespace NetFabric.Hyperlinq.UnitTests;

public class WhereTests
{
    [Test]
    [MethodDataSource(nameof(GetPredicates))]
    public async Task Where_WithVariousPredicates_ShouldMatchLinq(
        Func<IEnumerable<int>> sourceFactory,
        Func<int, bool> predicate,
        string description)
    {
        var source = sourceFactory();
        var hyperlinqResult = source.Where(predicate).ToList();
        var linqResult = StandardLinq.Where(source, predicate).ToList();
        
        await Assert.That(hyperlinqResult).IsEquivalentTo(linqResult);
    }
    
    public static IEnumerable<(Func<IEnumerable<int>> sourceFactory, Func<int, bool> predicate, string description)> GetPredicates()
    {
        yield return (() => new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, x => x % 2 == 0, "Even numbers");
        yield return (() => new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, x => x % 2 != 0, "Odd numbers");
        yield return (() => new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, x => x > 5, "Greater than 5");
        yield return (() => new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, x => x < 5, "Less than 5");
        yield return (() => new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, x => x >= 5 && x <= 7, "Between 5 and 7");
        yield return (() => new int[] { 1, 2, 3 }, x => x > 10, "No matches");
        yield return (() => new int[] { 1, 2, 3 }, x => true, "All match");
    }
}
