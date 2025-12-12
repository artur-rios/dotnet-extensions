using System.Text.RegularExpressions;

namespace ArturRios.Extensions.Tests;

public class ExceptionsExtensionsTests
{
    [Fact]
    public void Should_ReturnLogLine_WithExpectedFormatAndTraceId()
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
    public void Should_ContainExceptionTypeMessageAndStackTrace()
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
    public void Should_ThrowNullReference_When_ExceptionIsNull()
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
}
