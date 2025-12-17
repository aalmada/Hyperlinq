using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ArrayValueEnumerableExtensions
{
    extension<T>(ArrayValueEnumerable<T> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray()
            => source.Source.ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => source.Source.ToArray(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray(Func<T, bool> predicate)
            => source.Source.ToArray(predicate);
    }
}
