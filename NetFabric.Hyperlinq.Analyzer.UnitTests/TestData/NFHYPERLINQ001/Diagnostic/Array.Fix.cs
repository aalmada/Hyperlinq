using System.Linq;
using NetFabric.Hyperlinq;

namespace TestData.NFHYPERLINQ001.Diagnostic.Array;

class C
{
    void M()
    {
        var array = new int[0];
        var x = array.AsValueEnumerable().Select(i => i * 2);
    }
}
