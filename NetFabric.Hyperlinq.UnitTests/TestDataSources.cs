using System;
using System.Collections.Generic;
using System.Linq;

namespace NetFabric.Hyperlinq.UnitTests;

/// <summary>
/// Shared test data sources used across all unit tests.
/// Returns array factories for maximum flexibility - tests convert to List/Memory/Span as needed.
/// </summary>
public static class TestDataSources
{
    /// <summary>
    /// Standard int array sources covering various scenarios.
    /// </summary>
    public static IEnumerable<(Func<int[]> arrayFactory, string description)> GetIntArraySources()
    {
        yield return (() => new int[] { 1, 2, 3, 4, 5 }, "Array with 5 elements");
        yield return (() => new int[] { 10, 20, 30 }, "Array with 3 elements");
        yield return (() => Array.Empty<int>(), "Empty array");
        yield return (() => new int[] { 42 }, "Single element");
        yield return (() => Enumerable.Range(1, 100).ToArray(), "Large array (100 elements)");
    }
    
    /// <summary>
    /// Non-empty int array sources (for tests requiring at least one element).
    /// </summary>
    public static IEnumerable<(Func<int[]> arrayFactory, string description)> GetNonEmptyIntArraySources()
    {
        yield return (() => new int[] { 1, 2, 3, 4, 5 }, "Array with 5 elements");
        yield return (() => new int[] { 10, 20, 30 }, "Array with 3 elements");
        yield return (() => new int[] { 42 }, "Single element");
        yield return (() => Enumerable.Range(1, 100).ToArray(), "Large array (100 elements)");
    }
    
    /// <summary>
    /// Edge case int array sources for predicate testing.
    /// </summary>
    public static IEnumerable<(Func<int[]> arrayFactory, string description)> GetEdgeCaseIntArraySources()
    {
        yield return (() => new int[] { 2, 4, 6, 8, 10 }, "All match predicate (even numbers)");
        yield return (() => new int[] { 1, 3, 5, 7, 9 }, "None match predicate (odd numbers)");
    }
}
