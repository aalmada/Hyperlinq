using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ReadOnlyMemoryExtensions
{
    extension<TSource>(ReadOnlyMemory<TSource> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TSource MinBy<TKey>(Func<TSource, TKey> selector)
            where TKey : IComparable<TKey>
            => source.Span.MinBy(selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TSource MinBy<TKey, TSelector>(TSelector selector)
            where TSelector : struct, IFunction<TSource, TKey>
            where TKey : IComparable<TKey>
            => source.Span.MinBy<TSource, TKey, TSelector>(selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TSource MinBy<TKey, TSelector>(in TSelector selector)
            where TSelector : struct, IFunctionIn<TSource, TKey>
            where TKey : IComparable<TKey>
            => source.Span.MinBy<TSource, TKey, TSelector>(in selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<TSource> MinByOrNone<TKey>(Func<TSource, TKey> selector)
            where TKey : IComparable<TKey>
            => source.Span.MinByOrNone(selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<TSource> MinByOrNone<TKey, TSelector>(TSelector selector)
            where TSelector : struct, IFunction<TSource, TKey>
            where TKey : IComparable<TKey>
            => source.Span.MinByOrNone<TSource, TKey, TSelector>(selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<TSource> MinByOrNone<TKey, TSelector>(in TSelector selector)
            where TSelector : struct, IFunctionIn<TSource, TKey>
            where TKey : IComparable<TKey>
            => source.Span.MinByOrNone<TSource, TKey, TSelector>(in selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TSource MaxBy<TKey>(Func<TSource, TKey> selector)
            where TKey : IComparable<TKey>
            => source.Span.MaxBy(selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TSource MaxBy<TKey, TSelector>(TSelector selector)
            where TSelector : struct, IFunction<TSource, TKey>
            where TKey : IComparable<TKey>
            => source.Span.MaxBy<TSource, TKey, TSelector>(selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TSource MaxBy<TKey, TSelector>(in TSelector selector)
            where TSelector : struct, IFunctionIn<TSource, TKey>
            where TKey : IComparable<TKey>
            => source.Span.MaxBy<TSource, TKey, TSelector>(in selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<TSource> MaxByOrNone<TKey>(Func<TSource, TKey> selector)
            where TKey : IComparable<TKey>
            => source.Span.MaxByOrNone(selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<TSource> MaxByOrNone<TKey, TSelector>(TSelector selector)
            where TSelector : struct, IFunction<TSource, TKey>
            where TKey : IComparable<TKey>
            => source.Span.MaxByOrNone<TSource, TKey, TSelector>(selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<TSource> MaxByOrNone<TKey, TSelector>(in TSelector selector)
            where TSelector : struct, IFunctionIn<TSource, TKey>
            where TKey : IComparable<TKey>
            => source.Span.MaxByOrNone<TSource, TKey, TSelector>(in selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (TSource Min, TSource Max) MinMaxBy<TKey>(Func<TSource, TKey> selector)
            where TKey : IComparable<TKey>
            => source.Span.MinMaxBy(selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (TSource Min, TSource Max) MinMaxBy<TKey, TSelector>(TSelector selector)
            where TSelector : struct, IFunction<TSource, TKey>
            where TKey : IComparable<TKey>
            => source.Span.MinMaxBy<TSource, TKey, TSelector>(selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (TSource Min, TSource Max) MinMaxBy<TKey, TSelector>(in TSelector selector)
            where TSelector : struct, IFunctionIn<TSource, TKey>
            where TKey : IComparable<TKey>
            => source.Span.MinMaxBy<TSource, TKey, TSelector>(in selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<(TSource Min, TSource Max)> MinMaxByOrNone<TKey>(Func<TSource, TKey> selector)
            where TKey : IComparable<TKey>
            => source.Span.MinMaxByOrNone(selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<(TSource Min, TSource Max)> MinMaxByOrNone<TKey, TSelector>(TSelector selector)
            where TSelector : struct, IFunction<TSource, TKey>
            where TKey : IComparable<TKey>
            => source.Span.MinMaxByOrNone<TSource, TKey, TSelector>(selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<(TSource Min, TSource Max)> MinMaxByOrNone<TKey, TSelector>(in TSelector selector)
            where TSelector : struct, IFunctionIn<TSource, TKey>
            where TKey : IComparable<TKey>
            => source.Span.MinMaxByOrNone<TSource, TKey, TSelector>(in selector);
    }
}
