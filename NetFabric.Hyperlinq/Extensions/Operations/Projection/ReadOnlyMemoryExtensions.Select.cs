using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ReadOnlyMemoryExtensions
{
    extension<T>(ReadOnlyMemory<T> source)
    {
        /// <summary>
        /// Projects each element into a new form.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SelectReadOnlySpanEnumerable<T, TResult, FunctionWrapper<T, TResult>> Select<TResult>(Func<T, TResult> selector)
            => new SelectReadOnlySpanEnumerable<T, TResult, FunctionWrapper<T, TResult>>(source.Span, new FunctionWrapper<T, TResult>(selector));

        /// <summary>
        /// Projects each element into a new form using a value delegate passed by reference.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SelectReadOnlySpanInEnumerable<T, TResult, TSelector> Select<TResult, TSelector>(in TSelector selector)
            where TSelector : struct, IFunctionIn<T, TResult>
            => new SelectReadOnlySpanInEnumerable<T, TResult, TSelector>(source.Span, selector);
    }
}
