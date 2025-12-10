using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq
{
    public static partial class ReadOnlySpanExtensions
    {
        extension<TSource>(ReadOnlySpan<TSource> source)
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TSource MinBy<TKey>(Func<TSource, TKey> selector)
                where TKey : IComparable<TKey>
                => source.MinByOrNone(selector).Value;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TSource MinBy<TKey, TSelector>(TSelector selector)
                where TSelector : struct, IFunction<TSource, TKey>
                where TKey : IComparable<TKey>
                => source.MinByOrNone<TSource, TKey, TSelector>(selector).Value;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TSource MinBy<TKey, TSelector>(in TSelector selector)
                where TSelector : struct, IFunctionIn<TSource, TKey>
                where TKey : IComparable<TKey>
                => source.MinByOrNone<TSource, TKey, TSelector>(selector).Value;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<TSource> MinByOrNone<TKey>(Func<TSource, TKey> selector)
                where TKey : IComparable<TKey>
                => MinByOrNoneImpl<TSource, TKey, FunctionWrapper<TSource, TKey>>(source, new FunctionWrapper<TSource, TKey>(selector));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<TSource> MinByOrNone<TKey, TSelector>(TSelector selector)
                where TSelector : struct, IFunction<TSource, TKey>
                where TKey : IComparable<TKey>
                => MinByOrNoneImpl<TSource, TKey, TSelector>(source, selector);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<TSource> MinByOrNone<TKey, TSelector>(in TSelector selector)
                where TSelector : struct, IFunctionIn<TSource, TKey>
                where TKey : IComparable<TKey>
                => MinByOrNoneInImpl<TSource, TKey, TSelector>(source, selector);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TSource MaxBy<TKey>(Func<TSource, TKey> selector)
                where TKey : IComparable<TKey>
                => source.MaxByOrNone(selector).Value;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TSource MaxBy<TKey, TSelector>(TSelector selector)
                where TSelector : struct, IFunction<TSource, TKey>
                where TKey : IComparable<TKey>
                => source.MaxByOrNone<TSource, TKey, TSelector>(selector).Value;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TSource MaxBy<TKey, TSelector>(in TSelector selector)
                where TSelector : struct, IFunctionIn<TSource, TKey>
                where TKey : IComparable<TKey>
                => source.MaxByOrNone<TSource, TKey, TSelector>(selector).Value;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<TSource> MaxByOrNone<TKey>(Func<TSource, TKey> selector)
                where TKey : IComparable<TKey>
                => MaxByOrNoneImpl<TSource, TKey, FunctionWrapper<TSource, TKey>>(source, new FunctionWrapper<TSource, TKey>(selector));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<TSource> MaxByOrNone<TKey, TSelector>(TSelector selector)
                where TSelector : struct, IFunction<TSource, TKey>
                where TKey : IComparable<TKey>
                => MaxByOrNoneImpl<TSource, TKey, TSelector>(source, selector);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<TSource> MaxByOrNone<TKey, TSelector>(in TSelector selector)
                where TSelector : struct, IFunctionIn<TSource, TKey>
                where TKey : IComparable<TKey>
                => MaxByOrNoneInImpl<TSource, TKey, TSelector>(source, selector);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public (TSource Min, TSource Max) MinMaxBy<TKey>(Func<TSource, TKey> selector)
                where TKey : IComparable<TKey>
                => source.MinMaxByOrNone(selector).Value;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public (TSource Min, TSource Max) MinMaxBy<TKey, TSelector>(TSelector selector)
                where TSelector : struct, IFunction<TSource, TKey>
                where TKey : IComparable<TKey>
                => source.MinMaxByOrNone<TSource, TKey, TSelector>(selector).Value;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public (TSource Min, TSource Max) MinMaxBy<TKey, TSelector>(in TSelector selector)
                where TSelector : struct, IFunctionIn<TSource, TKey>
                where TKey : IComparable<TKey>
                => source.MinMaxByOrNone<TSource, TKey, TSelector>(selector).Value;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<(TSource Min, TSource Max)> MinMaxByOrNone<TKey>(Func<TSource, TKey> selector)
                where TKey : IComparable<TKey>
                => MinMaxByOrNoneImpl<TSource, TKey, FunctionWrapper<TSource, TKey>>(source, new FunctionWrapper<TSource, TKey>(selector));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<(TSource Min, TSource Max)> MinMaxByOrNone<TKey, TSelector>(TSelector selector)
                where TSelector : struct, IFunction<TSource, TKey>
                where TKey : IComparable<TKey>
                => MinMaxByOrNoneImpl<TSource, TKey, TSelector>(source, selector);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<(TSource Min, TSource Max)> MinMaxByOrNone<TKey, TSelector>(in TSelector selector)
                where TSelector : struct, IFunctionIn<TSource, TKey>
                where TKey : IComparable<TKey>
                => MinMaxByOrNoneInImpl<TSource, TKey, TSelector>(source, selector);
        }

        static Option<TSource> MinByOrNoneImpl<TSource, TKey, TSelector>(ReadOnlySpan<TSource> source, TSelector selector)
            where TSelector : struct, IFunction<TSource, TKey>
            where TKey : IComparable<TKey>
        {
            if (source.Length == 0)
                return Option<TSource>.None();

            var min = source[0];
            var minKey = selector.Invoke(min);

            foreach (var item in source[1..])
            {
                var key = selector.Invoke(item);
                if (key.CompareTo(minKey) < 0)
                {
                    min = item;
                    minKey = key;
                }
            }

            return Option<TSource>.Some(min);
        }

        static Option<TSource> MinByOrNoneInImpl<TSource, TKey, TSelector>(ReadOnlySpan<TSource> source, TSelector selector)
            where TSelector : struct, IFunctionIn<TSource, TKey>
            where TKey : IComparable<TKey>
        {
            if (source.Length == 0)
                return Option<TSource>.None();

            var min = source[0];
            var minKey = selector.Invoke(in min);

            foreach (ref readonly var item in source[1..])
            {
                var key = selector.Invoke(in item);
                if (key.CompareTo(minKey) < 0)
                {
                    min = item;
                    minKey = key;
                }
            }

            return Option<TSource>.Some(min);
        }

        static Option<TSource> MaxByOrNoneImpl<TSource, TKey, TSelector>(ReadOnlySpan<TSource> source, TSelector selector)
            where TSelector : struct, IFunction<TSource, TKey>
            where TKey : IComparable<TKey>
        {
            if (source.Length == 0)
                return Option<TSource>.None();

            var max = source[0];
            var maxKey = selector.Invoke(max);

            foreach (var item in source[1..])
            {
                var key = selector.Invoke(item);
                if (key.CompareTo(maxKey) > 0)
                {
                    max = item;
                    maxKey = key;
                }
            }

            return Option<TSource>.Some(max);
        }

        static Option<TSource> MaxByOrNoneInImpl<TSource, TKey, TSelector>(ReadOnlySpan<TSource> source, TSelector selector)
            where TSelector : struct, IFunctionIn<TSource, TKey>
            where TKey : IComparable<TKey>
        {
            if (source.Length == 0)
                return Option<TSource>.None();

            var max = source[0];
            var maxKey = selector.Invoke(in max);

            foreach (ref readonly var item in source[1..])
            {
                var key = selector.Invoke(in item);
                if (key.CompareTo(maxKey) > 0)
                {
                    max = item;
                    maxKey = key;
                }
            }

            return Option<TSource>.Some(max);
        }

        static Option<(TSource Min, TSource Max)> MinMaxByOrNoneImpl<TSource, TKey, TSelector>(ReadOnlySpan<TSource> source, TSelector selector)
            where TSelector : struct, IFunction<TSource, TKey>
            where TKey : IComparable<TKey>
        {
            if (source.Length == 0)
                return Option<(TSource Min, TSource Max)>.None();

            var min = source[0];
            var minKey = selector.Invoke(min);
            var max = min;
            var maxKey = minKey;

            foreach (var item in source[1..])
            {
                var key = selector.Invoke(item);
                if (key.CompareTo(minKey) < 0)
                {
                    min = item;
                    minKey = key;
                }
                else if (key.CompareTo(maxKey) > 0)
                {
                    max = item;
                    maxKey = key;
                }
            }

            return Option<(TSource Min, TSource Max)>.Some((min, max));
        }

        static Option<(TSource Min, TSource Max)> MinMaxByOrNoneInImpl<TSource, TKey, TSelector>(ReadOnlySpan<TSource> source, TSelector selector)
            where TSelector : struct, IFunctionIn<TSource, TKey>
            where TKey : IComparable<TKey>
        {
            if (source.Length == 0)
                return Option<(TSource Min, TSource Max)>.None();

            var min = source[0];
            var minKey = selector.Invoke(in min);
            var max = min;
            var maxKey = minKey;

            foreach (ref readonly var item in source[1..])
            {
                var key = selector.Invoke(in item);
                if (key.CompareTo(minKey) < 0)
                {
                    min = item;
                    minKey = key;
                }
                else if (key.CompareTo(maxKey) > 0)
                {
                    max = item;
                    maxKey = key;
                }
            }

            return Option<(TSource Min, TSource Max)>.Some((min, max));
        }
    }
}
