using System.Text.RegularExpressions;

namespace ArturRios.Extensions.Tests;

[Trait("Category", "Unit")]
public class ExceptionsExtensionsTests
{
    [Fact]
    public void GivenException_WhenCallingToLogLine_ThenReturnsLogLineWithExpectedFormatAndTraceId()
    {
        var ex = new InvalidOperationException("Invalid operation occurred");

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
        Assert.True(logLine.Contains("ExceptionsExtensionsTests.ThrowNested"),
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
        Assert.Contains("ExceptionsExtensionsTests.ThrowNested", logLine);
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
}
