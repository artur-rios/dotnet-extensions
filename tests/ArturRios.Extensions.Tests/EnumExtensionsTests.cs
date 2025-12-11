using ArturRios.Extensions.Tests.Mock;

namespace ArturRios.Extensions.Tests;

public class EnumExtensionsTests
{
    [Fact]
    public void Should_GetDescription()
    {
        var description = TestEnum.One.GetDescription();

        Assert.Equal("One", description);
    }

    [Fact]
    public void Should_ReturnNull_When_ValueHasNoDescription()
    {
        var description = TestEnum.Three.GetDescription();

        Assert.Null(description);
    }

    [Fact]
    public void Should_ReturnNull_When_ValueNotFound()
    {
        var description = ((TestEnum)100).GetDescription();

        Assert.Null(description);
    }
}