using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ReadOnlyMemoryExtensions
{
    extension<T>(ReadOnlyMemory<T> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray()
            => source.Span.ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => source.Span.ToArray(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray<TPredicate>(in TPredicate predicate)
            where TPredicate : struct, IFunctionIn<T, bool>
            => source.Span.ToArray(in predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray(Func<T, bool> predicate)
            => source.Span.ToArray(predicate);
    }
}
