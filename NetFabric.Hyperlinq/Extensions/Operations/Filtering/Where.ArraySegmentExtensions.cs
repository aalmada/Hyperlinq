using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ArraySegmentExtensions
{
    extension<T>(ArraySegment<T> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WhereArraySegmentEnumerable<T, TPredicate> Where<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => new WhereArraySegmentEnumerable<T, TPredicate>(source, predicate);

        /// <summary>
        /// Filters elements based on a predicate.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WhereArraySegmentEnumerable<T, FunctionWrapper<T, bool>> Where(Func<T, bool> predicate)
            => new WhereArraySegmentEnumerable<T, FunctionWrapper<T, bool>>(source, new FunctionWrapper<T, bool>(predicate));

        /// <summary>
        /// Filters elements based on a predicate using a value delegate passed by reference.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WhereArraySegmentInEnumerable<T, TPredicate> Where<TPredicate>(in TPredicate predicate)
            where TPredicate : struct, IFunctionIn<T, bool>
            => new WhereArraySegmentInEnumerable<T, TPredicate>(source, predicate);
    }
}
