using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq;

public static partial class ListExtensions
{
    extension<T>(List<T> source)
        where T : struct, INumber<T>, IMinMaxValue<T>
    {
        /// <summary>
        /// Returns the minimum value in a list using SIMD acceleration.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Min()
            => CollectionsMarshal.AsSpan(source).Min();

        /// <summary>
        /// Returns the minimum value that satisfies a condition.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Min(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source).Min(predicate);

        /// <summary>
        /// Computes both minimum and maximum values in a single iteration.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (T Min, T Max) MinMax()
            => CollectionsMarshal.AsSpan(source).MinMax();

        /// <summary>
        /// Computes both minimum and maximum values for elements that satisfy a condition.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (T Min, T Max) MinMax(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source).MinMax(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> MinOrNone()
            => CollectionsMarshal.AsSpan(source).MinOrNone();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> MinOrNone(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source).MinOrNone(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<(T Min, T Max)> MinMaxOrNone()
            => CollectionsMarshal.AsSpan(source).MinMaxOrNone();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<(T Min, T Max)> MinMaxOrNone(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source).MinMaxOrNone(predicate);
    }
}
