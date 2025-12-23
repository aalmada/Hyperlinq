using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class MemoryExtensions
{
    extension<T>(Memory<T> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Count()
            => ((ReadOnlyMemory<T>)source).Count();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Count<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => ((ReadOnlyMemory<T>)source).Count(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Count(Func<T, bool> predicate)
            => ((ReadOnlyMemory<T>)source).Count(predicate);
    }
}
