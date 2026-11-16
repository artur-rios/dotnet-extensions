using System.Globalization;
using System.Text.Json;
using ArturRios.Extensions.Tests.Mock;

namespace ArturRios.Extensions.Tests;

[Trait("Category", "Unit")]
public class StringExtensionsTests
{
    [Theory]
    [InlineData("abc", true)]
    [InlineData("ABC", false)]
    [InlineData("123", false)]
    [InlineData("AbC", true)]
    [InlineData("", false)]
    public void GivenString_WhenCheckingLowerChar_ThenReturnsExpectedResult(string input, bool expected) => Assert.Equal(expected, input.HasLowerChar());

    [Theory]
    [InlineData("ABC", true)]
    [InlineData("abc", false)]
    [InlineData("123", false)]
    [InlineData("aBc", true)]
    [InlineData("", false)]
    public void GivenString_WhenCheckingUpperChar_ThenReturnsExpectedResult(string input, bool expected) => Assert.Equal(expected, input.HasUpperChar());

    [Theory]
    [InlineData("abc", false)]
    [InlineData("123", true)]
    [InlineData("a1b", true)]
    [InlineData("", false)]
    public void GivenString_WhenCheckingNumber_ThenReturnsExpectedResult(string input, bool expected) => Assert.Equal(expected, input.HasNumber());

    [Theory]
    [InlineData("abc", 3, true)]
    [InlineData("abcd", 3, false)]
    [InlineData("", 0, true)]
    [InlineData("a", 0, false)]
    public void GivenString_WhenCheckingMaxLength_ThenReturnsExpectedResult(string input, int maxLength, bool expected) =>
        Assert.Equal(expected, input.HasMaxLength(maxLength));

    [Theory]
    [InlineData("abc", 3, true)]
    [InlineData("ab", 3, false)]
    [InlineData("", 0, true)]
    [InlineData("", 1, false)]
    public void GivenString_WhenCheckingMinLength_ThenReturnsExpectedResult(string input, int minLength, bool expected) =>
        Assert.Equal(expected, input.HasMinLength(minLength));

    [Theory]
    [InlineData("user@example.com", true)]
    [InlineData("user.name+tag@example.co.uk", true)]
    [InlineData("invalid", false)]
    [InlineData("user@", false)]
    [InlineData("@example.com", false)]
    public void GivenString_WhenValidatingEmail_ThenReturnsExpectedResult(string input, bool expected) => Assert.Equal(expected, input.IsValidEmail());

    [Theory]
    [InlineData(null, 'x', null)]
    [InlineData("", 'x', "")]
    [InlineData("  test  ", 't', "es")]
    [InlineData("xxvaluexx", 'x', "value")]
    [InlineData(" value ", ' ', "value")]
    public void GivenString_WhenTrimmingChar_ThenReturnsExpectedResult(string? input, char charToTrim, string? expected)
    {
        var actual = input?.TrimChar(charToTrim);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("true", true, true)]
    [InlineData("false", false, false)]
    [InlineData("false", true, false)]
    [InlineData("notabool", false, false)]
    [InlineData(null, true, true)]
    [InlineData(null, false, false)]
    public void GivenString_WhenParsingToBoolOrDefault_ThenReturnsExpectedResult(string? input, bool defaultValue, bool expected) =>
        Assert.Equal(expected, input.ParseToBoolOrDefault(defaultValue));

    [Theory]
    [InlineData("42", 0, 42)]
    [InlineData("-1", 99, -1)]
    [InlineData("notanint", 99, 99)]
    [InlineData(null, 7, 7)]
    public void GivenString_WhenParsingToIntOrDefault_ThenReturnsExpectedResult(string? input, int defaultValue, int expected) =>
        Assert.Equal(expected, input.ParseToIntOrDefault(defaultValue));

    [Fact]
    public void GivenValidJson_WhenParsingToObjectOrDefault_ThenReturnsObject()
    {
        var person = new Person { Name = "Alice", Age = 30, Home = new Address { Street = "Main", Number = 100 } };
        var json = JsonSerializer.Serialize(person);

        var parsed = json.ParseToObjectOrDefault<Person>();

        Assert.NotNull(parsed);
        Assert.Equal(person.Name, parsed.Name);
        Assert.Equal(person.Age, parsed.Age);
        Assert.NotNull(parsed.Home);
        Assert.Equal(person.Home.Street, parsed.Home.Street);
        Assert.Equal(person.Home.Number, parsed.Home.Number);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("{ invalid json }")]
    public void GivenInvalidOrEmptyJson_WhenParsingToObjectOrDefault_ThenReturnsNull(string? input) =>
        Assert.Null(input.ParseToObjectOrDefault<Person>());

    [Theory]
    [InlineData("One", true)]
    [InlineData("Two", true)]
    [InlineData("Unknown", false)]
    public void GivenString_WhenValidatingEnumValuesWithDefaultIgnoreCase_ThenReturnsExpectedResult(string input, bool expected) =>
        Assert.Equal(expected, input.IsValidEnumValue<TestEnum>());

    [Theory]
    [InlineData("One", true)]
    [InlineData("one", false)]
    public void GivenString_WhenValidatingEnumValuesRespectingCase_ThenReturnsExpectedResult(string input, bool expected) =>
        Assert.Equal(expected, input.IsValidEnumValue<TestEnum>(false));

    [Theory]
    [InlineData(null, "fallback", "fallback")]
    [InlineData("", "fallback", "fallback")]
    [InlineData("value", "fallback", "value")]
    public void GivenString_WhenCallingValueOrDefault_ThenReturnsExpectedResult(string? input, string? defaultValue, string? expected) =>
        Assert.Equal(expected, input.ValueOrDefault(defaultValue));

    [Fact]
    public void GivenStringArray_WhenJoiningWithDefaultSeparator_ThenJoinsStrings()
    {
        var source = new[] { "a", "b", "c" };
        var result = source.JoinWith();

        Assert.Equal("a, b, c", result);
    }

    [Fact]
    public void GivenStringArray_WhenJoiningWithCustomSeparator_ThenJoinsStrings()
    {
        var source = new[] { "a", "b", "c" };
        var result = source.JoinWith("|");

        Assert.Equal("a|b|c", result);
    }

    [Fact]
    public void GivenObjectArray_WhenJoiningWithDefaultSeparator_ThenJoinsObjects()
    {
        var source = new object?[] { 1, null, "x" };
        var result = source.JoinWith();

        Assert.Equal("1, , x", result);
    }

    [Fact]
    public void GivenObjectArray_WhenJoiningWithCustomSeparator_ThenJoinsObjects()
    {
        var source = new[] { new Person { Name = "A" }, new Person { Name = "B" } };
        var result = source.JoinWith(";");

        Assert.Equal(string.Join(";", source.Select(p => p.ToString())), result);
    }

    [Theory]
    [InlineData("fa-IR")]
    [InlineData("ar-SA")]
    [InlineData("en-US")]
    public void GivenANegativeNumberAndAnyCurrentCulture_WhenParsingToIntOrDefault_ThenTheInvariantFormIsParsed(string culture)
    {
        var original = CultureInfo.CurrentCulture;

        try
        {
            CultureInfo.CurrentCulture = new CultureInfo(culture);

            Assert.Equal(-5, "-5".ParseToIntOrDefault());
            Assert.Equal(1234, " 1234 ".ParseToIntOrDefault());
        }
        finally
        {
            CultureInfo.CurrentCulture = original;
        }
    }

    [Fact]
    public void GivenAGroupSeparator_WhenParsingToIntOrDefault_ThenTheDefaultIsReturned() =>
        Assert.Null("1,000".ParseToIntOrDefault());

    [Fact]
    public void GivenCamelCaseJson_WhenParsingToObjectOrDefault_ThenPropertiesBindRegardlessOfCase()
    {
        const string json = "{\"name\":\"Ana\",\"age\":25,\"home\":{\"street\":\"Main\",\"number\":7}}";

        var parsed = json.ParseToObjectOrDefault<Person>();

        Assert.NotNull(parsed);
        Assert.Equal("Ana", parsed.Name);
        Assert.Equal(25, parsed.Age);
        Assert.Equal("Main", parsed.Home.Street);
        Assert.Equal(7, parsed.Home.Number);
    }

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

    [Theory]
    [InlineData("abc", 3, true)]
    [InlineData("abc", 2, false)]
    [InlineData("", 0, true)]
    [InlineData("", -1, false)]
    public void GivenALengthAtTheBoundary_WhenCheckingMaxLength_ThenTheLimitIsInclusive(string value, int maxLength,
        bool expected)
    {
        Assert.Equal(expected, value.HasMaxLength(maxLength));
    }

    [Theory]
    [InlineData("abc", 3, true)]
    [InlineData("abc", 4, false)]
    [InlineData("", 0, true)]
    [InlineData("a", -1, true)]
    public void GivenALengthAtTheBoundary_WhenCheckingMinLength_ThenTheLimitIsInclusive(string value, int minLength,
        bool expected)
    {
        Assert.Equal(expected, value.HasMinLength(minLength));
    }

    [Theory]
    [InlineData("TRUE", true)]
    [InlineData(" false ", false)]
    [InlineData("1", null)]
    [InlineData("yes", null)]
    [InlineData(null, null)]
    public void GivenTheSpellingsBoolAccepts_WhenParsingToBoolOrDefault_ThenOnlyTrueAndFalseParse(string? value, bool? expected)
    {
        Assert.Equal(expected, value.ParseToBoolOrDefault());
    }

    [Theory]
    [InlineData(" 42 ", 42)]
    [InlineData("+7", 7)]
    [InlineData("2147483647", int.MaxValue)]
    [InlineData("-2147483648", int.MinValue)]
    [InlineData("2147483648", null)]
    [InlineData("1.0", null)]
    [InlineData("0x10", null)]
    [InlineData("", null)]
    [InlineData(null, null)]
    public void GivenTheEdgesOfInt_WhenParsingToIntOrDefault_ThenOnlyAnIntegerInRangeParses(string? value, int? expected)
    {
        Assert.Equal(expected, value.ParseToIntOrDefault());
    }

    [Fact]
    public void GivenADefault_WhenParsingFails_ThenTheDefaultComesBack()
    {
        Assert.Equal(-1, "x".ParseToIntOrDefault(-1));
        Assert.True("x".ParseToBoolOrDefault(true));
    }

    [Theory]
    [InlineData("null")]
    [InlineData("[]")]
    [InlineData("{")]
    [InlineData("\"text\"")]
    public void GivenJsonThatIsNotAnObjectOfTheType_WhenParsingToObjectOrDefault_ThenNullComesBack(string json)
    {
        Assert.Null(json.ParseToObjectOrDefault<Person>());
    }

    [Theory]
    [InlineData(null, "fallback", "fallback")]
    [InlineData("", "fallback", "fallback")]
    [InlineData(" ", "fallback", " ")]
    [InlineData("value", "fallback", "value")]
    [InlineData(null, null, null)]
    public void GivenNullEmptyOrWhitespace_WhenCallingValueOrDefault_ThenOnlyNullAndEmptyFallBack(string? value,
        string? defaultValue, string? expected)
    {
        Assert.Equal(expected, value.ValueOrDefault(defaultValue));
    }

    [Fact]
    public void GivenAnEmptySequence_WhenJoining_ThenTheResultIsEmpty()
    {
        Assert.Equal(string.Empty, Array.Empty<string>().JoinWith());
        Assert.Equal(string.Empty, Array.Empty<int>().JoinWith("|"));
    }

    [Fact]
    public void GivenAnEmptyString_WhenTrimmingAChar_ThenItComesBackUnchanged()
    {
        Assert.Equal(string.Empty, string.Empty.TrimChar('-'));
    }
}
