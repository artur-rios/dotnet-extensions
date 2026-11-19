using System.Globalization;

namespace ArturRios.Extensions.Tests.Mock;

/// <summary>
/// Sets the current culture for the lifetime of the scope, so a test that formats or parses text reads the same on
/// the owner's pt-BR machine and on the invariant-culture CI agent.
/// </summary>
public sealed class CultureScope : IDisposable
{
    private readonly CultureInfo _original = CultureInfo.CurrentCulture;

    public CultureScope(CultureInfo culture) => CultureInfo.CurrentCulture = culture;

    public void Dispose() => CultureInfo.CurrentCulture = _original;
}
