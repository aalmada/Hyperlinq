using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class RepeatListExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RepeatReadOnlySpanEnumerable<TSource> Repeat<TSource>(this List<TSource> source, int count)
        => System.Runtime.InteropServices.CollectionsMarshal.AsSpan(source).Repeat(count);
}
