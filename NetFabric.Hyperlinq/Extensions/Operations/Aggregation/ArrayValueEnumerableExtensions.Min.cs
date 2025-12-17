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
        public T Min()
            => source.Source.Min();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Min<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => source.Source.Min(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Min(Func<T, bool> predicate)
            => source.Source.Min(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (T Min, T Max) MinMax()
            => source.Source.MinMax();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (T Min, T Max) MinMax(Func<T, bool> predicate)
            => source.Source.MinMax(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> MinOrNone()
            => source.Source.MinOrNone();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> MinOrNone(Func<T, bool> predicate)
            => source.Source.MinOrNone(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<(T Min, T Max)> MinMaxOrNone()
            => source.Source.MinMaxOrNone();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<(T Min, T Max)> MinMaxOrNone(Func<T, bool> predicate)
            => source.Source.MinMaxOrNone(predicate);
    }
}
