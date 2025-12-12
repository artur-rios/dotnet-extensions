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
        /// <param name="range">Values to compare against.</param>
        /// <returns>True if a match is found; otherwise false.</returns>
        public bool In(params T[] range) => range.Any(t => t?.Equals(self) ?? self == null);

        /// <summary>
        ///     Returns true if the value is not equal to any element in the provided range.
        /// </summary>
        /// <param name="range">Values to compare against.</param>
        /// <returns>True if no match is found; otherwise false.</returns>
        public bool NotIn(params T[] range) => !self.In(range);
    }
}
