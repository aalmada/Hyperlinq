using System.Linq;
using NetFabric.Hyperlinq;

namespace TestData.NFHYPERLINQ005.Diagnostic.Range;

class C
{
    void M()
    {
        var x = ValueEnumerable.Range(0, 10);
    }
}
