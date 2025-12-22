using System;
using System.Collections.Generic;
using System.Linq;

namespace NetFabric.Hyperlinq.UnitTests.TestData;

/// <summary>
/// Provides common test data collections used across unit tests.
/// Centralizes test data creation to reduce duplication and improve maintainability.
/// </summary>
public static class CommonTestData
{
    /// <summary>
    /// Small array with 5 elements: [1, 2, 3, 4, 5]
    /// </summary>
    public static int[] SmallArray => new[] { 1, 2, 3, 4, 5 };

    /// <summary>
    /// Small list with 5 elements: [1, 2, 3, 4, 5]
    /// </summary>
    public static List<int> SmallList => new List<int> { 1, 2, 3, 4, 5 };

    /// <summary>
    /// Large array with 1000 elements: [1..1000]
    /// </summary>
    public static int[] LargeArray => Enumerable.Range(1, 1000).ToArray();

    /// <summary>
    /// Large list with 1000 elements: [1..1000]
    /// </summary>
    public static List<int> LargeList => Enumerable.Range(1, 1000).ToList();

    /// <summary>
    /// Empty array
    /// </summary>
    public static int[] EmptyArray => Array.Empty<int>();

    /// <summary>
    /// Empty list
    /// </summary>
    public static List<int> EmptyList => new List<int>();

    /// <summary>
    /// Array with all even numbers: [2, 4, 6, 8]
    /// </summary>
    public static int[] AllEvenArray => new[] { 2, 4, 6, 8 };

    /// <summary>
    /// Array with all odd numbers: [1, 3, 5, 7, 9]
    /// </summary>
    public static int[] AllOddArray => new[] { 1, 3, 5, 7, 9 };

    /// <summary>
    /// Array with mixed even and odd numbers: [1, 2, 3, 4, 5, 6]
    /// </summary>
    public static int[] MixedArray => new[] { 1, 2, 3, 4, 5, 6 };

    /// <summary>
    /// List with mixed even and odd numbers: [1, 2, 3, 4, 5, 6]
    /// </summary>
    public static List<int> MixedList => new List<int> { 1, 2, 3, 4, 5, 6 };

    /// <summary>
    /// Array with duplicate elements: [1, 2, 2, 3, 2, 4]
    /// </summary>
    public static int[] ArrayWithDuplicates => new[] { 1, 2, 2, 3, 2, 4 };

    /// <summary>
    /// List with duplicate elements: [1, 2, 2, 3, 2, 4]
    /// </summary>
    public static List<int> ListWithDuplicates => new List<int> { 1, 2, 2, 3, 2, 4 };

    /// <summary>
    /// Array with negative numbers: [-5, -3, 2, 4, -1]
    /// </summary>
    public static int[] ArrayWithNegatives => new[] { -5, -3, 2, 4, -1 };

    /// <summary>
    /// Single element array: [42]
    /// </summary>
    public static int[] SingleElementArray => new[] { 42 };

    /// <summary>
    /// Single element list: [42]
    /// </summary>
    public static List<int> SingleElementList => new List<int> { 42 };
}
