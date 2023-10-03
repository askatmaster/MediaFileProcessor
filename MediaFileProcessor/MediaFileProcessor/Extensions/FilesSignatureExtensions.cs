using System.Text;
using MediaFileProcessor.Models.Enums;

namespace MediaFileProcessor.Extensions;

/// <summary>
/// Extensions for file signatures
/// </summary>
public static class FilesSignatureExtensions
{
    /// <summary>
    /// Signatires of JPG
    /// </summary>
    private static List<byte[]> JPG =>
        new ()
        {
            new byte[]
            {
                0xFF, 0xD8, 0xFF, 0xE0, 0, 0, 0x4A, 0x46, 0x49, 0x46,
                0x00
            },
            new byte[]
            {
                0xFF, 0xD8, 0xFF, 0xE1, 0, 0, 0x45, 0x78, 0x69, 0x66,
                0x00
            },
            new byte[]
            {
                0xFF, 0xD8, 0xFF, 0xFE, 0, 0, 0x4C, 0x61, 0x76, 0x63,
                0x35
            },
            new byte[]
            {
                0xFF, 0xD8, 0xFF, 0xDB, 0, 0, 0, 0, 0, 0,
                0
            }
        };

    /// <summary>
    /// Signatires of PNG
    /// </summary>
    private static List<byte[]> PNG =>
        new ()
        {
            new byte[]
            {
                0x89, 0x50, 0x4e, 0x47, 0x0D, 0x0A, 0x1A, 0x0A
            }
        };

    /// <summary>
    /// Signatires of ICO
    /// </summary>
    private static List<byte[]> ICO =>
        new ()
        {
            new byte[]
            {
                0x00, 0x00, 0x01, 0x00, 0x01, 0x00
            }
        };

    /// <summary>
    /// Signatires of TIFF
    /// </summary>
    private static List<byte[]> TIFF =>
        new ()
        {
            new byte[]
            {
                0x4D, 0x4D, 0x00, 0x2A
            },
            new byte[]
            {
                0x49, 0x49, 0x2A, 0x00
            }
        };

    /// <summary>
    /// Signatires of _3GP
    /// </summary>
    private static List<byte[]> _3GP =>
        new ()
        {
            new byte[]
            {
                0, 0, 0, 0, 0x66, 0x74, 0x79, 0x70, 0x33, 0x67,
                0x70
            }
        };

    /// <summary>
    /// Signatires of M4V
    /// </summary>
    private static List<byte[]> M4V =>
        new ()
        {
            new byte[]
            {
                0, 0, 0, 0, 0x66, 0x74, 0x79, 0x70, 0x4D, 0x34,
                0x56, 0x20
            }
        };

    /// <summary>
    /// Signatires of MP4
    /// </summary>
    private static List<byte[]> MP4 =>
        new ()
        {
            new byte[]
            {
                0, 0, 0, 0, 0x66, 0x74, 0x79, 0x70
            },
            new byte[]
            {
                0, 0, 0, 0, 0x66, 0x74, 0x79, 0x70
            },
            new byte[]
            {
                0, 0, 0, 0, 0x66, 0x74, 0x79, 0x70, 0x6D, 0x70,
                0x34, 0x32
            },
            new byte[]
            {
                0, 0, 0, 0, 0x66, 0x74, 0x79, 0x70, 0x69, 0x73,
                0x6F, 0x6D
            },
            new byte[]
            {
                0, 0, 0, 0, 0x66, 0x74, 0x79, 0x70, 0x69, 0x73,
                0x6F, 0x6D
            },
            new byte[]
            {
                0, 0, 0, 0, 0x66, 0x74, 0x79, 0x70, 0x69, 0x73,
                0x6F, 0x6D
            }
        };

    /// <summary>
    /// Signatires of MOV
    /// </summary>
    private static List<byte[]> MOV =>
        new ()
        {
            new byte[]
            {
                0, 0, 0, 0, 0x66, 0x74, 0x79, 0x70, 0x71, 0x74,
                0x20, 0x20, 0x00, 0x00, 0x02, 0x00
            }
        };

    /// <summary>
    /// Signatires of MKV
    /// </summary>
    private static List<byte[]> MKV =>
        new ()
        {
            new byte[]
            {
                0x1A, 0x45, 0xDF, 0xA3
            }
        };

    /// <summary>
    /// Signatires of AVI
    /// </summary>
    private static List<byte[]> AVI =>
        new ()
        {
            new byte[]
            {
                0x52, 0x49, 0x46, 0x46, 0, 0, 0, 0, 0x41, 0x56,
                0x49, 0x20, 0x4C, 0x49, 0x53, 0x54
            },
            new byte[]
            {
                0x52, 0x49, 0x46, 0x46, 0, 0, 0, 0, 0x41, 0x56,
                0x49, 0x2D, 0x6D, 0x6D, 0x76, 0x69
            },
            new byte[]
            {
                0x52, 0x49, 0x46, 0x46, 0, 0, 0, 0, 0x41, 0x56,
                0x49, 0x38, 0x69, 0x64, 0x78, 0x31
            }
        };

    /// <summary>
    /// Signatires of MPEG
    /// </summary>
    private static List<byte[]> MPEG =>
        new ()
        {
            new byte[]
            {
                0x00, 0x00, 0x01, 0xBA
            },
            new byte[]
            {
                0x00, 0x00, 0x01, 0xB3
            },
            new byte[]
            {
                0x00, 0x00, 0x00, 0x01
            }
        };

    /// <summary>
    /// Signatires of GIF
    /// </summary>
    private static List<byte[]> GIF =>
        new ()
        {
            new byte[]
            {
                0x47, 0x49, 0x46, 0x38, 0x39, 0x61
            },
            new byte[]
            {
                0x47, 0x49, 0x46, 0x38, 0x37, 0x61
            }
        };

    /// <summary>
    /// Signatires of VOB
    /// </summary>
    private static List<byte[]> VOB =>
        new ()
        {
            new byte[]
            {
                0x00, 0x00, 0x01, 0xBA, 0x44, 0x00
            }
        };

    /// <summary>
    /// Signatires of M2TS
    /// </summary>
    private static List<byte[]> M2TS =>
        new ()
        {
            new byte[]
            {
                0x47, 0x40, 0x00, 0x10
            },
            new byte[]
            {
                0, 0, 0, 0, 0x47, 0x40, 0x11, 0x10
            }
        };

    /// <summary>
    /// Signatires of MXF
    /// </summary>
    private static List<byte[]> MXF =>
        new ()
        {
            new byte[]
            {
                0x06, 0x0E, 0x2B, 0x34, 0x02, 0x05, 0x01, 0x01, 0x0D, 0x01,
                0x02, 0x01, 0x01, 0x02
            }
        };

    /// <summary>
    /// Signatires of WEBM
    /// </summary>
    private static List<byte[]> WEBM =>
        new ()
        {
            new byte[]
            {
                0x1A, 0x45, 0xDF, 0xA3
            }
        };

    /// <summary>
    /// Signatires of GXF
    /// </summary>
    private static List<byte[]> GXF =>
        new ()
        {
            new byte[]
            {
                0x47, 0x58, 0x46, 0x30
            }
        };

    /// <summary>
    /// Signatires of FLV
    /// </summary>
    private static List<byte[]> FLV =>
        new ()
        {
            new byte[]
            {
                0x46, 0x4C, 0x56, 0x01
            }
        };

    /// <summary>
    /// Signatires of OGG
    /// </summary>
    private static List<byte[]> OGG =>
        new ()
        {
            new byte[]
            {
                0x4F, 0x67, 0x67, 0x53
            }
        };

    /// <summary>
    /// Signatires of WMV
    /// </summary>
    private static List<byte[]> WMV =>
        new ()
        {
            new byte[]
            {
                0x30, 0x26, 0xB2, 0x75, 0x8E, 0x66, 0xCF, 0x11, 0xA6, 0xD9,
                0x00, 0xAA, 0x00, 0x62, 0xCE, 0x6C
            }
        };

    /// <summary>
    /// Signatires of BMP
    /// </summary>
    private static List<byte[]> BMP =>
        new ()
        {
            new byte[]
            {
                0x42, 0x4D
            }
        };

    /// <summary>
    /// Signatires of ASF
    /// </summary>
    private static List<byte[]> ASF =>
        new ()
        {
            new byte[]
            {
                0x30, 0x26, 0xB2, 0x75, 0x8E, 0x66, 0xCF, 0x11, 0xA6, 0xD9,
                0x00, 0xAA, 0x00, 0x62, 0xCE, 0x6C
            }
        };

    /// <summary>
    /// Signatires of MP3
    /// </summary>
    private static List<byte[]> MP3 =>
        new ()
        {
            new byte[]
            {
                0x49, 0x44, 0x33
            }
        };

    /// <summary>
    /// Signatires of RM
    /// </summary>
    private static List<byte[]> RM =>
        new ()
        {
            new byte[]
            {
                0x2E, 0x52, 0x4D, 0x46
            }
        };

    /// <summary>
    /// Signatires of PSD
    /// </summary>
    private static List<byte[]> PSD =>
        new ()
        {
            new byte[]
            {
                0x38, 0x42, 0x50, 0x53
            }
        };

    /// <summary>
    /// Signatires of WEBP
    /// </summary>
    private static List<byte[]> WEBP =>
        new ()
        {
            new byte[]
            {
                0x52, 0x49, 0x46, 0x46, 0, 0, 0, 0, 0x57, 0x45,
                0x42, 0x50
            },
            new byte[]
            {
                0x52, 0x49, 0x46, 0x46, 0, 0, 0, 0, 0x56, 0x50,
                0x38, 0x58
            }
        };

    /// <summary>
    /// Signatires of WAV
    /// </summary>
    private static List<byte[]> WAV =>
        new ()
        {
            new byte[]
            {
                0x52, 0x49, 0x46, 0x46, 0, 0, 0, 0, 0x57, 0x41,
                0x56, 0x45
            },
            new byte[]
            {
                0x52, 0x49, 0x46, 0x46, 0, 0, 0, 0, 0x41, 0x56,
                0x49, 0x20
            },
            new byte[]
            {
                0x52, 0x49, 0x46, 0x46, 0, 0, 0, 0, 0x66, 0x6D,
                0x74, 0x20
            },
            new byte[]
            {
                0x52, 0x49, 0x46, 0x46, 0, 0, 0, 0, 0x4D, 0x41,
                0x43, 0x20
            }
        };

    /// <summary>
    /// Signatires of FLAC
    /// </summary>
    private static List<byte[]> FLAC =>
        new ()
        {
            new byte[]
            {
                0x66, 0x4C, 0x61, 0x43
            },
            new byte[]
            {
                0x66, 0x4C, 0x61, 0x58
            },
            new byte[]
            {
                0x66, 0x72, 0x65, 0x65
            },
            new byte[]
            {
                0x66, 0x4C, 0x61, 0x54
            }
        };

    /// <summary>
    /// Signatires of AAC
    /// </summary>
    private static List<byte[]> AAC =>
        new ()
        {
            new byte[]
            {
                0xFF, 0xF1
            },
            new byte[]
            {
                0xFF, 0xF9
            },
            new byte[]
            {
                0xFF, 0xFA
            },
            new byte[]
            {
                0xFF, 0xFB
            }
        };

    /// <summary>
    /// Signatires of WMA
    /// </summary>
    private static List<byte[]> WMA =>
        new ()
        {
            new byte[]
            {
                0x30, 0x26, 0xB2, 0x75, 0x8E, 0x66, 0xCF, 0x11, 0xA6, 0xD9,
                0x00, 0xAA, 0x00, 0x62, 0xCE, 0x6C
            },
            new byte[]
            {
                0x02, 0x00, 0x00, 0x00, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0
            },
            new byte[]
            {
                0xFE, 0xFF, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0
            }
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
            FileFormatType.M4V => M4V,
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

    /// <summary>
    /// Determines the file format based on the provided byte array signature.
    /// </summary>
    /// <param name="signature">The byte array containing the file signature.</param>
    /// <returns>The detected file format as a <see cref="FileFormatType"/> enumeration value.</returns>
    public static FileFormatType GetFormat(this byte[] signature)
    {
        return signature.Length switch
        {
            >= 3 when IsJPEG(signature) => FileFormatType.JPG,
            >= 8 when IsPNG(signature) => FileFormatType.PNG,
            >= 6 when IsICO(signature) => FileFormatType.ICO,
            >= 4 when IsTIFF(signature) => FileFormatType.TIFF,
            >= 11 when Is3GP(signature) => FileFormatType._3GP,
            >= 12 when IsM4V(signature) => FileFormatType.M4V,
            >= 8 when IsMP4(signature) => FileFormatType.MP4,
            >= 16 when IsMOV(signature) => FileFormatType.MOV,
            >= 6 when IsVOB(signature) => FileFormatType.VOB,
            >= 4 when IsMPEG(signature) => FileFormatType.MPEG,
            >= 16 when IsAVI(signature) => FileFormatType.AVI,
            >= 6 when IsGIF(signature) => FileFormatType.GIF,
            >= 8 when IsM2TS(signature) => FileFormatType.M2TS,
            >= 14 when IsMXF(signature) => FileFormatType.MXF,
            >= 4 when IsGXF(signature) => FileFormatType.GXF,
            >= 4 when IsFLV(signature) => FileFormatType.FLV,
            >= 4 when IsOGG(signature) => FileFormatType.OGG,
            >= 16 when IsWMV(signature) => FileFormatType.WMV,
            >= 2 when IsBMP(signature) => FileFormatType.BMP,
            >= 3 when IsMP3(signature) => FileFormatType.MP3,
            >= 4 when IsRM(signature) => FileFormatType.RM,
            >= 4 when IsPSD(signature) => FileFormatType.PSD,
            >= 12 when IsWEBP(signature) => FileFormatType.WEBP,
            >= 12 when IsWAV(signature) => FileFormatType.WAV,
            >= 4 when IsFLAC(signature) => FileFormatType.FLAC,
            >= 2 when IsAAC(signature) => FileFormatType.AAC,
            >= 16 when IsWMA(signature) => FileFormatType.WMA,
            >= 16 when IsASF(signature) => FileFormatType.ASF,
            >= 4 when IsWEBM(signature) => FileFormatType.WEBM,
            >= 4 when IsMKV(signature) => FileFormatType.MKV,
            >= 4 when IsTS(signature) => FileFormatType.TS,
            _ => throw new Exception("Unable to determine the format")
        };
    }

    /// <summary>
    /// Determines the file format based on the provided ReadOnlySpan of bytes signature.
    /// </summary>
    /// <param name="signature">The ReadOnlySpan of bytes containing the file signature.</param>
    /// <returns>The detected file format as a <see cref="FileFormatType"/> enumeration value.</returns>
    public static FileFormatType GetFormat(this ReadOnlySpan<byte> signature)
    {
        return signature.Length switch
        {
            >= 8 when IsPNG(signature) => FileFormatType.PNG,
            >= 3 when IsJPEG(signature) => FileFormatType.JPG,
            >= 6 when IsICO(signature) => FileFormatType.ICO,
            >= 4 when IsTIFF(signature) => FileFormatType.TIFF,
            >= 11 when Is3GP(signature) => FileFormatType._3GP,
            >= 12 when IsM4V(signature) => FileFormatType.M4V,
            >= 8 when IsMP4(signature) => FileFormatType.MP4,
            >= 16 when IsMOV(signature) => FileFormatType.MOV,
            >= 6 when IsVOB(signature) => FileFormatType.VOB,
            >= 4 when IsMPEG(signature) => FileFormatType.MPEG,
            >= 16 when IsAVI(signature) => FileFormatType.AVI,
            >= 6 when IsGIF(signature) => FileFormatType.GIF,
            >= 8 when IsM2TS(signature) => FileFormatType.M2TS,
            >= 14 when IsMXF(signature) => FileFormatType.MXF,
            >= 4 when IsGXF(signature) => FileFormatType.GXF,
            >= 4 when IsFLV(signature) => FileFormatType.FLV,
            >= 4 when IsOGG(signature) => FileFormatType.OGG,
            >= 16 when IsWMV(signature) => FileFormatType.WMV,
            >= 2 when IsBMP(signature) => FileFormatType.BMP,
            >= 3 when IsMP3(signature) => FileFormatType.MP3,
            >= 4 when IsRM(signature) => FileFormatType.RM,
            >= 4 when IsPSD(signature) => FileFormatType.PSD,
            >= 12 when IsWEBP(signature) => FileFormatType.WEBP,
            >= 12 when IsWAV(signature) => FileFormatType.WAV,
            >= 4 when IsFLAC(signature) => FileFormatType.FLAC,
            >= 2 when IsAAC(signature) => FileFormatType.AAC,
            >= 16 when IsWMA(signature) => FileFormatType.WMA,
            >= 16 when IsASF(signature) => FileFormatType.ASF,
            >= 4 when IsWEBM(signature) => FileFormatType.WEBM,
            >= 4 when IsMKV(signature) => FileFormatType.MKV,
            >= 4 when IsTS(signature) => FileFormatType.TS,
            _ => throw new Exception("Unable to determine the format")
        };
    }

    /// <summary>
    /// Checks if the provided byte array represents a Transport Stream (TS) file by verifying its signature.
    /// </summary>
    /// <param name="buffer">The byte array to check.</param>
    /// <returns>True if the byte array represents a TS file; otherwise, false.</returns>
    public static bool IsTS(this byte[] buffer)
    {
        if (buffer.Length < 3)
            throw new ArgumentException("The signature that you are verifying must be at least 3 bytes long");

        return buffer[0] == 0x47;
    }

    /// <summary>
    /// Checks if the provided ReadOnlySpan of bytes represents a Transport Stream (TS) file by verifying its signature.
    /// </summary>
    /// <param name="buffer">The ReadOnlySpan of bytes to check.</param>
    /// <returns>True if the ReadOnlySpan of bytes represents a TS file; otherwise, false.</returns>
    public static bool IsTS(this ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length < 3)
            throw new ArgumentException("The signature that you are verifying must be at least 3 bytes long");

        return buffer[0] == 0x47;
    }

    /// <summary>
    /// Checks if the provided byte array represents a JPEG (Joint Photographic Experts Group) image by verifying its signature.
    /// </summary>
    /// <param name="buffer">The byte array to check.</param>
    /// <returns>True if the byte array represents a JPEG image; otherwise, false.</returns>
    public static bool IsJPEG(this byte[] buffer)
    {
        if (buffer.Length < 3)
            throw new ArgumentException("The signature that you are verifying must be at least 3 bytes long");

        return buffer[0] == 0xFF && buffer[1] == 0xD8 && buffer[2] == 0xFF;
    }

    /// <summary>
    /// Checks if the provided ReadOnlySpan of bytes represents a JPEG (Joint Photographic Experts Group) image by verifying its signature.
    /// </summary>
    /// <param name="buffer">The ReadOnlySpan of bytes to check.</param>
    /// <returns>True if the ReadOnlySpan of bytes represents a JPEG image; otherwise, false.</returns>
    public static bool IsJPEG(this ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length < 3)
            throw new ArgumentException("The signature that you are verifying must be at least 3 bytes long");

        return buffer[0] == 0xFF && buffer[1] == 0xD8 && buffer[2] == 0xFF;
    }

    /// <summary>
    /// Checks if the provided byte array represents a PNG (Portable Network Graphics) image by verifying its signature.
    /// </summary>
    /// <param name="buffer">The byte array to check.</param>
    /// <returns>True if the byte array represents a PNG image; otherwise, false.</returns>
    public static bool IsPNG(this byte[] buffer)
    {
        if (buffer.Length < 8)
            throw new ArgumentException("The signature that you are verifying must be at least 8 bytes long");

        return buffer[0] == 0x89
               && buffer[1] == 0x50
               && buffer[2] == 0x4E
               && buffer[3] == 0x47
               && buffer[4] == 0x0D
               && buffer[5] == 0x0A
               && buffer[6] == 0x1A
               && buffer[7] == 0x0A;
    }

    /// <summary>
    /// Checks if the provided ReadOnlySpan of bytes represents a PNG (Portable Network Graphics) image by verifying its signature.
    /// </summary>
    /// <param name="buffer">The ReadOnlySpan of bytes to check.</param>
    /// <returns>True if the ReadOnlySpan of bytes represents a PNG image; otherwise, false.</returns>
    public static bool IsPNG(this ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length < 8)
            throw new ArgumentException("The signature that you are verifying must be at least 8 bytes long");

        return buffer[0] == 0x89
               && buffer[1] == 0x50
               && buffer[2] == 0x4E
               && buffer[3] == 0x47
               && buffer[4] == 0x0D
               && buffer[5] == 0x0A
               && buffer[6] == 0x1A
               && buffer[7] == 0x0A;
    }

    /// <summary>
    /// Checks if the provided byte array represents an ICO (Icon) file by verifying its signature.
    /// </summary>
    /// <param name="buffer">The byte array to check.</param>
    /// <returns>True if the byte array represents an ICO file; otherwise, false.</returns>
    public static bool IsICO(this byte[] buffer)
    {
        if (buffer.Length < 6)
            throw new ArgumentException("The signature that you are verifying must be at least 6 bytes long");

        return buffer[0] == 0x00 && buffer[1] == 0x00 && buffer[2] == 0x01 && buffer[3] == 0x00 && buffer[4] == 0x01 && buffer[5] == 0x00;
    }

    /// <summary>
    /// Checks if the provided ReadOnlySpan of bytes represents an ICO (Icon) file by verifying its signature.
    /// </summary>
    /// <param name="buffer">The ReadOnlySpan of bytes to check.</param>
    /// <returns>True if the ReadOnlySpan of bytes represents an ICO file; otherwise, false.</returns>
    public static bool IsICO(this ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length < 6)
            throw new ArgumentException("The signature that you are verifying must be at least 6 bytes long");

        return buffer[0] == 0x00 && buffer[1] == 0x00 && buffer[2] == 0x01 && buffer[3] == 0x00 && buffer[4] == 0x01 && buffer[5] == 0x00;
    }

    /// <summary>
    /// Checks if the provided byte array represents a TIFF (Tagged Image File Format) file by verifying its signature.
    /// </summary>
    /// <param name="buffer">The byte array to check.</param>
    /// <returns>True if the byte array represents a TIFF file; otherwise, false.</returns>
    public static bool IsTIFF(this byte[] buffer)
    {
        if (buffer.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        return (buffer[0] == 0x4D && buffer[1] == 0x4D && buffer[2] == 0x00 && buffer[3] == 0x2A)
               || (buffer[0] == 0x49 && buffer[1] == 0x49 && buffer[2] == 0x2A && buffer[3] == 0x00);
    }

    /// <summary>
    /// Checks if the provided ReadOnlySpan of bytes represents a TIFF (Tagged Image File Format) file by verifying its signature.
    /// </summary>
    /// <param name="buffer">The ReadOnlySpan of bytes to check.</param>
    /// <returns>True if the ReadOnlySpan of bytes represents a TIFF file; otherwise, false.</returns>
    public static bool IsTIFF(this ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        return (buffer[0] == 0x4D && buffer[1] == 0x4D && buffer[2] == 0x00 && buffer[3] == 0x2A)
               || (buffer[0] == 0x49 && buffer[1] == 0x49 && buffer[2] == 0x2A && buffer[3] == 0x00);
    }

    /// <summary>
    /// Checks if the provided byte array represents a 3GP (3rd Generation Partnership Project) file by verifying its signature.
    /// </summary>
    /// <param name="buffer">The byte array to check.</param>
    /// <returns>True if the byte array represents a 3GP file; otherwise, false.</returns>
    public static bool Is3GP(this byte[] buffer)
    {
        if (buffer.Length < 11)
            throw new ArgumentException("The signature that you are verifying must be at least 11 bytes long");

        return buffer[4] == 0x66 && buffer[5] == 0x74 && buffer[6] == 0x79 && buffer[7] == 0x70 && buffer[8] == 0x33 && buffer[9] == 0x67 && buffer[10] == 0x70;
    }

    /// <summary>
    /// Checks if the provided ReadOnlySpan of bytes represents a 3GP (3rd Generation Partnership Project) file by verifying its signature.
    /// </summary>
    /// <param name="buffer">The ReadOnlySpan of bytes to check.</param>
    /// <returns>True if the ReadOnlySpan of bytes represents a 3GP file; otherwise, false.</returns>
    public static bool Is3GP(this ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length < 11)
            throw new ArgumentException("The signature that you are verifying must be at least 11 bytes long");

        return buffer[4] == 0x66 && buffer[5] == 0x74 && buffer[6] == 0x79 && buffer[7] == 0x70 && buffer[8] == 0x33 && buffer[9] == 0x67 && buffer[10] == 0x70;
    }

    /// <summary>
    /// Checks if the provided byte array represents an M4V file by verifying its signature.
    /// </summary>
    /// <param name="buffer">The byte array to check.</param>
    /// <returns>True if the byte array represents an M4V file; otherwise, false.</returns>
    public static bool IsM4V(this byte[] buffer)
    {
        if (buffer.Length < 12)
            throw new ArgumentException("The signature that you are verifying must be at least 12 bytes long");

        return buffer[4] == 0x66
               && buffer[5] == 0x74
               && buffer[6] == 0x79
               && buffer[7] == 0x70
               && buffer[8] == 0x4D
               && buffer[9] == 0x34
               && buffer[10] == 0x56
               && buffer[11] == 0x20;
    }

    /// <summary>
    /// Checks if the provided ReadOnlySpan of bytes represents an M4V file by verifying its signature.
    /// </summary>
    /// <param name="buffer">The ReadOnlySpan of bytes to check.</param>
    /// <returns>True if the ReadOnlySpan of bytes represents an M4V file; otherwise, false.</returns>
    public static bool IsM4V(this ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length < 12)
            throw new ArgumentException("The signature that you are verifying must be at least 12 bytes long");

        return buffer[4] == 0x66
               && buffer[5] == 0x74
               && buffer[6] == 0x79
               && buffer[7] == 0x70
               && buffer[8] == 0x4D
               && buffer[9] == 0x34
               && buffer[10] == 0x56
               && buffer[11] == 0x20;
    }

    /// <summary>
    /// Checks if the provided byte array represents an MP4 file by verifying its signature.
    /// </summary>
    /// <param name="buffer">The byte array to check.</param>
    /// <returns>True if the byte array represents an MP4 file; otherwise, false.</returns>
    public static bool IsMP4(this byte[] buffer)
    {
        if (buffer.Length < 8)
            throw new ArgumentException("The signature that you are verifying must be at least 8 bytes long");

        return buffer[4] == 0x66 && buffer[5] == 0x74 && buffer[6] == 0x79 && buffer[7] == 0x70;
    }

    /// <summary>
    /// Checks if the provided ReadOnlySpan of bytes represents an MP4 file by verifying its signature.
    /// </summary>
    /// <param name="buffer">The ReadOnlySpan of bytes to check.</param>
    /// <returns>True if the ReadOnlySpan of bytes represents an MP4 file; otherwise, false.</returns>
    public static bool IsMP4(this ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length < 8)
            throw new ArgumentException("The signature that you are verifying must be at least 8 bytes long");

        return buffer[4] == 0x66 && buffer[5] == 0x74 && buffer[6] == 0x79 && buffer[7] == 0x70;
    }

    /// <summary>
    /// Checks if the provided byte array represents an MOV file by verifying its signature.
    /// </summary>
    /// <param name="buffer">The byte array to check.</param>
    /// <returns>True if the byte array represents an MOV file; otherwise, false.</returns>
    public static bool IsMOV(this byte[] buffer)
    {
        if (buffer.Length < 16)
            throw new ArgumentException("The signature that you are verifying must be at least 16 bytes long");

        return buffer[4] == 0x66
               && buffer[5] == 0x74
               && buffer[6] == 0x79
               && buffer[7] == 0x70
               && buffer[8] == 0x71
               && buffer[9] == 0x74
               && buffer[10] == 0x20
               && buffer[11] == 0x20
               && buffer[12] == 0x00
               && buffer[13] == 0x00
               && buffer[14] == 0x02
               && buffer[15] == 0x00;
    }

    /// <summary>
    /// Checks if the provided ReadOnlySpan of bytes represents an MOV file by verifying its signature.
    /// </summary>
    /// <param name="buffer">The ReadOnlySpan of bytes to check.</param>
    /// <returns>True if the ReadOnlySpan of bytes represents an MOV file; otherwise, false.</returns>
    public static bool IsMOV(this ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length < 16)
            throw new ArgumentException("The signature that you are verifying must be at least 16 bytes long");

        return buffer[4] == 0x66
               && buffer[5] == 0x74
               && buffer[6] == 0x79
               && buffer[7] == 0x70
               && buffer[8] == 0x71
               && buffer[9] == 0x74
               && buffer[10] == 0x20
               && buffer[11] == 0x20
               && buffer[12] == 0x00
               && buffer[13] == 0x00
               && buffer[14] == 0x02
               && buffer[15] == 0x00;
    }

    /// <summary>
    /// Checks if the provided byte array represents an MKV (Matroska) file by verifying its signature and format.
    /// </summary>
    /// <param name="buffer">The byte array to check.</param>
    /// <returns>True if the byte array represents an MKV file; otherwise, false.</returns>
    public static bool IsMKV(this byte[] buffer)
    {
        if (buffer.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        if (!(buffer[0] == 0x1A && buffer[1] == 0x45 && buffer[2] == 0xDF && buffer[3] == 0xA3))
            return false;

        var format = GetFileFormat(buffer);

        return format switch
        {
            null => false,
            "matroska" => true,
            _ => false
        };
    }

    /// <summary>
    /// Checks if the provided stream represents an MKV (Matroska) file by verifying its signature and format.
    /// </summary>
    /// <param name="stream">The stream to check.</param>
    /// <returns>True if the stream represents an MKV file; otherwise, false.</returns>
    public static bool IsMKV(this Stream stream)
    {
        if (stream.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        var buffer = new byte[4];
        stream.Read(buffer, 0, buffer.Length);

        if (!(buffer[0] == 0x1A && buffer[1] == 0x45 && buffer[2] == 0xDF && buffer[3] == 0xA3))
            return false;

        stream.Seek(0, SeekOrigin.Begin);

        var format = GetFileFormat(stream);

        stream.Seek(0, SeekOrigin.Begin);

        return format switch
        {
            null => false,
            "matroska" => true,
            _ => false
        };
    }

    /// <summary>
    /// Checks if the provided ReadOnlySpan of bytes represents an MKV (Matroska) file by verifying its signature and format.
    /// </summary>
    /// <param name="buffer">The ReadOnlySpan of bytes to check.</param>
    /// <returns>True if the ReadOnlySpan of bytes represents an MKV file; otherwise, false.</returns>
    public static bool IsMKV(this ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        if (!(buffer[0] == 0x1A && buffer[1] == 0x45 && buffer[2] == 0xDF && buffer[3] == 0xA3))
            return false;

        var format = GetFileFormat(buffer.ToArray());

        return format switch
        {
            null => false,
            "matroska" => true,
            _ => false
        };
    }

    /// <summary>
    /// Gets the file format from a given stream by examining its signature.
    /// </summary>
    /// <param name="stream">The input stream to analyze.</param>
    /// <returns>The detected file format (e.g., "webm" or "matroska") or null if the format is not recognized.</returns>
    private static string? GetFileFormat(Stream stream)
    {
        var buffer = new byte[4];
        stream.Read(buffer, 0, buffer.Length);

        if (!CompareByteArrays(buffer, MKV[0]))
            return null; // The file signature does not match EBML

        while (stream.Position < stream.Length)
        {
            var currentByte = stream.ReadByte();

            if (currentByte == 0x42)
            {
                var nextBytePosition = stream.Position;
                var nextByte = stream.ReadByte();

                if (nextByte == 0x82)
                {
                    var dataSize = ReadVariableLengthInteger(stream);
                    var docTypeBuffer = new byte[dataSize];
                    stream.Read(docTypeBuffer, 0, dataSize);

                    var docType = Encoding.ASCII.GetString(docTypeBuffer);

                    if (docType is "webm" or "matroska")
                        return docType;
                }
                else
                {
                    stream.Position = nextBytePosition;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Gets the file format from a given byte array by examining its signature.
    /// </summary>
    /// <param name="fileData">The input byte array to analyze.</param>
    /// <returns>The detected file format (e.g., "webm" or "matroska") or null if the format is not recognized.</returns>
    private static string? GetFileFormat(byte[] fileData)
    {
        var position = 0;

        if (!CompareByteArrays(MKV[0], fileData, position))
            return null; // The file signature does not match EBML

        position += MKV[0].Length;

        while (position < fileData.Length)
        {
            if (fileData[position] == 0x42)
            {
                var nextBytePosition = position + 1;

                if (fileData[nextBytePosition] == 0x82)
                {
                    position += 2;
                    var dataSize = ReadVariableLengthInteger(fileData, ref position);
                    var docTypeBuffer = new byte[dataSize];
                    Array.Copy(fileData, position, docTypeBuffer, 0, dataSize);

                    var docType = Encoding.ASCII.GetString(docTypeBuffer);

                    if (docType is "webm" or "matroska")
                        return docType;
                }
                else
                {
                    position = nextBytePosition;
                }
            }
            else
            {
                position++;
            }
        }

        return null;
    }

    /// <summary>
    /// Reads a variable-length integer from a stream.
    /// </summary>
    /// <param name="fs">The stream to read from.</param>
    /// <returns>The variable-length integer value read from the stream.</returns>
    private static int ReadVariableLengthInteger(Stream fs)
    {
        var firstByte = fs.ReadByte();
        var bytesRead = 0;

        if (firstByte == -1)
            return -1;

        var dataMask = 0x7F;

        for (var i = 7; i >= 0; i--)
        {
            if ((firstByte & (1 << i)) != 0)
            {
                bytesRead = 8 - i;

                break;
            }

            dataMask >>= 1;
        }

        var value = firstByte & dataMask;

        for (var i = 1; i < bytesRead; i++)
        {
            var nextByte = fs.ReadByte();

            if (nextByte == -1)
                return -1;

            value = (value << 7) | (nextByte & 0x7F);
        }

        return value;
    }

    /// <summary>
    /// Reads a variable-length integer from a byte array at a specified position.
    /// </summary>
    /// <param name="data">The byte array to read from.</param>
    /// <param name="position">The starting position in the byte array.</param>
    /// <returns>The variable-length integer value read from the byte array.</returns>
    private static int ReadVariableLengthInteger(byte[] data, ref int position)
    {
        int firstByte = data[position++];
        var bytesRead = 0;

        if (firstByte == -1)
            return -1;

        var dataMask = 0x7F;

        for (var i = 7; i >= 0; i--)
        {
            if ((firstByte & (1 << i)) != 0)
            {
                bytesRead = 8 - i;

                break;
            }

            dataMask >>= 1;
        }

        var value = firstByte & dataMask;

        for (var i = 1; i < bytesRead; i++)
        {
            int nextByte = data[position++];
            value = (value << 7) | (nextByte & 0x7F);
        }

        return value;
    }

    /// <summary>
    /// Compares two byte arrays for equality.
    /// </summary>
    /// <param name="arr1">The first byte array to compare.</param>
    /// <param name="arr2">The second byte array to compare.</param>
    /// <returns>True if the byte arrays are equal; otherwise, false.</returns>
    private static bool CompareByteArrays(byte[] arr1, byte[] arr2)
    {
        if (arr1.Length != arr2.Length)
            return false;

        for (var i = 0; i < arr1.Length; i++)
        {
            if (arr1[i] != arr2[i])
                return false;
        }

        return true;
    }

    /// <summary>
    /// Compares a portion of one byte array with another byte array.
    /// </summary>
    /// <param name="arr1">The byte array to compare.</param>
    /// <param name="arr2">The byte array to compare against.</param>
    /// <param name="position">The starting position in the second byte array.</param>
    /// <returns>True if the portion of the first byte array matches the second byte array; otherwise, false.</returns>
    private static bool CompareByteArrays(byte[] arr1,
                                          byte[] arr2,
                                          int position)
    {
        if (arr1.Length + position > arr2.Length)
            return false;

        for (var i = 0; i < arr1.Length; i++)
        {
            if (arr1[i] != arr2[position + i])
                return false;
        }

        return true;
    }

    /// <summary>
    /// Checks if the provided byte array represents an AVI file by verifying its signature.
    /// </summary>
    /// <param name="buffer">The byte array to check.</param>
    /// <returns>True if the byte array represents an AVI file; otherwise, false.</returns>
    public static bool IsAVI(this byte[] buffer)
    {
        if (buffer.Length < 16)
            throw new ArgumentException("The signature that you are verifying must be at least 16 bytes long");

        return (buffer[0] == 0x52
                && buffer[1] == 0x49
                && buffer[2] == 0x46
                && buffer[3] == 0x46
                && buffer[8] == 0x41
                && buffer[9] == 0x56
                && buffer[10] == 0x49
                && buffer[11] == 0x20
                && buffer[12] == 0x4C
                && buffer[13] == 0x49
                && buffer[14] == 0x53
                && buffer[15] == 0x54)
               || (buffer[0] == 0x52
                   && buffer[1] == 0x49
                   && buffer[2] == 0x46
                   && buffer[3] == 0x46
                   && buffer[8] == 0x41
                   && buffer[9] == 0x56
                   && buffer[10] == 0x49
                   && buffer[11] == 0x2D
                   && buffer[12] == 0x6D
                   && buffer[13] == 0x6D
                   && buffer[14] == 0x76
                   && buffer[15] == 0x69)
               || (buffer[0] == 0x52
                   && buffer[1] == 0x49
                   && buffer[2] == 0x46
                   && buffer[3] == 0x46
                   && buffer[8] == 0x41
                   && buffer[9] == 0x56
                   && buffer[10] == 0x49
                   && buffer[11] == 0x38
                   && buffer[12] == 0x69
                   && buffer[13] == 0x64
                   && buffer[14] == 0x78
                   && buffer[15] == 0x31);
    }

    /// <summary>
    /// Checks if the provided ReadOnlySpan of bytes represents an AVI file by verifying its signature.
    /// </summary>
    /// <param name="buffer">The ReadOnlySpan of bytes to check.</param>
    /// <returns>True if the ReadOnlySpan of bytes represents an AVI file; otherwise, false.</returns>
    public static bool IsAVI(this ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length < 16)
            throw new ArgumentException("The signature that you are verifying must be at least 16 bytes long");

        return (buffer[0] == 0x52
                && buffer[1] == 0x49
                && buffer[2] == 0x46
                && buffer[3] == 0x46
                && buffer[8] == 0x41
                && buffer[9] == 0x56
                && buffer[10] == 0x49
                && buffer[11] == 0x20
                && buffer[12] == 0x4C
                && buffer[13] == 0x49
                && buffer[14] == 0x53
                && buffer[15] == 0x54)
               || (buffer[0] == 0x52
                   && buffer[1] == 0x49
                   && buffer[2] == 0x46
                   && buffer[3] == 0x46
                   && buffer[8] == 0x41
                   && buffer[9] == 0x56
                   && buffer[10] == 0x49
                   && buffer[11] == 0x2D
                   && buffer[12] == 0x6D
                   && buffer[13] == 0x6D
                   && buffer[14] == 0x76
                   && buffer[15] == 0x69)
               || (buffer[0] == 0x52
                   && buffer[1] == 0x49
                   && buffer[2] == 0x46
                   && buffer[3] == 0x46
                   && buffer[8] == 0x41
                   && buffer[9] == 0x56
                   && buffer[10] == 0x49
                   && buffer[11] == 0x38
                   && buffer[12] == 0x69
                   && buffer[13] == 0x64
                   && buffer[14] == 0x78
                   && buffer[15] == 0x31);
    }

    /// <summary>
    /// Checks if the provided byte array represents an MPEG file by verifying its signature.
    /// </summary>
    /// <param name="buffer">The byte array to check.</param>
    /// <returns>True if the byte array represents an MPEG file; otherwise, false.</returns>
    public static bool IsMPEG(this byte[] buffer)
    {
        if (buffer.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        return (buffer[0] == 0x00 && buffer[1] == 0x00 && buffer[2] == 0x01 && buffer[3] == 0xBA)
               || (buffer[0] == 0x00 && buffer[1] == 0x00 && buffer[2] == 0x01 && buffer[3] == 0xB3)
               || (buffer[0] == 0x00 && buffer[1] == 0x00 && buffer[2] == 0x00 && buffer[3] == 0x01);
    }

    /// <summary>
    /// Checks if the provided ReadOnlySpan of bytes represents an MPEG file by verifying its signature.
    /// </summary>
    /// <param name="buffer">The ReadOnlySpan of bytes to check.</param>
    /// <returns>True if the ReadOnlySpan of bytes represents an MPEG file; otherwise, false.</returns>
    public static bool IsMPEG(this ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        return (buffer[0] == 0x00 && buffer[1] == 0x00 && buffer[2] == 0x01 && buffer[3] == 0xBA)
               || (buffer[0] == 0x00 && buffer[1] == 0x00 && buffer[2] == 0x01 && buffer[3] == 0xB3)
               || (buffer[0] == 0x00 && buffer[1] == 0x00 && buffer[2] == 0x00 && buffer[3] == 0x01);
    }

    /// <summary>
    /// Checks if the provided byte array represents a GIF file by verifying its signature.
    /// </summary>
    /// <param name="buffer">The byte array to check.</param>
    /// <returns>True if the byte array represents a GIF file; otherwise, false.</returns>
    public static bool IsGIF(this byte[] buffer)
    {
        if (buffer.Length < 6)
            throw new ArgumentException("The signature that you are verifying must be at least 6 bytes long");

        return (buffer[0] == 0x47 && buffer[1] == 0x49 && buffer[2] == 0x46 && buffer[3] == 0x38 && buffer[4] == 0x39 && buffer[5] == 0x61)
               || (buffer[0] == 0x47 && buffer[1] == 0x49 && buffer[2] == 0x46 && buffer[3] == 0x38 && buffer[4] == 0x37 && buffer[5] == 0x61);
    }

    /// <summary>
    /// Checks if the provided ReadOnlySpan of bytes represents a GIF file by verifying its signature.
    /// </summary>
    /// <param name="buffer">The ReadOnlySpan of bytes to check.</param>
    /// <returns>True if the ReadOnlySpan of bytes represents a GIF file; otherwise, false.</returns>
    public static bool IsGIF(this ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length < 6)
            throw new ArgumentException("The signature that you are verifying must be at least 6 bytes long");

        return (buffer[0] == 0x47 && buffer[1] == 0x49 && buffer[2] == 0x46 && buffer[3] == 0x38 && buffer[4] == 0x39 && buffer[5] == 0x61)
               || (buffer[0] == 0x47 && buffer[1] == 0x49 && buffer[2] == 0x46 && buffer[3] == 0x38 && buffer[4] == 0x37 && buffer[5] == 0x61);
    }

    /// <summary>
    /// Checks if the provided byte array represents a VOB file by verifying its signature.
    /// </summary>
    /// <param name="buffer">The byte array to check.</param>
    /// <returns>True if the byte array represents a VOB file; otherwise, false.</returns>
    public static bool IsVOB(this byte[] buffer)
    {
        if (buffer.Length < 6)
            throw new ArgumentException("The signature that you are verifying must be at least 6 bytes long");

        return buffer[0] == 0x00 && buffer[1] == 0x00 && buffer[2] == 0x01 && buffer[3] == 0xBA && buffer[4] == 0x44 && buffer[5] == 0x00;
    }

    /// <summary>
    /// Checks if the provided ReadOnlySpan of bytes represents a VOB file by verifying its signature.
    /// </summary>
    /// <param name="buffer">The ReadOnlySpan of bytes to check.</param>
    /// <returns>True if the ReadOnlySpan of bytes represents a VOB file; otherwise, false.</returns>
    public static bool IsVOB(this ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length < 6)
            throw new ArgumentException("The signature that you are verifying must be at least 6 bytes long");

        return buffer[0] == 0x00 && buffer[1] == 0x00 && buffer[2] == 0x01 && buffer[3] == 0xBA && buffer[4] == 0x44 && buffer[5] == 0x00;
    }

    /// <summary>
    /// Checks if the provided byte array represents an M2TS file by verifying its signature.
    /// </summary>
    /// <param name="buffer">The byte array to check.</param>
    /// <returns>True if the byte array represents an M2TS file; otherwise, false.</returns>
    public static bool IsM2TS(this byte[] buffer)
    {
        if (buffer.Length < 8)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        return (buffer[0] == 0x47 && buffer[1] == 0x40 && buffer[2] == 0x00 && buffer[3] == 0x10)
               || (buffer[4] == 0x47 && buffer[5] == 0x40 && buffer[6] == 0x11 && buffer[7] == 0x10);
    }

    /// <summary>
    /// Checks if the provided ReadOnlySpan of bytes represents an M2TS file by verifying its signature.
    /// </summary>
    /// <param name="buffer">The ReadOnlySpan of bytes to check.</param>
    /// <returns>True if the byte array represents an M2TS file; otherwise, false.</returns>
    public static bool IsM2TS(this ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length < 8)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        return (buffer[0] == 0x47 && buffer[1] == 0x40 && buffer[2] == 0x00 && buffer[3] == 0x10)
               || (buffer[4] == 0x47 && buffer[5] == 0x40 && buffer[6] == 0x11 && buffer[7] == 0x10);
    }

    /// <summary>
    /// Determines if the given byte array is an MXF file based on its signature.
    /// </summary>
    /// <param name="buffer">The byte array to check.</param>
    /// <returns>True if the byte array represents an MXF file, false otherwise.</returns>
    /// <exception cref="ArgumentException">Thrown when the buffer length is less than 14.</exception>
    public static bool IsMXF(this byte[] buffer)
    {
        if (buffer.Length < 14)
            throw new ArgumentException("The signature that you are verifying must be at least 14 bytes long");

        return buffer[0] == 0x06
               && buffer[1] == 0x0E
               && buffer[2] == 0x2B
               && buffer[3] == 0x34
               && buffer[4] == 0x02
               && buffer[5] == 0x05
               && buffer[6] == 0x01
               && buffer[7] == 0x01
               && buffer[8] == 0x0D
               && buffer[9] == 0x01
               && buffer[10] == 0x02
               && buffer[11] == 0x01
               && buffer[12] == 0x01
               && buffer[13] == 0x02;
    }

    /// <summary>
    /// Determines if the given ReadOnlySpan is an MXF file based on its signature.
    /// </summary>
    /// <param name="buffer">The byte span to check.</param>
    /// <returns>True if the byte span represents an MXF file, false otherwise.</returns>
    /// <exception cref="ArgumentException">Thrown when the buffer length is less than 14.</exception>
    public static bool IsMXF(this ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length < 14)
            throw new ArgumentException("The signature that you are verifying must be at least 14 bytes long");

        return buffer[0] == 0x06
               && buffer[1] == 0x0E
               && buffer[2] == 0x2B
               && buffer[3] == 0x34
               && buffer[4] == 0x02
               && buffer[5] == 0x05
               && buffer[6] == 0x01
               && buffer[7] == 0x01
               && buffer[8] == 0x0D
               && buffer[9] == 0x01
               && buffer[10] == 0x02
               && buffer[11] == 0x01
               && buffer[12] == 0x01
               && buffer[13] == 0x02;
    }

    /// <summary>
    /// Determines if the given byte array is a WEBM file based on its signature and format.
    /// </summary>
    /// <param name="buffer">The byte array to check.</param>
    /// <returns>True if the byte array represents a WEBM file, false otherwise.</returns>
    /// <exception cref="ArgumentException">Thrown when the buffer length is less than 4.</exception>
    public static bool IsWEBM(this byte[] buffer)
    {
        if (buffer.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        if (!(buffer[0] == 0x1A && buffer[1] == 0x45 && buffer[2] == 0xDF && buffer[3] == 0xA3))
            return false;

        var format = GetFileFormat(buffer);

        return format switch
        {
            null => false,
            "webm" => true,
            _ => false
        };
    }

    /// <summary>
    /// Determines if the given Stream is a WEBM file based on its signature and format.
    /// </summary>
    /// <param name="stream">The Stream to check.</param>
    /// <returns>True if the Stream represents a WEBM file, false otherwise.</returns>
    /// <exception cref="ArgumentException">Thrown when the stream length is less than 4.</exception>
    public static bool IsWEBM(this Stream stream)
    {
        if (stream.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        var buffer = new byte[4];
        stream.Read(buffer, 0, buffer.Length);

        if (!(buffer[0] == 0x1A && buffer[1] == 0x45 && buffer[2] == 0xDF && buffer[3] == 0xA3))
            return false;

        stream.Seek(0, SeekOrigin.Begin);

        var format = GetFileFormat(stream);

        stream.Seek(0, SeekOrigin.Begin);

        return format switch
        {
            null => false,
            "webm" => true,
            _ => false
        };
    }

    /// <summary>
    /// Determines if the given ReadOnlySpan is a WEBM file based on its signature.
    /// </summary>
    /// <param name="buffer">The byte span to check.</param>
    /// <returns>True if the byte span represents a WEBM file, false otherwise.</returns>
    /// <exception cref="ArgumentException">Thrown when the buffer length is less than 4.</exception>
    public static bool IsWEBM(this ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        if (!(buffer[0] == 0x1A && buffer[1] == 0x45 && buffer[2] == 0xDF && buffer[3] == 0xA3))
            return false;

        var format = GetFileFormat(buffer.ToArray());

        return format switch
        {
            null => false,
            "webm" => true,
            _ => false
        };
    }

    /// <summary>
    /// Determines if the given byte array is a GXF file based on its signature.
    /// </summary>
    /// <param name="buffer">The byte array to check.</param>
    /// <returns>True if the byte array represents a GXF file, false otherwise.</returns>
    /// <exception cref="ArgumentException">Thrown when the buffer length is less than 4.</exception>
    public static bool IsGXF(this byte[] buffer)
    {
        if (buffer.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        return buffer[0] == 0x47 && buffer[1] == 0x58 && buffer[2] == 0x46 && buffer[3] == 0x30;
    }

    //TODO fix recognition
    /// <summary>
    /// Determines if the given ReadOnlySpan is a GXF file based on its signature.
    /// </summary>
    /// <param name="buffer">The byte span to check.</param>
    /// <returns>True if the byte span represents a GXF file, false otherwise.</returns>
    /// <exception cref="ArgumentException">Thrown when the buffer length is less than 4.</exception>
    public static bool IsGXF(this ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        return buffer[0] == 0x47 && buffer[1] == 0x58 && buffer[2] == 0x46 && buffer[3] == 0x30;
    }

    /// <summary>
    /// Determines if the given byte array is a FLV file based on its signature.
    /// </summary>
    /// <param name="buffer">The byte array to check.</param>
    /// <returns>True if the byte array represents a FLV file, false otherwise.</returns>
    /// <exception cref="ArgumentException">Thrown when the buffer length is less than 4.</exception>
    public static bool IsFLV(this byte[] buffer)
    {
        if (buffer.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        return buffer[0] == 0x46 && buffer[1] == 0x4C && buffer[2] == 0x56 && buffer[3] == 0x01;
    }

    /// <summary>
    /// Determines if the given ReadOnlySpan is a FLV file based on its signature.
    /// </summary>
    /// <param name="buffer">The byte span to check.</param>
    /// <returns>True if the byte span represents a FLV file, false otherwise.</returns>
    /// <exception cref="ArgumentException">Thrown when the buffer length is less than 4.</exception>
    public static bool IsFLV(this ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        return buffer[0] == 0x46 && buffer[1] == 0x4C && buffer[2] == 0x56 && buffer[3] == 0x01;
    }

    /// <summary>
    /// Determines if the provided byte array is an Ogg (OGG) file based on its signature.
    /// </summary>
    /// <param name="buffer">The byte array containing the file data to verify.</param>
    /// <returns>Returns true if the buffer matches an OGG file signature, otherwise returns false.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided buffer is less than 4 bytes long.</exception>
    public static bool IsOGG(this byte[] buffer)
    {
        if (buffer.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        return buffer[0] == 0x4F && buffer[1] == 0x67 && buffer[2] == 0x67 && buffer[3] == 0x53;
    }

    /// <summary>
    /// Determines if the provided ReadOnlySpan buffer is an Ogg (OGG) file based on its signature.
    /// </summary>
    /// <param name="buffer">The ReadOnlySpan buffer containing the file data to verify.</param>
    /// <returns>Returns true if the buffer matches an OGG file signature, otherwise returns false.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided buffer is less than 4 bytes long.</exception>
    public static bool IsOGG(this ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        return buffer[0] == 0x4F && buffer[1] == 0x67 && buffer[2] == 0x67 && buffer[3] == 0x53;
    }

    /// <summary>
    /// Determines if the provided byte array is a Windows Media Video (WMV) file based on its signature.
    /// </summary>
    /// <param name="buffer">The byte array containing the file data to verify.</param>
    /// <returns>Returns true if the buffer matches a WMV file signature, otherwise returns false.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided buffer is less than 16 bytes long.</exception>
    public static bool IsWMV(this byte[] buffer)
    {
        if (buffer.Length < 16)
            throw new ArgumentException("The signature that you are verifying must be at least 16 bytes long");

        if (buffer[0] != 0x30
            || buffer[1] != 0x26
            || buffer[2] != 0xB2
            || buffer[3] != 0x75
            || buffer[4] != 0x8E
            || buffer[5] != 0x66
            || buffer[6] != 0xCF
            || buffer[7] != 0x11
            || buffer[8] != 0xA6
            || buffer[9] != 0xD9
            || buffer[10] != 0x00
            || buffer[11] != 0xAA
            || buffer[12] != 0x00
            || buffer[13] != 0x62
            || buffer[14] != 0xCE
            || buffer[15] != 0x6C)
            return false;

        if (buffer.Length < 1024)
            return true;

        var aspectRatio = Encoding.Unicode.GetBytes("AspectRatio");
        var windowsMediaVideo = Encoding.Unicode.GetBytes("WindowsMediaVideo");
        var wmv3 = "WMV3"u8.ToArray();
        var deviceConformanceTemplate = Encoding.Unicode.GetBytes("DeviceConformanceTemplate MP @ML");

        var headerBuff = buffer.AsSpan(0, 1024);

        if (IndexOf(headerBuff, aspectRatio) != -1)
            return true;

        if (IndexOf(headerBuff, windowsMediaVideo) != -1)
            return true;

        if (IndexOf(headerBuff, wmv3) != -1)
            return true;

        return IndexOf(headerBuff, deviceConformanceTemplate) != -1;
    }

    /// <summary>
    /// Determines if the provided ReadOnlySpan buffer is a Windows Media Video (WMV) file based on its signature.
    /// </summary>
    /// <param name="buffer">The ReadOnlySpan buffer containing the file data to verify.</param>
    /// <returns>Returns true if the buffer matches a WMV file signature, otherwise returns false.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided buffer is less than 16 bytes long.</exception>
    public static bool IsWMV(this ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length < 16)
            throw new ArgumentException("The signature that you are verifying must be at least 16 bytes long");

        if (buffer[0] != 0x30
            || buffer[1] != 0x26
            || buffer[2] != 0xB2
            || buffer[3] != 0x75
            || buffer[4] != 0x8E
            || buffer[5] != 0x66
            || buffer[6] != 0xCF
            || buffer[7] != 0x11
            || buffer[8] != 0xA6
            || buffer[9] != 0xD9
            || buffer[10] != 0x00
            || buffer[11] != 0xAA
            || buffer[12] != 0x00
            || buffer[13] != 0x62
            || buffer[14] != 0xCE
            || buffer[15] != 0x6C)
            return false;

        if (buffer.Length < 1024)
            return true;

        var aspectRatio = Encoding.Unicode.GetBytes("AspectRatio");
        var windowsMediaVideo = Encoding.Unicode.GetBytes("WindowsMediaVideo");
        var wmv3 = "WMV3"u8.ToArray();
        var deviceConformanceTemplate = Encoding.Unicode.GetBytes("DeviceConformanceTemplate MP @ML");

        var headerBuff = buffer[..1024];

        if (IndexOf(headerBuff, aspectRatio) != -1)
            return true;

        if (IndexOf(headerBuff, windowsMediaVideo) != -1)
            return true;

        if (IndexOf(headerBuff, wmv3) != -1)
            return true;

        return IndexOf(headerBuff, deviceConformanceTemplate) != -1;
    }

    /// <summary>
    /// Determines if the provided byte array is a Bitmap (BMP) file based on its signature.
    /// </summary>
    /// <param name="buffer">Byte array containing the file signature to verify.</param>
    /// <returns>Returns true if the buffer matches a BMP file signature, otherwise returns false.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided buffer is less than 2 bytes long.</exception>
    public static bool IsBMP(this byte[] buffer)
    {
        if (buffer.Length < 2)
            throw new ArgumentException("The signature that you are verifying must be at least 2 bytes long");

        return buffer[0] == 0x42 && buffer[1] == 0x4D;
    }

    /// <summary> buffer is a Bitmap (BMP) file based on its signature.
    /// </summary>
    /// <param name="buffer">The ReadOnlySpan buffer containing the file signature to verify.</param>
    /// <returns>Returns true if the buffer matches a BMP file signature, otherwise returns false.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided buffer is less than 2 bytes long.</exception>
    public static bool IsBMP(this ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length < 2)
            throw new ArgumentException("The signature that you are verifying must be at least 2 bytes long");

        return buffer[0] == 0x42 && buffer[1] == 0x4D;
    }

    /// <summary>
    /// Finds the index of a pattern within a source span.
    /// </summary>
    /// <param name="sourceSpan">The source span to search within.</param>
    /// <param name="patternSpan">The pattern span to find.</param>
    /// <returns>Returns the starting index of the pattern within the source if found; otherwise, returns -1.</returns>
    private static int IndexOf(this ReadOnlySpan<byte> sourceSpan, ReadOnlySpan<byte> patternSpan)
    {
        for (var i = 0; i <= sourceSpan.Length - patternSpan.Length; i++)
        {
            var match = true;

            for (var j = 0; j < patternSpan.Length; j++)
            {
                if (sourceSpan[i + j] != patternSpan[j])
                {
                    match = false;

                    break;
                }
            }

            if (match)
                return i;
        }

        return -1;
    }

    /// <summary>
    /// Determines if the provided byte array is an Advanced Systems Format (ASF) file based on its signature.
    /// </summary>
    /// <param name="buffer">Byte array containing the file signature to verify.</param>
    /// <returns>Returns true if the buffer matches an ASF file signature, otherwise returns false.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided buffer is less than 16 bytes long.</exception>
    public static bool IsASF(this byte[] buffer)
    {
        if (buffer.Length < 16)
            throw new ArgumentException("The signature that you are verifying must be at least 16 bytes long");

        return buffer[0] == 0x30
               && buffer[1] == 0x26
               && buffer[2] == 0xB2
               && buffer[3] == 0x75
               && buffer[4] == 0x8E
               && buffer[5] == 0x66
               && buffer[6] == 0xCF
               && buffer[7] == 0x11
               && buffer[8] == 0xA6
               && buffer[9] == 0xD9
               && buffer[10] == 0x00
               && buffer[11] == 0xAA
               && buffer[12] == 0x00
               && buffer[13] == 0x62
               && buffer[14] == 0xCE
               && buffer[15] == 0x6C;
    }

    /// <summary>
    /// Determines if the provided ReadOnlySpan<byte> buffer is an Advanced Systems Format (ASF) file based on its signature.
    /// </summary>
    /// <param name="buffer">The ReadOnlySpan<byte> buffer containing the file signature to verify.</param>
    /// <returns>Returns true if the buffer matches an ASF file signature, otherwise returns false.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided buffer is less than 16 bytes long.</exception>
    public static bool IsASF(this ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length < 16)
            throw new ArgumentException("The signature that you are verifying must be at least 16 bytes long");

        return buffer[0] == 0x30
               && buffer[1] == 0x26
               && buffer[2] == 0xB2
               && buffer[3] == 0x75
               && buffer[4] == 0x8E
               && buffer[5] == 0x66
               && buffer[6] == 0xCF
               && buffer[7] == 0x11
               && buffer[8] == 0xA6
               && buffer[9] == 0xD9
               && buffer[10] == 0x00
               && buffer[11] == 0xAA
               && buffer[12] == 0x00
               && buffer[13] == 0x62
               && buffer[14] == 0xCE
               && buffer[15] == 0x6C;
    }

    /// <summary>
    /// Determines if the provided byte array is an MP3 audio file based on its signature.
    /// </summary>
    /// <param name="buffer">Byte array containing the file signature to verify.</param>
    /// <returns>Returns true if the buffer matches an MP3 file signature, otherwise returns false.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided buffer is less than 3 bytes long.</exception>
    public static bool IsMP3(this byte[] buffer)
    {
        if (buffer.Length < 3)
            throw new ArgumentException("The signature that you are verifying must be at least 3 bytes long");

        return buffer[0] == 0x49 && buffer[1] == 0x44 && buffer[2] == 0x33;
    }

    /// <summary>
    /// Determines if the provided ReadOnlySpan<byte> buffer is an MP3 audio file based on its signature.
    /// </summary>
    /// <param name="buffer">The ReadOnlySpan<byte> buffer containing the file signature to verify.</param>
    /// <returns>Returns true if the buffer matches an MP3 file signature, otherwise returns false.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided buffer is less than 3 bytes long.</exception>
    public static bool IsMP3(this ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length < 3)
            throw new ArgumentException("The signature that you are verifying must be at least 3 bytes long");

        return buffer[0] == 0x49 && buffer[1] == 0x44 && buffer[2] == 0x33;
    }

    /// <summary>
    /// Determines if the provided byte array is a RealMedia (RM) file based on its signature.
    /// </summary>
    /// <param name="buffer">Byte array containing the file signature to verify.</param>
    /// <returns>Returns true if the buffer matches a RealMedia file signature, otherwise returns false.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided buffer is less than 4 bytes long.</exception>
    public static bool IsRM(this byte[] buffer)
    {
        if (buffer.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        return buffer[0] == 0x2E && buffer[1] == 0x52 && buffer[2] == 0x4D && buffer[3] == 0x46;
    }

    /// <summary>
    /// Determines if the provided ReadOnlySpan<byte> buffer is a RealMedia (RM) file based on its signature.
    /// </summary>
    /// <param name="buffer">The ReadOnlySpan<byte> buffer containing the file signature to verify.</param>
    /// <returns>Returns true if the buffer matches a RealMedia file signature, otherwise returns false.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided buffer is less than 4 bytes long.</exception>
    public static bool IsRM(this ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        return buffer[0] == 0x2E && buffer[1] == 0x52 && buffer[2] == 0x4D && buffer[3] == 0x46;
    }

    /// <summary>
    /// Determines if the provided byte array is a Photoshop (PSD) file based on its signature.
    /// </summary>
    /// <param name="buffer">Byte array containing the file signature to verify.</param>
    /// <returns>Returns true if the buffer matches a Photoshop file signature, otherwise returns false.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided buffer is less than 4 bytes long.</exception>
    public static bool IsPSD(this byte[] buffer)
    {
        if (buffer.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        return buffer[0] == 0x38 && buffer[1] == 0x42 && buffer[2] == 0x50 && buffer[3] == 0x53;
    }

    /// <summary>
    /// Determines if the provided ReadOnlySpan buffer is a Photoshop (PSD) file based on its signature.
    /// </summary>
    /// <param name="buffer">The ReadOnlySpan buffer containing the file signature to verify.</param>
    /// <returns>Returns true if the buffer matches a Photoshop file signature, otherwise returns false.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided buffer is less than 4 bytes long.</exception>
    public static bool IsPSD(this ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        return buffer[0] == 0x38 && buffer[1] == 0x42 && buffer[2] == 0x50 && buffer[3] == 0x53;
    }

    /// <summary>
    /// Determines if the provided byte array is a WebP image file based on its signature.
    /// </summary>
    /// <param name="buffer">Byte array containing the file signature to verify.</param>
    /// <returns>Returns true if the buffer matches a WebP image file signature, otherwise returns false.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided buffer is less than 12 bytes long.</exception>
    public static bool IsWEBP(this byte[] buffer)
    {
        if (buffer.Length < 12)
            throw new ArgumentException("The signature that you are verifying must be at least 12 bytes long");

        return (buffer[0] == 0x52
                && buffer[1] == 0x49
                && buffer[2] == 0x46
                && buffer[3] == 0x46
                && buffer[8] == 0x57
                && buffer[9] == 0x45
                && buffer[10] == 0x42
                && buffer[11] == 0x50)
               || (buffer[0] == 0x52
                   && buffer[1] == 0x49
                   && buffer[2] == 0x46
                   && buffer[3] == 0x46
                   && buffer[8] == 0x56
                   && buffer[9] == 0x50
                   && buffer[10] == 0x38
                   && buffer[11] == 0x58);
    }

    /// <summary>
    /// Determines if the provided ReadOnlySpan buffer is a WebP image file based on its signature.
    /// </summary>
    /// <param name="buffer">The ReadOnlySpan buffer containing the file signature to verify.</param>
    /// <returns>Returns true if the buffer matches a WebP image file signature, otherwise returns false.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided buffer is less than 12 bytes long.</exception>
    public static bool IsWEBP(this ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length < 12)
            throw new ArgumentException("The signature that you are verifying must be at least 12 bytes long");

        return (buffer[0] == 0x52
                && buffer[1] == 0x49
                && buffer[2] == 0x46
                && buffer[3] == 0x46
                && buffer[8] == 0x57
                && buffer[9] == 0x45
                && buffer[10] == 0x42
                && buffer[11] == 0x50)
               || (buffer[0] == 0x52
                   && buffer[1] == 0x49
                   && buffer[2] == 0x46
                   && buffer[3] == 0x46
                   && buffer[8] == 0x56
                   && buffer[9] == 0x50
                   && buffer[10] == 0x38
                   && buffer[11] == 0x58);
    }

    /// <summary>
    /// Determines if the provided byte array is a Waveform Audio File Format (WAV) file based on its signature.
    /// </summary>
    /// <param name="buffer">Byte array containing the file signature to verify.</param>
    /// <returns>Returns true if the buffer matches a WAV file signature, otherwise returns false.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided buffer is less than 12 bytes long.</exception>
    public static bool IsWAV(this byte[] buffer)
    {
        if (buffer.Length < 12)
            throw new ArgumentException("The signature that you are verifying must be at least 12 bytes long");

        return (buffer[0] == 0x52
                && buffer[1] == 0x49
                && buffer[2] == 0x46
                && buffer[3] == 0x46
                && buffer[8] == 0x57
                && buffer[9] == 0x41
                && buffer[10] == 0x56
                && buffer[11] == 0x45)
               || (buffer[0] == 0x52
                   && buffer[1] == 0x49
                   && buffer[2] == 0x46
                   && buffer[3] == 0x46
                   && buffer[8] == 0x41
                   && buffer[9] == 0x56
                   && buffer[10] == 0x49
                   && buffer[11] == 0x20)
               || (buffer[0] == 0x52
                   && buffer[1] == 0x49
                   && buffer[2] == 0x46
                   && buffer[3] == 0x46
                   && buffer[8] == 0x66
                   && buffer[9] == 0x6D
                   && buffer[10] == 0x74
                   && buffer[11] == 0x20)
               || (buffer[0] == 0x52
                   && buffer[1] == 0x49
                   && buffer[2] == 0x46
                   && buffer[3] == 0x46
                   && buffer[8] == 0x4D
                   && buffer[9] == 0x41
                   && buffer[10] == 0x43
                   && buffer[11] == 0x20);
    }

    /// <summary>
    /// Determines if the provided ReadOnlySpan buffer is a Waveform Audio File Format (WAV) file based on its signature.
    /// </summary>
    /// <param name="buffer">The ReadOnlySpan buffer containing the file signature to verify.</param>
    /// <returns>Returns true if the buffer matches a WAV file signature, otherwise returns false.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided buffer is less than 12 bytes long.</exception>
    public static bool IsWAV(this ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length < 12)
            throw new ArgumentException("The signature that you are verifying must be at least 12 bytes long");

        return (buffer[0] == 0x52
                && buffer[1] == 0x49
                && buffer[2] == 0x46
                && buffer[3] == 0x46
                && buffer[8] == 0x57
                && buffer[9] == 0x41
                && buffer[10] == 0x56
                && buffer[11] == 0x45)
               || (buffer[0] == 0x52
                   && buffer[1] == 0x49
                   && buffer[2] == 0x46
                   && buffer[3] == 0x46
                   && buffer[8] == 0x41
                   && buffer[9] == 0x56
                   && buffer[10] == 0x49
                   && buffer[11] == 0x20)
               || (buffer[0] == 0x52
                   && buffer[1] == 0x49
                   && buffer[2] == 0x46
                   && buffer[3] == 0x46
                   && buffer[8] == 0x66
                   && buffer[9] == 0x6D
                   && buffer[10] == 0x74
                   && buffer[11] == 0x20)
               || (buffer[0] == 0x52
                   && buffer[1] == 0x49
                   && buffer[2] == 0x46
                   && buffer[3] == 0x46
                   && buffer[8] == 0x4D
                   && buffer[9] == 0x41
                   && buffer[10] == 0x43
                   && buffer[11] == 0x20);
    }

    /// <summary>
    /// Determines if the provided byte array is a Free Lossless Audio Codec (FLAC) file based on its signature.
    /// </summary>
    /// <param name="buffer">Byte array containing the file signature to verify.</param>
    /// <returns>Returns true if the buffer matches a FLAC file signature, otherwise returns false.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided buffer is less than 4 bytes long.</exception>
    public static bool IsFLAC(this byte[] buffer)
    {
        if (buffer.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        return (buffer[0] == 0x66 && buffer[1] == 0x4C && buffer[2] == 0x61 && buffer[3] == 0x43)
               || (buffer[0] == 0x66 && buffer[1] == 0x4C && buffer[2] == 0x61 && buffer[3] == 0x58)
               || (buffer[0] == 0x66 && buffer[1] == 0x72 && buffer[2] == 0x65 && buffer[3] == 0x65)
               || (buffer[0] == 0x66 && buffer[1] == 0x4C && buffer[2] == 0x61 && buffer[3] == 0x54);
    }

    /// <summary>
    /// Determines if the provided ReadOnlySpan<byte> buffer is a Free Lossless Audio Codec (FLAC) file based on its signature.
    /// </summary>
    /// <param name="buffer">The ReadOnlySpan<byte> buffer containing the file signature to verify.</param>
    /// <returns>Returns true if the buffer matches a FLAC file signature, otherwise returns false.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided buffer is less than 4 bytes long.</exception>
    public static bool IsFLAC(this ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        return (buffer[0] == 0x66 && buffer[1] == 0x4C && buffer[2] == 0x61 && buffer[3] == 0x43)
               || (buffer[0] == 0x66 && buffer[1] == 0x4C && buffer[2] == 0x61 && buffer[3] == 0x58)
               || (buffer[0] == 0x66 && buffer[1] == 0x72 && buffer[2] == 0x65 && buffer[3] == 0x65)
               || (buffer[0] == 0x66 && buffer[1] == 0x4C && buffer[2] == 0x61 && buffer[3] == 0x54);
    }

    /// <summary>
    /// Determines if the provided byte array is an Advanced Audio Codec (AAC) file based on its signature.
    /// </summary>
    /// <param name="buffer">Byte array containing the file signature to verify.</param>
    /// <returns>Returns true if the buffer matches an AAC file signature, otherwise returns false.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided buffer is less than 2 bytes long.</exception>
    public static bool IsAAC(this byte[] buffer)
    {
        if (buffer.Length < 2)
            throw new ArgumentException("The signature that you are verifying must be at least 2 bytes long");

        return (buffer[0] == 0xFF && buffer[1] == 0xF1)
               || (buffer[0] == 0xFF && buffer[1] == 0xF9)
               || (buffer[0] == 0xFF && buffer[1] == 0xFA)
               || (buffer[0] == 0xFF && buffer[1] == 0xFB);
    }

    /// <summary>
    /// Determines if the provided ReadOnlySpan<byte> buffer is an Advanced Audio Codec (AAC) file based on its signature.
    /// </summary>
    /// <param name="buffer">The ReadOnlySpan<byte> buffer containing the file signature to verify.</param>
    /// <returns>Returns true if the buffer matches an AAC file signature, otherwise returns false.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided buffer is less than 2 bytes long.</exception>
    public static bool IsAAC(this ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length < 2)
            throw new ArgumentException("The signature that you are verifying must be at least 2 bytes long");

        return (buffer[0] == 0xFF && buffer[1] == 0xF1)
               || (buffer[0] == 0xFF && buffer[1] == 0xF9)
               || (buffer[0] == 0xFF && buffer[1] == 0xFA)
               || (buffer[0] == 0xFF && buffer[1] == 0xFB);
    }

    /// <summary>
    /// Determines if the provided byte array is a Windows Media Audio (WMA) file based on its signature.
    /// </summary>
    /// <param name="buffer">Byte array containing the file signature to verify.</param>
    /// <returns>Returns true if the buffer matches a WMA file signature, otherwise returns false.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided buffer is less than 16 bytes long.</exception>
    public static bool IsWMA(this byte[] buffer)
    {
        if (buffer.Length < 16)
            throw new ArgumentException("The signature that you are verifying must be at least 16 bytes long");

        if (!((buffer[0] == 0x30
               && buffer[1] == 0x26
               && buffer[2] == 0xB2
               && buffer[3] == 0x75
               && buffer[4] == 0x8E
               && buffer[5] == 0x66
               && buffer[6] == 0xCF
               && buffer[7] == 0x11
               && buffer[8] == 0xA6
               && buffer[9] == 0xD9
               && buffer[10] == 0x00
               && buffer[11] == 0xAA
               && buffer[12] == 0x00
               && buffer[13] == 0x62
               && buffer[14] == 0xCE
               && buffer[15] == 0x6C)
              || (buffer[0] == 0x02 && buffer[1] == 0x00 && buffer[2] == 0x00 && buffer[3] == 0x00)
              || (buffer[0] == 0xFE && buffer[1] == 0xFF)))
            return false;

        return IsWmaFormat(buffer);
    }

    /// <summary>
    /// Verifies if the given byte array represents a Windows Media Audio (WMA) file.
    /// </summary>
    /// <param name="fileData">Byte array containing the file data to be verified.</param>
    /// <returns>Returns true if the byte array represents a WMA file, otherwise returns false.</returns>
    private static bool IsWmaFormat(byte[] fileData)
    {
        // Create a MemoryStream from the file data
        using var ms = new MemoryStream(fileData);

        // Read the first 16 bytes to verify the ASF file signature
        var guidBuffer = new byte[16];
        ms.Read(guidBuffer, 0, 16);

        // Convert the 16 bytes to a GUID
        var fileGuid = new Guid(guidBuffer);

        // ASF file signature GUID
        var asfGuid = new Guid("75B22630-668E-11CF-A6D9-00AA0062CE6C");

        // Check if the file has a valid ASF signature
        if (fileGuid != asfGuid)
            return false;

        // Codec bytes for WMA
        var wmaCodecBytes = "161"u8.ToArray();

        // Read blocks of data to find an audio stream with a WMA codec
        var buffer = new byte[4096];
        int bytesRead;

        while ((bytesRead = ms.Read(buffer, 0, buffer.Length)) > 0)
        {
            // Check each block for WMA codec bytes
            for (var i = 0; i < bytesRead - wmaCodecBytes.Length; i++)
            {
                var match = true;

                for (var j = 0; j < wmaCodecBytes.Length; j++)
                {
                    if (buffer[i + j] != wmaCodecBytes[j])
                    {
                        match = false;

                        break;
                    }
                }

                if (match)
                    return true;
            }
        }

        // If no WMA codec bytes were found
        return false;
    }

    /// <summary>
    /// Determines if the provided buffer is a Windows Media Audio (WMA) file based on its signature.
    /// </summary>
    /// <param name="buffer">The ReadOnlySpan buffer containing the file signature to verify.</param>
    /// <returns>Returns true if the buffer matches a WMA file signature, otherwise returns false.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided buffer is less than 16 bytes long.</exception>
    public static bool IsWMA(this ReadOnlySpan<byte> buffer)
    {
        // Check if the buffer is at least 16 bytes long
        if (buffer.Length < 16)
            throw new ArgumentException("The signature that you are verifying must be at least 16 bytes long");

        // Check for WMA file signature
        if (!((buffer[0] == 0x30
               && buffer[1] == 0x26
               && buffer[2] == 0xB2
               && buffer[3] == 0x75

               // ... (omitted for brevity)
               && buffer[14] == 0xCE
               && buffer[15] == 0x6C)
              || (buffer[0] == 0x02 && buffer[1] == 0x00 && buffer[2] == 0x00 && buffer[3] == 0x00)
              || (buffer[0] == 0xFE && buffer[1] == 0xFF)))
            return false;

        // Additional verification for WMA format
        return IsWmaFormat(buffer.ToArray());
    }
}