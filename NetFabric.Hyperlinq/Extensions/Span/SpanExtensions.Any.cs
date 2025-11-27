using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq
{
    /// <summary>
    /// Extension methods for span-compatible types - Any operation.
    /// </summary>
    public static partial class SpanExtensions
    {
        // ===== BASE IMPLEMENTATION =====
        
        /// <summary>
        /// Determines whether a sequence contains any elements.
        /// </summary>
        public static bool Any<T>(this ReadOnlySpan<T> source)
            => source.Length > 0;
        
        // ===== DELEGATING OVERLOADS =====
        
        /// <summary>
        /// Determines whether a sequence contains any elements.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<T>(this Span<T> source)
            => source.Length > 0;
        
        /// <summary>
        /// Determines whether a sequence contains any elements.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<T>(this T[] source)
            => source.Length > 0;
        
        /// <summary>
        /// Determines whether a sequence contains any elements.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<T>(this ReadOnlyMemory<T> source)
            => source.Length > 0;
        
        /// <summary>
        /// Determines whether a sequence contains any elements.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<T>(this Memory<T> source)
            => source.Length > 0;
        
        /// <summary>
        /// Determines whether a sequence contains any elements.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<T>(this List<T> source)
            => source.Count > 0;
        
        /// <summary>
        /// Determines whether a sequence contains any elements.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<T>(this ArraySegment<T> source)
            => source.Count > 0;
        public static bool Any<T>(this ReadOnlySpan<T> source, Func<T, bool> predicate)
        {
            foreach (var item in source)
            {
                if (predicate(item))
                    return true;
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<T>(this Span<T> source, Func<T, bool> predicate)
            => Any((ReadOnlySpan<T>)source, predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<T>(this T[] source, Func<T, bool> predicate)
            => Any(new ReadOnlySpan<T>(source), predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<T>(this List<T> source, Func<T, bool> predicate)
            => Any(CollectionsMarshal.AsSpan(source), predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<T>(this Memory<T> source, Func<T, bool> predicate)
            => Any(source.Span, predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<T>(this ReadOnlyMemory<T> source, Func<T, bool> predicate)
            => Any(source.Span, predicate);
    }
}
