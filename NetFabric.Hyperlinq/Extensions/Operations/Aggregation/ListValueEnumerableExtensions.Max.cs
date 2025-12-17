using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq;

public static partial class ListValueEnumerableExtensions
{
    extension<T>(ListValueEnumerable<T> source)
        where T : struct, INumber<T>, IMinMaxValue<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Max()
            => CollectionsMarshal.AsSpan(source.Source).Max();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Max<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => CollectionsMarshal.AsSpan(source.Source).Max(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Max(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source.Source).Max(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> MaxOrNone()
            => CollectionsMarshal.AsSpan(source.Source).MaxOrNone();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> MaxOrNone(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source.Source).MaxOrNone(predicate);
    }
}
