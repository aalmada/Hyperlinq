using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq;

public static partial class ListValueEnumerableExtensions
{
    extension<T>(ListValueEnumerable<T> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T First()
            => CollectionsMarshal.AsSpan(source.Source).First();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T First<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => CollectionsMarshal.AsSpan(source.Source).First(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T First(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source.Source).First(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T FirstOrDefault()
            => CollectionsMarshal.AsSpan(source.Source).FirstOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T FirstOrDefault<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => CollectionsMarshal.AsSpan(source.Source).FirstOrDefault(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T FirstOrDefault(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source.Source).FirstOrDefault(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T FirstOrDefault<TPredicate>(TPredicate predicate, T defaultValue)
            where TPredicate : struct, IFunction<T, bool>
            => CollectionsMarshal.AsSpan(source.Source).FirstOrDefault(predicate, defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T FirstOrDefault(Func<T, bool> predicate, T defaultValue)
            => CollectionsMarshal.AsSpan(source.Source).FirstOrDefault(predicate, defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> FirstOrNone()
            => CollectionsMarshal.AsSpan(source.Source).FirstOrNone();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> FirstOrNone<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => CollectionsMarshal.AsSpan(source.Source).FirstOrNone(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> FirstOrNone(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source.Source).FirstOrNone(predicate);
    }
}
