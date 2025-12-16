using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq;

public static partial class WhereListInEnumerableExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TSource[] ToArray<TSource, TPredicate>(this WhereListInEnumerable<TSource, TPredicate> source)
        where TPredicate : struct, IFunctionIn<TSource, bool>
    {
        var predicate = source.Predicate;
        return CollectionsMarshal.AsSpan(source.Source).ToArray(in predicate);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static List<TSource> ToList<TSource, TPredicate>(this WhereListInEnumerable<TSource, TPredicate> source)
        where TPredicate : struct, IFunctionIn<TSource, bool>
    {
        var predicate = source.Predicate;
        return CollectionsMarshal.AsSpan(source.Source).ToList(in predicate);
    }
}
