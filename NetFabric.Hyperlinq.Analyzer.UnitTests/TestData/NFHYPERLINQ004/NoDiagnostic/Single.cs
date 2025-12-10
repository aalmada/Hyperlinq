using System.Collections.Generic;
using NetFabric.Hyperlinq;

namespace TestData.NFHYPERLINQ004.NoDiagnostic.Single;

class C
{
    void M()
    {
        var list = new List<int>();
        var x = list.Where(x => x > 0);
    }
}
