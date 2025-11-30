# API Reference

Detailed API documentation for NetFabric.Hyperlinq.

## Overview

NetFabric.Hyperlinq provides LINQ-style extension methods for various collection types with optimized implementations.

## Extension Methods by Type

### Contiguous Memory Types
- **Arrays (`T[]`)** - Direct indexing, SIMD optimization
- **Spans (`Span<T>`, `ReadOnlySpan<T>`)** - Zero-allocation ref struct operations
- **Memory (`Memory<T>`, `ReadOnlyMemory<T>`)** - Async-compatible span operations

### Collection Types
- **List (`List<T>`)** - Zero-copy via `CollectionsMarshal`
- **ArraySegment (`ArraySegment<T>`)** - Optimized for segments

### Generic Types
- **IEnumerable (`IEnumerable<T>`)** - Via `AsValueEnumerable()` wrapper

## Operation Categories

### Filtering
- `Where()` - Filter elements by predicate

### Projection
- `Select()` - Transform elements

### Aggregation
- `Sum()` - Sum elements (supports generic math)
- `Count()` - Count elements
- `Any()` - Check if any elements exist

### Element Access
- `First()`, `FirstOrDefault()`, `FirstOrNone()` - Get first element
- `Single()`, `SingleOrDefault()`, `SingleOrNone()` - Get single element
- `Last()`, `LastOrDefault()`, `LastOrNone()` - Get last element

### Conversion
- `ToArray()` - Convert to array
- `ToList()` - Convert to list

---

## Coming Soon

- **Extension Methods Reference** - Complete method reference
- **Span Operations** - Span/Memory-specific operations
- **Value Enumerables** - Value enumerable API details

---

[← Back to Documentation Index](../README.md)
