using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class RepeatReadOnlyMemoryExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RepeatReadOnlySpanEnumerable<TSource> Repeat<TSource>(this ReadOnlyMemory<TSource> source, int count)
        => source.Span.Repeat(count);
}
