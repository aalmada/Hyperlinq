using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ArrayValueEnumerableExtensions
{
    extension<T>(ArrayValueEnumerable<T> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Last()
            => source.Source.Last();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Last<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => source.Source.Last(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Last(Func<T, bool> predicate)
            => source.Source.Last(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> LastOrNone()
            => source.Source.LastOrNone();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> LastOrNone<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => source.Source.LastOrNone(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> LastOrNone(Func<T, bool> predicate)
            => source.Source.LastOrNone(predicate);
    }
}
