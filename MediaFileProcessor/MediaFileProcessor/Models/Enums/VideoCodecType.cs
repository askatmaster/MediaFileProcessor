namespace MediaFileProcessor.Models.Enums;

/// <summary>
/// Video codec type
/// </summary>
public enum VideoCodecType
{
    /// <summary>
    /// Copy codec type
    /// </summary>
    COPY = 0,

    /// <summary>
    /// Multicolor charset for Commodore 64 (codec a64_multi)
    /// </summary>
    A64MULTI = 1,

    /// <summary>
    /// Multicolor charset for Commodore 64 = , extended with 5th color (colram) (codec a64_multi5)
    /// </summary>
    A64MULTI5 = 2,

    /// <summary>
    ///Alias/Wavefront PIX image
    /// </summary>
    ALIAS_PIX = 3,

    /// <summary>
    ///AMV Video
    /// </summary>
    AMV = 4,

    /// <summary>
    ///APNG (Animated Portable Network Graphics) image
    /// </summary>
    APNG = 5,

    /// <summary>
    ///ASUS V1
    /// </summary>
    ASV1 = 6,

    /// <summary>
    ///ASUS V2
    /// </summary>
    ASV2 = 7,

    /// <summary>
    ///Avid 1:1 10-bit RGB Packer
    /// </summary>
    AVRP = 8,

    /// <summary>
    ///Avid Meridien Uncompressed
    /// </summary>
    AVUI = 9,

    /// <summary>
    /// Uncompressed packed MS 4:4:4:4
    /// </summary>
    AYUV = 10,

    /// <summary>
    ///BMP (Windows and OS/2 bitmap)
    /// </summary>
    BMP = 11,

    /// <summary>
    ///Cinepak
    /// </summary>
    CINEPAK = 12,

    /// <summary>
    ///Cirrus Logic AccuPak
    /// </summary>
    CLJR = 13,

    /// <summary>
    ///SMPTE VC-2 (codec dirac)
    /// </summary>
    VC2 = 14,

    /// <summary>
    ///VC3/DNxHD
    /// </summary>
    DNXHD = 15,

    /// <summary>
    ///DPX (Digital Picture Exchange) image
    /// </summary>
    DPX = 16,

    /// <summary>
    /// DV (Digital Video)
    /// </summary>
    DVVIDEO = 17,

    /// <summary>
    /// FFmpeg video codec #1
    /// </summary>
    FFV1 = 18,

    /// <summary>
    /// Huffyuv FFmpeg variant
    /// </summary>
    FFVHUFF = 19,

    /// <summary>
    /// Flexible Image Transport System
    /// </summary>
    FITS = 20,

    /// <summary>
    /// Flash Screen Video
    /// </summary>
    FLASHSV = 21,

    /// <summary>
    /// Flash Screen Video Version 2
    /// </summary>
    FLASHSV2 = 22,

    /// <summary>
    /// FLV / Sorenson Spark / Sorenson H.263 (Flash Video) (codec flv1)
    /// </summary>
    FLV = 23,

    /// <summary>
    /// GIF (Graphics Interchange Format)
    /// </summary>
    GIF = 24,

    /// <summary>
    /// H.261
    /// </summary>
    H261 = 25,

    /// <summary>
    /// H.263 / H.263-1996
    /// </summary>
    H263 = 26,

    /// <summary>
    /// H.264 / H.264-1996
    /// </summary>
    H264 = 27,

    /// <summary>
    /// H.263+ / H.263-1998 / H.263 version 2
    /// </summary>
    H263P = 28,

    /// <summary>
    /// libx264 H.264 / AVC / MPEG-4 AVC / MPEG-4 part 10 (codec h264)
    /// </summary>
    LIBX264 = 29,

    /// <summary>
    /// libx264 H.264 / AVC / MPEG-4 AVC / MPEG-4 part 10 RGB (codec h264)
    /// </summary>
    LIBX264RGB = 30,

    /// <summary>
    /// AMD AMF H.264 Encoder (codec h264)
    /// </summary>
    H264_AMF = 31,

    /// <summary>
    /// NVIDIA NVENC H.264 encoder (codec h264)
    /// </summary>
    H264_NVENC = 32,

    /// <summary>
    /// H.264 / AVC / MPEG-4 AVC / MPEG-4 part 10 (Intel Quick Sync Video acceleration) (codec h264)
    /// </summary>
    H264_QSV = 33,

    /// <summary>
    /// NVIDIA NVENC H.264 encoder (codec h264)
    /// </summary>
    NVENC = 34,

    /// <summary>
    /// NVIDIA NVENC H.264 encoder (codec h264)
    /// </summary>
    NVENC_H264 = 35,

    /// <summary>
    /// Vidvox Hap
    /// </summary>
    HAP = 36,

    /// <summary>
    /// libx265 H.265 / HEVC (codec hevc)
    /// </summary>
    LIBX265 = 37,

    /// <summary>
    /// NVIDIA NVENC hevc encoder (codec hevc)
    /// </summary>
    NVENC_HEVC = 38,

    /// <summary>
    /// AMD AMF HEVC encoder (codec hevc)
    /// </summary>
    HEVC_AMF = 39,

    /// <summary>
    /// NVIDIA NVENC hevc encoder (codec hevc)
    /// </summary>
    HEVC_NVENC = 40,

    /// <summary>
    /// HEVC (Intel Quick Sync Video acceleration) (codec hevc)
    /// </summary>
    HEVC_QSV = 41,

    /// <summary>
    /// Huffyuv / HuffYUV
    /// </summary>
    HUFFYUV = 42,

    /// <summary>
    /// JPEG 2000
    /// </summary>
    JPEG2000 = 43,

    /// <summary>
    /// OpenJPEG JPEG 2000 (codec jpeg2000)
    /// </summary>
    LIBOPENJPEG = 44,

    /// <summary>
    /// JPEG-LS
    /// </summary>
    JPEGLS = 45,

    /// <summary>
    /// Lossless JPEG
    /// </summary>
    LJPEG = 46,

    /// <summary>
    /// MagicYUV video
    /// </summary>
    MAGICYUV = 47,

    /// <summary>
    /// MJPEG (Motion JPEG)
    /// </summary>
    MJPEG = 48,

    /// <summary>
    /// MJPEG (Intel Quick Sync Video acceleration) (codec mjpeg)
    /// </summary>
    MJPEG_QSV = 49,

    /// <summary>
    /// MPEG-1 video
    /// </summary>
    MPEG1VIDEO = 50,

    /// <summary>
    /// MPEG-2 video
    /// </summary>
    MPEG2VIDEO = 51,

    /// <summary>
    /// MPEG-2 video (Intel Quick Sync Video acceleration) (codec mpeg2video)
    /// </summary>
    MPEG2_QSV = 52,

    /// <summary>
    /// MPEG-4 part 2
    /// </summary>
    MPEG4 = 53,

    /// <summary>
    /// libxvidcore MPEG-4 part 2 (codec mpeg4)
    /// </summary>
    LIBXVID = 54,

    /// <summary>
    /// MPEG-4 part 2 Microsoft variant version 2
    /// </summary>
    MSMPEG4V2 = 55,

    /// <summary>
    /// MPEG-4 part 2 Microsoft variant version 3 (codec msmpeg4v3)
    /// </summary>
    MSMPEG4 = 56,

    /// <summary>
    /// Microsoft Video-1
    /// </summary>
    MSVIDEO1 = 57,

    /// <summary>
    /// PAM (Portable AnyMap) image
    /// </summary>
    PAM = 58,

    /// <summary>
    /// PBM (Portable BitMap) image
    /// </summary>
    PBM = 59,

    /// <summary>
    /// PC Paintbrush PCX image
    /// </summary>
    PCX = 60,

    /// <summary>
    /// PGM (Portable GrayMap) image
    /// </summary>
    PGM = 61,

    /// <summary>
    /// PGMYUV (Portable GrayMap YUV) image
    /// </summary>
    PGMYUV = 62,

    /// <summary>
    /// PNG (Portable Network Graphics) image
    /// </summary>
    PNG = 63,

    /// <summary>
    /// PPM (Portable PixelMap) image
    /// </summary>
    PPM = 64,

    /// <summary>
    /// Apple ProRes
    /// </summary>
    PRORES = 65,

    /// <summary>
    /// Apple ProRes (codec prores)
    /// </summary>
    PRORES_AW = 66,

    /// <summary>
    /// Apple ProRes (iCodec Pro) (codec prores)
    /// </summary>
    PRORES_KS = 67,

    /// <summary>
    /// QuickTime Animation (RLE) video
    /// </summary>
    QTRLE = 68,

    /// <summary>
    /// AJA Kona 10-bit RGB Codec
    /// </summary>
    R10K = 69,

    /// <summary>
    /// Uncompressed RGB 10-bit
    /// </summary>
    R210 = 70,

    /// <summary>
    /// raw video
    /// </summary>
    RAWVIDEO = 71,

    /// <summary>
    /// id RoQ video (codec roq)
    /// </summary>
    ROQVIDEO = 72,

    /// <summary>
    /// RealVideo 1.0
    /// </summary>
    RV10 = 73,

    /// <summary>
    /// RealVideo 2.0
    /// </summary>
    RV20 = 74,

    /// <summary>
    /// SGI image
    /// </summary>
    SGI = 75,

    /// <summary>
    /// Snow
    /// </summary>
    SNOW = 76,

    /// <summary>
    /// Sun Rasterfile image
    /// </summary>
    SUNRAST = 77,

    /// <summary>
    /// Sorenson Vector Quantizer 1 / Sorenson Video 1 / SVQ1
    /// </summary>
    SVQ1 = 78,

    /// <summary>
    /// Truevision Targa image
    /// </summary>
    TARGA = 79,

    /// <summary>
    /// libtheora Theora (codec theora)
    /// </summary>
    LIBTHEORA = 80,

    /// <summary>
    /// TIFF image
    /// </summary>
    TIFF = 81,

    /// <summary>
    /// Ut Video
    /// </summary>
    UTVIDEO = 82,

    /// <summary>
    /// Uncompressed 4:2:2 10-bit
    /// </summary>
    V210 = 83,

    /// <summary>
    /// Uncompressed packed 4:4:4
    /// </summary>
    V308 = 84,

    /// <summary>
    /// Uncompressed packed QT 4:4:4:4
    /// </summary>
    V408 = 85,

    /// <summary>
    /// Uncompressed 4:4:4 10-bit
    /// </summary>
    V410 = 86,

    /// <summary>
    /// libvpx VP8 (codec vp8)
    /// </summary>
    LIBVPX = 87,

    /// <summary>
    /// VP9 video (Intel Quick Sync Video acceleration) (codec vp9)
    /// </summary>
    VP9_QSV = 88,

    /// <summary>
    /// libwebp WebP image (codec webp)
    /// </summary>
    LIBWEBP_ANIM = 89,

    /// <summary>
    /// libwebp WebP image (codec webp)
    /// </summary>
    LIBWEBP = 90,

    /// <summary>
    /// Windows Media Video 7
    /// </summary>
    WMV1 = 91,

    /// <summary>
    /// Windows Media Video 8
    /// </summary>
    WMV2 = 92,

    /// <summary>
    /// AVFrame to AVPacket passthrough
    /// </summary>
    WRAPPED_AVFRAME = 93,

    /// <summary>
    /// XBM (X BitMap) image
    /// </summary>
    XBM = 94,

    /// <summary>
    /// X-face image
    /// </summary>
    XFACE = 95,

    /// <summary>
    /// XWD (X Window Dump) image
    /// </summary>
    XWD = 96,

    /// <summary>
    /// Uncompressed YUV 4:1:1 12-bit
    /// </summary>
    Y41P = 97,

    /// <summary>
    /// Uncompressed packed 4:2:0
    /// </summary>
    YUV4 = 98,

    /// <summary>
    /// LCL (LossLess Codec Library) ZLIB
    /// </summary>
    ZLIB = 99,

    /// <summary>
    /// Zip Motion Blocks Video
    /// </summary>
    ZMBV = 100
}