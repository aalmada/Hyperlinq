using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ReadOnlyMemoryExtensions
{
    extension<T>(ReadOnlyMemory<T> source)
        where T : struct, INumber<T>, IMinMaxValue<T>
    {
        /// <summary>
        /// Returns the maximum value in a memory using SIMD acceleration.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Max()
            => source.Span.Max();

        /// <summary>
        /// Returns the maximum value that satisfies a condition.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Max(Func<T, bool> predicate)
            => source.Span.Max(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> MaxOrNone()
            => source.Span.MaxOrNone();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> MaxOrNone(Func<T, bool> predicate)
            => source.Span.MaxOrNone(predicate);
    }
}
