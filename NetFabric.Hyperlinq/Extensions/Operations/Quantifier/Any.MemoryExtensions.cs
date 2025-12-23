using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class MemoryExtensions
{
    extension<T>(Memory<T> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Any()
            => ((ReadOnlyMemory<T>)source).Any();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Any<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => ((ReadOnlyMemory<T>)source).Any(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Any(Func<T, bool> predicate)
            => ((ReadOnlyMemory<T>)source).Any(predicate);
    }
}
