namespace MediaFileProcessor.Models.Enums;

/// <summary>
/// Enum representing subtitle codec values.
/// </summary>
public enum SubtitleCodecType
{
    /// <summary>
    /// Subtitle codec used in MP4 files.
    /// </summary>
    Mov_Text,

    /// <summary>
    /// Codec for SubRip subtitles (usually with .srt extension).
    /// </summary>
    Srt,

    /// <summary>
    /// Codec for Advanced SubStation Alpha subtitles (usually with .ass extension).
    /// </summary>
    Ass,

    /// <summary>
    /// Codec for SubStation Alpha subtitles (usually with .ssa extension).
    /// </summary>
    Ssa,

    /// <summary>
    /// Codec for WebVTT subtitles (usually with .vtt extension).
    /// </summary>
    WebVTT,

    /// <summary>
    /// Codec for DVD subtitles.
    /// </summary>
    Dvd_Subtitle,

    /// <summary>
    /// Copy subtitles without changes.
    /// </summary>
    Copy
}