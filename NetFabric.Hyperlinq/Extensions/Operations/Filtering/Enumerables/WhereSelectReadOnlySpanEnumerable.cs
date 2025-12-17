using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public readonly ref struct WhereSelectReadOnlySpanEnumerable<TSource, TResult, TPredicate, TSelector>
    where TPredicate : struct, IFunction<TSource, bool>
    where TSelector : struct, IFunction<TSource, TResult>
{
    readonly ReadOnlySpan<TSource> source;
    readonly TPredicate predicate;
    readonly TSelector selector;

    internal ReadOnlySpan<TSource> Source => source;
    internal TPredicate Predicate => predicate;
    internal TSelector Selector => selector;

    public WhereSelectReadOnlySpanEnumerable(ReadOnlySpan<TSource> source, TPredicate predicate, TSelector selector)
    {
        this.source = source;
        this.predicate = predicate;
        this.selector = selector;
    }



    public Enumerator GetEnumerator() => new Enumerator(source, predicate, selector);

    public ref struct Enumerator
    {
        readonly ReadOnlySpan<TSource> source;
        readonly TPredicate predicate;
        readonly TSelector selector;
        int index;

        public Enumerator(ReadOnlySpan<TSource> source, TPredicate predicate, TSelector selector)
        {
            this.source = source;
            this.predicate = predicate;
            this.selector = selector;
            this.index = -1;
        }

        public readonly TResult Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => selector.Invoke(source[index]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            while ((uint)++index < (uint)source.Length)
            {
                if (predicate.Invoke(source[index]))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
