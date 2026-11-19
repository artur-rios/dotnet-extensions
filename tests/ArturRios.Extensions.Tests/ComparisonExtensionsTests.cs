namespace ArturRios.Extensions.Tests;

[Trait("Category", "Unit")]
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

    [Fact]
    public void GivenAnExpandedParamsCall_WhenCallingInAndNotIn_ThenBothAnswerCorrectly()
    {
        // Compiled with nullable warnings as errors, so this call also pins that an expanded params call
        // raises no CS8620 warning, which the extension-block form produced for every argument.
        Assert.True("b".In("a", "b"));
        Assert.True(4.NotIn(1, 2, 3));
    }

    [Fact]
    public void GivenANullRange_WhenCheckingMembership_ThenArgumentNullExceptionIsThrown()
    {
        Assert.Throws<ArgumentNullException>(() => 1.In(null!));
        Assert.Throws<ArgumentNullException>(() => 1.NotIn(null!));
    }

    [Fact]
    public void GivenAnEmptyRange_WhenCheckingMembership_ThenNothingIsIn()
    {
        Assert.False(1.In());
        Assert.True(1.NotIn());
    }

    [Fact]
    public void GivenANullValueAndARangeContainingNull_WhenCheckingMembership_ThenItIsIn()
    {
        string? value = null;

        Assert.True(value.In(null, "a"));
    }

    [Fact]
    public void GivenANullValueAndARangeWithoutNull_WhenCheckingMembership_ThenItIsNotIn()
    {
        string? value = null;

        Assert.False(value.In("a", "b"));
    }

    [Fact]
    public void GivenARangeContainingNull_WhenCheckingANonNullValue_ThenTheNullEntryDoesNotMatch()
    {
        Assert.False("z".In(null, "a"));
        Assert.True("a".In(null, "a"));
    }

    [Fact]
    public void GivenAValueTypeInARange_WhenCheckingMembership_ThenTheDefaultComparerDecides()
    {
        Assert.True(2.In(1, 2, 3));
        Assert.False(4.In(1, 2, 3));
    }
}
