## Fusion Operations Checklist

If this PR adds a new enumerable type or operation, verify:

### New Enumerable Type
- [ ] Exposes `internal Source` property
- [ ] Exposes `internal Predicate` property (if applicable)
- [ ] Exposes `internal Selector` property (if applicable)
- [ ] Has corresponding `XxxEnumerableExtensions.cs` file
- [ ] Implements consecutive operation fusion (Where, Select, etc.)
- [ ] Implements terminal operation fusion (Min, Max, Sum, etc.)
- [ ] Implements conversion operations (ToArray, ToList, etc.)
- [ ] Added to enumerable types table in `fusion_matrix.md`
- [ ] Added to all operation tables in `fusion_matrix.md`

### New Operation
- [ ] Implemented for base types (Span, List, Array, Memory, etc.)
- [ ] Fusion added to `WhereListEnumerableExtensions.cs`
- [ ] Fusion added to `WhereReadOnlySpanEnumerableExtensions.cs`
- [ ] Fusion added to `WhereSelectListEnumerableExtensions.cs`
- [ ] Fusion added to `WhereSelectReadOnlySpanEnumerableExtensions.cs`
- [ ] Fusion added to `SelectListEnumerableExtensions.cs` (if applicable)
- [ ] Fusion added to `SelectReadOnlySpanEnumerableExtensions.cs` (if applicable)
- [ ] Test added/updated in `FusionCompletenessTests.cs`
- [ ] Updated appropriate table in `fusion_matrix.md`

### Verification
- [ ] `dotnet test --filter "ClassName=FusionCompletenessTests"` passes
- [ ] Manual review of `fusion_matrix.md` for completeness
- [ ] All unit tests pass
- [ ] Benchmarks show expected performance improvements (if applicable)

### Documentation
- [ ] XML documentation added to public APIs
- [ ] Examples added to README (if user-facing feature)
- [ ] Architecture docs updated (if architectural change)

---

## Fusion Completeness System

This project uses an automated fusion completeness verification system:

- **`FusionCompletenessTests.cs`**: Reflection-based tests that automatically verify all enumerable types have required fusion operations
- **`fusion_matrix.md`**: Manual documentation tracking all fusion operations
- **`fusion_maintenance_guide.md`**: Step-by-step guide for maintaining fusion completeness

Please review these documents before adding new enumerable types or operations.
