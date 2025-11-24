using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq
{
    /// <summary>
    /// Extension methods for span-compatible types.
    /// Uses delegation pattern to minimize code duplication.
    /// </summary>
    public static partial class SpanExtensions
    {
        // ===== BASE IMPLEMENTATION =====
        // All other overloads delegate to this method
        
        /// <summary>
        /// Computes the sum of a sequence of numeric values using SIMD acceleration.
        /// </summary>
        public static T Sum<T>(this ReadOnlySpan<T> source)
            where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
            => System.Numerics.Tensors.TensorPrimitives.Sum<T>(source);
        
        // ===== DELEGATING OVERLOADS =====
        // Zero-copy conversions to ReadOnlySpan<T>
        
        /// <summary>
        /// Computes the sum of a sequence of numeric values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Sum<T>(this Span<T> source)
            where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
            => Sum((ReadOnlySpan<T>)source);
        
        /// <summary>
        /// Computes the sum of a sequence of numeric values using SIMD acceleration.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Sum<T>(this T[] source)
            where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
            => Sum((ReadOnlySpan<T>)source);
        
        /// <summary>
        /// Computes the sum of a sequence of numeric values using SIMD acceleration.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Sum<T>(this ReadOnlyMemory<T> source)
            where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
            => Sum(source.Span);
        
        /// <summary>
        /// Computes the sum of a sequence of numeric values using SIMD acceleration.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Sum<T>(this Memory<T> source)
            where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
            => Sum(source.Span);
        
        /// <summary>
        /// Computes the sum of a sequence of numeric values using SIMD acceleration.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Sum<T>(this List<T> source)
            where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
            => Sum(CollectionsMarshal.AsSpan(source));
        
        /// <summary>
        /// Computes the sum of a sequence of numeric values using SIMD acceleration.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Sum<T>(this ArraySegment<T> source)
            where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
            => Sum(source.AsSpan());
    }
}
