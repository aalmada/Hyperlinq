using System;
using System.Buffers;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class WhereReadOnlySpanInEnumerableExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSource Sum<TSource, TPredicate>(this WhereReadOnlySpanInEnumerable<TSource, TPredicate> source)
            where TSource : IAdditionOperators<TSource, TSource, TSource>, IAdditiveIdentity<TSource, TSource>
            where TPredicate : struct, IFunctionIn<TSource, bool>
            => source.Source.Sum(source.Predicate);
    }
}
