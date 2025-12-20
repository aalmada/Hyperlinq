# NetFabric.Hyperlinq

High-performance LINQ-style operations using value-type enumerables and span-based extensions with **zero allocations**.

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## ✨ Features

- ✅ **Zero Allocations** - Span-based operations with ref struct enumerators
- ✅ **SIMD Optimization** - Vectorized operations for numeric types
- ✅ **Generic Math** - `Sum()` works with any numeric type (int, double, BigInteger, etc.)
- ✅ **Operation Fusion** - Automatic fusion of `Where().Select().Sum()` chains
- ✅ **Pooled Memory** - `ToArrayPooled()` to reduce GC pressure
- ✅ **Roslyn Analyzer** - Suggests optimizations automatically

## 🚀 Quick Start

### Installation
```bash
dotnet add package NetFabric.Hyperlinq
```

### Basic Usage
```csharp
using NetFabric.Hyperlinq; // Required for arrays, spans, memory, and List<T>

int[] numbers = { 1, 2, 3, 4, 5 };
var sum = numbers.Sum();  // SIMD-optimized, zero allocations

// For IEnumerable<T>, use AsValueEnumerable()
IEnumerable<int> enumerable = GetNumbers();
var result = enumerable.AsValueEnumerable()
    .Where(x => x > 0)
    .Select(x => x * 2)
    .Sum();  // Fused into single pass!
```

## 📊 Performance

Compared to standard LINQ:
- **50-55% faster** for `IEnumerable` Where/WhereSelect operations
- **26% faster** for Sum operations on arrays/lists
- **Up to 75% less memory** allocated

See [benchmarks](docs/benchmarks/) for detailed results.

## 🎯 Supported Types

| Type | Usage | Performance |
|------|-------|-------------|
| `T[]` | Direct | Fastest - SIMD optimized |
| `Span<T>` / `ReadOnlySpan<T>` | Direct | Zero allocations |
| `Memory<T>` / `ReadOnlyMemory<T>` | Direct | Zero allocations |
| `List<T>` | Direct | Zero-copy via `CollectionsMarshal` |
| `IEnumerable<T>` | `.AsValueEnumerable()` | Struct enumerators |

## 📚 Documentation

- **[Getting Started](docs/getting-started/)** - Installation and quick start
- **[Guides](docs/guides/)** - In-depth usage guides
  - [Fusion Operations](docs/guides/fusion-operations.md)
  - [Pooled Memory](docs/guides/pooled-memory.md)
- **[Architecture](docs/architecture/)** - Design principles and internals
- **[API Reference](docs/api/)** - Complete API documentation
- **[Benchmarks](docs/benchmarks/)** - Performance benchmarks

## 🛠️ Contributing

Contributions are welcome! Please read the [Contributing Guide](CONTRIBUTING.md) and check out the [development guidelines](docs/contributing/).

### Development Guidelines
- [Coding Guidelines](docs/contributing/coding-guidelines.md)
- [First/Single Rules](docs/contributing/first-single-rules.md)
- [Optimization Guidelines](docs/contributing/optimization-guidelines.md)
- [Testing Guidelines](docs/contributing/testing-guidelines.md)

## 🔧 Requirements

- .NET 10 or later
- C# 14 language features

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

Copyright (c) 2025 Antão Almada

---

## 🌟 Examples

### Generic Math Support
```csharp
using System.Numerics;

// Works with any numeric type!
double[] doubles = { 1.5, 2.5, 3.5 };
var doubleSum = doubles.Sum(); // 7.5

BigInteger[] bigInts = { new(1), new(2), new(3) };
var bigSum = bigInts.Sum(); // 6
```

### Pooled Memory
Reduce GC pressure by using pooled buffers for temporary materialization:
```csharp
using NetFabric.Hyperlinq;

var largeArray = GetLargeArray();

// Materialize to a pooled buffer instead of allocating a new array
using var buffer = largeArray.AsSpan()
    .Where(x => x % 2 == 0)
    .ToArrayPooled(); // Returns PooledBuffer<T>

// Use the buffer
Process(buffer.AsSpan());

// Buffer is automatically returned to the pool when disposed
```

### Value Enumerable Factories
Generate sequences efficiently with zero-allocation enumeration:
```csharp
using NetFabric.Hyperlinq;

// Generate a range of integers
var range = ValueEnumerable.Range(0, 100);

// Supports indexing (IValueReadOnlyList)
var tenth = range[10]; // 10

// Materialization is SIMD-optimized for Range and Repeat!
// Uses Vector<T> for ToArray(), ToList(), and CopyTo()
var array = range.ToArray(); // No resizing needed, blazingly fast

// Chain operations
var evenSquares = range
    .Where(x => x % 2 == 0)
    .Select(x => x * x)
    .ToArray();
```

### Roslyn Analyzer
The analyzer automatically suggests optimizations:
```csharp
var list = new List<int> { 1, 2, 3 };
var result = list.Where(x => x > 1); // ⚠️ Analyzer suggests: Use AsValueEnumerable()

// After fix:
var result = list.AsValueEnumerable().Where(x => x > 1); // ✅ Optimized!
```

---

**Built with ❤️ for high-performance .NET applications**
