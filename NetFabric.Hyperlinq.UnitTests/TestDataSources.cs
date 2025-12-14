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
    /// <summary>
    /// Standard int array sources covering various scenarios.
    /// </summary>
    public static IEnumerable<TestCase<int[]>> GetIntArraySources()
    {
        yield return new(() => new int[] { 1, 2, 3, 4, 5 }, "Array with 5 elements");
        yield return new(() => new int[] { 10, 20, 30 }, "Array with 3 elements");
        yield return new(() => Array.Empty<int>(), "Empty array");
        yield return new(() => new int[] { 42 }, "Single element");
        yield return new(() => Enumerable.Range(1, 100).ToArray(), "Large array (100 elements)");
    }

    /// <summary>
    /// Non-empty int array sources (for tests requiring at least one element).
    /// </summary>
    public static IEnumerable<TestCase<int[]>> GetNonEmptyIntArraySources()
    {
        yield return new(() => new int[] { 1, 2, 3, 4, 5 }, "Array with 5 elements");
        yield return new(() => new int[] { 10, 20, 30 }, "Array with 3 elements");
        yield return new(() => new int[] { 42 }, "Single element");
        yield return new(() => Enumerable.Range(1, 100).ToArray(), "Large array (100 elements)");
    }

    /// <summary>
    /// Edge case int array sources for predicate testing.
    /// </summary>
    public static IEnumerable<TestCase<int[]>> GetEdgeCaseIntArraySources()
    {
        yield return new(() => new int[] { 2, 4, 6, 8, 10 }, "All match predicate (even numbers)");
        yield return new(() => new int[] { 1, 3, 5, 7, 9 }, "None match predicate (odd numbers)");
    }

    /// <summary>
    /// IEnumerable sources for AsValueEnumerable tests.
    /// Uses Enumerable.Range to create pure IEnumerable instances.
    /// </summary>
    public static IEnumerable<TestCase<IEnumerable<int>>> GetEnumerableSources()
    {
        yield return new(() => Enumerable.Range(1, 5), "IEnumerable with 5 elements");
        yield return new(() => Enumerable.Range(10, 3), "IEnumerable with 3 elements");
        yield return new(() => Enumerable.Empty<int>(), "Empty IEnumerable");
        yield return new(() => Enumerable.Range(42, 1), "Single element IEnumerable");
        yield return new(() => Enumerable.Range(1, 100), "Large IEnumerable (100 elements)");
    }

    /// <summary>
    /// Non-empty IEnumerable sources.
    /// </summary>
    public static IEnumerable<TestCase<IEnumerable<int>>> GetNonEmptyEnumerableSources()
    {
        yield return new(() => Enumerable.Range(1, 5), "IEnumerable with 5 elements");
        yield return new(() => Enumerable.Range(10, 3), "IEnumerable with 3 elements");
        yield return new(() => Enumerable.Range(42, 1), "Single element IEnumerable");
        yield return new(() => Enumerable.Range(1, 100), "Large IEnumerable (100 elements)");
    }

    /// <summary>
    /// ICollection sources for AsValueEnumerable tests.
    /// Uses List as the ICollection implementation.
    /// </summary>
    public static IEnumerable<TestCase<ICollection<int>>> GetCollectionSources()
    {
        yield return new(() => new List<int> { 1, 2, 3, 4, 5 }, "ICollection with 5 elements");
        yield return new(() => new List<int> { 10, 20, 30 }, "ICollection with 3 elements");
        yield return new(() => new List<int>(), "Empty ICollection");
        yield return new(() => new List<int> { 42 }, "Single element ICollection");
        yield return new(() => Enumerable.Range(1, 100).ToList(), "Large ICollection (100 elements)");
    }

    /// <summary>
    /// Non-empty ICollection sources.
    /// </summary>
    public static IEnumerable<TestCase<ICollection<int>>> GetNonEmptyCollectionSources()
    {
        yield return new(() => new List<int> { 1, 2, 3, 4, 5 }, "ICollection with 5 elements");
        yield return new(() => new List<int> { 10, 20, 30 }, "ICollection with 3 elements");
        yield return new(() => new List<int> { 42 }, "Single element ICollection");
        yield return new(() => Enumerable.Range(1, 100).ToList(), "Large ICollection (100 elements)");
    }
}
