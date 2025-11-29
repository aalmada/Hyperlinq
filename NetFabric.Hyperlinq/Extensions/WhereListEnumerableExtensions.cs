using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq
{
    public static partial class WhereListEnumerableExtensions
    {
        extension<TSource>(WhereListEnumerable<TSource> source)
            where TSource : IAdditionOperators<TSource, TSource, TSource>, IAdditiveIdentity<TSource, TSource>
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TSource Sum()
                => CollectionsMarshal.AsSpan(source.Source).Sum(source.Predicate);
        }

        extension<TSource>(WhereListEnumerable<TSource> source)
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Any()
                => CollectionsMarshal.AsSpan(source.Source).Any(source.Predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int Count()
                => CollectionsMarshal.AsSpan(source.Source).Count(source.Predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TSource First()
                => CollectionsMarshal.AsSpan(source.Source).First(source.Predicate);

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
                => CollectionsMarshal.AsSpan(source.Source).Single(source.Predicate);

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
