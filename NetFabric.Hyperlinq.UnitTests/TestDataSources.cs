using System;
using System.Collections.Generic;
using System.Linq;

namespace NetFabric.Hyperlinq.UnitTests;

/// <summary>
/// Shared test data sources to avoid duplication across test classes.
/// Returns Func<T> for reference types to ensure proper test isolation.
/// </summary>
public static class TestDataSources
{
    public static IEnumerable<(Func<IEnumerable<int>> sourceFactory, string description)> GetIntEnumerableSources()
    {
        yield return (() => new List<int> { 1, 2, 3, 4, 5 }, "List with 5 elements");
        yield return (() => new int[] { 10, 20, 30, 40, 50 }, "Array with 5 elements");
        yield return (() => new List<int>(), "Empty list");
        yield return (() => new int[] { }, "Empty array");
        yield return (() => Enumerable.Range(1, 100), "Large enumerable (not ICollection)");
        yield return (() => new int[] { 42 }, "Single element");
    }
    
    public static IEnumerable<(Func<ICollection<int>> sourceFactory, string description)> GetIntCollectionSources()
    {
        yield return (() => new List<int> { 1, 2, 3, 4, 5 }, "List with 5 elements");
        yield return (() => new int[] { 10, 20, 30, 40, 50 }, "Array with 5 elements");
        yield return (() => new List<int>(), "Empty list");
        yield return (() => new int[] { }, "Empty array");
        yield return (() => new HashSet<int> { 1, 2, 3 }, "HashSet with 3 elements");
        yield return (() => new int[] { 42 }, "Single element");
    }
    
    public static IEnumerable<(Func<IEnumerable<int>> sourceFactory, string description)> GetNonEmptyIntEnumerableSources()
    {
        yield return (() => new List<int> { 1, 2, 3, 4, 5 }, "List with 5 elements");
        yield return (() => new int[] { 10, 20, 30, 40, 50 }, "Array with 5 elements");
        yield return (() => Enumerable.Range(1, 100), "Large enumerable");
        yield return (() => new int[] { 42 }, "Single element");
    }
    
    public static IEnumerable<(Func<IEnumerable<int>> sourceFactory, string description)> GetSingleElementSources()
    {
        yield return (() => new List<int> { 42 }, "List with single element");
        yield return (() => new int[] { 99 }, "Array with single element");
        yield return (() => Enumerable.Range(1, 1), "Enumerable with single element");
    }
}
