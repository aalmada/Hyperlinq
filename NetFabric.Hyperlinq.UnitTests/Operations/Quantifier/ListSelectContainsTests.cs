using System;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Assertive;
using NetFabric.Hyperlinq.UnitTests.TestData;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests.Operations.Quantifier;

public class ListSelectContainsTests
{
    [Test]
    public void List_Select_Contains_WithMatchingElement_ShouldReturnTrue()
    {
        var list = CommonTestData.SmallList;
        
        var result = list
            .Select(x => x * 2)
            .Contains(6); // 3 * 2 = 6
        
        _ = result.Must().BeTrue();
    }

    [Test]
    public void List_Select_Contains_WithNoMatch_ShouldReturnFalse()
    {
        var list = CommonTestData.SmallList;
        
        var result = list
            .Select(x => x * 2)
            .Contains(11);
        
        _ = result.Must().BeFalse();
    }

    [Test]
    public void List_Select_Contains_EmptyList_ShouldReturnFalse()
    {
        var list = CommonTestData.EmptyList;
        
        var result = list
            .Select(x => x * 2)
            .Contains(6);
        
        _ = result.Must().BeFalse();
    }

    [Test]
    public void List_Select_Contains_FirstElement_ShouldReturnTrue()
    {
        var list = CommonTestData.SmallList;
        
        var result = list
            .Select(x => x * 2)
            .Contains(2); // 1 * 2 = 2
        
        _ = result.Must().BeTrue();
    }

    [Test]
    public void List_Select_Contains_LastElement_ShouldReturnTrue()
    {
        var list = CommonTestData.SmallList;
        
        var result = list
            .Select(x => x * 2)
            .Contains(10); // 5 * 2 = 10
        
        _ = result.Must().BeTrue();
    }

    [Test]
    public void List_Select_Contains_LargeList_ShouldWork()
    {
        var list = CommonTestData.LargeList;
        
        var result = list
            .Select(x => x * 2)
            .Contains(1000); // 500 * 2 = 1000
        
        _ = result.Must().BeTrue();
    }

    [Test]
    public void List_Select_Contains_MultipleOccurrences_ShouldReturnTrue()
    {
        var list = CommonTestData.ListWithDuplicates;
        
        var result = list
            .Select(x => x * 2)
            .Contains(4); // 2 * 2 = 4 (appears 3 times)
        
        _ = result.Must().BeTrue();
    }
}
