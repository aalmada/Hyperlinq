using System;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Assertive;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests;

public class AsValueEnumerableIndexerTests
{
    [Test]
    public void List_AsValueEnumerable_Indexer_ShouldWork()
    {
        var list = new List<int> { 10, 20, 30, 40, 50 };
        var valueEnum = list.AsValueEnumerable();

        // Test indexer access
        _ = valueEnum[0].Must().BeEqualTo(10);
        _ = valueEnum[2].Must().BeEqualTo(30);
        _ = valueEnum[4].Must().BeEqualTo(50);
    }

    [Test]
    public void Array_AsValueEnumerable_Indexer_ShouldWork()
    {
        var array = new int[] { 10, 20, 30, 40, 50 };
        var valueEnum = array.AsValueEnumerable();

        _ = valueEnum[0].Must().BeEqualTo(10);
        _ = valueEnum[2].Must().BeEqualTo(30);
        _ = valueEnum[4].Must().BeEqualTo(50);
    }
}
