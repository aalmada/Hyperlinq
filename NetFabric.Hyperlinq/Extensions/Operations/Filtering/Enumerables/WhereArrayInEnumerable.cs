using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public readonly struct WhereArrayInEnumerable<TSource, TPredicate> : IValueEnumerable<TSource, WhereArrayInEnumerable<TSource, TPredicate>.Enumerator>
    where TPredicate : struct, IFunctionIn<TSource, bool>
{
    readonly TSource[] source;
    readonly TPredicate predicate;

    public WhereArrayInEnumerable(TSource[] source, in TPredicate predicate)
    {
        this.source = source ?? throw new ArgumentNullException(nameof(source));
        this.predicate = predicate;
    }

    internal TSource[] Source => source;
    internal TPredicate Predicate => predicate;

    public Enumerator GetEnumerator() => new Enumerator(source, in predicate);
    IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator() => GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public struct Enumerator : IEnumerator<TSource>
    {
        readonly TSource[] source;
        readonly TPredicate predicate;
        int index;

        public Enumerator(TSource[] source, in TPredicate predicate)
        {
            this.source = source;
            this.predicate = predicate;
            this.index = -1;
        }

        public TSource Current => source[index];
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
