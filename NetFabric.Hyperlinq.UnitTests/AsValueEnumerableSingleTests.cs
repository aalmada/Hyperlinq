using System;
using System.Collections.Generic;
using System.Linq;
using TUnit.Core;
using NetFabric.Assertive;

namespace NetFabric.Hyperlinq.UnitTests;

public class AsValueEnumerableSingleTests
{
    [Test]
    public void List_AsValueEnumerable_Single_ShouldMatchLinq()
    {
        var list = new List<int> { 42 };
        var valueEnum = list.AsValueEnumerable();
        
        valueEnum.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(list)
            .EvaluateTrue(e => e.Single() == list.Single());
    }
}
