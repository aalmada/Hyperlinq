using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ReadOnlyMemoryExtensions
{
    extension<T>(ReadOnlyMemory<T> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray<TPredicate>(TPredicate predicate, ArrayPool<T>? pool = default)
            where TPredicate : struct, IFunction<T, bool>
            => source.Span.ToArray(predicate, pool);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray<TPredicate>(in TPredicate predicate, ArrayPool<T>? pool = default)
            where TPredicate : struct, IFunctionIn<T, bool>
            => source.Span.ToArray(in predicate, pool);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray(Func<T, bool> predicate, ArrayPool<T>? pool = default)
            => source.Span.ToArray(predicate, pool);
    }
}
