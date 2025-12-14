using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public readonly struct PredicateAnd<T, TPre1, TPre2>
    : IFunction<T, bool>
    where TPre1 : struct, IFunction<T, bool>
    where TPre2 : struct, IFunction<T, bool>
{
    readonly TPre1 first;
    readonly TPre2 second;

    public PredicateAnd(TPre1 first, TPre2 second)
    {
        this.first = first;
        this.second = second;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Invoke(T item)
        => first.Invoke(item) && second.Invoke(item);
}

public readonly struct PredicateAndIn<T, TPre1, TPre2>
    : IFunctionIn<T, bool>, IFunction<T, bool>
    where TPre1 : struct, IFunctionIn<T, bool>
    where TPre2 : struct, IFunctionIn<T, bool>
{
    readonly TPre1 first;
    readonly TPre2 second;

    public PredicateAndIn(in TPre1 first, in TPre2 second)
    {
        this.first = first;
        this.second = second;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Invoke(in T item)
        => first.Invoke(in item) && second.Invoke(in item);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Invoke(T item)
        => first.Invoke(in item) && second.Invoke(in item);
}
