using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using NetFabric.Hyperlinq;

namespace NetFabric.Hyperlinq.Benchmarks;

[MemoryDiagnoser]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class DualEnumerationBenchmarks
{
    private int[] array = null!;
    private List<int> list = null!;

    [Params(10_000)]
    public int Count { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        array = ValueEnumerable.Range(0, Count).ToArray();
        list = array.ToList();
    }

    // ===== Array Sum =====

    [BenchmarkCategory("Array_Sum"), Benchmark(Baseline = true)]
    public int Array_Sum_LINQ() => Enumerable.Sum(array);

    [BenchmarkCategory("Array_Sum"), Benchmark]
    public int Array_Sum_Hyperlinq_RefStruct() => array.Sum();

    [BenchmarkCategory("Array_Sum"), Benchmark]
    public int Array_Sum_Hyperlinq_AsValueEnumerable() => array.AsValueEnumerable().Sum();

    // ===== List Sum =====

    [BenchmarkCategory("List_Sum"), Benchmark(Baseline = true)]
    public int List_Sum_LINQ() => Enumerable.Sum(list);

    [BenchmarkCategory("List_Sum"), Benchmark]
    public int List_Sum_Hyperlinq_RefStruct() => list.Sum();

    [BenchmarkCategory("List_Sum"), Benchmark]
    public int List_Sum_Hyperlinq_AsValueEnumerable() => list.AsValueEnumerable().Sum();

    // ===== Array Where Sum =====

    [BenchmarkCategory("Array_Where_Sum"), Benchmark(Baseline = true)]
    public int Array_Where_Sum_LINQ() => Enumerable.Sum(Enumerable.Where(array, x => x % 2 == 0));

    [BenchmarkCategory("Array_Where_Sum"), Benchmark]
    public int Array_Where_Sum_Hyperlinq_RefStruct() => array.Where(x => x % 2 == 0).Sum();

    [BenchmarkCategory("Array_Where_Sum"), Benchmark]
    public int Array_Where_Sum_Hyperlinq_AsValueEnumerable() => array.AsValueEnumerable().Where(x => x % 2 == 0).Sum();

    // ===== List Where Sum =====

    [BenchmarkCategory("List_Where_Sum"), Benchmark(Baseline = true)]
    public int List_Where_Sum_LINQ() => Enumerable.Sum(Enumerable.Where(list, x => x % 2 == 0));

    [BenchmarkCategory("List_Where_Sum"), Benchmark]
    public int List_Where_Sum_Hyperlinq_RefStruct() => list.Where(x => x % 2 == 0).Sum();

    [BenchmarkCategory("List_Where_Sum"), Benchmark]
    public int List_Where_Sum_Hyperlinq_AsValueEnumerable() => list.AsValueEnumerable().Where(x => x % 2 == 0).Sum();

    // ===== Array Select Sum =====

    [BenchmarkCategory("Array_Select_Sum"), Benchmark(Baseline = true)]
    public int Array_Select_Sum_LINQ() => Enumerable.Sum(Enumerable.Select(array, x => x * 2));

    [BenchmarkCategory("Array_Select_Sum"), Benchmark]
    public int Array_Select_Sum_Hyperlinq_RefStruct() => array.AsValueEnumerable().Select(x => x * 2).Sum();

    [BenchmarkCategory("Array_Select_Sum"), Benchmark]
    public int Array_Select_Sum_Hyperlinq_AsValueEnumerable() => array.AsValueEnumerable().Select(x => x * 2).Sum();

    // ===== List Select Sum =====

    [BenchmarkCategory("List_Select_Sum"), Benchmark(Baseline = true)]
    public int List_Select_Sum_LINQ() => Enumerable.Sum(Enumerable.Select(list, x => x * 2));

    [BenchmarkCategory("List_Select_Sum"), Benchmark]
    public int List_Select_Sum_Hyperlinq_RefStruct() => list.AsValueEnumerable().Select(x => x * 2).Sum();

    [BenchmarkCategory("List_Select_Sum"), Benchmark]
    public int List_Select_Sum_Hyperlinq_AsValueEnumerable() => list.AsValueEnumerable().Select(x => x * 2).Sum();

    // ===== Array Where Select Sum =====

    [BenchmarkCategory("Array_WhereSelect_Sum"), Benchmark(Baseline = true)]
    public int Array_WhereSelect_Sum_LINQ() => Enumerable.Sum(Enumerable.Select(Enumerable.Where(array, x => x % 2 == 0), x => x * 2));

    [BenchmarkCategory("Array_WhereSelect_Sum"), Benchmark]
    public int Array_WhereSelect_Sum_Hyperlinq_RefStruct() => array.AsValueEnumerable().Where(x => x % 2 == 0).Select(x => x * 2).Sum();

    [BenchmarkCategory("Array_WhereSelect_Sum"), Benchmark]
    public int Array_WhereSelect_Sum_Hyperlinq_AsValueEnumerable()
    {
        var where = array.AsValueEnumerable().Where(x => x % 2 == 0);
        var select = where.Select(x => x * 2);
        return select.Sum();
    }

    // ===== List Where Select Sum =====

    [BenchmarkCategory("List_WhereSelect_Sum"), Benchmark(Baseline = true)]
    public int List_WhereSelect_Sum_LINQ() => Enumerable.Sum(Enumerable.Select(Enumerable.Where(list, x => x % 2 == 0), x => x * 2));

    [BenchmarkCategory("List_WhereSelect_Sum"), Benchmark]
    public int List_WhereSelect_Sum_Hyperlinq_AsValueEnumerable() => list.AsValueEnumerable().Where(x => x % 2 == 0).Select(x => x * 2).Sum();
}
