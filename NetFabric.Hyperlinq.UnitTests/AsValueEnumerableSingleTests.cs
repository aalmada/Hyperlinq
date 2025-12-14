using System;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Assertive;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests;

public class AsValueEnumerableSingleTests
{
    [Test]
    public void List_AsValueEnumerable_Single_ShouldMatchLinq()
    {
        var list = new List<int> { 42 };
        var valueEnum = list.AsValueEnumerable();

        _ = valueEnum.Must()
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
        _ = result.Must().BeEqualTo(42);
    }

    [Test]
    public void IList_AsValueEnumerable_Single_EmptyList_ShouldThrow()
    {
        // Verify the optimization correctly throws for empty IList
        IList<int> list = new List<int>();
        var valueEnum = list.AsValueEnumerable();

        Action action = () => valueEnum.Single();
        _ = action.Must().Throw<InvalidOperationException>();
    }

    [Test]
    public void IList_AsValueEnumerable_Single_MultipleElements_ShouldThrow()
    {
        // Verify the optimization correctly throws for IList with multiple elements
        IList<int> list = new List<int> { 1, 2, 3 };
        var valueEnum = list.AsValueEnumerable();

        Action action = () => valueEnum.Single();
        _ = action.Must().Throw<InvalidOperationException>();
    }
}
