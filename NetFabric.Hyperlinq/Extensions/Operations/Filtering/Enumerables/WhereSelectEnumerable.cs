using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;

namespace NetFabric.Hyperlinq;

/// <summary>
/// WhereSelectEnumerable for IEnumerable sources (fused Where+Select)
/// </summary>
public readonly struct WhereSelectEnumerable<TSource, TResult> : IValueEnumerable<TResult, WhereSelectEnumerable<TSource, TResult>.Enumerator>
{
    readonly IEnumerable<TSource> source;
    readonly Func<TSource, bool> predicate;
    readonly Func<TSource, TResult> selector;

    public WhereSelectEnumerable(IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, TResult> selector)
    {
        this.source = source ?? throw new ArgumentNullException(nameof(source));
        this.predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
        this.selector = selector ?? throw new ArgumentNullException(nameof(selector));
    }

    internal IEnumerable<TSource> Source => source;
    internal Func<TSource, bool> Predicate => predicate;

    public Enumerator GetEnumerator() => new Enumerator(source.GetEnumerator(), predicate, selector);
    IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator() => GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public TResult[] ToArray()
    {
        using var builder = new ArrayBuilder<TResult>(ArrayPool<TResult>.Shared);
        foreach (var item in source)
        {
            if (predicate(item))
            {
                builder.Add(selector(item));
            }
        }
        return builder.ToArray();
    }

    public PooledBuffer<TResult> ToArrayPooled(ArrayPool<TResult>? pool = null)
    {
        using var builder = new ArrayBuilder<TResult>(pool ?? ArrayPool<TResult>.Shared);
        foreach (var item in source)
        {
            if (predicate(item))
            {
                builder.Add(selector(item));
            }
        }
        return builder.ToPooledBuffer();
    }

    public List<TResult> ToList()
    {
        using var builder = new ArrayBuilder<TResult>(ArrayPool<TResult>.Shared);
        foreach (var item in source)
        {
            if (predicate(item))
            {
                builder.Add(selector(item));
            }
        }
        return builder.ToList();
    }

    public struct Enumerator : IEnumerator<TResult>
    {
        readonly IEnumerator<TSource> sourceEnumerator;
        readonly Func<TSource, bool> predicate;
        readonly Func<TSource, TResult> selector;

        public Enumerator(IEnumerator<TSource> sourceEnumerator, Func<TSource, bool> predicate, Func<TSource, TResult> selector)
        {
            this.sourceEnumerator = sourceEnumerator;
            this.predicate = predicate;
            this.selector = selector;
        }

        public TResult Current => selector(sourceEnumerator.Current);
        object? IEnumerator.Current => Current;

        public bool MoveNext()
        {
            while (sourceEnumerator.MoveNext())
            {
                if (predicate(sourceEnumerator.Current))
                {
                    return true;
                }
            }
            return false;
        }

        public void Reset() => sourceEnumerator.Reset();
        public void Dispose() => sourceEnumerator.Dispose();
    }
}
