using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

/// <summary>
/// WhereEnumerable for IEnumerable sources (fallback)
/// </summary>
public readonly struct WhereEnumerable<TSource> : IValueEnumerable<TSource, WhereEnumerable<TSource>.Enumerator>
{
    readonly IEnumerable<TSource> source;
    readonly Func<TSource, bool> predicate;

    public WhereEnumerable(IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        this.source = source ?? throw new ArgumentNullException(nameof(source));
        this.predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public WhereSelectEnumerable<TSource, TResult> Select<TResult>(Func<TSource, TResult> selector)
        => new WhereSelectEnumerable<TSource, TResult>(source, predicate, selector);

    public Enumerator GetEnumerator() => new Enumerator(source.GetEnumerator(), predicate);
    IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator() => GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public PooledBuffer<TSource> ToArrayPooled(ArrayPool<TSource>? pool = null)
    {
        using var builder = new ArrayBuilder<TSource>(pool ?? ArrayPool<TSource>.Shared);
        using var enumerator = GetEnumerator();
        while (enumerator.MoveNext())
        {
            builder.Add(enumerator.Current);
        }
        return builder.ToPooledBuffer();
    }

    public List<TSource> ToList()
    {
        using var builder = new ArrayBuilder<TSource>(ArrayPool<TSource>.Shared);
        using var enumerator = GetEnumerator();
        while (enumerator.MoveNext())
        {
            builder.Add(enumerator.Current);
        }
        return builder.ToList();
    }

    public struct Enumerator : IEnumerator<TSource>
    {
        readonly IEnumerator<TSource> sourceEnumerator;
        readonly Func<TSource, bool> predicate;

        public Enumerator(IEnumerator<TSource> sourceEnumerator, Func<TSource, bool> predicate)
        {
            this.sourceEnumerator = sourceEnumerator;
            this.predicate = predicate;
        }

        public TSource Current => sourceEnumerator.Current;
        object? IEnumerator.Current => Current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
