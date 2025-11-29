using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class WhereReadOnlySpanEnumerableExtensions
    {
        extension<TSource>(WhereReadOnlySpanEnumerable<TSource> source)
            where TSource : IAdditionOperators<TSource, TSource, TSource>, IAdditiveIdentity<TSource, TSource>
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TSource Sum()
            {
                var sum = TSource.AdditiveIdentity;
                foreach (var item in source)
                {
                    sum += item;
                }
                return sum;
            }
        }
    }
}
