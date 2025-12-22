using System;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Assertive;
using NetFabric.Hyperlinq.UnitTests.TestData;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests.Operations.Quantifier;

public class ArraySelectContainsTests
{
    [Test]
    public void Array_Select_Contains_WithMatchingElement_ShouldReturnTrue()
    {
        var array = CommonTestData.SmallArray;
        
        var result = array.AsValueEnumerable()
            .Select(x => x * 2)
            .Contains(6); // 3 * 2 = 6
        
        _ = result.Must().BeTrue();
    }

    [Test]
    public void Array_Select_Contains_WithNoMatch_ShouldReturnFalse()
    {
        var array = CommonTestData.SmallArray;
        
        var result = array.AsValueEnumerable()
            .Select(x => x * 2)
            .Contains(11);
        
        _ = result.Must().BeFalse();
    }

    [Test]
    public void Array_Select_Contains_EmptyArray_ShouldReturnFalse()
    {
        var array = CommonTestData.EmptyArray;
        
        var result = array.AsValueEnumerable()
            .Select(x => x * 2)
            .Contains(6);
        
        _ = result.Must().BeFalse();
    }

    [Test]
    public void Array_Select_Contains_FirstElement_ShouldReturnTrue()
    {
        var array = CommonTestData.SmallArray;
        
        var result = array.AsValueEnumerable()
            .Select(x => x * 2)
            .Contains(2); // 1 * 2 = 2
        
        _ = result.Must().BeTrue();
    }

    [Test]
    public void Array_Select_Contains_LastElement_ShouldReturnTrue()
    {
        var array = CommonTestData.SmallArray;
        
        var result = array.AsValueEnumerable()
            .Select(x => x * 2)
            .Contains(10); // 5 * 2 = 10
        
        _ = result.Must().BeTrue();
    }

    [Test]
    public void Array_Select_Contains_LargeArray_ShouldWork()
    {
        var array = CommonTestData.LargeArray;
        
        var result = array.AsValueEnumerable()
            .Select(x => x * 2)
            .Contains(1000); // 500 * 2 = 1000
        
        _ = result.Must().BeTrue();
    }
}
