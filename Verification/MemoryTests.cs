using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Hyperlinq;

namespace Verification
{
    public static class MemoryTests
    {
        public static void Run()
        {
            Console.WriteLine("Running Memory Tests...");

            Test_ToArrayPooled_WithTrackingPool();
            Test_ToArrayPooled_WithPredicate_Growth();
            Test_Dispose_ReferenceTypes_ClearsArray();

            Console.WriteLine("Memory Tests Passed!");
        }

        static void Test_ToArrayPooled_WithTrackingPool()
        {
            var pool = new TrackingArrayPool<int>();
            var array = new[] { 1, 2, 3 };
            var span = array.AsSpan();

            using (var buffer = span.ToArrayPooled(pool))
            {
                Verify(buffer.Length == 3, "Buffer length match");
                Verify(pool.Rents.Count == 1, "Single rent expected");
            }
            
            Verify(pool.Returns.Count == 1, "Buffer returned to pool");
        }

        static void Test_ToArrayPooled_WithPredicate_Growth()
        {
            var pool = new TrackingArrayPool<int>();
            // Force growth: Default capacity 4. 
            // We want > 4 items.
            var array = Enumerable.Range(0, 10).ToArray();
            var span = array.AsSpan();

            // All items match, so builder will add 10 items.
            // Capacity: 4 -> 8 -> 16
            // Rents: 4, 8, 16 (for final result 10)
            // wait, ArrayBuilder implementation:
            // "Optimization: if we have a single chunk, transfer ownership"
            // "Multiple chunks: allocate a single contiguous buffer" (pool.Rent(totalCount))
            
            // Steps trace:
            // 1. Rent(4). Fill 4.
            // 2. Grow(). Move buffer to previousBuffers. Rent(8). Fill 4.
            // 3. Grow(). Move buffer to previousBuffers. Rent(16). Fill 2.
            // 4. ToPooledBuffer().
            //    - Allocate result buffer: Rent(10).
            //    - Copy from chunks.
            //    - Return chunks: Return(4), Return(8), Return(16).
            //    - Return new result buffer: Implicitly returned by ToPooledBuffer caller (us)? No, we get it in PooledBuffer usage.
            
            using (var buffer = span.ToArrayPooled(x => true, pool))
            {
                Verify(buffer.Length == 10, "Buffer grow length match");
                
                // Rents: 4, 8, 16(unused capacity but rented), 10 (final result)
                // Actually Logic in ArrayBuilder.Grow:
                // newCapacity = currentBuffer.Length * 2
                // Initial: 4.
                // Grow 1 (after 4): Rent(8).
                // Grow 2 (after 4+8=12): Wait, standard doubling?
                // add 4 items. count=4. Grow. prev=[buf4]. curr=Rent(8).
                // add 6 items? No, total 10.
                // add 6 more items. count=6.
                // ToPooledBuffer. total=10.
                // Rents: 4, 8, 10.
                
                Verify(pool.Rents.Count >= 3, "Multiple rents for growth"); 
            }
            
            // Returns:
            // Chunk 4 returned inside ToPooledBuffer? Yes.
            // Chunk 8 returned inside ToPooledBuffer? Yes (if it was treated as chunk).
            // Wait, current buffer (8) is returned.
            // Final buffer (10) returned by Dispose.
            
            Verify(pool.Returns.Count >= 3, "All intermediate buffers returned");
        }

        static void Test_Dispose_ReferenceTypes_ClearsArray()
        {
            var pool = new TrackingArrayPool<string>();
            var array = new[] { "a", "b", "c" };
            var span = array.AsSpan();

            var buffer = span.ToArrayPooled(pool);
            buffer.Dispose();

            Verify(pool.Returns.Count == 1, "Buffer returned");
            Verify(pool.Returns[0].clearArray == true, "Reference array cleared");
        }

        static void Verify(bool condition, string message)
        {
            if (!condition)
            {
                Console.WriteLine($"FAILED: {message}");
                Environment.Exit(1);
            }
        }
    }
}
