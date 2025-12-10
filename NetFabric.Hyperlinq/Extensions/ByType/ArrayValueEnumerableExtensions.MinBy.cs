using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq
{
    public static partial class ArrayValueEnumerableExtensions
    {
        extension<TSource>(ArrayValueEnumerable<TSource> source)
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TSource MinBy<TKey>(Func<TSource, TKey> selector)
                where TKey : IComparable<TKey>
                => source.Source.AsSpan().MinBy(selector);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TSource MinBy<TKey, TSelector>(TSelector selector)
                where TSelector : struct, IFunction<TSource, TKey>
                where TKey : IComparable<TKey>
                => source.Source.AsSpan().MinBy<TSource, TKey, TSelector>(selector);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TSource MinBy<TKey, TSelector>(in TSelector selector)
                where TSelector : struct, IFunctionIn<TSource, TKey>
                where TKey : IComparable<TKey>
                => source.Source.AsSpan().MinBy<TSource, TKey, TSelector>(in selector);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<TSource> MinByOrNone<TKey>(Func<TSource, TKey> selector)
                where TKey : IComparable<TKey>
                => source.Source.AsSpan().MinByOrNone(selector);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<TSource> MinByOrNone<TKey, TSelector>(TSelector selector)
                where TSelector : struct, IFunction<TSource, TKey>
                where TKey : IComparable<TKey>
                => source.Source.AsSpan().MinByOrNone<TSource, TKey, TSelector>(selector);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<TSource> MinByOrNone<TKey, TSelector>(in TSelector selector)
                where TSelector : struct, IFunctionIn<TSource, TKey>
                where TKey : IComparable<TKey>
                => source.Source.AsSpan().MinByOrNone<TSource, TKey, TSelector>(in selector);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TSource MaxBy<TKey>(Func<TSource, TKey> selector)
                where TKey : IComparable<TKey>
                => source.Source.AsSpan().MaxBy(selector);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TSource MaxBy<TKey, TSelector>(TSelector selector)
                where TSelector : struct, IFunction<TSource, TKey>
                where TKey : IComparable<TKey>
                => source.Source.AsSpan().MaxBy<TSource, TKey, TSelector>(selector);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TSource MaxBy<TKey, TSelector>(in TSelector selector)
                where TSelector : struct, IFunctionIn<TSource, TKey>
                where TKey : IComparable<TKey>
                => source.Source.AsSpan().MaxBy<TSource, TKey, TSelector>(in selector);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<TSource> MaxByOrNone<TKey>(Func<TSource, TKey> selector)
                where TKey : IComparable<TKey>
                => source.Source.AsSpan().MaxByOrNone(selector);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<TSource> MaxByOrNone<TKey, TSelector>(TSelector selector)
                where TSelector : struct, IFunction<TSource, TKey>
                where TKey : IComparable<TKey>
                => source.Source.AsSpan().MaxByOrNone<TSource, TKey, TSelector>(selector);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<TSource> MaxByOrNone<TKey, TSelector>(in TSelector selector)
                where TSelector : struct, IFunctionIn<TSource, TKey>
                where TKey : IComparable<TKey>
                => source.Source.AsSpan().MaxByOrNone<TSource, TKey, TSelector>(in selector);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public (TSource Min, TSource Max) MinMaxBy<TKey>(Func<TSource, TKey> selector)
                where TKey : IComparable<TKey>
                => source.Source.AsSpan().MinMaxBy(selector);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public (TSource Min, TSource Max) MinMaxBy<TKey, TSelector>(TSelector selector)
                where TSelector : struct, IFunction<TSource, TKey>
                where TKey : IComparable<TKey>
                => source.Source.AsSpan().MinMaxBy<TSource, TKey, TSelector>(selector);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public (TSource Min, TSource Max) MinMaxBy<TKey, TSelector>(in TSelector selector)
                where TSelector : struct, IFunctionIn<TSource, TKey>
                where TKey : IComparable<TKey>
                => source.Source.AsSpan().MinMaxBy<TSource, TKey, TSelector>(in selector);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<(TSource Min, TSource Max)> MinMaxByOrNone<TKey>(Func<TSource, TKey> selector)
                where TKey : IComparable<TKey>
                => source.Source.AsSpan().MinMaxByOrNone(selector);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<(TSource Min, TSource Max)> MinMaxByOrNone<TKey, TSelector>(TSelector selector)
                where TSelector : struct, IFunction<TSource, TKey>
                where TKey : IComparable<TKey>
                => source.Source.AsSpan().MinMaxByOrNone<TSource, TKey, TSelector>(selector);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<(TSource Min, TSource Max)> MinMaxByOrNone<TKey, TSelector>(in TSelector selector)
                where TSelector : struct, IFunctionIn<TSource, TKey>
                where TKey : IComparable<TKey>
                => source.Source.AsSpan().MinMaxByOrNone<TSource, TKey, TSelector>(in selector);
        }
    }
}
