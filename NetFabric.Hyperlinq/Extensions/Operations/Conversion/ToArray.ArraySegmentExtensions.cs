using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ArraySegmentExtensions
{
    extension<T>(ArraySegment<T> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray()
            => source.AsSpan().ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => source.AsSpan().ToArray(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray<TPredicate>(in TPredicate predicate)
            where TPredicate : struct, IFunctionIn<T, bool>
            => source.AsSpan().ToArray(in predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray(Func<T, bool> predicate)
            => source.AsSpan().ToArray(predicate);
    }
}
