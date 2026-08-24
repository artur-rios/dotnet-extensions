using Newtonsoft.Json;

namespace ArturRios.Extensions;

/// <summary>
///     Provides generic helper extensions, including deep-cloning via JSON serialization.
/// </summary>
public static class GenericExtensions
{
    /// <summary>
    ///     Creates a deep clone of the provided object via JSON serialization.
    /// </summary>
    /// <typeparam name="T">Type of the object.</typeparam>
    /// <param name="source">The instance to clone.</param>
    /// <returns>
    ///     A cloned instance of <typeparamref name="T"/>, or <c>null</c> when <paramref name="source"/> is
    ///     <c>null</c>.
    /// </returns>
    /// <exception cref="JsonException">
    ///     The value cannot be round-tripped through JSON — an object graph containing a reference cycle, for
    ///     example. Failures are reported rather than turned into <c>null</c>, which would be
    ///     indistinguishable from cloning a null.
    /// </exception>
    /// <remarks>
    ///     The clone is only as faithful as the JSON round trip: members the serializer ignores are absent from
    ///     the result, and reference identity between members of the graph is not preserved.
    /// </remarks>
    public static T? Clone<T>(this T source)
    {
        var serialized = JsonConvert.SerializeObject(source);

        return JsonConvert.DeserializeObject<T>(serialized);
    }
}
