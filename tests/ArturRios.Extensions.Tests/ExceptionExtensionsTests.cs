using System.Globalization;
using System.Text.RegularExpressions;
using ArturRios.Extensions.Tests.Mock;

namespace ArturRios.Extensions.Tests;

[Trait("Category", "Unit")]
public class ExceptionExtensionsTests
{
    [Fact]
    public void GivenException_WhenCallingToLogLine_ThenReturnsLogLineWithExpectedFormatAndTraceId()
    {
        var ex = new InvalidOperationException("Invalid operation occurred");

        // The timestamp is formatted in the current culture: pinned, so the pattern holds on any machine.
        using var culture = new CultureScope(CultureInfo.InvariantCulture);

        var logLine = ex.ToLogLine(out var traceId);

        Assert.NotEqual(Guid.Empty, traceId);
        Assert.False(string.IsNullOrWhiteSpace(logLine));

        // Expect: "yyyy-MM-dd HH:mm:ss | TraceId: <guid> | Exception: <Type> | Message: <Message> | StackTrace: <stack>"
        var pattern = @"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2} \| TraceId: " + Regex.Escape(traceId.ToString()) +
                      @" \| Exception: InvalidOperationException \| Message: Invalid operation occurred \| StackTrace: .+";

        Assert.Matches(new Regex(pattern, RegexOptions.Singleline), logLine);
    }

    [Fact]
    public void GivenNestedException_WhenCallingToLogLine_ThenContainsExceptionTypeMessageAndStackTrace()
    {
        Exception? ex = null;

        try
        {
            ThrowNested();
        }
        catch (Exception caught)
        {
            ex = caught;
        }

        Assert.NotNull(ex);
        var logLine = ex.ToLogLine(out var traceId);

        Assert.NotEqual(Guid.Empty, traceId);
        Assert.Contains($"Exception: {ex.GetType().Name}", logLine);
        Assert.Contains($"Message: {ex.Message}", logLine);
        Assert.Contains("StackTrace:", logLine);
        Assert.True(logLine.Contains("ExceptionExtensionsTests.ThrowNested"),
            "Log line should include method name from stack trace");
    }

    [Fact]
    public void GivenNullException_WhenCallingToLogLine_ThenThrowsNullReferenceException()
    {
        Exception? ex = null;

        Assert.Throws<NullReferenceException>(() => _ = ex!.ToLogLine(out _));
    }

    private static void ThrowNested()
    {
        try
        {
            Inner();
        }
        catch (Exception e)
        {
            throw new InvalidOperationException("Wrapped", e);
        }

        return;

        void Inner() => throw new ArgumentException("Bad arg");
    }

    [Fact]
    public void GivenExceptionWithAMultiFrameStackTrace_WhenCallingToLogLine_ThenTheResultIsASingleLine()
    {
        Exception? ex = null;

        try
        {
            ThrowNested();
        }
        catch (Exception caught)
        {
            ex = caught;
        }

        Assert.NotNull(ex);
        Assert.Contains('\n', ex.StackTrace!);

        var logLine = ex.ToLogLine(out _);

        Assert.DoesNotContain('\n', logLine);
        Assert.DoesNotContain('\r', logLine);
        Assert.Contains("ExceptionExtensionsTests.ThrowNested", logLine);
    }

    [Theory]
    [InlineData("first\nsecond")]
    [InlineData("first\r\nsecond")]
    [InlineData("first\rsecond")]
    [InlineData("first\u2028second")]
    public void GivenAMessageWithALineBreak_WhenCallingToLogLine_ThenTheBreakIsEscapedRatherThanStartingANewEntry(string message)
    {
        var logLine = new InvalidOperationException(message).ToLogLine(out _);

        Assert.Contains("Message: first\\nsecond | StackTrace:", logLine);
        Assert.Single(logLine.Split(['\n', '\r', '\u2028', '\u2029']));
    }

    [Fact]
    public void GivenAnExceptionThatWasNeverThrown_WhenCallingToLogLine_ThenTheStackTraceSaysSo()
    {
        var logLine = new InvalidOperationException("never thrown").ToLogLine(out _);

        Assert.EndsWith(" | StackTrace: No stack trace available", logLine);
    }

    [Fact]
    public void GivenTheSameException_WhenCallingToLogLineTwice_ThenEachEntryGetsItsOwnTraceId()
    {
        var exception = new InvalidOperationException("twice");

        var first = exception.ToLogLine(out var firstTraceId);
        var second = exception.ToLogLine(out var secondTraceId);

        Assert.NotEqual(firstTraceId, secondTraceId);
        Assert.Contains($"TraceId: {firstTraceId} ", first);
        Assert.Contains($"TraceId: {secondTraceId} ", second);
    }

    [Fact]
    public void GivenAnException_WhenCallingToLogLine_ThenTheTimestampIsTheCurrentUtcTimeToTheSecond()
    {
        using var culture = new CultureScope(CultureInfo.InvariantCulture);

        var before = DateTime.UtcNow.AddSeconds(-1);

        var logLine = new InvalidOperationException("now").ToLogLine(out _);

        var after = DateTime.UtcNow.AddSeconds(1);
        var timestamp = DateTime.ParseExact(logLine[..19], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture,
            DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);

        Assert.InRange(timestamp, before, after);
    }

    [Fact]
    public void GivenAMessageWithAParagraphSeparator_WhenCallingToLogLine_ThenTheBreakIsEscaped()
    {
        var logLine = new InvalidOperationException("first\u2029second").ToLogLine(out _);

        Assert.Contains("Message: first\\nsecond | StackTrace:", logLine);
    }

    [Fact]
    public void GivenAMessageWithSeveralBreaks_WhenCallingToLogLine_ThenEachBreakBecomesOneEscape()
    {
        var logLine = new InvalidOperationException("a\r\n\r\nb\n\rc").ToLogLine(out _);

        Assert.Contains("Message: a\\n\\nb\\n\\nc | StackTrace:", logLine);
    }
}
