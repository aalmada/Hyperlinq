using System;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Assertive;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests.Operations.Conversion;

public class SelectCopyToTests
{
    [Test]
    public void Array_Select_CopyTo_ShouldCopyTransformedElements()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var destination = new int[5];
        
        var enumerable = array.AsValueEnumerable().Select(x => x * 2);
        enumerable.CopyTo(destination, 0);
        
        _ = destination.Must().BeEnumerableOf<int>().BeEqualTo(new[] { 2, 4, 6, 8, 10 });
    }



    [Test]
    public void List_Select_CopyTo_ShouldCopyTransformedElements()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };
        var destination = new int[5];
        
        var enumerable = list.Select(x => x * 2);
        enumerable.CopyTo(destination, 0);
        
        _ = destination.Must().BeEnumerableOf<int>().BeEqualTo(new[] { 2, 4, 6, 8, 10 });
    }





    [Test]
    public void Array_Select_CopyTo_LargeArray_ShouldWork()
    {
        var array = Enumerable.Range(1, 100).ToArray();
        var destination = new int[100];
        
        var enumerable = array.AsValueEnumerable().Select(x => x * 2);
        enumerable.CopyTo(destination, 0);
        
        var expected = Enumerable.Range(1, 100).Select(x => x * 2).ToArray();
        _ = destination.Must().BeEnumerableOf<int>().BeEqualTo(expected);
    }
}
