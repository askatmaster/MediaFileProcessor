namespace MediaFileProcessor.Models.Enums;

/// <summary>
/// Encoder timebase when stream copying
/// </summary>
public enum EncoderTimebaseType
{
    /// <summary>
    /// Use the demuxer timebase.
    /// The time base is copied to the output encoder from the corresponding input demuxer. This is sometimes required to avoid non monotonically increasing timestamps when copying video streams with variable frame rate.
    /// </summary>
    Decoder = 0,
    
    /// <summary>
    /// Use the decoder timebase.
    /// The time base is copied to the output encoder from the corresponding input decoder.
    /// </summary>
    Demuxer = 1,
    
    /// <summary>
    /// Try to make the choice automatically, in order to generate a sane output.
    /// </summary>
    Auto = -1
}