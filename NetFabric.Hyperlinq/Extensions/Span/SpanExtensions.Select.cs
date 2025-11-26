using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq
{
    /// <summary>
    /// Extension methods for span-compatible types - Select operation.
    /// </summary>
    public static partial class SpanExtensions
    {
        // ===== BASE IMPLEMENTATION (Memory-based) =====
        
        /// <summary>
        /// Projects each element of a sequence into a new form.
        /// </summary>
        public static SelectMemoryEnumerable<TSource, TResult> Select<TSource, TResult>(
            this ReadOnlyMemory<TSource> source,
            Func<TSource, TResult> selector)
            => new SelectMemoryEnumerable<TSource, TResult>(source, selector);
        
        // ===== DELEGATING OVERLOADS =====
        
        /// <summary>
        /// Projects each element of a sequence into a new form.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SelectMemoryEnumerable<TSource, TResult> Select<TSource, TResult>(
            this TSource[] source,
            Func<TSource, TResult> selector)
            => Select((ReadOnlyMemory<TSource>)source, selector);
        
        /// <summary>
        /// Projects each element of a sequence into a new form.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SelectMemoryEnumerable<TSource, TResult> Select<TSource, TResult>(
            this Memory<TSource> source,
            Func<TSource, TResult> selector)
            => Select((ReadOnlyMemory<TSource>)source, selector);
        
        /// <summary>
        /// Projects each element of a sequence into a new form.
        /// Uses SelectListEnumerable for optimal List performance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SelectListEnumerable<TSource, TResult> Select<TSource, TResult>(
            this List<TSource> source,
            Func<TSource, TResult> selector)
            => new SelectListEnumerable<TSource, TResult>(source, selector);
        
        /// <summary>
        /// Projects each element of a sequence into a new form.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SelectMemoryEnumerable<TSource, TResult> Select<TSource, TResult>(
            this ArraySegment<TSource> source,
            Func<TSource, TResult> selector)
            => Select(source.AsMemory(), selector);
    }
}
