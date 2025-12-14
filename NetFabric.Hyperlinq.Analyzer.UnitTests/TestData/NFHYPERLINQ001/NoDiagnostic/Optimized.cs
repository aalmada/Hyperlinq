using System.Collections.Generic;
using System.Linq;
using NetFabric.Hyperlinq;

namespace TestData.NFHYPERLINQ001.NoDiagnostic.Optimized;

class C
{
    void M()
    {
        var list = new List<int>();
        var x = list.AsValueEnumerable().Where(i => i > 0);

        var array = new int[0];
        var y = array.AsValueEnumerable().Select(i => i * 2);
    }
}
