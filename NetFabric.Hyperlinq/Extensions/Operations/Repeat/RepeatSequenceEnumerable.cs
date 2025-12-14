using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public readonly partial struct RepeatSequenceEnumerable<TEnumerator, TSource>
    : IValueEnumerable<TSource, RepeatSequenceEnumerable<TEnumerator, TSource>.Enumerator>
    where TEnumerator : struct, IEnumerator<TSource>
{
    readonly IValueEnumerable<TSource, TEnumerator> source;
    readonly int count;

    internal RepeatSequenceEnumerable(IValueEnumerable<TSource, TEnumerator> source, int count)
    {
        this.source = source;
        this.count = count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Enumerator GetEnumerator() => new Enumerator(in this);

    IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator() => new Enumerator(in this);
    IEnumerator IEnumerable.GetEnumerator() => new Enumerator(in this);

    public struct Enumerator
        : IEnumerator<TSource>
    {
        readonly IValueEnumerable<TSource, TEnumerator> source;
        TEnumerator enumerator;
        int remaining;

        internal Enumerator(in RepeatSequenceEnumerable<TEnumerator, TSource> enumerable)
        {
            source = enumerable.source;
            remaining = enumerable.count;
            enumerator = remaining > 0 ? source.GetEnumerator() : default;
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
            if (remaining == 0)
            {
                return false;
            }

            if (enumerator.MoveNext())
            {
                return true;
            }

            remaining--;
            enumerator.Dispose();

            if (remaining > 0)
            {
                enumerator = source.GetEnumerator();
                return enumerator.MoveNext();
            }

            return false;
        }

        public void Reset() => throw new NotSupportedException();

        public void Dispose() => enumerator.Dispose();
    }
}
