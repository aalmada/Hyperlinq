# Value Delegates

Value delegates are a technique to define operations (like predicates or selectors) using `struct` types implementing `IFunction` interfaces, rather than standard `Func` or `Action` delegates.

## Why use Value Delegates?

Standard delegates in C# (`Func<T, TResult>`) have three main performance overheads:
1.  **Delegate Allocation**: Creating the delegate instance allocates memory on the heap.
2.  **Closure Allocation**: If the delegate captures local variables, a closure class is allocated on the heap to hold them.
3.  **Virtual Call / No Inlining**: Delegate invocation is an indirect call. The JIT compiler often cannot inline the target method, blocking other optimizations (like loop unrolling or vectorization).

Value delegates solve all three problems.

### 1. Zero Allocations
By using a `struct`, the "delegate" is allocated on the stack (or embedded in another struct). No GC pressure.

### 2. Zero-Allocation Closures
State can be captured as fields in the struct, keeping it on the stack.

### 3. Aggressive Inlining
Because the struct type is known at compile time (via generics), the `Invoke` call is monomorphic. The JIT can verify the target exact method and **inline it completely**. This allows the JIT to see the "whole picture" of the loop + operation, enabling powerful optimizations.

## How to use

NetFabric.Hyperlinq defines `IFunction` interfaces in `NetFabric.Hyperlinq.Abstractions` (implied).

### Defining a Value Delegate

```csharp
using NetFabric.Hyperlinq;
using System.Runtime.CompilerServices;

public readonly struct GreaterThan : IFunction<int, bool>
{
    readonly int threshold;

    public GreaterThan(int threshold)
        => this.threshold = threshold;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Invoke(int item)
        => item > threshold;
}
```

### Applying the Delegate

Use the generic overloads of `Select`, `Where`, etc., found in `NetFabric.Hyperlinq`.

```csharp
var threshold = 10;
var result = source.Where<int, GreaterThan>(new GreaterThan(threshold));
```

## When is it advantageous?

### ✅ When there is a closure
If your logic captures variables from the surrounding scope, a standard lambda `x => x > threshold` will allocate a closure class. A value delegate struct `new GreaterThan(threshold)` will not.

### ✅ When the operation is tiny (Inlining)
If the operation is small (e.g., `x => x * 2`), the overhead of the delegate call is significant relative to the work. Value delegates allow inlining, eliminating the call overhead entirely.

### ✅ In tight loops
For operations running millions of times, the lack of inlining with standard delegates can be a major bottleneck.

## Using `Link` vs `Func`
Hyperlinq provides overloads for both.
-   `Select(Func)`: Easier to write, good performance (uses `FunctionWrapper` internally but still has delegate overhead).
-   `Select<...>(TSelector)`: Usage requires defining a struct, but yields **maximum performance**.

Use value delegates when you need to squeeze the absolute maximum performance out of a critical path.
