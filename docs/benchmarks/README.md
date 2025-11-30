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

---

[← Back to Documentation Index](../README.md)
