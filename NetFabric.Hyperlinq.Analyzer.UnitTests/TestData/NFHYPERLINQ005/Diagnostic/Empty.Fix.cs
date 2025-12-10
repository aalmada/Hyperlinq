using System.Linq;
using NetFabric.Hyperlinq;

namespace TestData.NFHYPERLINQ005.Diagnostic.Empty;

class C
{
    void M()
    {
        var x = ValueEnumerable.Empty<int>();
    }
}
