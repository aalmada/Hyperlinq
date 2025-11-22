using System;
using System.Collections;
using System.Collections.Generic;

namespace NetFabric.Hyperlinq
{
    public readonly struct WhereSelectEnumerable<TSource, TResult> : IValueEnumerable<TResult, WhereSelectEnumerable<TSource, TResult>.Enumerator>
    {
        readonly IEnumerable<TSource> source;
        readonly Func<TSource, bool> predicate;
        readonly Func<TSource, TResult> selector;

        public WhereSelectEnumerable(IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, TResult> selector)
        {
            this.source = source;
            this.predicate = predicate;
            this.selector = selector;
        }

        public Enumerator GetEnumerator() => new Enumerator(source.GetEnumerator(), predicate, selector);
        IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

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
            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                while (sourceEnumerator.MoveNext())
                {
                    if (predicate(sourceEnumerator.Current))
                        return true;
                }
                return false;
            }

            public void Reset() => sourceEnumerator.Reset();

            public void Dispose() => sourceEnumerator.Dispose();
        }
    }
}
