# Migration Guide: System.Linq to NetFabric.Hyperlinq

## Overview

NetFabric.Hyperlinq provides a drop-in replacement for System.Linq with significantly better performance through span-based extensions and value-type enumerables.

## Quick Migration

### Step 1: Install Package
```bash
dotnet add package NetFabric.Hyperlinq
```

### Step 2: Update Using Directive
```csharp
// Before
using System.Linq;

// After
using NetFabric.Hyperlinq;
```

### Step 3: Remove Unnecessary Wrappers
```csharp
// Old pattern (no longer needed)
var result = array.AsValueEnumerable().Sum();

// New pattern (direct)
var result = array.Sum();
```

## Migration Scenarios

### Arrays and Lists (No Changes Needed!)

```csharp
// These work identically, but faster
int[] array = { 1, 2, 3, 4, 5 };
List<int> list = [1, 2, 3, 4, 5];

// All these "just work" with better performance
var sum = array.Sum();
var count = list.Count();
var first = array.First();
var filtered = list.Where(x => x > 2).Sum();
```

### IEnumerable<T> Sources

For `IEnumerable<T>`, add `.AsValueEnumerable()`:

```csharp
IEnumerable<int> data = GetData();

// Add AsValueEnumerable for IEnumerable sources
var result = data
    .AsValueEnumerable()
    .Where(x => x > 0)
    .Sum();
```

### Async Operations

Use `Memory<T>` or `ReadOnlyMemory<T>` for async-safe operations:

```csharp
// Before (not async-safe)
async Task<int> ProcessAsync(int[] data)
{
    await Task.Delay(100);
    return data.Sum();  // Works but not ideal
}

// After (async-safe)
async Task<int> ProcessAsync(ReadOnlyMemory<int> data)
{
    await Task.Delay(100);
    return data.Sum();  // Memory<T> is async-safe
}
```

## Performance Comparison

### Before (System.Linq)
```csharp
using System.Linq;

var result = array
    .Where(x => x % 2 == 0)  // Allocates enumerator
    .Select(x => x * 2)       // Allocates enumerator
    .Sum();                   // Allocates enumerator
// Total: 3 heap allocations
```

### After (NetFabric.Hyperlinq)
```csharp
using NetFabric.Hyperlinq;

var result = array
    .Where(x => x % 2 == 0)  // Value-type enumerable
    .Select(x => x * 2)       // Fused with Where
    .Sum();                   // SIMD-optimized
// Total: 0 heap allocations
```

## Breaking Changes

### None for Basic Usage!

NetFabric.Hyperlinq is designed to be a drop-in replacement. Most code will work without changes.

### Potential Issues

1. **Extension Method Ambiguity**
   ```csharp
   // If you have both using directives
   using System.Linq;
   using NetFabric.Hyperlinq;
   
   // Solution: Remove System.Linq or use explicit qualification
   using NetFabric.Hyperlinq;
   ```

2. **IEnumerable<T> Sources**
   ```csharp
   // Requires AsValueEnumerable() for optimal performance
   IEnumerable<int> data = GetData();
   var result = data.AsValueEnumerable().Sum();
   ```

## Supported Operations

### Currently Available
- ✅ `Sum()` - SIMD-optimized
- ✅ `Count()` - O(1) for collections
- ✅ `Any()` - O(1) for collections
- ✅ `First()` - O(1) index access
- ✅ `Last()` - O(1) index access
- ✅ `Single()` - Validates single element
- ✅ `Where()` - Lazy filtering
- ✅ `Select()` - Via Where fusion

### Coming Soon
- `Average()`, `Min()`, `Max()`
- `FirstOrDefault()`, `LastOrDefault()`, `SingleOrDefault()`
- `All()`, `Contains()`
- `OrderBy()`, `ThenBy()`
- `Take()`, `Skip()`, `Distinct()`

## Analyzer Support (Planned)

Future versions will include Roslyn analyzers to help migration:

- **HQL001**: Warn on unnecessary `.AsValueEnumerable()` calls
- **HQL002**: Suggest span-based alternatives
- **HQL003**: Detect async-unsafe span usage

## Best Practices

### 1. Prefer Direct Operations
```csharp
// Good
var sum = array.Sum();

// Avoid (unnecessary wrapper)
var sum = array.AsValueEnumerable().Sum();
```

### 2. Use Memory<T> for Async
```csharp
// Good
async Task<int> ProcessAsync(ReadOnlyMemory<int> data)
{
    await Task.Delay(100);
    return data.Sum();
}

// Avoid (Span<T> not async-safe)
async Task<int> ProcessAsync(ReadOnlySpan<int> data)  // Won't compile!
{
    await Task.Delay(100);
    return data.Sum();
}
```

### 3. Chain Operations Freely
```csharp
// Good - zero allocations
var result = array
    .Where(x => x > 0)
    .Select(x => x * 2)
    .Sum();
```

## Troubleshooting

### "Ambiguous call" errors
Remove `using System.Linq;` or use explicit qualification.

### "No suitable method found"
Ensure you're using `NetFabric.Hyperlinq` namespace and the operation is supported.

### Performance not improving
- Verify you're running in Release mode
- Check that you're not using `IEnumerable<T>` unnecessarily
- Use BenchmarkDotNet to measure actual performance

## Getting Help

- [GitHub Issues](https://github.com/NetFabric/NetFabric.Hyperlinq/issues)
- [Documentation](https://github.com/NetFabric/NetFabric.Hyperlinq)
- [Coding Guidelines](CODING_GUIDELINES.md)
