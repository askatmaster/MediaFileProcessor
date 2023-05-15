namespace MediaFileProcessor.Models.Enums;

/// <summary>
/// Represents some common MOV format flags for FFmpeg.
/// </summary>
public enum MovFlagsType
{
    /// <summary>
    /// Move the moov atom to the beginning of the file.
    /// </summary>
    Faststart = 1,

    /// <summary>
    /// Each fragment starts with a keyframe.
    /// </summary>
    FragKeyframe = 2,

    /// <summary>
    /// Create an empty moov atom. Generally used with live streams.
    /// </summary>
    EmptyMoov = 3,

    /// <summary>
    /// Disable automatic generation of 'udta' metadata.
    /// </summary>
    DisableChpl = 4,

    /// <summary>
    /// Enable streaming and forbid seeking from the output.
    /// </summary>
    IsmlLive = 5
}