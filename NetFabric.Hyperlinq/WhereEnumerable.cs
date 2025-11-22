using System;
using System.Collections;
using System.Collections.Generic;

namespace NetFabric.Hyperlinq
{
    public readonly struct WhereEnumerable<TSource> : IValueEnumerable<TSource, WhereEnumerable<TSource>.Enumerator>
    {
        readonly IEnumerable<TSource> source;
        readonly Func<TSource, bool> predicate;

        public WhereEnumerable(IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            this.source = source;
            this.predicate = predicate;
        }

        public Enumerator GetEnumerator() => new Enumerator(source.GetEnumerator(), predicate);
        IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

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
