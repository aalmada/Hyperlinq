using System.Collections.Generic;
using System.Linq;

namespace NetFabric.Hyperlinq.Benchmarks.Baseline
{
    public static class LinqBenchmarks
    {
        public static int WhereCount(IEnumerable<int> source)
        {
            var count = 0;
            foreach (var item in source.Where(x => x % 2 == 0))
            {
                count++;
            }
            return count;
        }

        public static int SelectCount(IEnumerable<int> source)
        {
            var count = 0;
            foreach (var item in source.Select(x => x * 2))
            {
                count++;
            }
            return count;
        }
    }
}
