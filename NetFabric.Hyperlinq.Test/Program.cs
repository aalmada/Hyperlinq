using System;
using System.Collections.Generic;
using System.Linq;

namespace NetFabric.Hyperlinq.Test;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Testing AsValueEnumerable with LINQ methods...\n");

        var list = new List<int> { 1, 2, 3, 4, 5 };
        var valueEnum = list.AsValueEnumerable();

        // Test Any
        Console.WriteLine($"Any: {valueEnum.Any()}");

        // Test Count
        Console.WriteLine($"Count: {valueEnum.Count()}");

        // Test First
        Console.WriteLine($"First: {valueEnum.First()}");

        // Test Single (with single element)
        var singleList = new List<int> { 42 };
        Console.WriteLine($"Single: {singleList.AsValueEnumerable().Single()}");

        // Test Sum
        Console.WriteLine($"Sum: {valueEnum.Sum()}");

        Console.WriteLine("\nTesting chained operations:");
        var result = list.AsValueEnumerable()
                        .Where(x => x % 2 == 0)
                        .Select(x => x * 10);

        Console.WriteLine($"Where/Select Count: {result.Count()}");
        Console.WriteLine($"Where/Select Sum: {result.Sum()}");
        Console.Write("Where/Select values: ");
        foreach (var item in result)
        {
            Console.Write($"{item} ");
        }

        Console.WriteLine();

        Console.WriteLine("\nTesting with array:");
        var array = new int[] { 10, 20, 30 };
        var arrayValueEnum = array.AsValueEnumerable();
        Console.WriteLine($"Array Count: {arrayValueEnum.Count()}");
        Console.WriteLine($"Array Sum: {arrayValueEnum.Sum()}");
        Console.WriteLine($"Array Any: {arrayValueEnum.Any()}");
    }
}
