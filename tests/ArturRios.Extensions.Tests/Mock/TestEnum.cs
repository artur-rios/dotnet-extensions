using System.ComponentModel;

namespace ArturRios.Extensions.Tests.Mock;

public enum TestEnum
{
    [Description("One")] One = 1,

    [Description("Two")] Two = 2,

    Three = 3
}
