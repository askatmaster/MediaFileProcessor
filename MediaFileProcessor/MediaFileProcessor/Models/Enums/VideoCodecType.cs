namespace MediaFileProcessor.Models.Enums;

public enum VideoCodecType
{
    COPY,
    A64MULTI,        //Multicolor charset for Commodore 64 (codec a64_multi)
    A64MULTI5,       //Multicolor charset for Commodore 64, extended with 5th color (colram) (codec a64_multi5)
    ALIAS_PIX,       //Alias/Wavefront PIX image
    AMV,             //AMV Video
    APNG,            //APNG (Animated Portable Network Graphics) image
    ASV1,            //ASUS V1
    ASV2,            //ASUS V2
    AVRP,            //Avid 1:1 10-bit RGB Packer
    AVUI,            //Avid Meridien Uncompressed
    AYUV,            //Uncompressed packed MS 4:4:4:4
    BMP,             //BMP (Windows and OS/2 bitmap)
    CINEPAK,         //Cinepak
    CLJR,            //Cirrus Logic AccuPak
    VC2,             //SMPTE VC-2 (codec dirac)
    DNXHD,           //VC3/DNxHD
    DPX,             //DPX (Digital Picture Exchange) image
    DVVIDEO,         //DV (Digital Video)
    FFV1,            //FFmpeg video codec #1
    FFVHUFF,         //Huffyuv FFmpeg variant
    FITS,            //Flexible Image Transport System
    FLASHSV,         //Flash Screen Video
    FLASHSV2,        //Flash Screen Video Version 2
    FLV,             //FLV / Sorenson Spark / Sorenson H.263 (Flash Video) (codec flv1)
    GIF,             //GIF (Graphics Interchange Format)
    H261,            //H.261
    H263,            //H.263 / H.263-1996
    H263P,           //H.263+ / H.263-1998 / H.263 version 2
    LIBX264,         //libx264 H.264 / AVC / MPEG-4 AVC / MPEG-4 part 10 (codec h264)
    LIBX264RGB,      //libx264 H.264 / AVC / MPEG-4 AVC / MPEG-4 part 10 RGB (codec h264)
    H264_AMF,        //AMD AMF H.264 Encoder (codec h264)
    H264_NVENC,      //NVIDIA NVENC H.264 encoder (codec h264)
    H264_QSV,        //H.264 / AVC / MPEG-4 AVC / MPEG-4 part 10 (Intel Quick Sync Video acceleration) (codec h264)
    NVENC,           //NVIDIA NVENC H.264 encoder (codec h264)
    NVENC_H264,      //NVIDIA NVENC H.264 encoder (codec h264)
    HAP,             //Vidvox Hap
    LIBX265,         //libx265 H.265 / HEVC (codec hevc)
    NVENC_HEVC,      //NVIDIA NVENC hevc encoder (codec hevc)
    HEVC_AMF,        //AMD AMF HEVC encoder (codec hevc)
    HEVC_NVENC,      //NVIDIA NVENC hevc encoder (codec hevc)
    HEVC_QSV,        //HEVC (Intel Quick Sync Video acceleration) (codec hevc)
    HUFFYUV,         //Huffyuv / HuffYUV
    JPEG2000,        //JPEG 2000
    LIBOPENJPEG,     //OpenJPEG JPEG 2000 (codec jpeg2000)
    JPEGLS,          //JPEG-LS
    LJPEG,           //Lossless JPEG
    MAGICYUV,        //MagicYUV video
    MJPEG,           //MJPEG (Motion JPEG)
    MJPEG_QSV,       //MJPEG (Intel Quick Sync Video acceleration) (codec mjpeg)
    MPEG1VIDEO,      //MPEG-1 video
    MPEG2VIDEO,      //MPEG-2 video
    MPEG2_QSV,       //MPEG-2 video (Intel Quick Sync Video acceleration) (codec mpeg2video)
    MPEG4,           //MPEG-4 part 2
    LIBXVID,         //libxvidcore MPEG-4 part 2 (codec mpeg4)
    MSMPEG4V2,       //MPEG-4 part 2 Microsoft variant version 2
    MSMPEG4,         //MPEG-4 part 2 Microsoft variant version 3 (codec msmpeg4v3)
    MSVIDEO1,        //Microsoft Video-1
    PAM,             //PAM (Portable AnyMap) image
    PBM,             //PBM (Portable BitMap) image
    PCX,             //PC Paintbrush PCX image
    PGM,             //PGM (Portable GrayMap) image
    PGMYUV,          //PGMYUV (Portable GrayMap YUV) image
    PNG,             //PNG (Portable Network Graphics) image
    PPM,             //PPM (Portable PixelMap) image
    PRORES,          //Apple ProRes
    PRORES_AW,       //Apple ProRes (codec prores)
    PRORES_KS,       //Apple ProRes (iCodec Pro) (codec prores)
    QTRLE,           //QuickTime Animation (RLE) video
    R10K,            //AJA Kona 10-bit RGB Codec
    R210,            //Uncompressed RGB 10-bit
    RAWVIDEO,        //raw video
    ROQVIDEO,        //id RoQ video (codec roq)
    RV10,            //RealVideo 1.0
    RV20,            //RealVideo 2.0
    SGI,             //SGI image
    SNOW,            //Snow
    SUNRAST,         //Sun Rasterfile image
    SVQ1,            //Sorenson Vector Quantizer 1 / Sorenson Video 1 / SVQ1
    TARGA,           //Truevision Targa image
    LIBTHEORA,       //libtheora Theora (codec theora)
    TIFF,            //TIFF image
    UTVIDEO,         //Ut Video
    V210,            //Uncompressed 4:2:2 10-bit
    V308,            //Uncompressed packed 4:4:4
    V408,            //Uncompressed packed QT 4:4:4:4
    V410,            //Uncompressed 4:4:4 10-bit
    LIBVPX,          //libvpx VP8 (codec vp8)
    VP9_QSV,         //VP9 video (Intel Quick Sync Video acceleration) (codec vp9)
    LIBWEBP_ANIM,    //libwebp WebP image (codec webp)
    LIBWEBP,         //libwebp WebP image (codec webp)
    WMV1,            //Windows Media Video 7
    WMV2,            //Windows Media Video 8
    WRAPPED_AVFRAME, //AVFrame to AVPacket passthrough
    XBM,             //XBM (X BitMap) image
    XFACE,           //X-face image
    XWD,             //XWD (X Window Dump) image
    Y41P,            //Uncompressed YUV 4:1:1 12-bit
    YUV4,            //Uncompressed packed 4:2:0
    ZLIB,            //LCL (LossLess Codec Library) ZLIB
    ZMBV             //Zip Motion Blocks Video
}