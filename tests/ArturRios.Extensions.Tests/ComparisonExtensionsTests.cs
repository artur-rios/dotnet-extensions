namespace ArturRios.Extensions.Tests;

public class ComparisonExtensionsTests
{
    private static readonly int[] EvenNumbers = [0, 2, 4, 6, 8, 10];

    [Fact]
    public void Should_ReturnTrue_When_ElementIsInList()
    {
        const int two = 2;

        Assert.True(two.In(EvenNumbers));
    }

    [Fact]
    public void Should_ReturnFalse_When_ElementIsNotInList()
    {
        const int three = 3;

        Assert.False(three.In(EvenNumbers));
    }

    [Fact]
    public void Should_ReturnTrue_When_ElementIsNotInList()
    {
        const int three = 3;

        Assert.True(three.NotIn(EvenNumbers));
    }

    [Fact]
    public void Should_ReturnFalse_When_ElementIsInList_NotIn()
    {
        const int two = 2;

        Assert.False(two.NotIn(EvenNumbers));
    }
}
