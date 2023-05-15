namespace MediaFileProcessor.Models.Enums;

/// <summary>
/// Represents the strictness levels for FFmpeg.
/// </summary>
public enum FFmpegStrictness
{
    /// <summary>
    /// Allow all FFmpeg features, including those considered experimental.
    /// </summary>
    Experimental = -2,

    /// <summary>
    /// Allow everything except for unofficial encoders, decoders, muxers, demuxers, filters, etc.
    /// </summary>
    Unofficial = -1,

    /// <summary>
    /// This is the default value. It allows all official FFmpeg features.
    /// </summary>
    Normal = 0,

    /// <summary>
    /// Only allow standards-compliant features. This might disable some features that work but are not fully standards-compliant.
    /// </summary>
    Strict = 1,

    /// <summary>
    /// Similar to Strict, but even more restrictive.
    /// </summary>
    Very = 2
}