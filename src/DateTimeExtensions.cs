namespace ArturRios.Extensions;

/// <summary>
///     Provides extension methods for DateTime, such as helpers to remove milliseconds precision.
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    ///     Returns the same DateTime truncated to whole seconds, preserving the Kind.
    /// </summary>
    /// <param name="dateTime">The DateTime to normalize.</param>
    /// <returns>A DateTime truncated to seconds.</returns>
    /// <remarks>
    ///     Every sub-second component is dropped, not only the milliseconds: microseconds and the remaining
    ///     ticks go with them. Truncation is towards zero, so a value is never rounded up to the next second.
    /// </remarks>
    public static DateTime RemoveMilliseconds(this DateTime dateTime) =>
        new(
            dateTime.Year,
            dateTime.Month,
            dateTime.Day,
            dateTime.Hour,
            dateTime.Minute,
            dateTime.Second,
            dateTime.Kind);
}
