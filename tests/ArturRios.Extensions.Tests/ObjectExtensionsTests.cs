using ArturRios.Extensions.Tests.Mock;

namespace ArturRios.Extensions.Tests;

public class ObjectExtensionsTests
{
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
