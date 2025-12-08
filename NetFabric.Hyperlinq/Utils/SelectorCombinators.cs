using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public readonly struct SelectorCompose<TSource, TIntermediate, TResult, TFirst, TSecond>
        : IFunction<TSource, TResult>
        where TFirst : struct, IFunction<TSource, TIntermediate>
        where TSecond : struct, IFunction<TIntermediate, TResult>
    {
        readonly TFirst first;
        readonly TSecond second;

        public SelectorCompose(TFirst first, TSecond second)
        {
            this.first = first;
            this.second = second;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TResult Invoke(TSource item)
            => second.Invoke(first.Invoke(item));
    }
}
