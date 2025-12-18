using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class WhereEnumerableExtensions
{
    extension<TSource>(WhereEnumerable<TSource> source)
    {
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
            {
                return Option<TSource>.Some(enumerator.Current);
            }

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
            {
                return Option<TSource>.None();
            }

            var first = enumerator.Current;
            if (enumerator.MoveNext())
            {
                throw new InvalidOperationException("Sequence contains more than one element");
            }

            return Option<TSource>.Some(first);
        }
    }
}
