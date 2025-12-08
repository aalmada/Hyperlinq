using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            Assert.That(sourceProperty, Is.Not.Null, $"{type.Name} is missing Source property");
            
            // Check for Predicate property
            var predicateProperty = type.GetProperty("Predicate", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            Assert.That(predicateProperty, Is.Not.Null, $"{type.Name} is missing Predicate property");
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
            Assert.That(sourceProperty, Is.Not.Null, $"{type.Name} is missing Source property");
            
            // Check for Selector property
            var selectorProperty = type.GetProperty("Selector", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            Assert.That(selectorProperty, Is.Not.Null, $"{type.Name} is missing Selector property");
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
            Assert.That(sourceProperty, Is.Not.Null, $"{type.Name} is missing Source property");
            
            // Check for Predicate property
            var predicateProperty = type.GetProperty("Predicate", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            Assert.That(predicateProperty, Is.Not.Null, $"{type.Name} is missing Predicate property");
            
            // Check for Selector property
            var selectorProperty = type.GetProperty("Selector", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            Assert.That(selectorProperty, Is.Not.Null, $"{type.Name} is missing Selector property");
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
            
            Assert.That(extensionsType, Is.Not.Null, 
                $"{type.Name} is missing corresponding {type.Name}Extensions class");
        }
    }

    [Test]
    public void AllWhereEnumerables_ShouldHaveWhereWhereF fusion()
    {
        var whereTypes = GetEnumerableTypes("Where", excludeWhereSelect: true);
        
        foreach (var type in whereTypes)
        {
            var extensionsTypeName = $"{type.FullName}Extensions";
            var extensionsType = HyperlinqAssembly.GetType(extensionsTypeName);
            
            if (extensionsType == null) continue; // Skip if extensions class doesn't exist
            
            // Look for Where(this WhereXxxEnumerable source, Func predicate) method
            var whereMethods = extensionsType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(m => m.Name == "Where" && m.GetParameters().Length == 2)
                .ToList();
            
            Assert.That(whereMethods, Is.Not.Empty, 
                $"{type.Name} is missing Where().Where() fusion method in {extensionsType.Name}");
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
            
            if (extensionsType == null) continue; // Skip if extensions class doesn't exist
            
            // Look for Select(this SelectXxxEnumerable source, Func selector) method
            var selectMethods = extensionsType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(m => m.Name == "Select" && m.GetParameters().Length == 2)
                .ToList();
            
            Assert.That(selectMethods, Is.Not.Empty, 
                $"{type.Name} is missing Select().Select() fusion method in {extensionsType.Name}");
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
            
            if (extensionsType == null) continue;
            
            // Look for Min() method
            var minMethods = extensionsType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(m => m.Name == "Min" && m.GetParameters().Length == 1)
                .ToList();
            
            Assert.That(minMethods, Is.Not.Empty, 
                $"{type.Name} is missing Min() fusion method in {extensionsType.Name}");
            
            // Look for Max() method
            var maxMethods = extensionsType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(m => m.Name == "Max" && m.GetParameters().Length == 1)
                .ToList();
            
            Assert.That(maxMethods, Is.Not.Empty, 
                $"{type.Name} is missing Max() fusion method in {extensionsType.Name}");
        }
    }

    private static List<Type> GetEnumerableTypes(string namePattern, bool excludeWhereSelect = false)
    {
        return HyperlinqAssembly.GetTypes()
            .Where(t => t.Name.Contains(namePattern) && 
                       t.Name.EndsWith("Enumerable") && 
                       !t.IsAbstract &&
                       !t.IsInterface &&
                       (!excludeWhereSelect || !t.Name.Contains("WhereSelect")))
            .ToList();
    }
}
