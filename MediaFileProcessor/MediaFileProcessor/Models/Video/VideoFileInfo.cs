namespace MediaFileProcessor.Models.Video;

/// <summary>
/// Detailed information about the video file
/// </summary>
public class VideoFileInfo
{
    /// <summary>
    /// Represents the information of a stream in a media file
    /// </summary>
    public StreamInfo[]? Streams { get; set; }

    /// <summary>
    /// Represents the format of a video file
    /// </summary>
    public Format? Format { get; set; }
}