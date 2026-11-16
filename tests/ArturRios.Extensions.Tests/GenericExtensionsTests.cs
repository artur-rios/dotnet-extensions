using ArturRios.Extensions.Tests.Mock;

namespace ArturRios.Extensions.Tests;

[Trait("Category", "Unit")]
public class GenericExtensionsTests
{
    [Fact]
    public void GivenNullSource_WhenCloning_ThenReturnsNull()
    {
        Person? original = null;

        var clone = original.Clone();

        Assert.Null(clone);
    }

    [Fact]
    public void GivenValueType_WhenCloning_ThenClonesValueType()
    {
        const int original = 42;

        var clone = original.Clone();

        Assert.Equal(original, clone);
    }

    [Fact]
    public void GivenReferenceType_WhenCloning_ThenDeepClonesWithEqualValuesAndIndependentInstances()
    {
        var original = new Person { Name = "Alice", Age = 30, Home = new Address { Street = "Main", Number = 100 } };

        var clone = original.Clone();

        Assert.NotNull(clone);
        Assert.Equal(original.Name, clone.Name);
        Assert.Equal(original.Age, clone.Age);
        Assert.NotNull(clone.Home);
        Assert.Equal(original.Home.Street, clone.Home.Street);
        Assert.Equal(original.Home.Number, clone.Home.Number);

        clone.Name = "Bob";
        clone.Home.Street = "Second";
        clone.Home.Number = 200;

        Assert.Equal("Alice", original.Name);
        Assert.Equal("Main", original.Home.Street);
        Assert.Equal(100, original.Home.Number);
    }

    [Fact]
    public void GivenAGraphWithAReferenceCycle_WhenCloning_ThenTheFailureIsReported()
    {
        var first = new Node();
        var second = new Node { Other = first };

        first.Other = second;

        Assert.Throws<Newtonsoft.Json.JsonSerializationException>(() => first.Clone());
    }

    private sealed class Node
    {
        public Node? Other { get; set; }
    }

    [Fact]
    public void GivenACollection_WhenCloning_ThenTheCopyIsANewCollectionOfNewItems()
    {
        List<Person> original = [new() { Name = "Ada" }, new() { Name = "Grace" }];

        var clone = original.Clone();

        Assert.NotNull(clone);
        Assert.NotSame(original, clone);
        Assert.Equal(["Ada", "Grace"], clone.Select(person => person.Name));
        Assert.All(clone.Zip(original), pair => Assert.NotSame(pair.Second, pair.First));
    }

    [Fact]
    public void GivenAString_WhenCloning_ThenAnEqualStringComesBack()
    {
        Assert.Equal("text", "text".Clone<string>());
    }
}
