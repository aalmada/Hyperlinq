using System;
using System.Buffers;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq;

public static partial class WhereArrayEnumerableExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WhereArrayEnumerable<TSource, PredicateAnd<TSource, TPredicate, FunctionWrapper<TSource, bool>>> Where<TSource, TPredicate>(
        this WhereArrayEnumerable<TSource, TPredicate> source,
        Func<TSource, bool> predicate)
        where TPredicate : struct, IFunction<TSource, bool>
        => new WhereArrayEnumerable<TSource, PredicateAnd<TSource, TPredicate, FunctionWrapper<TSource, bool>>>(
            source.Source,
            new PredicateAnd<TSource, TPredicate, FunctionWrapper<TSource, bool>>(
                source.Predicate,
                new FunctionWrapper<TSource, bool>(predicate)));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WhereSelectArrayEnumerable<TSource, TResult, TPredicate, TSelector> Select<TSource, TResult, TPredicate, TSelector>(this WhereArrayEnumerable<TSource, TPredicate> source, TSelector selector)
        where TPredicate : struct, IFunction<TSource, bool>
        where TSelector : struct, IFunction<TSource, TResult>
        => new WhereSelectArrayEnumerable<TSource, TResult, TPredicate, TSelector>(source.Source, source.Predicate, selector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WhereSelectArrayEnumerable<TSource, TResult, TPredicate, FunctionWrapper<TSource, TResult>> Select<TSource, TResult, TPredicate>(this WhereArrayEnumerable<TSource, TPredicate> source, Func<TSource, TResult> selector)
        where TPredicate : struct, IFunction<TSource, bool>
        => new WhereSelectArrayEnumerable<TSource, TResult, TPredicate, FunctionWrapper<TSource, TResult>>(source.Source, source.Predicate, new FunctionWrapper<TSource, TResult>(selector));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TSource Sum<TSource, TPredicate>(this WhereArrayEnumerable<TSource, TPredicate> source)
        where TSource : struct, INumberBase<TSource>
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.AsSpan().Sum(source.Predicate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TSource Min<TSource, TPredicate>(this WhereArrayEnumerable<TSource, TPredicate> source)
        where TSource : struct, INumber<TSource>, IMinMaxValue<TSource>
        where TPredicate : struct, IFunction<TSource, bool>
        => source.MinOrNone<TSource, TPredicate>().Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TSource> MinOrNone<TSource, TPredicate>(this WhereArrayEnumerable<TSource, TPredicate> source)
        where TSource : struct, INumber<TSource>, IMinMaxValue<TSource>
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.AsSpan().MinOrNone(source.Predicate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TSource Max<TSource, TPredicate>(this WhereArrayEnumerable<TSource, TPredicate> source)
        where TSource : struct, INumber<TSource>, IMinMaxValue<TSource>
        where TPredicate : struct, IFunction<TSource, bool>
        => source.MaxOrNone<TSource, TPredicate>().Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TSource> MaxOrNone<TSource, TPredicate>(this WhereArrayEnumerable<TSource, TPredicate> source)
        where TSource : struct, INumber<TSource>, IMinMaxValue<TSource>
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.AsSpan().MaxOrNone(source.Predicate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (TSource Min, TSource Max) MinMax<TSource, TPredicate>(this WhereArrayEnumerable<TSource, TPredicate> source)
        where TSource : struct, INumber<TSource>, IMinMaxValue<TSource>
        where TPredicate : struct, IFunction<TSource, bool>
        => source.MinMaxOrNone<TSource, TPredicate>().Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<(TSource Min, TSource Max)> MinMaxOrNone<TSource, TPredicate>(this WhereArrayEnumerable<TSource, TPredicate> source)
        where TSource : struct, INumber<TSource>, IMinMaxValue<TSource>
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.AsSpan().MinMaxOrNone(source.Predicate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Count<TSource, TPredicate>(this WhereArrayEnumerable<TSource, TPredicate> source)
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.AsSpan().Count(source.Predicate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Any<TSource, TPredicate>(this WhereArrayEnumerable<TSource, TPredicate> source)
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.AsSpan().Any(source.Predicate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TSource First<TSource, TPredicate>(this WhereArrayEnumerable<TSource, TPredicate> source)
        where TPredicate : struct, IFunction<TSource, bool>
        => source.FirstOrNone().Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TSource> FirstOrNone<TSource, TPredicate>(this WhereArrayEnumerable<TSource, TPredicate> source)
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.AsSpan().FirstOrNone(source.Predicate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TSource Single<TSource, TPredicate>(this WhereArrayEnumerable<TSource, TPredicate> source)
        where TPredicate : struct, IFunction<TSource, bool>
        => source.SingleOrNone().Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TSource> SingleOrNone<TSource, TPredicate>(this WhereArrayEnumerable<TSource, TPredicate> source)
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.AsSpan().SingleOrNone(source.Predicate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TSource Last<TSource, TPredicate>(this WhereArrayEnumerable<TSource, TPredicate> source)
        where TPredicate : struct, IFunction<TSource, bool>
        => source.LastOrNone().Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TSource> LastOrNone<TSource, TPredicate>(this WhereArrayEnumerable<TSource, TPredicate> source)
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.AsSpan().LastOrNone(source.Predicate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TSource[] ToArray<TSource, TPredicate>(this WhereArrayEnumerable<TSource, TPredicate> source)
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.AsSpan().ToArray(source.Predicate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static List<TSource> ToList<TSource, TPredicate>(this WhereArrayEnumerable<TSource, TPredicate> source)
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.AsSpan().ToList(source.Predicate);
}
