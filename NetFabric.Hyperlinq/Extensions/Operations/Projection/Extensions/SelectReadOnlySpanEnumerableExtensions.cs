using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class SelectReadOnlySpanEnumerableExtensions
{
    /// <summary>
    /// Fuses consecutive Select operations by composing selectors.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SelectReadOnlySpanEnumerable<TSource, TResult, SelectorCompose<TSource, TIntermediate, TResult, TSelector, FunctionWrapper<TIntermediate, TResult>>> Select<TSource, TIntermediate, TResult, TSelector>(
        this SelectReadOnlySpanEnumerable<TSource, TIntermediate, TSelector> source,
        Func<TIntermediate, TResult> selector)
        where TSelector : struct, IFunction<TSource, TIntermediate>
    {
        var firstSelector = source.Selector;
        return new SelectReadOnlySpanEnumerable<TSource, TResult, SelectorCompose<TSource, TIntermediate, TResult, TSelector, FunctionWrapper<TIntermediate, TResult>>>(
            source.Source,
            new SelectorCompose<TSource, TIntermediate, TResult, TSelector, FunctionWrapper<TIntermediate, TResult>>(
                firstSelector,
                new FunctionWrapper<TIntermediate, TResult>(selector)));
    }

    /// <summary>
    /// Returns the minimum value after applying the selector.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult Min<TSource, TResult, TSelector>(this SelectReadOnlySpanEnumerable<TSource, TResult, TSelector> source)
        where TSelector : struct, IFunction<TSource, TResult>
        where TResult : INumber<TResult>
        => source.MinOrNone().Value;

    /// <summary>
    /// Returns the maximum value after applying the selector.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult Max<TSource, TResult, TSelector>(this SelectReadOnlySpanEnumerable<TSource, TResult, TSelector> source)
        where TSelector : struct, IFunction<TSource, TResult>
        where TResult : INumber<TResult>
        => source.MaxOrNone().Value;

    /// <summary>
    /// Computes the sum after applying the selector.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult Sum<TSource, TResult, TSelector>(this SelectReadOnlySpanEnumerable<TSource, TResult, TSelector> source)
        where TSelector : struct, IFunction<TSource, TResult>
        where TResult : IAdditionOperators<TResult, TResult, TResult>, IAdditiveIdentity<TResult, TResult>
    {
        var sum = TResult.AdditiveIdentity;
        var selector = source.Selector;
        var span = source.Source;

        foreach (ref readonly var item in span)
        {
            sum += selector.Invoke(item);
        }

        return sum;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TResult> MinOrNone<TSource, TResult, TSelector>(this SelectReadOnlySpanEnumerable<TSource, TResult, TSelector> source)
        where TSelector : struct, IFunction<TSource, TResult>
        where TResult : INumber<TResult>
    {
        var span = source.Source;
        if (span.Length == 0)
        {
            return Option<TResult>.None();
        }

        var selector = source.Selector;
        var min = selector.Invoke(span[0]);

        foreach (ref readonly var item in span[1..])
        {
            var value = selector.Invoke(item);
            if (value < min)
            {
                min = value;
            }
        }

        return Option<TResult>.Some(min);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TResult> MaxOrNone<TSource, TResult, TSelector>(this SelectReadOnlySpanEnumerable<TSource, TResult, TSelector> source)
        where TSelector : struct, IFunction<TSource, TResult>
        where TResult : INumber<TResult>
    {
        var span = source.Source;
        if (span.Length == 0)
        {
            return Option<TResult>.None();
        }

        var selector = source.Selector;
        var max = selector.Invoke(span[0]);

        foreach (ref readonly var item in span[1..])
        {
            var value = selector.Invoke(item);
            if (value > max)
            {
                max = value;
            }
        }

        return Option<TResult>.Some(max);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (TResult Min, TResult Max) MinMax<TSource, TResult, TSelector>(this SelectReadOnlySpanEnumerable<TSource, TResult, TSelector> source)
        where TSelector : struct, IFunction<TSource, TResult>
        where TResult : INumber<TResult>
        => source.MinMaxOrNone().Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<(TResult Min, TResult Max)> MinMaxOrNone<TSource, TResult, TSelector>(this SelectReadOnlySpanEnumerable<TSource, TResult, TSelector> source)
        where TSelector : struct, IFunction<TSource, TResult>
        where TResult : INumber<TResult>
    {
        var span = source.Source;
        if (span.Length == 0)
        {
            return Option<(TResult Min, TResult Max)>.None();
        }

        var selector = source.Selector;
        var value = selector.Invoke(span[0]);
        var min = value;
        var max = value;

        foreach (ref readonly var item in span[1..])
        {
            value = selector.Invoke(item);
            if (value < min)
            {
                min = value;
            }
            else if (value > max)
            {
                max = value;
            }
        }

        return Option<(TResult Min, TResult Max)>.Some((min, max));
    }
}
