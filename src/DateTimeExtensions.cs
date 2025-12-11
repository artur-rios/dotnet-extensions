namespace ArturRios.Extensions;

/// <summary>
/// Provides extension methods for DateTime, such as helpers to remove milliseconds precision.
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// Returns the same DateTime without milliseconds precision, preserving the Kind.
    /// </summary>
    /// <param name="dateTime">The DateTime to normalize.</param>
    /// <returns>A DateTime truncated to seconds.</returns>
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
