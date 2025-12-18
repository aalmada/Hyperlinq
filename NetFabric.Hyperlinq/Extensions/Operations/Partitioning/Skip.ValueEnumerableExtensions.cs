using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ValueEnumerableExtensions
{
    /// <summary>
    /// Bypasses a specified number of elements in a sequence and then returns the remaining elements.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SkipTakeEnumerable<TSource, TEnumerable, TEnumerator> Skip<TEnumerable, TEnumerator, TSource>(
        this TEnumerable source,
        int count)
        where TEnumerable : IValueEnumerable<TSource, TEnumerator>
        where TEnumerator : struct, IEnumerator<TSource> => new SkipTakeEnumerable<TSource, TEnumerable, TEnumerator>(source, count, int.MaxValue);
}
