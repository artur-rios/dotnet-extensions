namespace ArturRios.Extensions;

/// <summary>
///     Provides comparison helpers and syntactic sugar for checking membership (In/NotIn).
/// </summary>
public static class ComparisonExtensions
{
    /// <summary>
    ///     Provides comparison helpers for the given value.
    /// </summary>
    extension<T>(T self)
    {
        /// <summary>
        ///     Returns true if the value is equal to any element in the provided range.
        /// </summary>
        /// <param name="range">Values to compare against. An empty range never matches.</param>
        /// <returns>True if a match is found; otherwise false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="range"/> is <c>null</c>.</exception>
        /// <remarks>
        ///     Equality is decided by <see cref="EqualityComparer{T}.Default"/>, so a type that overrides
        ///     <see cref="object.Equals(object)"/> or implements <see cref="IEquatable{T}"/> is honoured, and two
        ///     nulls compare equal without either side having to be dereferenced.
        /// </remarks>
        public bool In(params T[] range)
        {
            ArgumentNullException.ThrowIfNull(range);

            return Array.Exists(range, other => EqualityComparer<T>.Default.Equals(self, other));
        }

        /// <summary>
        ///     Returns true if the value is not equal to any element in the provided range.
        /// </summary>
        /// <param name="range">Values to compare against. An empty range never matches, so the result is true.</param>
        /// <returns>True if no match is found; otherwise false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="range"/> is <c>null</c>.</exception>
        public bool NotIn(params T[] range) => !self.In(range);
    }
}
