using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq;

public static partial class ListValueEnumerableExtensions
{
    extension<T>(ListValueEnumerable<T> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray()
            => CollectionsMarshal.AsSpan(source.Source).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => CollectionsMarshal.AsSpan(source.Source).ToArray(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source.Source).ToArray(predicate);
    }
}
