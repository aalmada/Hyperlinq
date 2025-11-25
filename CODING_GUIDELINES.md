# NetFabric.Hyperlinq Coding Guidelines

> **See also**: [OPTIMIZATION_GUIDELINES.md](OPTIMIZATION_GUIDELINES.md) for detailed performance optimization techniques.

## 1. Introduction

NetFabric.Hyperlinq is a high-performance LINQ implementation that eliminates heap allocations and minimizes overhead through value-type enumeration, aggressive inlining, and specialized implementations.

**Key Goals:**
- Zero allocations (struct-based enumerators)
- Faster than System.Linq
- Full LINQ compatibility
- Type-safe, compile-time optimizations

---

## 2. Core Architecture

### 2.1 Value-Type Enumeration

**Requirements:**
- All enumerables MUST be `readonly struct`
- All enumerators MUST be `struct`
- Return concrete struct types, NEVER `IEnumerable<T>`

**Why?**
- Eliminates boxing
- No heap allocations
- Better cache locality
- Enables compile-time optimizations

### 2.2 Interface Hierarchy

Use the appropriate interface based on capabilities:

| Interface | Capabilities | Use For |
|-----------|-------------|---------|
| `IValueEnumerable<T, TEnumerator>` | Enumeration only | Filtered/transformed sequences (count unknown) |
| `IValueReadOnlyCollection<T, TEnumerator>` | + `Count` property | Collections with known count |
| `IValueReadOnlyList<T, TEnumerator>` | + `this[int]` indexer | Random-access sources (arrays, lists) |

**Principle:** Accept the least features required (parameters), return the most features supported (return types).

### 2.3 Backward Compatibility

To ensure compatibility with existing .NET code:

**Requirements:**
- Types implementing `IValueReadOnlyCollection` MUST also implement `ICollection<T>` (read-only)
- Types implementing `IValueReadOnlyList` MUST also implement `IList<T>` (read-only)

**Implementation:**
```csharp
public readonly struct ArrayValueEnumerable<T> 
    : IValueReadOnlyList<T, Enumerator>, IList<T>
{
    // IList<T> implementation
    bool ICollection<T>.IsReadOnly => true;
    void ICollection<T>.Add(T item) => throw new NotSupportedException();
    void IList<T>.Insert(int index, T item) => throw new NotSupportedException();
    // ... other mutation methods throw NotSupportedException
    
    // Implement Contains, CopyTo, IndexOf efficiently
}
```

### 2.4 Leveraging ICollection for Optimizations

Since `IValueReadOnlyCollection` implementations also implement `ICollection<T>`, optimize operations by checking for `ICollection<T>`. This provides broader compatibility with standard .NET collections.

**Pattern:**
```csharp
// Generic method with optimization
public static int Count<TEnumerable, TEnumerator, TSource>(this TEnumerable source)
    where TEnumerable : IValueEnumerable<TSource, TEnumerator>
    where TEnumerator : struct, IEnumerator<TSource>
{
    // Check for ICollection<T> (broader than IValueReadOnlyCollection)
    if (source is ICollection<TSource> collection)
        return collection.Count;  // O(1)

    // Fallback to enumeration
    var count = 0;
    foreach (var _ in source)
        count++;
    return count;
}

// Dedicated ICollection<T> overload
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static int Count<T>(this ICollection<T> source)
    => source.Count;
```

**Benefits:**
- Works with any `ICollection<T>` (List, HashSet, custom collections)
- O(1) performance for Count, Any operations
- Better IntelliSense for standard .NET users

---

## 3. Implementation Patterns

### 3.1 Specialized Enumerables

Create type-specific implementations to avoid runtime branching:

```csharp
// ✅ GOOD: Separate types for each source
public readonly struct WhereArrayEnumerable<T> { }      // For T[]
public readonly struct WhereListEnumerable<T> { }       // For List<T>
public readonly struct WhereEnumerable<T> { }           // For IEnumerable<T>

// ❌ BAD: Single type with runtime checks
public readonly struct WhereEnumerable<T>
{
    object source;  // Runtime type checking - avoid!
}
```

### 3.2 Overload Delegation (Zero-Copy)

Minimize code duplication by delegating to a base implementation:

```csharp
// Base implementation
public static T Sum<T>(this ReadOnlySpan<T> source) { /* ... */ }

// Delegating overloads (zero-copy conversions)
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static T Sum<T>(this T[] source) 
    => Sum((ReadOnlySpan<T>)source);

[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static T Sum<T>(this List<T> source) 
    => Sum(CollectionsMarshal.AsSpan(source));
```

### 3.3 Operation Fusion

Combine operations to eliminate intermediate enumerators:

```csharp
public readonly struct WhereEnumerable<T>
{
    // Fusion: Where().Select() -> WhereSelectEnumerable
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public WhereSelectEnumerable<T, TResult> Select<TResult>(Func<T, TResult> selector)
        => new WhereSelectEnumerable<T, TResult>(source, predicate, selector);
}
```

---

## 4. Performance Requirements

### 4.1 Aggressive Inlining

Use `[MethodImpl(MethodImplOptions.AggressiveInlining)]` on:
- Extension methods (entry points)
- `Enumerator.MoveNext()` (hottest path)
- Fusion methods
- Small property getters

**Do NOT use on:**
- Large methods (>50 lines)
- Complex control flow
- Exception-throwing methods

### 4.2 Zero Allocations

**Goal:** 0 bytes allocated per operation (excluding result materialization).

**Verification:** Use `[MemoryDiagnoser]` in benchmarks.

### 4.3 Advanced Optimizations

For detailed techniques (loop unrolling, branch elimination, SIMD, etc.), see [OPTIMIZATION_GUIDELINES.md](OPTIMIZATION_GUIDELINES.md).

---

## 5. Project Structure

### 5.1 File Organization

```
NetFabric.Hyperlinq/
├── Wrappers/
│   ├── ArrayValueEnumerable.cs
│   ├── ListValueEnumerable.cs
│   └── EnumerableValueEnumerable.cs
└── Extensions/
    ├── Aggregation/
    │   ├── ValueEnumerableExtensions.Sum.cs
    │   └── ValueEnumerableExtensions.Count.cs
    ├── Filtering/
    │   ├── WhereEnumerable.cs
    │   ├── WhereListEnumerable.cs
    │   └── ValueEnumerableExtensions.Where.cs
    ├── Projection/
    │   ├── SelectEnumerable.cs
    │   ├── SelectListEnumerable.cs
    │   └── ValueEnumerableExtensions.Select.cs
    └── Span/
        ├── SpanExtensions.Sum.cs
        └── SpanExtensions.Where.cs
```

**Principles:**
- Group by operation category
- Co-locate enumerables with their extension methods
- Use partial classes for large extension method sets

### 5.2 Naming Conventions

| Type | Convention | Example |
|------|-----------|---------|
| Enumerable structs | `{Operation}{Source}Enumerable<T>` | `WhereListEnumerable<T>` |
| Enumerator structs | Nested `Enumerator` | `WhereEnumerable<T>.Enumerator` |
| Extension methods | Match LINQ | `Where`, `Select`, `Sum` |
| Wrapper methods | `AsValueEnumerable()` | `list.AsValueEnumerable()` |

---

## 6. Testing & Benchmarking

### 6.1 Unit Tests

**Requirements:**
- Test all source types (Array, List, IEnumerable, Memory)
- Test edge cases (empty, single element)
- Verify LINQ compatibility
- Use property-based testing where applicable

### 6.2 Benchmarks

**Setup:**
```csharp
[MemoryDiagnoser]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class Benchmarks
{
    [Params(100, 10_000)]
    public int Count { get; set; }

    [BenchmarkCategory("Sum"), Benchmark(Baseline = true)]
    public int LINQ_Sum() => list.Sum();

    [BenchmarkCategory("Sum"), Benchmark]
    public int Hyperlinq_Sum() => list.AsValueEnumerable().Sum();
}
```

**Metrics:**
- Mean time (lower is better)
- Allocated memory (should be 0 B)
- Ratio (Hyperlinq/LINQ < 1.0)

---

## 7. Implementation Checklist

Before committing a new operation:

### Architecture
- [ ] Enumerable is `readonly struct`
- [ ] Enumerator is `struct`
- [ ] Implements appropriate interface:
  - [ ] `IValueReadOnlyList` + `IList` (if random access)
  - [ ] `IValueReadOnlyCollection` + `ICollection` (if count known)
  - [ ] `IValueEnumerable` (otherwise)
- [ ] Backward compatibility interfaces implemented explicitly
- [ ] Type-specific implementations (no runtime branching)

### Performance
- [ ] `[MethodImpl(AggressiveInlining)]` on extensions and `MoveNext`
- [ ] Zero allocations verified with `[MemoryDiagnoser]`
- [ ] Optimization techniques applied (see [OPTIMIZATION_GUIDELINES.md](OPTIMIZATION_GUIDELINES.md))
- [ ] Benchmarked against System.Linq

### Testing
- [ ] Unit tests for all source types
- [ ] Edge cases tested
- [ ] LINQ compatibility verified
- [ ] Benchmarks added

### Documentation
- [ ] XML documentation on public APIs
- [ ] Examples in README if new feature
- [ ] OPTIMIZATION_GUIDELINES.md updated if new technique

---

## 8. Common Pitfalls

### ❌ DON'T: Return IEnumerable<T>
```csharp
public static IEnumerable<T> Where<T>(...)
    => new WhereEnumerable<T>(...);  // Boxing!
```

### ✅ DO: Return concrete struct
```csharp
public static WhereEnumerable<T> Where<T>(...)
    => new WhereEnumerable<T>(...);  // No boxing
```

### ❌ DON'T: Use class for enumerables
```csharp
public class WhereEnumerable<T> { }  // Heap allocation
```

### ✅ DO: Use readonly struct
```csharp
public readonly struct WhereEnumerable<T> { }  // Stack allocation
```

### ❌ DON'T: Runtime type checking
```csharp
public bool MoveNext()
{
    if (source is List<T> list) { }  // Runtime overhead
}
```

### ✅ DO: Type-specific implementations
```csharp
public readonly struct WhereListEnumerable<T> { }  // Compile-time
```

---

## 9. References

- [NetFabric.Hyperlinq.Abstractions](https://github.com/NetFabric/NetFabric.Hyperlinq.Abstractions)
- [OPTIMIZATION_GUIDELINES.md](OPTIMIZATION_GUIDELINES.md) - Detailed performance techniques
- [MethodImplOptions.AggressiveInlining](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.methodimploptions)
- [Value Types Best Practices](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/struct)
