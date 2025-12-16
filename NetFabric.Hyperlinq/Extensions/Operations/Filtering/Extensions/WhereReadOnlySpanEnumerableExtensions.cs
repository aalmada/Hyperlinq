using System;
using System.Buffers;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class WhereReadOnlySpanEnumerableExtensions
{
    /// <summary>
    /// Fuses consecutive Where operations by combining predicates with AND logic.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WhereReadOnlySpanEnumerable<TSource, PredicateAnd<TSource, TPredicate, TNewPredicate>> Where<TSource, TPredicate, TNewPredicate>(this WhereReadOnlySpanEnumerable<TSource, TPredicate> source, TNewPredicate predicate)
        where TPredicate : struct, IFunction<TSource, bool>
        where TNewPredicate : struct, IFunction<TSource, bool>
        => new(source.Source, new PredicateAnd<TSource, TPredicate, TNewPredicate>(source.Predicate, predicate));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WhereReadOnlySpanEnumerable<TSource, PredicateAnd<TSource, TPredicate, FunctionWrapper<TSource, bool>>> Where<TSource, TPredicate>(
        this WhereReadOnlySpanEnumerable<TSource, TPredicate> source,
        Func<TSource, bool> predicate)
        where TPredicate : struct, IFunction<TSource, bool> => new WhereReadOnlySpanEnumerable<TSource, PredicateAnd<TSource, TPredicate, FunctionWrapper<TSource, bool>>>(
            source.Source,
            new PredicateAnd<TSource, TPredicate, FunctionWrapper<TSource, bool>>(
                source.Predicate,
                new FunctionWrapper<TSource, bool>(predicate)));
    public static WhereSelectReadOnlySpanEnumerable<TSource, TResult, TPredicate, TSelector> Select<TSource, TResult, TPredicate, TSelector>(this WhereReadOnlySpanEnumerable<TSource, TPredicate> source, TSelector selector)
        where TPredicate : struct, IFunction<TSource, bool>
        where TSelector : struct, IFunction<TSource, TResult>
        => new WhereSelectReadOnlySpanEnumerable<TSource, TResult, TPredicate, TSelector>(source.Source, source.Predicate, selector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WhereSelectReadOnlySpanEnumerable<TSource, TResult, TPredicate, FunctionWrapper<TSource, TResult>> Select<TSource, TResult, TPredicate>(this WhereReadOnlySpanEnumerable<TSource, TPredicate> source, Func<TSource, TResult> selector)
        where TPredicate : struct, IFunction<TSource, bool>
        => new WhereSelectReadOnlySpanEnumerable<TSource, TResult, TPredicate, FunctionWrapper<TSource, TResult>>(source.Source, source.Predicate, new FunctionWrapper<TSource, TResult>(selector));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Count<TSource, TPredicate>(this WhereReadOnlySpanEnumerable<TSource, TPredicate> source)
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.Count(source.Predicate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Any<TSource, TPredicate>(this WhereReadOnlySpanEnumerable<TSource, TPredicate> source)
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.Any(source.Predicate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TSource First<TSource, TPredicate>(this WhereReadOnlySpanEnumerable<TSource, TPredicate> source)
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.First(source.Predicate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TSource> FirstOrNone<TSource, TPredicate>(this WhereReadOnlySpanEnumerable<TSource, TPredicate> source)
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.FirstOrNone(source.Predicate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TSource Single<TSource, TPredicate>(this WhereReadOnlySpanEnumerable<TSource, TPredicate> source)
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.Single(source.Predicate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TSource> SingleOrNone<TSource, TPredicate>(this WhereReadOnlySpanEnumerable<TSource, TPredicate> source)
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.SingleOrNone(source.Predicate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TSource Last<TSource, TPredicate>(this WhereReadOnlySpanEnumerable<TSource, TPredicate> source)
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.Last(source.Predicate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TSource> LastOrNone<TSource, TPredicate>(this WhereReadOnlySpanEnumerable<TSource, TPredicate> source)
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.LastOrNone(source.Predicate);

    public static TSource[] ToArray<TSource, TPredicate>(this WhereReadOnlySpanEnumerable<TSource, TPredicate> source)
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.ToArray(source.Predicate);

    public static List<TSource> ToList<TSource, TPredicate>(this WhereReadOnlySpanEnumerable<TSource, TPredicate> source)
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.ToList(source.Predicate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]


    public static TSource Sum<TSource, TPredicate>(this WhereReadOnlySpanEnumerable<TSource, TPredicate> source)
        where TSource : struct, INumberBase<TSource>
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.Sum(source.Predicate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TSource Min<TSource, TPredicate>(this WhereReadOnlySpanEnumerable<TSource, TPredicate> source)
        where TSource : struct, INumber<TSource>, IMinMaxValue<TSource>
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.Min(source.Predicate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TSource Max<TSource, TPredicate>(this WhereReadOnlySpanEnumerable<TSource, TPredicate> source)
        where TSource : struct, INumber<TSource>, IMinMaxValue<TSource>
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.Max(source.Predicate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TSource> MinOrNone<TSource, TPredicate>(this WhereReadOnlySpanEnumerable<TSource, TPredicate> source)
        where TSource : struct, INumber<TSource>, IMinMaxValue<TSource>
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.MinOrNone(source.Predicate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TSource> MaxOrNone<TSource, TPredicate>(this WhereReadOnlySpanEnumerable<TSource, TPredicate> source)
        where TSource : struct, INumber<TSource>, IMinMaxValue<TSource>
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.MaxOrNone(source.Predicate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (TSource Min, TSource Max) MinMax<TSource, TPredicate>(this WhereReadOnlySpanEnumerable<TSource, TPredicate> source)
        where TSource : struct, INumber<TSource>, IMinMaxValue<TSource>
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.MinMax(source.Predicate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<(TSource Min, TSource Max)> MinMaxOrNone<TSource, TPredicate>(this WhereReadOnlySpanEnumerable<TSource, TPredicate> source)
        where TSource : struct, INumber<TSource>, IMinMaxValue<TSource>
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.MinMaxOrNone(source.Predicate);
}
