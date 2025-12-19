using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ArrayExtensions
{
    extension<T>(T[] source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Any()
            => source.AsSpan().Any();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Any<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => source.AsSpan().Any(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Any(Func<T, bool> predicate)
            => source.AsSpan().Any(predicate);
    }
}
