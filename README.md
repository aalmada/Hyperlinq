# NetFabric.Hyperlinq

High‑performance LINQ‑style operations using value‑type enumerables and span‑based extensions with zero allocations.

## Quick Start
```bash
dotnet add package NetFabric.Hyperlinq
```
```csharp
using NetFabric.Hyperlinq; // required for List<T> and contiguous memory types (array, Span<T>, Memory<T>)

int[] numbers = { 1, 2, 3, 4, 5 };
var sum = numbers.Sum();               // SIMD‑optimized, works for any numeric type supporting generic math
var listSum = new List<int>{1,2,3}.Sum(); // zero‑copy via CollectionsMarshal
```
> **Note**: For `IEnumerable<T>` sources (e.g., custom collections) you must call `.AsValueEnumerable()` first. The built‑in Roslyn analyzer will suggest this automatically.

## Supported Types
| Type | Usage |
|------|-------|
| `T[]` | Direct `using NetFabric.Hyperlinq;` – fastest |
| `Span<T>` / `ReadOnlySpan<T>` | Direct `using NetFabric.Hyperlinq;` |
| `Memory<T>` / `ReadOnlyMemory<T>` | Direct `using NetFabric.Hyperlinq;` |
| `List<T>` | Direct `using NetFabric.Hyperlinq;` – zero‑copy via `CollectionsMarshal` |
| `IEnumerable<T>` | Call `.AsValueEnumerable()` (analyzer can add it) |

## Aggregation – `Sum`
`Sum()` works with any type that implements `IAdditionOperators<T, T, T>` and `IAdditiveIdentity<T, T>` (generic math). No extra overloads are needed.
```csharp
double[] d = { 1.5, 2.5, 3.5 };
var dSum = d.Sum(); // 7.5

BigInteger[] big = { new BigInteger(1), new BigInteger(2) };
var bigSum = big.Sum(); // 3
```

## Analyzer & Code Fix
The Roslyn analyzer detects when a source can be wrapped with `AsValueEnumerable()` and offers a one‑click fix.
```csharp
var list = new List<int>{1,2,3};
var result = list.Where(x => x > 1); // ⚠️ NFHYPERLINQ001 – suggest AsValueEnumerable()
// After fix:
var result = list.AsValueEnumerable().Where(x => x > 1);
```

## Documentation
* **[CODING_GUIDELINES.md](CODING_GUIDELINES.md)** – Architecture, patterns, and standards
* **[OPTIMIZATION_GUIDELINES.md](OPTIMIZATION_GUIDELINES.md)** – Low-level performance techniques

## Contributing
Contributions are welcome! Please read the guidelines linked above before submitting PRs.

---
© 2025 NetFabric – MIT License
