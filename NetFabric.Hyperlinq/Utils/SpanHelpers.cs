using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

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

    /// <summary>
    /// Creates a pooled array from a source span, applying a selector function to each element.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static PooledBuffer<TResult> ToArrayPooled<TSource, TResult, TSelector>(
        ReadOnlySpan<TSource> source,
        TSelector selector,
        ArrayPool<TResult>? pool = null)
        where TSelector : struct, IFunction<TSource, TResult>
    {
        pool ??= ArrayPool<TResult>.Shared;
        var result = pool.Rent(source.Length);
        var index = 0;
        foreach (ref readonly var item in source)
        {
            result[index++] = selector.Invoke(item);
        }
        return new PooledBuffer<TResult>(result, source.Length, pool);
    }

    /// <summary>
    /// Creates a pooled array from a source span, applying a selector function (passed by reference) to each element.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static PooledBuffer<TResult> ToArrayPooledIn<TSource, TResult, TSelector>(
        ReadOnlySpan<TSource> source,
        in TSelector selector,
        ArrayPool<TResult>? pool = null)
        where TSelector : struct, IFunctionIn<TSource, TResult>
    {
        pool ??= ArrayPool<TResult>.Shared;
        var result = pool.Rent(source.Length);
        var index = 0;
        foreach (ref readonly var item in source)
        {
            result[index++] = selector.Invoke(in item);
        }
        return new PooledBuffer<TResult>(result, source.Length, pool);
    }

    /// <summary>
    /// Determines whether a span contains a specific value after applying a selector function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Contains<TSource, TResult, TSelector>(
        ReadOnlySpan<TSource> source,
        TSelector selector,
        TResult item)
        where TSelector : struct, IFunction<TSource, TResult>
    {
        foreach (ref readonly var sourceItem in source)
        {
            if (EqualityComparer<TResult>.Default.Equals(selector.Invoke(sourceItem), item))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Determines whether a span contains a specific value after applying a selector function (passed by reference).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ContainsIn<TSource, TResult, TSelector>(
        ReadOnlySpan<TSource> source,
        in TSelector selector,
        TResult item)
        where TSelector : struct, IFunctionIn<TSource, TResult>
    {
        foreach (ref readonly var sourceItem in source)
        {
            if (EqualityComparer<TResult>.Default.Equals(selector.Invoke(in sourceItem), item))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Returns the index of the first occurrence of a value after applying a selector function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOf<TSource, TResult, TSelector>(
        ReadOnlySpan<TSource> source,
        TSelector selector,
        TResult item)
        where TSelector : struct, IFunction<TSource, TResult>
    {
        var index = 0;
        foreach (ref readonly var sourceItem in source)
        {
            if (EqualityComparer<TResult>.Default.Equals(selector.Invoke(sourceItem), item))
            {
                return index;
            }

            index++;
        }
        return -1;
    }

    /// <summary>
    /// Returns the index of the first occurrence of a value after applying a selector function (passed by reference).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOfIn<TSource, TResult, TSelector>(
        ReadOnlySpan<TSource> source,
        in TSelector selector,
        TResult item)
        where TSelector : struct, IFunctionIn<TSource, TResult>
    {
        var index = 0;
        foreach (ref readonly var sourceItem in source)
        {
            if (EqualityComparer<TResult>.Default.Equals(selector.Invoke(in sourceItem), item))
            {
                return index;
            }

            index++;
        }
        return -1;
    }

    /// <summary>
    /// Creates an array from a source span, applying a selector function to each element.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult[] ToArray<TSource, TResult, TSelector>(
        ReadOnlySpan<TSource> source,
        TSelector selector)
        where TSelector : struct, IFunction<TSource, TResult>
    {
        var array = GC.AllocateUninitializedArray<TResult>(source.Length);
        var index = 0;
        foreach (ref readonly var item in source)
        {
            array[index++] = selector.Invoke(item);
        }
        return array;
    }

    /// <summary>
    /// Creates an array from a source span, applying a selector function (passed by reference) to each element.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult[] ToArrayIn<TSource, TResult, TSelector>(
        ReadOnlySpan<TSource> source,
        in TSelector selector)
        where TSelector : struct, IFunctionIn<TSource, TResult>
    {
        var array = GC.AllocateUninitializedArray<TResult>(source.Length);
        var index = 0;
        foreach (ref readonly var item in source)
        {
            array[index++] = selector.Invoke(in item);
        }
        return array;
    }
}
