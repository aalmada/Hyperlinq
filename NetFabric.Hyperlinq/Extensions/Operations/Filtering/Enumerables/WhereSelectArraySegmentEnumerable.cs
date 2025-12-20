using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq;

public readonly struct WhereSelectArraySegmentEnumerable<TSource, TResult, TPredicate, TSelector> : IValueEnumerable<TResult, WhereSelectArraySegmentEnumerable<TSource, TResult, TPredicate, TSelector>.Enumerator>
    where TPredicate : struct, IFunction<TSource, bool>
    where TSelector : struct, IFunction<TSource, TResult>
{
    readonly ArraySegment<TSource> source;
    readonly TPredicate predicate;
    readonly TSelector selector;

    public WhereSelectArraySegmentEnumerable(ArraySegment<TSource> source, TPredicate predicate, TSelector selector)
    {
        this.source = source;
        this.predicate = predicate;
        this.selector = selector;
    }

    internal ArraySegment<TSource> Source => source;
    internal TPredicate Predicate => predicate;
    internal TSelector Selector => selector;

    public Enumerator GetEnumerator() => new(source, predicate, selector);
    IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator() => GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public struct Enumerator : IEnumerator<TResult>
    {
        readonly ArraySegment<TSource> source;
        readonly TPredicate predicate;
        readonly TSelector selector;
        int index;

        public Enumerator(ArraySegment<TSource> source, TPredicate predicate, TSelector selector)
        {
            this.source = source;
            this.predicate = predicate;
            this.selector = selector;
            this.index = -1;
        }

        public TResult Current => selector.Invoke(source.AsSpan()[index]);
        object? IEnumerator.Current => Current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            var span = source.AsSpan();
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
