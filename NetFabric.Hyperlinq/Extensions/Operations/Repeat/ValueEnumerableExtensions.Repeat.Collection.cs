using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ValueEnumerableExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RepeatCollectionEnumerable<TEnumerator, TSource> Repeat<TEnumerator, TSource>(this IValueReadOnlyCollection<TSource, TEnumerator> source, int count)
        where TEnumerator : struct, IEnumerator<TSource>
    {
        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count));
        }

        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        return new RepeatCollectionEnumerable<TEnumerator, TSource>(source, count);
    }
}
