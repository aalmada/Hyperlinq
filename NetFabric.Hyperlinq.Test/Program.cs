using System;
using System.Collections.Generic;
using System.Linq;

namespace NetFabric.Hyperlinq.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<int> list = new List<int> { 1, 2, 3 };
            
            Console.WriteLine("--- Any ---");
            var any = list.Any();
            Console.WriteLine($"Any: {any}");

            Console.WriteLine("--- Count ---");
            var count = list.Count();
            Console.WriteLine($"Count: {count}");

            Console.WriteLine("--- First ---");
            var first = list.First();
            Console.WriteLine($"First: {first}");

            Console.WriteLine("--- Single (Exception expected) ---");
            try {
                var single = list.Single();
                Console.WriteLine($"Single: {single}");
            } catch (Exception ex) {
                Console.WriteLine($"Single threw: {ex.Message}");
            }

            Console.WriteLine("--- Select ---");
            var select = list.Select(x => x * 2);
            foreach (var item in select) Console.Write(item + " ");
            Console.WriteLine();

            Console.WriteLine("--- Where ---");
            foreach (var item in list.Where(i => i % 2 == 0))
            {
                Console.Write($"{item} ");
            }
            Console.WriteLine();

            Console.WriteLine("--- Where().Select() ---");
            foreach (var item in list.Where(i => i % 2 == 0).Select(i => i * 10))
            {
                Console.Write($"{item} ");
            }
            Console.WriteLine();

            Console.WriteLine("--- Sum (int) ---");
            var sumInt = list.Sum();
            Console.WriteLine($"Sum: {sumInt}");

            Console.WriteLine("--- Sum (double) ---");
            IEnumerable<double> doubleList = new List<double> { 1.5, 2.5, 3.5 };
            var sumDouble = doubleList.Sum();
            Console.WriteLine($"Sum: {sumDouble}");

            Console.WriteLine("--- Sum (array) ---");
            IEnumerable<int> array = new int[] { 10, 20, 30, 40, 50 };
            var sumArray = array.Sum();
            Console.WriteLine($"Sum: {sumArray}");
        }
    }
}
