using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ListValueEnumerableExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WhereListEnumerable<T, FunctionWrapper<T, bool>> Where<T>(this ListValueEnumerable<T> source, Func<T, bool> predicate)
        => new WhereListEnumerable<T, FunctionWrapper<T, bool>>(source.Source, new FunctionWrapper<T, bool>(predicate));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WhereListInEnumerable<T, TPredicate> Where<T, TPredicate>(this ListValueEnumerable<T> source, in TPredicate predicate)
        where TPredicate : struct, IFunctionIn<T, bool>
        => new WhereListInEnumerable<T, TPredicate>(source.Source, predicate);
}
