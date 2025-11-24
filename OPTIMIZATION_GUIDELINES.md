# Performance Optimization Guidelines

This document outlines proven optimization techniques to apply when implementing LINQ-style operations in NetFabric.Hyperlinq.

## Table of Contents

1. [Value-Type Enumerators](#value-type-enumerators)
2. [Operation Fusion](#operation-fusion)
3. [IReadOnlyList Optimization](#ireadonlylist-optimization)
4. [Loop Unrolling (Instruction-Level Parallelism)](#loop-unrolling)
5. [Branching Optimization](#branching-optimization)
6. [Bounds Check Elimination](#bounds-check-elimination)
7. [SIMD Optimization](#simd-optimization)
8. [Type-Specific Implementations](#type-specific-implementations)

---

## Value-Type Enumerators

**Reference**: [Leveraging C#'s foreach Loop](https://aalmada.github.io/posts/Leveraging-csharp-foreach-loop/), [LINQ Internals](https://aalmada.github.io/posts/LINQ-internals-speed-optimizations/)

### Technique

Always use `struct` enumerators instead of `class` enumerators to avoid heap allocations.

### Implementation Pattern

```csharp
// ✅ GOOD: Struct enumerator (zero allocations)
public struct Enumerator : IEnumerator<T>
{
    public T Current => ...;
    public bool MoveNext() => ...;
}

// ❌ BAD: Class enumerator (heap allocation)
public class Enumerator : IEnumerator<T> { }
```

### Benefits

- Zero allocations in `foreach` loops
- Better cache locality
- No GC pressure
- Faster enumeration (no virtual dispatch)

---

## Operation Fusion

**Reference**: [LINQ Internals](https://aalmada.github.io/posts/LINQ-internals-speed-optimizations/)

### Technique

Combine operations like `Where().Select()` into a single `WhereSelect` enumerator.

### Implementation Pattern

```csharp
public readonly struct WhereMemoryEnumerable<TSource>
{
    public WhereSelectMemoryEnumerable<TSource, TResult> Select<TResult>(Func<TSource, TResult> selector)
        => new WhereSelectMemoryEnumerable<TSource, TResult>(source, predicate, selector);
}
```

### Benefits

- Eliminates intermediate enumerators
- 10-20% performance improvement
- Better CPU cache utilization

---

## IReadOnlyList Optimization

**Reference**: [IEnumerable and Pals](https://aalmada.github.io/posts/IEnumerable-and-pals/)

### Technique

Use indexer-based iteration (`for` loop) instead of enumerator for `IReadOnlyList<T>`.

### Implementation Pattern

```csharp
// ✅ GOOD: Indexer access
public static int Sum(this IReadOnlyList<int> source)
{
    var sum = 0;
    for (var i = 0; i < source.Count; i++)
        sum += source[i];
    return sum;
}
```

### Benefits

- No enumerator allocation
- Fewer method calls per item
- Better for random-access collections

---

## Loop Unrolling (Instruction-Level Parallelism)

**Reference**: [Unleashing Parallelism](https://aalmada.github.io/posts/Unleashing-parallelism/)

### Technique

Process **4 items per iteration** to enable CPU instruction-level parallelism (ILP).

### When to Apply

✅ **Applicable to**: Filtering operations (`Where`), projection operations (`Select`), element operations (`First`, `Last`)

### Implementation Pattern

```csharp
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public bool MoveNext()
{
    var span = memory.Span;
    ref var spanRef = ref MemoryMarshal.GetReference(span);
    var length = span.Length;
    
    // Process 4 items at a time for ILP
    var end = length - 3;
    for (; index < end; index += 4)
    {
        // Check 4 items in sequence
        var i0 = index + 1;
        if (predicate(Unsafe.Add(ref spanRef, i0)))
        {
            index = i0;
            return true;
        }
        
        var i1 = index + 2;
        if (predicate(Unsafe.Add(ref spanRef, i1)))
        {
            index = i1;
            return true;
        }
        
        var i2 = index + 3;
        if (predicate(Unsafe.Add(ref spanRef, i2)))
        {
            index = i2;
            return true;
        }
        
        var i3 = index + 4;
        if (predicate(Unsafe.Add(ref spanRef, i3)))
        {
            index = i3;
            return true;
        }
    }
    
    // Handle remaining items (0-3) with switch to minimize branching
    switch (length - index - 1)
    {
        case 3:
            if (predicate(Unsafe.Add(ref spanRef, ++index)))
                return true;
            goto case 2;
        case 2:
            if (predicate(Unsafe.Add(ref spanRef, ++index)))
                return true;
            goto case 1;
        case 1:
            if (predicate(Unsafe.Add(ref spanRef, ++index)))
                return true;
            break;
    }
    
    return false;
}
```

### Benefits

- Reduces loop overhead by 75%
- Enables CPU to parallelize instruction execution
- Better pipeline utilization
- Improved branch prediction
- Switch for remainder eliminates loop overhead (fewer jumps than while loop)

---

## Branching Optimization

**Reference**: [CPU Branching and Parallelization](https://aalmada.github.io/posts/CPU-branching-and-parallelization/)

### Technique

Eliminate `if` statements by converting boolean results to numeric values (0 or 1) using `Unsafe.As<bool, byte>()`.

### When to Apply

✅ **Applicable to**: Aggregation operations (`Sum`, `Count`, `Average`, `Aggregate`)  
❌ **NOT applicable to**: Filtering operations (`Where`, `First`, `Last`, `Any`, `All`)

### Why the Distinction?

**Aggregation operations** process ALL items and accumulate a result:
```csharp
// Can eliminate branch
var predicateResult = predicate(item);
sum += item * Unsafe.As<bool, byte>(ref predicateResult);  // 0 or item
```

**Filtering operations** must STOP at each match and yield the item:
```csharp
// CANNOT eliminate branch - need to stop and return
if (predicate(item))
    return true;  // Must stop here
```

### Implementation Pattern (Aggregation)

```csharp
public static int Sum<T>(ReadOnlySpan<T> source, Func<T, bool> predicate)
    where T : struct, IAdditionOperators<T, T, T>
{
    var sum0 = 0;
    var sum1 = 0;
    
    ref var sourceRef = ref MemoryMarshal.GetReference(source);
    var end = source.Length - 1;
    
    // Process 2 items per iteration for parallelization
    for (var index = 0; index < end; index += 2)
    {
        // Eliminate branch with multiplication
        var predicateValue0 = predicate(Unsafe.Add(ref sourceRef, index));
        sum0 += Unsafe.Add(ref sourceRef, index) * Unsafe.As<bool, byte>(ref predicateValue0);
        
        var predicateValue1 = predicate(Unsafe.Add(ref sourceRef, index + 1));
        sum1 += Unsafe.Add(ref sourceRef, index + 1) * Unsafe.As<bool, byte>(ref predicateValue1);
    }
    
    // Handle last item if odd length (also branchless)
    var isOddLength = (source.Length & 1) != 0;
    var lastPredicateValue = predicate(source[^1]);
    sum0 += source[^1] * Unsafe.As<bool, byte>(ref isOddLength) * Unsafe.As<bool, byte>(ref lastPredicateValue);
    
    return sum0 + sum1;
}
```

### Benefits

- Eliminates branch misprediction penalties
- Enables better CPU pipelining
- 20-40% performance improvement for aggregations

---

## Bounds Check Elimination

**Reference**: [Unleashing Parallelism](https://aalmada.github.io/posts/Unleashing-parallelism/)

### Technique

Use `MemoryMarshal.GetReference()` + `Unsafe.Add()` instead of array indexing.

### When to Apply

✅ **Always applicable** when iterating over spans or arrays

### Implementation Pattern

```csharp
// ❌ BAD: Bounds checking on every access
for (var i = 0; i < span.Length; i++)
{
    var item = span[i];  // Bounds check
}

// ✅ GOOD: No bounds checking
ref var spanRef = ref MemoryMarshal.GetReference(span);
for (var i = 0; i < span.Length; i++)
{
    var item = Unsafe.Add(ref spanRef, i);  // No bounds check
}
```

### Benefits

- Eliminates redundant bounds checks
- Reduces CPU cycles per iteration
- Combines well with loop unrolling

---

## SIMD Optimization

**Reference**: [SIMD in .NET](https://aalmada.github.io/posts/SIMD-in-dotnet/)

### Technique

Use `Vector<T>` or `TensorPrimitives` for parallel data processing.

### When to Apply

✅ **Applicable to**: Numeric aggregations (`Sum`, `Average`, `Min`, `Max`), transformations  
❌ **NOT applicable to**: Operations requiring early termination (`First`, `Any`, `All`)

### Implementation Pattern

```csharp
public static int Sum(ReadOnlySpan<int> source)
{
    var sum = 0;
    var start = 0;
    
    // SIMD processing
    if (Vector.IsHardwareAccelerated && source.Length >= Vector<int>.Count)
    {
        var sumVector = Vector<int>.Zero;
        var vectors = MemoryMarshal.Cast<int, Vector<int>>(source);
        
        foreach (ref readonly var vector in vectors)
        {
            sumVector += vector;
        }
        
        sum = Vector.Sum(sumVector);
        start = vectors.Length * Vector<int>.Count;
    }
    
    // Process remaining items with scalar code
    for (var i = start; i < source.Length; i++)
    {
        sum += source[i];
    }
    
    return sum;
}
```

### Benefits

- Process multiple elements per CPU instruction
- 4-16x throughput improvement (depending on vector size)
- Automatic hardware utilization

---

## Type-Specific Implementations

### Technique

Create separate enumerable types for different source types to avoid runtime branching.

### When to Apply

✅ **Always** - Avoid discriminated unions with runtime type checks

### Implementation Pattern

```csharp
// ✅ GOOD: Separate types
public readonly struct WhereMemoryEnumerable<T> { }   // For arrays, Memory
public readonly struct WhereListEnumerable<T> { }     // For List<T>
public readonly struct WhereEnumerable<T> { }         // For IEnumerable<T>

// ❌ BAD: Single type with runtime branching
public readonly struct WhereEnumerable<T>
{
    object source;  // Could be Memory, List, or IEnumerable
    
    public bool MoveNext()
    {
        switch (source)  // Runtime branching - BAD!
        {
            case ReadOnlyMemory<T> memory: ...
            case List<T> list: ...
            case IEnumerable<T> enumerable: ...
        }
    }
}
```

### Benefits

- Eliminates runtime type checks
- Better branch prediction
- Improved CPU cache utilization
- Compile-time optimization

---

## Quick Reference Matrix

| Operation Type | Loop Unrolling | Branch Elimination | Bounds Check Elimination | SIMD |
|----------------|----------------|-------------------|-------------------------|------|
| **Where** | ✅ Yes | ❌ No | ✅ Yes | ❌ No |
| **Select** | ✅ Yes | ❌ No | ✅ Yes | ⚠️ Maybe |
| **Sum** | ✅ Yes | ✅ Yes | ✅ Yes | ✅ Yes |
| **Count** | ✅ Yes | ✅ Yes | ✅ Yes | ✅ Yes |
| **Any** | ❌ No* | ❌ No | ✅ Yes | ❌ No |
| **All** | ❌ No* | ❌ No | ✅ Yes | ❌ No |
| **First** | ✅ Yes | ❌ No | ✅ Yes | ❌ No |
| **Average** | ✅ Yes | ✅ Yes | ✅ Yes | ✅ Yes |

\* Early termination operations don't benefit from loop unrolling

---

## Implementation Checklist

When implementing a new operation:

1. ✅ Determine operation category (filtering, aggregation, projection, etc.)
2. ✅ Create type-specific implementations (Memory, List, IEnumerable)
3. ✅ Apply bounds check elimination (`MemoryMarshal.GetReference` + `Unsafe.Add`)
4. ✅ Apply loop unrolling if applicable (4-way for filtering/projection)
5. ✅ Apply branch elimination if applicable (aggregations only)
6. ✅ Apply SIMD if applicable (numeric aggregations)
7. ✅ Add `[MethodImpl(MethodImplOptions.AggressiveInlining)]` to hot paths
8. ✅ Benchmark against LINQ baseline
9. ✅ Verify zero allocations with BenchmarkDotNet `[MemoryDiagnoser]`

---

## Example: Fully Optimized Where Implementation

```csharp
public readonly struct WhereMemoryEnumerable<TSource> : IValueEnumerable<TSource, WhereMemoryEnumerable<TSource>.Enumerator>
{
    readonly ReadOnlyMemory<TSource> source;
    readonly Func<TSource, bool> predicate;

    public WhereMemoryEnumerable(ReadOnlyMemory<TSource> source, Func<TSource, bool> predicate)
    {
        this.source = source;
        this.predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
    }

    public Enumerator GetEnumerator() => new Enumerator(source, predicate);

    public struct Enumerator : IEnumerator<TSource>
    {
        readonly ReadOnlyMemory<TSource> memory;
        readonly Func<TSource, bool> predicate;
        int index;

        public Enumerator(ReadOnlyMemory<TSource> memory, Func<TSource, bool> predicate)
        {
            this.memory = memory;
            this.predicate = predicate;
            this.index = -1;
        }

        public TSource Current => memory.Span[index];
        object? IEnumerator.Current => Current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            var span = memory.Span;
            ref var spanRef = ref MemoryMarshal.GetReference(span);  // Bounds check elimination
            var length = span.Length;
            
            // Loop unrolling: 4 items per iteration
            var end = length - 3;
            for (; index < end; index += 4)
            {
                var i0 = index + 1;
                if (predicate(Unsafe.Add(ref spanRef, i0)))
                {
                    index = i0;
                    return true;
                }
                
                var i1 = index + 2;
                if (predicate(Unsafe.Add(ref spanRef, i1)))
                {
                    index = i1;
                    return true;
                }
                
                var i2 = index + 3;
                if (predicate(Unsafe.Add(ref spanRef, i2)))
                {
                    index = i2;
                    return true;
                }
                
                var i3 = index + 4;
                if (predicate(Unsafe.Add(ref spanRef, i3)))
                {
                    index = i3;
                    return true;
                }
            }
            
            // Handle remaining items
            while (++index < length)
            {
                if (predicate(Unsafe.Add(ref spanRef, index)))
                    return true;
            }
            
            return false;
        }

        public void Reset() => index = -1;
        public void Dispose() { }
    }
}
```

---

## References

- [Unleashing Parallelism](https://aalmada.github.io/posts/Unleashing-parallelism/)
- [CPU Branching and Parallelization](https://aalmada.github.io/posts/CPU-branching-and-parallelization/)
- [SIMD in .NET](https://aalmada.github.io/posts/SIMD-in-dotnet/)
- [Leveraging C#'s foreach Loop](https://aalmada.github.io/posts/Leveraging-csharp-foreach-loop/)
- [LINQ Internals: Speed Optimizations](https://aalmada.github.io/posts/LINQ-internals-speed-optimizations/)
- [IEnumerable and Pals](https://aalmada.github.io/posts/IEnumerable-and-pals/)
- [Array Iteration Performance in C#](https://aalmada.github.io/Array-iteration-performance-in-csharp.html)
