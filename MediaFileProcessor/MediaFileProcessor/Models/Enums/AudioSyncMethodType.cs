namespace MediaFileProcessor.Models.Enums;

/// <summary>
/// Enum representing audio sync values.
/// </summary>
public enum AudioSyncMethodType
{
    /// <summary>
    /// Audio is stretched/squeezed to match the timestamps.
    /// </summary>
    Stretch,

    /// <summary>
    /// Audio is passed through as is.
    /// </summary>
    Passthrough
}