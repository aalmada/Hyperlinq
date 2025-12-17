using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ListValueEnumerableExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SelectListEnumerable<T, TResult, FunctionWrapper<T, TResult>> Select<T, TResult>(this ListValueEnumerable<T> source, Func<T, TResult> selector)
        => new SelectListEnumerable<T, TResult, FunctionWrapper<T, TResult>>(source.Source, new FunctionWrapper<T, TResult>(selector));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SelectListInEnumerable<T, TResult, TSelector> Select<T, TResult, TSelector>(this ListValueEnumerable<T> source, in TSelector selector)
        where TSelector : struct, IFunctionIn<T, TResult>
        => new SelectListInEnumerable<T, TResult, TSelector>(source.Source, selector);
}
