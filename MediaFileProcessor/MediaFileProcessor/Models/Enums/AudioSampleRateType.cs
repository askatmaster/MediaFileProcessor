namespace MediaFileProcessor.Models.Enums;

/// <summary>
/// Enumeration for the supported audio sample rate types.
/// </summary>
public enum AudioSampleRateType
{
    /// <summary>
    /// The audio sample rate type for 22050 Hz.
    /// </summary>
    /// <remarks>
    /// 22050 Hz is a standard sample rate for many audio applications,
    /// including voice recordings, podcasting, and speech recognition.
    /// </remarks>
    Hz22050 = 0,

    /// <summary>
    /// The audio sample rate type for 44100 Hz.
    /// </summary>
    /// <remarks>
    /// 44100 Hz is a standard sample rate for audio CDs and many other audio applications.
    /// </remarks>
    Hz44100 = 1,

    /// <summary>
    /// The audio sample rate type for 48000 Hz.
    /// </summary>
    /// <remarks>
    /// 48000 Hz is a common sample rate for professional audio applications,
    /// including digital audio workstations and video post-production.
    /// </remarks>
    Hz48000 = 2
}