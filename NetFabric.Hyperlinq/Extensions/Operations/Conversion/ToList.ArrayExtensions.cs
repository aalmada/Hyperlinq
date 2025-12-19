using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ArrayExtensions
{
    extension<T>(T[] source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<T> ToList<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => source.AsSpan().ToList(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<T> ToList<TPredicate>(in TPredicate predicate)
            where TPredicate : struct, IFunctionIn<T, bool>
            => source.AsSpan().ToList(in predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<T> ToList(Func<T, bool> predicate)
            => source.AsSpan().ToList(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<T> ToList()
            => source.AsSpan().ToList();
    }
}
