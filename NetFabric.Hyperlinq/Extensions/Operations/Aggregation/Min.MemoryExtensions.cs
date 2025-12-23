using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class MemoryExtensions
{
    extension<T>(Memory<T> source)
        where T : struct, INumber<T>, IMinMaxValue<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Min()
            => ((ReadOnlyMemory<T>)source).MinOrNone().Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Min<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => ((ReadOnlyMemory<T>)source).MinOrNone(predicate).Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Min(Func<T, bool> predicate)
            => ((ReadOnlyMemory<T>)source).MinOrNone(predicate).Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> MinOrNone()
            => ((ReadOnlyMemory<T>)source).MinOrNone();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> MinOrNone<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => ((ReadOnlyMemory<T>)source).MinOrNone(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> MinOrNone(Func<T, bool> predicate)
            => ((ReadOnlyMemory<T>)source).MinOrNone(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Max()
            => ((ReadOnlyMemory<T>)source).MaxOrNone().Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Max<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => ((ReadOnlyMemory<T>)source).MaxOrNone(predicate).Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Max(Func<T, bool> predicate)
            => ((ReadOnlyMemory<T>)source).MaxOrNone(predicate).Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> MaxOrNone()
            => ((ReadOnlyMemory<T>)source).MaxOrNone();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> MaxOrNone<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => ((ReadOnlyMemory<T>)source).MaxOrNone(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> MaxOrNone(Func<T, bool> predicate)
            => ((ReadOnlyMemory<T>)source).MaxOrNone(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (T Min, T Max) MinMax()
            => ((ReadOnlyMemory<T>)source).MinMaxOrNone().Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (T Min, T Max) MinMax<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => ((ReadOnlyMemory<T>)source).MinMaxOrNone(predicate).Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (T Min, T Max) MinMax(Func<T, bool> predicate)
            => ((ReadOnlyMemory<T>)source).MinMaxOrNone(predicate).Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<(T Min, T Max)> MinMaxOrNone()
            => ((ReadOnlyMemory<T>)source).MinMaxOrNone();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<(T Min, T Max)> MinMaxOrNone<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => ((ReadOnlyMemory<T>)source).MinMaxOrNone(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<(T Min, T Max)> MinMaxOrNone(Func<T, bool> predicate)
            => ((ReadOnlyMemory<T>)source).MinMaxOrNone(predicate);
    }
}
