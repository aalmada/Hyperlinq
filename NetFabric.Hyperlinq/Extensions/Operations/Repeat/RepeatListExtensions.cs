using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class RepeatListExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RepeatListEnumerable<TSource> Repeat<TSource>(this List<TSource> source, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return new RepeatListEnumerable<TSource>(source, count);
        }
    }
}
