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
    public void Should_CheckLowerChar(string input, bool expected) => Assert.Equal(expected, input.HasLowerChar());

    [Theory]
    [InlineData("ABC", true)]
    [InlineData("abc", false)]
    [InlineData("123", false)]
    [InlineData("aBc", true)]
    [InlineData("", false)]
    public void Should_CheckUpperChar(string input, bool expected) => Assert.Equal(expected, input.HasUpperChar());

    [Theory]
    [InlineData("abc", false)]
    [InlineData("123", true)]
    [InlineData("a1b", true)]
    [InlineData("", false)]
    public void Should_CheckNumber(string input, bool expected) => Assert.Equal(expected, input.HasNumber());

    [Theory]
    [InlineData("abc", 3, true)]
    [InlineData("abcd", 3, false)]
    [InlineData("", 0, true)]
    [InlineData("a", 0, false)]
    public void Should_CheckMaxLength(string input, int maxLength, bool expected) =>
        Assert.Equal(expected, input.HasMaxLength(maxLength));

    [Theory]
    [InlineData("abc", 3, true)]
    [InlineData("ab", 3, false)]
    [InlineData("", 0, true)]
    [InlineData("", 1, false)]
    public void Should_CheckMinLength(string input, int minLength, bool expected) =>
        Assert.Equal(expected, input.HasMinLength(minLength));

    [Theory]
    [InlineData("user@example.com", true)]
    [InlineData("user.name+tag@example.co.uk", true)]
    [InlineData("invalid", false)]
    [InlineData("user@", false)]
    [InlineData("@example.com", false)]
    public void Should_ValidateEmail(string input, bool expected) => Assert.Equal(expected, input.IsValidEmail());

    [Theory]
    [InlineData(null, 'x', null)]
    [InlineData("", 'x', "")]
    [InlineData("  test  ", 't', "es")]
    [InlineData("xxvaluexx", 'x', "value")]
    [InlineData(" value ", ' ', "value")]
    public void Should_TrimChar(string? input, char charToTrim, string? expected)
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
    public void Should_ParseToBoolOrDefault(string? input, bool defaultValue, bool expected) =>
        Assert.Equal(expected, input.ParseToBoolOrDefault(defaultValue));

    [Theory]
    [InlineData("42", 0, 42)]
    [InlineData("-1", 99, -1)]
    [InlineData("notanint", 99, 99)]
    [InlineData(null, 7, 7)]
    public void Should_ParseToIntOrDefault(string? input, int defaultValue, int expected) =>
        Assert.Equal(expected, input.ParseToIntOrDefault(defaultValue));

    [Fact]
    public void Should_ParseToObjectOrDefault_WithValidJson()
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
    public void Should_ReturnNull_OnInvalidOrEmptyJson(string? input) =>
        Assert.Null(input.ParseToObjectOrDefault<Person>());

    [Theory]
    [InlineData("One", true)]
    [InlineData("Two", true)]
    [InlineData("Unknown", false)]
    public void Should_ValidateEnumValues_DefaultIgnoreCase(string input, bool expected) =>
        Assert.Equal(expected, input.IsValidEnumValue<TestEnum>());

    [Theory]
    [InlineData("One", true)]
    [InlineData("one", false)]
    public void Should_ValidateEnumValues_RespectCase(string input, bool expected) =>
        Assert.Equal(expected, input.IsValidEnumValue<TestEnum>(false));

    [Theory]
    [InlineData(null, "fallback", "fallback")]
    [InlineData("", "fallback", "fallback")]
    [InlineData("value", "fallback", "value")]
    public void Should_ReturnValueOrDefault(string? input, string? defaultValue, string? expected) =>
        Assert.Equal(expected, input.ValueOrDefault(defaultValue));

    [Fact]
    public void Should_JoinWith_DefaultSeparator_ForStrings()
    {
        var source = new[] { "a", "b", "c" };
        var result = source.JoinWith();

        Assert.Equal("a, b, c", result);
    }

    [Fact]
    public void Should_JoinWith_CustomSeparator_ForStrings()
    {
        var source = new[] { "a", "b", "c" };
        var result = source.JoinWith("|");

        Assert.Equal("a|b|c", result);
    }

    [Fact]
    public void Should_JoinWith_DefaultSeparator_ForObjects()
    {
        var source = new object?[] { 1, null, "x" };
        var result = source.JoinWith();

        Assert.Equal("1, , x", result);
    }

    [Fact]
    public void Should_JoinWith_CustomSeparator_ForObjects()
    {
        var source = new[] { new Person { Name = "A" }, new Person { Name = "B" } };
        var result = source.JoinWith(";");

        Assert.Equal(string.Join(";", source.Select(p => p.ToString())), result);
    }
}
