using System.Linq;

namespace TestData.NFHYPERLINQ005.Diagnostic.Repeat;

class C
{
    void M()
    {
        var x = Enumerable.Repeat(0, 10);
    }
}
