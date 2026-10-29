namespace ArturRios.Extensions;

/// <summary>
///     Provides comparison helpers and syntactic sugar for checking membership (In/NotIn).
/// </summary>
public static class ComparisonExtensions
{
    /// <summary>
    ///     Returns true if the value is equal to any element in the provided range.
    /// </summary>
    /// <typeparam name="T">Type of the value and of the range elements.</typeparam>
    /// <param name="self">The value to look for.</param>
    /// <param name="range">Values to compare against. An empty range never matches.</param>
    /// <returns>True if a match is found; otherwise false.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="range"/> is <c>null</c>.</exception>
    /// <remarks>
    ///     <para>
    ///     Equality is decided by <see cref="EqualityComparer{T}.Default"/>, so a type that overrides
    ///     <see cref="object.Equals(object)"/> or implements <see cref="IEquatable{T}"/> is honoured, and two
    ///     nulls compare equal without either side having to be dereferenced.
    ///     </para>
    ///     <para>
    ///     Declared as a classic extension method rather than in an <c>extension</c> block: the compiler
    ///     reports a spurious nullability warning (CS8620) for every argument of an expanded <c>params</c>
    ///     call to an extension-block member, so <c>2.In(1, 2, 3)</c> produced three warnings in every
    ///     consumer with nullable reference types enabled.
    ///     </para>
    /// </remarks>
    public static bool In<T>(this T self, params T[] range)
    {
        ArgumentNullException.ThrowIfNull(range);

        return Array.Exists(range, other => EqualityComparer<T>.Default.Equals(self, other));
    }

    /// <summary>
    ///     Returns true if the value is not equal to any element in the provided range.
    /// </summary>
    /// <typeparam name="T">Type of the value and of the range elements.</typeparam>
    /// <param name="self">The value to look for.</param>
    /// <param name="range">Values to compare against. An empty range never matches, so the result is true.</param>
    /// <returns>True if no match is found; otherwise false.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="range"/> is <c>null</c>.</exception>
    public static bool NotIn<T>(this T self, params T[] range) => !self.In(range);
}
