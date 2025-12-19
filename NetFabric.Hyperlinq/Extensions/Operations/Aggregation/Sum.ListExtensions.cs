using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq;

public static partial class ListExtensions
{
    extension<T>(List<T> source)
        where T : struct, INumberBase<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Sum()
            => CollectionsMarshal.AsSpan(source).Sum();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Sum<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => CollectionsMarshal.AsSpan(source).Sum(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Sum<TPredicate>(in TPredicate predicate)
            where TPredicate : struct, IFunctionIn<T, bool>
            => CollectionsMarshal.AsSpan(source).Sum(in predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Sum(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source).Sum(predicate);
    }
}
