using ArturRios.Extensions.Tests.Mock;

namespace ArturRios.Extensions.Tests;

[Trait("Category", "Unit")]
public class ObjectExtensionsTests
{
    [Fact]
    public void GivenObjectWithNullProperties_WhenCallingNonNullPropertiesToDictionary_ThenMapsOnlyNonNullProperties()
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
    public void GivenObjectWithNullProperties_WhenCallingPropertiesToDictionary_ThenMapsAllPropertiesIncludingNulls()
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

    [Fact]
    public void GivenObjectWithAnIndexer_WhenCallingPropertiesToDictionary_ThenTheIndexerIsSkipped()
    {
        var dict = new WithIndexer().PropertiesToDictionary();

        Assert.Equal(["Name"], dict.Keys);
    }

    [Fact]
    public void GivenObjectWithAnIndexer_WhenCallingNonNullPropertiesToDictionary_ThenTheIndexerIsSkipped()
    {
        var dict = new WithIndexer().NonNullPropertiesToDictionary();

        Assert.Equal(["Name"], dict.Keys);
    }

    [Fact]
    public void GivenObjectWithStaticAndWriteOnlyProperties_WhenCallingPropertiesToDictionary_ThenOnlyInstanceStateIsMapped()
    {
        var dict = new WithStaticAndWriteOnly().PropertiesToDictionary();

        Assert.Equal(["Name"], dict.Keys);
    }

    private sealed class WithIndexer
    {
        public string Name { get; set; } = "indexed";

        public int this[int index] => index;
    }

    private sealed class WithStaticAndWriteOnly
    {
        public static int Instances { get; set; } = 3;

        public string Name { get; set; } = "plain";

        public string Secret
        {
            set => _ = value;
        }
    }
}
