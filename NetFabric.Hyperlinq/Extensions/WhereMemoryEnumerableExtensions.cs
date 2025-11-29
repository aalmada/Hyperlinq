using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class WhereMemoryEnumerableExtensions
    {
        extension<TSource>(WhereMemoryEnumerable<TSource> source)
            where TSource : IAdditionOperators<TSource, TSource, TSource>, IAdditiveIdentity<TSource, TSource>
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TSource Sum()
                => source.Source.Span.Sum(source.Predicate);
        }

        extension<TSource>(WhereMemoryEnumerable<TSource> source)
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Any()
                => source.Source.Span.Any(source.Predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int Count()
                => source.Source.Span.Count(source.Predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TSource First()
                => source.Source.Span.First(source.Predicate);

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

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TSource Single()
                => source.Source.Span.Single(source.Predicate);

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
