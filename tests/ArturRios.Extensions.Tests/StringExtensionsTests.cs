using System.Text.Json;
using ArturRios.Extensions.Tests.Mock;

namespace ArturRios.Extensions.Tests;

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
}
