using MediaFileProcessor.Models.Enums;
namespace MediaFileProcessor.Models.Common;

public static class FilesSignatures
{
    private static List<byte[]> JPG => new ()
    {
        new byte[] { 0xFF, 0xD8, 0xFF, 0xE0, 0, 0, 0x4A, 0x46, 0x49, 0x46, 0x00 },
        new byte[] { 0xFF, 0xD8, 0xFF, 0xE1, 0, 0, 0x45, 0x78, 0x69, 0x66, 0x00 },
        new byte[] { 0xFF, 0xD8, 0xFF, 0xDB, 0, 0, 0 }
    };

    private static List<byte[]> PNG => new ()
    {
        new byte[] { 0x89, 0x50, 0x4e, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }
    };

    private static List<byte[]> ICO => new ()
    {
        new byte[] { 0x00, 0x00, 0x01, 0x00, 0x01, 0x00 }
    };

    private static List<byte[]> TIFF => new()
    {
        new byte[] { 0x4D, 0x4D, 0x00, 0x2A },
        new byte[] { 0x49, 0x49, 0x2A, 0x00 }
    };

    private static List<byte[]> _3GP => new ()
    {
        new byte[] { 0, 0, 0, 0, 0x66, 0x74, 0x79, 0x70, 0x33, 0x67, 0x70 }
    };

    private static List<byte[]> MP4 => new ()
    {
        new byte[] { 0, 0, 0, 0, 0x66, 0x74, 0x79, 0x70 },
        new byte[] { 0, 0, 0, 0, 0x66, 0x74, 0x79, 0x70 },
        new byte[] { 0, 0, 0, 0, 0x66, 0x74, 0x79, 0x70, 0x6D, 0x70, 0x34, 0x32 },
        new byte[] { 0, 0, 0, 0, 0x66, 0x74, 0x79, 0x70, 0x69, 0x73, 0x6F, 0x6D },
        new byte[] { 0, 0, 0, 0, 0x66, 0x74, 0x79, 0x70, 0x69, 0x73, 0x6F, 0x6D },
        new byte[] { 0, 0, 0, 0, 0x66, 0x74, 0x79, 0x70, 0x69, 0x73, 0x6F, 0x6D }
    };

    private static List<byte[]> MOV => new ()
    {
        new byte[] { 0, 0, 0, 0, 0x66, 0x74, 0x79, 0x70, 0x71, 0x74, 0x20, 0x20, 0x00, 0x00, 0x02, 0x00 }
    };

    private static List<byte[]> MKV => new ()
    {
        new byte[] { 0x1A, 0x45, 0xDF, 0xA3 }
    };

    private static List<byte[]> AVI => new ()
    {
        new byte[] { 0x52, 0x49, 0x46, 0x46, 0, 0, 0, 0, 0x41, 0x56, 0x49, 0x20 },
        new byte[] { 0x52, 0x49, 0x46, 0x46, 0, 0, 0, 0, 0x41, 0x56, 0x49, 0x2D },
        new byte[] { 0x52, 0x49, 0x46, 0x46, 0, 0, 0, 0, 0x41, 0x56, 0x49, 0x38 },
        new byte[] { 0x52, 0x49, 0x46, 0x46, 0, 0, 0, 0, 0x41, 0x56, 0x49, 0x4F }
    };

    private static List<byte[]> MPEG => new ()
    {
        new byte[] { 0x00, 0x00, 0x01, 0xBA }
    };

    private static List<byte[]> GIF => new ()
    {
        new byte[] { 0x47, 0x49, 0x46, 0x38 }
    };

    private static List<byte[]> VOB => new ()
    {
        new byte[] { 0x00, 0x00, 0x01, 0xBA }
    };

    private static List<byte[]> M2TS => new ()
    {
        new byte[] { 0x47, 0x40, 0x00, 0x10 }
    };

    private static List<byte[]> MXF => new ()
    {
        new byte[] { 0x06, 0x0E, 0x2B, 0x34, 0x02, 0x05, 0x01, 0x01, 0x0D, 0x01, 0x02, 0x01, 0x01, 0x02 }
    };

    private static List<byte[]> WEBM => new ()
    {
        new byte[] { 0x1A, 0x45, 0xDF, 0xA3 }
    };

    private static List<byte[]> GXF => new ()
    {
        new byte[] { 0x47, 0x58, 0x46, 0x30 }
    };

    private static List<byte[]> FLV => new ()
    {
        new byte[] { 0x46, 0x4C, 0x56, 0x01 }
    };

    private static List<byte[]> OGG => new ()
    {
        new byte[] { 0x4F, 0x67, 0x67, 0x53 }
    };

    private static List<byte[]> WMV => new ()
    {
        new byte[] { 0x30, 0x26, 0xB2, 0x75, 0x8E, 0x66, 0xCF, 0x11, 0xA6, 0xD9, 0x00, 0xAA, 0x00, 0x62, 0xCE, 0x6C }
    };

    private static List<byte[]> BMP => new ()
    {
        new byte[] { 0x42, 0x4D }
    };

    private static List<byte[]> ASF => new ()
    {
        new byte[] { 0x30, 0x26, 0xB2, 0x75, 0x8E, 0x66, 0xCF, 0x11, 0xA6, 0xD9, 0x00, 0xAA, 0x00, 0x62, 0xCE, 0x6C }
    };

    private static List<byte[]> MP3 => new ()
    {
        new byte[] { 0x49, 0x44, 0x33 }
    };

    private static List<byte[]> RM => new ()
    {
        new byte[] { 0x2E, 0x52, 0x4D, 0x46 }
    };

    private static List<byte[]> PSD => new ()
    {
        new byte[] { 0x38, 0x42, 0x50, 0x53 }
    };

    private static List<byte[]> WEBP => new ()
    {
        new byte[] { 0x52, 0x49, 0x46, 0x46 }
    };

    private static List<byte[]> WAV => new ()
    {
        new byte[] { 0x52, 0x49, 0x46, 0x46, 0, 0, 0, 0, 0x57, 0x41, 0x56, 0x45 },
        new byte[] { 0x52, 0x49, 0x46, 0x46, 0, 0, 0, 0, 0x41, 0x56, 0x49, 0x20 },
        new byte[] { 0x52, 0x49, 0x46, 0x46, 0, 0, 0, 0, 0x66, 0x6D, 0x74, 0x20 },
        new byte[] { 0x52, 0x49, 0x46, 0x46, 0, 0, 0, 0, 0x4D, 0x41, 0x43, 0x20 }
    };

    private static List<byte[]> FLAC => new ()
    {
        new byte[] { 0x66, 0x4C, 0x61, 0x43 },
        new byte[] { 0x66, 0x4C, 0x61, 0x58 },
        new byte[] { 0x66, 0x72, 0x65, 0x65 },
        new byte[] { 0x66, 0x4C, 0x61, 0x54 }
    };

    private static List<byte[]> AAC => new ()
    {
        new byte[] { 0xFF, 0xF1 },
        new byte[] { 0xFF, 0xF9 },
        new byte[] { 0xFF, 0xFA },
        new byte[] { 0xFF, 0xFB }
    };

    private static List<byte[]> WMA => new ()
    {
        new byte[] { 0x30, 0x26, 0xB2, 0x75, 0x8E, 0x66, 0xCF, 0x11, 0xA6, 0xD9, 0x00, 0xAA, 0x00, 0x62, 0xCE, 0x6C },
        new byte[] { 0x02, 0x00, 0x00, 0x00 },
        new byte[] { 0xFE, 0xFF }
    };

    /// <summary>
    /// Get file signature in bytes by its format
    /// </summary>
    /// <param name="outputFormatType">File format</param>
    /// <returns>Byte signature</returns>
    /// <exception cref="NotSupportedException">Exception if file format does not have a specific permanent signature or it is not supported</exception>
    public static List<byte[]> GetSignature(this FileFormatType outputFormatType)
    {
        return outputFormatType switch
        {
            FileFormatType.JPG => JPG,
            FileFormatType.PNG => PNG,
            FileFormatType._3GP => _3GP,
            FileFormatType.MP4 => MP4,
            FileFormatType.MOV => MOV,
            FileFormatType.MKV => MKV,
            FileFormatType.AVI => AVI,
            FileFormatType.MPEG => MPEG,
            FileFormatType.GIF => GIF,
            FileFormatType.VOB => VOB,
            FileFormatType.M2TS => M2TS,
            FileFormatType.MXF => MXF,
            FileFormatType.WEBM => WEBM,
            FileFormatType.GXF => GXF,
            FileFormatType.FLV => FLV,
            FileFormatType.OGG => OGG,
            FileFormatType.WMV => WMV,
            FileFormatType.BMP => BMP,
            FileFormatType.ASF => ASF,
            FileFormatType.MP3 => MP3,
            FileFormatType.RM => RM,
            FileFormatType.ICO => ICO,
            FileFormatType.TIFF => TIFF,
            FileFormatType.WEBP => WEBP,
            FileFormatType.WAV => WAV,
            FileFormatType.FLAC => FLAC,
            FileFormatType.AAC => AAC,
            FileFormatType.WMA => WMA,
            FileFormatType.RAW or FileFormatType.SVG => throw new NotSupportedException("The signatures for \"RAW\" and \"SVG\" "
                                                                                      + "were not specified in the original question, "
                                                                                      + "so their byte arrays are left empty in the code."
                                                                                      + " The actual file signatures for these formats depend on the specific file format and encoding used."
                                                                                      + " In order to determine the correct signatures, "
                                                                                      + "you would need to consult the specifications for the specific file formats you are working with."),
            FileFormatType.PSD => PSD,
            _ => throw new NotSupportedException("This format does not have a signature")
        };
    }
}