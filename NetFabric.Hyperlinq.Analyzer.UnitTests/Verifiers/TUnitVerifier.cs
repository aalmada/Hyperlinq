using Microsoft.CodeAnalysis.Testing;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace NetFabric.Hyperlinq.Analyzer.UnitTests.Verifiers
{
    public class TUnitVerifier : IVerifier
    {
        public void Empty<T>(string collectionName, IEnumerable<T> collection)
        {
             if (collection.Any())
                 throw new Exception($"Collection '{collectionName}' matches 'not empty' but expected 'empty'.");
        }

        public void Equal<T>(T expected, T actual, string? message = null)
        {
            if (!EqualityComparer<T>.Default.Equals(expected, actual))
                 throw new Exception($"Expected: {expected}\nActual: {actual}\nMessage: {message}");
        }

        [DoesNotReturn]
        public void Fail(string? message = null)
        {
             throw new Exception(message ?? "Test failed");
        }

        public void False(bool condition, string? message = null)
        {
             if (condition) throw new Exception(message ?? "Expected false");
        }

        public void LanguageIsSupported(string language)
        {
        }

        public void NotEmpty<T>(string collectionName, IEnumerable<T> collection)
        {
             if (!collection.Any())
                 throw new Exception($"Collection '{collectionName}' matches 'empty' but expected 'not empty'.");
        }

        public void True(bool condition, string? message = null)
        {
             if (!condition) throw new Exception(message ?? "Expected true");
        }

        public void SequenceEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, IEqualityComparer<T>? comparer = null, string? message = null)
        {
             comparer ??= EqualityComparer<T>.Default;
             if (!expected.SequenceEqual(actual, comparer))
                  throw new Exception(message ?? "Sequence mismatch");
        }

        public IVerifier PushContext(string context)
        {
             return this;
        }
    }
}
