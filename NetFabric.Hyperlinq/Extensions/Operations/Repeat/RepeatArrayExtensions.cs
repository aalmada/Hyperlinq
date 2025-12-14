using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class RepeatArrayExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RepeatArrayEnumerable<TSource> Repeat<TSource>(this TSource[] source, int count)
    {
        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count));
        }

        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        return new RepeatArrayEnumerable<TSource>(source, count);
    }
}
