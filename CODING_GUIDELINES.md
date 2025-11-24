# NetFabric.Hyperlinq Coding Guidelines

## Span and Memory Extension Methods

### Overload Delegation Pattern

To minimize code duplication while supporting multiple source types, use a delegation hierarchy where all overloads delegate to a single base implementation.

#### Pattern 1: Direct Operations (Sum, Count, Any, etc.)

**Base Implementation**: `ReadOnlySpan<T>` - contains the actual logic
**Delegating Overloads**: All other types convert to `ReadOnlySpan<T>` with zero-copy conversions

```csharp
public static class SpanExtensions
{
    // BASE IMPLEMENTATION - Single source of truth
    public static T Sum<T>(this ReadOnlySpan<T> source)
        where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
        => TensorPrimitives.Sum<T>(source);
    
    // DELEGATING OVERLOADS - Zero-copy conversions
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Sum<T>(this Span<T> source)
        where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
        => Sum((ReadOnlySpan<T>)source);  // Implicit conversion
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Sum<T>(this T[] source)
        where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
        => Sum((ReadOnlySpan<T>)source);  // Implicit conversion
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Sum<T>(this ReadOnlyMemory<T> source)
        where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
        => Sum(source.Span);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Sum<T>(this Memory<T> source)
        where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
        => Sum(source.Span);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Sum<T>(this List<T> source)
        where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
        => Sum(CollectionsMarshal.AsSpan(source));
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Sum<T>(this ArraySegment<T> source)
        where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
        => Sum(source.AsSpan());
}
```

#### Pattern 2: Chaining Operations (Where, Select)

**Base Implementation**: `ReadOnlyMemory<T>` or `List<T>` (can be stored in struct fields)
**Special Handling**: `List<T>` stored directly for efficiency

```csharp
public static class SpanExtensions
{
    // BASE IMPLEMENTATION for Memory-based sources
    public static WhereEnumerable<T> Where<T>(
        this ReadOnlyMemory<T> source, 
        Func<T, bool> predicate)
        => new WhereEnumerable<T>(source, predicate);
    
    // DELEGATING OVERLOADS
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WhereEnumerable<T> Where<T>(
        this T[] source, 
        Func<T, bool> predicate)
        => Where((ReadOnlyMemory<T>)source, predicate);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WhereEnumerable<T> Where<T>(
        this Memory<T> source, 
        Func<T, bool> predicate)
        => Where((ReadOnlyMemory<T>)source, predicate);
    
    // SPECIAL CASE - Store List directly (more efficient)
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WhereEnumerable<T> Where<T>(
        this List<T> source, 
        Func<T, bool> predicate)
        => new WhereEnumerable<T>(source, predicate);
}
```

#### Zero-Copy Conversion Matrix

| Source Type | Converts To | Method | Cost |
|-------------|-------------|--------|------|
| `T[]` | `ReadOnlySpan<T>` | Implicit cast | Zero |
| `Span<T>` | `ReadOnlySpan<T>` | Implicit cast | Zero |
| `Memory<T>` | `ReadOnlySpan<T>` | `.Span` property | Zero |
| `ReadOnlyMemory<T>` | `ReadOnlySpan<T>` | `.Span` property | Zero |
| `List<T>` | `ReadOnlySpan<T>` | `CollectionsMarshal.AsSpan()` | Zero |
| `ArraySegment<T>` | `ReadOnlySpan<T>` | `.AsSpan()` | Zero |

#### Benefits

✅ **Single Implementation**: Logic exists in one place
✅ **Zero Overhead**: Aggressive inlining eliminates delegation cost
✅ **Easy Maintenance**: Update one method, all overloads benefit
✅ **Type Safety**: Compiler ensures correct overload resolution

## Performance Optimization Patterns

This document describes the coding patterns and conventions used in NetFabric.Hyperlinq to achieve optimal performance through value-type enumeration.

## Core Principles

### 1. Value-Type Enumeration
- Use `IValueEnumerable<T, TEnumerator>` instead of `IEnumerable<T>` to avoid boxing
- Enumerators MUST be structs (value types)
- Return value-type enumerables from LINQ-like methods

### 2. Interface Hierarchy - Return Most Features
**Principle:** Methods should accept the least features required as parameters and return the most features supported.

The NetFabric.Hyperlinq.Abstractions package provides an interface hierarchy:
- `IValueEnumerable<T, TEnumerator>` - Basic enumeration
- `IValueReadOnlyCollection<T, TEnumerator>` - Adds `Count` property
- `IValueReadOnlyList<T, TEnumerator>` - Adds indexer `this[int index]`

**Guidelines:**
- **Arrays and List<T>**: Return `IValueReadOnlyList<T, TEnumerator>` (supports Count + indexer)
- **Filtered/Transformed sequences**: Return `IValueEnumerable<T, TEnumerator>` (no Count/indexer)
- **Method parameters**: Accept `IValueEnumerable<T, TEnumerator>` (least features required)

**Example:**
```csharp
// ✅ Returns IValueReadOnlyList (most features)
public static ArrayValueEnumerable<T> AsValueEnumerable<T>(this T[] source)
    => new ArrayValueEnumerable<T>(source);

// ArrayValueEnumerable implements IValueReadOnlyList<T, Enumerator>
public readonly struct ArrayValueEnumerable<T> : IValueReadOnlyList<T, Enumerator>
{
    public int Count => source.Length;        // From IValueReadOnlyCollection
    public T this[int index] => source[index]; // From IValueReadOnlyList
}

// ✅ Accepts IValueEnumerable (least features required)
public static int Count<TEnumerable, TEnumerator, TSource>(this TEnumerable source)
    where TEnumerable : IValueEnumerable<TSource, TEnumerator>
    where TEnumerator : struct, IEnumerator<TSource>
```

### 3. Aggressive Inlining
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

**For collections with Count and indexer (arrays, List<T>):**
```csharp
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public readonly struct ArrayValueEnumerable<T> 
        : IValueReadOnlyList<T, ArrayValueEnumerable<T>.Enumerator>
    {
        readonly T[] source;

        public ArrayValueEnumerable(T[] source)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
        }

        public int Count => source.Length;
        public T this[int index] => source[index];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Enumerator GetEnumerator() => new Enumerator(source);
        
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct Enumerator : IEnumerator<T>
        {
            readonly T[] array;
            int index;

            public Enumerator(T[] array)
            {
                this.array = array;
                this.index = -1;
            }

            public T Current => array[index];
            object IEnumerator.Current => Current;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext() => ++index < array.Length;

            public void Reset() => index = -1;
            public void Dispose() { }
        }
    }
}
```

**For filtered/transformed sequences (no Count/indexer):**
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

## File Organization

### Partial Classes for Extension Methods

The `ValueEnumerableExtensions` class is split into multiple partial class files organized by LINQ method categories. Enumerable implementation files are co-located with their extension methods:

```
NetFabric.Hyperlinq/
├── ValueEnumerableExtensions.cs (main partial class)
├── AsValueEnumerableExtensions.cs
├── Optimized.cs
├── Wrappers/
│   ├── ArrayValueEnumerable.cs
│   ├── ListValueEnumerable.cs
│   └── EnumerableValueEnumerable.cs
└── Extensions/
    ├── Aggregation/
    │   ├── ValueEnumerableExtensions.Count.cs
    │   └── ValueEnumerableExtensions.Sum.cs
    ├── Element/
    │   ├── ValueEnumerableExtensions.First.cs
    │   └── ValueEnumerableExtensions.Single.cs
    ├── Filtering/
    │   ├── WhereEnumerable.cs
    │   └── WhereSelectEnumerable.cs
    ├── Projection/
    │   └── SelectEnumerable.cs
    └── Quantifier/
        └── ValueEnumerableExtensions.Any.cs
```

**Benefits:**
- Easy to locate specific methods and their implementations
- Enumerable structs are co-located with extension methods that use them
- Reduces merge conflicts
- Scales well as more methods are added
- Clear categorization by LINQ operation type

**Categories:**
- **Aggregation**: Sum, Count, Average, Min, Max, Aggregate
- **Element**: First, FirstOrDefault, Last, LastOrDefault, Single, SingleOrDefault, ElementAt
- **Quantifier**: Any, All, Contains
- **Projection**: Select, SelectMany, Zip
- **Filtering**: Where, OfType, Distinct
- **Ordering**: OrderBy, OrderByDescending, ThenBy, Reverse
- **Partitioning**: Take, Skip, TakeLast, SkipLast
- **Set**: Union, Intersect, Except
- **Conversion**: ToArray, ToList, ToDictionary
- **Wrappers**: AsValueEnumerable wrappers for List, Array, IEnumerable

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
- [ ] Implements appropriate interface:
  - [ ] `IValueReadOnlyList<T, TEnumerator>` for arrays/List<T> (with Count + indexer)
  - [ ] `IValueEnumerable<T, TEnumerator>` for filtered/transformed sequences
- [ ] Method parameters accept `IValueEnumerable<T, TEnumerator>` (least features)
- [ ] Return type exposes most features available (IValueReadOnlyList > IValueEnumerable)
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
