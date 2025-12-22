using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace NetFabric.Hyperlinq.Benchmarks;

/// <summary>
/// Benchmark comparing performance of delegates vs value delegates:
/// - Func&lt;T, TResult&gt; (reference-type delegate)
/// - IFunction&lt;T, TResult&gt; (value-type delegate)
/// </summary>
[MemoryDiagnoser]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class DelegateBenchmarks
{
    int[] data = null!;

    [Params(10_000)]
    public int Count { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        data = new int[Count];
        for (var i = 0; i < Count; i++)
        {
            data[i] = i;
        }
    }

    // ===== Where with Delegate =====

    [BenchmarkCategory("Where"), Benchmark(Baseline = true)]
    public int WhereDelegate()
    {
        var sum = 0;
        Func<int, bool> predicate = x => x % 2 == 0;
        
        foreach (var item in data)
        {
            if (predicate(item))
            {
                sum += item;
            }
        }
        return sum;
    }

    [BenchmarkCategory("Where"), Benchmark]
    public int WhereValueDelegate()
    {
        var sum = 0;
        var predicate = new IsEvenFunction();
        
        foreach (var item in data)
        {
            if (predicate.Invoke(item))
            {
                sum += item;
            }
        }
        return sum;
    }

    [BenchmarkCategory("Where"), Benchmark]
    public int WhereValueDelegateGeneric()
    {
        var sum = 0;
        var predicate = new IsEvenFunction();
        
        foreach (var item in data)
        {
            if (InvokeGeneric<int, bool, IsEvenFunction>(predicate, item))
            {
                sum += item;
            }
        }
        return sum;
    }

    [BenchmarkCategory("Where"), Benchmark]
    public int WhereWrappedDelegate()
    {
        var sum = 0;
        Func<int, bool> predicate = x => x % 2 == 0;
        var wrapper = new FuncWrapper<int, bool>(predicate);
        
        foreach (var item in data)
        {
            if (wrapper.Invoke(item))
            {
                sum += item;
            }
        }
        return sum;
    }

    // ===== Select with Delegate =====

    [BenchmarkCategory("Select"), Benchmark(Baseline = true)]
    public int SelectDelegate()
    {
        var sum = 0;
        Func<int, int> selector = x => x * 2;
        
        foreach (var item in data)
        {
            sum += selector(item);
        }
        return sum;
    }

    [BenchmarkCategory("Select"), Benchmark]
    public int SelectValueDelegate()
    {
        var sum = 0;
        var selector = new DoubleFunction();
        
        foreach (var item in data)
        {
            sum += selector.Invoke(item);
        }
        return sum;
    }

    [BenchmarkCategory("Select"), Benchmark]
    public int SelectValueDelegateGeneric()
    {
        var sum = 0;
        var selector = new DoubleFunction();
        
        foreach (var item in data)
        {
            sum += InvokeGeneric<int, int, DoubleFunction>(selector, item);
        }
        return sum;
    }

    [BenchmarkCategory("Select"), Benchmark]
    public int SelectWrappedDelegate()
    {
        var sum = 0;
        Func<int, int> selector = x => x * 2;
        var wrapper = new FuncWrapper<int, int>(selector);
        
        foreach (var item in data)
        {
            sum += wrapper.Invoke(item);
        }
        return sum;
    }

    // ===== WhereSelect with Delegate =====

    [BenchmarkCategory("WhereSelect"), Benchmark(Baseline = true)]
    public int WhereSelectDelegate()
    {
        var sum = 0;
        Func<int, bool> predicate = x => x % 2 == 0;
        Func<int, int> selector = x => x * 2;
        
        foreach (var item in data)
        {
            if (predicate(item))
            {
                sum += selector(item);
            }
        }
        return sum;
    }

    [BenchmarkCategory("WhereSelect"), Benchmark]
    public int WhereSelectValueDelegate()
    {
        var sum = 0;
        var predicate = new IsEvenFunction();
        var selector = new DoubleFunction();
        
        foreach (var item in data)
        {
            if (predicate.Invoke(item))
            {
                sum += selector.Invoke(item);
            }
        }
        return sum;
    }

    [BenchmarkCategory("WhereSelect"), Benchmark]
    public int WhereSelectValueDelegateGeneric()
    {
        var sum = 0;
        var predicate = new IsEvenFunction();
        var selector = new DoubleFunction();
        
        foreach (var item in data)
        {
            if (InvokeGeneric<int, bool, IsEvenFunction>(predicate, item))
            {
                sum += InvokeGeneric<int, int, DoubleFunction>(selector, item);
            }
        }
        return sum;
    }

    [BenchmarkCategory("WhereSelect"), Benchmark]
    public int WhereSelectWrappedDelegate()
    {
        var sum = 0;
        Func<int, bool> predicate = x => x % 2 == 0;
        Func<int, int> selector = x => x * 2;
        var predicateWrapper = new FuncWrapper<int, bool>(predicate);
        var selectorWrapper = new FuncWrapper<int, int>(selector);
        
        foreach (var item in data)
        {
            if (predicateWrapper.Invoke(item))
            {
                sum += selectorWrapper.Invoke(item);
            }
        }
        return sum;
    }

    // Helper method to test generic invocation
    static TResult InvokeGeneric<TSource, TResult, TFunction>(TFunction function, TSource value)
        where TFunction : struct, IFunction<TSource, TResult>
        => function.Invoke(value);
}

// Wrapper struct that holds a delegate
readonly struct FuncWrapper<TSource, TResult> : IFunction<TSource, TResult>
{
    readonly Func<TSource, TResult> func;

    public FuncWrapper(Func<TSource, TResult> func)
    {
        this.func = func;
    }

    public TResult Invoke(TSource element) => func(element);
}
