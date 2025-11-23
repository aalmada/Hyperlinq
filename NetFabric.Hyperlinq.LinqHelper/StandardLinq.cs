using System;
using System.Collections.Generic;
using System.Linq;

namespace NetFabric.Hyperlinq.LinqHelper
{
    /// <summary>
    /// Provides standard LINQ methods without Hyperlinq interception.
    /// This class is in a separate project that does NOT reference the source generator.
    /// </summary>
    public static class StandardLinq
    {
        public static bool Any<T>(IEnumerable<T> source)
            => Enumerable.Any(source);

        public static int Count<T>(IEnumerable<T> source)
            => Enumerable.Count(source);

        public static T First<T>(IEnumerable<T> source)
            => Enumerable.First(source);

        public static T Single<T>(IEnumerable<T> source)
            => Enumerable.Single(source);

        public static IEnumerable<TResult> Select<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, TResult> selector)
            => Enumerable.Select(source, selector);

        public static IEnumerable<TSource> Where<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
            => Enumerable.Where(source, predicate);

        public static int SumInt(IEnumerable<int> source)
            => Enumerable.Sum(source);

        public static long SumLong(IEnumerable<long> source)
            => Enumerable.Sum(source);

        public static float SumFloat(IEnumerable<float> source)
            => Enumerable.Sum(source);

        public static double SumDouble(IEnumerable<double> source)
            => Enumerable.Sum(source);
    }
}
