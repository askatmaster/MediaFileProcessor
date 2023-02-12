using MediaFileProcessor.Models.Enums;
namespace MediaFileProcessor.Models.Common;

public static class FilesSignatures
{
    private static byte[] JPG => new byte[] { 0xFF, 0xD8, 0xFF };

    private static byte[] JPEG => new byte[] { 0xFF, 0xD8, 0xFF };

    private static byte[] PNG => new byte[] { 0x89, 0x50, 0x4e, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };

    private static byte[] ICO => new byte[] { 0x00, 0x00, 0x01, 0x00 };

    private static byte[] BIN => new byte[] { 0x53, 0x50, 0x30, 0x31 };

    private static byte[] TIFF => new byte[] { 0x4D, 0x4D, 0x00, 0x2A };

    private static byte[] TIF => new byte[] { 0x49, 0x49, 0x2A, 0x00 };

    private static byte[] _3GP => new byte[] { 0x33, 0x67, 0x70, 0x33 };

    private static byte[] MP4 => new byte[] { 0x00, 0x00, 0x00, 0x18, 0x66, 0x74, 0x79, 0x70, 0x4D, 0x53, 0x4E, 0x56 };

    private static byte[] MOV => new byte[] { 0x6D, 0x6F, 0x6F, 0x76 };

    private static byte[] MATROSKA => new byte[] { 0x1A, 0x45, 0xDF, 0xA3 };

    private static byte[] AVI => new byte[] { 0x52, 0x49, 0x46, 0x46 };

    private static byte[] MPEG => new byte[] { 0x00, 0x00, 0x01, 0xBA };

    private static byte[] MPEGTS => new byte[] { 0x47 };

    private static byte[] SVCD => new byte[] { 0x00, 0x00, 0x01, 0xBA, 0x44, 0x00, 0x04, 0x80 };

    private static byte[] GIF => new byte[] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61, 0x1 };

    private static byte[] VOB => new byte[] { 0x00, 0x00, 0x01, 0xBA };

    private static byte[] M2TS => new byte[] { 0x47 };

    private static byte[] MXF => new byte[] { 0x06, 0x0E, 0x2B, 0x34, 0x02, 0x05, 0x01, 0x01, 0x0D, 0x01, 0x02, 0x01, 0x01, 0x02 };

    private static byte[] WEBM => new byte[] { 0x1A, 0x45, 0xDF, 0xA3 };

    private static byte[] GXF => new byte[] { 0x47, 0x58, 0x46 };

    private static byte[] FLV => new byte[] { 0x46, 0x4C, 0x56, 0x01 };

    private static byte[] OGG => new byte[] { 0x4F, 0x67, 0x67, 0x53 };

    private static byte[] WMV => new byte[] { 0x30, 0x26, 0xB2, 0x75, 0x8E, 0x66, 0xCF, 0x11, 0xA6, 0xD9, 0x00, 0xAA, 0x00, 0x62, 0xCE, 0x6C };

    private static byte[] BMP => new byte[] { 0x42, 0x4D };

    private static byte[] ASF => new byte[] { 0x30, 0x26, 0xB2, 0x75, 0x8E, 0x66, 0xCF, 0x11, 0xA6, 0xD9, 0x00, 0xAA, 0x00, 0x62, 0xCE, 0x6C };

    private static byte[] IMAGE2PIPE => new byte[] { 0x69, 0x73, 0x74 };

    private static byte[] IMAGE2 => new byte[] { 0x49, 0x49, 0x2A, 0x00 };

    private static byte[] RAWVIDEO => new byte[] { 0x52, 0x49, 0x46, 0x46 };

    private static byte[] MP3 => new byte[] { 0x49, 0x44, 0x33 };

    private static byte[] RM => new byte[] { 0x2E, 0x52, 0x4D, 0x46 };

    private static byte[] PSD => new byte[] { 0x38, 0x42, 0x50, 0x53 };

    /// <summary>
    /// Get file signature in bytes by its format
    /// </summary>
    /// <param name="outputFormatType">File format</param>
    /// <returns>Byte signature</returns>
    /// <exception cref="NotSupportedException">Exception if file format does not have a specific permanent signature or it is not supported</exception>
    public static byte[] GetSignature(this FileFormatType outputFormatType)
    {
        return outputFormatType switch
        {
            FileFormatType.JPEG => JPEG,
            FileFormatType.JPG => JPG,
            FileFormatType.PNG => PNG,
            FileFormatType._3GP => _3GP,
            FileFormatType.MP4 => MP4,
            FileFormatType.MOV => MOV,
            FileFormatType.MATROSKA => MATROSKA,
            FileFormatType.AVI => AVI,
            FileFormatType.MPEG => MPEG,
            FileFormatType.MPEGTS => MPEGTS,
            FileFormatType.SVCD => SVCD,
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
            FileFormatType.IMAGE2PIPE => IMAGE2PIPE,
            FileFormatType.IMAGE2 => IMAGE2,
            FileFormatType.RAWVIDEO => RAWVIDEO,
            FileFormatType.MP3 => MP3,
            FileFormatType.RM => RM,
            FileFormatType.ICO => ICO,
            FileFormatType.BIN => BIN,
            FileFormatType.TIFF => TIFF,
            FileFormatType.TIF => TIF,
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