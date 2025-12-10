using System.Linq;
using NetFabric.Hyperlinq;

namespace TestData.NFHYPERLINQ005.Diagnostic.Repeat;

class C
{
    void M()
    {
        var x = ValueEnumerable.Repeat(0, 10);
    }
}
