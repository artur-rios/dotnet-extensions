using System.Text.Json;
using ArturRios.Util.RegularExpressions;

namespace ArturRios.Extensions;

/// <summary>
/// Provides extension methods for working with strings, including validation helpers,
/// parsing utilities, and convenient join operations.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Provides validation helpers for the given non-null string.
    /// </summary>
    extension(string @string)
    {
        /// <summary>
        /// Checks whether the string contains at least one lowercase character.
        /// </summary>
        /// <returns>True if the string has a lowercase character; otherwise false.</returns>
        public bool HasLowerChar() => RegexCollection.HasLowerChar().IsMatch(@string);

        /// <summary>
        /// Checks whether the string length is less than or equal to the given maximum.
        /// </summary>
        /// <param name="maxLength">Maximum allowed length.</param>
        /// <returns>True if within the max length; otherwise false.</returns>
        public bool HasMaxLength(int maxLength) => !(@string.Length > maxLength);

        /// <summary>
        /// Checks whether the string length is greater than or equal to the given minimum.
        /// </summary>
        /// <param name="minLength">Minimum required length.</param>
        /// <returns>True if meets the min length; otherwise false.</returns>
        public bool HasMinLength(int minLength) => !(@string.Length < minLength);

        /// <summary>
        /// Checks whether the string contains at least one numeric character.
        /// </summary>
        /// <returns>True if a number is present; otherwise false.</returns>
        public bool HasNumber() => RegexCollection.HasNumber().IsMatch(@string);

        /// <summary>
        /// Checks whether the string contains at least one uppercase character.
        /// </summary>
        /// <returns>True if the string has an uppercase character; otherwise false.</returns>
        public bool HasUpperChar() => RegexCollection.HasUpperChar().IsMatch(@string);

        /// <summary>
        /// Validates whether the string matches a basic email format.
        /// </summary>
        /// <returns>True if the string is a valid email; otherwise false.</returns>
        public bool IsValidEmail() => RegexCollection.Email().IsMatch(@string);

        /// <summary>
        /// Trims leading/trailing whitespace and the specified character from the ends of the string.
        /// </summary>
        /// <param name="charToTrim">Character to trim from both ends.</param>
        /// <returns>The trimmed string.</returns>
        public string TrimChar(char charToTrim) =>
            string.IsNullOrEmpty(@string) ? @string : @string.Trim().Trim(charToTrim);
    }

    /// <summary>
    /// Provides parsing helpers for a nullable string.
    /// </summary>
    extension(string? @string)
    {
        /// <summary>
        /// Parses the string to a boolean, or returns the provided default if parsing fails.
        /// </summary>
        /// <param name="defaultValue">Value to return when parsing fails.</param>
        /// <returns>The parsed boolean or the default value.</returns>
        public bool? ParseToBoolOrDefault(bool? defaultValue = null) =>
            bool.TryParse(@string, out var result) ? result : defaultValue;

        /// <summary>
        /// Parses the string to an integer, or returns the provided default if parsing fails.
        /// </summary>
        /// <param name="defaultValue">Value to return when parsing fails.</param>
        /// <returns>The parsed integer or the default value.</returns>
        public int? ParseToIntOrDefault(int? defaultValue = null) =>
            int.TryParse(@string, out var result) ? result : defaultValue;

        /// <summary>
        /// Attempts to deserialize the JSON string to an object of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Target reference type.</typeparam>
        /// <returns>An instance of T if deserialization succeeds; otherwise null.</returns>
        public T? ParseToObjectOrDefault<T>() where T : class
        {
            if (string.IsNullOrEmpty(@string))
            {
                return null;
            }

            try
            {
                return JsonSerializer.Deserialize<T>(@string);
            }
            catch
            {
                return null;
            }
        }
    }

    /// <summary>
    /// Checks if the string corresponds to a valid value of the specified enum type.
    /// </summary>
    /// <typeparam name="TEnum">The enum type to validate against.</typeparam>
    /// <param name="string">Input string value.</param>
    /// <param name="ignoreCase">Whether to ignore case during parsing (default true).</param>
    /// <returns>True if the string can be parsed to the enum; otherwise false.</returns>
    public static bool IsValidEnumValue<TEnum>(this string @string, bool ignoreCase = true) where TEnum : Enum =>
        Enum.TryParse(typeof(TEnum), @string, ignoreCase, out _);

    /// <summary>
    /// Returns the string if it has value; otherwise returns the provided default.
    /// </summary>
    /// <param name="string">The input string.</param>
    /// <param name="defaultValue">Default value when input is null or empty.</param>
    /// <returns>The input string or the default value.</returns>
    public static string? ValueOrDefault(this string? @string, string? defaultValue = null) =>
        string.IsNullOrEmpty(@string) ? defaultValue : @string;

    /// <summary>
    /// Joins the sequence of strings using the provided separator.
    /// </summary>
    /// <param name="source">The sequence of strings to join.</param>
    /// <param name="separator">Separator to use (default ", ").</param>
    /// <returns>A single concatenated string.</returns>
    public static string JoinWith(this IEnumerable<string> source, string separator = ", ") =>
        string.Join(separator, source);

    /// <summary>
    /// Joins the sequence of items using the provided separator, converting each to string.
    /// </summary>
    /// <typeparam name="T">Item type.</typeparam>
    /// <param name="source">The sequence to join.</param>
    /// <param name="separator">Separator to use (default ", ").</param>
    /// <returns>A single concatenated string.</returns>
    public static string JoinWith<T>(this IEnumerable<T> source, string separator = ", ") =>
        string.Join(separator, source.Select(x => x?.ToString()));
}
