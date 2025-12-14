using NetFabric.Assertive;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests.Operations.Partitioning;

public class SkipTakeFusionTests
{
    [Test]
    public void SkipTake_ShouldWork()
    {
        var array = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        var result = array.AsValueEnumerable().Skip(2).Take(5);

        _ = result.ToArray().Must().BeEqualTo(new[] { 3, 4, 5, 6, 7 });
    }

    [Test]
    public void SkipTake_ZeroSkip_ShouldWork()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var result = array.AsValueEnumerable().Skip(0).Take(3);

        _ = result.ToArray().Must().BeEqualTo(new[] { 1, 2, 3 });
    }

    [Test]
    public void SkipTake_ZeroTake_ShouldReturnEmpty()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var result = array.AsValueEnumerable().Skip(2).Take(0);

        _ = result.ToArray().Must().BeEqualTo(Array.Empty<int>());
    }

    [Test]
    public void SkipTake_SkipAll_ShouldReturnEmpty()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var result = array.AsValueEnumerable().Skip(10).Take(3);

        _ = result.ToArray().Must().BeEqualTo(Array.Empty<int>());
    }

    [Test]
    public void SkipTake_TakeMoreThanAvailable_ShouldReturnRemaining()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var result = array.AsValueEnumerable().Skip(2).Take(10);

        _ = result.ToArray().Must().BeEqualTo(new[] { 3, 4, 5 });
    }

    [Test]
    public void SkipTake_NegativeSkip_ShouldTreatAsZero()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var result = array.AsValueEnumerable().Skip(-5).Take(3);

        _ = result.ToArray().Must().BeEqualTo(new[] { 1, 2, 3 });
    }

    [Test]
    public void SkipTake_NegativeTake_ShouldReturnEmpty()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var result = array.AsValueEnumerable().Skip(2).Take(-5);

        _ = result.ToArray().Must().BeEqualTo(Array.Empty<int>());
    }

    [Test]
    public void SkipTake_Pagination_ShouldWork()
    {
        // Common pagination scenario
        var array = Enumerable.Range(1, 100).ToArray();

        // Page 1: Skip 0, Take 10
        var page1 = array.AsValueEnumerable().Skip(0).Take(10);
        _ = page1.ToArray().Must().BeEqualTo(Enumerable.Range(1, 10).ToArray());

        // Page 2: Skip 10, Take 10
        var page2 = array.AsValueEnumerable().Skip(10).Take(10);
        _ = page2.ToArray().Must().BeEqualTo(Enumerable.Range(11, 10).ToArray());

        // Page 3: Skip 20, Take 10
        var page3 = array.AsValueEnumerable().Skip(20).Take(10);
        _ = page3.ToArray().Must().BeEqualTo(Enumerable.Range(21, 10).ToArray());
    }

    [Test]
    public void TakeSkip_ShouldWork()
    {
        // Less common but supported: Take().Skip()
        var array = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        var result = array.AsValueEnumerable().Take(8).Skip(2);

        // Take first 8 [1-8], then skip 2 [3-8]
        _ = result.ToArray().Must().BeEqualTo(new[] { 3, 4, 5, 6, 7, 8 });
    }

    [Test]
    public void SkipTake_Chaining_ShouldWork()
    {
        var array = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        // Chain multiple Skip/Take operations
        var result = array.AsValueEnumerable().Skip(1).Take(8).Skip(1).Take(5);

        // Should be: Skip 1 [2-10], Take 8 [2-9], Skip 1 [3-9], Take 5 [3-7]
        _ = result.ToArray().Must().BeEqualTo(new[] { 3, 4, 5, 6, 7 });
    }
}
