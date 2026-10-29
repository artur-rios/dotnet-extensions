namespace ArturRios.Extensions.Tests;

/// <summary>
/// Serializes the test classes that redirect <see cref="Console.Out"/>. The redirection is process-wide, so
/// two such classes running in parallel could capture each other's output, or restore a writer the other
/// one had just installed.
/// </summary>
[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class ConsoleCollection
{
    /// <summary>Name of the collection.</summary>
    public const string Name = "Console";
}
