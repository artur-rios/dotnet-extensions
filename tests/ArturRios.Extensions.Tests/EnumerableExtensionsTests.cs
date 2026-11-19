using System.Collections;
using System.Globalization;
using ArturRios.Extensions.Tests.Mock;

namespace ArturRios.Extensions.Tests;

[Trait("Category", "Unit")]
[Collection(ConsoleCollection.Name)]
public class EnumerableExtensionsTests
{
    [Theory]
    [ClassData(typeof(EmptyCollections))]
    public void GivenEmptyCollections_WhenCallingIsEmpty_ThenReturnsTrue(IEnumerable? collection) => Assert.True(collection.IsEmpty());

    [Theory]
    [ClassData(typeof(NotEmptyCollections))]
    public void GivenNotEmptyCollections_WhenCallingIsEmpty_ThenReturnsFalse(IEnumerable collection) => Assert.False(collection.IsEmpty());

    [Theory]
    [ClassData(typeof(EmptyCollections))]
    public void GivenEmptyCollections_WhenCallingIsNotEmpty_ThenReturnsFalse(IEnumerable? collection) => Assert.False(collection.IsNotEmpty());

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

    [Fact]
    public void GivenCollections_WhenCallingIsEmpty_ThenTheirCountDecides()
    {
        Assert.True(Array.Empty<int>().IsEmpty());
        Assert.True(new List<string>().IsEmpty());
        Assert.True(new Dictionary<int, int>().IsEmpty());
        Assert.False(new[] { 0 }.IsEmpty());
        Assert.False(new ArrayList { null }.IsEmpty());
        Assert.True(new[] { 0 }.IsNotEmpty());
    }

    [Fact]
    public void GivenALazySequence_WhenCallingIsEmpty_ThenOnlyTheFirstElementIsReadAndTheEnumeratorIsDisposed()
    {
        var sequence = new CountingSequence(3);

        Assert.False(sequence.IsEmpty());
        Assert.Equal(1, sequence.Reads);
        Assert.True(sequence.Disposed);

        Assert.True(new CountingSequence(0).IsEmpty());
    }

    [Fact]
    public void GivenAnIteratorMethod_WhenCallingIsEmptyAndIsNotEmpty_ThenTheyAnswerFromTheFirstElement()
    {
        Assert.True(Yield().IsEmpty());
        Assert.True(Yield(1).IsNotEmpty());

        return;

        static IEnumerable<int> Yield(params int[] values)
        {
            foreach (var value in values)
            {
                yield return value;
            }
        }
    }

    [Fact]
    public void GivenNullItems_WhenPrintingContents_ThenEachIsPrintedAsNull()
    {
        IEnumerable collection = new object?[] { null, 1, null };

        var lines = CaptureConsole(collection.PrintContents).Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        Assert.Equal(["null", "1", "null"], lines);
    }

    [Fact]
    public void GivenAnItemWithNoReadableProperties_WhenPrintingContents_ThenItsToStringIsPrinted()
    {
        IEnumerable collection = new[] { new NoProperties() };

        Assert.Equal($"no properties{Environment.NewLine}", CaptureConsole(collection.PrintContents));
    }

    [Fact]
    public void GivenAnEmptyEnumerable_WhenPrintingContents_ThenNothingIsPrinted()
    {
        Assert.Equal(string.Empty, CaptureConsole(Array.Empty<int>().PrintContents));
    }

    [Fact]
    public void GivenEverySingleValueType_WhenPrintingContents_ThenEachItemIsPrintedAsItselfInTheCurrentCulture()
    {
        // The values print through ToString, so the culture is pinned: pt-BR, as on the owner's machine.
        using var culture = new CultureScope(CultureInfo.GetCultureInfo("pt-BR"));

        var at = new DateTime(2026, 1, 2, 3, 4, 5, DateTimeKind.Utc);
        object[] values =
        [
            'c', true, 1.5, 2.5f, 3.25m, (byte)4, 5L, "text", at, new DateTimeOffset(at), DateOnly.FromDateTime(at),
            TimeOnly.FromDateTime(at), TimeSpan.FromSeconds(90), Guid.Empty, DayOfWeek.Monday
        ];

        var lines = CaptureConsole(((IEnumerable)values).PrintContents)
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        Assert.Equal(values.Select(value => value.ToString()), lines);
        Assert.Contains("1,5", lines);
        Assert.Contains("3,25", lines);
    }

    private sealed class NoProperties
    {
        public override string ToString() => "no properties";
    }

    /// <summary>
    /// A non-generic sequence that counts the elements read from it and whether its enumerator was disposed.
    /// </summary>
    private sealed class CountingSequence(int count) : IEnumerable
    {
        public int Reads { get; private set; }

        public bool Disposed { get; private set; }

        public IEnumerator GetEnumerator() => new Enumerator(this, count);

        private sealed class Enumerator(CountingSequence owner, int count) : IEnumerator, IDisposable
        {
            private int _index = -1;

            public object Current => _index;

            public bool MoveNext()
            {
                if (_index + 1 >= count)
                {
                    return false;
                }

                _index++;
                owner.Reads++;

                return true;
            }

            public void Reset() => _index = -1;

            public void Dispose() => owner.Disposed = true;
        }
    }
}
