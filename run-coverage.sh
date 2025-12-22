#!/bin/bash
# Script to run tests with code coverage and generate HTML report

set -e

echo "🧪 Running tests with code coverage..."

# Clean previous coverage results
rm -rf CoverageReport

# Change to test project directory and run tests with coverage using TUnit's built-in coverage
cd NetFabric.Hyperlinq.UnitTests
dotnet test \
  --coverage \
  --coverage-output-format cobertura
cd ..

# Find the coverage file (TUnit creates it in bin/Debug/net10.0/TestResults/)
COVERAGE_FILE=$(find ./NetFabric.Hyperlinq.UnitTests/bin -name "*.cobertura.xml" | head -n 1)

if [ -z "$COVERAGE_FILE" ]; then
    echo "❌ Coverage file not found!"
    exit 1
fi

echo "📊 Generating HTML coverage report..."
echo "Using coverage file: $COVERAGE_FILE"

# Generate HTML report using reportgenerator global tool
reportgenerator \
  -reports:"$COVERAGE_FILE" \
  -targetdir:"CoverageReport" \
  -reporttypes:"Html;TextSummary"

# Display summary
echo ""
echo "✅ Coverage report generated!"
echo ""
cat CoverageReport/Summary.txt
echo ""
echo "📂 Open the report: CoverageReport/index.html"
