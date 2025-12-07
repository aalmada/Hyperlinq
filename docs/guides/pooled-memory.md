# Pooled Memory

NetFabric.Hyperlinq provides `ToArrayPooled()` extension methods that utilize `ArrayPool<T>` for temporary materialization. This significantly reduces Garbage Collector (GC) pressure by reusing arrays instead of allocating new ones for every operation.

## Why use Pooled Memory?

Standard LINQ's `ToArray()` and `ToList()` always allocate a new array or list. If you are materializing collections frequently (e.g., inside a loop or high-throughput request handler) just to iterate over them once and discard them, this creates a lot of work for the GC.

Pooled memory allows you to "rent" an array, use it, and "return" it.

## Basic Usage

The `ToArrayPooled()` method returns a `PooledBuffer<T>`, which implements `IDisposable`.

> [!IMPORTANT]
> You **must** dispose of the `PooledBuffer<T>` to return the array to the pool. The `using` statement is the best way to ensure this.

```csharp
using NetFabric.Hyperlinq;

var data = new int[] { 1, 2, 3, 4, 5 };

// Materialize to a pooled buffer
using var buffer = data.AsSpan()
    .Where(x => x % 2 == 0)
    .ToArrayPooled();

// Access the data via AsSpan()
foreach (var item in buffer.AsSpan())
{
    Console.WriteLine(item);
}
```

## `PooledBuffer<T>`

The `PooledBuffer<T>` struct is a lightweight wrapper around the rented array.

### Properties and Methods

- **`Length`**: The number of elements in the buffer.
- **`AsSpan()`**: Returns a `ReadOnlySpan<T>` of the valid data in the buffer.
- **`ToArray()`**: Creates a new, independent `T[]` copy of the data (useful if you need to keep the data after disposing the buffer).
- **`Dispose()`**: Returns the underlying array to the `ArrayPool<T>`.

### Reference Types

If `T` is a reference type (e.g., `string`, `object`), `PooledBuffer<T>` automatically clears the array when returning it to the pool to prevent memory leaks (references held by the pooled array).

## Custom ArrayPool

By default, `ToArrayPooled` uses `ArrayPool<T>.Shared`. You can also provide your own `ArrayPool<T>` instance.

```csharp
using System.Buffers;

// Create a custom pool
var myPool = ArrayPool<int>.Create(maxArrayLength: 1000, maxArraysPerBucket: 50);

using var buffer = data.AsSpan()
    .ToArrayPooled(myPool);
```

This is useful for:
- Isolating memory usage of different components.
- Configuring pool limits (max array size, max arrays per bucket).
- Monitoring pool usage.

## Supported Types

Pooled memory methods are available for:
- `ReadOnlySpan<T>`
- `List<T>` (via `AsValueEnumerable()`)
- `Where` enumerables
- `WhereSelect` enumerables

## Best Practices

1.  **Always Dispose**: Use `using` to ensure the buffer is returned.
2.  **Short-Lived**: Use pooled buffers for temporary, short-lived data.
3.  **Don't Leak**: Do not store references to the underlying array or span returned by `AsSpan()` after the buffer is disposed.
4.  **Use AsSpan()**: Access data via `AsSpan()` for zero-copy access.
