using System.Collections;
using ArturRios.Extensions.Tests.Mock;

namespace ArturRios.Extensions.Tests;

public class EnumerableExtensionsTests
{
    [Theory]
    [ClassData(typeof(EmptyCollections))]
    public void Should_ReturnTrue_ForEmptyCollections(IEnumerable collection) => Assert.True(collection.IsEmpty());

    [Theory]
    [ClassData(typeof(NotEmptyCollections))]
    public void Should_ReturnFalse_FotNotEmptyCollections(IEnumerable collection) => Assert.False(collection.IsEmpty());

    [Theory]
    [ClassData(typeof(EmptyCollections))]
    public void Should_ReturnFalse_ForEmptyCollections(IEnumerable collection) => Assert.False(collection.IsNotEmpty());

    [Theory]
    [ClassData(typeof(NotEmptyCollections))]
    public void Should_ReturnTrue_ForNotEmptyCollections(IEnumerable collection) =>
        Assert.True(collection.IsNotEmpty());

    [Fact]
    public void Should_PrintMessage_ForNullEnumerable()
    {
        IEnumerable? collection = null;

        using var sw = new StringWriter();
        var originalOut = Console.Out;

        Console.SetOut(sw);

        try
        {
            collection.PrintContents();
        }
        finally
        {
            Console.SetOut(originalOut);
        }

        var output = sw.ToString().Trim();

        Assert.Equal("Enumerable is null", output);
    }

    [Fact]
    public void Should_PrintPrimitiveItems_ForSimpleEnumerable()
    {
        IEnumerable collection = new[] { 1, 2, 3 };

        using var sw = new StringWriter();
        var originalOut = Console.Out;

        Console.SetOut(sw);

        try
        {
            collection.PrintContents();
        }
        finally
        {
            Console.SetOut(originalOut);
        }

        var output = sw.ToString();

        Assert.Contains("1", output);
        Assert.Contains("2", output);
        Assert.Contains("3", output);
    }

    [Fact]
    public void Should_PrintProperties_ForComplexObjects()
    {
        var people = new List<Person> { new() { Name = "John", Age = 30 }, new() { Name = "Jane", Age = 25 } };

        using var sw = new StringWriter();
        var originalOut = Console.Out;

        Console.SetOut(sw);

        try
        {
            people.PrintContents();
        }
        finally
        {
            Console.SetOut(originalOut);
        }

        var output = sw.ToString();

        Assert.Contains("Name: John", output);
        Assert.Contains("Age: 30", output);
        Assert.Contains("Name: Jane", output);
        Assert.Contains("Age: 25", output);
    }
}
