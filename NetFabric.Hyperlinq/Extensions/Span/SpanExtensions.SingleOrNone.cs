using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class SpanExtensions
    {
        /// <summary>
        /// Returns an option containing the only element of a span, or None if the span is empty.
        /// Throws an exception if there is more than one element in the span.
        /// </summary>
        /// <exception cref="InvalidOperationException">The span contains more than one element.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> SingleOrNone<T>(this ReadOnlySpan<T> source)
        {
            if (source.Length == 0)
                return Option<T>.None();
            if (source.Length > 1)
                throw new InvalidOperationException("Sequence contains more than one element");
            return Option<T>.Some(source[0]);
        }

        /// <summary>
        /// Returns an option containing the only element of an array, or None if the array is empty.
        /// Throws an exception if there is more than one element in the array.
        /// </summary>
        /// <exception cref="InvalidOperationException">The array contains more than one element.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> SingleOrNone<T>(this T[] source)
        {
            if (source.Length == 0)
                return Option<T>.None();
            if (source.Length > 1)
                throw new InvalidOperationException("Sequence contains more than one element");
            return Option<T>.Some(source[0]);
        }

        /// <summary>
        /// Returns an option containing the only element of a memory, or None if the memory is empty.
        /// Throws an exception if there is more than one element in the memory.
        /// </summary>
        /// <exception cref="InvalidOperationException">The memory contains more than one element.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> SingleOrNone<T>(this ReadOnlyMemory<T> source)
        {
            if (source.Length == 0)
                return Option<T>.None();
            if (source.Length > 1)
                throw new InvalidOperationException("Sequence contains more than one element");
            return Option<T>.Some(source.Span[0]);
        }

        /// <summary>
        /// Returns an option containing the only element of a memory, or None if the memory is empty.
        /// Throws an exception if there is more than one element in the memory.
        /// </summary>
        /// <exception cref="InvalidOperationException">The memory contains more than one element.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> SingleOrNone<T>(this Memory<T> source)
        {
            if (source.Length == 0)
                return Option<T>.None();
            if (source.Length > 1)
                throw new InvalidOperationException("Sequence contains more than one element");
            return Option<T>.Some(source.Span[0]);
        }

        /// <summary>
        /// Returns an option containing the only element of a list, or None if the list is empty.
        /// Throws an exception if there is more than one element in the list.
        /// </summary>
        /// <exception cref="InvalidOperationException">The list contains more than one element.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> SingleOrNone<T>(this List<T> source)
        {
            if (source.Count == 0)
                return Option<T>.None();
            if (source.Count > 1)
                throw new InvalidOperationException("Sequence contains more than one element");
            return Option<T>.Some(source[0]);
        }

        /// <summary>
        /// Returns an option containing the only element of an array segment, or None if the segment is empty.
        /// Throws an exception if there is more than one element in the segment.
        /// </summary>
        /// <exception cref="InvalidOperationException">The segment contains more than one element.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> SingleOrNone<T>(this ArraySegment<T> source)
        {
            if (source.Count == 0)
                return Option<T>.None();
            if (source.Count > 1)
                throw new InvalidOperationException("Sequence contains more than one element");
            return Option<T>.Some(source[0]);
        }
    }
}
