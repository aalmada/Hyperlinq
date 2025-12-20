using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public readonly struct WhereArraySegmentInEnumerable<TSource, TPredicate> : IValueEnumerable<TSource, WhereArraySegmentInEnumerable<TSource, TPredicate>.Enumerator>
    where TPredicate : struct, IFunctionIn<TSource, bool>
{
    readonly ArraySegment<TSource> source;
    readonly TPredicate predicate;

    public WhereArraySegmentInEnumerable(ArraySegment<TSource> source, in TPredicate predicate)
    {
        this.source = source;
        this.predicate = predicate;
    }

    internal ArraySegment<TSource> Source => source;
    internal TPredicate Predicate => predicate;

    public Enumerator GetEnumerator() => new Enumerator(source, in predicate);
    IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator() => GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public struct Enumerator : IEnumerator<TSource>
    {
        readonly ArraySegment<TSource> source;
        readonly TPredicate predicate;
        int index;

        public Enumerator(ArraySegment<TSource> source, in TPredicate predicate)
        {
            this.source = source;
            this.predicate = predicate;
            this.index = -1;
        }

        public TSource Current => source.AsSpan()[index];
        object? IEnumerator.Current => Current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            var span = source.AsSpan();
            while (++index < span.Length)
            {
                if (predicate.Invoke(in span[index]))
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
