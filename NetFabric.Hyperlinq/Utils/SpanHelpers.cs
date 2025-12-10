using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    /// <summary>
    /// Provides helper methods for span operations.
    /// </summary>
    static class SpanHelpers
    {
        /// <summary>
        /// Copies elements from a source span to a destination span, applying a selector function to each element.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the source span.</typeparam>
        /// <typeparam name="TResult">The type of elements in the destination span.</typeparam>
        /// <typeparam name="TSelector">The type of the selector function.</typeparam>
        /// <param name="source">The source span to copy from.</param>
        /// <param name="selector">The selector function to apply to each element.</param>
        /// <param name="destination">The destination span to copy to.</param>
        /// <param name="arrayIndex">The zero-based index in the destination at which copying begins.</param>
        /// <remarks>
        /// This method uses different iteration strategies based on arrayIndex:
        /// - When arrayIndex is 0, uses foreach for better performance
        /// - When arrayIndex > 0, uses for loop to enable JIT bounds check elimination
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo<TSource, TResult, TSelector>(
            ReadOnlySpan<TSource> source,
            TSelector selector,
            Span<TResult> destination,
            int arrayIndex)
            where TSelector : struct, IFunction<TSource, TResult>
        {
            if (arrayIndex == 0)
            {
                // Fast path: copy to start of array using foreach
                var index = 0;
                foreach (ref readonly var item in source)
                {
                    destination[index++] = selector.Invoke(item);
                }
            }
            else
            {
                // Offset copy: use for loop for JIT optimization
                var destinationLength = destination.Length - arrayIndex;
                var length = source.Length < destinationLength ? source.Length : destinationLength;
                for (var i = 0; i < length; i++)
                {
                    destination[arrayIndex + i] = selector.Invoke(source[i]);
                }
            }
        }

        /// <summary>
        /// Copies elements from a source span to a destination span, applying a selector function (passed by reference) to each element.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the source span.</typeparam>
        /// <typeparam name="TResult">The type of elements in the destination span.</typeparam>
        /// <typeparam name="TSelector">The type of the selector function.</typeparam>
        /// <param name="source">The source span to copy from.</param>
        /// <param name="selector">The selector function to apply to each element, passed by reference.</param>
        /// <param name="destination">The destination span to copy to.</param>
        /// <param name="arrayIndex">The zero-based index in the destination at which copying begins.</param>
        /// <remarks>
        /// This method uses different iteration strategies based on arrayIndex:
        /// - When arrayIndex is 0, uses foreach for better performance
        /// - When arrayIndex > 0, uses for loop to enable JIT bounds check elimination
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyToIn<TSource, TResult, TSelector>(
            ReadOnlySpan<TSource> source,
            in TSelector selector,
            Span<TResult> destination,
            int arrayIndex)
            where TSelector : struct, IFunctionIn<TSource, TResult>
        {
            if (arrayIndex == 0)
            {
                // Fast path: copy to start of array using foreach
                var index = 0;
                foreach (ref readonly var item in source)
                {
                    destination[index++] = selector.Invoke(in item);
                }
            }
            else
            {
                // Offset copy: use for loop for JIT optimization
                var destinationLength = destination.Length - arrayIndex;
                var length = source.Length < destinationLength ? source.Length : destinationLength;
                for (var i = 0; i < length; i++)
                {
                    destination[arrayIndex + i] = selector.Invoke(in source[i]);
                }
            }
        }
    }
}
