namespace MediaFileProcessor.Models.Video;

/// <summary>
/// Represents the format of a video file
/// </summary>
public class Format
{
    /// <summary>
    /// Represents the duration of the video file in string format
    /// </summary>
    public string? Duration { get; set; }

    /// <summary>
    /// Represents the size of the video file in string format
    /// </summary>
    public string? Size { get; set; }

    /// <summary>
    /// Represents the bit rate of the video file in string format
    /// </summary>
    public string? BitRate { get; set; }

    /// <summary>
    /// Represents the tag information of the video file
    /// </summary>
    public Tag? Tag { get; set; }
}