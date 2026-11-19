namespace ArturRios.Extensions.Tests;

[Trait("Category", "Unit")]
public class DateTimeExtensionsTests
{
    [Fact]
    public void GivenDateTimeWithMilliseconds_WhenRemovingMilliseconds_ThenMillisecondsAreZero()
    {
        var original = new DateTime(2025, 12, 10, 15, 30, 45, 123, DateTimeKind.Unspecified);

        var result = original.RemoveMilliseconds();

        Assert.Equal(0, result.Millisecond);
        Assert.Equal(new DateTime(2025, 12, 10, 15, 30, 45, DateTimeKind.Unspecified), result);
    }

    [Fact]
    public void GivenUtcDateTimeWithMilliseconds_WhenRemovingMilliseconds_ThenPreservesDateTimeKind()
    {
        var original = new DateTime(2025, 12, 10, 15, 30, 45, 999, DateTimeKind.Utc);

        var result = original.RemoveMilliseconds();

        Assert.Equal(DateTimeKind.Utc, result.Kind);
        Assert.Equal(new DateTime(2025, 12, 10, 15, 30, 45, DateTimeKind.Utc), result);
    }

    [Fact]
    public void GivenDateTimeWithMilliseconds_WhenRemovingMilliseconds_ThenDoesNotChangeOtherComponents()
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

    [Fact]
    public void GivenADateTimeWithSubSecondTicks_WhenRemovingMilliseconds_ThenEveryTickBelowASecondIsDropped()
    {
        var original = new DateTime(2026, 8, 24, 13, 45, 30, DateTimeKind.Utc).AddTicks(9_876_543);

        var truncated = original.RemoveMilliseconds();

        Assert.Equal(new DateTime(2026, 8, 24, 13, 45, 30, DateTimeKind.Utc), truncated);
        Assert.Equal(0, truncated.Millisecond);
        Assert.Equal(0, truncated.Ticks % TimeSpan.TicksPerSecond);
        Assert.Equal(DateTimeKind.Utc, truncated.Kind);
    }

    [Theory]
    [InlineData(DateTimeKind.Utc)]
    [InlineData(DateTimeKind.Local)]
    [InlineData(DateTimeKind.Unspecified)]
    public void GivenAnyKind_WhenRemovingMilliseconds_ThenTheKindIsPreserved(DateTimeKind kind)
    {
        var original = new DateTime(2026, 8, 24, 13, 45, 30, 999, kind);

        Assert.Equal(kind, original.RemoveMilliseconds().Kind);
    }
}
