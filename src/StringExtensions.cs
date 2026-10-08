using System.Globalization;
using System.Text.Json;
using ArturRios.Util.RegularExpressions;
using ArturRios.Util.Text;

namespace ArturRios.Extensions;

/// <summary>
///     Provides extension methods for working with strings, including validation helpers,
///     parsing utilities, and convenient join operations.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    ///     Options used by <see cref="ParseToObjectOrDefault{T}(string?)"/>: property names match regardless
    ///     of case.
    /// </summary>
    private static readonly JsonSerializerOptions CaseInsensitiveJson = new() { PropertyNameCaseInsensitive = true };

    /// <summary>
    ///     Checks if the string corresponds to a value declared by the specified enum type.
    /// </summary>
    /// <typeparam name="TEnum">The enum type to validate against.</typeparam>
    /// <param name="string">Input string value.</param>
    /// <param name="ignoreCase">Whether to ignore case during parsing (default true).</param>
    /// <returns>True if the string names a declared member of the enum; otherwise false.</returns>
    /// <remarks>
    ///     Leading and trailing whitespace is tolerated, as <see cref="Enum.TryParse(Type, string, bool, out object)"/>
    ///     trims it. A numeric string is only accepted when the number it denotes is a declared member.
    ///     <see cref="Enum.TryParse(Type, string, bool, out object)"/> on its own accepts any number in the
    ///     underlying type's range, so "999" parsed as a three-member enum used to be reported as valid.
    /// </remarks>
    public static bool IsValidEnumValue<TEnum>(this string @string, bool ignoreCase = true) where TEnum : Enum =>
        Enum.TryParse(typeof(TEnum), @string, ignoreCase, out var parsed) && Enum.IsDefined(typeof(TEnum), parsed!);

    /// <summary>
    ///     Returns the string if it has value; otherwise returns the provided default.
    /// </summary>
    /// <param name="string">The input string.</param>
    /// <param name="defaultValue">Default value when input is null or empty.</param>
    /// <returns>The input string or the default value.</returns>
    public static string? ValueOrDefault(this string? @string, string? defaultValue = null) =>
        string.IsNullOrEmpty(@string) ? defaultValue : @string;

    /// <summary>
    ///     Joins the sequence of strings using the provided separator.
    /// </summary>
    /// <param name="source">The sequence of strings to join.</param>
    /// <param name="separator">Separator to use (default ", ").</param>
    /// <returns>A single concatenated string.</returns>
    public static string JoinWith(this IEnumerable<string> source, string separator = ", ") =>
        string.Join(separator, source);

    /// <summary>
    ///     Joins the sequence of items using the provided separator, converting each to string.
    /// </summary>
    /// <typeparam name="T">Item type.</typeparam>
    /// <param name="source">The sequence to join.</param>
    /// <param name="separator">Separator to use (default ", ").</param>
    /// <returns>A single concatenated string.</returns>
    public static string JoinWith<T>(this IEnumerable<T> source, string separator = ", ") =>
        string.Join(separator, source.Select(x => x?.ToString()));

    /// <summary>
    ///     Provides validation helpers for the given non-null string.
    /// </summary>
    extension(string @string)
    {
        /// <summary>
        ///     Checks whether the string contains at least one lowercase character.
        /// </summary>
        /// <returns>True if the string has a lowercase character; otherwise false.</returns>
        public bool HasLowerChar() => RegexCollection.HasLowerChar().IsMatch(@string);

        /// <summary>
        ///     Checks whether the string length is less than or equal to the given maximum.
        /// </summary>
        /// <param name="maxLength">Maximum allowed length.</param>
        /// <returns>True if within the max length; otherwise false.</returns>
        public bool HasMaxLength(int maxLength) => !(@string.Length > maxLength);

        /// <summary>
        ///     Checks whether the string length is greater than or equal to the given minimum.
        /// </summary>
        /// <param name="minLength">Minimum required length.</param>
        /// <returns>True if meets the min length; otherwise false.</returns>
        public bool HasMinLength(int minLength) => !(@string.Length < minLength);

        /// <summary>
        ///     Checks whether the string contains at least one numeric character.
        /// </summary>
        /// <returns>True if a number is present; otherwise false.</returns>
        public bool HasNumber() => RegexCollection.HasNumber().IsMatch(@string);

        /// <summary>
        ///     Checks whether the string contains at least one uppercase character.
        /// </summary>
        /// <returns>True if the string has an uppercase character; otherwise false.</returns>
        public bool HasUpperChar() => RegexCollection.HasUpperChar().IsMatch(@string);

        /// <summary>
        ///     Validates whether the string is a well-formed email address.
        /// </summary>
        /// <returns>True if the string is a valid email; otherwise false.</returns>
        /// <remarks>
        ///     Delegates to <see cref="EmailAddress.IsValid"/>, so a mixed-case or internationalized domain is
        ///     normalized before the syntax check rather than rejected outright. Applying
        ///     <see cref="RegexCollection.Email"/> directly, as this used to, made the same address valid here
        ///     and invalid there depending on which of the two a caller happened to reach for.
        /// </remarks>
        public bool IsValidEmail() => EmailAddress.IsValid(@string);

        /// <summary>
        ///     Trims leading and trailing whitespace, then trims the specified character from the ends of the
        ///     result.
        /// </summary>
        /// <param name="charToTrim">Character to trim from both ends.</param>
        /// <returns>The trimmed string.</returns>
        /// <remarks>
        ///     The two passes run in that order and once each, so whitespace uncovered by removing
        ///     <paramref name="charToTrim"/> is left in place: <c>"- a -"</c> trimmed of <c>'-'</c> yields
        ///     <c>" a "</c>.
        /// </remarks>
        public string TrimChar(char charToTrim) =>
            string.IsNullOrEmpty(@string) ? @string : @string.Trim().Trim(charToTrim);
    }

    /// <summary>
    ///     Provides parsing helpers for a nullable string.
    /// </summary>
    extension(string? @string)
    {
        /// <summary>
        ///     Parses the string to a boolean, or returns the provided default if parsing fails.
        /// </summary>
        /// <param name="defaultValue">Value to return when parsing fails.</param>
        /// <returns>The parsed boolean or the default value.</returns>
        public bool? ParseToBoolOrDefault(bool? defaultValue = null) =>
            bool.TryParse(@string, out var result) ? result : defaultValue;

        /// <summary>
        ///     Parses the string to an integer, or returns the provided default if parsing fails.
        /// </summary>
        /// <param name="defaultValue">Value to return when parsing fails.</param>
        /// <returns>The parsed integer or the default value.</returns>
        /// <remarks>
        ///     Parsing uses the invariant culture, so the result does not depend on the machine it runs on.
        ///     Under the current culture, <c>"-5"</c> came back as the default on a machine set to a culture
        ///     whose negative sign is not the ASCII hyphen-minus, such as fa-IR or ar-SA. Surrounding
        ///     whitespace and a leading sign are accepted; group separators are not.
        /// </remarks>
        public int? ParseToIntOrDefault(int? defaultValue = null) =>
            int.TryParse(@string, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result)
                ? result
                : defaultValue;

        /// <summary>
        ///     Attempts to deserialize the JSON string to an object of type <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">Target reference type.</typeparam>
        /// <returns>An instance of T if deserialization succeeds; otherwise null.</returns>
        /// <remarks>
        ///     Property names are matched without regard to case, so <c>{"name":"Ana"}</c> binds to a
        ///     <c>Name</c> property. Matching only the exact case left every member at its default without
        ///     any error, unlike <see cref="GenericExtensions.Clone{T}"/>, the HTTP helpers of ArturRios.Util
        ///     and Microsoft.Extensions.Configuration binding, which all ignore case.
        /// </remarks>
        public T? ParseToObjectOrDefault<T>() where T : class
        {
            if (string.IsNullOrEmpty(@string))
            {
                return null;
            }

            try
            {
                return JsonSerializer.Deserialize<T>(@string, CaseInsensitiveJson);
            }
            catch
            {
                return null;
            }
        }
    }
}
