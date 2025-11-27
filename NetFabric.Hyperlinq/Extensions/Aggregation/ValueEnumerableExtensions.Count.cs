using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class ValueEnumerableExtensions
    {
        /// <summary>
        /// Returns the number of elements in a sequence.
        /// Optimized for ICollection to use Count property.
        /// </summary>
        public static int Count<TEnumerable, TEnumerator, TSource>(this TEnumerable source)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
        {
            // Optimize for ICollection (includes IValueReadOnlyCollection implementations)
            if (source is ICollection<TSource> collection)
                return collection.Count;

            // Fallback to enumeration
            var count = 0;
            foreach (var _ in source)
                count++;
            return count;
        }

        /// <summary>
        /// Returns the number of elements in a collection.
        /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<T>(this ICollection<T> source)
            => source.Count;

        public static int Count<TEnumerable, TEnumerator, TSource>(this TEnumerable source, Func<TSource, bool> predicate)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
        {
            var count = 0;
            foreach (var item in source)
            {
                if (predicate(item))
                    count++;
            }
            return count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<T>(this ArrayValueEnumerable<T> source, Func<T, bool> predicate)
        {
            var count = 0;
            foreach (var item in source)
            {
                if (predicate(item))
                    count++;
            }
            return count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<T>(this ListValueEnumerable<T> source, Func<T, bool> predicate)
        {
            var count = 0;
            foreach (var item in source)
            {
                if (predicate(item))
                    count++;
            }
            return count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<T>(this EnumerableValueEnumerable<T> source, Func<T, bool> predicate)
        {
            var count = 0;
            foreach (var item in source)
            {
                if (predicate(item))
                    count++;
            }
            return count;
        }

        public static int Count<TSource>(this WhereEnumerable<TSource> source)
        {
            var count = 0;
            using var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
                count++;
            return count;
        }

        public static int Count<TSource>(this WhereMemoryEnumerable<TSource> source)
        {
            var count = 0;
            using var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
                count++;
            return count;
        }

        public static int Count<TSource>(this WhereListEnumerable<TSource> source)
        {
            var count = 0;
            using var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
                count++;
            return count;
        }
    }
}
