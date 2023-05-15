namespace MediaFileProcessor.Models.Enums;

/// <summary>
/// Specifies the pixel format for video encoding or decoding.
/// </summary>
public enum PixelFormatType
{
    /// <summary>
    /// 8 bits per pixel, YUV 4:0:0, full scale (JPEG).
    /// </summary>
    YUV400P = 0,

    /// <summary>
    /// 8 bits per pixel, planar YUV 4:1:0, (1 Cr & Cb sample per 4x4 Y samples).
    /// </summary>
    YUV410P = 1,

    /// <summary>
    /// 8 bits per pixel, planar YUV 4:2:0, (1 Cr & Cb sample per 2x2 Y samples).
    /// </summary>
    YUV420P = 2,

    /// <summary>
    /// 8 bits per pixel, planar YUV 4:2:2, (1 Cr & Cb sample per 2x1 Y samples).
    /// </summary>
    YUV422P = 3,

    /// <summary>
    /// 8 bits per pixel, planar YUV 4:4:4, (1 Cr & Cb sample per 1x1 Y samples).
    /// </summary>
    YUV444P = 4,

    /// <summary>
    /// 8 bits per pixel, packed YUV 4:2:2, Y0 Cb Y1 Cr.
    /// </summary>
    YUYV422 = 5,

    /// <summary>
    /// 24 bits per pixel, packed RGB 8:8:8, RGBRGB...
    /// </summary>
    RGB24 = 6,

    /// <summary>
    /// 24 bits per pixel, packed RGB 8:8:8, BGRBGR...
    /// </summary>
    BGR24 = 7,

    /// <summary>
    /// 32 bits per pixel, packed RGB 8:8:8, XRGBXRGB...
    /// </summary>
    XRGB = 8,

    /// <summary>
    /// 32 bits per pixel, packed RGB 8:8:8, XBGRXBGR...
    /// </summary>
    XBGR = 9,

    /// <summary>
    /// 32 bits per pixel, packed RGBA 8:8:8:8, RGBA...
    /// </summary>
    RGBA = 10,

    /// <summary>
    /// 32 bits per pixel, packed BGRA 8:8:8:8, ARGB...
    /// </summary>
    BGRA = 11,

    /// <summary>
    /// 8 bits per pixel, grayscale.
    /// </summary>
    GRAY = 12,

    /// <summary>
    /// 8 bits per pixel, monochrome, 0 is white, 1 is black, pixels are ordered from the msb to the lsb.
    /// </summary>
    MONOWHITE = 13,

    /// <summary>
    /// 8 bits per pixel, monochrome, 0 is black, 1 is white, pixels are ordered from the msb to the lsb.
    /// </summary>
    MONOBLACK = 14,

    /// <summary>
    /// 4 bits per pixel, 16 color VGA palette.
    /// </summary>
    PAL8 = 15,
    /// <summary>
    /// 16 bits per pixel, planar YUV 4:1:1, (1 Cr & Cb sample per 4x1 Y samples).
    /// </summary>
    YUV411P = 16,

    /// <summary>
    /// 9 bits per pixel, planar YUV 4:0:0, (1 Cr & Cb sample per 1x1 Y samples).
    /// </summary>
    GRAY9BE = 17,

    /// <summary>
    /// 9 bits per pixel, planar YUV 4:0:0, (1 Cr & Cb sample per 1x1 Y samples).
    /// </summary>
    GRAY9LE = 18,

    /// <summary>
    /// 10 bits per pixel, planar YUV 4:0:0, (1 Cr & Cb sample per 1x1 Y samples).
    /// </summary>
    GRAY10BE = 19,

    /// <summary>
    /// 10 bits per pixel, planar YUV 4:0:0, (1 Cr & Cb sample per 1x1 Y samples).
    /// </summary>
    GRAY10LE = 20,

    /// <summary>
    /// 12 bits per pixel, planar YUV 4:0:0, (1 Cr & Cb sample per 1x1 Y samples).
    /// </summary>
    GRAY12BE = 21,

    /// <summary>
    /// 12 bits per pixel, planar YUV 4:0:0, (1 Cr & Cb sample per 1x1 Y samples).
    /// </summary>
    GRAY12LE = 22,

    /// <summary>
    /// 16 bits per pixel, planar YUV 4:0:0, (1 Cr & Cb sample per 1x1 Y samples).
    /// </summary>
    GRAY16BE = 23,

    /// <summary>
    /// 16 bits per pixel, planar YUV 4:0:0, (1 Cr & Cb sample per 1x1 Y samples).
    /// </summary>
    GRAY16LE = 24,

    /// <summary>
    /// 10 bits per pixel, packed YUV 4:2:2, Y0 Cb Y1 Cr.
    /// </summary>
    YUV422P10BE = 25,

    /// <summary>
    /// 10 bits per pixel, packed YUV 4:2:2, Y0 Cb Y1 Cr.
    /// </summary>
    YUV422P10LE = 26,

    /// <summary>
    /// 12 bits per pixel, packed YUV 4:2:2, Y0 Cb Y1 Cr.
    /// </summary>
    YUV422P12BE = 27,

    /// <summary>
    /// 12 bits per pixel, packed YUV 4:2:2, Y0 Cb Y1 Cr.
    /// </summary>
    YUV422P12LE = 28,

    /// <summary>
    /// 16 bits per pixel, packed YUV 4:2:2, Y0 Cb Y1 Cr.
    /// </summary>
    YUV422P16BE = 29,
    /// <summary>
    /// 16 bits per pixel, packed YUV 4:2:2, Y0 Cb Y1 Cr.
    /// </summary>
    YUV422P16LE = 30,
    /// <summary>
    /// 10 bits per pixel, planar YUV 4:2:0, (1 Cr & Cb sample per 2x2 Y samples).
    /// </summary>
    YUV420P10BE = 31,

    /// <summary>
    /// 10 bits per pixel, planar YUV 4:2:0, (1 Cr & Cb sample per 2x2 Y samples).
    /// </summary>
    YUV420P10LE = 32,

    /// <summary>
    /// 12 bits per pixel, planar YUV 4:2:0, (1 Cr & Cb sample per 2x2 Y samples).
    /// </summary>
    YUV420P12BE = 33,

    /// <summary>
    /// 12 bits per pixel, planar YUV 4:2:0, (1 Cr & Cb sample per 2x2 Y samples).
    /// </summary>
    YUV420P12LE = 34,

    /// <summary>
    /// 16 bits per pixel, planar YUV 4:2:0, (1 Cr & Cb sample per 2x2 Y samples).
    /// </summary>
    YUV420P16BE = 35,

    /// <summary>
    /// 16 bits per pixel, planar YUV 4:2:0, (1 Cr & Cb sample per 2x2 Y samples).
    /// </summary>
    YUV420P16LE = 36,

    /// <summary>
    /// 10 bits per pixel, planar YUV 4:4:4, (1 Cr & Cb sample per 1x1 Y samples).
    /// </summary>
    YUV444P10BE = 37,

    /// <summary>
    /// 10 bits per pixel, planar YUV 4:4:4, (1 Cr & Cb sample per 1x1 Y samples).
    /// </summary>
    YUV444P10LE = 38,

    /// <summary>
    /// 12 bits per pixel, planar YUV 4:4:4, (1 Cr & Cb sample per 1x1 Y samples).
    /// </summary>
    YUV444P12BE = 39,

    /// <summary>
    /// 12 bits per pixel, planar YUV 4:4:4, (1 Cr & Cb sample per 1x1 Y samples).
    /// </summary>
    YUV444P12LE = 40,

    /// <summary>
    /// 16 bits per pixel, planar YUV 4:4:4, (1 Cr & Cb sample per 1x1 Y samples).
    /// </summary>
    YUV444P16BE = 41,

    /// <summary>
    /// 16 bits per pixel, planar YUV 4:4:4, (1 Cr & Cb sample per 1x1 Y samples).
    /// </summary>
    YUV444P16LE = 42,

    /// <summary>
    /// 8 bits per pixel, packed RGB 3:3:2, RGBRGB...
    /// </summary>
    RGB8 = 43,

    /// <summary>
    /// 4 bits per pixel, packed RGB 1:2:1, RPBRPBRPB...
    /// </summary>
    RGB4 = 44,
    /// <summary>
    /// 2bits per pixel, packed RGB 1:2:1, RPBRPBRPB...
    /// </summary>
    RGB4_BYTE = 45,
    /// <summary>
    /// 16 bits per pixel, packed RGB 5:6:5, RGBRGB...
    /// </summary>
    RGB565 = 46,

    /// <summary>
    /// 24 bits per pixel, packed RGB 8:8:8, RGBXRGBX...
    /// </summary>
    RGB24X = 47,

    /// <summary>
    /// 32 bits per pixel, packed RGB 8:8:8, XRGBXRGB...
    /// </summary>
    XRGB1555 = 48,

    /// <summary>
    /// 32 bits per pixel, packed RGB 8:8:8, ARGBARGB...
    /// </summary>
    ARGB1555 = 49,

    /// <summary>
    /// 32 bits per pixel, packed RGBA 8:8:8:8, XRGBXRGB...
    /// </summary>
    RGBA64BE = 50,

    /// <summary>
    /// 32 bits per pixel, packed RGBA 8:8:8:8, XRGBXRGB...
    /// </summary>
    RGBA64LE = 51,

    /// <summary>
    /// 16 bits per pixel, planar GBR 4:4:4 24bpp, GGG... BBB...
    /// </summary>
    GBRP = 52,

    /// <summary>
    /// 16 bits per pixel, planar GBR 4:4:4 24bpp, BBB... GGG...
    /// </summary>
    GBRP9BE = 53,

    /// <summary>
    /// 16 bits per pixel, planar GBR 4:4:4 24bpp, BBB... GGG...
    /// </summary>
    GBRP9LE = 54,

    /// <summary>
    /// 16 bits per pixel, planar GBR 4:4:4 24bpp, BBB... GGG...
    /// </summary>
    GBRP10BE = 55,

    /// <summary>
    /// 16 bits per pixel, planar GBR 4:4:4 24bpp, BBB... GGG...
    /// </summary>
    GBRP10LE = 56,

    /// <summary>
    /// 16 bits per pixel, planar GBR 4:4:4 24bpp, BBB... GGG...
    /// </summary>
    GBRP12BE = 57,

    /// <summary>
    /// 16 bits per pixel, planar GBR 4:4:4 24bpp, BBB... GGG...
    /// </summary>
    GBRP12LE = 58,

    /// <summary>
    /// 16 bits per pixel, planar GBR 4:4:4 24bpp, BBB... GGG...
    /// </summary>
    GBRP14BE = 59,

    /// <summary>
    /// 16 bits per pixel, planar GBR 4:4:4 24bpp, BBB... GGG...
    /// </summary>
    GBRP14LE = 60,

    /// <summary>
    /// 16 bits per pixel, planar GBR 4:4:4 24bpp, BBB... GGG...
    /// </summary>
    GBRP16BE = 61,
    /// <summary>
    ///16 bits per pixel, planar GBR 4:4:4 24bpp, BBB... GGG...
    /// </summary>
    GBRP16LE = 62,

    /// <summary>
    /// 10 bits per pixel, packed YUV 4:2:0, Y0 Cb Y1 Cr.
    /// </summary>
    YUVA420P10BE = 63,

    /// <summary>
    /// 10 bits per pixel, packed YUV 4:2:0, Y0 Cb Y1 Cr.
    /// </summary>
    YUVA420P10LE = 64,

    /// <summary>
    /// 12 bits per pixel, packed YUV 4:2:0, Y0 Cb Y1 Cr.
    /// </summary>
    YUVA420P12BE = 65,

    /// <summary>
    /// 12 bits per pixel, packed YUV 4:2:0, Y0 Cb Y1 Cr.
    /// </summary>
    YUVA420P12LE = 66,

    /// <summary>
    /// 16 bits per pixel, packed YUV 4:2:0, Y0 Cb Y1 Cr.
    /// </summary>
    YUVA420P16BE = 67,

    /// <summary>
    /// 16 bits per pixel, packed YUV 4:2:0, Y0 Cb Y1 Cr.
    /// </summary>
    YUVA420P16LE = 68,

    /// <summary>
    /// 10 bits per pixel, planar YUV 4:2:2, (1 Cr & Cb sample per 2x1 Y samples).
    /// </summary>
    YUVA422P10BE = 69,

    /// <summary>
    /// 10 bits per pixel, planar YUV 4:2:2, (1 Cr & Cb sample per 2x1 Y samples).
    /// </summary>
    YUVA422P10LE = 70,

    /// <summary>
    /// 12 bits per pixel, planar YUV 4:2:2, (1 Cr & Cb sample per 2x1 Y samples).
    /// </summary>
    YUVA422P12BE = 71,

    /// <summary>
    /// 12 bits per pixel, planar YUV 4:2:2, (1 Cr & Cb sample per 2x1 Y samples).
    /// </summary>
    YUVA422P12LE = 72,

    /// <summary>
    /// 16 bits per pixel, planar YUV 4:2:2, (1 Cr & Cb sample per 2x1 Y samples).
    /// </summary>
    YUVA422P16BE = 73,

    /// <summary>
    /// 16 bits per pixel, planar YUV 4:2:2, (1 Cr & Cb sample per 2x1 Y samples).
    /// </summary>
    YUVA422P16LE = 74,

    /// <summary>
    /// 10 bits per pixel, planar YUV 4:4:4, (1 Cr & Cb sample per 1x1 Y samples).
    /// </summary>
    YUVA444P10BE = 75,

    /// <summary>
    /// 10 bits per pixel, planar YUV 4:4:4, (1 Cr & Cb sample per 1x1 Y samples).
    /// </summary>
    YUVA444P10LE = 76,

    /// <summary>
    /// 12 bits per pixel, planar YUV 4:4:4, (1 Cr & Cb sample per 1x1 Y samples).
    /// </summary>
    YUVA444P12BE = 77,

    /// <summary>
    /// 12 bits per pixel, planar YUV 4:4:4, (1 Cr & Cb sample per 1x1 Y samples).
    /// </summary>
    YUVA444P12LE = 78,

    /// <summary>
    /// 16 bits per pixel, planar YUV 4:4:4, (1 Cr & Cb sample per 1x1 Y samples).
    /// </summary>
    YUVA444P16BE = 79,

    /// <summary>
    /// 16 bits per pixel, planar YUV 4:4:4, (1 Cr & Cb sample per 1x1 Y samples).
    /// </summary>
    YUVA444P16LE = 80,

    /// <summary>
    /// Alias for YUVJ411P for backward compatibility.
    /// </summary>
    YUVJ411P = 81,

    /// <summary>
    /// Alias for YUVJ420P for backward compatibility.
    /// </summary>
    YUVJ420P = 82,

    /// <summary>
    /// Alias for YUVJ422P for backward compatibility.
    /// </summary>
    YUVJ422P = 83,

    /// <summary>
    /// Alias for YUVJ444P for backward compatibility.
    /// </summary>
    YUVJ444P = 84,

    /// <summary>
    /// Alias for XYZ12BE for backward compatibility.
    /// </summary>
    XYZ12 = 85,

    /// <summary>
    /// Alias for NV24 for backward compatibility.
    /// </summary>
    NV24 = 86,

    /// <summary>
    /// Alias for NV42 for backward compatibility.
    /// </summary>
    NV42 = 87,

    /// <summary>
    /// 24 bits per pixel, packed BGR 8:8:8, BGRBGR..
    /// </summary>
    BGR8 = 88
}