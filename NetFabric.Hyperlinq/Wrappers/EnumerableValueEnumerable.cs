using System;
using System.Collections;
using System.Collections.Generic;

namespace NetFabric.Hyperlinq
{
    /// <summary>
    /// Fallback value-type enumerable wrapper for IEnumerable&lt;T&gt;.
    /// Uses the source's enumerator (may be a reference type).
    /// </summary>
    public readonly struct EnumerableValueEnumerable<T> : IValueEnumerable<T, EnumerableValueEnumerable<T>.Enumerator>
    {
        private readonly IEnumerable<T> source;

        public EnumerableValueEnumerable(IEnumerable<T> source)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
        }

        internal IEnumerable<T> Source => source;

        public Enumerator GetEnumerator() => new Enumerator(source.GetEnumerator());
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => source.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => source.GetEnumerator();

        public struct Enumerator : IEnumerator<T>
        {
            private readonly IEnumerator<T> enumerator;

            public Enumerator(IEnumerator<T> enumerator)
            {
                this.enumerator = enumerator;
            }

            public T Current => enumerator.Current;
            object? IEnumerator.Current => Current;

            public bool MoveNext() => enumerator.MoveNext();

            public void Reset() => enumerator.Reset();

            public void Dispose() => enumerator.Dispose();
        }
    }
}
