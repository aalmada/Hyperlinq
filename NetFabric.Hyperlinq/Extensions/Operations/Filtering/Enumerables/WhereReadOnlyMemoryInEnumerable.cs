using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public readonly struct WhereReadOnlyMemoryInEnumerable<TSource, TPredicate> : IValueEnumerable<TSource, WhereReadOnlyMemoryInEnumerable<TSource, TPredicate>.Enumerator>
    where TPredicate : struct, IFunctionIn<TSource, bool>
{
    readonly ReadOnlyMemory<TSource> source;
    readonly TPredicate predicate;

    public WhereReadOnlyMemoryInEnumerable(ReadOnlyMemory<TSource> source, TPredicate predicate)
    {
        this.source = source;
        this.predicate = predicate;
    }

    internal ReadOnlyMemory<TSource> Source => source;
    internal TPredicate Predicate => predicate;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Enumerator GetEnumerator() => new(source, predicate);

    IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator() => GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public struct Enumerator : IEnumerator<TSource>
    {
        readonly ReadOnlyMemory<TSource> source;
        readonly TPredicate predicate;
        int index;

        public Enumerator(ReadOnlyMemory<TSource> source, TPredicate predicate)
        {
            this.source = source;
            this.predicate = predicate;
            this.index = -1;
        }

        public TSource Current => source.Span[index];
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
