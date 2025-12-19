using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq;

public static partial class ListExtensions
{
    extension<T>(List<T> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<T> ToList<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => CollectionsMarshal.AsSpan(source).ToList(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<T> ToList<TPredicate>(in TPredicate predicate)
            where TPredicate : struct, IFunctionIn<T, bool>
            => CollectionsMarshal.AsSpan(source).ToList(in predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<T> ToList(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source).ToList(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<T> ToList()
            => CollectionsMarshal.AsSpan(source).ToList();
    }
}
