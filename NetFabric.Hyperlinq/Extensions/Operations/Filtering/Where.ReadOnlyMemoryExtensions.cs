using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ReadOnlyMemoryExtensions
{
    extension<T>(ReadOnlyMemory<T> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WhereReadOnlyMemoryEnumerable<T, TPredicate> Where<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => new WhereReadOnlyMemoryEnumerable<T, TPredicate>(source, predicate);

        /// <summary>
        /// Filters elements based on a predicate.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WhereReadOnlyMemoryEnumerable<T, FunctionWrapper<T, bool>> Where(Func<T, bool> predicate)
            => new WhereReadOnlyMemoryEnumerable<T, FunctionWrapper<T, bool>>(source, new FunctionWrapper<T, bool>(predicate));

        /// <summary>
        /// Filters elements based on a predicate using a value delegate passed by reference.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WhereReadOnlyMemoryInEnumerable<T, TPredicate> Where<TPredicate>(in TPredicate predicate)
            where TPredicate : struct, IFunctionIn<T, bool>
            => new WhereReadOnlyMemoryInEnumerable<T, TPredicate>(source, predicate);
    }
}
