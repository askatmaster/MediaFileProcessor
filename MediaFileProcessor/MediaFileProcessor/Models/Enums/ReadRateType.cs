namespace MediaFileProcessor.Models.Enums;

/// <summary>
/// Represents some common read rates for FFmpeg.
/// </summary>
public enum ReadRateType
{
    /// <summary>
    /// 1 KB per second.
    /// </summary>
    Kb1PerSec = 1024,

    /// <summary>
    /// 10 KB per second.
    /// </summary>
    Kb10PerSec = 10240,

    /// <summary>
    /// 100 KB per second.
    /// </summary>
    Kb100PerSec = 102400,

    /// <summary>
    /// 1 MB per second.
    /// </summary>
    Mb1PerSec = 1048576,

    /// <summary>
    /// 10 MB per second.
    /// </summary>
    Mb10PerSec = 10485760
}