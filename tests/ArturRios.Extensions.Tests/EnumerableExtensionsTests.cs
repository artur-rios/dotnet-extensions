using System.Collections;
using ArturRios.Extensions.Tests.Mock;

namespace ArturRios.Extensions.Tests;

[Trait("Category", "Unit")]
[Collection(ConsoleCollection.Name)]
public class EnumerableExtensionsTests
{
    [Theory]
    [ClassData(typeof(EmptyCollections))]
    public void GivenEmptyCollections_WhenCallingIsEmpty_ThenReturnsTrue(IEnumerable collection) => Assert.True(collection.IsEmpty());

    [Theory]
    [ClassData(typeof(NotEmptyCollections))]
    public void GivenNotEmptyCollections_WhenCallingIsEmpty_ThenReturnsFalse(IEnumerable collection) => Assert.False(collection.IsEmpty());

    [Theory]
    [ClassData(typeof(EmptyCollections))]
    public void GivenEmptyCollections_WhenCallingIsNotEmpty_ThenReturnsFalse(IEnumerable collection) => Assert.False(collection.IsNotEmpty());

    [Theory]
    [ClassData(typeof(NotEmptyCollections))]
    public void GivenNotEmptyCollections_WhenCallingIsNotEmpty_ThenReturnsTrue(IEnumerable collection) =>
        Assert.True(collection.IsNotEmpty());

    [Fact]
    public void GivenNullEnumerable_WhenPrintingContents_ThenPrintsMessage()
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
    public void GivenSimpleEnumerable_WhenPrintingContents_ThenPrintsPrimitiveItems()
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
    public void GivenComplexObjects_WhenPrintingContents_ThenPrintsProperties()
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

    [Fact]
    public void GivenSingleValueItems_WhenPrintingContents_ThenEachItemIsPrintedAsItself()
    {
        var id = Guid.Parse("0f8fad5b-d9cb-469f-a165-70867728950e");
        var at = new DateTime(2026, 1, 2, 3, 4, 5, DateTimeKind.Utc);
        IEnumerable collection = new object[] { id, DayOfWeek.Friday, at, TimeSpan.FromMinutes(90) };

        var lines = CaptureConsole(collection.PrintContents)
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        Assert.Equal([id.ToString(), "Friday", at.ToString(), TimeSpan.FromMinutes(90).ToString()], lines);
    }

    [Fact]
    public void GivenItemsWithStaticPropertiesOrIndexers_WhenPrintingContents_ThenOnlyInstancePropertiesArePrinted()
    {
        IEnumerable collection = new[] { new Indexed() };

        var output = CaptureConsole(collection.PrintContents);

        Assert.Equal($"Name: indexed{Environment.NewLine}", output);
    }

    private static string CaptureConsole(Action action)
    {
        using var sw = new StringWriter();
        var originalOut = Console.Out;

        Console.SetOut(sw);

        try
        {
            action();
        }
        finally
        {
            Console.SetOut(originalOut);
        }

        return sw.ToString();
    }

    private sealed class Indexed
    {
        public static int Created { get; set; } = 1;

        public string Name { get; set; } = "indexed";

        public int this[int index] => index;
    }
}
