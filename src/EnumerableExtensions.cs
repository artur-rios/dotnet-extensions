using System.Collections;

namespace ArturRios.Extensions;

/// <summary>
///     Provides extension methods for IEnumerable instances, including emptiness checks and printing contents.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    ///     Provides enumerable helpers for the given sequence.
    /// </summary>
    /// <param name="enumerable">The sequence to print.</param>
    extension(IEnumerable? enumerable)
    {
        /// <summary>
        ///     Determines whether the enumerable is null or has no elements.
        /// </summary>
        /// <returns>True if empty or null; otherwise false.</returns>
        public bool IsEmpty()
        {
            switch (enumerable)
            {
                case null:
                    return true;
                case ICollection collection:
                    return collection.Count == 0;
                default:
                {
                    var enumerator = enumerable.GetEnumerator();
                    try
                    {
                        return !enumerator.MoveNext();
                    }
                    finally
                    {
                        if (enumerator is IDisposable disposable)
                        {
                            disposable.Dispose();
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Determines whether the enumerable contains at least one element.
        /// </summary>
        /// <returns>True if not empty; otherwise false.</returns>
        public bool IsNotEmpty() => !IsEmpty(enumerable);

        /// <summary>
        ///     Prints each item in the enumerable. For complex objects, prints property name and value pairs.
        /// </summary>
        public void PrintContents()
        {
            if (enumerable is null)
            {
                Console.WriteLine("Enumerable is null");

                return;
            }

            foreach (var item in enumerable)
            {
                if (item == null)
                {
                    Console.WriteLine("null");

                    continue;
                }

                var type = item.GetType();

                if (type.IsPrimitive || item is string || item is decimal)
                {
                    Console.WriteLine(item);
                }
                else
                {
                    var properties = type.GetProperties();
                    foreach (var prop in properties)
                    {
                        var value = prop.GetValue(item, null);
                        Console.WriteLine($"{prop.Name}: {value}");
                    }
                }
            }
        }
    }
}
