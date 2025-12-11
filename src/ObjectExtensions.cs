using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;

namespace ArturRios.Extensions;

/// <summary>
/// Provides extension methods for working with objects, such as converting to JSON StringContent
/// and extracting property dictionaries.
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// Provides object helpers for the given instance.
    /// </summary>
    extension(object @object)
    {
        /// <summary>
        /// Serializes the object into JSON and wraps it in a <see cref="StringContent"/> with UTF-8 and application/json.
        /// </summary>
        /// <returns>A StringContent containing the JSON representation.</returns>
        public StringContent ToJsonStringContent()
        {
            var json = JsonConvert.SerializeObject(@object);

            return new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
        }

        /// <summary>
        /// Creates a dictionary of the object's properties containing only those with non-null values.
        /// </summary>
        /// <returns>A dictionary mapping property names to non-null values.</returns>
        public Dictionary<string, object> NonNullPropertiesToDictionary()
        {
            Dictionary<string, object> dictionary = new();

            foreach (var propertyInfo in @object.GetType().GetProperties())
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
        /// Creates a dictionary of the object's properties including those with null values.
        /// </summary>
        /// <returns>A dictionary mapping property names to values, possibly null.</returns>
        public Dictionary<string, object?> PropertiesToDictionary()
        {
            Dictionary<string, object?> dictionary = new();

            foreach (var propertyInfo in @object.GetType().GetProperties())
            {
                var value = propertyInfo.GetValue(@object);

                dictionary[propertyInfo.Name] = value;
            }

            return dictionary;
        }
    }
}
