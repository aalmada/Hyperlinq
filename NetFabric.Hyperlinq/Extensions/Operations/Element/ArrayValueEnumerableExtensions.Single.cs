using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ArrayValueEnumerableExtensions
{
    extension<T>(ArrayValueEnumerable<T> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Single()
            => source.SingleOrNone().Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Single<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => source.SingleOrNone(predicate).Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Single(Func<T, bool> predicate)
            => source.SingleOrNone(predicate).Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T SingleOrDefault()
            => source.SingleOrNone().GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T SingleOrDefault<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => source.SingleOrNone(predicate).GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T SingleOrDefault(Func<T, bool> predicate)
            => source.SingleOrNone(predicate).GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T SingleOrDefault<TPredicate>(TPredicate predicate, T defaultValue)
            where TPredicate : struct, IFunction<T, bool>
            => source.SingleOrNone(predicate).GetValueOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T SingleOrDefault(Func<T, bool> predicate, T defaultValue)
            => source.SingleOrNone(predicate).GetValueOrDefault(defaultValue);

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
