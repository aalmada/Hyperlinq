using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq
{
    /// <summary>
    /// WhereSelectEnumerable for List sources (fused Where+Select)
    /// </summary>
    public readonly struct WhereSelectListEnumerable<TSource, TResult> : IValueEnumerable<TResult, WhereSelectListEnumerable<TSource, TResult>.Enumerator>
    {
        readonly List<TSource> source;
        readonly Func<TSource, bool> predicate;
        readonly Func<TSource, TResult> selector;

        public WhereSelectListEnumerable(List<TSource> source, Func<TSource, bool> predicate, Func<TSource, TResult> selector)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
            this.predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
            this.selector = selector ?? throw new ArgumentNullException(nameof(selector));
        }

        public Enumerator GetEnumerator() => new Enumerator(source, predicate, selector);
        IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct Enumerator : IEnumerator<TResult>
        {
            readonly List<TSource> list;
            readonly Func<TSource, bool> predicate;
            readonly Func<TSource, TResult> selector;
            readonly int length;
            int index;

            public Enumerator(List<TSource> list, Func<TSource, bool> predicate, Func<TSource, TResult> selector)
            {
                this.list = list;
                this.predicate = predicate;
                this.selector = selector;
                this.length = CollectionsMarshal.AsSpan(list).Length;
                this.index = -1;
            }

            public TResult Current => selector(CollectionsMarshal.AsSpan(list)[index]);
            object? IEnumerator.Current => Current;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                var span = CollectionsMarshal.AsSpan(list);
                while (++index < length)
                {
                    if (predicate(span[index]))
                        return true;
                }
                return false;
            }

            public void Reset() => index = -1;
            public void Dispose() { }
        }
    }
}
