namespace ArturRios.Extensions;

/// <summary>
///     Provides extension methods for Exceptions, including utilities to format an exception into a log line.
/// </summary>
public static class ExceptionExtensions
{
    /// <summary>
    ///     Formats exception details into a single log line and outputs a generated trace identifier.
    /// </summary>
    /// <param name="exception">The exception to format.</param>
    /// <param name="traceId">The generated trace identifier associated with this log entry.</param>
    /// <returns>A single-line string containing timestamp, trace id, exception type, message and stack trace.</returns>
    /// <remarks>
    ///     Line breaks in the message and the stack trace are written as the two-character escape <c>\n</c>,
    ///     so the result really is one line. A stack trace spans one line per frame, and a message can carry
    ///     line breaks taken from untrusted input; written raw, either one split the entry across several
    ///     lines of a line-oriented log, and a crafted message could forge entries that look genuine.
    /// </remarks>
    public static string ToLogLine(this Exception exception, out Guid traceId)
    {
        traceId = Guid.NewGuid();

        var message = EscapeLineBreaks(exception.Message);
        var stackTrace = EscapeLineBreaks(exception.StackTrace ?? "No stack trace available");

        return
            $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} | TraceId: {traceId} | Exception: {exception.GetType().Name} | Message: {message} | StackTrace: {stackTrace}";
    }

    /// <summary>
    ///     Replaces every line break — CR LF, LF, CR, and the Unicode line and paragraph separators — with
    ///     the literal escape <c>\n</c>.
    /// </summary>
    private static string EscapeLineBreaks(string value) =>
        value.Replace("\r\n", "\\n")
            .Replace('\r', '\n')
            .Replace('\u2028', '\n')
            .Replace('\u2029', '\n')
            .Replace("\n", "\\n");
}
