using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class SpanExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SingleOrDefault<T>(this ReadOnlySpan<T> source)
            => source.SingleOrNone().GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SingleOrDefault<T>(this ReadOnlySpan<T> source, T defaultValue)
            => source.SingleOrNone().GetValueOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SingleOrDefault<T>(this Span<T> source)
            => source.SingleOrNone().GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SingleOrDefault<T>(this Span<T> source, T defaultValue)
            => source.SingleOrNone().GetValueOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SingleOrDefault<T>(this T[] source)
            => source.SingleOrNone().GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SingleOrDefault<T>(this T[] source, T defaultValue)
            => source.SingleOrNone().GetValueOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SingleOrDefault<T>(this ReadOnlyMemory<T> source)
            => source.SingleOrNone().GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SingleOrDefault<T>(this ReadOnlyMemory<T> source, T defaultValue)
            => source.SingleOrNone().GetValueOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SingleOrDefault<T>(this Memory<T> source)
            => source.SingleOrNone().GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SingleOrDefault<T>(this Memory<T> source, T defaultValue)
            => source.SingleOrNone().GetValueOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SingleOrDefault<T>(this List<T> source)
            => source.SingleOrNone().GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SingleOrDefault<T>(this List<T> source, T defaultValue)
            => source.SingleOrNone().GetValueOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SingleOrDefault<T>(this ArraySegment<T> source)
            => source.SingleOrNone().GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SingleOrDefault<T>(this ArraySegment<T> source, T defaultValue)
            => source.SingleOrNone().GetValueOrDefault(defaultValue);
    }
}
