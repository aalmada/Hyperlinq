using System.Collections.Generic;
using System.Linq;

namespace TestData.NFHYPERLINQ001.Diagnostic.List;

class C
{
    void M()
    {
        var list = new List<int>();
        var x = list.Where(i => i > 0);
    }
}
