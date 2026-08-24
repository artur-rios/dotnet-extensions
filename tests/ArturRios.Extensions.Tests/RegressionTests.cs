using ArturRios.Extensions.Tests.Mock;

namespace ArturRios.Extensions.Tests;

/// <summary>
/// Covers the edge cases the individual extension test classes did not reach, and pins the behaviours
/// corrected in this pass so they cannot quietly regress.
/// </summary>
[Trait("Category", "Unit")]
public class RegressionTests
{
    [Theory]
    [InlineData("999")]
    [InlineData("0")]
    [InlineData("-1")]
    [InlineData("2147483647")]
    public void GivenANumericStringOutsideTheDeclaredMembers_WhenValidatingEnumValue_ThenItIsRejected(string input)
    {
        Assert.False(input.IsValidEnumValue<TestEnum>());
    }

    [Theory]
    [InlineData("1")]
    [InlineData("2")]
    [InlineData("3")]
    public void GivenANumericStringMatchingADeclaredMember_WhenValidatingEnumValue_ThenItIsAccepted(string input)
    {
        Assert.True(input.IsValidEnumValue<TestEnum>());
    }

    [Fact]
    public void GivenAMemberNamePaddedWithWhitespace_WhenValidatingEnumValue_ThenItIsAccepted()
    {
        Assert.True("  One  ".IsValidEnumValue<TestEnum>());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void GivenNoMeaningfulInput_WhenValidatingEnumValue_ThenItIsRejected(string? input)
    {
        Assert.False(input!.IsValidEnumValue<TestEnum>());
    }

    [Fact]
    public void GivenAnInternationalizedDomain_WhenValidatingEmail_ThenItIsAccepted()
    {
        Assert.True("user@héllo.com".IsValidEmail());
    }

    [Fact]
    public void GivenAMixedCaseDomain_WhenValidatingEmail_ThenItIsAccepted()
    {
        Assert.True("MA@Hostname.COM".IsValidEmail());
    }

    [Theory]
    [InlineData("john@doe.com")]
    [InlineData("first.last@sub.domain.co.uk")]
    public void GivenAWellFormedAddress_WhenValidatingEmail_ThenItIsAccepted(string input)
    {
        Assert.True(input.IsValidEmail());
    }

    [Theory]
    [InlineData("not-an-email")]
    [InlineData("user@")]
    [InlineData("@host.com")]
    [InlineData("user@host")]
    [InlineData(" user@host.com")]
    public void GivenAMalformedAddress_WhenValidatingEmail_ThenItIsRejected(string input)
    {
        Assert.False(input.IsValidEmail());
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

    [Fact]
    public void GivenADateTimeWithSubSecondTicks_WhenRemovingMilliseconds_ThenEveryTickBelowASecondIsDropped()
    {
        var original = new DateTime(2026, 8, 24, 13, 45, 30, DateTimeKind.Utc).AddTicks(9_876_543);

        var truncated = original.RemoveMilliseconds();

        Assert.Equal(new DateTime(2026, 8, 24, 13, 45, 30, DateTimeKind.Utc), truncated);
        Assert.Equal(0, truncated.Millisecond);
        Assert.Equal(0, truncated.Ticks % TimeSpan.TicksPerSecond);
        Assert.Equal(DateTimeKind.Utc, truncated.Kind);
    }

    [Theory]
    [InlineData(DateTimeKind.Utc)]
    [InlineData(DateTimeKind.Local)]
    [InlineData(DateTimeKind.Unspecified)]
    public void GivenAnyKind_WhenRemovingMilliseconds_ThenTheKindIsPreserved(DateTimeKind kind)
    {
        var original = new DateTime(2026, 8, 24, 13, 45, 30, 999, kind);

        Assert.Equal(kind, original.RemoveMilliseconds().Kind);
    }

    [Fact]
    public void GivenANullSource_WhenCloning_ThenNullComesBack()
    {
        Person? source = null;

        Assert.Null(source.Clone());
    }

    [Fact]
    public void GivenAGraphWithAReferenceCycle_WhenCloning_ThenTheFailureIsReported()
    {
        var first = new Node();
        var second = new Node { Other = first };

        first.Other = second;

        Assert.ThrowsAny<Exception>(() => first.Clone());
    }

    [Fact]
    public void GivenAStringWithWhitespaceInsideTheTrimmedCharacters_WhenTrimmingAChar_ThenTheWhitespaceRemains()
    {
        Assert.Equal(" a ", "- a -".TrimChar('-'));
        Assert.Equal("a", " -a- ".TrimChar('-'));
    }

    [Fact]
    public void GivenAStringOfOnlyTheTrimmedCharacter_WhenTrimmingAChar_ThenTheResultIsEmpty()
    {
        Assert.Equal(string.Empty, "---".TrimChar('-'));
    }

    private sealed class Node
    {
        public Node? Other { get; set; }
    }
}
