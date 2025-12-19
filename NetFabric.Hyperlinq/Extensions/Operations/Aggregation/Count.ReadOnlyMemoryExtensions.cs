using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ReadOnlyMemoryExtensions
{
    extension<T>(ReadOnlyMemory<T> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Count()
            => source.Span.Count();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Count<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => source.Span.Count(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Count(Func<T, bool> predicate)
            => source.Span.Count(predicate);
    }
}
