using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ArrayExtensions
{
    extension<T>(T[] source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WhereReadOnlySpanEnumerable<T, TPredicate> Where<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => new WhereReadOnlySpanEnumerable<T, TPredicate>(source.AsSpan(), predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WhereReadOnlySpanEnumerable<T, FunctionWrapper<T, bool>> Where(Func<T, bool> predicate)
            => new WhereReadOnlySpanEnumerable<T, FunctionWrapper<T, bool>>(source.AsSpan(), new FunctionWrapper<T, bool>(predicate));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WhereReadOnlySpanInEnumerable<T, TPredicate> Where<TPredicate>(in TPredicate predicate)
            where TPredicate : struct, IFunctionIn<T, bool>
            => new WhereReadOnlySpanInEnumerable<T, TPredicate>(source.AsSpan(), predicate);
    }
}
