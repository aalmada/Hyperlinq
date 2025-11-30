# First/Single Method Implementation Rules

## Overview

This document defines the mandatory rules for implementing `First*()` and `Single*()` methods across the NetFabric.Hyperlinq codebase to ensure consistency, maintainability, and correctness.

---

## Core Rules

### Rule 1: Symmetry Requirement

**If a type implements `First()`, it MUST implement `Single()` (and vice versa)**

Both methods are fundamental element access operations and must be provided together to ensure API completeness.

**Example**:
```csharp
// ✅ CORRECT - Both First() and Single() present
public T First() => FirstOrNone().Value;
public T Single() => SingleOrNone().Value;

// ❌ INCORRECT - Only First() present
public T First() => FirstOrNone().Value;
// Missing Single()
```

---

### Rule 2: Complete Method Family

**If a type implements ANY `First*()` or `Single*()` method, it MUST implement ALL 8 methods:**

#### First Family (4 methods):
1. `First()` - Returns first element, throws if empty
2. `FirstOrDefault()` - Returns first element or `default(T)`
3. `FirstOrDefault(T defaultValue)` - Returns first element or specified default
4. `FirstOrNone()` - Returns `Option<T>` with first element

#### Single Family (4 methods):
1. `Single()` - Returns only element, throws if empty or multiple
2. `SingleOrDefault()` - Returns only element or `default(T)`
3. `SingleOrDefault(T defaultValue)` - Returns only element or specified default
4. `SingleOrNone()` - Returns `Option<T>` with only element

**Example**:
```csharp
// ✅ CORRECT - Complete First family
public T First() => FirstOrNone().Value;
public T FirstOrDefault() => FirstOrNone().GetValueOrDefault();
public T FirstOrDefault(T defaultValue) => FirstOrNone().GetValueOrDefault(defaultValue);
public Option<T> FirstOrNone() { /* implementation */ }

// ✅ CORRECT - Complete Single family
public T Single() => SingleOrNone().Value;
public T SingleOrDefault() => SingleOrNone().GetValueOrDefault();
public T SingleOrDefault(T defaultValue) => SingleOrNone().GetValueOrDefault(defaultValue);
public Option<T> SingleOrNone() { /* implementation */ }

// ❌ INCORRECT - Incomplete family
public T First() => FirstOrNone().Value;
public Option<T> FirstOrNone() { /* implementation */ }
// Missing FirstOrDefault() and FirstOrDefault(T)
```

---

### Rule 3: Single Source of Truth

**`*OrNone()` methods are the ONLY algorithm implementations. All other methods MUST be thin wrappers.**

- `FirstOrNone()` contains the actual iteration and logic for finding the first element
- `SingleOrNone()` contains the actual iteration and logic for finding the single element
- All other methods (`First()`, `FirstOrDefault()`, etc.) MUST delegate to `*OrNone()`

**Benefits**:
- Single source of truth for algorithm logic
- Consistent behavior across all variants
- Easier maintenance and bug fixes
- Simpler testing (test `*OrNone()` validates all variants)

**Example**:
```csharp
// ✅ CORRECT - FirstOrNone() has the algorithm
public Option<T> FirstOrNone()
{
    foreach (var item in source)
    {
        if (predicate(item))
            return Option.Some(item);
    }
    return Option.None<T>();
}

// ✅ CORRECT - All others delegate to FirstOrNone()
public T First() => FirstOrNone().Value;
public T FirstOrDefault() => FirstOrNone().GetValueOrDefault();
public T FirstOrDefault(T defaultValue) => FirstOrNone().GetValueOrDefault(defaultValue);

// ❌ INCORRECT - First() has its own implementation
public T First()
{
    foreach (var item in source)  // ❌ Duplicate logic
    {
        if (predicate(item))
            return item;
    }
    throw new InvalidOperationException();
}
```

---

## Implementation Templates

### Template: Parameterless Methods

```csharp
/// <summary>
/// Returns the first element of the sequence.
/// </summary>
/// <exception cref="InvalidOperationException">The sequence is empty.</exception>
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public T First()
    => FirstOrNone().Value;

/// <summary>
/// Returns the first element, or default(T) if empty.
/// </summary>
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public T FirstOrDefault()
    => FirstOrNone().GetValueOrDefault();

/// <summary>
/// Returns the first element, or the specified default value if empty.
/// </summary>
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public T FirstOrDefault(T defaultValue)
    => FirstOrNone().GetValueOrDefault(defaultValue);

/// <summary>
/// Returns an Option containing the first element, or None if empty.
/// </summary>
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public Option<T> FirstOrNone()
{
    // ALGORITHM IMPLEMENTATION - Only this method has actual logic
    foreach (var item in source)
    {
        return Option.Some(item);
    }
    return Option.None<T>();
}

/// <summary>
/// Returns the only element of the sequence.
/// </summary>
/// <exception cref="InvalidOperationException">The sequence is empty or contains more than one element.</exception>
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public T Single()
    => SingleOrNone().Value;

/// <summary>
/// Returns the only element, or default(T) if empty.
/// </summary>
/// <exception cref="InvalidOperationException">The sequence contains more than one element.</exception>
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public T SingleOrDefault()
    => SingleOrNone().GetValueOrDefault();

/// <summary>
/// Returns the only element, or the specified default value if empty.
/// </summary>
/// <exception cref="InvalidOperationException">The sequence contains more than one element.</exception>
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public T SingleOrDefault(T defaultValue)
    => SingleOrNone().GetValueOrDefault(defaultValue);

/// <summary>
/// Returns an Option containing the only element, or None if empty or multiple.
/// </summary>
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public Option<T> SingleOrNone()
{
    // ALGORITHM IMPLEMENTATION - Only this method has actual logic
    var found = false;
    var result = default(T);
    foreach (var item in source)
    {
        if (found)
            return Option.None<T>(); // Multiple elements
        found = true;
        result = item;
    }
    return found ? Option.Some(result!) : Option.None<T>();
}
```

### Template: Methods with Predicate

```csharp
/// <summary>
/// Returns the first element that satisfies the condition.
/// </summary>
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public T First(Func<T, bool> predicate)
    => FirstOrNone(predicate).Value;

/// <summary>
/// Returns the first element that satisfies the condition, or default(T).
/// </summary>
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public T FirstOrDefault(Func<T, bool> predicate)
    => FirstOrNone(predicate).GetValueOrDefault();

/// <summary>
/// Returns the first element that satisfies the condition, or the specified default.
/// </summary>
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public T FirstOrDefault(Func<T, bool> predicate, T defaultValue)
    => FirstOrNone(predicate).GetValueOrDefault(defaultValue);

/// <summary>
/// Returns an Option containing the first element that satisfies the condition.
/// </summary>
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public Option<T> FirstOrNone(Func<T, bool> predicate)
{
    // ALGORITHM IMPLEMENTATION
    foreach (var item in source)
    {
        if (predicate(item))
            return Option.Some(item);
    }
    return Option.None<T>();
}

/// <summary>
/// Returns the only element that satisfies the condition.
/// </summary>
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public T Single(Func<T, bool> predicate)
    => SingleOrNone(predicate).Value;

/// <summary>
/// Returns the only element that satisfies the condition, or default(T).
/// </summary>
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public T SingleOrDefault(Func<T, bool> predicate)
    => SingleOrNone(predicate).GetValueOrDefault();

/// <summary>
/// Returns the only element that satisfies the condition, or the specified default.
/// </summary>
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public T SingleOrDefault(Func<T, bool> predicate, T defaultValue)
    => SingleOrNone(predicate).GetValueOrDefault(defaultValue);

/// <summary>
/// Returns an Option containing the only element that satisfies the condition.
/// </summary>
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public Option<T> SingleOrNone(Func<T, bool> predicate)
{
    // ALGORITHM IMPLEMENTATION
    var found = false;
    var result = default(T);
    foreach (var item in source)
    {
        if (predicate(item))
        {
            if (found)
                return Option.None<T>(); // Multiple matching elements
            found = true;
            result = item;
        }
    }
    return found ? Option.Some(result!) : Option.None<T>();
}
```

---

## Validation Checklist

When implementing or reviewing First/Single methods, verify:

- [ ] **Rule 1**: If `First()` exists, does `Single()` exist?
- [ ] **Rule 1**: If `Single()` exists, does `First()` exist?
- [ ] **Rule 2**: Are all 4 First variants present?
  - [ ] `First()`
  - [ ] `FirstOrDefault()`
  - [ ] `FirstOrDefault(T defaultValue)`
  - [ ] `FirstOrNone()`
- [ ] **Rule 2**: Are all 4 Single variants present?
  - [ ] `Single()`
  - [ ] `SingleOrDefault()`
  - [ ] `SingleOrDefault(T defaultValue)`
  - [ ] `SingleOrNone()`
- [ ] **Rule 3**: Does `FirstOrNone()` contain the actual algorithm?
- [ ] **Rule 3**: Does `SingleOrNone()` contain the actual algorithm?
- [ ] **Rule 3**: Does `First()` delegate to `FirstOrNone()`?
- [ ] **Rule 3**: Does `FirstOrDefault()` delegate to `FirstOrNone()`?
- [ ] **Rule 3**: Does `FirstOrDefault(T)` delegate to `FirstOrNone()`?
- [ ] **Rule 3**: Does `Single()` delegate to `SingleOrNone()`?
- [ ] **Rule 3**: Does `SingleOrDefault()` delegate to `SingleOrNone()`?
- [ ] **Rule 3**: Does `SingleOrDefault(T)` delegate to `SingleOrNone()`?
- [ ] All methods marked with `[MethodImpl(MethodImplOptions.AggressiveInlining)]`?

---

## Common Violations

### ❌ Violation 1: Incomplete Method Family

```csharp
// ❌ WRONG - Missing FirstOrDefault(T) and all Single methods
public T First() => FirstOrNone().Value;
public T FirstOrDefault() => FirstOrNone().GetValueOrDefault();
public Option<T> FirstOrNone() { /* ... */ }
```

### ❌ Violation 2: Duplicate Algorithm Implementation

```csharp
// ❌ WRONG - Both First() and FirstOrNone() have iteration logic
public T First()
{
    foreach (var item in source)  // ❌ Duplicate
        return item;
    throw new InvalidOperationException();
}

public Option<T> FirstOrNone()
{
    foreach (var item in source)  // ❌ Duplicate
        return Option.Some(item);
    return Option.None<T>();
}
```

### ❌ Violation 3: Wrong Delegation

```csharp
// ❌ WRONG - First() delegates to external source instead of own FirstOrNone()
public T First()
    => source.FirstOrNone(predicate).Value;  // ❌ Should use own FirstOrNone()

public Option<T> FirstOrNone()
{
    // ... implementation
}
```

---

## Rationale

### Why `*OrNone()` as the Single Implementation?

1. **Type Safety**: `Option<T>` explicitly represents "may or may not have a value"
2. **No Exceptions**: `*OrNone()` never throws, making it the safest variant
3. **Composability**: `Option<T>` can be easily converted to any other variant
4. **Testability**: Testing `*OrNone()` validates all other methods

### Why All 8 Methods?

1. **API Completeness**: Users expect symmetry between First and Single
2. **Flexibility**: Different use cases need different error handling
3. **LINQ Compatibility**: Matches System.Linq API surface
4. **Discoverability**: IntelliSense shows all options

### Why Inline Wrappers?

1. **Zero Overhead**: Inlining eliminates method call overhead
2. **Simplicity**: Wrappers are trivial one-liners
3. **Maintainability**: Changes only need to be made in `*OrNone()`

---

## Enforcement

These rules are **mandatory** for all types that implement element access methods. Violations should be caught during:

1. Code review
2. Automated validation scripts
3. Unit test coverage analysis

---

## See Also

- [Validation Report](validation_report.md) - Current violations and fixes
- [Option<T> Documentation](../Option.md) - Understanding the Option type
- [CODING_GUIDELINES.md](../../CODING_GUIDELINES.md) - General coding standards
