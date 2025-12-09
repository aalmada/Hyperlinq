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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSource[] ToArray<TSource, TPredicate>(this WhereReadOnlySpanInEnumerable<TSource, TPredicate> source)
            where TPredicate : struct, IFunctionIn<TSource, bool>
        {
            var predicate = source.Predicate;
            return source.Source.ToArray(in predicate);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<TSource> ToList<TSource, TPredicate>(this WhereReadOnlySpanInEnumerable<TSource, TPredicate> source)
            where TPredicate : struct, IFunctionIn<TSource, bool>
        {
            var predicate = source.Predicate;
            return source.Source.ToList(in predicate);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PooledBuffer<TSource> ToArrayPooled<TSource, TPredicate>(this WhereReadOnlySpanInEnumerable<TSource, TPredicate> source)
            where TPredicate : struct, IFunctionIn<TSource, bool>
        {
            var predicate = source.Predicate;
            return source.Source.ToArrayPooled(in predicate);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PooledBuffer<TSource> ToArrayPooled<TSource, TPredicate>(this WhereReadOnlySpanInEnumerable<TSource, TPredicate> source, ArrayPool<TSource>? pool)
            where TPredicate : struct, IFunctionIn<TSource, bool>
        {
            var predicate = source.Predicate;
            return source.Source.ToArrayPooled(in predicate, pool);
        }
    }
}
