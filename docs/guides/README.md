# Guides

In-depth guides for using NetFabric.Hyperlinq effectively.

## Available Guides

### [Fusion Operations](fusion-operations.md)
Learn how Hyperlinq automatically fuses operations like `Where().Select().Sum()` into a single pass for maximum performance.

**Topics covered:**
- What is operation fusion?
- How fusion works in Hyperlinq
- Performance benefits
- Best practices for fusion-friendly code
- Best practices for fusion-friendly code

### [Pooled Memory](pooled-memory.md)
Learn how to use `ToArrayPooled()` and `ToListPooled()` to reduce GC pressure by using `ArrayPool<T>`.

**Topics covered:**
- Why use pooled memory?
- `PooledBuffer<T>` usage and disposal
- Custom `ArrayPool<T>` support
- Best practices
---

## Coming Soon

- **Performance Guide** - Best practices for maximum performance
- **Benchmarking Guide** - How to benchmark your code with BenchmarkDotNet
- **Advanced Patterns** - Advanced usage patterns and techniques

---

[← Back to Documentation Index](../README.md)
