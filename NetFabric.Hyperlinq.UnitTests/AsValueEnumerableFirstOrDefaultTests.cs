using System;
using System.Collections.Generic;
using NetFabric.Assertive;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests;

public class AsValueEnumerableFirstOrDefaultTests
{
    [Test]
    public void List_AsValueEnumerable_FirstOrDefault_NonEmpty_ShouldReturnFirst()
    {
        var list = new List<int> { 1, 2, 3 };
        var result = list.AsValueEnumerable().FirstOrDefault();

        _ = result.Must().BeEqualTo(1);
    }

    [Test]
    public void List_AsValueEnumerable_FirstOrDefault_Empty_ShouldReturnDefault()
    {
        var list = new List<int>();
        var result = list.AsValueEnumerable().FirstOrDefault();

        _ = result.Must().BeEqualTo(0);
    }

    [Test]
    public void List_AsValueEnumerable_FirstOrDefault_Empty_WithDefault_ShouldReturnProvidedDefault()
    {
        var list = new List<int>();
        var result = list.AsValueEnumerable().FirstOrDefault(99);

        _ = result.Must().BeEqualTo(99);
    }

    [Test]
    public void IEnumerable_AsValueEnumerable_FirstOrDefault_NonEmpty_ShouldReturnFirst()
    {
        IEnumerable<int> enumerable = System.Linq.Enumerable.Range(1, 3);
        var result = enumerable.AsValueEnumerable().FirstOrDefault();

        _ = result.Must().BeEqualTo(1);
    }

    [Test]
    public void IEnumerable_AsValueEnumerable_FirstOrDefault_Empty_ShouldReturnDefault()
    {
        IEnumerable<int> enumerable = System.Linq.Enumerable.Empty<int>();
        var result = enumerable.AsValueEnumerable().FirstOrDefault();

        _ = result.Must().BeEqualTo(0);
    }
}
