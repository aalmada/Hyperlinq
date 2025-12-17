using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ArrayValueEnumerableExtensions
{
    extension<T>(ArrayValueEnumerable<T> source)
        where T : struct, INumber<T>, IMinMaxValue<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Max()
            => source.Source.Max();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Max<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => source.Source.Max(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Max(Func<T, bool> predicate)
            => source.Source.Max(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> MaxOrNone()
            => source.Source.MaxOrNone();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> MaxOrNone(Func<T, bool> predicate)
            => source.Source.MaxOrNone(predicate);
    }
}
