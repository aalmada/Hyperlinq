using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ArrayValueEnumerableExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SelectArrayEnumerable<T, TResult, TSelector> Select<T, TResult, TSelector>(this ArrayValueEnumerable<T> source, TSelector selector)
        where TSelector : struct, IFunction<T, TResult>
        => new SelectArrayEnumerable<T, TResult, TSelector>(source.Source, selector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SelectArrayEnumerable<T, TResult, FunctionWrapper<T, TResult>> Select<T, TResult>(this ArrayValueEnumerable<T> source, Func<T, TResult> selector)
        => new SelectArrayEnumerable<T, TResult, FunctionWrapper<T, TResult>>(source.Source, new FunctionWrapper<T, TResult>(selector));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SelectArrayInEnumerable<T, TResult, TSelector> Select<T, TResult, TSelector>(this ArrayValueEnumerable<T> source, in TSelector selector)
        where TSelector : struct, IFunctionIn<T, TResult>
        => new SelectArrayInEnumerable<T, TResult, TSelector>(source.Source, selector);
}
