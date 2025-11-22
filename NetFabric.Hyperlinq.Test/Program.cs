using System;
using System.Collections.Generic;
using System.Linq;

namespace NetFabric.Hyperlinq.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = new List<int> { 1, 2, 3 };
            
            Console.WriteLine("--- Any ---");
            var any = list.Any();
            Console.WriteLine($"Any: {any}");

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
            var where = list.Where(x => x > 1);
            foreach (var item in where) Console.Write(item + " ");
            Console.WriteLine();
        }
    }
}
