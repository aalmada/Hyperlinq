using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ReadOnlyMemoryExtensions
{
    extension<T>(ReadOnlyMemory<T> source)
        where T : struct, INumberBase<T>
    {
        /// <summary>
        /// Computes the sum of a sequence of numeric values using SIMD acceleration.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Sum()
            => source.Span.Sum();

        /// <summary>
        /// Computes the sum of elements that satisfy a condition.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Sum(Func<T, bool> predicate)
            => source.Span.Sum(predicate);
    }
}
