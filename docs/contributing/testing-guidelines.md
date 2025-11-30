# NetFabric.Hyperlinq Unit Testing Guidelines

## Core Principles

### 1. Backward Compatibility First
**All tests MUST compare Hyperlinq results against System.Linq results**

This ensures:
- Backward compatibility with standard LINQ
- Correct behavior across all scenarios
- Confidence in refactoring and optimizations

```csharp
// ✅ GOOD: Compare against LINQ
var hyperlinqResult = array.Select(x => x * 2);
var linqResult = array.Select(x => x * 2);

hyperlinqResult.Must()
    .BeEnumerableOf<int>()
    .BeEqualTo(linqResult);

// ❌ BAD: Hardcoded expected values
var result = array.Select(x => x * 2);
result.Must()
    .BeEnumerableOf<int>()
    .BeEqualTo(new[] { 2, 4, 6 });  // Fragile!
```

### 2. Data-Source-Driven Tests
**Use parameterized tests with data sources for multiple scenarios**

Benefits:
- Test multiple scenarios with single test method
- Easy to add new test cases
- Consistent test coverage
- Reduced code duplication

```csharp
// Define data sources
public static IEnumerable<(Func<int[]> arrayFactory, string description)> GetIntArraySources()
{
    yield return (() => new int[] { 1, 2, 3, 4, 5 }, "Array with 5 elements");
    yield return (() => Array.Empty<int>(), "Empty array");
    yield return (() => new int[] { 42 }, "Single element");
    yield return (() => Enumerable.Range(1, 100).ToArray(), "Large array");
}

// Use in tests
[Test]
[MethodDataSource(nameof(GetIntArraySources))]
public void Array_Operation_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
{
    var array = testCase.arrayFactory();
    
    var hyperlinqResult = array.SomeOperation();
    var linqResult = array.SomeOperation();
    
    hyperlinqResult.Must()
        .BeEnumerableOf<int>()
        .BeEqualTo(linqResult);
}
```

### 3. Test Isolation
**Use factory functions (Func<T>) for reference types**

Ensures each test gets a fresh instance:

```csharp
// ✅ GOOD: Factory function
yield return (() => new List<int> { 1, 2, 3 }, "List with 3 elements");

// ❌ BAD: Shared instance
var sharedList = new List<int> { 1, 2, 3 };
yield return (sharedList, "List with 3 elements");  // Tests might mutate!
```

## Test Organization

### File Structure
```
NetFabric.Hyperlinq.UnitTests/
├── TestDataSources.cs              # Shared data sources
├── AsValueEnumerableTests.cs       # Value enumerable wrapper tests
└── Span/
    ├── SpanCoreOperationsTests.cs  # Count, Any, First, Last
    ├── SpanSelectTests.cs          # Select operations
    ├── SpanSumTests.cs             # Sum operations
    └── SpanChainingTests.cs        # Chained operations
```

### Naming Conventions

**Test Classes:**
- `{Feature}Tests` - e.g., `SpanSelectTests`, `AsValueEnumerableTests`

**Test Methods:**
- `{SourceType}_{Operation}_{Expectation}` - e.g., `Array_Select_ShouldMatchLinq`
- Use `ShouldMatchLinq` for comparison tests
- Use descriptive names for specific scenarios

**Data Sources:**
- `Get{Type}{Collection}Sources` - e.g., `GetIntArraySources`, `GetDoubleListSources`

## Standard Data Sources

### Shared Data Sources (TestDataSources.cs)
```csharp
public static class TestDataSources
{
    // For IEnumerable<T> tests
    public static IEnumerable<(Func<IEnumerable<int>> sourceFactory, string description)> 
        GetIntEnumerableSources()
    {
        yield return (() => new List<int> { 1, 2, 3, 4, 5 }, "List with 5 elements");
        yield return (() => new int[] { 10, 20, 30 }, "Array with 3 elements");
        yield return (() => new List<int>(), "Empty list");
        yield return (() => new int[] { 42 }, "Single element");
        yield return (() => Enumerable.Range(1, 100), "Large enumerable");
    }
    
    // For ICollection<T> tests
    public static IEnumerable<(Func<ICollection<int>> sourceFactory, string description)> 
        GetIntCollectionSources() { /* ... */ }
}
```

### Test-Specific Data Sources
For span-specific or specialized tests, define data sources in the test class:

```csharp
public class SpanSelectTests
{
    public static IEnumerable<(Func<int[]> arrayFactory, string description)> GetIntArraySources()
    {
        yield return (() => new int[] { 1, 2, 3, 4, 5 }, "Array with 5 elements");
        yield return (() => Array.Empty<int>(), "Empty array");
        // ... more scenarios
    }
}
```

## Test Patterns

### Pattern 1: Basic Operation Test
```csharp
[Test]
[MethodDataSource(nameof(GetIntArraySources))]
public void Array_Sum_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
{
    var array = testCase.arrayFactory();
    
    var hyperlinqResult = array.Sum();
    var linqResult = Enumerable.Sum(array);
    
    await Assert.That(hyperlinqResult).IsEqualTo(linqResult);
}
```

### Pattern 2: Chained Operations Test
```csharp
[Test]
[MethodDataSource(nameof(GetIntArraySources))]
public void Array_WhereSelect_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
{
    var array = testCase.arrayFactory();
    
    var hyperlinqResult = array
        .Where(x => x % 2 == 0)
        .Select(x => x * 10);
    
    var linqResult = array
        .Where(x => x % 2 == 0)
        .Select(x => x * 10);
    
    hyperlinqResult.Must()
        .BeEnumerableOf<int>()
        .BeEqualTo(linqResult);
}
```

### Pattern 3: Property Preservation Test
```csharp
[Test]
[MethodDataSource(nameof(GetIntArraySources))]
public async Task Array_Select_Count_ShouldMatchSource((Func<int[]> arrayFactory, string description) testCase)
{
    var array = testCase.arrayFactory();
    var result = array.Select(x => x * 2);
    
    // Verify Count property is preserved
    await Assert.That(result.Count).IsEqualTo(array.Length);
}
```

### Pattern 4: Type-Specific Test
For tests that don't benefit from parameterization:

```csharp
[Test]
public void Array_Select_Double_ShouldMatchLinq()
{
    var array = new double[] { 1.5, 2.5, 3.5 };
    
    var hyperlinqResult = array.Select(x => x * 2.0);
    var linqResult = array.Select(x => x * 2.0);
    
    hyperlinqResult.Must()
        .BeEnumerableOf<double>()
        .BeEqualTo(linqResult);
}
```

## Common Scenarios to Test

### Essential Coverage
- ✅ Empty collections
- ✅ Single element
- ✅ Small collections (3-5 elements)
- ✅ Large collections (100+ elements)
- ✅ Different numeric types (int, long, double, float)
- ✅ Reference types (string, custom objects)

### Operation-Specific
- ✅ Chained operations (Where + Select, multiple Where, etc.)
- ✅ Property preservation (Count, indexer access)
- ✅ Edge cases (null predicates should throw, etc.)
- ✅ Type changes (int → string, etc.)

## Assertions

### NetFabric.Assertive - Comprehensive Enumerable Testing

**`BeEnumerableOf<T>()` tests ALL forms of enumeration:**
- ✅ `IEnumerable<T>.GetEnumerator()`
- ✅ `IEnumerable.GetEnumerator()` (non-generic)
- ✅ Public `GetEnumerator()` methods
- ✅ Indexer access (`this[int]`) for `IReadOnlyList<T>` and `IList<T>`
- ✅ `Count` property correctness
- ✅ Multiple enumeration consistency

**This means you DON'T need separate tests for:**
- ❌ Indexer access - `BeEnumerableOf` already tests it
- ❌ Count property - `BeEnumerableOf` already verifies it
- ❌ Multiple enumeration - `BeEnumerableOf` tests consistency

**Example:**
```csharp
result.Must()
    .BeEnumerableOf<int>()
    .BeEqualTo(expected);
// This ALREADY tests:
// - Enumeration via GetEnumerator()
// - Indexer access (if IReadOnlyList/IList)
// - Count property
// - Multiple enumeration forms
```

### When to Use NetFabric.Assertive vs TUnit

**Use NetFabric.Assertive for:**
- ✅ **Enumerables** - Use `.Must().BeEnumerableOf<T>().BeEqualTo(expected)`
- ✅ **Scalar values** - Use `.Must().BeEqualTo(expected)`
- ✅ **Type assertions** - Use `.Must().BeOfType<T>()`
- ✅ **Exceptions** - Use `action.Must().Throw<T>()`
- ✅ **Enumerable properties** - Chain assertions after `BeEnumerableOf()`

```csharp
// Enumerable comparison
hyperlinqResult.Must()
    .BeEnumerableOf<int>()
    .BeEqualTo(linqResult);

// Scalar value comparison
result.Must().BeEqualTo(expected);

// Type assertion
valueEnum.Must().BeOfType<ListValueEnumerable<int>>();

// Exception assertions
Action action = () => array.First();
action.Must().Throw<InvalidOperationException>();
```

**Use TUnit Assert.That for:**
- ✅ **Numeric comparisons (ranges, approximations)**
- ✅ **Complex scalar conditions not supported by Assertive**

```csharp
// Numeric comparisons
await Assert.That(Math.Abs(result - expected)).IsLessThan(1e-10);
```

### String Comparisons (Type Changes)
When comparing results with type changes, use `SequenceEqual`:

```csharp
hyperlinqResult.Must()
    .BeEnumerableOf<string>()
    .EvaluateTrue(e => e.SequenceEqual(linqResult));
```

## Anti-Patterns

### ❌ DON'T: Hardcode Expected Values
```csharp
// BAD: Fragile and doesn't guarantee LINQ compatibility
var result = array.Where(x => x > 2);
result.Must().BeEqualTo(new[] { 3, 4, 5 });
```

### ❌ DON'T: Test Single Scenario
```csharp
// BAD: Only tests one scenario
[Test]
public void Array_Sum_ShouldWork()
{
    var array = new int[] { 1, 2, 3 };
    await Assert.That(array.Sum()).IsEqualTo(6);
}
```

### ❌ DON'T: Share Mutable State
```csharp
// BAD: Tests might interfere with each other
private static List<int> sharedList = new List<int> { 1, 2, 3 };
```

## Checklist for New Tests

Before submitting tests, verify:

- [ ] Tests compare against System.Linq results
- [ ] Uses data sources for multiple scenarios
- [ ] Covers empty, single, small, and large collections
- [ ] Uses factory functions for reference types
- [ ] Test names follow naming conventions
- [ ] No hardcoded expected values
- [ ] No shared mutable state
- [ ] All tests pass

## Example: Complete Test Class

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TUnit.Core;
using NetFabric.Assertive;

namespace NetFabric.Hyperlinq.UnitTests.Span;

public class SpanExampleTests
{
    // Data sources
    public static IEnumerable<(Func<int[]> arrayFactory, string description)> GetIntArraySources()
    {
        yield return (() => new int[] { 1, 2, 3, 4, 5 }, "Array with 5 elements");
        yield return (() => Array.Empty<int>(), "Empty array");
        yield return (() => new int[] { 42 }, "Single element");
        yield return (() => Enumerable.Range(1, 100).ToArray(), "Large array");
    }
    
    // Parameterized test
    [Test]
    [MethodDataSource(nameof(GetIntArraySources))]
    public void Array_Operation_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        
        var hyperlinqResult = array.SomeOperation();
        var linqResult = array.SomeOperation();
        
        hyperlinqResult.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(linqResult);
    }
    
    // Type-specific test
    [Test]
    public void Array_Operation_Double_ShouldMatchLinq()
    {
        var array = new double[] { 1.5, 2.5, 3.5 };
        
        var hyperlinqResult = array.SomeOperation();
        var linqResult = array.SomeOperation();
        
        hyperlinqResult.Must()
            .BeEnumerableOf<double>()
            .BeEqualTo(linqResult);
    }
}
```
