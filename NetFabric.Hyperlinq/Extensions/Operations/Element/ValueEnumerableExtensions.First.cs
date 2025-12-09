using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class ValueEnumerableExtensions
    {
        /// <summary>
        /// Returns the first element of a sequence.
        /// </summary>
        public static TSource First<TEnumerable, TEnumerator, TSource>(this TEnumerable source)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            => source.FirstOrNone<TEnumerable, TEnumerator, TSource>().Value;

        /// <summary>
        /// Returns the first element of a sequence that satisfies a specified condition.
        /// </summary>
        public static TSource First<TEnumerable, TEnumerator, TSource>(this TEnumerable source, Func<TSource, bool> predicate)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            => ValueEnumerableExtensions.FirstOrNone<TEnumerable, TEnumerator, TSource>(source, predicate).Value;


    }
}
