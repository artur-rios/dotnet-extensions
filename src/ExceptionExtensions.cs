namespace ArturRios.Extensions;

/// <summary>
/// Provides extension methods for Exceptions, including utilities to format an exception into a log line.
/// </summary>
public static class ExceptionExtensions
{
    /// <summary>
    /// Formats exception details into a single log line and outputs a generated trace identifier.
    /// </summary>
    /// <param name="exception">The exception to format.</param>
    /// <param name="traceId">The generated trace identifier associated with this log entry.</param>
    /// <returns>A single-line string containing timestamp, trace id, exception type, message and stack trace.</returns>
    public static string ToLogLine(this Exception exception, out Guid traceId)
    {
        traceId = Guid.NewGuid();

        return
            $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} | TraceId: {traceId} | Exception: {exception.GetType().Name} | Message: {exception.Message} | StackTrace: {exception.StackTrace ?? "No stack trace available"}";
    }
}
