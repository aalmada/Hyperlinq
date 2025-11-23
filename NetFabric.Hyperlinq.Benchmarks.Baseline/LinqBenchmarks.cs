using System.Collections.Generic;
using System.Linq;

namespace NetFabric.Hyperlinq.Benchmarks.Baseline
{
    public static class LinqBenchmarks
    {
        public static int WhereSum(IEnumerable<int> source)
        {
            var sum = 0;
            foreach (var item in source.Where(x => x % 2 == 0))
            {
                sum += item;
            }
            return sum;
        }

        public static int SelectSum(IEnumerable<int> source)
        {
            var sum = 0;
            foreach (var item in source.Select(x => x * 2))
            {
                sum += item;
            }
            return sum;
        }
    }
}
