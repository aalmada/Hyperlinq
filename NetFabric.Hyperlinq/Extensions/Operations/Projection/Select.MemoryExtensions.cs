using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class MemoryExtensions
{
    extension<T>(Memory<T> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SelectReadOnlySpanEnumerable<T, TResult, TSelector> Select<TResult, TSelector>(TSelector selector)
            where TSelector : struct, IFunction<T, TResult>
            => source.Span.Select<T, TResult, TSelector>(selector);

        /// <summary>
        /// Projects each element into a new form.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SelectReadOnlySpanEnumerable<T, TResult, FunctionWrapper<T, TResult>> Select<TResult>(Func<T, TResult> selector)
            => source.Span.Select(selector);

        /// <summary>
        /// Projects each element into a new form using a value delegate passed by reference.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SelectReadOnlySpanInEnumerable<T, TResult, TSelector> Select<TResult, TSelector>(in TSelector selector)
            where TSelector : struct, IFunctionIn<T, TResult>
            => source.Span.Select<T, TResult, TSelector>(in selector);
    }
}
