namespace ArturRios.Extensions.Tests;

public class DateTimeExtensionsTests
{
    [Fact]
    public void Should_RemoveMilliseconds()
    {
        var original = new DateTime(2025, 12, 10, 15, 30, 45, 123, DateTimeKind.Unspecified);

        var result = original.RemoveMilliseconds();

        Assert.Equal(0, result.Millisecond);
        Assert.Equal(new DateTime(2025, 12, 10, 15, 30, 45, DateTimeKind.Unspecified), result);
    }

    [Fact]
    public void Should_RemoveMilliseconds_And_PreservesDateTimeKind()
    {
        var original = new DateTime(2025, 12, 10, 15, 30, 45, 999, DateTimeKind.Utc);

        var result = original.RemoveMilliseconds();

        Assert.Equal(DateTimeKind.Utc, result.Kind);
        Assert.Equal(new DateTime(2025, 12, 10, 15, 30, 45, DateTimeKind.Utc), result);
    }

    [Fact]
    public void Should_RemoveMilliseconds_And_DoNotChangeOtherComponents()
    {
        var original = new DateTime(1999, 2, 28, 23, 59, 59, 1, DateTimeKind.Unspecified);

        var result = original.RemoveMilliseconds();

        Assert.Equal(1999, result.Year);
        Assert.Equal(2, result.Month);
        Assert.Equal(28, result.Day);
        Assert.Equal(23, result.Hour);
        Assert.Equal(59, result.Minute);
        Assert.Equal(59, result.Second);
        Assert.Equal(0, result.Millisecond);
    }
}
