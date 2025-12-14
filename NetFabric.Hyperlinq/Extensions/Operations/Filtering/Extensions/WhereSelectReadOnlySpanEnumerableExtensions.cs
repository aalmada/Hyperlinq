using System;
using System.Buffers;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class WhereSelectReadOnlySpanEnumerableExtensions
{
    /// <summary>
    /// Fuses consecutive Where operations by combining predicates with AND logic.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WhereSelectReadOnlySpanEnumerable<TSource, TResult, PredicateAnd<TSource, TPredicate, SelectorCompose<TSource, TResult, bool, TSelector, FunctionWrapper<TResult, bool>>>, TSelector> Where<TSource, TResult, TPredicate, TSelector>(
        this WhereSelectReadOnlySpanEnumerable<TSource, TResult, TPredicate, TSelector> source,
        Func<TResult, bool> predicate)
        where TPredicate : struct, IFunction<TSource, bool>
        where TSelector : struct, IFunction<TSource, TResult> => new WhereSelectReadOnlySpanEnumerable<TSource, TResult, PredicateAnd<TSource, TPredicate, SelectorCompose<TSource, TResult, bool, TSelector, FunctionWrapper<TResult, bool>>>, TSelector>(
            source.Source,
            new PredicateAnd<TSource, TPredicate, SelectorCompose<TSource, TResult, bool, TSelector, FunctionWrapper<TResult, bool>>>(
                source.Predicate,
                new SelectorCompose<TSource, TResult, bool, TSelector, FunctionWrapper<TResult, bool>>(
                    source.Selector,
                    new FunctionWrapper<TResult, bool>(predicate))),
            source.Selector
        );

    /// <summary>
    /// Computes the sum of a WhereSelectReadOnlySpanEnumerable for numeric result types.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult Sum<TSource, TResult, TPredicate, TSelector>(this WhereSelectReadOnlySpanEnumerable<TSource, TResult, TPredicate, TSelector> source)
        where TResult : IAdditionOperators<TResult, TResult, TResult>, IAdditiveIdentity<TResult, TResult>
        where TPredicate : struct, IFunction<TSource, bool>
        where TSelector : struct, IFunction<TSource, TResult>
    {
        var sum = TResult.AdditiveIdentity;
        foreach (var item in source)
        {
            sum += item;
        }
        return sum;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Count<TSource, TResult, TPredicate, TSelector>(this WhereSelectReadOnlySpanEnumerable<TSource, TResult, TPredicate, TSelector> source)
        where TPredicate : struct, IFunction<TSource, bool>
        where TSelector : struct, IFunction<TSource, TResult>
    {
        var count = 0;
        var span = source.Source;
        var predicate = source.Predicate;
        foreach (var item in span)
        {
            var result = predicate.Invoke(item);
            count += Unsafe.As<bool, byte>(ref result);
        }
        return count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Any<TSource, TResult, TPredicate, TSelector>(this WhereSelectReadOnlySpanEnumerable<TSource, TResult, TPredicate, TSelector> source)
        where TPredicate : struct, IFunction<TSource, bool>
        where TSelector : struct, IFunction<TSource, TResult>
    {
        var span = source.Source;
        var predicate = source.Predicate;
        foreach (var item in span)
        {
            if (predicate.Invoke(item))
            {
                return true;
            }
        }
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult First<TSource, TResult, TPredicate, TSelector>(this WhereSelectReadOnlySpanEnumerable<TSource, TResult, TPredicate, TSelector> source)
        where TPredicate : struct, IFunction<TSource, bool>
        where TSelector : struct, IFunction<TSource, TResult>
        => FirstOrNone(source).Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult FirstOrDefault<TSource, TResult, TPredicate, TSelector>(this WhereSelectReadOnlySpanEnumerable<TSource, TResult, TPredicate, TSelector> source)
        where TPredicate : struct, IFunction<TSource, bool>
        where TSelector : struct, IFunction<TSource, TResult>
        => FirstOrNone(source).GetValueOrDefault();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult FirstOrDefault<TSource, TResult, TPredicate, TSelector>(this WhereSelectReadOnlySpanEnumerable<TSource, TResult, TPredicate, TSelector> source, TResult defaultValue)
        where TPredicate : struct, IFunction<TSource, bool>
        where TSelector : struct, IFunction<TSource, TResult>
        => FirstOrNone(source).GetValueOrDefault(defaultValue);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TResult> FirstOrNone<TSource, TResult, TPredicate, TSelector>(this WhereSelectReadOnlySpanEnumerable<TSource, TResult, TPredicate, TSelector> source)
        where TPredicate : struct, IFunction<TSource, bool>
        where TSelector : struct, IFunction<TSource, TResult>
    {
        var span = source.Source;
        var predicate = source.Predicate;
        var selector = source.Selector;
        foreach (var item in span)
        {
            if (predicate.Invoke(item))
            {
                return Option<TResult>.Some(selector.Invoke(item));
            }
        }
        return Option<TResult>.None();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult Single<TSource, TResult, TPredicate, TSelector>(this WhereSelectReadOnlySpanEnumerable<TSource, TResult, TPredicate, TSelector> source)
        where TPredicate : struct, IFunction<TSource, bool>
        where TSelector : struct, IFunction<TSource, TResult>
        => SingleOrNone(source).Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult SingleOrDefault<TSource, TResult, TPredicate, TSelector>(this WhereSelectReadOnlySpanEnumerable<TSource, TResult, TPredicate, TSelector> source)
        where TPredicate : struct, IFunction<TSource, bool>
        where TSelector : struct, IFunction<TSource, TResult>
        => SingleOrNone(source).GetValueOrDefault();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult SingleOrDefault<TSource, TResult, TPredicate, TSelector>(this WhereSelectReadOnlySpanEnumerable<TSource, TResult, TPredicate, TSelector> source, TResult defaultValue)
        where TPredicate : struct, IFunction<TSource, bool>
        where TSelector : struct, IFunction<TSource, TResult>
        => SingleOrNone(source).GetValueOrDefault(defaultValue);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TResult> SingleOrNone<TSource, TResult, TPredicate, TSelector>(this WhereSelectReadOnlySpanEnumerable<TSource, TResult, TPredicate, TSelector> source)
        where TPredicate : struct, IFunction<TSource, bool>
        where TSelector : struct, IFunction<TSource, TResult>
    {
        var span = source.Source;
        var predicate = source.Predicate;
        var selector = source.Selector;
        var found = false;
        var result = default(TResult);
        foreach (var item in span)
        {
            if (predicate.Invoke(item))
            {
                if (found)
                {
                    throw new InvalidOperationException("Sequence contains more than one matching element");
                }

                found = true;
                result = selector.Invoke(item);
            }
        }
        return found ? Option<TResult>.Some(result!) : Option<TResult>.None();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult Min<TSource, TResult, TPredicate, TSelector>(this WhereSelectReadOnlySpanEnumerable<TSource, TResult, TPredicate, TSelector> source)
        where TResult : INumber<TResult>
        where TPredicate : struct, IFunction<TSource, bool>
        where TSelector : struct, IFunction<TSource, TResult>
        => source.MinOrNone<TSource, TResult, TPredicate, TSelector>().Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TResult> MinOrNone<TSource, TResult, TPredicate, TSelector>(this WhereSelectReadOnlySpanEnumerable<TSource, TResult, TPredicate, TSelector> source)
        where TResult : INumber<TResult>
        where TPredicate : struct, IFunction<TSource, bool>
        where TSelector : struct, IFunction<TSource, TResult>
    {
        var span = source.Source;
        var predicate = source.Predicate;
        var selector = source.Selector;

        // Find first matching element
        foreach (var item in span)
        {
            if (predicate.Invoke(item))
            {
                var min = selector.Invoke(item);

                // Process remaining elements
                foreach (var remaining in span)
                {
                    if (predicate.Invoke(remaining))
                    {
                        var value = selector.Invoke(remaining);
                        if (value < min)
                        {
                            min = value;
                        }
                    }
                }

                return Option<TResult>.Some(min);
            }
        }

        return Option<TResult>.None();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult Max<TSource, TResult, TPredicate, TSelector>(this WhereSelectReadOnlySpanEnumerable<TSource, TResult, TPredicate, TSelector> source)
        where TResult : INumber<TResult>
        where TPredicate : struct, IFunction<TSource, bool>
        where TSelector : struct, IFunction<TSource, TResult>
        => source.MaxOrNone<TSource, TResult, TPredicate, TSelector>().Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TResult> MaxOrNone<TSource, TResult, TPredicate, TSelector>(this WhereSelectReadOnlySpanEnumerable<TSource, TResult, TPredicate, TSelector> source)
        where TResult : INumber<TResult>
        where TPredicate : struct, IFunction<TSource, bool>
        where TSelector : struct, IFunction<TSource, TResult>
    {
        var span = source.Source;
        var predicate = source.Predicate;
        var selector = source.Selector;

        // Find first matching element
        foreach (var item in span)
        {
            if (predicate.Invoke(item))
            {
                var max = selector.Invoke(item);

                // Process remaining elements
                foreach (var remaining in span)
                {
                    if (predicate.Invoke(remaining))
                    {
                        var value = selector.Invoke(remaining);
                        if (value > max)
                        {
                            max = value;
                        }
                    }
                }

                return Option<TResult>.Some(max);
            }
        }

        return Option<TResult>.None();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (TResult Min, TResult Max) MinMax<TSource, TResult, TPredicate, TSelector>(this WhereSelectReadOnlySpanEnumerable<TSource, TResult, TPredicate, TSelector> source)
        where TResult : INumber<TResult>
        where TPredicate : struct, IFunction<TSource, bool>
        where TSelector : struct, IFunction<TSource, TResult>
        => source.MinMaxOrNone<TSource, TResult, TPredicate, TSelector>().Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<(TResult Min, TResult Max)> MinMaxOrNone<TSource, TResult, TPredicate, TSelector>(this WhereSelectReadOnlySpanEnumerable<TSource, TResult, TPredicate, TSelector> source)
        where TResult : INumber<TResult>
        where TPredicate : struct, IFunction<TSource, bool>
        where TSelector : struct, IFunction<TSource, TResult>
    {
        var span = source.Source;
        var predicate = source.Predicate;
        var selector = source.Selector;

        // Find first matching element
        for (var i = 0; i < span.Length; i++)
        {
            if (predicate.Invoke(span[i]))
            {
                var value = selector.Invoke(span[i]);
                var min = value;
                var max = value;

                // Process remaining elements
                for (i++; i < span.Length; i++)
                {
                    if (predicate.Invoke(span[i]))
                    {
                        value = selector.Invoke(span[i]);
                        if (value < min)
                        {
                            min = value;
                        }
                        else if (value > max)
                        {
                            max = value;
                        }
                    }
                }

                return Option<(TResult Min, TResult Max)>.Some((min, max));
            }
        }

        return Option<(TResult Min, TResult Max)>.None();
    }

    public static TResult[] ToArray<TSource, TResult, TPredicate, TSelector>(this WhereSelectReadOnlySpanEnumerable<TSource, TResult, TPredicate, TSelector> source, ArrayPool<TResult>? pool = default)
        where TPredicate : struct, IFunction<TSource, bool>
        where TSelector : struct, IFunction<TSource, TResult>
    {
        using var builder = new ArrayBuilder<TResult>(pool ?? ArrayPool<TResult>.Shared);
        var span = source.Source;
        var predicate = source.Predicate;
        var selector = source.Selector;
        for (var i = 0; i < span.Length; i++)
        {
            if (predicate.Invoke(span[i]))
            {
                builder.Add(selector.Invoke(span[i]));
            }
        }
        return builder.ToArray();
    }

    public static List<TResult> ToList<TSource, TResult, TPredicate, TSelector>(this WhereSelectReadOnlySpanEnumerable<TSource, TResult, TPredicate, TSelector> source)
        where TPredicate : struct, IFunction<TSource, bool>
        where TSelector : struct, IFunction<TSource, TResult>
    {
        var list = new List<TResult>();
        var span = source.Source;
        var predicate = source.Predicate;
        var selector = source.Selector;
        foreach (var item in span)
        {
            if (predicate.Invoke(item))
            {
                list.Add(selector.Invoke(item));
            }
        }
        return list;
    }

    public static PooledBuffer<TResult> ToArrayPooled<TSource, TResult, TPredicate, TSelector>(this WhereSelectReadOnlySpanEnumerable<TSource, TResult, TPredicate, TSelector> source)
        where TPredicate : struct, IFunction<TSource, bool>
        where TSelector : struct, IFunction<TSource, TResult>
        => source.ToArrayPooled((ArrayPool<TResult>?)null);

    public static PooledBuffer<TResult> ToArrayPooled<TSource, TResult, TPredicate, TSelector>(this WhereSelectReadOnlySpanEnumerable<TSource, TResult, TPredicate, TSelector> source, ArrayPool<TResult>? pool)
        where TPredicate : struct, IFunction<TSource, bool>
        where TSelector : struct, IFunction<TSource, TResult>
    {
        using var builder = new ArrayBuilder<TResult>(pool ?? ArrayPool<TResult>.Shared);
        var span = source.Source;
        var predicate = source.Predicate;
        var selector = source.Selector;
        for (var i = 0; i < span.Length; i++)
        {
            if (predicate.Invoke(span[i]))
            {
                builder.Add(selector.Invoke(span[i]));
            }
        }
        return builder.ToPooledBuffer();
    }


}
