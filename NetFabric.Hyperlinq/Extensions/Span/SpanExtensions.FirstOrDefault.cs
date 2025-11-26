using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class SpanExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FirstOrDefault<T>(this ReadOnlySpan<T> source)
            => source.FirstOrNone().GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FirstOrDefault<T>(this ReadOnlySpan<T> source, T defaultValue)
            => source.FirstOrNone().GetValueOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FirstOrDefault<T>(this Span<T> source)
            => source.FirstOrNone().GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FirstOrDefault<T>(this Span<T> source, T defaultValue)
            => source.FirstOrNone().GetValueOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FirstOrDefault<T>(this T[] source)
            => source.FirstOrNone().GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FirstOrDefault<T>(this T[] source, T defaultValue)
            => source.FirstOrNone().GetValueOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FirstOrDefault<T>(this ReadOnlyMemory<T> source)
            => source.FirstOrNone().GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FirstOrDefault<T>(this ReadOnlyMemory<T> source, T defaultValue)
            => source.FirstOrNone().GetValueOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FirstOrDefault<T>(this Memory<T> source)
            => source.FirstOrNone().GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FirstOrDefault<T>(this Memory<T> source, T defaultValue)
            => source.FirstOrNone().GetValueOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FirstOrDefault<T>(this List<T> source)
            => source.FirstOrNone().GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FirstOrDefault<T>(this List<T> source, T defaultValue)
            => source.FirstOrNone().GetValueOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FirstOrDefault<T>(this ArraySegment<T> source)
            => source.FirstOrNone().GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FirstOrDefault<T>(this ArraySegment<T> source, T defaultValue)
            => source.FirstOrNone().GetValueOrDefault(defaultValue);
    }
}
