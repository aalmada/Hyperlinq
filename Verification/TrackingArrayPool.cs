using System;
using System.Buffers;
using System.Collections.Generic;

namespace Verification
{
    public class TrackingArrayPool<T> : ArrayPool<T>, IDisposable
    {
        public List<(int minimumLength, T[] array)> Rents { get; } = new();
        public List<(T[] array, bool clearArray)> Returns { get; } = new();

        public int RentCalls => Rents.Count;
        public int ReturnCalls => Returns.Count;

        public override T[] Rent(int minimumLength)
        {
            var array = new T[minimumLength];
            Rents.Add((minimumLength, array));
            return array;
        }

        public override void Return(T[] array, bool clearArray = false)
        {
            Returns.Add((array, clearArray));
        }

        public void Dispose() { }
    }
}
