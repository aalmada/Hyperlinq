using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class ValueEnumerableExtensions
    {
        /// <summary>
        /// Determines whether a sequence contains any elements.
        /// Optimized for ICollection to use Count property.
        /// </summary>
        public static bool Any<TEnumerable, TEnumerator, TSource>(this TEnumerable source)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
        {
            // Optimize for ICollection (includes IValueReadOnlyCollection implementations)
            if (source is ICollection<TSource> collection)
                return collection.Count != 0;

            // Fallback to enumeration
            using var enumerator = source.GetEnumerator();
            return enumerator.MoveNext();
        }

        /// <summary>
        /// Determines whether a collection contains any elements.
        /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<T>(this ICollection<T> source)
            => source.Count != 0;

        public static bool Any<TEnumerable, TEnumerator, TSource>(this TEnumerable source, Func<TSource, bool> predicate)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            => source.Where(predicate).Any();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<T>(this ArrayValueEnumerable<T> source, Func<T, bool> predicate)
            => ValueEnumerableExtensions.Where<ArrayValueEnumerable<T>, ArrayValueEnumerable<T>.Enumerator, T>(source, predicate).Any();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<T>(this ListValueEnumerable<T> source, Func<T, bool> predicate)
            => ValueEnumerableExtensions.Where<ListValueEnumerable<T>, List<T>.Enumerator, T>(source, predicate).Any();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<T>(this EnumerableValueEnumerable<T> source, Func<T, bool> predicate)
            => ValueEnumerableExtensions.Where<EnumerableValueEnumerable<T>, EnumerableValueEnumerable<T>.Enumerator, T>(source, predicate).Any();

        public static bool Any<TSource>(this WhereEnumerable<TSource> source)
        {
            using var enumerator = source.GetEnumerator();
            return enumerator.MoveNext();
        }

        public static bool Any<TSource>(this WhereMemoryEnumerable<TSource> source)
        {
            using var enumerator = source.GetEnumerator();
            return enumerator.MoveNext();
        }

        public static bool Any<TSource>(this WhereListEnumerable<TSource> source)
        {
            using var enumerator = source.GetEnumerator();
            return enumerator.MoveNext();
        }
    }
}
