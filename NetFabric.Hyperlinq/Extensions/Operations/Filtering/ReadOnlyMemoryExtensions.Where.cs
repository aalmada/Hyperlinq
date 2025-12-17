using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ReadOnlyMemoryExtensions
{
    extension<T>(ReadOnlyMemory<T> source)
    {
        /// <summary>
        /// Filters elements based on a predicate.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WhereReadOnlySpanEnumerable<T, FunctionWrapper<T, bool>> Where(Func<T, bool> predicate)
            => new WhereReadOnlySpanEnumerable<T, FunctionWrapper<T, bool>>(source.Span, new FunctionWrapper<T, bool>(predicate));

        /// <summary>
        /// Filters elements based on a predicate using a value delegate passed by reference.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WhereReadOnlySpanInEnumerable<T, TPredicate> Where<TPredicate>(in TPredicate predicate)
            where TPredicate : struct, IFunctionIn<T, bool>
            => new WhereReadOnlySpanInEnumerable<T, TPredicate>(source.Span, predicate);
    }
}
