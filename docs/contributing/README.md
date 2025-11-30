# Contributing to NetFabric.Hyperlinq

Thank you for your interest in contributing! This section contains all the guidelines and standards for contributors.

## Development Guidelines

### [Coding Guidelines](coding-guidelines.md)
Architecture, patterns, and coding standards for the project.

**Key topics:**
- C# 14 `extension` syntax
- Value-type enumerables
- Ref struct patterns
- Performance considerations
- Code organization

### [First/Single Rules](first-single-rules.md)
Mandatory implementation rules for `First*()` and `Single*()` methods.

**Key rules:**
- Complete method families (8 methods total)
- `*OrNone()` as single source of truth
- Wrapper delegation pattern

### [Optimization Guidelines](optimization-guidelines.md)
Performance optimization techniques and low-level optimizations.

**Key topics:**
- SIMD vectorization
- Bounds check elimination
- Inlining strategies
- Memory layout optimization
- Benchmark-driven optimization

### [Testing Guidelines](testing-guidelines.md)
Unit testing standards and practices.

**Key topics:**
- Test organization
- Property-based testing
- Performance testing
- Test naming conventions

---

## Contribution Workflow

1. **Fork** the repository
2. **Create** a feature branch
3. **Follow** the coding guidelines
4. **Write** tests for your changes
5. **Run** benchmarks if performance-related
6. **Submit** a pull request

## Before Submitting

- [ ] Code follows [coding guidelines](coding-guidelines.md)
- [ ] All tests pass
- [ ] New code has test coverage
- [ ] Performance-sensitive code has benchmarks
- [ ] Documentation is updated

---

[← Back to Documentation Index](../README.md)
