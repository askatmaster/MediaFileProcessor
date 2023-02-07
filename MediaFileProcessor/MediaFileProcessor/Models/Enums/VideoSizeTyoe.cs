namespace MediaFileProcessor.Models.Enums;

/// <summary>
/// Enumeration for different video sizes.
/// </summary>
public enum VideoSizeType
{
    /// <summary>
    /// 16 times Common Intermediate Format (CIF)
    /// </summary>
    _16CIF,

    /// <summary>
    /// 2K resolution (approximately 2048 pixels wide)
    /// </summary>
    _2K,

    /// <summary>
    /// 2K resolution with a flat aspect ratio
    /// </summary>
    _2KFLAT,

    /// <summary>
    /// 2K resolution with a widescreen aspect ratio
    /// </summary>
    _2KSCOPE,

    /// <summary>
    /// 4 times Common Intermediate Format (CIF)
    /// </summary>
    _4CIF,

    /// <summary>
    /// 4K resolution (approximately 4096 pixels wide)
    /// </summary>
    _4K,

    /// <summary>
    /// 4K resolution with a flat aspect ratio
    /// </summary>
    _4KFLAT,

    /// <summary>
    /// 4K resolution with a widescreen aspect ratio
    /// </summary>
    _4KSCOPE,

    /// <summary>
    /// Color Graphics Array (CGA) resolution
    /// </summary>
    CGA,

    /// <summary>
    /// Common Intermediate Format (CIF) resolution
    /// </summary>
    CIF,

    /// <summary>
    /// Enhanced Graphics Array (EGA) resolution
    /// </summary>
    EGA,

    /// <summary>
    /// Film resolution
    /// </summary>
    FILM,

    /// <summary>
    /// FWQVGA resolution (480x320)
    /// </summary>
    FWQVGA,

    /// <summary>
    /// HD1080 resolution (1920x1080)
    /// </summary>
    HD1080,

    /// <summary>
    /// HD480 resolution (854x480)
    /// </summary>
    HD480,

    /// <summary>
    /// HD720 resolution (1280x720)
    /// </summary>
    HD720,

    /// <summary>
    /// HQVGA resolution (240x160)
    /// </summary>
    HQVGA,

    /// <summary>
    /// HSXGA resolution (5120x4096)
    /// </summary>
    HSXGA,
    HVGA,
    NHD,
    NTSC,
    NTSC_FILM,
    PAL,
    QCIF,
    QHD,
    QNTSC,
    QPAL,
    QQVGA,
    QSXGA,
    QVGA,
    QXGA,
    SNTSC,
    SPAL,
    SQCIF,
    SVGA,
    SXGA,
    UXGA,
    VGA,
    WHSXGA,
    WHUXGA,
    WOXGA,
    WQSXGA,
    WQUXGA,
    WQVGA,
    WSXGA,
    WUXGA,
    WVGA,
    WXGA,
    XGA,
    CUSTOM
}