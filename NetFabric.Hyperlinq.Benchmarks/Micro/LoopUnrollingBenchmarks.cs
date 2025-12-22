using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace NetFabric.Hyperlinq.Benchmarks;

[MemoryDiagnoser]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class LoopUnrollingBenchmarks
{
    int[] array = null!;

    [Params(10_000)]
    public int Count { get; set; }

    [GlobalSetup]
    public void Setup() 
    {
        array = new int[Count];
        for (var i = 0; i < Count; i++)
        {
            array[i] = i;
        }
    }

    // ===== Sum =====

    [BenchmarkCategory("Sum"), Benchmark(Baseline = true)]
    public int Sum_Foreach()
    {
        var sum = 0;
        foreach (var item in array)
        {
            sum += item;
        }
        return sum;
    }

    [BenchmarkCategory("Sum"), Benchmark]
    public int Sum_For()
    {
        var localArray = array;
        var sum = 0;
        for (var i = 0; i < localArray.Length; i++)
        {
            sum += localArray[i];
        }
        return sum;
    }

    [BenchmarkCategory("Sum"), Benchmark]
    public int Sum_For_Unroll2()
    {
        var localArray = array;
        var sum = 0;
        var length = localArray.Length;
        var i = 0;
        
        // Process 2 elements at a time
        for (; i <= length - 2; i += 2)
        {
            sum += localArray[i];
            sum += localArray[i + 1];
        }
        
        // Process remaining elements with switch
        switch (length - i)
        {
            case 1:
                sum += localArray[i];
                break;
        }
        
        return sum;
    }

    [BenchmarkCategory("Sum"), Benchmark]
    public int Sum_For_Unroll4()
    {
        var localArray = array;
        var sum = 0;
        var length = localArray.Length;
        var i = 0;
        
        // Process 4 elements at a time
        for (; i <= length - 4; i += 4)
        {
            sum += localArray[i];
            sum += localArray[i + 1];
            sum += localArray[i + 2];
            sum += localArray[i + 3];
        }
        
        // Process remaining elements with switch
        switch (length - i)
        {
            case 3:
                sum += localArray[i];
                sum += localArray[i + 1];
                sum += localArray[i + 2];
                break;
            case 2:
                sum += localArray[i];
                sum += localArray[i + 1];
                break;
            case 1:
                sum += localArray[i];
                break;
        }
        
        return sum;
    }

    [BenchmarkCategory("Sum"), Benchmark]
    public int Sum_For_Unroll8()
    {
        var localArray = array;
        var sum = 0;
        var length = localArray.Length;
        var i = 0;
        
        // Process 8 elements at a time
        for (; i <= length - 8; i += 8)
        {
            sum += localArray[i];
            sum += localArray[i + 1];
            sum += localArray[i + 2];
            sum += localArray[i + 3];
            sum += localArray[i + 4];
            sum += localArray[i + 5];
            sum += localArray[i + 6];
            sum += localArray[i + 7];
        }
        
        // Process remaining elements with switch
        switch (length - i)
        {
            case 7:
                sum += localArray[i];
                sum += localArray[i + 1];
                sum += localArray[i + 2];
                sum += localArray[i + 3];
                sum += localArray[i + 4];
                sum += localArray[i + 5];
                sum += localArray[i + 6];
                break;
            case 6:
                sum += localArray[i];
                sum += localArray[i + 1];
                sum += localArray[i + 2];
                sum += localArray[i + 3];
                sum += localArray[i + 4];
                sum += localArray[i + 5];
                break;
            case 5:
                sum += localArray[i];
                sum += localArray[i + 1];
                sum += localArray[i + 2];
                sum += localArray[i + 3];
                sum += localArray[i + 4];
                break;
            case 4:
                sum += localArray[i];
                sum += localArray[i + 1];
                sum += localArray[i + 2];
                sum += localArray[i + 3];
                break;
            case 3:
                sum += localArray[i];
                sum += localArray[i + 1];
                sum += localArray[i + 2];
                break;
            case 2:
                sum += localArray[i];
                sum += localArray[i + 1];
                break;
            case 1:
                sum += localArray[i];
                break;
        }
        
        return sum;
    }

    // ===== Transform and Sum =====

    [BenchmarkCategory("TransformSum"), Benchmark(Baseline = true)]
    public int TransformSum_Foreach()
    {
        var sum = 0;
        foreach (var item in array)
        {
            sum += item * 2;
        }
        return sum;
    }

    [BenchmarkCategory("TransformSum"), Benchmark]
    public int TransformSum_For()
    {
        var localArray = array;
        var sum = 0;
        for (var i = 0; i < localArray.Length; i++)
        {
            sum += localArray[i] * 2;
        }
        return sum;
    }

    [BenchmarkCategory("TransformSum"), Benchmark]
    public int TransformSum_For_Unroll2()
    {
        var localArray = array;
        var sum = 0;
        var length = localArray.Length;
        var i = 0;
        
        // Process 2 elements at a time
        for (; i <= length - 2; i += 2)
        {
            sum += localArray[i] * 2;
            sum += localArray[i + 1] * 2;
        }
        
        // Process remaining elements with switch
        switch (length - i)
        {
            case 1:
                sum += localArray[i] * 2;
                break;
        }
        
        return sum;
    }

    [BenchmarkCategory("TransformSum"), Benchmark]
    public int TransformSum_For_Unroll4()
    {
        var localArray = array;
        var sum = 0;
        var length = localArray.Length;
        var i = 0;
        
        // Process 4 elements at a time
        for (; i <= length - 4; i += 4)
        {
            sum += localArray[i] * 2;
            sum += localArray[i + 1] * 2;
            sum += localArray[i + 2] * 2;
            sum += localArray[i + 3] * 2;
        }
        
        // Process remaining elements with switch
        switch (length - i)
        {
            case 3:
                sum += localArray[i] * 2;
                sum += localArray[i + 1] * 2;
                sum += localArray[i + 2] * 2;
                break;
            case 2:
                sum += localArray[i] * 2;
                sum += localArray[i + 1] * 2;
                break;
            case 1:
                sum += localArray[i] * 2;
                break;
        }
        
        return sum;
    }

    [BenchmarkCategory("TransformSum"), Benchmark]
    public int TransformSum_For_Unroll8()
    {
        var localArray = array;
        var sum = 0;
        var length = localArray.Length;
        var i = 0;
        
        // Process 8 elements at a time
        for (; i <= length - 8; i += 8)
        {
            sum += localArray[i] * 2;
            sum += localArray[i + 1] * 2;
            sum += localArray[i + 2] * 2;
            sum += localArray[i + 3] * 2;
            sum += localArray[i + 4] * 2;
            sum += localArray[i + 5] * 2;
            sum += localArray[i + 6] * 2;
            sum += localArray[i + 7] * 2;
        }
        
        // Process remaining elements with switch
        switch (length - i)
        {
            case 7:
                sum += localArray[i] * 2;
                sum += localArray[i + 1] * 2;
                sum += localArray[i + 2] * 2;
                sum += localArray[i + 3] * 2;
                sum += localArray[i + 4] * 2;
                sum += localArray[i + 5] * 2;
                sum += localArray[i + 6] * 2;
                break;
            case 6:
                sum += localArray[i] * 2;
                sum += localArray[i + 1] * 2;
                sum += localArray[i + 2] * 2;
                sum += localArray[i + 3] * 2;
                sum += localArray[i + 4] * 2;
                sum += localArray[i + 5] * 2;
                break;
            case 5:
                sum += localArray[i] * 2;
                sum += localArray[i + 1] * 2;
                sum += localArray[i + 2] * 2;
                sum += localArray[i + 3] * 2;
                sum += localArray[i + 4] * 2;
                break;
            case 4:
                sum += localArray[i] * 2;
                sum += localArray[i + 1] * 2;
                sum += localArray[i + 2] * 2;
                sum += localArray[i + 3] * 2;
                break;
            case 3:
                sum += localArray[i] * 2;
                sum += localArray[i + 1] * 2;
                sum += localArray[i + 2] * 2;
                break;
            case 2:
                sum += localArray[i] * 2;
                sum += localArray[i + 1] * 2;
                break;
            case 1:
                sum += localArray[i] * 2;
                break;
        }
        
        return sum;
    }

    // ===== Filter and Sum =====

    [BenchmarkCategory("FilterSum"), Benchmark(Baseline = true)]
    public int FilterSum_Foreach()
    {
        var sum = 0;
        foreach (var item in array)
        {
            if (item % 2 == 0)
                sum += item;
        }
        return sum;
    }

    [BenchmarkCategory("FilterSum"), Benchmark]
    public int FilterSum_For()
    {
        var localArray = array;
        var sum = 0;
        for (var i = 0; i < localArray.Length; i++)
        {
            if (localArray[i] % 2 == 0)
                sum += localArray[i];
        }
        return sum;
    }

    [BenchmarkCategory("FilterSum"), Benchmark]
    public int FilterSum_For_Unroll2()
    {
        var localArray = array;
        var sum = 0;
        var length = localArray.Length;
        var i = 0;
        
        // Process 2 elements at a time
        for (; i <= length - 2; i += 2)
        {
            if (localArray[i] % 2 == 0)
                sum += localArray[i];
            if (localArray[i + 1] % 2 == 0)
                sum += localArray[i + 1];
        }
        
        // Process remaining elements with switch
        switch (length - i)
        {
            case 1:
                if (localArray[i] % 2 == 0)
                    sum += localArray[i];
                break;
        }
        
        return sum;
    }

    [BenchmarkCategory("FilterSum"), Benchmark]
    public int FilterSum_For_Unroll4()
    {
        var localArray = array;
        var sum = 0;
        var length = localArray.Length;
        var i = 0;
        
        // Process 4 elements at a time
        for (; i <= length - 4; i += 4)
        {
            if (localArray[i] % 2 == 0)
                sum += localArray[i];
            if (localArray[i + 1] % 2 == 0)
                sum += localArray[i + 1];
            if (localArray[i + 2] % 2 == 0)
                sum += localArray[i + 2];
            if (localArray[i + 3] % 2 == 0)
                sum += localArray[i + 3];
        }
        
        // Process remaining elements with switch
        switch (length - i)
        {
            case 3:
                if (localArray[i] % 2 == 0)
                    sum += localArray[i];
                if (localArray[i + 1] % 2 == 0)
                    sum += localArray[i + 1];
                if (localArray[i + 2] % 2 == 0)
                    sum += localArray[i + 2];
                break;
            case 2:
                if (localArray[i] % 2 == 0)
                    sum += localArray[i];
                if (localArray[i + 1] % 2 == 0)
                    sum += localArray[i + 1];
                break;
            case 1:
                if (localArray[i] % 2 == 0)
                    sum += localArray[i];
                break;
        }
        
        return sum;
    }

    [BenchmarkCategory("FilterSum"), Benchmark]
    public int FilterSum_For_Unroll8()
    {
        var localArray = array;
        var sum = 0;
        var length = localArray.Length;
        var i = 0;
        
        // Process 8 elements at a time
        for (; i <= length - 8; i += 8)
        {
            if (localArray[i] % 2 == 0)
                sum += localArray[i];
            if (localArray[i + 1] % 2 == 0)
                sum += localArray[i + 1];
            if (localArray[i + 2] % 2 == 0)
                sum += localArray[i + 2];
            if (localArray[i + 3] % 2 == 0)
                sum += localArray[i + 3];
            if (localArray[i + 4] % 2 == 0)
                sum += localArray[i + 4];
            if (localArray[i + 5] % 2 == 0)
                sum += localArray[i + 5];
            if (localArray[i + 6] % 2 == 0)
                sum += localArray[i + 6];
            if (localArray[i + 7] % 2 == 0)
                sum += localArray[i + 7];
        }
        
        // Process remaining elements with switch
        switch (length - i)
        {
            case 7:
                if (localArray[i] % 2 == 0)
                    sum += localArray[i];
                if (localArray[i + 1] % 2 == 0)
                    sum += localArray[i + 1];
                if (localArray[i + 2] % 2 == 0)
                    sum += localArray[i + 2];
                if (localArray[i + 3] % 2 == 0)
                    sum += localArray[i + 3];
                if (localArray[i + 4] % 2 == 0)
                    sum += localArray[i + 4];
                if (localArray[i + 5] % 2 == 0)
                    sum += localArray[i + 5];
                if (localArray[i + 6] % 2 == 0)
                    sum += localArray[i + 6];
                break;
            case 6:
                if (localArray[i] % 2 == 0)
                    sum += localArray[i];
                if (localArray[i + 1] % 2 == 0)
                    sum += localArray[i + 1];
                if (localArray[i + 2] % 2 == 0)
                    sum += localArray[i + 2];
                if (localArray[i + 3] % 2 == 0)
                    sum += localArray[i + 3];
                if (localArray[i + 4] % 2 == 0)
                    sum += localArray[i + 4];
                if (localArray[i + 5] % 2 == 0)
                    sum += localArray[i + 5];
                break;
            case 5:
                if (localArray[i] % 2 == 0)
                    sum += localArray[i];
                if (localArray[i + 1] % 2 == 0)
                    sum += localArray[i + 1];
                if (localArray[i + 2] % 2 == 0)
                    sum += localArray[i + 2];
                if (localArray[i + 3] % 2 == 0)
                    sum += localArray[i + 3];
                if (localArray[i + 4] % 2 == 0)
                    sum += localArray[i + 4];
                break;
            case 4:
                if (localArray[i] % 2 == 0)
                    sum += localArray[i];
                if (localArray[i + 1] % 2 == 0)
                    sum += localArray[i + 1];
                if (localArray[i + 2] % 2 == 0)
                    sum += localArray[i + 2];
                if (localArray[i + 3] % 2 == 0)
                    sum += localArray[i + 3];
                break;
            case 3:
                if (localArray[i] % 2 == 0)
                    sum += localArray[i];
                if (localArray[i + 1] % 2 == 0)
                    sum += localArray[i + 1];
                if (localArray[i + 2] % 2 == 0)
                    sum += localArray[i + 2];
                break;
            case 2:
                if (localArray[i] % 2 == 0)
                    sum += localArray[i];
                if (localArray[i + 1] % 2 == 0)
                    sum += localArray[i + 1];
                break;
            case 1:
                if (localArray[i] % 2 == 0)
                    sum += localArray[i];
                break;
        }
        
        return sum;
    }
}
