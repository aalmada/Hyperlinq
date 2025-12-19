using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq;

public static partial class ListExtensions
{
    extension<T>(List<T> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray<TPredicate>(TPredicate predicate, ArrayPool<T>? pool = default)
            where TPredicate : struct, IFunction<T, bool>
            => CollectionsMarshal.AsSpan(source).ToArray(predicate, pool);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray<TPredicate>(in TPredicate predicate, ArrayPool<T>? pool = default)
            where TPredicate : struct, IFunctionIn<T, bool>
            => CollectionsMarshal.AsSpan(source).ToArray(in predicate, pool);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray(Func<T, bool> predicate, ArrayPool<T>? pool = default)
            => CollectionsMarshal.AsSpan(source).ToArray(predicate, pool);
    }
}
