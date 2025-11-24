using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq
{
    /// <summary>
    /// Extension methods for span-compatible types - Where operation.
    /// </summary>
    public static partial class SpanExtensions
    {
        // ===== BASE IMPLEMENTATION (Memory-based) =====
        
        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        public static WhereMemoryEnumerable<T> Where<T>(
            this ReadOnlyMemory<T> source,
            Func<T, bool> predicate)
            => new WhereMemoryEnumerable<T>(source, predicate);
        
        // ===== DELEGATING OVERLOADS =====
        
        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WhereMemoryEnumerable<T> Where<T>(
            this T[] source,
            Func<T, bool> predicate)
            => Where((ReadOnlyMemory<T>)source, predicate);
        
        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WhereMemoryEnumerable<T> Where<T>(
            this Memory<T> source,
            Func<T, bool> predicate)
            => Where((ReadOnlyMemory<T>)source, predicate);
        
        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// Uses WhereListEnumerable for optimal List performance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WhereListEnumerable<T> Where<T>(
            this List<T> source,
            Func<T, bool> predicate)
            => new WhereListEnumerable<T>(source, predicate);
        
        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WhereMemoryEnumerable<T> Where<T>(
            this ArraySegment<T> source,
            Func<T, bool> predicate)
            => Where(source.AsMemory(), predicate);
    }
}
