using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ReadOnlySpanExtensions
{
    extension<T>(ReadOnlySpan<T> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SelectReadOnlySpanEnumerable<T, TResult, TSelector> Select<TResult, TSelector>(TSelector selector)
            where TSelector : struct, IFunction<T, TResult>
            => new SelectReadOnlySpanEnumerable<T, TResult, TSelector>(source, selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SelectReadOnlySpanEnumerable<T, TResult, FunctionWrapper<T, TResult>> Select<TResult>(Func<T, TResult> selector)
            => new SelectReadOnlySpanEnumerable<T, TResult, FunctionWrapper<T, TResult>>(source, new FunctionWrapper<T, TResult>(selector));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SelectReadOnlySpanInEnumerable<T, TResult, TSelector> Select<TResult, TSelector>(in TSelector selector)
            where TSelector : struct, IFunctionIn<T, TResult>
            => new SelectReadOnlySpanInEnumerable<T, TResult, TSelector>(source, selector);
    }
}
