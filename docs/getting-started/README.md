# Getting Started with NetFabric.Hyperlinq

This section helps you get up and running with NetFabric.Hyperlinq.

## Installation

```bash
dotnet add package NetFabric.Hyperlinq
```

## Quick Start

```csharp
using NetFabric.Hyperlinq; // Required for arrays, spans, memory, and List<T>

int[] numbers = { 1, 2, 3, 4, 5 };
var sum = numbers.Sum();  // SIMD-optimized, zero allocations

// For IEnumerable<T>, use AsValueEnumerable()
IEnumerable<int> enumerable = GetNumbers();
var result = enumerable.AsValueEnumerable().Where(x => x > 0).Sum();
```

## Documentation

- **[Migration Guide](migration-guide.md)** - Migrating from standard LINQ or Hyperlinq v1

## Supported Types

| Type | Usage |
|------|-------|
| `T[]` | Direct - fastest performance |
| `Span<T>` / `ReadOnlySpan<T>` | Direct - zero allocations |
| `Memory<T>` / `ReadOnlyMemory<T>` | Direct - zero allocations |
| `List<T>` | Direct - zero-copy via `CollectionsMarshal` |
| `IEnumerable<T>` | Call `.AsValueEnumerable()` first |

## Key Features

- ✅ **Zero Allocations**: Span-based operations with ref struct enumerators
- ✅ **SIMD Optimization**: Vectorized operations for numeric types
- ✅ **Generic Math**: `Sum()` works with any numeric type
- ✅ **Fusion**: Automatic operation fusion for maximum performance
- ✅ **Roslyn Analyzer**: Suggests optimizations automatically

## Next Steps

- Read the [Fusion Operations Guide](../guides/fusion-operations.md)
- Explore the [API Reference](../api/)
- Check out [Performance Benchmarks](../benchmarks/)

---

[← Back to Documentation Index](../README.md)
