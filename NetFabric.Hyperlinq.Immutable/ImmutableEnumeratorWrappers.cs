using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    // Wrappers to satisfy TEnumerator : IEnumerator<T> constraint
    
    public struct ImmutableArrayEnumeratorWrapper<T> : IEnumerator<T>
    {
        private ImmutableArray<T>.Enumerator inner;
        
        public ImmutableArrayEnumeratorWrapper(ImmutableArray<T>.Enumerator inner) 
            => this.inner = inner;

        public T Current 
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => inner.Current; 
        }

        object IEnumerator.Current => inner.Current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext() => inner.MoveNext();

        public void Reset() => throw new NotSupportedException();
        public void Dispose() { } // ImmutableArray enumerator strictly doesn't need disposal usually, but interface requires method.
    }

    public struct ImmutableListEnumeratorWrapper<T> : IEnumerator<T>
    {
        private ImmutableList<T>.Enumerator inner;

        public ImmutableListEnumeratorWrapper(ImmutableList<T>.Enumerator inner) 
            => this.inner = inner;

        public T Current 
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => inner.Current; 
        }

        object IEnumerator.Current => inner.Current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext() => inner.MoveNext();

        public void Reset() => inner.Reset();
        public void Dispose() => inner.Dispose();
    }

    public struct ImmutableQueueEnumeratorWrapper<T> : IEnumerator<T>
    {
        private ImmutableQueue<T>.Enumerator inner;

        public ImmutableQueueEnumeratorWrapper(ImmutableQueue<T>.Enumerator inner) 
            => this.inner = inner;

        public T Current 
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => inner.Current; 
        }

        object IEnumerator.Current => inner.Current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext() => inner.MoveNext();

        public void Reset() => throw new NotSupportedException();
        public void Dispose() { if (inner is IDisposable d) d.Dispose(); }
    }

    public struct ImmutableStackEnumeratorWrapper<T> : IEnumerator<T>
    {
        private ImmutableStack<T>.Enumerator inner;

        public ImmutableStackEnumeratorWrapper(ImmutableStack<T>.Enumerator inner) 
            => this.inner = inner;

        public T Current 
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => inner.Current; 
        }

        object IEnumerator.Current => inner.Current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext() => inner.MoveNext();

        public void Reset() => throw new NotSupportedException();
        public void Dispose() { if (inner is IDisposable d) d.Dispose(); }
    }

    public struct ImmutableHashSetEnumeratorWrapper<T> : IEnumerator<T>
    {
        private ImmutableHashSet<T>.Enumerator inner;

        public ImmutableHashSetEnumeratorWrapper(ImmutableHashSet<T>.Enumerator inner) 
            => this.inner = inner;

        public T Current 
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => inner.Current; 
        }

        object IEnumerator.Current => inner.Current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext() => inner.MoveNext();

        public void Reset() => inner.Reset();
        public void Dispose() => inner.Dispose();
    }


    public struct ImmutableSortedSetEnumeratorWrapper<T> : IEnumerator<T>
    {
        private ImmutableSortedSet<T>.Enumerator inner;

        public ImmutableSortedSetEnumeratorWrapper(ImmutableSortedSet<T>.Enumerator inner) 
            => this.inner = inner;

        public T Current 
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => inner.Current; 
        }

        object IEnumerator.Current => inner.Current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext() => inner.MoveNext();

        public void Reset() => inner.Reset();
        public void Dispose() => inner.Dispose();
    }

    public struct ImmutableDictionaryEnumeratorWrapper<TKey, TValue> : IEnumerator<KeyValuePair<TKey, TValue>>
        where TKey : notnull
    {
        private ImmutableDictionary<TKey, TValue>.Enumerator inner;

        public ImmutableDictionaryEnumeratorWrapper(ImmutableDictionary<TKey, TValue>.Enumerator inner) 
            => this.inner = inner;

        public KeyValuePair<TKey, TValue> Current 
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => inner.Current; 
        }

        object IEnumerator.Current => inner.Current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext() => inner.MoveNext();

        public void Reset() => inner.Reset();
        public void Dispose() => inner.Dispose();
    }

    public struct ImmutableSortedDictionaryEnumeratorWrapper<TKey, TValue> : IEnumerator<KeyValuePair<TKey, TValue>>
        where TKey : notnull
    {
        private ImmutableSortedDictionary<TKey, TValue>.Enumerator inner;

        public ImmutableSortedDictionaryEnumeratorWrapper(ImmutableSortedDictionary<TKey, TValue>.Enumerator inner) 
            => this.inner = inner;

        public KeyValuePair<TKey, TValue> Current 
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => inner.Current; 
        }

        object IEnumerator.Current => inner.Current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext() => inner.MoveNext();

        public void Reset() => inner.Reset();
        public void Dispose() => inner.Dispose();
    }
}
