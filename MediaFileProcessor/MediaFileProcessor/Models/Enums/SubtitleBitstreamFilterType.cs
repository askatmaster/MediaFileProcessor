namespace MediaFileProcessor.Models.Enums;

/// <summary>
/// Represents some common bitstream filters for subtitles in FFmpeg.
/// </summary>
public enum SubtitleBitstreamFilterType
{
    /// <summary>
    /// Convert MOV text to srt.
    /// </summary>
    Mov2TextSub = 1,

    /// <summary>
    /// Convert text subtitles to MOV text.
    /// </summary>
    Text2MovSub = 2
}