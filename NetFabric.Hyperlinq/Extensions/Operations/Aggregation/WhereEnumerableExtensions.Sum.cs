using System;
using System.Numerics;

namespace NetFabric.Hyperlinq;

public static partial class WhereEnumerableExtensions
{
    extension<TSource>(WhereEnumerable<TSource> source)
        where TSource : IAdditionOperators<TSource, TSource, TSource>, IAdditiveIdentity<TSource, TSource>
    {
        public TSource Sum()
        {
            var sum = TSource.AdditiveIdentity;
            using var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                sum += enumerator.Current;
            }

            return sum;
        }
    }
}
