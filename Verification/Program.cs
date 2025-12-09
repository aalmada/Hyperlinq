using System;
using System.Collections.Generic;
using NetFabric.Hyperlinq;

public class Program
{
    public static void Main()
    {
        Console.WriteLine("Verifying Fusion Operations...");

        var array = new int[] { 1, 2, 3, 4, 5 };
        var list = new List<int> { 1, 2, 3, 4, 5 };

        // Test ArrayValueEnumerable
        var arrayWrapper = array.AsValueEnumerable();
        
        Console.WriteLine($"Array Sum: {arrayWrapper.Sum()} (Expected 15)");
        Verify(arrayWrapper.Sum() == 15, "Array Sum");

        Console.WriteLine($"Array Sum Predicate: {arrayWrapper.Sum(x => x > 2)} (Expected 12)");
        Verify(arrayWrapper.Sum(x => x > 2) == 12, "Array Sum Predicate");

        Console.WriteLine($"Array ToList: {arrayWrapper.ToList().Count} items");
        Verify(arrayWrapper.ToList().Count == 5, "Array ToList");

        Console.WriteLine($"Array First: {arrayWrapper.First()} (Expected 1)");
        Verify(arrayWrapper.First() == 1, "Array First");

        Console.WriteLine($"Array First Predicate: {arrayWrapper.First(x => x > 3)} (Expected 4)");
        Verify(arrayWrapper.First(x => x > 3) == 4, "Array First Predicate");

        // Test ListValueEnumerable
        var listWrapper = list.AsValueEnumerable();
        
        Console.WriteLine($"List Sum: {listWrapper.Sum()} (Expected 15)");
        Verify(listWrapper.Sum() == 15, "List Sum");

        Console.WriteLine($"List Sum Predicate: {listWrapper.Sum(x => x > 2)} (Expected 12)");
        Verify(listWrapper.Sum(x => x > 2) == 12, "List Sum Predicate");

        Console.WriteLine($"List ToList: {listWrapper.ToList().Count} items");
        Verify(listWrapper.ToList().Count == 5, "List ToList");

        Console.WriteLine($"List First: {listWrapper.First()} (Expected 1)");
        Verify(listWrapper.First() == 1, "List First");

        Console.WriteLine($"List First Predicate: {listWrapper.First(x => x > 3)} (Expected 4)");
        Verify(listWrapper.First(x => x > 3) == 4, "List First Predicate");

        Console.WriteLine("All checks passed!");
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
