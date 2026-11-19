using System.Globalization;

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
    ///     The timestamp is UTC, written as <c>yyyy-MM-dd HH:mm:ss</c> with the invariant culture, so it reads
    ///     the same whatever the current culture's time separator or calendar.
    /// </remarks>
    public static string ToLogLine(this Exception exception, out Guid traceId)
    {
        traceId = Guid.NewGuid();

        var message = EscapeLineBreaks(exception.Message);
        var stackTrace = EscapeLineBreaks(exception.StackTrace ?? "No stack trace available");

        var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

        return
            $"{timestamp} | TraceId: {traceId} | Exception: {exception.GetType().Name} | Message: {message} | StackTrace: {stackTrace}";
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
