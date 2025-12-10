using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class ValueEnumerableExtensions
    {
        public static TSource MinBy<TEnumerable, TEnumerator, TSource, TKey>(this TEnumerable source, Func<TSource, TKey> selector)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            where TKey : IComparable<TKey>
            => source.MinByOrNone<TEnumerable, TEnumerator, TSource, TKey>(selector).Value;

        public static TSource MinBy<TEnumerable, TEnumerator, TSource, TKey, TSelector>(this TEnumerable source, TSelector selector)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            where TSelector : struct, IFunction<TSource, TKey>
            where TKey : IComparable<TKey>
            => source.MinByOrNone<TEnumerable, TEnumerator, TSource, TKey, TSelector>(selector).Value;

        public static Option<TSource> MinByOrNone<TEnumerable, TEnumerator, TSource, TKey>(this TEnumerable source, Func<TSource, TKey> selector)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            where TKey : IComparable<TKey>
            => source.MinByOrNone<TEnumerable, TEnumerator, TSource, TKey, FunctionWrapper<TSource, TKey>>(new FunctionWrapper<TSource, TKey>(selector));

        public static Option<TSource> MinByOrNone<TEnumerable, TEnumerator, TSource, TKey, TSelector>(this TEnumerable source, TSelector selector)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            where TSelector : struct, IFunction<TSource, TKey>
            where TKey : IComparable<TKey>
        {
            using var enumerator = source.GetEnumerator();
            
            if (!enumerator.MoveNext())
                return Option<TSource>.None();
            
            var min = enumerator.Current;
            var minKey = selector.Invoke(min);
            
            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                var key = selector.Invoke(item);
                if (key.CompareTo(minKey) < 0)
                {
                    min = item;
                    minKey = key;
                }
            }
            
            return Option<TSource>.Some(min);
        }

        public static TSource MaxBy<TEnumerable, TEnumerator, TSource, TKey>(this TEnumerable source, Func<TSource, TKey> selector)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            where TKey : IComparable<TKey>
            => source.MaxByOrNone<TEnumerable, TEnumerator, TSource, TKey>(selector).Value;

        public static TSource MaxBy<TEnumerable, TEnumerator, TSource, TKey, TSelector>(this TEnumerable source, TSelector selector)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            where TSelector : struct, IFunction<TSource, TKey>
            where TKey : IComparable<TKey>
            => source.MaxByOrNone<TEnumerable, TEnumerator, TSource, TKey, TSelector>(selector).Value;

        public static Option<TSource> MaxByOrNone<TEnumerable, TEnumerator, TSource, TKey>(this TEnumerable source, Func<TSource, TKey> selector)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            where TKey : IComparable<TKey>
            => source.MaxByOrNone<TEnumerable, TEnumerator, TSource, TKey, FunctionWrapper<TSource, TKey>>(new FunctionWrapper<TSource, TKey>(selector));

        public static Option<TSource> MaxByOrNone<TEnumerable, TEnumerator, TSource, TKey, TSelector>(this TEnumerable source, TSelector selector)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            where TSelector : struct, IFunction<TSource, TKey>
            where TKey : IComparable<TKey>
        {
            using var enumerator = source.GetEnumerator();
            
            if (!enumerator.MoveNext())
                return Option<TSource>.None();
            
            var max = enumerator.Current;
            var maxKey = selector.Invoke(max);
            
            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                var key = selector.Invoke(item);
                if (key.CompareTo(maxKey) > 0)
                {
                    max = item;
                    maxKey = key;
                }
            }
            
            return Option<TSource>.Some(max);
        }

        public static (TSource Min, TSource Max) MinMaxBy<TEnumerable, TEnumerator, TSource, TKey>(this TEnumerable source, Func<TSource, TKey> selector)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            where TKey : IComparable<TKey>
            => source.MinMaxByOrNone<TEnumerable, TEnumerator, TSource, TKey>(selector).Value;

        public static (TSource Min, TSource Max) MinMaxBy<TEnumerable, TEnumerator, TSource, TKey, TSelector>(this TEnumerable source, TSelector selector)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            where TSelector : struct, IFunction<TSource, TKey>
            where TKey : IComparable<TKey>
            => source.MinMaxByOrNone<TEnumerable, TEnumerator, TSource, TKey, TSelector>(selector).Value;

        public static Option<(TSource Min, TSource Max)> MinMaxByOrNone<TEnumerable, TEnumerator, TSource, TKey>(this TEnumerable source, Func<TSource, TKey> selector)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            where TKey : IComparable<TKey>
            => source.MinMaxByOrNone<TEnumerable, TEnumerator, TSource, TKey, FunctionWrapper<TSource, TKey>>(new FunctionWrapper<TSource, TKey>(selector));

        public static Option<(TSource Min, TSource Max)> MinMaxByOrNone<TEnumerable, TEnumerator, TSource, TKey, TSelector>(this TEnumerable source, TSelector selector)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            where TSelector : struct, IFunction<TSource, TKey>
            where TKey : IComparable<TKey>
        {
            using var enumerator = source.GetEnumerator();
            
            if (!enumerator.MoveNext())
                return Option<(TSource Min, TSource Max)>.None();
            
            var min = enumerator.Current;
            var minKey = selector.Invoke(min);
            var max = min;
            var maxKey = minKey;
            
            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
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
    }
}
