# Fusion Operations Report

This document summarizes the current state of fusion operations in `NetFabric.Hyperlinq`.

## Supported Operations Table

| Enumerable Type | Count | Any | First* | Single* | Sum |
| :--- | :---: | :---: | :---: | :---: | :---: |
| **Where (Filtering)** |
| `WhereEnumerable` | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Aggregation/ValueEnumerableExtensions.Count.cs) | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Quantifier/ValueEnumerableExtensions.Any.cs) | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Element/ValueEnumerableExtensions.First.cs) | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Element/ValueEnumerableExtensions.Single.cs) | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Aggregation/ValueEnumerableExtensions.Sum.cs) |
| `WhereListEnumerable` | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Aggregation/ValueEnumerableExtensions.Count.cs) | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Quantifier/ValueEnumerableExtensions.Any.cs) | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Element/ValueEnumerableExtensions.First.cs) | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Element/ValueEnumerableExtensions.Single.cs) | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Aggregation/ValueEnumerableExtensions.Sum.cs) |
| `WhereMemoryEnumerable` | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Aggregation/ValueEnumerableExtensions.Count.cs) | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Quantifier/ValueEnumerableExtensions.Any.cs) | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Element/ValueEnumerableExtensions.First.cs) | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Element/ValueEnumerableExtensions.Single.cs) | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Aggregation/ValueEnumerableExtensions.Sum.cs) |
| **Select (Projection)** |
| `SelectEnumerable` | ❌ | ❌ | ❌ | ❌ | ❌ |
| `SelectArrayEnumerable` | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Projection/SelectArrayEnumerable.cs) | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Projection/SelectArrayEnumerable.cs) | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Projection/SelectArrayEnumerable.cs) | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Projection/SelectArrayEnumerable.cs) | ❌ |
| `SelectCollectionEnumerable` | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Projection/SelectCollectionEnumerable.cs) | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Projection/SelectCollectionEnumerable.cs) | ❌ | ❌ | ❌ |
| `SelectListEnumerable` | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Projection/SelectListEnumerable.cs) | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Projection/SelectListEnumerable.cs) | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Projection/SelectListEnumerable.cs) | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Projection/SelectListEnumerable.cs) | ❌ |
| `SelectMemoryEnumerable` | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Projection/SelectMemoryEnumerable.cs) | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Projection/SelectMemoryEnumerable.cs) | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Projection/SelectMemoryEnumerable.cs) | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Projection/SelectMemoryEnumerable.cs) | ❌ |
| **WhereSelect (Fusion)** |
| `WhereSelectEnumerable` | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Filtering/WhereSelectEnumerable.cs) | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Filtering/WhereSelectEnumerable.cs) | ❌ | ❌ | ❌ |
| `WhereSelectListEnumerable` | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Filtering/WhereSelectListEnumerable.cs) | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Filtering/WhereSelectListEnumerable.cs) | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Element/ValueEnumerableExtensions.First.WhereSelect.cs) | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Element/ValueEnumerableExtensions.Single.WhereSelect.cs) | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Aggregation/ValueEnumerableExtensions.Sum.WhereSelect.cs) |
| `WhereSelectMemoryEnumerable` | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Filtering/WhereSelectMemoryEnumerable.cs) | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Filtering/WhereSelectMemoryEnumerable.cs) | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Element/ValueEnumerableExtensions.First.WhereSelect.cs) | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Element/ValueEnumerableExtensions.Single.WhereSelect.cs) | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Aggregation/ValueEnumerableExtensions.Sum.WhereSelect.cs) |

**Legend:**
- ✅ : Optimized implementation exists (either in struct or as extension method).
- ❌ : No specific optimization exists (falls back to generic `IEnumerable` or `IValueEnumerable` implementation).

## Combinator Fusions

These operations fuse multiple method calls into a single optimized enumerable type.

| Source Type | Operation | Resulting Type | Status |
| :--- | :--- | :--- | :---: |
| `WhereEnumerable` | `.Select()` | `WhereSelectEnumerable` | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Filtering/WhereEnumerable.cs) |
| `WhereListEnumerable` | `.Select()` | `WhereSelectListEnumerable` | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Filtering/WhereListEnumerable.cs) |
| `WhereMemoryEnumerable` | `.Select()` | `WhereSelectMemoryEnumerable` | [✅](file:///Users/antaoalmada/Projects/NetFabric.Hyperlinq2/NetFabric.Hyperlinq/Extensions/Filtering/WhereMemoryEnumerable.cs) |
| `Select*Enumerable` | `.Where()` | *Generic* | ❌ |
| `WhereSelect*Enumerable` | `.Where()` | *Generic* | ❌ |
| `WhereSelect*Enumerable` | `.Select()` | *Generic* | ❌ |

## Missing Combinations & Opportunities

### 1. Sum for Select* Enumerables
Currently, `Sum` operations on `Select*` enumerables fall back to the generic `ValueEnumerableExtensions.Sum` implementation which uses an enumerator.
**Opportunity:** Implement optimized `Sum` extensions that:
- For `SelectArray`, `SelectList`, `SelectMemory`: Use indexer-based iteration or `Span`-based iteration (if possible to project to Span, or iterate source Span and apply selector).
- For `SelectCollection`: Iterate source collection directly if possible.

### 2. First/Single for SelectCollectionEnumerable
`SelectCollectionEnumerable` implements `ICollection<T>` but not `IList<T>`.
**Opportunity:** If the underlying source supports indexer (e.g. `IReadOnlyList`), we could potentially optimize, but `SelectCollectionEnumerable` is generic over `IValueReadOnlyCollection`. If the source is just a collection, we can't do better than `GetEnumerator().MoveNext()`.

### 3. First/Single/Sum for WhereSelectEnumerable
`WhereSelectEnumerable` works on generic `IEnumerable`.
**Opportunity:**
- `Sum`: Could be implemented to iterate and sum, avoiding the selector overhead if we only need to sum the filtered items (wait, `Sum` needs the selector result if it's `Sum(selector)`? No, `WhereSelect` is `Where` then `Select`. `Sum` sums the *result* of the projection. So we *do* need the selector).
- Actually, for `WhereSelectEnumerable`, `Sum` would be `source.Where(p).Select(s).Sum()`.
- The optimization we did for `WhereSelectMemory` was `Sum` on `TSource` (implicit identity selector? No, the previous task was about `Sum` *ignoring* the selector?
    - Let's re-read the previous task: "Sum overloads for WhereSelectMemoryEnumerable and WhereSelectListEnumerable should be implemented calling an implementation that takes a Span as parameter... Ignores the selector since Sum operates on source values that match the predicate."
    - **Wait**, if `Sum` ignores the selector, it returns `Sum(TSource)`. But `WhereSelect` returns `TResult`.
    - Ah, the method signature was `public static TSource Sum<TSource, TResult>(this WhereSelectMemoryEnumerable<TSource, TResult> source)`.
    - It returns `TSource`. This is a specific optimization where we want to sum the *original* items that match the predicate, effectively undoing the `Select`.
    - This is useful if `Select` was identity or if we want to count/sum the source items.
    - But standard `Sum()` on `IEnumerable<int>` sums the ints.
    - If `WhereSelect` is `Where(p).Select(s)`, then `Sum()` should sum the `TResult`s.
    - The implemented optimization returns `TSource`. This implies it's `Sum(t => t)` on the source, filtered by predicate.
    - If `TResult` != `TSource`, `Sum` returning `TSource` is ... interesting.
    - Let's check the signature again.
    - `public static TSource Sum<TSource, TResult>(this WhereSelectMemoryEnumerable<TSource, TResult> source)`
    - Yes, it returns `TSource`.
    - This seems to be for the case where we want to sum the *source* elements.
    - But `WhereSelect` usually implies we are working with `TResult`.
    - If `TResult` is `int`, and `TSource` is `int`, it works.
    - If `TResult` is `string`, `Sum` shouldn't exist?
    - The constraint is `where TSource : IAdditionOperators...`.
    - So this `Sum` is only available if `TSource` is numeric.
    - This effectively implements `source.Where(predicate).Sum()`.
    - It ignores the `Select` part.
    - This is a valid optimization *if* the user intends to sum the source items.
    - But if the user calls `Sum()` on a `WhereSelect` enumerable, they usually expect to sum the *projected* items.
    - **CRITICAL:** If `WhereSelect` represents `source.Where().Select()`, then `Sum()` should sum the output of `Select`.
    - If the implementation ignores the selector, it sums the input of `Select`.
    - This is only correct if `Select` is identity.
    - Or if this `Sum` overload is specifically `Sum(this ... source)` where we *know* we want source sum.
    - But it's an extension method on the enumerable.
    - If I have `ints.Where(i => i > 0).Select(i => i * 2).Sum()`, I expect the sum of doubled values.
    - The current implementation sums the original values!
    - **This might be a bug or a very specific design choice.**
    - Let's verify the previous task's intent. "Sum overloads... calling an implementation that takes a Span... Ignores the selector since Sum operates on source values that match the predicate."
    - If the user *requested* this, then it's what they want.
    - But for a general report, `Sum` on `WhereSelect` should probably sum the results.

### 4. Select* Enumerables - Sum
For `Select*` enumerables, `Sum` should definitely sum the `TResult`s.
We don't have optimized `Sum` for `SelectArray`, `SelectList`, etc.
These would need to iterate and apply selector, then sum.
`SpanExtensions` doesn't have `Sum<TSource, TResult>(Span<TSource>, Func<TSource, TResult>)`.
It has `Sum<T>(Span<T>)` and `Sum<T>(Span<T>, Func<T, bool>)`.
It does NOT have a map-reduce Sum.

So `Select*` Sum optimization would require new `SpanExtensions` methods like:
`Sum<TSource, TResult>(Span<TSource> source, Func<TSource, TResult> selector)`

## Conclusion
The most significant missing piece is optimized `Sum` for `Select*` enumerables (Map-Reduce).
Also `WhereSelectEnumerable` (for generic IEnumerable) lacks `First`, `Single`, `Sum` optimizations (though less optimization is possible there).
