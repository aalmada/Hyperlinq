using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class EnumerableValueEnumerableExtensions
{
    extension<T>(EnumerableValueEnumerable<T> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SelectEnumerable<T, TResult> Select<TResult>(Func<T, TResult> selector)
            => new SelectEnumerable<T, TResult>(source.Source, selector);
    }
}
