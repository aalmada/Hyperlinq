using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetFabric.Assertive;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests;

/// <summary>
/// Reflection-based tests to verify all enumerable types have required fusion operations.
/// This helps catch missing fusion implementations at test time.
/// </summary>
public class FusionCompletenessTests
{
    private static readonly Assembly HyperlinqAssembly = typeof(ReadOnlySpanExtensions).Assembly;

    [Test]
    public void AllWhereEnumerables_ShouldExposeSourceAndPredicateProperties()
    {
        var whereTypes = GetEnumerableTypes("Where", excludeWhereSelect: true);

        foreach (var type in whereTypes)
        {
            // Check for Source property
            var sourceProperty = type.GetProperty("Source", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            _ = sourceProperty.Must().EvaluateTrue(p => p is not null);

            // Check for Predicate property
            var predicateProperty = type.GetProperty("Predicate", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            _ = predicateProperty.Must().EvaluateTrue(p => p is not null);
        }
    }

    [Test]
    public void AllSelectEnumerables_ShouldExposeSourceAndSelectorProperties()
    {
        var selectTypes = GetEnumerableTypes("Select", excludeWhereSelect: true);

        foreach (var type in selectTypes)
        {
            // Check for Source property
            var sourceProperty = type.GetProperty("Source", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            _ = sourceProperty.Must().EvaluateTrue(p => p is not null);

            // Check for Selector property
            var selectorProperty = type.GetProperty("Selector", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            _ = selectorProperty.Must().EvaluateTrue(p => p is not null);
        }
    }

    [Test]
    public void AllWhereSelectEnumerables_ShouldExposeAllProperties()
    {
        var whereSelectTypes = GetEnumerableTypes("WhereSelect");

        foreach (var type in whereSelectTypes)
        {
            // Check for Source property
            var sourceProperty = type.GetProperty("Source", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            _ = sourceProperty.Must().EvaluateTrue(p => p is not null);

            // Check for Predicate property
            var predicateProperty = type.GetProperty("Predicate", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            _ = predicateProperty.Must().EvaluateTrue(p => p is not null);

            // Check for Selector property
            var selectorProperty = type.GetProperty("Selector", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            _ = selectorProperty.Must().EvaluateTrue(p => p is not null);
        }
    }

    [Test]
    public void AllWhereEnumerables_ShouldHaveExtensionsClass()
    {
        var whereTypes = GetEnumerableTypes("Where", excludeWhereSelect: true);

        foreach (var type in whereTypes)
        {
            var extensionsTypeName = $"{type.FullName}Extensions";
            var extensionsType = HyperlinqAssembly.GetType(extensionsTypeName);

            _ = extensionsType.Must().EvaluateTrue(t => t is not null);
        }
    }

    [Test]
    public void AllWhereEnumerables_ShouldHaveWhereWhereFusion()
    {
        var whereTypes = GetEnumerableTypes("Where", excludeWhereSelect: true);

        foreach (var type in whereTypes)
        {
            var extensionsTypeName = $"{type.FullName}Extensions";
            var extensionsType = HyperlinqAssembly.GetType(extensionsTypeName);

            if (extensionsType == null)
            {
                continue; // Skip if extensions class doesn't exist
            }

            // Look for Where(this WhereXxxEnumerable source, Func predicate) method
            var whereMethods = extensionsType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(m => m.Name == "Where" && m.GetParameters().Length == 2)
                .ToList();

            _ = whereMethods.Must().EvaluateTrue(l => l.Count > 0);
        }
    }

    [Test]
    public void AllSelectEnumerables_ShouldHaveSelectSelectFusion()
    {
        var selectTypes = GetEnumerableTypes("Select", excludeWhereSelect: true);

        foreach (var type in selectTypes)
        {
            var extensionsTypeName = $"{type.FullName}Extensions";
            var extensionsType = HyperlinqAssembly.GetType(extensionsTypeName);

            if (extensionsType == null)
            {
                continue; // Skip if extensions class doesn't exist
            }

            // Look for Select(this SelectXxxEnumerable source, Func selector) method
            var selectMethods = extensionsType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(m => m.Name == "Select" && m.GetParameters().Length == 2)
                .ToList();

            _ = selectMethods.Must().EvaluateTrue(l => l.Count > 0);
        }
    }

    [Test]
    public void AllWhereEnumerables_ShouldHaveMinMaxFusion()
    {
        var whereTypes = GetEnumerableTypes("Where");

        foreach (var type in whereTypes)
        {
            var extensionsTypeName = $"{type.FullName}Extensions";
            var extensionsType = HyperlinqAssembly.GetType(extensionsTypeName);

            if (extensionsType == null)
            {
                continue;
            }

            // Look for Min() method
            var minMethods = extensionsType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(m => m.Name == "Min" && m.GetParameters().Length == 1)
                .ToList();

            _ = minMethods.Must().EvaluateTrue(l => l.Count > 0);

            // Look for Max() method
            var maxMethods = extensionsType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(m => m.Name == "Max" && m.GetParameters().Length == 1)
                .ToList();

            _ = maxMethods.Must().EvaluateTrue(l => l.Count > 0);
        }
    }

    private static List<Type> GetEnumerableTypes(string namePattern, bool excludeWhereSelect = false) => HyperlinqAssembly.GetTypes()
            .Where(t => t.Name.Contains(namePattern) &&
                       t.Name.EndsWith("Enumerable") &&
                       !t.IsAbstract &&
                       !t.IsInterface &&
                       (!excludeWhereSelect || !t.Name.Contains("WhereSelect")))
            .ToList();
}
