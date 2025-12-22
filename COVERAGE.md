# Code Coverage

This project uses [Coverlet](https://github.com/coverlet-coverage/coverlet) for code coverage collection and [ReportGenerator](https://github.com/danielpalme/ReportGenerator) for HTML report generation.

## Quick Start

### Run Tests with Coverage

```bash
./run-coverage.sh
```

This will:
1. Run all unit tests with code coverage collection
2. Generate an HTML coverage report in `CoverageReport/`
3. Display a summary in the terminal
4. Output the path to the HTML report

### View Coverage Report

Open the generated HTML report:

```bash
open CoverageReport/index.html
```

## Manual Commands

### Collect Coverage

```bash
dotnet test NetFabric.Hyperlinq.UnitTests/NetFabric.Hyperlinq.UnitTests.csproj \
  --collect:"XPlat Code Coverage" \
  --results-directory:./TestResults \
  --settings:coverlet.runsettings
```

### Generate HTML Report

```bash
dotnet reportgenerator \
  -reports:"./TestResults/**/coverage.cobertura.xml" \
  -targetdir:"CoverageReport" \
  -reporttypes:"Html;TextSummary"
```

### Generate Different Report Formats

```bash
# Generate multiple formats
dotnet reportgenerator \
  -reports:"./TestResults/**/coverage.cobertura.xml" \
  -targetdir:"CoverageReport" \
  -reporttypes:"Html;Cobertura;TextSummary;Badges"
```

Available report types:
- `Html` - Interactive HTML report
- `HtmlSummary` - Summary HTML report
- `Cobertura` - Cobertura XML format (for CI/CD)
- `TextSummary` - Text summary in console
- `Badges` - SVG badges for README
- `lcov` - LCOV format (for SonarQube, etc.)

## Configuration

Coverage settings are configured in [`coverlet.runsettings`](./coverlet.runsettings):

- **Format**: Cobertura XML
- **Excludes**: Test assemblies (`*.Tests`, `*.UnitTests`)
- **Exclude by Attribute**: `Obsolete`, `GeneratedCode`, `CompilerGenerated`
- **Source Link**: Enabled for accurate source mapping

## CI/CD Integration

### GitHub Actions Example

```yaml
- name: Run Tests with Coverage
  run: dotnet test --collect:"XPlat Code Coverage" --settings:coverlet.runsettings

- name: Upload Coverage to Codecov
  uses: codecov/codecov-action@v3
  with:
    files: ./TestResults/**/coverage.cobertura.xml
```

## Coverage Metrics

The coverage report includes:
- **Line Coverage**: Percentage of code lines executed
- **Branch Coverage**: Percentage of conditional branches taken
- **Method Coverage**: Percentage of methods called

## Viewing Specific Coverage

To see coverage for specific files or methods:
1. Open `CoverageReport/index.html`
2. Navigate through the namespace/class hierarchy
3. Click on any file to see line-by-line coverage with color coding:
   - 🟢 Green: Covered lines
   - 🔴 Red: Uncovered lines
   - 🟡 Yellow: Partially covered branches

## Troubleshooting

### No coverage file found

If the script reports "Coverage file not found":
1. Ensure tests are running successfully
2. Check that `coverlet.collector` package is installed
3. Verify `coverlet.runsettings` exists

### Low coverage numbers

Check the exclusions in `coverlet.runsettings` - you may be excluding too much code.

### Report not generating

Ensure `ReportGenerator` is installed:
```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
```
