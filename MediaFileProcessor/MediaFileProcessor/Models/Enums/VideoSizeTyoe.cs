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
    /// <summary>
    /// High-definition television 720p.
    /// </summary>
    HVGA,

    /// <summary>
    /// Non-high-definition television.
    /// </summary>
    NHD,

    /// <summary>
    /// The National Television System Committee television standard.
    /// </summary>
    NTSC,

    /// <summary>
    /// The National Television System Committee standard for film.
    /// </summary>
    NTSC_FILM,

    /// <summary>
    /// The Phase Alternating Line television standard used in Europe and other countries.
    /// </summary>
    PAL,

    /// <summary>
    /// Quarter Common Intermediate Format used in videoconferencing and mobile devices.
    /// </summary>
    QCIF,

    /// <summary>
    /// High-definition television 720p.
    /// </summary>
    QHD,

    /// <summary>
    /// Quarter National Television System Committee.
    /// </summary>
    QNTSC,

    /// <summary>
    /// Quarter Phase Alternating Line.
    /// </summary>
    QPAL,

    /// <summary>
    /// Quarter Quarter VGA.
    /// </summary>
    QQVGA,

    /// <summary>
    /// Quarter Super Extended Graphics Array.
    /// </summary>
    QSXGA,

    /// <summary>
    /// Quarter VGA.
    /// </summary>
    QVGA,

    /// <summary>
    /// Quarter XGA.
    /// </summary>
    QXGA,

    /// <summary>
    /// Super NTSC.
    /// </summary>
    SNTSC,

    /// <summary>
    /// Super PAL.
    /// </summary>
    SPAL,

    /// <summary>
    /// Super QCIF.
    /// </summary>
    SQCIF,

    /// <summary>
    /// Super Video Graphics Array.
    /// </summary>
    SVGA,

    /// <summary>
    /// Super Extended Graphics Array.
    /// </summary>
    SXGA,

    /// <summary>
    /// Ultra Extended Graphics Array.
    /// </summary>
    UXGA,

    /// <summary>
    /// Video Graphics Array.
    /// </summary>
    VGA,

    /// <summary>
    /// Widescreen High Super Extended Graphics Array.
    /// </summary>
    WHSXGA,

    /// <summary>
    /// Widescreen High Ultra Extended Graphics Array.
    /// </summary>
    WHUXGA,

    /// <summary>
    /// Widescreen Organic Extended Graphics Array.
    /// </summary>
    WOXGA,

    /// <summary>
    /// Widescreen Quarter Super Extended Graphics Array.
    /// </summary>
    WQSXGA,

    /// <summary>
    /// Widescreen Quarter Ultra Extended Graphics Array.
    /// </summary>
    WQUXGA,

    /// <summary>
    /// Widescreen Quarter Video Graphics Array.
    /// </summary>
    WQVGA,

    /// <summary>
    /// Widescreen Super Extended Graphics Array.
    /// </summary>
    WSXGA,

    /// <summary>
    /// Widescreen Ultra Extended Graphics Array.
    /// </summary>
    WUXGA,

    /// <summary>
    /// Widescreen Video Graphics Array.
    /// </summary>
    WVGA,

    /// <summary>
    /// Widescreen XGA.
    /// </summary>
    WXGA,

    /// <summary>
    /// Custom size
    /// </summary>
    CUSTOM
}