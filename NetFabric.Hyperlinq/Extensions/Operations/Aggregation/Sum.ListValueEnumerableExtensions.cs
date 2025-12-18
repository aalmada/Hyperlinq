using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq;

public static partial class ListValueEnumerableExtensions
{
    extension<T>(ListValueEnumerable<T> source)
        where T : struct, INumberBase<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Sum()
            => NetFabric.Numerics.Tensors.TensorOperations.Sum<T>(CollectionsMarshal.AsSpan(source.Source));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Sum<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => CollectionsMarshal.AsSpan(source.Source).Sum(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Sum(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source.Source).Sum(predicate);
    }
}
