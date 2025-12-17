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
        /// Returns the maximum value in a list using SIMD acceleration.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Max()
            => CollectionsMarshal.AsSpan(source).Max();

        /// <summary>
        /// Returns the maximum value that satisfies a condition.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Max(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source).Max(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> MaxOrNone()
            => CollectionsMarshal.AsSpan(source).MaxOrNone();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> MaxOrNone(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source).MaxOrNone(predicate);
    }
}
