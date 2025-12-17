using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class EnumerableValueEnumerableExtensions
{
    extension<T>(EnumerableValueEnumerable<T> source)
        where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Sum()
        {
            var sum = T.AdditiveIdentity;
            foreach (var item in source)
            {
                sum += item;
            }
            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Sum(Func<T, bool> predicate)
        {
            var sum = T.AdditiveIdentity;
            foreach (var item in source)
            {
                if (predicate(item))
                {
                    sum += item;
                }
            }
            return sum;
        }
    }
}
