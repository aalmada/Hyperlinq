using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
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
                    sum += enumerator.Current;
                return sum;
            }
        }

        extension<TSource>(WhereEnumerable<TSource> source)
        {
            public bool Any()
            {
                using var enumerator = source.GetEnumerator();
                return enumerator.MoveNext();
            }

            public int Count()
            {
                var count = 0;
                using var enumerator = source.GetEnumerator();
                while (enumerator.MoveNext())
                    count++;
                return count;
            }

            public TSource First()
                => source.FirstOrNone().Value;

            public TSource FirstOrDefault()
                => source.FirstOrNone().GetValueOrDefault();

            public TSource FirstOrDefault(TSource defaultValue)
                => source.FirstOrNone().GetValueOrDefault(defaultValue);

            public Option<TSource> FirstOrNone()
            {
                using var enumerator = source.GetEnumerator();
                if (enumerator.MoveNext())
                    return Option<TSource>.Some(enumerator.Current);
                return Option<TSource>.None();
            }

            public TSource Single()
                => source.SingleOrNone().Value;

            public TSource SingleOrDefault()
                => source.SingleOrNone().GetValueOrDefault();

            public TSource SingleOrDefault(TSource defaultValue)
                => source.SingleOrNone().GetValueOrDefault(defaultValue);

            public Option<TSource> SingleOrNone()
            {
                using var enumerator = source.GetEnumerator();
                if (!enumerator.MoveNext())
                    return Option<TSource>.None();
                
                var first = enumerator.Current;
                if (enumerator.MoveNext())
                    throw new InvalidOperationException("Sequence contains more than one element");

                return Option<TSource>.Some(first);
            }
        }
    }
}
