using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ArrayValueEnumerableExtensions
{
    extension<T>(ArrayValueEnumerable<T> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T First()
            => source.Source.First();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T First<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => source.Source.First(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T First(Func<T, bool> predicate)
            => source.Source.First(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T FirstOrDefault()
            => source.Source.FirstOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T FirstOrDefault<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => source.Source.FirstOrDefault(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T FirstOrDefault(Func<T, bool> predicate)
            => source.Source.FirstOrDefault(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T FirstOrDefault<TPredicate>(TPredicate predicate, T defaultValue)
            where TPredicate : struct, IFunction<T, bool>
            => source.Source.FirstOrDefault(predicate, defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T FirstOrDefault(Func<T, bool> predicate, T defaultValue)
            => source.Source.FirstOrDefault(predicate, defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> FirstOrNone()
            => source.Source.FirstOrNone();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> FirstOrNone<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => source.Source.FirstOrNone(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> FirstOrNone(Func<T, bool> predicate)
            => source.Source.FirstOrNone(predicate);
    }
}
