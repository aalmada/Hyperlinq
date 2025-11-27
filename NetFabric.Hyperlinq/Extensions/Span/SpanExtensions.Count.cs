using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq
{
    /// <summary>
    /// Extension methods for span-compatible types - Count operation.
    /// </summary>
    public static partial class SpanExtensions
    {
        // ===== BASE IMPLEMENTATION =====
        
        /// <summary>
        /// Returns the number of elements in a sequence.
        /// </summary>
        public static int Count<T>(this ReadOnlySpan<T> source)
            => source.Length;
        
        // ===== DELEGATING OVERLOADS =====
        
        /// <summary>
        /// Returns the number of elements in a sequence.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<T>(this Span<T> source)
            => source.Length;
        
        /// <summary>
        /// Returns the number of elements in a sequence.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<T>(this T[] source)
            => source.Length;
        
        /// <summary>
        /// Returns the number of elements in a sequence.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<T>(this ReadOnlyMemory<T> source)
            => source.Length;
        
        /// <summary>
        /// Returns the number of elements in a sequence.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<T>(this Memory<T> source)
            => source.Length;
        
        /// <summary>
        /// Returns the number of elements in a sequence.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<T>(this List<T> source)
            => source.Count;
        
        /// <summary>
        /// Returns the number of elements in a sequence.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<T>(this ArraySegment<T> source)
            => source.Count;
        public static int Count<T>(this ReadOnlySpan<T> source, Func<T, bool> predicate)
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
        public static int Count<T>(this Span<T> source, Func<T, bool> predicate)
            => Count((ReadOnlySpan<T>)source, predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<T>(this T[] source, Func<T, bool> predicate)
            => Count(new ReadOnlySpan<T>(source), predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<T>(this List<T> source, Func<T, bool> predicate)
            => Count(CollectionsMarshal.AsSpan(source), predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<T>(this Memory<T> source, Func<T, bool> predicate)
            => Count(source.Span, predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<T>(this ReadOnlyMemory<T> source, Func<T, bool> predicate)
            => Count(source.Span, predicate);
    }
}
