using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public readonly struct WhereSelectReadOnlyMemoryEnumerable<TSource, TResult, TPredicate, TSelector> : IValueEnumerable<TResult, WhereSelectReadOnlyMemoryEnumerable<TSource, TResult, TPredicate, TSelector>.Enumerator>
    where TPredicate : struct, IFunction<TSource, bool>
    where TSelector : struct, IFunction<TSource, TResult>
{
    readonly ReadOnlyMemory<TSource> source;
    readonly TPredicate predicate;
    readonly TSelector selector;

    public WhereSelectReadOnlyMemoryEnumerable(ReadOnlyMemory<TSource> source, TPredicate predicate, TSelector selector)
    {
        this.source = source;
        this.predicate = predicate;
        this.selector = selector;
    }

    internal ReadOnlyMemory<TSource> Source => source;
    internal TPredicate Predicate => predicate;
    internal TSelector Selector => selector;

    public Enumerator GetEnumerator() => new(source, predicate, selector);
    IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator() => GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public struct Enumerator : IEnumerator<TResult>
    {
        readonly ReadOnlyMemory<TSource> source;
        readonly TPredicate predicate;
        readonly TSelector selector;
        int index;

        public Enumerator(ReadOnlyMemory<TSource> source, TPredicate predicate, TSelector selector)
        {
            this.source = source;
            this.predicate = predicate;
            this.selector = selector;
            this.index = -1;
        }

        public TResult Current => selector.Invoke(source.Span[index]);
        object? IEnumerator.Current => Current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            var span = source.Span;
            while (++index < span.Length)
            {
                if (predicate.Invoke(span[index]))
                {
                    return true;
                }
            }
            return false;
        }

        public void Reset() => index = -1;
        public void Dispose() { }
    }
}
