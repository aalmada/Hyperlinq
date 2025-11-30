# Architecture

Understanding the design principles and internal architecture of NetFabric.Hyperlinq.

## Overview

NetFabric.Hyperlinq is built on several core architectural principles:

1. **Value-Type Enumerables**: Using structs instead of classes to avoid heap allocations
2. **Ref Structs**: Leveraging `ref struct` for stack-only, zero-allocation operations
3. **Generic Math**: Supporting any numeric type through C# 11 generic math
4. **SIMD Optimization**: Vectorized operations for maximum performance
5. **Operation Fusion**: Automatic fusion of chained operations

## Core Concepts

### Value Enumerables

Hyperlinq uses value-type enumerables (`struct` instead of `class`) to eliminate heap allocations during enumeration. This is achieved through:

- `IValueEnumerable<T, TEnumerator>` interface
- Struct-based enumerator types
- Extension methods that preserve value semantics

### Ref Structs for Spans

For `ReadOnlySpan<T>` operations, Hyperlinq uses `readonly ref struct` types that:
- Live entirely on the stack
- Cannot be boxed or stored in fields
- Enable zero-allocation LINQ-style operations
- Support operation fusion

### Extension Method Organization

Extension methods are organized by:
- **Target type**: Separate files for arrays, lists, spans, etc.
- **Operation category**: Filtering, projection, aggregation, etc.
- **Syntax**: New C# 14 `extension` syntax vs old `this` syntax

---

## Coming Soon

- **Design Principles** - Core design philosophy
- **Ref Struct Patterns** - Patterns for ref struct implementations
- **Value Enumerable Design** - Deep dive into value enumerable architecture
- **Performance Architecture** - How performance optimizations are structured

---

[← Back to Documentation Index](../README.md)
