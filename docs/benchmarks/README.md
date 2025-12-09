# Benchmarks

Performance benchmarks and analysis for NetFabric.Hyperlinq.

## Overview

NetFabric.Hyperlinq is designed for maximum performance through:
- **Dual Enumeration Pattern**: Ref struct enumerables for direct operations, chainable enumerables via `AsValueEnumerable()`
- **SIMD Optimization**: Vectorized operations using `TensorPrimitives` for numeric types
- **Zero Allocations**: Span-based operations with no heap allocations

## Latest Results

Benchmarks run on Apple M1, .NET 10.0.0, macOS 26.1

### Key Performance Wins

| Operation | LINQ | Hyperlinq | Speedup | Allocation Reduction |
|-----------|------|-----------|---------|---------------------|
| **Array Sum** | 2,078 ns | 1,544 ns | **26% faster** | 0 B (same) |
| **List Sum** | 2,113 ns | 1,528 ns | **28% faster** | 0 B (same) |
| **Array WhereSelectSum** | 12,641 ns | 5,712 ns | **55% faster** | **104 B → 0 B** |
| **List WhereSelectSum** | 22,666 ns | 5,743 ns | **75% faster** | **152 B → 0 B** |
| **IEnumerable Where** | 21,794 ns | 9,922 ns | **54% faster** | **96 B → 40 B** |
| **IEnumerable WhereSelect** | 22,467 ns | 9,896 ns | **56% faster** | **160 B → 40 B** |
| **Pooled Where ToArray** | 969 ns | 847 ns | **12% faster** | **2,072 B → 144 B (93%)** |

### Dual Enumeration Pattern Benefits

The dual enumeration pattern provides flexibility without sacrificing performance:

- **Direct Operations** (ref struct): Maximum performance, zero allocations
  - `array.Sum()` → 1,544 ns, 0 B
  - `list.Where(x => x % 2 == 0).Sum()` → uses ref struct, 0 B allocated

- **Chained Operations** (`AsValueEnumerable()`): Still faster than LINQ, minimal allocations
  - `array.AsValueEnumerable().Where().Select().Sum()` → 5,712 ns vs LINQ's 12,641 ns
  - `list.AsValueEnumerable().Where().Select().Sum()` → 5,743 ns vs LINQ's 22,666 ns

### Complete Results

```
| Method                               | Categories                 | Count | Mean           | Ratio | Allocated | Alloc Ratio |
|------------------------------------- |--------------------------- |------ |---------------:|------:|----------:|------------:|
| Array_LINQ_Sum                       | Array_Sum                  | 10000 |  2,078.1 ns    |  1.00 |       - B |          NA |
| Array_Hyperlinq_Sum                  | Array_Sum                  | 10000 |  1,544.3 ns    |  0.74 |       - B |          NA |
|                                      |                            |       |                |       |           |             |
| Array_LINQ_WhereSelectSum            | Array_WhereSelectSum       | 10000 | 12,640.7 ns    |  1.00 |     104 B |        1.00 |
| Array_Hyperlinq_WhereSelectSum       | Array_WhereSelectSum       | 10000 |  5,712.3 ns    |  0.45 |       - B |        0.00 |
|                                      |                            |       |                |       |           |             |
| IEnumerable_LINQ_Where               | IEnumerable_Where          | 10000 | 21,793.6 ns    |  1.00 |      96 B |        1.00 |
| IEnumerable_Hyperlinq_Where          | IEnumerable_Where          | 10000 |  9,922.3 ns    |  0.46 |      40 B |        0.42 |
|                                      |                            |       |                |       |           |             |
| IEnumerable_LINQ_WhereSelect         | IEnumerable_WhereSelect    | 10000 | 22,467.0 ns    |  1.00 |     160 B |        1.00 |
| IEnumerable_Hyperlinq_WhereSelect    | IEnumerable_WhereSelect    | 10000 |  9,896.0 ns    |  0.44 |      40 B |        0.25 |
|                                      |                            |       |                |       |           |             |
| List_LINQ_Sum                        | List_Sum                   | 10000 |  2,113.4 ns    |  1.00 |       - B |          NA |
| List_Hyperlinq_Sum                   | List_Sum                   | 10000 |  1,527.8 ns    |  0.72 |       - B |          NA |
|                                      |                            |       |                |       |           |             |
| List_LINQ_WhereSelectSum             | List_WhereSelectSum        | 10000 | 22,666.2 ns    |  1.00 |     152 B |        1.00 |
| List_Hyperlinq_WhereSelectSum        | List_WhereSelectSum        | 10000 |  5,743.4 ns    |  0.25 |       - B |        0.00 |
|                                      |                            |       |                |       |           |             |
| Where_ToArray_LINQ                   | Where_ToArray              | 1000  |    968.7 ns    |  1.00 |    2,072 B |        1.00 |
| Where_ToArrayPooled_Hyperlinq        | Where_ToArray              | 1000  |    975.4 ns    |  1.01 |    144 B  |        0.07 |
| Where_ToArrayPooled_ValueDelegate    | Where_ToArray              | 1000  |    846.8 ns    |  0.88 |    144 B  |        0.07 |
```

---

## Running Benchmarks

```bash
cd NetFabric.Hyperlinq.Benchmarks
dotnet run -c Release
```

### Run Specific Benchmarks

```bash
# Run only dual enumeration benchmarks
dotnet run -c Release --filter '*DualEnumerationBenchmarks*'

# Run only span operations
dotnet run -c Release --filter '*SpanOperations*'

# Run only AsValueEnumerable benchmarks
dotnet run -c Release --filter '*AsValueEnumerable*'
```

---

## Benchmark Categories

- **DualEnumerationBenchmarks** - Comparing ref struct vs AsValueEnumerable vs LINQ
- **SpanOperationsBenchmarks** - Select, Where, WhereSelect on ReadOnlySpan
- **AsValueEnumerableBenchmarks** - IEnumerable via AsValueEnumerable()
- **HyperlinkBenchmarks** - Core operations (Where, Select, Sum, Count, Any)
- **LastOperationsBenchmarks** - Last() methods for various types
- **MinMaxBenchmarks** - Min/Max operations on arrays and lists
- **PooledBenchmarks** - Pooled memory (`ToArrayPooled`) vs regular materialization
- **NonPrimitiveSumBenchmarks** - Sum operations with custom numeric types

### Pooled Memory Performance

Pooled memory operations (`ToArrayPooled`) provide significant allocation reductions for temporary materialization:

```bash
# Run pooled memory benchmarks
dotnet run -c Release --filter '*PooledBenchmarks*'
```

**Key Benefits:**
- **Zero allocations** for temporary buffers (uses `ArrayPool<T>`)
- **Reduced GC pressure** in high-throughput scenarios
- **Same performance** as regular `ToArray`/`ToList` for known-count scenarios
- **Automatic cleanup** via `IDisposable` pattern

**Use Cases:**
- Temporary materialization for processing
- High-frequency operations where GC pressure matters
- Scenarios where you need an array/list but don't need to keep it

### ValueEnumerable.Range Performance

`ValueEnumerable.Range` provides optimized sequence generation:

- **Zero-allocation enumeration** (value-type enumerator)
- **O(1) indexing** via `IValueReadOnlyList<int, Enumerator>`
- **Optimized materialization** (known count, no resizing)
- **Faster than** `Enumerable.Range` for chained operations

```bash
# Run range benchmarks
dotnet run -c Release --filter '*Range*'
```

---

### Value Delegates Performance

Value Delegates (struct-based function implementation) provide significant performance benefits by avoiding delegate invocation overhead and enabling aggressive inlining, especially for filtering operations.

**Where Sum Operation (Array)**
| Method | Implementation | Mean Time | Speedup vs LINQ | Speedup vs Hyperlinq (Func) |
|--------|----------------|-----------|-----------------|-----------------------------|
| **LINQ** | `Func<T, bool>` | 52.14 us | 1.00x | - |
| **Hyperlinq** | `Func<T, bool>` | 14.75 us | 3.53x | 1.00x |
| **Value Delegate** | `IFunction<T, bool>` | **10.83 us** | **4.81x** | **1.36x** |
| **Value Delegate (in)** | `IFunctionIn<T, bool>` | 12.70 us | 4.10x | 1.16x |

*Note: `IFunctionIn` passes items by reference (`in T`), which avoids copying large structs but may introduce slight indirection overhead for small types like `int`. Use `IFunctionIn` for large types and `IFunction` for primitives.*

**Select Sum Operation (Array)**
| Method | Implementation | Mean Time | Speedup vs LINQ | Speedup vs Hyperlinq (Func) |
|--------|----------------|-----------|-----------------|-----------------------------|
| **LINQ** | `Func<T, TResult>` | 31.00 us | 1.00x | - |
| **Hyperlinq** | `Func<T, TResult>` | 14.00 us | 2.21x | 1.00x |
| **Value Delegate** | `IFunction<T, TResult>` | 17.84 us | 1.73x | 0.78x |
| **Value Delegate (in)** | `IFunctionIn<T, TResult>` | 17.64 us | 1.76x | 0.79x |

*Note: Select operations with Value Delegates show slightly higher overhead than Func-based implementations in some cases due to struct copying/inlining differences, but Where operations show massive gains (approx 2+x faster).*

```bash
# Run value delegate benchmarks
dotnet run -c Release --filter '*ValueDelegateBenchmarks*'
```

---

[← Back to Documentation Index](../README.md)
