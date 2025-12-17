using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ArrayValueEnumerableExtensions
{
    extension<T>(ArrayValueEnumerable<T> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Single()
            => source.Source.Single();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Single<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => source.Source.Single(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Single(Func<T, bool> predicate)
            => source.Source.Single(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T SingleOrDefault()
            => source.Source.SingleOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T SingleOrDefault<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => source.Source.SingleOrDefault(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T SingleOrDefault(Func<T, bool> predicate)
            => source.Source.SingleOrDefault(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T SingleOrDefault<TPredicate>(TPredicate predicate, T defaultValue)
            where TPredicate : struct, IFunction<T, bool>
            => source.Source.SingleOrDefault(predicate, defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T SingleOrDefault(Func<T, bool> predicate, T defaultValue)
            => source.Source.SingleOrDefault(predicate, defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> SingleOrNone()
            => source.Source.SingleOrNone();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> SingleOrNone<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => source.Source.SingleOrNone(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> SingleOrNone(Func<T, bool> predicate)
            => source.Source.SingleOrNone(predicate);
    }
}
