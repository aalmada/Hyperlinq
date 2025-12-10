using System.Collections.Generic;
using NetFabric.Hyperlinq;

namespace TestData.NFHYPERLINQ003.Diagnostic.SingleOperation;

class C
{
    void M()
    {
        var list = new List<int>();
        var x = list.AsValueEnumerable().Where(x => x > 0);
    }
}
