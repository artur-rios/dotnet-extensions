using ArturRios.Util.Math;

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
    public static bool IsPrime(this int number) => PrimeUtils.IsPrimeNumber(number);

    /// <summary>
    /// Determines whether the specified number is a prime number.
    /// </summary>
    /// <param name="number">The number to check.</param>
    /// <returns>true if the number is prime; otherwise, false.</returns>
    public static bool IsPrime(this long number) => PrimeUtils.IsPrimeNumber(number);

    /// <summary>
    /// Determines whether the specified number is a prime number.
    /// </summary>
    /// <param name="number">The number to check.</param>
    /// <returns>true if the number is prime; otherwise, false.</returns>
    public static bool IsPrime(this short number) => PrimeUtils.IsPrimeNumber(number);

    /// <summary>
    /// Determines whether the specified number is a prime number.
    /// </summary>
    /// <param name="number">The number to check.</param>
    /// <returns>true if the number is prime; otherwise, false.</returns>
    public static bool IsPrime(this byte number) => PrimeUtils.IsPrimeNumber(number);

    /// <summary>
    /// Determines whether the specified number is a prime number.
    /// </summary>
    /// <param name="number">The number to check.</param>
    /// <returns>true if the number is prime; otherwise, false.</returns>
    public static bool IsPrime(this uint number) => PrimeUtils.IsPrimeNumber(number);

    /// <summary>
    /// Determines whether the specified number is a prime number.
    /// </summary>
    /// <param name="number">The number to check.</param>
    /// <returns>true if the number is prime; otherwise, false.</returns>
    public static bool IsPrime(this ulong number) => PrimeUtils.IsPrimeNumber(number);

    /// <summary>
    /// Determines whether the specified number is a prime number.
    /// </summary>
    /// <param name="number">The number to check.</param>
    /// <returns>true if the number is prime; otherwise, false.</returns>
    public static bool IsPrime(this ushort number) => PrimeUtils.IsPrimeNumber(number);

    /// <summary>
    /// Determines whether the specified number is a prime number.
    /// </summary>
    /// <param name="number">The number to check.</param>
    /// <returns>true if the number is prime; otherwise, false.</returns>
    public static bool IsPrime(this sbyte number) => PrimeUtils.IsPrimeNumber(number);
}
