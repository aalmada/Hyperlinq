using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ArrayValueEnumerableExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WhereReadOnlySpanEnumerable<T, TPredicate> Where<T, TPredicate>(this ArrayValueEnumerable<T> source, TPredicate predicate)
        where TPredicate : struct, IFunction<T, bool>
        => new WhereReadOnlySpanEnumerable<T, TPredicate>(source.Source, predicate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WhereReadOnlySpanInEnumerable<T, TPredicate> Where<T, TPredicate>(this ArrayValueEnumerable<T> source, in TPredicate predicate)
        where TPredicate : struct, IFunctionIn<T, bool>
        => new WhereReadOnlySpanInEnumerable<T, TPredicate>(source.Source, predicate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WhereReadOnlySpanEnumerable<T, FunctionWrapper<T, bool>> Where<T>(this ArrayValueEnumerable<T> source, Func<T, bool> predicate)
        => new WhereReadOnlySpanEnumerable<T, FunctionWrapper<T, bool>>(source.Source, new FunctionWrapper<T, bool>(predicate));
}
