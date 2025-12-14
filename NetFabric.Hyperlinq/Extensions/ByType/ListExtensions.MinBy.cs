using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq;

public static partial class ListExtensions
{
    extension<TSource>(List<TSource> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TSource MinBy<TKey>(Func<TSource, TKey> selector)
            where TKey : IComparable<TKey>
            => CollectionsMarshal.AsSpan(source).MinBy(selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TSource MinBy<TKey, TSelector>(TSelector selector)
            where TSelector : struct, IFunction<TSource, TKey>
            where TKey : IComparable<TKey>
            => CollectionsMarshal.AsSpan(source).MinBy<TSource, TKey, TSelector>(selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TSource MinBy<TKey, TSelector>(in TSelector selector)
            where TSelector : struct, IFunctionIn<TSource, TKey>
            where TKey : IComparable<TKey>
            => CollectionsMarshal.AsSpan(source).MinBy<TSource, TKey, TSelector>(in selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<TSource> MinByOrNone<TKey>(Func<TSource, TKey> selector)
            where TKey : IComparable<TKey>
            => CollectionsMarshal.AsSpan(source).MinByOrNone(selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<TSource> MinByOrNone<TKey, TSelector>(TSelector selector)
            where TSelector : struct, IFunction<TSource, TKey>
            where TKey : IComparable<TKey>
            => CollectionsMarshal.AsSpan(source).MinByOrNone<TSource, TKey, TSelector>(selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<TSource> MinByOrNone<TKey, TSelector>(in TSelector selector)
            where TSelector : struct, IFunctionIn<TSource, TKey>
            where TKey : IComparable<TKey>
            => CollectionsMarshal.AsSpan(source).MinByOrNone<TSource, TKey, TSelector>(in selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TSource MaxBy<TKey>(Func<TSource, TKey> selector)
            where TKey : IComparable<TKey>
            => CollectionsMarshal.AsSpan(source).MaxBy(selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TSource MaxBy<TKey, TSelector>(TSelector selector)
            where TSelector : struct, IFunction<TSource, TKey>
            where TKey : IComparable<TKey>
            => CollectionsMarshal.AsSpan(source).MaxBy<TSource, TKey, TSelector>(selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TSource MaxBy<TKey, TSelector>(in TSelector selector)
            where TSelector : struct, IFunctionIn<TSource, TKey>
            where TKey : IComparable<TKey>
            => CollectionsMarshal.AsSpan(source).MaxBy<TSource, TKey, TSelector>(in selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<TSource> MaxByOrNone<TKey>(Func<TSource, TKey> selector)
            where TKey : IComparable<TKey>
            => CollectionsMarshal.AsSpan(source).MaxByOrNone(selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<TSource> MaxByOrNone<TKey, TSelector>(TSelector selector)
            where TSelector : struct, IFunction<TSource, TKey>
            where TKey : IComparable<TKey>
            => CollectionsMarshal.AsSpan(source).MaxByOrNone<TSource, TKey, TSelector>(selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<TSource> MaxByOrNone<TKey, TSelector>(in TSelector selector)
            where TSelector : struct, IFunctionIn<TSource, TKey>
            where TKey : IComparable<TKey>
            => CollectionsMarshal.AsSpan(source).MaxByOrNone<TSource, TKey, TSelector>(in selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (TSource Min, TSource Max) MinMaxBy<TKey>(Func<TSource, TKey> selector)
            where TKey : IComparable<TKey>
            => CollectionsMarshal.AsSpan(source).MinMaxBy(selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (TSource Min, TSource Max) MinMaxBy<TKey, TSelector>(TSelector selector)
            where TSelector : struct, IFunction<TSource, TKey>
            where TKey : IComparable<TKey>
            => CollectionsMarshal.AsSpan(source).MinMaxBy<TSource, TKey, TSelector>(selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (TSource Min, TSource Max) MinMaxBy<TKey, TSelector>(in TSelector selector)
            where TSelector : struct, IFunctionIn<TSource, TKey>
            where TKey : IComparable<TKey>
            => CollectionsMarshal.AsSpan(source).MinMaxBy<TSource, TKey, TSelector>(in selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<(TSource Min, TSource Max)> MinMaxByOrNone<TKey>(Func<TSource, TKey> selector)
            where TKey : IComparable<TKey>
            => CollectionsMarshal.AsSpan(source).MinMaxByOrNone(selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<(TSource Min, TSource Max)> MinMaxByOrNone<TKey, TSelector>(TSelector selector)
            where TSelector : struct, IFunction<TSource, TKey>
            where TKey : IComparable<TKey>
            => CollectionsMarshal.AsSpan(source).MinMaxByOrNone<TSource, TKey, TSelector>(selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<(TSource Min, TSource Max)> MinMaxByOrNone<TKey, TSelector>(in TSelector selector)
            where TSelector : struct, IFunctionIn<TSource, TKey>
            where TKey : IComparable<TKey>
            => CollectionsMarshal.AsSpan(source).MinMaxByOrNone<TSource, TKey, TSelector>(in selector);
    }
}
