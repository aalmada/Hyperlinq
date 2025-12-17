using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ListExtensions
{
    /// <summary>
    /// Filters elements based on a predicate.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WhereListEnumerable<T, TPredicate> Where<T, TPredicate>(this List<T> source, TPredicate predicate)
        where TPredicate : struct, IFunction<T, bool>
        => new WhereListEnumerable<T, TPredicate>(source, predicate);

    /// <summary>
    /// Filters elements based on a predicate.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WhereListEnumerable<T, FunctionWrapper<T, bool>> Where<T>(this List<T> source, Func<T, bool> predicate)
        => new WhereListEnumerable<T, FunctionWrapper<T, bool>>(source, new FunctionWrapper<T, bool>(predicate));

    /// <summary>
    /// Filters elements based on a predicate.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WhereListInEnumerable<T, TPredicate> Where<T, TPredicate>(this List<T> source, in TPredicate predicate)
        where TPredicate : struct, IFunctionIn<T, bool>
        => new WhereListInEnumerable<T, TPredicate>(source, predicate);
}
