namespace ArturRios.Extensions.Tests;

public class ComparisonExtensionsTests
{
    private static readonly int[] s_evenNumbers = [0, 2, 4, 6, 8, 10];

    [Fact]
    public void GivenElementInList_WhenCallingIn_ThenReturnsTrue()
    {
        const int two = 2;

        Assert.True(two.In(s_evenNumbers));
    }

    [Fact]
    public void GivenElementNotInList_WhenCallingIn_ThenReturnsFalse()
    {
        const int three = 3;

        Assert.False(three.In(s_evenNumbers));
    }

    [Fact]
    public void GivenElementNotInList_WhenCallingNotIn_ThenReturnsTrue()
    {
        const int three = 3;

        Assert.True(three.NotIn(s_evenNumbers));
    }

    [Fact]
    public void GivenElementInList_WhenCallingNotIn_ThenReturnsFalse()
    {
        const int two = 2;

        Assert.False(two.NotIn(s_evenNumbers));
    }
}
