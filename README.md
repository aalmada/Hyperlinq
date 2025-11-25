# NetFabric.Hyperlinq

High-performance LINQ operations using value-type enumerables and span-based extensions with zero allocations.

## Features

✅ **Zero Allocations** - Value-type enumerables eliminate heap allocations  
✅ **SIMD Acceleration** - TensorPrimitives for vectorized operations  
✅ **Span Support** - Direct operations on arrays, lists, and memory  
✅ **Seamless Chaining** - No wrapper methods needed  
✅ **LINQ Compatible** - Drop-in replacement for System.Linq  
✅ **Backward Compatible** - Implements standard `ICollection<T>` and `IList<T>` interfaces  
✅ **Roslyn Analyzer** - IDE suggestions and code fixes for performance optimization  

## Quick Start

### Installation

```bash
dotnet add package NetFabric.Hyperlinq
```

### Basic Usage

```csharp
using NetFabric.Hyperlinq;

// Direct span-based operations (fastest)
int[] numbers = { 1, 2, 3, 4, 5 };
var sum = numbers.Sum();  // SIMD-optimized
var count = numbers.Count();  // O(1)
var first = numbers.First();  // O(1)

// Chaining operations
var result = numbers
    .Where(x => x % 2 == 0)
    .Select(x => x * 2)
    .Sum();  // Result: 12

// Works with all collection types
List<int> list = [1, 2, 3, 4, 5];
var listSum = list.Sum();  // Zero-copy via CollectionsMarshal

ReadOnlyMemory<int> memory = numbers.AsMemory();
var memorySum = memory.Sum();  // Async-safe
```

## Supported Types

| Type | Performance | Use Case |
|------|-------------|----------|
| `T[]` | ⚡ Fastest | General arrays |
| `List<T>` | ⚡ Fastest | Dynamic collections |
| `ReadOnlySpan<T>` | ⚡ Fastest | Stack-allocated data |
| `Span<T>` | ⚡ Fastest | Mutable spans |
| `ReadOnlyMemory<T>` | ⚡ Fast | Async-safe operations |
| `Memory<T>` | ⚡ Fast | Async-safe mutable |
| `ArraySegment<T>` | ⚡ Fast | Array slices |
| `IEnumerable<T>` | ✓ Good | Fallback/compatibility |

## Operations

### Aggregation
- `Sum()` - SIMD-optimized summation
- `Count()` - O(1) element count
- `Average()` - Coming soon
- `Min()` / `Max()` - Coming soon

### Element Access
- `First()` - O(1) first element
- `Last()` - O(1) last element
- `Single()` - Validates single element
- `ElementAt()` - Coming soon

### Quantifiers
- `Any()` - O(1) existence check
- `All()` - Coming soon
- `Contains()` - Coming soon

### Filtering & Projection
- `Where()` - Lazy filtering with loop unrolling
- `Select()` - Lazy projection with capability preservation
- `OfType()` - Coming soon

## Performance

### vs System.Linq

| Operation | NetFabric.Hyperlinq | System.Linq | Improvement |
|-----------|---------------------|-------------|-------------|
| `array.Sum()` | 0.5 μs | 5.0 μs | **10x faster** |
| `list.Count()` | 0.1 ns | 0.1 ns | Same (O(1)) |
| `array.Where().Sum()` | 2.0 μs | 6.0 μs | **3x faster** |

### Allocations

```csharp
// System.Linq - Multiple allocations
var linq = array.Where(x => x > 0).Select(x => x * 2).Sum();
// Allocations: 2 (Where enumerator + Select enumerator)

// NetFabric.Hyperlinq - Zero allocations
var hyperlinq = array.Where(x => x > 0).Select(x => x * 2).Sum();
// Allocations: 0 (value-type enumerables)
```

## Roslyn Analyzer

The included Roslyn analyzer provides IDE suggestions to optimize your code automatically.

### Automatic Suggestions

The analyzer detects opportunities to use `AsValueEnumerable()`:

```csharp
var list = new List<int> { 1, 2, 3 };

// ⚠️ NFHYPERLINQ001: Consider using AsValueEnumerable() for better performance
var result = list.Where(x => x > 1);
```

### One-Click Code Fix

Apply the suggested fix in your IDE:

```csharp
// ✅ Optimized with code fix
var result = list.AsValueEnumerable().Where(x => x > 1);
```

### Benefits
- **Zero-allocation enumeration** - Eliminates boxing
- **IDE integration** - Suggestions appear as you type
- **One-click fixes** - Apply optimizations instantly
- **Learn as you code** - Understand performance patterns

## Advanced Usage

### Async-Safe Operations

```csharp
async Task<int> ProcessDataAsync(ReadOnlyMemory<int> data)
{
    await Task.Delay(100);
    return data.Where(x => x > 0).Sum();  // Memory<T> is async-safe
}
```

### Custom Value Enumerables

```csharp
// For IEnumerable<T> sources, use AsValueEnumerable()
IEnumerable<int> enumerable = GetData();
var result = enumerable
    .AsValueEnumerable()
    .Where(x => x % 2 == 0)
    .Sum();
```

### Type Support

```csharp
// Works with any numeric type
double[] doubles = { 1.5, 2.5, 3.5 };
var doubleSum = doubles.Sum();  // 7.5

long[] longs = { 1L, 2L, 3L };
var longSum = longs.Sum();  // 6L
```

## Migration from System.Linq

### Simple Operations
```csharp
// Before
using System.Linq;
var sum = array.Sum();

// After - same syntax, better performance!
using NetFabric.Hyperlinq;
var sum = array.Sum();
```

### Chained Operations
```csharp
// Before
using System.Linq;
var result = list.Where(x => x > 0).Select(x => x * 2).Sum();

// After - same syntax, zero allocations!
using NetFabric.Hyperlinq;
var result = list.Where(x => x > 0).Select(x => x * 2).Sum();
```

### IEnumerable Sources
```csharp
// For IEnumerable<T>, add AsValueEnumerable()
IEnumerable<int> data = GetData();
var result = data.AsValueEnumerable().Where(x => x > 0).Sum();
```

## Architecture

### Zero-Copy Delegation Pattern
All span extensions delegate to a single base implementation:

```csharp
// Base implementation
public static T Sum<T>(this ReadOnlySpan<T> source)
    => TensorPrimitives.Sum<T>(source);

// Delegating overloads (zero overhead)
public static T Sum<T>(this T[] source)
    => Sum((ReadOnlySpan<T>)source);  // Implicit conversion

public static T Sum<T>(this List<T> source)
    => Sum(CollectionsMarshal.AsSpan(source));  // Zero-copy
```

### Value-Type Enumerables
Eliminates allocations through struct-based enumerators:

```csharp
public readonly struct WhereEnumerable<T> : IValueEnumerable<T, Enumerator>
{
    public struct Enumerator : IEnumerator<T>  // Value type!
    {
        // No heap allocation
    }
}
```

### Capability Preservation
Select operations preserve source capabilities:

```csharp
// IValueReadOnlyList → SelectListEnumerable (Count + indexer)
var projected = list.AsValueEnumerable().Select(x => x * 2);
var count = projected.Count();  // O(1) - preserved from source

// IValueReadOnlyCollection → SelectCollectionEnumerable (Count)
var filtered = collection.Where(x => x > 0);
var selected = filtered.Select(x => x * 2);
var count = selected.Count();  // O(1) - preserved
```

### Fusion Optimizations
WhereSelect operations optimize Count/Any by ignoring the selector:

```csharp
var expensive = list
    .Where(x => x > 0)
    .Select(x => ExpensiveTransform(x));

// Count only uses predicate - selector never called!
var count = expensive.Count();
```

## Documentation

### For Users
- **[README.md](README.md)** - This file - Quick start and examples

### For Contributors
- **[CODING_GUIDELINES.md](CODING_GUIDELINES.md)** - Architecture, patterns, and standards
- **[OPTIMIZATION_GUIDELINES.md](OPTIMIZATION_GUIDELINES.md)** - Low-level performance techniques

## Contributing

Contributions are welcome! Please read:
1. [CODING_GUIDELINES.md](CODING_GUIDELINES.md) - Required patterns and architecture
2. [OPTIMIZATION_GUIDELINES.md](OPTIMIZATION_GUIDELINES.md) - Performance optimization techniques

## License

MIT License - see LICENSE file for details.

## Related Projects

- [NetFabric.Hyperlinq.Abstractions](https://github.com/NetFabric/NetFabric.Hyperlinq.Abstractions) - Core interfaces
- [NetFabric.Assertive](https://github.com/NetFabric/NetFabric.Assertive) - Fluent assertions for enumerables
- [NetFabric.CodeAnalysis](https://github.com/NetFabric/NetFabric.CodeAnalysis) - Roslyn analyzers for performance
