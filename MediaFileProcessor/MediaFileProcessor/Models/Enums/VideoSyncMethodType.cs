namespace MediaFileProcessor.Models.Enums;

/// <summary>
/// Enum representing video sync values.
/// </summary>
public enum VideoSyncMethodType
{
    /// <summary>
    /// Each frame is passed with its timestamp from the demuxer to the muxer.
    /// </summary>
    Passthrough,

    /// <summary>
    /// Frames are passed through as is.
    /// </summary>
    Cfr,

    /// <summary>
    /// Frames are passed through only if timestamps are reset.
    /// </summary>
    Vfr
}