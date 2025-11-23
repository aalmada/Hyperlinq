# NetFabric.Hyperlinq

High-performance LINQ implementation using value-type enumeration to eliminate boxing and reduce allocations.

## Features

- ✅ **Zero-allocation enumeration** - Value-type enumerators eliminate boxing
- ✅ **Aggressive inlining** - Hot paths are inlined for maximum performance
- ✅ **Operation fusion** - `Where().Select()` chains are fused into single operations
- ✅ **Roslyn analyzer** - IDE suggestions to use `AsValueEnumerable()`
- ✅ **Code fix provider** - One-click optimization

## Quick Start

```csharp
using NetFabric.Hyperlinq;

var list = new List<int> { 1, 2, 3, 4, 5 };

// Standard LINQ (boxing occurs)
var result1 = list.Where(x => x % 2 == 0).Select(x => x * 10).Sum();

// Optimized with AsValueEnumerable (no boxing!)
var result2 = list.AsValueEnumerable()
                  .Where(x => x % 2 == 0)
                  .Select(x => x * 10)
                  .Sum();
```

## Performance

The analyzer will suggest using `AsValueEnumerable()` where it improves performance:

```csharp
var list = new List<int> { 1, 2, 3 };
var result = list.Where(x => x > 1);  // ⚠️ NFHYPERLINQ001: Consider using AsValueEnumerable()
```

Apply the code fix to optimize:

```csharp
var result = list.AsValueEnumerable().Where(x => x > 1);  // ✅ Optimized!
```

## Supported Operations

- `Any()` - Check if sequence has elements
- `Count()` - Count elements
- `First()` - Get first element
- `Single()` - Get single element
- `Sum()` - Sum numeric values
- `Where()` - Filter elements
- `Select()` - Transform elements

## Documentation

- [Coding Guidelines](CODING_GUIDELINES.md) - Performance patterns and best practices
- [Architecture](docs/ARCHITECTURE.md) - Design decisions and implementation details

## Contributing

See [CODING_GUIDELINES.md](CODING_GUIDELINES.md) for performance patterns and conventions.

## License

MIT
