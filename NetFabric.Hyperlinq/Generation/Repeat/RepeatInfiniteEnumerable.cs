using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public readonly partial struct RepeatInfiniteEnumerable<T>
    : IValueEnumerable<T, RepeatInfiniteEnumerable<T>.Enumerator>
{
    readonly T element;

    internal RepeatInfiniteEnumerable(T element) => this.element = element;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Enumerator GetEnumerator() => new Enumerator(in this);

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => new Enumerator(in this);
    IEnumerator IEnumerable.GetEnumerator() => new Enumerator(in this);

    public struct Enumerator
        : IEnumerator<T>
    {
        readonly T element;

        internal Enumerator(in RepeatInfiniteEnumerable<T> enumerable) => element = enumerable.element;

        public T Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => element;
        }

        object? IEnumerator.Current => element;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext() => true;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset() { }

        public void Dispose() { }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Any() => true;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(T value)
        => EqualityComparer<T>.Default.Equals(element, value);
}
