using NetFabric.Hyperlinq;

namespace TestData.NFHYPERLINQ002.Diagnostic.AlreadyValueEnumerable;

class C
{
    void M()
    {
        var array = new int[0];
        var x = array.AsValueEnumerable();
    }
}
