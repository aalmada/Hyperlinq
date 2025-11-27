using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq
{
    public static partial class SpanExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Single<T>(this ReadOnlySpan<T> source, Func<T, bool> predicate)
            => source.SingleOrNone(predicate).Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Single<T>(this Span<T> source, Func<T, bool> predicate)
            => source.SingleOrNone(predicate).Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Single<T>(this T[] source, Func<T, bool> predicate)
            => source.SingleOrNone(predicate).Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Single<T>(this List<T> source, Func<T, bool> predicate)
            => source.SingleOrNone(predicate).Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Single<T>(this Memory<T> source, Func<T, bool> predicate)
            => source.SingleOrNone(predicate).Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Single<T>(this ReadOnlyMemory<T> source, Func<T, bool> predicate)
            => source.SingleOrNone(predicate).Value;
    }
}
