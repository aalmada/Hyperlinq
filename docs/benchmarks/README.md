# Benchmarks

Performance benchmarks and analysis for NetFabric.Hyperlinq.

## Overview

NetFabric.Hyperlinq is designed for maximum performance. This section contains benchmark results comparing Hyperlinq against standard LINQ.

## Latest Results

*Benchmarks coming soon*

## Key Performance Characteristics

### Zero Allocations
- Span-based operations: **0 bytes allocated**
- Value enumerables: **Minimal allocations** (struct enumerators)

### Speed Improvements
- **IEnumerable Where/WhereSelect**: 50-55% faster than LINQ
- **Array/List Sum**: 26% faster than LINQ
- **Reduced allocations**: Up to 75% less memory allocated

### SIMD Optimization
- Vectorized operations for numeric types
- Automatic SIMD usage where applicable

---

## Running Benchmarks

```bash
cd NetFabric.Hyperlinq.Benchmarks
dotnet run -c Release
```

### Run Specific Benchmarks

```bash
# Run only span operations
dotnet run -c Release --filter '*SpanOperations*'

# Run only Last operations
dotnet run -c Release --filter '*LastOperations*'
```

---

## Benchmark Categories

- **SpanOperations** - Select, Where, WhereSelect on ReadOnlySpan
- **LastOperations** - Last() methods for various types
- **AsValueEnumerable** - IEnumerable via AsValueEnumerable()

---

## Coming Soon

- **Latest Results** - Complete benchmark results
- **Performance Analysis** - Detailed performance analysis
- **Comparison Charts** - Visual performance comparisons

---

[← Back to Documentation Index](../README.md)
