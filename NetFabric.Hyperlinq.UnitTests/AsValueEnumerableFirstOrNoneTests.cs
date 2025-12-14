using System;
using System.Collections.Generic;
using NetFabric.Assertive;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests;

public class AsValueEnumerableFirstOrNoneTests
{
    [Test]
    public void List_AsValueEnumerable_FirstOrNone_NonEmpty_ShouldReturnSome()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };
        var option = list.AsValueEnumerable().FirstOrNone();

        _ = option.HasValue.Must().BeTrue();
        _ = option.Value.Must().BeEqualTo(1);
    }

    [Test]
    public void List_AsValueEnumerable_FirstOrNone_Empty_ShouldReturnNone()
    {
        var list = new List<int>();
        var option = list.AsValueEnumerable().FirstOrNone();

        _ = option.HasValue.Must().BeFalse();
    }

    [Test]
    public void IList_AsValueEnumerable_FirstOrNone_ShouldUseOptimization()
    {
        IList<int> list = new List<int> { 10, 20, 30 };
        var option = list.AsValueEnumerable().FirstOrNone();

        _ = option.HasValue.Must().BeTrue();
        _ = option.Value.Must().BeEqualTo(10);
    }

    [Test]
    public void IEnumerable_AsValueEnumerable_FirstOrNone_NonEmpty_ShouldReturnSome()
    {
        IEnumerable<int> enumerable = System.Linq.Enumerable.Range(1, 5);
        var option = enumerable.AsValueEnumerable().FirstOrNone();

        _ = option.HasValue.Must().BeTrue();
        _ = option.Value.Must().BeEqualTo(1);
    }

    [Test]
    public void IEnumerable_AsValueEnumerable_FirstOrNone_Empty_ShouldReturnNone()
    {
        IEnumerable<int> enumerable = System.Linq.Enumerable.Empty<int>();
        var option = enumerable.AsValueEnumerable().FirstOrNone();

        _ = option.HasValue.Must().BeFalse();
    }
}
