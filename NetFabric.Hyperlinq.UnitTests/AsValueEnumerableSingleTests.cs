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
    
    [Test]
    public void IList_AsValueEnumerable_Single_ShouldUseOptimization()
    {
        // Create an IList<int> to verify the optimization path is taken
        IList<int> list = new List<int> { 42 };
        var valueEnum = list.AsValueEnumerable();
        
        // The optimization should return the single element without enumeration
        var result = valueEnum.Single();
        result.Must().BeEqualTo(42);
    }
    
    [Test]
    public void IList_AsValueEnumerable_Single_EmptyList_ShouldThrow()
    {
        // Verify the optimization correctly throws for empty IList
        IList<int> list = new List<int>();
        var valueEnum = list.AsValueEnumerable();
        
        Action action = () => valueEnum.Single();
        action.Must().Throw<InvalidOperationException>();
    }
    
    [Test]
    public void IList_AsValueEnumerable_Single_MultipleElements_ShouldThrow()
    {
        // Verify the optimization correctly throws for IList with multiple elements
        IList<int> list = new List<int> { 1, 2, 3 };
        var valueEnum = list.AsValueEnumerable();
        
        Action action = () => valueEnum.Single();
        action.Must().Throw<InvalidOperationException>();
    }
}
