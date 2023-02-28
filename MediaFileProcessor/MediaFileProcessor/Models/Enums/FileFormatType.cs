namespace MediaFileProcessor.Models.Enums;

/// <summary>
/// Enumeration for the supported file format types.
/// </summary>
public enum FileFormatType
{
    /// <summary>
    /// The file format type for 3GP files.
    /// </summary>
    /// <remarks>
    /// 3GP is a multimedia container format that is commonly used for storing video and audio on mobile devices.
    /// </remarks>
    _3GP = 0,

    /// <summary>
    /// The file format type for MP4 files.
    /// </summary>
    /// <remarks>
    /// MP4 is a multimedia container format that is widely used for storing video and audio,
    /// and it provides efficient compression without sacrificing audio/video quality.
    /// </remarks>
    MP4 = 1,

    /// <summary>
    /// The file format type for ICO files.
    /// </summary>
    /// <remarks>
    /// ICO is a file format for storing icons in Microsoft Windows,
    /// and it can store multiple images of different sizes and color depths in a single file.
    /// </remarks>
    ICO = 2,

    /// <summary>
    /// The file format type for BIN files.
    /// </summary>
    /// <remarks>
    /// BIN is a binary file format that is commonly used for storing executable code or data.
    /// </remarks>
    BIN = 3,

    /// <summary>
    /// The file format type for MOV files.
    /// </summary>
    /// <remarks>
    /// MOV is a multimedia container format that is commonly used for storing video and audio on Apple devices.
    /// </remarks>
    MOV = 4,

    /// <summary>
    /// The file format type for TIFF files.
    /// </summary>
    /// <remarks>
    /// TIFF is a file format for storing raster graphics images,
    /// and it is widely used in the printing and publishing industry.
    /// </remarks>
    TIFF = 5,

    /// <summary>
    /// The file format type for MATROSKA files.
    /// </summary>
    /// <remarks>
    /// MATROSKA is a free and open-source multimedia container format,
    /// and it is commonly used for storing high-definition video and audio.
    /// </remarks>
    MKV = 7,
    /// <summary>
    /// The file format type for AVI files.
    /// </summary>
    /// <remarks>
    /// AVI is a popular multimedia container format that was developed by Microsoft,
    /// and it is widely used for storing video and audio.
    /// </remarks>
    AVI = 8,

    /// <summary>
    /// The file format type for MPEG files.
    /// </summary>
    /// <remarks>
    /// MPEG is a family of multimedia file formats that are used for storing video and audio,
    /// and it provides high compression with good video quality.
    /// </remarks>
    MPEG = 9,

    //TODO tests
    /// <summary>
    /// The file format type for MPEG-TS files.
    /// </summary>
    /// <remarks>
    /// MPEG-TS is a standard format for the transport of video and audio over the Internet,
    /// and it is widely used for delivering video to set-top boxes and other digital devices.
    /// </remarks>
    MPEGTS = 10,

    //TODO tests
    /// <summary>
    /// The file format type for SVCD files.
    /// </summary>
    /// <remarks>
    /// SVCD is a file format for storing video on CDs,
    /// and it provides better video quality than VCD with a resolution of 480x576 pixels.
    /// </remarks>
    SVCD = 11,

    /// <summary>
    /// The file format type for GIF files.
    /// </summary>
    /// <remarks>
    /// GIF is a file format for storing graphics and animations,
    /// and it is widely used for simple animations on the web.
    /// </remarks>
    GIF = 12,

    /// <summary>
    /// The file format type for VOB files.
    /// </summary>
    /// <remarks>
    /// VOB is a file format for storing video data on DVDs,
    /// and it is used to store the main movie, menus, and other video content on a DVD.
    /// </remarks>
    VOB = 13,

    /// <summary>
    /// The file format type for M2TS files.
    /// </summary>
    /// <remarks>
    /// M2TS is a file format for storing high-definition video and audio on Blu-ray discs,
    /// and it provides high-quality video and audio playback.
    /// </remarks>
    M2TS = 14,

    /// <summary>
    /// The file format type for MXF files.
    /// </summary>
    /// <remarks>
    /// MXF is a file format for storing professional-quality video and audio,
    /// and it is commonly used in the television and film industries.
    /// </remarks>
    MXF = 15,

    /// <summary>
    /// The file format type for WEBM files.
    /// </summary>
    /// <remarks>
    /// WEBM is a free and open-source multimedia file format that is designed for the web,
    /// and it provides efficient compression with good video quality.
    /// </remarks>
    WEBM = 16,

    //TODO tests
    /// <summary>
    /// GXF file format. This format is commonly used in broadcast applications to store video and audio content.
    /// </summary>
    GXF = 17,

    /// <summary>
    /// FLV file format. This format is commonly used to deliver video over the internet and supports video and audio codecs such as H.263, VP6, and MP3.
    /// </summary>
    FLV = 18,

    /// <summary>
    /// OGG file format. This format is commonly used for streaming audio and video content over the internet and uses open source codecs such as Theora and Vorbis.
    /// </summary>
    OGG = 19,

    /// <summary>
    /// WMV file format. This format is developed by Microsoft and commonly used for delivering video content over the internet. It supports Windows Media Audio and Windows Media Video codecs.
    /// </summary>
    WMV = 20,

    /// <summary>
    /// BMP file format. This format is used to store bitmap images and widely supported across various platforms.
    /// </summary>
    BMP = 21,

    /// <summary>
    /// ASF file format. This format is developed by Microsoft and commonly used for delivering video and audio content over the internet. It supports a wide range of codecs including Windows Media Audio and Video.
    /// </summary>
    ASF = 22,

    /// <summary>
    /// JPG file format. This format is used to store images and is commonly used for digital photography and graphics.
    /// </summary>
    JPG = 23,

    /// <summary>
    /// JPEG file format. This format is used to store images and is commonly used for digital photography and graphics.
    /// </summary>
    JPEG = 24,

    /// <summary>
    /// IMAGE2PIPE file format. This format is used to stream images through pipes, it's supported by FFmpeg.
    /// </summary>
    IMAGE2PIPE = 25,

    /// <summary>
    /// The IMAGE2 format. This format is commonly used for image data.
    /// </summary>
    IMAGE2 = 26,

    /// <summary>
    /// The PNG format. This is a lossless image format that is commonly used for web graphics.
    /// </summary>
    PNG = 27,

    /// <summary>
    /// The raw video format. This is a format that stores video data in its raw form, without any compression or encoding.
    /// </summary>
    RAWVIDEO = 28,

    /// <summary>
    /// The MP3 format. This is a popular audio compression format that provides good sound quality with small file sizes.
    /// </summary>
    MP3 = 29,

    /// <summary>
    /// The raw format. This format is used to store data in its raw, unprocessed form.
    /// </summary>
    RAW = 30,

    /// <summary>
    /// The SVG format. This is a vector graphics format that is commonly used for creating images and graphics for the web.
    /// </summary>
    SVG = 31,

    /// <summary>
    /// The PSD format. This is the native format of Adobe Photoshop, and is used to store image data in a layered format.
    /// </summary>
    PSD = 32,

    /// <summary>
    /// The RM format. This is a multimedia format that is commonly used for streaming audio and video over the internet.
    /// </summary>
    RM = 33
}