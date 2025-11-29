using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class WhereSelectReadOnlySpanEnumerableExtensions
    {
        extension<TSource, TResult>(WhereSelectReadOnlySpanEnumerable<TSource, TResult> source)
            where TResult : IAdditionOperators<TResult, TResult, TResult>, IAdditiveIdentity<TResult, TResult>
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TResult Sum()
            {
                var sum = TResult.AdditiveIdentity;
                foreach (var item in source)
                {
                    sum += item;
                }
                return sum;
            }
        }
    }
}
