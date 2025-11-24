using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests.Span;

public class SpanCoreOperationsTests
{
    // ===== Count Tests =====
    
    [Test]
    public async Task Array_Count_ShouldWork()
    {
        var array = new int[] { 1, 2, 3, 4, 5 };
        await Assert.That(array.Count()).IsEqualTo(5);
    }
    
    [Test]
    public async Task List_Count_ShouldWork()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };
        await Assert.That(list.Count()).IsEqualTo(5);
    }
    
    [Test]
    public async Task Memory_Count_ShouldWork()
    {
        ReadOnlyMemory<int> memory = new int[] { 1, 2, 3 }.AsMemory();
        await Assert.That(memory.Count()).IsEqualTo(3);
    }
    
    // ===== Any Tests =====
    
    [Test]
    public async Task Array_Any_NonEmpty_ShouldReturnTrue()
    {
        var array = new int[] { 1 };
        await Assert.That(array.Any()).IsTrue();
    }
    
    [Test]
    public async Task Array_Any_Empty_ShouldReturnFalse()
    {
        var array = Array.Empty<int>();
        await Assert.That(array.Any()).IsFalse();
    }
    
    [Test]
    public async Task List_Any_ShouldWork()
    {
        var list = new List<int> { 1, 2, 3 };
        await Assert.That(list.Any()).IsTrue();
    }
    
    // ===== First Tests =====
    
    [Test]
    public async Task Array_First_ShouldReturnFirstElement()
    {
        var array = new int[] { 10, 20, 30 };
        await Assert.That(array.First()).IsEqualTo(10);
    }
    
    [Test]
    public async Task List_First_ShouldWork()
    {
        var list = new List<int> { 5, 10, 15 };
        await Assert.That(list.First()).IsEqualTo(5);
    }
    
    [Test]
    public async Task Memory_First_ShouldWork()
    {
        ReadOnlyMemory<int> memory = new int[] { 100, 200 }.AsMemory();
        await Assert.That(memory.First()).IsEqualTo(100);
    }
    
    [Test]
    public async Task Array_First_Empty_ShouldThrow()
    {
        var array = Array.Empty<int>();
        await Assert.That(() => array.First()).Throws<InvalidOperationException>();
    }
    
    // ===== Last Tests =====
    
    [Test]
    public async Task Array_Last_ShouldReturnLastElement()
    {
        var array = new int[] { 10, 20, 30 };
        await Assert.That(array.Last()).IsEqualTo(30);
    }
    
    [Test]
    public async Task List_Last_ShouldWork()
    {
        var list = new List<int> { 5, 10, 15 };
        await Assert.That(list.Last()).IsEqualTo(15);
    }
    
    [Test]
    public async Task Memory_Last_ShouldWork()
    {
        ReadOnlyMemory<int> memory = new int[] { 100, 200, 300 }.AsMemory();
        await Assert.That(memory.Last()).IsEqualTo(300);
    }
    
    [Test]
    public async Task Array_Last_Empty_ShouldThrow()
    {
        var array = Array.Empty<int>();
        await Assert.That(() => array.Last()).Throws<InvalidOperationException>();
    }
    
    // ===== Combined Operations =====
    
    [Test]
    public async Task Array_Where_Count_ShouldWork()
    {
        var array = new int[] { 1, 2, 3, 4, 5, 6 };
        var result = array.Where(x => x % 2 == 0).Count();
        await Assert.That(result).IsEqualTo(3);
    }
    
    [Test]
    public async Task List_Where_Any_ShouldWork()
    {
        var list = new List<int> { 1, 3, 5, 7 };
        var result = list.Where(x => x % 2 == 0).Any();
        await Assert.That(result).IsFalse();
    }
    
    [Test]
    public async Task Array_Where_First_ShouldWork()
    {
        var array = new int[] { 1, 2, 3, 4, 5 };
        var result = array.Where(x => x > 3).First();
        await Assert.That(result).IsEqualTo(4);
    }
    
    [Test]
    public async Task Array_Where_Last_ShouldWork()
    {
        var array = new int[] { 1, 2, 3, 4, 5 };
        var result = array.Where(x => x > 2).Last();
        await Assert.That(result).IsEqualTo(5);
    }
}
