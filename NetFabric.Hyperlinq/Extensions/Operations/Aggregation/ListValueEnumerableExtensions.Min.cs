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
        public T Min()
            => CollectionsMarshal.AsSpan(source.Source).Min();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Min<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => CollectionsMarshal.AsSpan(source.Source).Min(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Min(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source.Source).Min(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (T Min, T Max) MinMax()
            => CollectionsMarshal.AsSpan(source.Source).MinMax();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (T Min, T Max) MinMax(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source.Source).MinMax(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> MinOrNone()
            => CollectionsMarshal.AsSpan(source.Source).MinOrNone();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> MinOrNone(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source.Source).MinOrNone(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<(T Min, T Max)> MinMaxOrNone()
            => CollectionsMarshal.AsSpan(source.Source).MinMaxOrNone();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<(T Min, T Max)> MinMaxOrNone(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source.Source).MinMaxOrNone(predicate);
    }
}
