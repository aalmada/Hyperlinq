using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class ValueEnumerableExtensions
    {
        /// <summary>
        /// Returns the only element of a sequence, and throws an exception if there is not exactly one element.
        /// </summary>
        public static TSource Single<TEnumerable, TEnumerator, TSource>(this TEnumerable source)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            => source.SingleOrNone<TEnumerable, TEnumerator, TSource>().Value;

        /// <summary>
        /// Returns the only element of a sequence that satisfies a specified condition, and throws an exception if more than one such element exists.
        /// </summary>
        public static TSource Single<TEnumerable, TEnumerator, TSource>(this TEnumerable source, Func<TSource, bool> predicate)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            => ValueEnumerableExtensions.Where<TEnumerable, TEnumerator, TSource>(source, predicate).Single();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Single<T>(this ArrayValueEnumerable<T> source, Func<T, bool> predicate)
            => source.Where(predicate).Single();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Single<T>(this ListValueEnumerable<T> source, Func<T, bool> predicate)
            => source.Where(predicate).Single();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Single<T>(this EnumerableValueEnumerable<T> source, Func<T, bool> predicate)
            => ValueEnumerableExtensions.Where<EnumerableValueEnumerable<T>, EnumerableValueEnumerable<T>.Enumerator, T>(source, predicate).Single();

        public static TSource Single<TSource>(this WhereEnumerable<TSource> source)
            => source.SingleOrNone().Value;

        public static TSource Single<TSource>(this WhereMemoryEnumerable<TSource> source)
            => source.SingleOrNone().Value;

        public static TSource Single<TSource>(this WhereListEnumerable<TSource> source)
            => source.SingleOrNone().Value;
    }
}
