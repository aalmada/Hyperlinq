# NetFabric.Hyperlinq Coding Guidelines

## Performance Optimization Patterns

This document describes the coding patterns and conventions used in NetFabric.Hyperlinq to achieve optimal performance through value-type enumeration.

## Core Principles

### 1. Value-Type Enumeration
- Use `IValueEnumerable<T, TEnumerator>` instead of `IEnumerable<T>` to avoid boxing
- Enumerators MUST be structs (value types)
- Return value-type enumerables from LINQ-like methods

### 2. Aggressive Inlining
Use `[MethodImpl(MethodImplOptions.AggressiveInlining)]` on:
- **All extension methods** that create value enumerables
- **Enumerator.MoveNext()** methods (hot path)
- **Fusion methods** (e.g., `Where().Select()` optimization)
- **Small property getters** in performance-critical paths

**Example:**
```csharp
using System.Runtime.CompilerServices;

[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static ListValueEnumerable<T> AsValueEnumerable<T>(this List<T> source)
    => new ListValueEnumerable<T>(source);
```

### 3. Struct Enumerables
All enumerable types MUST be `readonly struct`:

```csharp
public readonly struct WhereEnumerable<TSource> 
    : IValueEnumerable<TSource, WhereEnumerable<TSource>.Enumerator>
{
    readonly IEnumerable<TSource> source;
    readonly Func<TSource, bool> predicate;
    
    public Enumerator GetEnumerator() => new Enumerator(source.GetEnumerator(), predicate);
    
    public struct Enumerator : IEnumerator<TSource>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext() { /* ... */ }
    }
}
```

## Required Attributes

### MethodImpl Locations

| Location | Attribute | Reason |
|----------|-----------|--------|
| `AsValueEnumerable()` extensions | `[MethodImpl(AggressiveInlining)]` | Entry point to optimized path |
| `Enumerator.MoveNext()` | `[MethodImpl(AggressiveInlining)]` | Called every iteration (hot path) |
| Fusion methods (e.g., `Select()` on `WhereEnumerable`) | `[MethodImpl(AggressiveInlining)]` | Enable operation fusion |
| Small getters in enumerators | `[MethodImpl(AggressiveInlining)]` | Eliminate call overhead |

### DO NOT Use AggressiveInlining On:
- Large methods (>50 lines)
- Methods with complex control flow
- Methods that are rarely called
- Exception-throwing methods

## File Structure Patterns

### Extension Method Files
```csharp
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static class AsValueEnumerableExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ListValueEnumerable<T> AsValueEnumerable<T>(this List<T> source)
            => new ListValueEnumerable<T>(source);
    }
}
```

### Enumerable Struct Files
```csharp
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public readonly struct WhereEnumerable<TSource> 
        : IValueEnumerable<TSource, WhereEnumerable<TSource>.Enumerator>
    {
        readonly IEnumerable<TSource> source;
        readonly Func<TSource, bool> predicate;

        public WhereEnumerable(IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            this.source = source;
            this.predicate = predicate;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Enumerator GetEnumerator() => new Enumerator(source.GetEnumerator(), predicate);
        
        IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct Enumerator : IEnumerator<TSource>
        {
            readonly IEnumerator<TSource> sourceEnumerator;
            readonly Func<TSource, bool> predicate;

            public Enumerator(IEnumerator<TSource> sourceEnumerator, Func<TSource, bool> predicate)
            {
                this.sourceEnumerator = sourceEnumerator;
                this.predicate = predicate;
            }

            public TSource Current => sourceEnumerator.Current;
            object IEnumerator.Current => Current;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                while (sourceEnumerator.MoveNext())
                {
                    if (predicate(sourceEnumerator.Current))
                        return true;
                }
                return false;
            }

            public void Reset() => sourceEnumerator.Reset();
            public void Dispose() => sourceEnumerator.Dispose();
        }
    }
}
```

## Operation Fusion Pattern

When an operation can be fused with another (e.g., `Where().Select()`), provide a fusion method:

```csharp
public readonly struct WhereEnumerable<TSource> : IValueEnumerable<...>
{
    // Fusion: Where().Select() -> WhereSelectEnumerable
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public WhereSelectEnumerable<TSource, TResult> Select<TResult>(Func<TSource, TResult> selector)
        => new WhereSelectEnumerable<TSource, TResult>(source, predicate, selector);
}
```

## Naming Conventions

- **Enumerable structs**: `{Operation}Enumerable<T>` (e.g., `WhereEnumerable<T>`)
- **Enumerator structs**: Nested `Enumerator` struct
- **Extension methods**: Match LINQ naming (e.g., `Where`, `Select`, `Sum`)
- **Wrapper methods**: `AsValueEnumerable()`

## Testing Requirements

### Unit Tests
- Test each operation with various data sources
- Test chaining operations
- Test edge cases (empty, single element)
- Compare results with standard LINQ

### Benchmarks
- Use `[BenchmarkCategory]` to group related benchmarks
- Use `[Params]` to test different collection sizes
- Compare against standard LINQ (set as `Baseline = true`)
- Use `[MemoryDiagnoser]` to track allocations

**Example:**
```csharp
[MemoryDiagnoser]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class Benchmarks
{
    [Params(100, 10_000)]
    public int Count { get; set; }

    [BenchmarkCategory("Sum"), Benchmark(Baseline = true)]
    public int LINQ_Sum() => list.Sum();

    [BenchmarkCategory("Sum"), Benchmark]
    public int AsValueEnumerable_Sum() => list.AsValueEnumerable().Sum();
}
```

## Analyzer Patterns

### Diagnostic IDs
- `NFHYPERLINQ001`: Suggest using `AsValueEnumerable()`
- `NFHYPERLINQ0xx`: Future diagnostics

### Severity Levels
- **Info**: Performance suggestions (e.g., use `AsValueEnumerable()`)
- **Warning**: Potential bugs or anti-patterns
- **Error**: Breaking changes or incorrect usage

## Common Pitfalls to Avoid

### ❌ DON'T: Return IEnumerable<T>
```csharp
public static IEnumerable<T> Where<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    => new WhereEnumerable<T>(source, predicate); // Boxing occurs!
```

### ✅ DO: Return value-type enumerable
```csharp
public static WhereEnumerable<T> Where<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    => new WhereEnumerable<T>(source, predicate); // No boxing!
```

### ❌ DON'T: Use class for enumerables
```csharp
public class WhereEnumerable<T> { } // Heap allocation!
```

### ✅ DO: Use readonly struct
```csharp
public readonly struct WhereEnumerable<T> { } // Stack allocation!
```

### ❌ DON'T: Forget AggressiveInlining on hot paths
```csharp
public bool MoveNext() { } // Method call overhead
```

### ✅ DO: Inline hot paths
```csharp
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public bool MoveNext() { } // Inlined!
```

## Performance Checklist

Before committing new LINQ operations:

- [ ] Enumerable is `readonly struct`
- [ ] Enumerator is `struct`
- [ ] Implements `IValueEnumerable<T, TEnumerator>`
- [ ] `AsValueEnumerable()` has `[MethodImpl(AggressiveInlining)]`
- [ ] `MoveNext()` has `[MethodImpl(AggressiveInlining)]`
- [ ] Fusion methods have `[MethodImpl(AggressiveInlining)]`
- [ ] Unit tests added
- [ ] Benchmarks added
- [ ] Compared against standard LINQ

## References

- [IValueEnumerable Documentation](https://github.com/NetFabric/NetFabric.Hyperlinq.Abstractions)
- [MethodImplOptions.AggressiveInlining](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.methodimploptions)
- [Value Types Best Practices](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/struct)
