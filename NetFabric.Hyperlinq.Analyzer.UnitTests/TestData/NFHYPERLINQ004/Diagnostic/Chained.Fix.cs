using System.Collections.Generic;
using NetFabric.Hyperlinq;

namespace TestData.NFHYPERLINQ004.Diagnostic.Chained;

class C
{
    void M()
    {
        var list = new List<int>();
        var x = list.AsValueEnumerable().Where(x => x > 0).Select(x => x * 2);
    }
}
