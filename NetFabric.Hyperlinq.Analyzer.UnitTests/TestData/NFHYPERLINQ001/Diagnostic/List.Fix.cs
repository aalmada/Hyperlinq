using System.Collections.Generic;
using System.Linq;
using NetFabric.Hyperlinq;

namespace TestData.NFHYPERLINQ001.Diagnostic.List;

class C
{
    void M()
    {
        var list = new List<int>();
        var x = list.AsValueEnumerable().Where(i => i > 0);
    }
}
