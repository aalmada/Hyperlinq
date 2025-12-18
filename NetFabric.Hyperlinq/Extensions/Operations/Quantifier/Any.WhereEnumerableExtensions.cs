using System;

namespace NetFabric.Hyperlinq;

public static partial class WhereEnumerableExtensions
{
    extension<TSource>(WhereEnumerable<TSource> source)
    {
        public bool Any()
        {
            using var enumerator = source.GetEnumerator();
            return enumerator.MoveNext();
        }
    }
}
