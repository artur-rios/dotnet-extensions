namespace ArturRios.Extensions;

/// <summary>
/// Provides extension methods for integer types.
/// </summary>
public static class IntExtensions
{
    /// <summary>
    /// Determines whether the specified number is a prime number.
    /// </summary>
    /// <param name="number">The number to check.</param>
    /// <returns>true if the number is prime; otherwise, false.</returns>
    public static bool IsPrime(this int number) => IsPrimeCore(number);

    /// <summary>
    /// Determines whether the specified number is a prime number.
    /// </summary>
    /// <param name="number">The number to check.</param>
    /// <returns>true if the number is prime; otherwise, false.</returns>
    public static bool IsPrime(this long number) => IsPrimeCore(number);

    /// <summary>
    /// Determines whether the specified number is a prime number.
    /// </summary>
    /// <param name="number">The number to check.</param>
    /// <returns>true if the number is prime; otherwise, false.</returns>
    public static bool IsPrime(this short number) => IsPrimeCore(number);

    /// <summary>
    /// Determines whether the specified number is a prime number.
    /// </summary>
    /// <param name="number">The number to check.</param>
    /// <returns>true if the number is prime; otherwise, false.</returns>
    public static bool IsPrime(this byte number) => IsPrimeCore(number);

    /// <summary>
    /// Determines whether the specified number is a prime number.
    /// </summary>
    /// <param name="number">The number to check.</param>
    /// <returns>true if the number is prime; otherwise, false.</returns>
    public static bool IsPrime(this uint number) => IsPrimeCore(number);

    /// <summary>
    /// Determines whether the specified number is a prime number.
    /// </summary>
    /// <param name="number">The number to check.</param>
    /// <returns>true if the number is prime; otherwise, false.</returns>
    public static bool IsPrime(this ulong number) => IsPrimeCore(number);

    /// <summary>
    /// Determines whether the specified number is a prime number.
    /// </summary>
    /// <param name="number">The number to check.</param>
    /// <returns>true if the number is prime; otherwise, false.</returns>
    public static bool IsPrime(this ushort number) => IsPrimeCore(number);

    /// <summary>
    /// Determines whether the specified number is a prime number.
    /// </summary>
    /// <param name="number">The number to check.</param>
    /// <returns>true if the number is prime; otherwise, false.</returns>
    public static bool IsPrime(this sbyte number) => IsPrimeCore(number);

    /// <summary>
    /// Core logic for checking if a number is prime.
    /// </summary>
    private static bool IsPrimeCore<T>(T number) where T : IComparable<T>, IEquatable<T>
    {
        // Convert to long for comparison and arithmetic operations
        var num = Convert.ToInt64(number);

        // Numbers less than 2 are not prime
        if (num < 2)
        {
            return false;
        }

        // 2 is prime
        if (num == 2)
        {
            return true;
        }

        // Even numbers (except 2) are not prime
        if (num % 2 == 0)
        {
            return false;
        }

        // Check odd divisors up to sqrt(num)
        var sqrt = (long)Math.Sqrt(num);

        for (long i = 3; i <= sqrt; i += 2)
        {
            if (num % i == 0)
            {
                return false;
            }
        }

        return true;
    }
}
