using System.ComponentModel;

namespace ArturRios.Extensions;

/// <summary>
/// Provides extension methods for enums, such as retrieving the DescriptionAttribute value.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Gets the value of the <see cref="DescriptionAttribute"/> for the enum value, if present.
    /// </summary>
    /// <param name="value">The enum value.</param>
    /// <returns>The description text, or null if not found.</returns>
    public static string? GetDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = field?.GetCustomAttributes(typeof(DescriptionAttribute), false)
            .Cast<DescriptionAttribute>()
            .FirstOrDefault();

        return attribute?.Description;
    }
}
