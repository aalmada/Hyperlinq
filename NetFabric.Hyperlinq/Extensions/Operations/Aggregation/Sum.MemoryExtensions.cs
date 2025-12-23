using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class MemoryExtensions
{
    extension<T>(Memory<T> source)
        where T : struct, INumberBase<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Sum()
            => ((ReadOnlyMemory<T>)source).Sum();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Sum<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => ((ReadOnlyMemory<T>)source).Sum(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Sum<TPredicate>(in TPredicate predicate)
            where TPredicate : struct, IFunctionIn<T, bool>
            => ((ReadOnlyMemory<T>)source).Sum(in predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Sum(Func<T, bool> predicate)
            => ((ReadOnlyMemory<T>)source).Sum(predicate);
    }
}
