using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class EnumerableValueEnumerableExtensions
{
    extension<T>(EnumerableValueEnumerable<T> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WhereEnumerable<T> Where(Func<T, bool> predicate)
            => new WhereEnumerable<T>(source.Source, predicate);
    }
}
