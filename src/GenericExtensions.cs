using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
    ///     the result, and reference identity between members of the graph is not preserved. A settable member
    ///     is replaced with the value read back, so a collection the constructor fills holds the source's items
    ///     once; a collection with no setter can only be added to, so one the constructor fills also keeps the
    ///     constructor's items.
    /// </remarks>
    public static T? Clone<T>(this T source)
    {
        var serialized = JsonConvert.SerializeObject(source, CloneSettings);

        return JsonConvert.DeserializeObject<T>(serialized, CloneSettings);
    }

    // Json.NET's default ObjectCreationHandling.Auto reuses a collection the constructor already created and
    // adds the items it reads to it: a List<string> initialized to ["default"] came back as
    // ["default", "default", "x"], and one the caller had emptied came back as ["default"].
    private static readonly JsonSerializerSettings CloneSettings = new()
    {
        ContractResolver = new ReplaceSettableMembersResolver()
    };

    private sealed class ReplaceSettableMembersResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            // Replace only what can be set: with no setter, Replace would drop the member's value altogether.
            if (property.Writable)
            {
                property.ObjectCreationHandling = ObjectCreationHandling.Replace;
            }

            return property;
        }
    }
}
