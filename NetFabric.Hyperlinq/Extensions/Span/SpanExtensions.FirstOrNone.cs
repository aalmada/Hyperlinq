using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq
{
    public static partial class SpanExtensions
    {
        /// <summary>
        /// Returns an option containing the first element of a span, or None if the span is empty.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> FirstOrNone<T>(this ReadOnlySpan<T> source)
            => source.Length == 0 ? Option<T>.None() : Option<T>.Some(source[0]);

        /// <summary>
        /// Returns an option containing the first element of an array, or None if the array is empty.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> FirstOrNone<T>(this T[] source)
            => source.Length == 0 ? Option<T>.None() : Option<T>.Some(source[0]);

        /// <summary>
        /// Returns an option containing the first element of a memory, or None if the memory is empty.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> FirstOrNone<T>(this ReadOnlyMemory<T> source)
            => source.Length == 0 ? Option<T>.None() : Option<T>.Some(source.Span[0]);

        /// <summary>
        /// Returns an option containing the first element of a memory, or None if the memory is empty.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> FirstOrNone<T>(this Memory<T> source)
            => source.Length == 0 ? Option<T>.None() : Option<T>.Some(source.Span[0]);

        /// <summary>
        /// Returns an option containing the first element of a list, or None if the list is empty.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> FirstOrNone<T>(this List<T> source)
            => source.Count == 0 ? Option<T>.None() : Option<T>.Some(source[0]);

        /// <summary>
        /// Returns an option containing the first element of an array segment, or None if the segment is empty.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> FirstOrNone<T>(this ArraySegment<T> source)
            => source.Count == 0 ? Option<T>.None() : Option<T>.Some(source[0]);
        public static Option<T> FirstOrNone<T>(this ReadOnlySpan<T> source, Func<T, bool> predicate)
        {
            foreach (var item in source)
            {
                if (predicate(item))
                    return Option<T>.Some(item);
            }
            return Option<T>.None();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> FirstOrNone<T>(this Span<T> source, Func<T, bool> predicate)
            => FirstOrNone((ReadOnlySpan<T>)source, predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> FirstOrNone<T>(this T[] source, Func<T, bool> predicate)
            => FirstOrNone(new ReadOnlySpan<T>(source), predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> FirstOrNone<T>(this List<T> source, Func<T, bool> predicate)
            => FirstOrNone(CollectionsMarshal.AsSpan(source), predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> FirstOrNone<T>(this Memory<T> source, Func<T, bool> predicate)
            => FirstOrNone(source.Span, predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> FirstOrNone<T>(this ReadOnlyMemory<T> source, Func<T, bool> predicate)
            => FirstOrNone(source.Span, predicate);
    }
}
