using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ListValueEnumerableExtensions
{
    extension<T>(ListValueEnumerable<T> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Any()
            => source.Source.Count != 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Any<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => System.Runtime.InteropServices.CollectionsMarshal.AsSpan(source.Source).Any(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Any(Func<T, bool> predicate)
            => System.Runtime.InteropServices.CollectionsMarshal.AsSpan(source.Source).Any(predicate);
    }
}
