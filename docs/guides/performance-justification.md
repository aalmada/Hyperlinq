# Why Use NetFabric.Hyperlinq on Modern .NET?

Even with the significant performance improvements in modern .NET (including .NET 10), `NetFabric.Hyperlinq` maintains a distinct performance advantage through architectural choices that the standard `System.Linq` implementation cannot easily adopt due to backward compatibility.

## 1. Zero-Allocation Enumeration (Struct Chaining)

Standard LINQ (`IEnumerable<T>`) is built on reference types and interface dispatch. Even with optimizations, chaining operations often results in heap allocations for iterators.

**Hyperlinq Advantage:**  
Hyperlinq uses **Struct Chaining**. Each operation (`Where`, `Select`, etc.) returns a specific `struct` type (e.g., `WhereEnumerable<operand...>`).
*   **Zero Heap Allocations**: The entire query internal state is kept on the stack.
*   **Inlining Friendly**: The exact type is known at compile time, allowing the JIT to aggressively inline the entire query pipeline into a single loop, eliminating overhead.

## 2. Virtual Call Elimination (Duck Typing)

Standard LINQ relies on `IEnumerator<T>` for iteration, which requires virtual calls (`MoveNext`, `Current`) for every element, even if devirtualization optimizations exist.

**Hyperlinq Advantage:**  
Hyperlinq iterators use **Duck Typing**. They do not box to `IEnumerator<T>` unless cast.
*   **Virtual Call Elimination**: Method calls are direct, static calls to the struct's methods.
*   **State Machine Optimization**: The state machine logic is significantly simpler and more amenable to CPU branch prediction.

## 3. Value Delegates (`IFunction<T, R>`)

Modern .NET uses `Func<T, R>` delegates, which are reference types. Creating a closure (capturing variables) allocates a class instance on the heap. Invoking a delegate involves an indirect call.

**Hyperlinq Advantage:**  
Hyperlinq supports **Value Delegates** (structs implementing `IFunction`).
*   **Allocations**: Zero. Closures are structs passed by value (or reference).
*   **Indirection**: Zero. The `Invoke` method is a concrete method on a struct, allowing the JIT to inline the user's lambda logic directly into the query loop.

## 4. Enhanced SIMD & Vectorization

While .NET has added SIMD to some basic operations (like `Sum` on arrays), it generally requires using specific types strategies.

**Hyperlinq Advantage:**  
Hyperlinq checks the underlying memory layout (e.g., contiguous memory in `List<T>`, `Span<T>`) and automatically leverages **Vector<T>** and **TensorPrimitives** for operations like `Sum`, `Min`, `Max`—often achieving 10x-20x speedups over scalar iteration, even with filtering predicates (where possible).
The factory methods `ValueEnumerable.Range` and `ValueEnumerable.Repeat` also use SIMD to accelerate `ToArray`, `ToList`, and `CopyTo` operations.

## Comparison Summary

| Feature | System.Linq (.NET 10) | NetFabric.Hyperlinq | Impact |
| :--- | :--- | :--- | :--- |
| **Pipeline Composition** | Interface-based (Allocating) | Struct-based (Zero-Allocation) | **GC Pressure Eliminated** |
| **Iteration Call** | Virtual / Interface Dispatch | Direct Call (Inlinable) | **CPU Cycles Reduced** |
| **Closures** | Heap Allocated Classes | Stack Allocated Structs | **GC Pressure Eliminated** |
| **Optimization Check** | Runtime Type Checks (limited) | Compile-Time Type Resolution | **Runtime Overhead Removed** |

In scenarios requiring high-throughput data processing, strict latency controls, or zero-allocation budgets (e.g., game loops, real-time systems), Hyperlinq provides guarantees that standard LINQ cannot.
