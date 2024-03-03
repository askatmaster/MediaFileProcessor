namespace MediaFileProcessor.Extensions;

/// <summary>
/// Provides extension methods for handling FFmpeg related operations.
/// </summary>
public static class FFmpegExtensions
{
    /// <summary>
    /// Converts a TimeSpan duration to FFmpeg format.
    /// </summary>
    /// <param name="duration">The TimeSpan duration to convert.</param>
    /// <returns>The FFmpeg-formatted string representation of the duration.</returns>
    public static string ToFfmpegDuration(this TimeSpan duration)
    {
        var isNegative = duration.TotalSeconds < 0;
        var sign = isNegative ? "-" : "";
        var absDuration = isNegative ? -duration : duration;
        return $"{sign}{(int)absDuration.TotalHours}:{absDuration.Minutes:00}:{absDuration.Seconds:00}.{absDuration.Milliseconds:000}";
    }
}