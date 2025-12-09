using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using NetFabric.Hyperlinq;

namespace Verification
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TestCoreExtensions();
                TestImmutableExtensions();
                Console.WriteLine("All verification tests passed!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed: {ex}");
                Environment.Exit(1);
            }
        }

        static void TestCoreExtensions()
        {
            Console.WriteLine("Testing Core extensions...");
            
            var hashSet = new HashSet<int> { 1, 2, 3 };
            var hsWrapper = hashSet.AsValueEnumerable();
            if (hsWrapper.Count != 3) throw new Exception("HashSet Count mismatch");

            var linkedList = new LinkedList<int>();
            linkedList.AddLast(1);
            linkedList.AddLast(2);
            var llWrapper = linkedList.AsValueEnumerable();
            if (llWrapper.Count != 2) throw new Exception("LinkedList Count mismatch: " + llWrapper.Count);

            var queue = new Queue<int>();
            queue.Enqueue(1);
            var qWrapper = queue.AsValueEnumerable();
            int qCount = 0;
            foreach(var item in qWrapper) qCount++;
            if (qCount != 1) throw new Exception("Queue enumeration mismatch. Count: " + qCount);

            var stack = new Stack<int>();
            stack.Push(1);
            var sWrapper = stack.AsValueEnumerable();
            int sCount = 0;
            foreach(var item in sWrapper) sCount++;
            if (sCount != 1) throw new Exception("Stack enumeration mismatch. Count: " + sCount);

            var dict = new Dictionary<int, string> { { 1, "a" }, { 2, "b" } };
            var dictWrapper = dict.AsValueEnumerable();
            if (dictWrapper.Count != 2) throw new Exception("Dictionary Count mismatch");
            
            var keyWrapper = dict.Keys.AsValueEnumerable();
            if (keyWrapper.Count != 2) throw new Exception("Dictionary.Keys Count mismatch");

            var valueWrapper = dict.Values.AsValueEnumerable();
            if (valueWrapper.Count != 2) throw new Exception("Dictionary.Values Count mismatch");
        }

        static void TestImmutableExtensions()
        {
            Console.WriteLine("Testing Immutable extensions...");

            var array = ImmutableArray.Create(1, 2, 3);
            var arrayWrapper = array.AsValueEnumerable();
            if (arrayWrapper.Count != 3) throw new Exception("ImmutableArray Count mismatch");
            if (arrayWrapper[0] != 1) throw new Exception("ImmutableArray Indexer mismatch");

            var list = ImmutableList.Create(1, 2, 3);
            var listWrapper = list.AsValueEnumerable();
            if (listWrapper.Count != 3) throw new Exception("ImmutableList Count mismatch");
            if (listWrapper[0] != 1) throw new Exception("ImmutableList Indexer mismatch");

            var queue = ImmutableQueue.Create(1, 2, 3);
            var qWrapper = queue.AsValueEnumerable();
            int qCount = 0;
            foreach (var item in qWrapper) qCount++;
            if (qCount != 3) throw new Exception("ImmutableQueue enumeration mismatch. Count: " + qCount);

            var stack = ImmutableStack.Create(1, 2, 3);
            var sWrapper = stack.AsValueEnumerable();
            int sCount = 0;
            foreach (var item in sWrapper) sCount++;
            if (sCount != 3) throw new Exception("ImmutableStack enumeration mismatch. Count: " + sCount);

            var hashSet = ImmutableHashSet.Create(1, 2, 3);
            var hsWrapper = hashSet.AsValueEnumerable();
            if (hsWrapper.Count != 3) throw new Exception("ImmutableHashSet Count mismatch");

            var dict = ImmutableDictionary.CreateRange(new[] { new KeyValuePair<int, string>(1, "a"), new KeyValuePair<int, string>(2, "b") });
            var dictWrapper = dict.AsValueEnumerable();
            if (dictWrapper.Count != 2) throw new Exception("ImmutableDictionary Count mismatch");

            var sortedDict = ImmutableSortedDictionary.CreateRange(new[] { new KeyValuePair<int, string>(1, "a"), new KeyValuePair<int, string>(2, "b") });
            var sortedDictWrapper = sortedDict.AsValueEnumerable();
            if (sortedDictWrapper.Count != 2) throw new Exception("ImmutableSortedDictionary Count mismatch");
        }
    }
}
