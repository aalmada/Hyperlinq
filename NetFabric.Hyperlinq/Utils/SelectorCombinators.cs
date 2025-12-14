using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

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

public readonly struct SelectorComposeIn<TSource, TIntermediate, TResult, TFirst, TSecond>
    : IFunctionIn<TSource, TResult>, IFunction<TSource, TResult>
    where TFirst : struct, IFunctionIn<TSource, TIntermediate>
    where TSecond : struct, IFunctionIn<TIntermediate, TResult>
{
    readonly TFirst first;
    readonly TSecond second;

    public SelectorComposeIn(in TFirst first, in TSecond second)
    {
        this.first = first;
        this.second = second;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TResult Invoke(in TSource item)
        => second.Invoke(first.Invoke(in item));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TResult Invoke(TSource item)
        => second.Invoke(first.Invoke(in item));
}
