using Newtonsoft.Json;

namespace ArturRios.Extensions;

/// <summary>
/// Provides generic helper extensions, including deep-cloning via JSON serialization.
/// </summary>
public static class GenericExtensions
{
    /// <summary>
    /// Creates a deep clone of the provided object via JSON serialization.
    /// </summary>
    /// <typeparam name="T">Type of the object.</typeparam>
    /// <param name="source">The instance to clone.</param>
    /// <returns>A cloned instance of T, or null if serialization fails.</returns>
    public static T? Clone<T>(this T source)
    {
        var serialized = JsonConvert.SerializeObject(source);

        return JsonConvert.DeserializeObject<T>(serialized);
    }
}
