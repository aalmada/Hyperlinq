using System;
using System.Buffers;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class WhereReadOnlySpanEnumerableExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WhereSelectReadOnlySpanEnumerable<TSource, TResult> Select<TSource, TResult>(this WhereReadOnlySpanEnumerable<TSource> source, Func<TSource, TResult> selector)
            => new WhereSelectReadOnlySpanEnumerable<TSource, TResult>(source.Source, source.Predicate, selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<TSource>(this WhereReadOnlySpanEnumerable<TSource> source)
            => source.Source.Count(source.Predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<TSource>(this WhereReadOnlySpanEnumerable<TSource> source)
            => source.Source.Any(source.Predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSource First<TSource>(this WhereReadOnlySpanEnumerable<TSource> source)
            => source.Source.First(source.Predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<TSource> FirstOrNone<TSource>(this WhereReadOnlySpanEnumerable<TSource> source)
            => source.Source.FirstOrNone(source.Predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSource Single<TSource>(this WhereReadOnlySpanEnumerable<TSource> source)
            => source.Source.Single(source.Predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<TSource> SingleOrNone<TSource>(this WhereReadOnlySpanEnumerable<TSource> source)
            => source.Source.SingleOrNone(source.Predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSource Last<TSource>(this WhereReadOnlySpanEnumerable<TSource> source)
            => source.Source.Last(source.Predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<TSource> LastOrNone<TSource>(this WhereReadOnlySpanEnumerable<TSource> source)
            => source.Source.LastOrNone(source.Predicate);

        public static TSource[] ToArray<TSource>(this WhereReadOnlySpanEnumerable<TSource> source)
            => source.Source.ToArray(source.Predicate);

        public static List<TSource> ToList<TSource>(this WhereReadOnlySpanEnumerable<TSource> source)
            => source.Source.ToList(source.Predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PooledBuffer<TSource> ToArrayPooled<TSource>(this WhereReadOnlySpanEnumerable<TSource> source)
            => source.Source.ToArrayPooled(source.Predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PooledBuffer<TSource> ToArrayPooled<TSource>(this WhereReadOnlySpanEnumerable<TSource> source, ArrayPool<TSource>? pool)
            => source.Source.ToArrayPooled(source.Predicate, pool);



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSource Sum<TSource>(this WhereReadOnlySpanEnumerable<TSource> source)
            where TSource : IAdditionOperators<TSource, TSource, TSource>, IAdditiveIdentity<TSource, TSource>
        {
            var sum = TSource.AdditiveIdentity;
            foreach (var item in source)
            {
                sum += item;
            }
            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSource Min<TSource>(this WhereReadOnlySpanEnumerable<TSource> source)
            where TSource : INumber<TSource>
            => source.Source.Min(source.Predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSource Max<TSource>(this WhereReadOnlySpanEnumerable<TSource> source)
            where TSource : INumber<TSource>
            => source.Source.Max(source.Predicate);
    }
}
