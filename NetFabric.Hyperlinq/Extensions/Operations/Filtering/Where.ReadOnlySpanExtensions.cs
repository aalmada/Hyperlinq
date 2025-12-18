using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ReadOnlySpanExtensions
{
    extension<T>(ReadOnlySpan<T> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WhereReadOnlySpanEnumerable<T, TPredicate> Where<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => new WhereReadOnlySpanEnumerable<T, TPredicate>(source, predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WhereReadOnlySpanEnumerable<T, FunctionWrapper<T, bool>> Where(Func<T, bool> predicate)
            => new WhereReadOnlySpanEnumerable<T, FunctionWrapper<T, bool>>(source, new FunctionWrapper<T, bool>(predicate));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WhereReadOnlySpanInEnumerable<T, TPredicate> Where<TPredicate>(in TPredicate predicate)
            where TPredicate : struct, IFunctionIn<T, bool>
            => new WhereReadOnlySpanInEnumerable<T, TPredicate>(source, predicate);
    }
}
