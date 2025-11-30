# Contributing to NetFabric.Hyperlinq

Thank you for your interest in contributing to NetFabric.Hyperlinq! This document provides guidelines for contributing to the project.

## Quick Links

- **[Coding Guidelines](docs/contributing/coding-guidelines.md)** - Architecture, patterns, and coding standards
- **[First/Single Rules](docs/contributing/first-single-rules.md)** - Implementation rules for First/Single methods
- **[Optimization Guidelines](docs/contributing/optimization-guidelines.md)** - Performance optimization techniques
- **[Testing Guidelines](docs/contributing/testing-guidelines.md)** - Unit testing standards

## Getting Started

1. **Fork** the repository on GitHub
2. **Clone** your fork locally
3. **Create** a feature branch: `git checkout -b feature/my-feature`
4. **Make** your changes following the guidelines
5. **Test** your changes: `dotnet test`
6. **Commit** your changes with clear messages
7. **Push** to your fork: `git push origin feature/my-feature`
8. **Submit** a pull request

## Development Setup

### Prerequisites
- .NET 10 SDK or later
- C# 14 language features

### Building
```bash
dotnet build
```

### Running Tests
```bash
dotnet test
```

### Running Benchmarks
```bash
cd NetFabric.Hyperlinq.Benchmarks
dotnet run -c Release
```

## Contribution Guidelines

### Code Quality
- Follow the [Coding Guidelines](docs/contributing/coding-guidelines.md)
- Use C# 14 `extension` syntax for new extension methods
- Implement complete method families (see [First/Single Rules](docs/contributing/first-single-rules.md))
- Optimize for performance (see [Optimization Guidelines](docs/contributing/optimization-guidelines.md))

### Testing
- Write unit tests for all new functionality
- Follow the [Testing Guidelines](docs/contributing/testing-guidelines.md)
- Ensure all tests pass before submitting
- Add benchmarks for performance-critical code

### Pull Requests
- Keep PRs focused on a single feature or fix
- Write clear, descriptive commit messages
- Reference related issues in PR description
- Ensure CI builds pass
- Be responsive to code review feedback

## Code of Conduct

- Be respectful and inclusive
- Focus on constructive feedback
- Help others learn and grow
- Maintain a positive environment

## Questions?

- Open an issue for bugs or feature requests
- Start a discussion for questions or ideas
- Check existing documentation in the [docs/](docs/) directory

## License

By contributing, you agree that your contributions will be licensed under the MIT License.

---

Thank you for contributing to NetFabric.Hyperlinq! 🚀
