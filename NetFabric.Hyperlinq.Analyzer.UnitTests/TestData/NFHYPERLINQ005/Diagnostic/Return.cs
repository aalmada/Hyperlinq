namespace System.Linq
{
    public static class EnumerableEx
    {
        public static System.Collections.Generic.IEnumerable<TResult> Return<TResult>(TResult value)
            => throw new NotImplementedException();
    }
}

class C
{
    void M()
    {
        var x = System.Linq.EnumerableEx.Return(10);
    }
}
