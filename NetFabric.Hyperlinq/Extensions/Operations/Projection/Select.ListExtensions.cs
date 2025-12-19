using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ListExtensions
{
    extension<T>(List<T> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SelectListEnumerable<T, TResult, TSelector> Select<TResult, TSelector>(TSelector selector)
            where TSelector : struct, IFunction<T, TResult>
            => new SelectListEnumerable<T, TResult, TSelector>(source, selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SelectListEnumerable<T, TResult, FunctionWrapper<T, TResult>> Select<TResult>(Func<T, TResult> selector)
            => new SelectListEnumerable<T, TResult, FunctionWrapper<T, TResult>>(source, new FunctionWrapper<T, TResult>(selector));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SelectListInEnumerable<T, TResult, TSelector> Select<TResult, TSelector>(in TSelector selector)
            where TSelector : struct, IFunctionIn<T, TResult>
            => new SelectListInEnumerable<T, TResult, TSelector>(source, selector);
    }
}
