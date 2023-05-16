namespace MediaFileProcessor.Models.Enums;

/// <summary>
/// Represents some common DPI values for FFmpeg.
/// </summary>
public enum DpiValueType
{
    /// <summary>
    /// 72 DPI, common for web images.
    /// </summary>
    Dpi72 = 72,

    /// <summary>
    /// 96 DPI, common for Windows and other desktop screens.
    /// </summary>
    Dpi96 = 96,

    /// <summary>
    /// 150 DPI, higher resolution for print images.
    /// </summary>
    Dpi150 = 150,

    /// <summary>
    /// 300 DPI, high resolution for print images.
    /// </summary>
    Dpi300 = 300,

    /// <summary>
    /// 600 DPI, very high resolution for print images.
    /// </summary>
    Dpi600 = 600
}