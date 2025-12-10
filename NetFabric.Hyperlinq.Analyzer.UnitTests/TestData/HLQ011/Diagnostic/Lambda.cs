using NetFabric.Hyperlinq;

namespace TestData.HLQ011.Diagnostic.Lambda;

class C
{
    void M()
    {
        var array = new int[0];
        var x = array.AsValueEnumerable().Where(x => x > 0);
    }
}
