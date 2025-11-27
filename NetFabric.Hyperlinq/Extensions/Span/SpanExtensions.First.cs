using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq
{
    public static partial class SpanExtensions
    {
        /// <summary>
        /// Returns the first element of a span.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T First<T>(this ReadOnlySpan<T> source)
            => source.FirstOrNone().Value;

        /// <summary>
        /// Returns the first element of a span.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T First<T>(this Span<T> source)
            => source.FirstOrNone().Value;

        /// <summary>
        /// Returns the first element of an array.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T First<T>(this T[] source)
            => source.FirstOrNone().Value;

        /// <summary>
        /// Returns the first element of a memory.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T First<T>(this ReadOnlyMemory<T> source)
            => source.FirstOrNone().Value;

        /// <summary>
        /// Returns the first element of a memory.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T First<T>(this Memory<T> source)
            => source.FirstOrNone().Value;

        /// <summary>
        /// Returns the first element of a list.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T First<T>(this List<T> source)
            => source.FirstOrNone().Value;

        /// <summary>
        /// Returns the first element of an array segment.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T First<T>(this ArraySegment<T> source)
            => source.FirstOrNone().Value;
            
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T First<T>(this ReadOnlySpan<T> source, Func<T, bool> predicate)
            => source.FirstOrNone(predicate).Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T First<T>(this Span<T> source, Func<T, bool> predicate)
            => source.FirstOrNone(predicate).Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T First<T>(this T[] source, Func<T, bool> predicate)
            => source.FirstOrNone(predicate).Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T First<T>(this List<T> source, Func<T, bool> predicate)
            => source.FirstOrNone(predicate).Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T First<T>(this Memory<T> source, Func<T, bool> predicate)
            => source.FirstOrNone(predicate).Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T First<T>(this ReadOnlyMemory<T> source, Func<T, bool> predicate)
            => source.FirstOrNone(predicate).Value;
    }
}
