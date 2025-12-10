using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class ValueEnumerableExtensions
    {
        /// <summary>
        /// Returns the first element of a sequence, or a default value if the sequence contains no elements.
        /// </summary>
        public static TSource FirstOrDefault<TEnumerable, TEnumerator, TSource>(this TEnumerable source)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            => source.FirstOrNone<TEnumerable, TEnumerator, TSource>().GetValueOrDefault();

        /// <summary>
        /// Returns the first element of a sequence, or a specified default value if the sequence contains no elements.
        /// </summary>
        public static TSource FirstOrDefault<TEnumerable, TEnumerator, TSource>(this TEnumerable source, TSource defaultValue)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            => source.FirstOrNone<TEnumerable, TEnumerator, TSource>().GetValueOrDefault(defaultValue);
        public static TSource FirstOrDefault<TEnumerable, TEnumerator, TSource, TPredicate>(this TEnumerable source, TPredicate predicate)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            where TPredicate : struct, IFunction<TSource, bool>
            => ValueEnumerableExtensions.FirstOrNone<TEnumerable, TEnumerator, TSource, TPredicate>(source, predicate).GetValueOrDefault();

        public static TSource FirstOrDefault<TEnumerable, TEnumerator, TSource, TPredicate>(this TEnumerable source, TPredicate predicate, TSource defaultValue)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            where TPredicate : struct, IFunction<TSource, bool>
            => ValueEnumerableExtensions.FirstOrNone<TEnumerable, TEnumerator, TSource, TPredicate>(source, predicate).GetValueOrDefault(defaultValue);

        /// <summary>
        /// Returns the first element of a sequence that satisfies a specified condition, or a default value if the sequence contains no elements.
        /// </summary>
        public static TSource FirstOrDefault<TEnumerable, TEnumerator, TSource>(this TEnumerable source, Func<TSource, bool> predicate)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            => ValueEnumerableExtensions.FirstOrNone<TEnumerable, TEnumerator, TSource>(source, predicate).GetValueOrDefault();

        /// <summary>
        /// Returns the first element of a sequence that satisfies a specified condition, or a specified default value if the sequence contains no elements.
        /// </summary>
        public static TSource FirstOrDefault<TEnumerable, TEnumerator, TSource>(this TEnumerable source, Func<TSource, bool> predicate, TSource defaultValue)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            => ValueEnumerableExtensions.FirstOrNone<TEnumerable, TEnumerator, TSource>(source, predicate).GetValueOrDefault(defaultValue);


    }
}
