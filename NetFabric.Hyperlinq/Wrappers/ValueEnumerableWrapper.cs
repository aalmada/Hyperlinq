using System;
using System.Collections;
using System.Collections.Generic;

namespace NetFabric.Hyperlinq
{
    /// <summary>
    /// Generic value-type enumerable wrapper for any IEnumerable&lt;T&gt; providing a value-type enumerator.
    /// Implements IValueEnumerable for basic enumeration support.
    /// </summary>
    public readonly struct ValueEnumerableWrapper<TEnumerable, TEnumerator, TSource> 
        : IValueEnumerable<TSource, TEnumerator>
        where TEnumerable : IEnumerable<TSource>
        where TEnumerator : struct, IEnumerator<TSource>
    {
        private readonly TEnumerable source;

        public ValueEnumerableWrapper(TEnumerable source)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
        }

        internal TEnumerable Source => source;

        public TEnumerator GetEnumerator() => (TEnumerator)source.GetEnumerator();
        IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator() => source.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => source.GetEnumerator();
    }
}
