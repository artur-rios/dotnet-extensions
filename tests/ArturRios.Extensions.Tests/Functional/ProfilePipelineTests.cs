using System.Text.Json;
using ArturRios.Extensions.Tests.Mock;

namespace ArturRios.Extensions.Tests.Functional;

/// <summary>
/// Runs the extensions together over a real JSON file on disk and real console output, the way a caller
/// would: read the file, bind it, clone it, project it to a dictionary, validate it and render it. The unit
/// tests cover each extension on its own.
/// </summary>
[Trait("Category", "Functional")]
[Collection(ConsoleCollection.Name)]
public sealed class ProfilePipelineTests : IDisposable
{
    private readonly string _directory =
        Path.Combine(Path.GetTempPath(), "arturrios-extensions-" + Guid.NewGuid().ToString("N"));

    public ProfilePipelineTests() => Directory.CreateDirectory(_directory);

    public void Dispose() => Directory.Delete(_directory, recursive: true);

    [Fact]
    public void GivenAProfileOnDisk_WhenRunTheWholePipeline_ThenEveryStepAgreesOnTheSameData()
    {
        var path = WriteProfile("""
            {
              "Name": "Ada Lovelace",
              "Age": 36,
              "Home": { "Street": "Great Marlborough Street", "Number": 12 }
            }
            """);

        var profile = File.ReadAllText(path).ParseToObjectOrDefault<Person>();

        Assert.NotNull(profile);

        var clone = profile!.Clone();

        Assert.NotNull(clone);
        Assert.NotSame(profile, clone);
        Assert.Equal(profile.Name, clone!.Name);
        Assert.Equal(profile.Home.Street, clone.Home.Street);

        clone.Name = "Someone Else";

        Assert.Equal("Ada Lovelace", profile.Name);

        var properties = clone.PropertiesToDictionary();

        Assert.Equal(new[] { "Name", "Age", "Home" }, properties.Keys);
        Assert.Equal(36, properties["Age"]);
    }

    [Fact]
    public void GivenAProfileWithMissingMembers_WhenBinding_ThenTheAbsentMembersAreLeftAtTheirDefaults()
    {
        var path = WriteProfile("""{ "Name": "Partial" }""");

        var profile = File.ReadAllText(path).ParseToObjectOrDefault<Person>();

        Assert.NotNull(profile);
        Assert.Equal("Partial", profile!.Name);
        Assert.Equal(0, profile.Age);

        var nonNull = profile.NonNullPropertiesToDictionary();

        Assert.Contains("Name", nonNull.Keys);
        Assert.Contains("Age", nonNull.Keys);
    }

    [Fact]
    public void GivenAFileThatIsNotJson_WhenBinding_ThenNullComesBackInsteadOfAnException()
    {
        var path = WriteProfile("this is not json at all");

        Assert.Null(File.ReadAllText(path).ParseToObjectOrDefault<Person>());
    }

    [Fact]
    public void GivenAnEmptyFile_WhenBinding_ThenNullComesBack()
    {
        var path = WriteProfile(string.Empty);

        Assert.Null(File.ReadAllText(path).ParseToObjectOrDefault<Person>());
    }

    [Fact]
    public void GivenACollectionOfProfiles_WhenPrintingContents_ThenEveryPropertyReachesTheConsole()
    {
        var people = new List<Person>
        {
            new() { Name = "Ada", Age = 36 },
            new() { Name = "Grace", Age = 45 }
        };

        var original = Console.Out;
        using var writer = new StringWriter();

        Console.SetOut(writer);

        try
        {
            people.PrintContents();
        }
        finally
        {
            Console.SetOut(original);
        }

        var output = writer.ToString();

        Assert.Contains("Name: Ada", output);
        Assert.Contains("Age: 36", output);
        Assert.Contains("Name: Grace", output);
        Assert.Contains("Age: 45", output);
    }

    [Fact]
    public void GivenProfilesOnDisk_WhenSummarising_ThenTheJoinedSummaryReadsAsExpected()
    {
        var names = new[] { "Ada", "Grace", "Katherine" };

        Assert.Equal("Ada, Grace, Katherine", names.JoinWith());
        Assert.Equal("Ada | Grace | Katherine", names.JoinWith(" | "));
    }

    [Fact]
    public void GivenARoundTrippedProfile_WhenComparedAgainstTheSourceFile_ThenTheJsonIsEquivalent()
    {
        var path = WriteProfile("""{"Name":"Ada","Age":36,"Home":{"Street":"Marlborough","Number":12}}""");

        var profile = File.ReadAllText(path).ParseToObjectOrDefault<Person>();

        Assert.NotNull(profile);

        var rewritten = JsonSerializer.Serialize(profile);
        var reread = rewritten.ParseToObjectOrDefault<Person>();

        Assert.NotNull(reread);
        Assert.Equal(profile!.Name, reread!.Name);
        Assert.Equal(profile.Age, reread.Age);
        Assert.Equal(profile.Home.Street, reread.Home.Street);
    }

    private string WriteProfile(string json)
    {
        var path = Path.Combine(_directory, Guid.NewGuid().ToString("N") + ".json");

        File.WriteAllText(path, json);

        return path;
    }
}
