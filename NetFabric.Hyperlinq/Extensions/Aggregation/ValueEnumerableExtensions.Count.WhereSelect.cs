using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq
{
    public static partial class ValueEnumerableExtensions
    {
        // ---------------------------------------------------------------------------------
        // Count
        // ---------------------------------------------------------------------------------

        /// <summary>
        /// Returns the number of elements that satisfy the predicate.
        /// Optimized to ignore the selector since Count doesn't need projected values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<TSource, TResult>(this WhereSelectEnumerable<TSource, TResult> source)
        {
            var count = 0;
            foreach (var item in source.Source)
            {
                if (source.Predicate(item))
                    count++;
            }
            return count;
        }

        /// <summary>
        /// Returns the number of elements that satisfy the predicate.
        /// Optimized to ignore the selector since Count doesn't need projected values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<TSource, TResult>(this WhereSelectMemoryEnumerable<TSource, TResult> source)
        {
            var count = 0;
            var span = source.Source.Span;
            ref var spanRef = ref MemoryMarshal.GetReference(span);
            var length = span.Length;

            for (var i = 0; i < length; i++)
            {
                if (source.Predicate(Unsafe.Add(ref spanRef, i)))
                    count++;
            }
            return count;
        }

        /// <summary>
        /// Returns the number of elements that satisfy the predicate.
        /// Optimized to ignore the selector since Count doesn't need projected values.
        /// Uses CollectionsMarshal for zero-copy access.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<TSource, TResult>(this WhereSelectListEnumerable<TSource, TResult> source)
        {
            var count = 0;
            var span = CollectionsMarshal.AsSpan(source.Source);
            ref var spanRef = ref MemoryMarshal.GetReference(span);
            var length = span.Length;

            for (var i = 0; i < length; i++)
            {
                if (source.Predicate(Unsafe.Add(ref spanRef, i)))
                    count++;
            }
            return count;
        }
    }
}
