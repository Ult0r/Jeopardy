using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Jeopardy.Validation;

public static class NullAssertion
{
    public static void AssertNotNull<T>([NotNull] this T? value, params string[] propertyName)
    {
        if (value is null)
        {
            throw new NullAssertionException($"Value should not be null: {propertyName.ToList().Aggregate((a, b) => a + "." + b)}");
        }
    }
}

public class NullAssertionException : Exception
{
    public NullAssertionException(string msg) : base(msg) { }
}