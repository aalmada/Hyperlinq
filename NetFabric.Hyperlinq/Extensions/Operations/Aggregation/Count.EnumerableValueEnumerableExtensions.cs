using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class EnumerableValueEnumerableExtensions
{
    extension<T>(EnumerableValueEnumerable<T> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Count(Func<T, bool> predicate)
        {
            var count = 0;
            foreach (var item in source)
            {
                if (predicate(item))
                {
                    count++;
                }
            }
            return count;
        }
    }
}
