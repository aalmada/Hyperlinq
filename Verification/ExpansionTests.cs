using System;
using System.Buffers;
using NetFabric.Hyperlinq;

namespace Verification
{
    public static class ExpansionTests
    {
        public static void Run()
        {
            Console.WriteLine("Running Expansion Tests...");

            using var pool = new TrackingArrayPool<int>();

            // 1. ArrayValueEnumerable.ToArrayPooled
            Console.WriteLine("Testing ArrayValueEnumerable.ToArrayPooled...");
            var array = new[] { 1, 2, 3, 4, 5 };
            using (var buffer = array.AsValueEnumerable().ToArrayPooled(pool))
            {
                if (buffer.Length != 5) throw new Exception("Length mismatch");
                if (pool.RentCalls == 0) throw new Exception("Did not rent from pool");
                Console.WriteLine("  Verified.");
            }
            if (pool.ReturnCalls == 0) throw new Exception("Did not return to pool");

            // 2. SelectArrayEnumerable.ToArrayPooled (Known Length)
            Console.WriteLine("Testing SelectArrayEnumerable.ToArrayPooled...");
            using (var buffer = array.AsValueEnumerable().Select(x => x * 2).ToArrayPooled(pool))
            {
                 if (buffer.Length != 5) throw new Exception("Length mismatch");
                 if (buffer.AsSpan()[0] != 2) throw new Exception("Value mismatch");
                 Console.WriteLine("  Verified.");
            }

            // 3. SelectEnumerable.ToArrayPooled (Unknown Length - via generic IEnumerable)
            Console.WriteLine("Testing SelectEnumerable.ToArrayPooled (Unknown Length)...");
            var enumerable = GetEnumerable();
            // Note: AsValueEnumerable() on IEnumerable<T> returns EnumerableValueEnumerable<T>
            // We want to force SelectEnumerable (generic) which we modified.
            // EnumerableValueEnumerable<T>.Select returns SelectEnumerable<T, TResult>.
            // So:
            using (var buffer = enumerable.AsValueEnumerable().Select(x => x * 2).ToArrayPooled(pool))
            {
                 if (buffer.Length != 5) throw new Exception("Length mismatch");
                 if (buffer.AsSpan()[0] != 2) throw new Exception("Value mismatch");
                 Console.WriteLine("  Verified.");
            }
            
             // 4. WhereEnumerable.ToArrayPooled (Unknown Length)
            Console.WriteLine("Testing WhereEnumerable.ToArrayPooled...");
            using (var buffer = enumerable.AsValueEnumerable().Where(x => x > 2).ToArrayPooled(pool))
            {
                 if (buffer.Length != 3) throw new Exception("Length mismatch");
                 Console.WriteLine("  Verified.");
            }

            Console.WriteLine("Expansion Tests Passed!");
        }

        static System.Collections.Generic.IEnumerable<int> GetEnumerable()
        {
            yield return 1;
            yield return 2;
            yield return 3;
            yield return 4;
            yield return 5;
        }
    }
}
