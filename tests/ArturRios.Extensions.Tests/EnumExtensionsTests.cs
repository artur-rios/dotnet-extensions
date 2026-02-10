using ArturRios.Extensions.Tests.Mock;

namespace ArturRios.Extensions.Tests;

public class EnumExtensionsTests
{
    [Fact]
    public void GivenEnumValueWithDescription_WhenGettingDescription_ThenReturnsDescription()
    {
        var description = TestEnum.One.GetDescription();

        Assert.Equal("One", description);
    }

    [Fact]
    public void GivenEnumValueWithoutDescription_WhenGettingDescription_ThenReturnsNull()
    {
        var description = TestEnum.Three.GetDescription();

        Assert.Null(description);
    }

    [Fact]
    public void GivenInvalidEnumValue_WhenGettingDescription_ThenReturnsNull()
    {
        var description = ((TestEnum)100).GetDescription();

        Assert.Null(description);
    }
}
