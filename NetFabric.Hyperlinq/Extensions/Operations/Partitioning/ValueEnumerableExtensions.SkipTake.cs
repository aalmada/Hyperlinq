using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ValueEnumerableExtensions
{
    /// <summary>
    /// Chains Take after Skip on a SkipTakeEnumerable, updating the take count.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SkipTakeEnumerable<TSource, TEnumerable, TEnumerator> Take<TSource, TEnumerable, TEnumerator>(
        this SkipTakeEnumerable<TSource, TEnumerable, TEnumerator> source,
        int count)
        where TEnumerable : IValueEnumerable<TSource, TEnumerator>
        where TEnumerator : struct, IEnumerator<TSource> => new SkipTakeEnumerable<TSource, TEnumerable, TEnumerator>(
            source.Source,
            source.SkipCount,
            count);

    /// <summary>
    /// Chains Skip after Take on a SkipTakeEnumerable, updating the skip count.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SkipTakeEnumerable<TSource, TEnumerable, TEnumerator> Skip<TSource, TEnumerable, TEnumerator>(
        this SkipTakeEnumerable<TSource, TEnumerable, TEnumerator> source,
        int count)
        where TEnumerable : IValueEnumerable<TSource, TEnumerator>
        where TEnumerator : struct, IEnumerator<TSource> => new SkipTakeEnumerable<TSource, TEnumerable, TEnumerator>(
            source.Source,
            count,
            source.TakeCount);
}
