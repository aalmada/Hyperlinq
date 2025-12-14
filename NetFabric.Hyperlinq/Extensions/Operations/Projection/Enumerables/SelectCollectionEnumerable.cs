using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public readonly struct SelectCollectionEnumerable<TEnumerator, TSource, TResult>
    : IValueReadOnlyCollection<TResult, SelectCollectionEnumerable<TEnumerator, TSource, TResult>.Enumerator>, ICollection<TResult>
    where TEnumerator : struct, IEnumerator<TSource>
{
    readonly IValueReadOnlyCollection<TSource, TEnumerator> source;
    readonly Func<TSource, TResult> selector;

    public SelectCollectionEnumerable(IValueReadOnlyCollection<TSource, TEnumerator> source, Func<TSource, TResult> selector)
    {
        this.source = source;
        this.selector = selector;
    }

    public int Count => source.Count;

    public Enumerator GetEnumerator() => new Enumerator(source.GetEnumerator(), selector);
    IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator() => GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    bool ICollection<TResult>.IsReadOnly => true;

    public void CopyTo(TResult[] array, int arrayIndex)
    {
        if (array is null)
        {
            throw new ArgumentNullException(nameof(array));
        }

        if (arrayIndex < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(arrayIndex));
        }

        if (array.Length - arrayIndex < Count)
        {
            throw new ArgumentException("Destination array is not long enough.");
        }

        var index = arrayIndex;
        foreach (var item in this)
        {
            array[index++] = item;
        }
    }

    public bool Contains(TResult item)
    {
        foreach (var current in this)
        {
            if (EqualityComparer<TResult>.Default.Equals(current, item))
            {
                return true;
            }
        }
        return false;
    }

    void ICollection<TResult>.Add(TResult item) => throw new NotSupportedException();
    void ICollection<TResult>.Clear() => throw new NotSupportedException();
    bool ICollection<TResult>.Remove(TResult item) => throw new NotSupportedException();

    public struct Enumerator : IEnumerator<TResult>
    {
        TEnumerator sourceEnumerator;
        readonly Func<TSource, TResult> selector;

        public Enumerator(TEnumerator sourceEnumerator, Func<TSource, TResult> selector)
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
