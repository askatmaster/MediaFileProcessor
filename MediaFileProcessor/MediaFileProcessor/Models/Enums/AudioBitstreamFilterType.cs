namespace MediaFileProcessor.Models.Enums;

/// <summary>
/// Represents some common bitstream filters for audio in FFmpeg.
/// </summary>
public enum AudioBitstreamFilterType
{
    /// <summary>
    /// Converts AAC ADTS to an MPEG-4 Audio Specific Config.
    /// </summary>
    Aac_Adtstoasc = 1,

    /// <summary>
    /// Add noise to the input audio.
    /// </summary>
    Noise = 2,

    /// <summary>
    /// Remove extradata from the input audio.
    /// </summary>
    Remove_Extra = 3
}