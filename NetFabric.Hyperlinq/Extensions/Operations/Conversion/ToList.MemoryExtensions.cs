using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class MemoryExtensions
{
    extension<T>(Memory<T> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<T> ToList()
            => ((ReadOnlyMemory<T>)source).ToList();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<T> ToList<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => ((ReadOnlyMemory<T>)source).ToList(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<T> ToList<TPredicate>(in TPredicate predicate)
            where TPredicate : struct, IFunctionIn<T, bool>
            => ((ReadOnlyMemory<T>)source).ToList(in predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<T> ToList(Func<T, bool> predicate)
            => ((ReadOnlyMemory<T>)source).ToList(predicate);
    }
}
