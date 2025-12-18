using System;

namespace NetFabric.Hyperlinq;

public static partial class WhereEnumerableExtensions
{
    extension<TSource>(WhereEnumerable<TSource> source)
    {
        public int Count()
        {
            var count = 0;
            using var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                count++;
            }

            return count;
        }
    }
}
