using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public readonly partial struct RepeatInfiniteSequenceEnumerable<TEnumerator, TSource>
        : IValueEnumerable<TSource, RepeatInfiniteSequenceEnumerable<TEnumerator, TSource>.Enumerator>
        where TEnumerator : struct, IEnumerator<TSource>
    {
        private readonly IValueEnumerable<TSource, TEnumerator> source;

        internal RepeatInfiniteSequenceEnumerable(IValueEnumerable<TSource, TEnumerator> source)
        {
            this.source = source;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Enumerator GetEnumerator() => new Enumerator(in this);

        IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator() => new Enumerator(in this);
        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(in this);

        public struct Enumerator
            : IEnumerator<TSource>
        {
            private readonly IValueEnumerable<TSource, TEnumerator> source;
            private TEnumerator enumerator;

            internal Enumerator(in RepeatInfiniteSequenceEnumerable<TEnumerator, TSource> enumerable)
            {
                source = enumerable.source;
                enumerator = source.GetEnumerator();
            }

            public readonly TSource Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => enumerator.Current;
            }

            readonly object? IEnumerator.Current => enumerator.Current;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                if (enumerator.MoveNext())
                    return true;

                enumerator.Dispose();
                enumerator = source.GetEnumerator();
                return enumerator.MoveNext();
            }

            public void Reset() => throw new NotSupportedException();

            public void Dispose() => enumerator.Dispose();
        }
    }
}
