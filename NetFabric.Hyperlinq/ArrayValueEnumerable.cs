using System;
using System.Collections;
using System.Collections.Generic;

namespace NetFabric.Hyperlinq
{
    /// <summary>
    /// Value-type enumerable wrapper for arrays.
    /// Uses ArrayEnumerator&lt;T&gt; which is a value type.
    /// </summary>
    public readonly struct ArrayValueEnumerable<T> : IValueEnumerable<T, ArrayValueEnumerable<T>.Enumerator>
    {
        private readonly T[] source;

        public ArrayValueEnumerable(T[] source)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
        }

        public Enumerator GetEnumerator() => new Enumerator(source);
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct Enumerator : IEnumerator<T>
        {
            private readonly T[] array;
            private int index;

            public Enumerator(T[] array)
            {
                this.array = array;
                this.index = -1;
            }

            public T Current => array[index];
            object? IEnumerator.Current => Current;

            public bool MoveNext() => ++index < array.Length;

            public void Reset() => index = -1;

            public void Dispose() { }
        }
    }
}
