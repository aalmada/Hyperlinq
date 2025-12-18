using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ArrayValueEnumerableExtensions
{
    extension<T>(ArrayValueEnumerable<T> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<T> ToList()
            => new List<T>(source.Source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<T> ToList<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => source.Source.ToList(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<T> ToList(Func<T, bool> predicate)
            => source.Source.ToList(predicate);
    }
}
