using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ArrayValueEnumerableExtensions
{
    extension<T>(ArrayValueEnumerable<T> source)
        where T : struct, INumberBase<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Sum()
            => NetFabric.Numerics.Tensors.TensorOperations.Sum<T>(source.Source.AsSpan());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Sum<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => source.Source.Sum(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Sum(Func<T, bool> predicate)
            => source.Source.Sum(predicate);
    }
}
