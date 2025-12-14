using System.Collections.Generic;
using System.Linq;
using NetFabric.Hyperlinq;

namespace TestData.NFHYPERLINQ002.NoDiagnostic.KeepAsValueEnumerable;

class C
{
    void M(IEnumerable<int> enumerable)
    {
        var x = enumerable.AsValueEnumerable().Count();

        var list = new List<int>();
        var y = list.AsValueEnumerable().Where(i => i > 0);
    }
}
