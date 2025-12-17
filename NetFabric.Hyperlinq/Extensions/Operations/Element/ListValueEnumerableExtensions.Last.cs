using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq;

public static partial class ListValueEnumerableExtensions
{
    extension<T>(ListValueEnumerable<T> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Last()
            => CollectionsMarshal.AsSpan(source.Source).Last();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Last<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => CollectionsMarshal.AsSpan(source.Source).Last(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Last(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source.Source).Last(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> LastOrNone()
            => CollectionsMarshal.AsSpan(source.Source).LastOrNone();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> LastOrNone<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => CollectionsMarshal.AsSpan(source.Source).LastOrNone(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> LastOrNone(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source.Source).LastOrNone(predicate);
    }
}
