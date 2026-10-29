using System.Reflection;

namespace ArturRios.Extensions;

/// <summary>
///     Provides extension methods for working with objects
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    ///     Provides object helpers for the given instance.
    /// </summary>
    extension(object @object)
    {
        /// <summary>
        ///     Creates a dictionary of the object's properties containing only those with non-null values.
        /// </summary>
        /// <returns>A dictionary mapping property names to non-null values.</returns>
        /// <remarks>
        ///     Only public, readable instance properties are included. Static properties are not state of the
        ///     object, and indexers and write-only properties have no single value to read; calling the
        ///     reflection getter on either used to throw.
        /// </remarks>
        public Dictionary<string, object> NonNullPropertiesToDictionary()
        {
            Dictionary<string, object> dictionary = new();

            foreach (var propertyInfo in ReadableInstanceProperties(@object.GetType()))
            {
                var value = propertyInfo.GetValue(@object);

                if (value is not null)
                {
                    dictionary[propertyInfo.Name] = value;
                }
            }

            return dictionary;
        }

        /// <summary>
        ///     Creates a dictionary of the object's properties including those with null values.
        /// </summary>
        /// <returns>A dictionary mapping property names to values, possibly null.</returns>
        /// <remarks>
        ///     Only public, readable instance properties are included, on the same terms as
        ///     <see cref="NonNullPropertiesToDictionary"/>.
        /// </remarks>
        public Dictionary<string, object?> PropertiesToDictionary()
        {
            Dictionary<string, object?> dictionary = new();

            foreach (var propertyInfo in ReadableInstanceProperties(@object.GetType()))
            {
                var value = propertyInfo.GetValue(@object);

                dictionary[propertyInfo.Name] = value;
            }

            return dictionary;
        }
    }

    /// <summary>
    ///     The public instance properties of <paramref name="type"/> that have a getter and take no index.
    /// </summary>
    /// <remarks>
    ///     <see cref="Type.GetProperties()"/> also returns static properties, and indexers or write-only
    ///     properties, which <see cref="PropertyInfo.GetValue(object)"/> cannot read without throwing.
    /// </remarks>
    internal static IEnumerable<PropertyInfo> ReadableInstanceProperties(Type type) =>
        type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(property => property.CanRead && property.GetIndexParameters().Length == 0);
}
