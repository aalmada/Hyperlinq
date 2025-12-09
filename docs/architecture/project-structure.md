# Project Structure

This document explains the organization of the NetFabric.Hyperlinq project and provides guidance on navigating the codebase.

## Overview

The project has been organized to clearly separate concerns and make it easier to find and maintain code:

- **Type-specific extensions** are in `Extensions/ByType/`
- **Operation implementations** are in `Extensions/Operations/`
- **Tests** mirror the operation structure in `Operations/`

## Directory Structure

### Core Library (`NetFabric.Hyperlinq/`)

```
NetFabric.Hyperlinq/
├── Extensions/
│   ├── ByType/                          # Type-specific extension entry points
│   │   ├── ArraySegmentExtensions.cs
│   │   ├── ArrayValueEnumerableExtensions.cs
│   │   ├── EnumerableValueEnumerableExtensions.cs
│   │   ├── ListExtensions.cs
│   │   ├── ListValueEnumerableExtensions.cs
│   │   ├── MemoryExtensions.cs
│   │   ├── ReadOnlySpanExtensions.cs
│   │   └── WhereEnumerableExtensions.cs
│   └── Operations/                      # Operation implementations
│       ├── Aggregation/                 # Count, Sum, Min, Max
│       ├── Element/                     # First, Single, Last, etc.
│       ├── Filtering/                   # Where operations
│       │   ├── Enumerables/            # Enumerable struct definitions
│       │   ├── Extensions/             # Extension methods for enumerables
│       │   └── ValueEnumerableExtensions.Where.cs
│       ├── Projection/                  # Select operations
│       │   ├── Enumerables/
│       │   ├── Extensions/
│       │   └── ValueEnumerableExtensions.Select.cs
│       └── Quantifier/                  # Any operations
├── Generation/                          # Value enumerable factories (Range, etc.)
├── Utils/                               # Internal utilities
└── Wrappers/                            # Value enumerable wrappers
```

### Test Project (`NetFabric.Hyperlinq.UnitTests/`)

```
NetFabric.Hyperlinq.UnitTests/
├── Operations/                          # Operation-specific tests
│   ├── Aggregation/
│   │   ├── SpanCountTests.cs
│   │   ├── SpanMinMaxTests.cs
│   │   └── SpanSumTests.cs
│   ├── Element/
│   │   ├── SpanFirstTests.cs
│   │   ├── SpanFirstOrDefaultTests.cs
│   │   ├── SpanFirstOrNoneTests.cs
│   │   ├── SpanLastTests.cs
│   │   ├── SpanSingleOrDefaultTests.cs
│   │   └── SpanSingleOrNoneTests.cs
│   ├── Filtering/
│   │   ├── SpanWhereTests.cs
│   │   ├── SpanWhereSelectTests.cs
│   │   ├── SpanWhereSumTests.cs
│   │   └── SpanWhereSelectSumTests.cs
│   ├── Projection/
│   │   └── SpanSelectTests.cs
│   └── Quantifier/
│       └── SpanAnyTests.cs
├── Wrappers/                            # Wrapper tests
├── Utils/                               # Utility tests
└── AsValueEnumerable*.cs                # Integration tests
```

## Finding Code

### I want to find extensions for a specific type

Look in `Extensions/ByType/`:

- **Arrays**: `ArrayValueEnumerableExtensions.cs`
- **Lists**: `ListExtensions.cs` or `ListValueEnumerableExtensions.cs`
- **Spans**: `ReadOnlySpanExtensions.cs`
- **Memory**: `MemoryExtensions.cs`
- **ArraySegment**: `ArraySegmentExtensions.cs`
- **IEnumerable**: `EnumerableValueEnumerableExtensions.cs`

### I want to understand how an operation works

Look in `Extensions/Operations/{OperationCategory}/`:

- **Where/filtering**: `Operations/Filtering/`
- **Select/projection**: `Operations/Projection/`
- **Count/Sum/Min/Max**: `Operations/Aggregation/`
- **First/Single/Last**: `Operations/Element/`
- **Any**: `Operations/Quantifier/`

Within each operation directory:
- `Enumerables/` contains the enumerable struct definitions (e.g., `WhereListEnumerable`)
- `Extensions/` contains extension methods for those enumerables (e.g., `WhereListEnumerableExtensions`)
- `ValueEnumerableExtensions.{Operation}.cs` contains generic extensions

### I want to find tests for a feature

- **Operation-specific tests**: Look in `UnitTests/Operations/{OperationCategory}/`
- **Integration tests**: Look for `AsValueEnumerable*.cs` files in the test root
- **Wrapper tests**: `UnitTests/Wrappers/`

## Adding New Features

### Adding a new operation (e.g., "Take")

1. Create `Extensions/Operations/Take/` directory
2. Add enumerable definitions in `Take/Enumerables/`
3. Add extension methods in `Take/Extensions/`
4. Add generic extensions in `Take/ValueEnumerableExtensions.Take.cs`
5. Create tests in `UnitTests/Operations/Take/`

### Adding support for a new type

1. Create a new file in `Extensions/ByType/` (e.g., `ImmutableArrayExtensions.cs`)
2. Implement extensions that delegate to existing operations
3. Create enumerable wrappers if needed in `Wrappers/`
4. Add tests

### Adding a new enumerable variant

For example, adding a `WhereArrayEnumerable`:

1. Create `WhereArrayEnumerable.cs` in `Extensions/Operations/Filtering/Enumerables/`
2. Create `WhereArrayEnumerableExtensions.cs` in `Extensions/Operations/Filtering/Extensions/`
3. Update type-specific extensions in `Extensions/ByType/ArrayValueEnumerableExtensions.cs` to use the new enumerable
4. Add tests in `UnitTests/Operations/Filtering/`

## Design Principles

### Separation of Concerns

- **ByType extensions** are entry points - they know about specific types but delegate to operations
- **Operations** are reusable - they don't know about specific entry point types
- **Enumerables** are data structures - they define how to iterate
- **Extensions** are behaviors - they define what operations can be performed

### Namespace Consistency

All files use the `NetFabric.Hyperlinq` namespace regardless of their physical location. This allows files to be organized by concern without affecting the public API.

### Test Organization

Tests mirror the operation structure to make it easy to find tests for a specific operation. Integration tests that span multiple operations remain at the root level.

## Common Patterns

### Fusion Methods

Fusion methods (like `Sum()` on `WhereListEnumerable`) are implemented as extension methods in the `Extensions/` subdirectory of each operation. This allows chaining operations without materializing intermediate results.

### Value Delegates

Operations use value delegates (`IFunction<T, TResult>`, `IFunctionIn<T, TResult>`, etc.) for zero-allocation predicate and selector passing. These are defined in the core library root.

### Ref Struct Enumerables

Ref struct enumerables (e.g., `WhereReadOnlySpanEnumerable`) are used for span-based operations to ensure zero allocations. These are always in the `Enumerables/` subdirectory.

## Migration Notes

This structure was established in December 2024 to improve maintainability as the project grew. The reorganization:

- Moved all type-specific extensions to `Extensions/ByType/`
- Moved all operations to `Extensions/Operations/`
- Separated enumerable definitions from extension methods
- Reorganized tests to mirror the operation structure

No code changes were required - only file moves, as C# allows files to be in any directory regardless of namespace.
