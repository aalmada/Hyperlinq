using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq;

public static partial class WhereReadOnlyMemoryEnumerableExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WhereSelectReadOnlyMemoryEnumerable<TSource, TResult, TPredicate, TSelector> Select<TSource, TResult, TPredicate, TSelector>(this WhereReadOnlyMemoryEnumerable<TSource, TPredicate> source, TSelector selector)
        where TPredicate : struct, IFunction<TSource, bool>
        where TSelector : struct, IFunction<TSource, TResult>
        => new(source.Source, source.Predicate, selector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WhereSelectReadOnlyMemoryEnumerable<TSource, TResult, TPredicate, FunctionWrapper<TSource, TResult>> Select<TSource, TResult, TPredicate>(this WhereReadOnlyMemoryEnumerable<TSource, TPredicate> source, Func<TSource, TResult> selector)
        where TPredicate : struct, IFunction<TSource, bool>
        => new(source.Source, source.Predicate, new FunctionWrapper<TSource, TResult>(selector));
        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WhereReadOnlyMemoryEnumerable<TSource, PredicateAnd<TSource, TPredicate, TNewPredicate>> Where<TSource, TPredicate, TNewPredicate>(this WhereReadOnlyMemoryEnumerable<TSource, TPredicate> source, TNewPredicate predicate)
        where TPredicate : struct, IFunction<TSource, bool>
        where TNewPredicate : struct, IFunction<TSource, bool>
        => new(source.Source, new PredicateAnd<TSource, TPredicate, TNewPredicate>(source.Predicate, predicate));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WhereReadOnlyMemoryEnumerable<TSource, PredicateAnd<TSource, TPredicate, FunctionWrapper<TSource, bool>>> Where<TSource, TPredicate>(this WhereReadOnlyMemoryEnumerable<TSource, TPredicate> source, Func<TSource, bool> predicate)
        where TPredicate : struct, IFunction<TSource, bool>
        => new(source.Source, new PredicateAnd<TSource, TPredicate, FunctionWrapper<TSource, bool>>(source.Predicate, new FunctionWrapper<TSource, bool>(predicate)));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TSource Sum<TSource, TPredicate>(this WhereReadOnlyMemoryEnumerable<TSource, TPredicate> source)
        where TSource : struct, INumberBase<TSource>
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.Span.Sum(source.Predicate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TSource Min<TSource, TPredicate>(this WhereReadOnlyMemoryEnumerable<TSource, TPredicate> source)
        where TSource : struct, INumber<TSource>, IMinMaxValue<TSource>
        where TPredicate : struct, IFunction<TSource, bool>
        => source.MinOrNone<TSource, TPredicate>().Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TSource> MinOrNone<TSource, TPredicate>(this WhereReadOnlyMemoryEnumerable<TSource, TPredicate> source)
        where TSource : struct, INumber<TSource>, IMinMaxValue<TSource>
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.Span.MinOrNone(source.Predicate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TSource Max<TSource, TPredicate>(this WhereReadOnlyMemoryEnumerable<TSource, TPredicate> source)
        where TSource : struct, INumber<TSource>, IMinMaxValue<TSource>
        where TPredicate : struct, IFunction<TSource, bool>
        => source.MaxOrNone<TSource, TPredicate>().Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TSource> MaxOrNone<TSource, TPredicate>(this WhereReadOnlyMemoryEnumerable<TSource, TPredicate> source)
        where TSource : struct, INumber<TSource>, IMinMaxValue<TSource>
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.Span.MaxOrNone(source.Predicate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (TSource Min, TSource Max) MinMax<TSource, TPredicate>(this WhereReadOnlyMemoryEnumerable<TSource, TPredicate> source)
        where TSource : struct, INumber<TSource>, IMinMaxValue<TSource>
        where TPredicate : struct, IFunction<TSource, bool>
        => source.MinMaxOrNone<TSource, TPredicate>().Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<(TSource Min, TSource Max)> MinMaxOrNone<TSource, TPredicate>(this WhereReadOnlyMemoryEnumerable<TSource, TPredicate> source)
        where TSource : struct, INumber<TSource>, IMinMaxValue<TSource>
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.Span.MinMaxOrNone(source.Predicate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Count<TSource, TPredicate>(this WhereReadOnlyMemoryEnumerable<TSource, TPredicate> source)
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.Span.Count(source.Predicate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Any<TSource, TPredicate>(this WhereReadOnlyMemoryEnumerable<TSource, TPredicate> source)
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.Span.Any(source.Predicate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TSource First<TSource, TPredicate>(this WhereReadOnlyMemoryEnumerable<TSource, TPredicate> source)
        where TPredicate : struct, IFunction<TSource, bool>
        => source.FirstOrNone().Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TSource> FirstOrNone<TSource, TPredicate>(this WhereReadOnlyMemoryEnumerable<TSource, TPredicate> source)
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.Span.FirstOrNone(source.Predicate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TSource Single<TSource, TPredicate>(this WhereReadOnlyMemoryEnumerable<TSource, TPredicate> source)
        where TPredicate : struct, IFunction<TSource, bool>
        => source.SingleOrNone().Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TSource> SingleOrNone<TSource, TPredicate>(this WhereReadOnlyMemoryEnumerable<TSource, TPredicate> source)
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.Span.SingleOrNone(source.Predicate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TSource Last<TSource, TPredicate>(this WhereReadOnlyMemoryEnumerable<TSource, TPredicate> source)
        where TPredicate : struct, IFunction<TSource, bool>
        => source.LastOrNone().Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TSource> LastOrNone<TSource, TPredicate>(this WhereReadOnlyMemoryEnumerable<TSource, TPredicate> source)
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.Span.LastOrNone(source.Predicate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TSource[] ToArray<TSource, TPredicate>(this WhereReadOnlyMemoryEnumerable<TSource, TPredicate> source)
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.Span.ToArray(source.Predicate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static List<TSource> ToList<TSource, TPredicate>(this WhereReadOnlyMemoryEnumerable<TSource, TPredicate> source)
        where TPredicate : struct, IFunction<TSource, bool>
        => source.Source.Span.ToList(source.Predicate);
}
