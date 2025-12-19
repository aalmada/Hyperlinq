using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ArrayExtensions
{
    extension<T>(T[] source)
        where T : struct, INumberBase<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Sum()
            => source.AsSpan().Sum();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Sum<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => source.AsSpan().Sum(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Sum<TPredicate>(in TPredicate predicate)
            where TPredicate : struct, IFunctionIn<T, bool>
            => source.AsSpan().Sum(in predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Sum(Func<T, bool> predicate)
            => source.AsSpan().Sum(predicate);
    }
}
