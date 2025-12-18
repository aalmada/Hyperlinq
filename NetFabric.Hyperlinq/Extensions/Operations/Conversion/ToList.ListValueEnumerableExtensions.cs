using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq;

public static partial class ListValueEnumerableExtensions
{
    extension<T>(ListValueEnumerable<T> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<T> ToList()
            => new List<T>(source.Source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<T> ToList<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => CollectionsMarshal.AsSpan(source.Source).ToList(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<T> ToList(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source.Source).ToList(predicate);
    }
}
