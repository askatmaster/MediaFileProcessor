namespace MediaFileProcessor.Models.Enums;

/// <summary>
/// Represents some common bitstream filters for FFmpeg.
/// </summary>
public enum VideoBitstreamFilter
{
    /// <summary>
    /// Convert H.264 bitstream from length-prefixed mode to start code mode.
    /// </summary>
    H264_Mp4ToAnnexB = 1,

    /// <summary>
    /// Add noise to the input video.
    /// </summary>
    Noise = 2,

    /// <summary>
    /// Remove extradata from the input video.
    /// </summary>
    RemoveExtra = 3
}