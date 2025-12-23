using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class RepeatMemoryExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RepeatReadOnlySpanEnumerable<TSource> Repeat<TSource>(this Memory<TSource> source, int count)
        => ((ReadOnlyMemory<TSource>)source).Repeat(count);
}
