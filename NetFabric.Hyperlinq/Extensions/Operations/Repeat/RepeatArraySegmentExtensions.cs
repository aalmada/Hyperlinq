using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class RepeatArraySegmentExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RepeatReadOnlySpanEnumerable<TSource> Repeat<TSource>(this ArraySegment<TSource> source, int count)
        => source.AsSpan().Repeat(count);
}
