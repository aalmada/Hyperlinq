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
        public T[] ToArray()
            => CollectionsMarshal.AsSpan(source).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => CollectionsMarshal.AsSpan(source).ToArray(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray<TPredicate>(in TPredicate predicate)
            where TPredicate : struct, IFunctionIn<T, bool>
            => CollectionsMarshal.AsSpan(source).ToArray(in predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source).ToArray(predicate);
    }
}
