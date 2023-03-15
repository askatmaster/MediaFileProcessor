using System.Text;
using MediaFileProcessor.Models.Enums;
namespace MediaFileProcessor.Models.Common;

public static class FilesSignatures
{
    private static List<byte[]> JPG => new ()
    {
        new byte[] { 0xFF, 0xD8, 0xFF, 0xE0, 0, 0, 0x4A, 0x46, 0x49, 0x46, 0x00 },
        new byte[] { 0xFF, 0xD8, 0xFF, 0xE1, 0, 0, 0x45, 0x78, 0x69, 0x66, 0x00 },
        new byte[] { 0xFF, 0xD8, 0xFF, 0xFE, 0, 0, 0x4C, 0x61, 0x76, 0x63, 0x35 },
        new byte[] { 0xFF, 0xD8, 0xFF, 0xDB, 0, 0, 0, 0, 0, 0, 0 }
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

    private static  List<byte[]> MP4 => new ()
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
        new byte[] { 0x52, 0x49, 0x46, 0x46, 0, 0, 0, 0, 0x41, 0x56, 0x49, 0x20, 0x4C, 0x49, 0x53, 0x54 },
        new byte[] { 0x52, 0x49, 0x46, 0x46, 0, 0, 0, 0, 0x41, 0x56, 0x49, 0x2D, 0x6D, 0x6D, 0x76, 0x69 },
        new byte[] { 0x52, 0x49, 0x46, 0x46, 0, 0, 0, 0, 0x41, 0x56, 0x49, 0x38, 0x69, 0x64, 0x78, 0x31 }
    };

    private static List<byte[]> MPEG => new ()
    {
        new byte[] { 0x00, 0x00, 0x01, 0xBA },
        new byte[] { 0x00, 0x00, 0x01, 0xB3 },
        new byte[] { 0x00, 0x00, 0x00, 0x01 }
    };

    private static List<byte[]> GIF => new ()
    {
        new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 },
        new byte[] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 }
    };

    private static List<byte[]> VOB => new ()
    {
        new byte[] { 0x00, 0x00, 0x01, 0xBA, 0x44, 0x00 }
    };

    private static List<byte[]> M2TS => new ()
    {
        new byte[] { 0x47, 0x40, 0x00, 0x10 }
    };

    private static List<byte[]> MXF => new ()
    {
        new byte[] { 0x06, 0x0E, 0x2B, 0x34, 0x02, 0x05, 0x01, 0x01, 0x0D, 0x01, 0x02, 0x01, 0x01, 0x02 }
    };

    //TODO distinguish between mkv
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

    //TODO distinguish between WMA
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
        new byte[] { 0x52, 0x49, 0x46, 0x46, 0, 0, 0, 0, 0x57, 0x45, 0x42, 0x50 },
        new byte[] { 0x52, 0x49, 0x46, 0x46, 0, 0, 0, 0, 0x56, 0x50, 0x38, 0x58 }
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
        new byte[] { 0x02, 0x00, 0x00, 0x00, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new byte[] { 0xFE, 0xFF, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
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

    public static FileFormatType GetFormat(this byte[] signature)
    {
        return signature.Length switch
        {
            >= 3 when IsJPEG(signature) => FileFormatType.JPG,
            >= 8 when IsPNG(signature) => FileFormatType.PNG,
            >= 6 when IsICO(signature) => FileFormatType.ICO,
            >= 4 when IsTIFF(signature) => FileFormatType.TIFF,
            >= 11 when Is3GP(signature) => FileFormatType._3GP,
            >= 8 when IsMP4(signature) => FileFormatType.MP4,
            >= 16 when IsMOV(signature) => FileFormatType.MOV,
            >= 4 when IsMKV(signature) => FileFormatType.MKV,
            >= 6 when IsVOB(signature) => FileFormatType.VOB,
            >= 4 when IsMPEG(signature) => FileFormatType.MPEG,
            >= 16 when IsAVI(signature) => FileFormatType.AVI,
            >= 6 when IsGIF(signature) => FileFormatType.GIF,
            >= 4 when IsM2TS(signature) => FileFormatType.M2TS,
            >= 14 when IsMXF(signature) => FileFormatType.MXF,
            >= 4 when IsWEBM(signature) => FileFormatType.WEBM,
            >= 4 when IsGXF(signature) => FileFormatType.GXF,
            >= 4 when IsFLV(signature) => FileFormatType.FLV,
            >= 4 when IsOGG(signature) => FileFormatType.OGG,
            >= 16 when IsWMV(signature) => FileFormatType.WMV,
            >= 2 when IsBMP(signature) => FileFormatType.BMP,
            >= 16 when IsASF(signature) => FileFormatType.ASF,
            >= 3 when IsMP3(signature) => FileFormatType.MP3,
            >= 4 when IsRM(signature) => FileFormatType.RM,
            >= 4 when IsPSD(signature) => FileFormatType.PSD,
            >= 12 when IsWEBP(signature) => FileFormatType.WEBP,
            >= 12 when IsWAV(signature) => FileFormatType.WAV,
            >= 4 when IsFLAC(signature) => FileFormatType.FLAC,
            >= 2 when IsAAC(signature) => FileFormatType.AAC,
            >= 16 when IsWMA(signature) => FileFormatType.WMA,
            _ => throw new Exception("Unable to determine the format")
        };
    }

    public static bool IsJPEG(this byte[] buffer)
    {
        if(buffer.Length < 3)
            throw new ArgumentException("The signature that you are verifying must be at least 3 bytes long");

        return buffer[0] == 0xFF && buffer[1] == 0xD8 && buffer[2] == 0xFF;
    }

    public static bool IsPNG(this byte[] buffer)
    {
        if(buffer.Length < 8)
            throw new ArgumentException("The signature that you are verifying must be at least 8 bytes long");

        return buffer[0] == 0x89
         && buffer[1] == 0x50
         && buffer[2] == 0x4e
         && buffer[3] == 0x47
         && buffer[4] == 0x0D
         && buffer[5] == 0x0A
         && buffer[6] == 0x1A
         && buffer[7] == 0x0A;
    }

    public static bool IsICO(this byte[] buffer)
    {
        if(buffer.Length < 6)
            throw new ArgumentException("The signature that you are verifying must be at least 6 bytes long");

        return buffer[0] == 0x00 && buffer[1] == 0x00 && buffer[2] == 0x01 && buffer[3] == 0x00 && buffer[4] == 0x01 && buffer[5] == 0x00;
    }

    public static bool IsTIFF(this byte[] buffer)
    {
        if(buffer.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        return (buffer[0] == 0x4D && buffer[1] == 0x4D && buffer[2] == 0x00 && buffer[3] == 0x2A)
         || (buffer[0] == 0x49 && buffer[1] == 0x49 && buffer[2] == 0x2A && buffer[3] == 0x00);
    }

    public static bool Is3GP(this byte[] buffer)
    {
        if(buffer.Length < 11)
            throw new ArgumentException("The signature that you are verifying must be at least 11 bytes long");

        return buffer[4] == 0x66 && buffer[5] == 0x74 && buffer[6] == 0x79 && buffer[7] == 0x70 && buffer[8] == 0x33 && buffer[9] == 0x67 && buffer[10] == 0x70;
    }

    public static bool IsMP4(this byte[] buffer)
    {
        if(buffer.Length < 8)
            throw new ArgumentException("The signature that you are verifying must be at least 8 bytes long");

        return buffer[4] == 0x66 && buffer[5] == 0x74 && buffer[6] == 0x79 && buffer[7] == 0x70;
    }

    public static bool IsMOV(this byte[] buffer)
    {
        if(buffer.Length < 16)
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

    public static bool IsMKV(this byte[] buffer)
    {
        if(buffer.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        return buffer[0] == 0x1A && buffer[1] == 0x45 && buffer[2] == 0xDF && buffer[3] == 0xA3;
    }

    public static bool IsAVI(this byte[] buffer)
    {
        if(buffer.Length < 16)
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

    public static bool IsMPEG(this byte[] buffer)
    {
        if(buffer.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        return (buffer[0] == 0x00 && buffer[1] == 0x00 && buffer[2] == 0x01 && buffer[3] == 0xBA)
         || (buffer[0] == 0x00 && buffer[1] == 0x00 && buffer[2] == 0x01 && buffer[3] == 0xB3)
         || (buffer[0] == 0x00 && buffer[1] == 0x00 && buffer[2] == 0x00 && buffer[3] == 0x01);
    }

    public static bool IsGIF(this byte[] buffer)
    {
        if(buffer.Length < 6)
            throw new ArgumentException("The signature that you are verifying must be at least 6 bytes long");

        return (buffer[0] == 0x47 && buffer[1] == 0x49 && buffer[2] == 0x46 && buffer[3] == 0x38 && buffer[4] == 0x39 && buffer[5] == 0x61)
         || (buffer[0] == 0x47 && buffer[1] == 0x49 && buffer[2] == 0x46 && buffer[3] == 0x38 && buffer[4] == 0x37 && buffer[5] == 0x61);
    }

    public static bool IsVOB(this byte[] buffer)
    {
        if(buffer.Length < 6)
            throw new ArgumentException("The signature that you are verifying must be at least 6 bytes long");

        return buffer[0] == 0x00 && buffer[1] == 0x00 && buffer[2] == 0x01 && buffer[3] == 0xBA && buffer[4] == 0x44 && buffer[5] == 0x00;
    }

    public static bool IsM2TS(this byte[] buffer)
    {
        if(buffer.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        return buffer[0] == 0x47 && buffer[1] == 0x40 && buffer[2] == 0x00 && buffer[3] == 0x10;
    }

    public static bool IsMXF(this byte[] buffer)
    {
        if(buffer.Length < 14)
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

    public static bool IsWEBM(this byte[] buffer)
    {
        if(buffer.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        return buffer[0] == 0x1A && buffer[1] == 0x45 && buffer[2] == 0xDF && buffer[3] == 0xA3;
    }

    public static bool IsGXF(this byte[] buffer)
    {
        if(buffer.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        return buffer[0] == 0x47 && buffer[1] == 0x58 && buffer[2] == 0x46 && buffer[3] == 0x30;
    }

    public static bool IsFLV(this byte[] buffer)
    {
        if(buffer.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        return buffer[0] == 0x46 && buffer[1] == 0x4C && buffer[2] == 0x56 && buffer[3] == 0x01;
    }

    public static bool IsOGG(this byte[] buffer)
    {
        if(buffer.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        return buffer[0] == 0x4F && buffer[1] == 0x67 && buffer[2] == 0x67 && buffer[3] == 0x53;
    }

    public static bool IsWMV(this byte[] buffer)
    {
        if(buffer.Length < 16)
            throw new ArgumentException("The signature that you are verifying must be at least 16 bytes long");

        if(buffer[0] != 0x30
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

        if(buffer.Length < 1024)
            return true;

        var aspectRatio = Encoding.Unicode.GetBytes("AspectRatio");
        var windowsMediaVideo = Encoding.Unicode.GetBytes("WindowsMediaVideo");
        var wmv3 = Encoding.ASCII.GetBytes("WMV3");
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

    public static bool IsBMP(this byte[] buffer)
    {
        if(buffer.Length < 2)
            throw new ArgumentException("The signature that you are verifying must be at least 2 bytes long");

        return buffer[0] == 0x42 && buffer[1] == 0x4D;
    }

    private static int IndexOf(this byte[] source, byte[] pattern)
    {
        for (var i = 0; i <= source.Length - pattern.Length; i++)
        {
            var match = !pattern.Where((t, j) => source[i + j] != t).Any();

            if (match)
                return i;
        }

        return -1;
    }

    public static bool IsASF(this byte[] buffer)
    {
        if(buffer.Length < 16)
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

    public static bool IsMP3(this byte[] buffer)
    {
        if(buffer.Length < 3)
            throw new ArgumentException("The signature that you are verifying must be at least 3 bytes long");

        return buffer[0] == 0x49 && buffer[1] == 0x44 && buffer[2] == 0x33;
    }

    public static bool IsRM(this byte[] buffer)
    {
        if(buffer.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        return buffer[0] == 0x2E && buffer[1] == 0x52 && buffer[2] == 0x4D && buffer[3] == 0x46;
    }

    public static bool IsPSD(this byte[] buffer)
    {
        if(buffer.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        return buffer[0] == 0x38 && buffer[1] == 0x42 && buffer[2] == 0x50 && buffer[3] == 0x53;
    }

    public static bool IsWEBP(this byte[] buffer)
    {
        if(buffer.Length < 12)
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

    public static bool IsWAV(this byte[] buffer)
    {
        if(buffer.Length < 12)
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

    public static bool IsFLAC(this byte[] buffer)
    {
        if(buffer.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        return (buffer[0] == 0x66 && buffer[1] == 0x4C && buffer[2] == 0x61 && buffer[3] == 0x43)
         || (buffer[0] == 0x66 && buffer[1] == 0x4C && buffer[2] == 0x61 && buffer[3] == 0x58)
         || (buffer[0] == 0x66 && buffer[1] == 0x72 && buffer[2] == 0x65 && buffer[3] == 0x65)
         || (buffer[0] == 0x66 && buffer[1] == 0x4C && buffer[2] == 0x61 && buffer[3] == 0x54);
    }

    public static bool IsAAC(this byte[] buffer)
    {
        if(buffer.Length < 2)
            throw new ArgumentException("The signature that you are verifying must be at least 2 bytes long");

        return (buffer[0] == 0xFF && buffer[1] == 0xF1)
         || (buffer[0] == 0xFF && buffer[1] == 0xF9)
         || (buffer[0] == 0xFF && buffer[1] == 0xFA)
         || (buffer[0] == 0xFF && buffer[1] == 0xFB);
    }

    public static bool IsWMA(this byte[] buffer)
    {
        if(buffer.Length < 16)
            throw new ArgumentException("The signature that you are verifying must be at least 16 bytes long");

        return (buffer[0] == 0x30
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
         || (buffer[0] == 0xFE && buffer[1] == 0xFF);
    }
}