using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class ValueEnumerableExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RepeatInfiniteSequenceEnumerable<TEnumerator, TSource> Repeat<TEnumerator, TSource>(this IValueEnumerable<TSource, TEnumerator> source)
            where TEnumerator : struct, IEnumerator<TSource>
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return new RepeatInfiniteSequenceEnumerable<TEnumerator, TSource>(source);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RepeatSequenceEnumerable<TEnumerator, TSource> Repeat<TEnumerator, TSource>(this IValueEnumerable<TSource, TEnumerator> source, int count)
            where TEnumerator : struct, IEnumerator<TSource>
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return new RepeatSequenceEnumerable<TEnumerator, TSource>(source, count);
        }
    }
}
