using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ArrayExtensions
{
    extension<TSource>(TSource[] source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TSource MinBy<TKey>(Func<TSource, TKey> selector)
            where TKey : IComparable<TKey>
            => source.AsSpan().MinBy(selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TSource MinBy<TKey, TSelector>(TSelector selector)
            where TSelector : struct, IFunction<TSource, TKey>
            where TKey : IComparable<TKey>
            => source.AsSpan().MinBy<TSource, TKey, TSelector>(selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TSource MinBy<TKey, TSelector>(in TSelector selector)
            where TSelector : struct, IFunctionIn<TSource, TKey>
            where TKey : IComparable<TKey>
            => source.AsSpan().MinBy<TSource, TKey, TSelector>(in selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<TSource> MinByOrNone<TKey>(Func<TSource, TKey> selector)
            where TKey : IComparable<TKey>
            => source.AsSpan().MinByOrNone(selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<TSource> MinByOrNone<TKey, TSelector>(TSelector selector)
            where TSelector : struct, IFunction<TSource, TKey>
            where TKey : IComparable<TKey>
            => source.AsSpan().MinByOrNone<TSource, TKey, TSelector>(selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<TSource> MinByOrNone<TKey, TSelector>(in TSelector selector)
            where TSelector : struct, IFunctionIn<TSource, TKey>
            where TKey : IComparable<TKey>
            => source.AsSpan().MinByOrNone<TSource, TKey, TSelector>(in selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TSource MaxBy<TKey>(Func<TSource, TKey> selector)
            where TKey : IComparable<TKey>
            => source.AsSpan().MaxBy(selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TSource MaxBy<TKey, TSelector>(TSelector selector)
            where TSelector : struct, IFunction<TSource, TKey>
            where TKey : IComparable<TKey>
            => source.AsSpan().MaxBy<TSource, TKey, TSelector>(selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TSource MaxBy<TKey, TSelector>(in TSelector selector)
            where TSelector : struct, IFunctionIn<TSource, TKey>
            where TKey : IComparable<TKey>
            => source.AsSpan().MaxBy<TSource, TKey, TSelector>(in selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<TSource> MaxByOrNone<TKey>(Func<TSource, TKey> selector)
            where TKey : IComparable<TKey>
            => source.AsSpan().MaxByOrNone(selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<TSource> MaxByOrNone<TKey, TSelector>(TSelector selector)
            where TSelector : struct, IFunction<TSource, TKey>
            where TKey : IComparable<TKey>
            => source.AsSpan().MaxByOrNone<TSource, TKey, TSelector>(selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<TSource> MaxByOrNone<TKey, TSelector>(in TSelector selector)
            where TSelector : struct, IFunctionIn<TSource, TKey>
            where TKey : IComparable<TKey>
            => source.AsSpan().MaxByOrNone<TSource, TKey, TSelector>(in selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (TSource Min, TSource Max) MinMaxBy<TKey>(Func<TSource, TKey> selector)
            where TKey : IComparable<TKey>
            => source.AsSpan().MinMaxBy(selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (TSource Min, TSource Max) MinMaxBy<TKey, TSelector>(TSelector selector)
            where TSelector : struct, IFunction<TSource, TKey>
            where TKey : IComparable<TKey>
            => source.AsSpan().MinMaxBy<TSource, TKey, TSelector>(selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (TSource Min, TSource Max) MinMaxBy<TKey, TSelector>(in TSelector selector)
            where TSelector : struct, IFunctionIn<TSource, TKey>
            where TKey : IComparable<TKey>
            => source.AsSpan().MinMaxBy<TSource, TKey, TSelector>(in selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<(TSource Min, TSource Max)> MinMaxByOrNone<TKey>(Func<TSource, TKey> selector)
            where TKey : IComparable<TKey>
            => source.AsSpan().MinMaxByOrNone(selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<(TSource Min, TSource Max)> MinMaxByOrNone<TKey, TSelector>(TSelector selector)
            where TSelector : struct, IFunction<TSource, TKey>
            where TKey : IComparable<TKey>
            => source.AsSpan().MinMaxByOrNone<TSource, TKey, TSelector>(selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<(TSource Min, TSource Max)> MinMaxByOrNone<TKey, TSelector>(in TSelector selector)
            where TSelector : struct, IFunctionIn<TSource, TKey>
            where TKey : IComparable<TKey>
            => source.AsSpan().MinMaxByOrNone<TSource, TKey, TSelector>(in selector);
    }
}
