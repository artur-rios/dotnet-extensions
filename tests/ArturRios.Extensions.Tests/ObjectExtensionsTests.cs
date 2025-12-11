using System.Net.Mime;
using ArturRios.Extensions.Tests.Mock;

namespace ArturRios.Extensions.Tests;

public class ObjectExtensionsTests
{
    [Fact]
    public async Task Should_CreateValidJsonStringContent()
    {
        var person = new Person { Name = "Alice", Age = 30, Home = new Address { Street = "Main", Number = 100 } };

        var content = person.ToJsonStringContent();

        Assert.NotNull(content);
        Assert.Equal(MediaTypeNames.Application.Json, content.Headers.ContentType!.MediaType);

        var json = await content.ReadAsStringAsync();

        Assert.Contains("\"Name\":\"Alice\"", json);
        Assert.Contains("\"Age\":30", json);
        Assert.Contains("\"Home\":", json);
    }

    [Fact]
    public void Should_MapOnlyNonNullProperties_ToDictionary()
    {
        var obj = new { Name = "Bob", Age = 25, Home = (Address?)null };

        var dict = obj.NonNullPropertiesToDictionary();

        Assert.NotNull(dict);
        Assert.True(dict.ContainsKey("Name"));
        Assert.True(dict.ContainsKey("Age"));
        Assert.False(dict.ContainsKey("Home"));

        Assert.Equal("Bob", dict["Name"]);
        Assert.Equal(25, dict["Age"]);
    }

    [Fact]
    public void Should_MapAllProperties_IncludingNulls_ToDictionary()
    {
        var obj = new { Name = "Carol", Age = 40, Home = (Address?)null };

        var dict = obj.PropertiesToDictionary();

        Assert.NotNull(dict);
        Assert.True(dict.ContainsKey("Name"));
        Assert.True(dict.ContainsKey("Age"));
        Assert.True(dict.ContainsKey("Home"));
        Assert.Equal("Carol", dict["Name"]);
        Assert.Equal(40, dict["Age"]);
        Assert.Null(dict["Home"]);
    }
}
