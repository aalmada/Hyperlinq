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
            => source.MinOrNone().Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Min<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => source.MinOrNone(predicate).Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Min(Func<T, bool> predicate)
            => source.MinOrNone(predicate).Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Max()
            => source.MaxOrNone().Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Max<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => source.MaxOrNone(predicate).Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Max(Func<T, bool> predicate)
            => source.MaxOrNone(predicate).Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (T Min, T Max) MinMax()
            => source.MinMaxOrNone().Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (T Min, T Max) MinMax(Func<T, bool> predicate)
            => source.MinMaxOrNone(predicate).Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (T Min, T Max) MinMax<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => source.MinMaxOrNone(predicate).Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> MinOrNone()
            => CollectionsMarshal.AsSpan(source.Source).MinOrNone();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> MinOrNone(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source.Source).MinOrNone(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> MaxOrNone()
            => CollectionsMarshal.AsSpan(source.Source).MaxOrNone();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> MaxOrNone(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source.Source).MaxOrNone(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<(T Min, T Max)> MinMaxOrNone()
            => CollectionsMarshal.AsSpan(source.Source).MinMaxOrNone();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<(T Min, T Max)> MinMaxOrNone(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source.Source).MinMaxOrNone(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> MinOrNone<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => CollectionsMarshal.AsSpan(source.Source).MinOrNone(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> MaxOrNone<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => CollectionsMarshal.AsSpan(source.Source).MaxOrNone(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<(T Min, T Max)> MinMaxOrNone<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => CollectionsMarshal.AsSpan(source.Source).MinMaxOrNone(predicate);
    }
}
