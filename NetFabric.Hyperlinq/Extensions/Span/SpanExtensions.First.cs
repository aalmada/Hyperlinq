using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq
{
    /// <summary>
    /// Extension methods for span-compatible types - First operation.
    /// </summary>
    public static partial class SpanExtensions
    {
        // ===== BASE IMPLEMENTATION =====
        
        /// <summary>
        /// Returns the first element of a sequence.
        /// </summary>
        public static T First<T>(this ReadOnlySpan<T> source)
        {
            if (source.Length == 0)
                throw new InvalidOperationException("Sequence contains no elements");
            return source[0];
        }
        
        // ===== DELEGATING OVERLOADS =====
        
        /// <summary>
        /// Returns the first element of a sequence.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T First<T>(this Span<T> source)
            => First((ReadOnlySpan<T>)source);
        
        /// <summary>
        /// Returns the first element of a sequence.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T First<T>(this T[] source)
            => First((ReadOnlySpan<T>)source);
        
        /// <summary>
        /// Returns the first element of a sequence.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T First<T>(this ReadOnlyMemory<T> source)
            => First(source.Span);
        
        /// <summary>
        /// Returns the first element of a sequence.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T First<T>(this Memory<T> source)
            => First(source.Span);
        
        /// <summary>
        /// Returns the first element of a sequence.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T First<T>(this List<T> source)
            => First(CollectionsMarshal.AsSpan(source));
        
        /// <summary>
        /// Returns the first element of a sequence.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T First<T>(this ArraySegment<T> source)
            => First(source.AsSpan());
    }
}
