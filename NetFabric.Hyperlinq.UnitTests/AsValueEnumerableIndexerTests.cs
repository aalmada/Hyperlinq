using System;
using System.Collections.Generic;
using System.Linq;
using TUnit.Core;
using NetFabric.Assertive;

namespace NetFabric.Hyperlinq.UnitTests;

public class AsValueEnumerableIndexerTests
{
    [Test]
    public void List_AsValueEnumerable_Indexer_ShouldWork()
    {
        var list = new List<int> { 10, 20, 30, 40, 50 };
        var valueEnum = list.AsValueEnumerable();
        
        // Test indexer access
        valueEnum[0].Must().BeEqualTo(10);
        valueEnum[2].Must().BeEqualTo(30);
        valueEnum[4].Must().BeEqualTo(50);
    }
    
    [Test]
    public void Array_AsValueEnumerable_Indexer_ShouldWork()
    {
        var array = new int[] { 10, 20, 30, 40, 50 };
        var valueEnum = array.AsValueEnumerable();
        
        valueEnum[0].Must().BeEqualTo(10);
        valueEnum[2].Must().BeEqualTo(30);
        valueEnum[4].Must().BeEqualTo(50);
    }
}
