using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq
{
    /// <summary>
    /// Extension methods for span-compatible types - Last operation.
    /// </summary>
    public static partial class SpanExtensions
    {
        // ===== BASE IMPLEMENTATION =====
        
        /// <summary>
        /// Returns the last element of a sequence.
        /// </summary>
        public static T Last<T>(this ReadOnlySpan<T> source)
        {
            if (source.Length == 0)
                throw new InvalidOperationException("Sequence contains no elements");
            return source[^1];
        }
        
        // ===== DELEGATING OVERLOADS =====
        
        /// <summary>
        /// Returns the last element of a sequence.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Last<T>(this Span<T> source)
            => Last((ReadOnlySpan<T>)source);
        
        /// <summary>
        /// Returns the last element of a sequence.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Last<T>(this T[] source)
            => Last((ReadOnlySpan<T>)source);
        
        /// <summary>
        /// Returns the last element of a sequence.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Last<T>(this ReadOnlyMemory<T> source)
            => Last(source.Span);
        
        /// <summary>
        /// Returns the last element of a sequence.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Last<T>(this Memory<T> source)
            => Last(source.Span);
        
        /// <summary>
        /// Returns the last element of a sequence.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Last<T>(this List<T> source)
            => Last(CollectionsMarshal.AsSpan(source));
        
        /// <summary>
        /// Returns the last element of a sequence.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Last<T>(this ArraySegment<T> source)
            => Last(source.AsSpan());
    }
}
