using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class RepeatReadOnlySpanExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RepeatReadOnlySpanEnumerable<TSource> Repeat<TSource>(this ReadOnlySpan<TSource> source, int count)
    {
        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count));
        }

        return new RepeatReadOnlySpanEnumerable<TSource>(source, count);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RepeatReadOnlySpanEnumerable<TSource> Repeat<TSource>(this Span<TSource> source, int count)
    {
        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count));
        }

        return new RepeatReadOnlySpanEnumerable<TSource>(source, count);
    }
}
