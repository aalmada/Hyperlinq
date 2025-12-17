using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ListExtensions
{
    /// <summary>
    /// Projects each element into a new form.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SelectListEnumerable<T, TResult, TSelector> Select<T, TResult, TSelector>(this List<T> source, TSelector selector)
        where TSelector : struct, IFunction<T, TResult>
        => new SelectListEnumerable<T, TResult, TSelector>(source, selector);

    /// <summary>
    /// Projects each element into a new form.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SelectListEnumerable<T, TResult, FunctionWrapper<T, TResult>> Select<T, TResult>(this List<T> source, Func<T, TResult> selector)
        => new SelectListEnumerable<T, TResult, FunctionWrapper<T, TResult>>(source, new FunctionWrapper<T, TResult>(selector));

    /// <summary>
    /// Projects each element into a new form.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SelectListInEnumerable<T, TResult, TSelector> Select<T, TResult, TSelector>(this List<T> source, in TSelector selector)
        where TSelector : struct, IFunctionIn<T, TResult>
        => new SelectListInEnumerable<T, TResult, TSelector>(source, selector);
}
