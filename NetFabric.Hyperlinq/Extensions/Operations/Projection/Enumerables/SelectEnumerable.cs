using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public readonly struct SelectEnumerable<TSource, TResult> : IValueEnumerable<TResult, SelectEnumerable<TSource, TResult>.Enumerator>
{
    readonly IEnumerable<TSource> source;
    readonly Func<TSource, TResult> selector;

    public SelectEnumerable(IEnumerable<TSource> source, Func<TSource, TResult> selector)
    {
        this.source = source;
        this.selector = selector;
    }

    public Enumerator GetEnumerator() => new Enumerator(source.GetEnumerator(), selector);
    IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator() => GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public PooledBuffer<TResult> ToArrayPooled(ArrayPool<TResult>? pool = null)
    {
        using var builder = new ArrayBuilder<TResult>(pool ?? ArrayPool<TResult>.Shared);
        using var enumerator = GetEnumerator();
        while (enumerator.MoveNext())
        {
            builder.Add(enumerator.Current);
        }
        return builder.ToPooledBuffer();
    }

    public struct Enumerator : IEnumerator<TResult>
    {
        readonly IEnumerator<TSource> sourceEnumerator;
        readonly Func<TSource, TResult> selector;

        public Enumerator(IEnumerator<TSource> sourceEnumerator, Func<TSource, TResult> selector)
        {
            this.sourceEnumerator = sourceEnumerator;
            this.selector = selector;
        }

        public TResult Current => selector(sourceEnumerator.Current);
        object? IEnumerator.Current => Current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext() => sourceEnumerator.MoveNext();

        public void Reset() => sourceEnumerator.Reset();

        public void Dispose() => sourceEnumerator.Dispose();
    }
}
