using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq;

public static partial class ListValueEnumerableExtensions
{
    extension<T>(ListValueEnumerable<T> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Single()
            => CollectionsMarshal.AsSpan(source.Source).Single();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Single<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => CollectionsMarshal.AsSpan(source.Source).Single(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Single(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source.Source).Single(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T SingleOrDefault()
            => CollectionsMarshal.AsSpan(source.Source).SingleOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T SingleOrDefault<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => CollectionsMarshal.AsSpan(source.Source).SingleOrDefault(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T SingleOrDefault(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source.Source).SingleOrDefault(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T SingleOrDefault<TPredicate>(TPredicate predicate, T defaultValue)
            where TPredicate : struct, IFunction<T, bool>
            => CollectionsMarshal.AsSpan(source.Source).SingleOrDefault(predicate, defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T SingleOrDefault(Func<T, bool> predicate, T defaultValue)
            => CollectionsMarshal.AsSpan(source.Source).SingleOrDefault(predicate, defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> SingleOrNone()
            => CollectionsMarshal.AsSpan(source.Source).SingleOrNone();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> SingleOrNone<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => CollectionsMarshal.AsSpan(source.Source).SingleOrNone(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> SingleOrNone(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source.Source).SingleOrNone(predicate);
    }
}
