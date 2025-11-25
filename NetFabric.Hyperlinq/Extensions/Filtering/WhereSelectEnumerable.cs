using System;
using System.Collections;
using System.Collections.Generic;

namespace NetFabric.Hyperlinq
{
    /// <summary>
    /// WhereSelectEnumerable for IEnumerable sources (fused Where+Select)
    /// </summary>
    public readonly struct WhereSelectEnumerable<TSource, TResult> : IValueEnumerable<TResult, WhereSelectEnumerable<TSource, TResult>.Enumerator>
    {
        readonly IEnumerable<TSource> source;
        readonly Func<TSource, bool> predicate;
        readonly Func<TSource, TResult> selector;

        public WhereSelectEnumerable(IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, TResult> selector)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
            this.predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
            this.selector = selector ?? throw new ArgumentNullException(nameof(selector));
        }

        public Enumerator GetEnumerator() => new Enumerator(source.GetEnumerator(), predicate, selector);
        IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Returns the number of elements that satisfy the predicate.
        /// Optimized to ignore the selector since Count doesn't need projected values.
        /// </summary>
        public int Count()
        {
            var count = 0;
            foreach (var item in source)
            {
                if (predicate(item))
                    count++;
            }
            return count;
        }

        /// <summary>
        /// Determines whether any element satisfies the predicate.
        /// Optimized to ignore the selector since Any doesn't need projected values.
        /// </summary>
        public bool Any()
        {
            foreach (var item in source)
            {
                if (predicate(item))
                    return true;
            }
            return false;
        }

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
            object? IEnumerator.Current => Current;

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
