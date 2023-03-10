using System.Text;
using MediaFileProcessor.Models.Enums;
namespace MediaFileProcessor.Models.Common;

public static class FilesSignatures
{
    private static Dictionary<int[]?, byte[]> JPG => new ()
    {
        { new [] { 4, 5 } , new byte[] { 0xFF, 0xD8, 0xFF, 0xE0, 0, 0, 0x4A, 0x46, 0x49, 0x46, 0x00 } },
        { new [] { 4, 5 } , new byte[] { 0xFF, 0xD8, 0xFF, 0xE1, 0, 0, 0x45, 0x78, 0x69, 0x66, 0x00 } },
        { new [] { 4, 5 } , new byte[] { 0xFF, 0xD8, 0xFF, 0xFE, 0, 0, 0x4C, 0x61, 0x76, 0x63, 0x35 } },
        { new [] { 4, 5 } , new byte[] { 0xFF, 0xD8, 0xFF, 0xDB, 0, 0, 0, 0, 0, 0, 0 } }
    };

    private static Dictionary<int[]?, byte[]> PNG => new ()
    {
        { null , new byte[] { 0x89, 0x50, 0x4e, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } }
    };

    private static Dictionary<int[]?, byte[]> ICO => new ()
    {
        { null , new byte[] { 0x00, 0x00, 0x01, 0x00, 0x01, 0x00 } }
    };

    private static Dictionary<int[]?, byte[]> TIFF => new()
    {
        { null , new byte[] { 0x4D, 0x4D, 0x00, 0x2A } },
        { null , new byte[] { 0x49, 0x49, 0x2A, 0x00 } }
    };

    private static Dictionary<int[]?, byte[]> _3GP => new ()
    {
        { new [] { 0, 1, 2, 3 } , new byte[] { 0, 0, 0, 0, 0x66, 0x74, 0x79, 0x70, 0x33, 0x67, 0x70 } }
    };

    private static Dictionary<int[]?, byte[]> MP4 => new ()
    {
        { new [] { 0, 1, 2, 3 } , new byte[] { 0, 0, 0, 0, 0x66, 0x74, 0x79, 0x70 } },
        { new [] { 0, 1, 2, 3 } , new byte[] { 0, 0, 0, 0, 0x66, 0x74, 0x79, 0x70 } },
        { new [] { 0, 1, 2, 3 } , new byte[] { 0, 0, 0, 0, 0x66, 0x74, 0x79, 0x70, 0x6D, 0x70, 0x34, 0x32 } },
        { new [] { 0, 1, 2, 3 } , new byte[] { 0, 0, 0, 0, 0x66, 0x74, 0x79, 0x70, 0x69, 0x73, 0x6F, 0x6D } },
        { new [] { 0, 1, 2, 3 } , new byte[] { 0, 0, 0, 0, 0x66, 0x74, 0x79, 0x70, 0x69, 0x73, 0x6F, 0x6D } },
        { new [] { 0, 1, 2, 3 } , new byte[] { 0, 0, 0, 0, 0x66, 0x74, 0x79, 0x70, 0x69, 0x73, 0x6F, 0x6D } }
    };

    private static Dictionary<int[]?, byte[]> MOV => new ()
    {
        { new [] { 0, 1, 2, 3 } , new byte[] { 0, 0, 0, 0, 0x66, 0x74, 0x79, 0x70, 0x71, 0x74, 0x20, 0x20, 0x00, 0x00, 0x02, 0x00 } }
    };

    private static Dictionary<int[]?, byte[]> MKV => new ()
    {
        { null , new byte[] { 0x1A, 0x45, 0xDF, 0xA3 } }
    };

    private static Dictionary<int[]?, byte[]> AVI => new ()
    {
        { new [] { 4, 5, 6, 7 } , new byte[] { 0x52, 0x49, 0x46, 0x46, 0, 0, 0, 0, 0x41, 0x56, 0x49, 0x20, 0x4C, 0x49, 0x53, 0x54 } },
        { new [] { 4, 5, 6, 7 } , new byte[] { 0x52, 0x49, 0x46, 0x46, 0, 0, 0, 0, 0x41, 0x56, 0x49, 0x2D, 0x6D, 0x6D, 0x76, 0x69 } },
        { new [] { 4, 5, 6, 7 } , new byte[] { 0x52, 0x49, 0x46, 0x46, 0, 0, 0, 0, 0x41, 0x56, 0x49, 0x38, 0x69, 0x64, 0x78, 0x31 } }
    };

    private static Dictionary<int[]?, byte[]> MPEG => new ()
    {
        { null , new byte[] { 0x00, 0x00, 0x01, 0xBA } },
        { null , new byte[] { 0x00, 0x00, 0x01, 0xB3 } },
        { null , new byte[] { 0x00, 0x00, 0x00, 0x01 } }
    };

    private static Dictionary<int[]?, byte[]> GIF => new ()
    {
        { null , new byte[] { 0x47, 0x49, 0x46, 0x38, 39, 61 } },
        { null , new byte[] { 0x47, 0x49, 0x46, 0x38, 37, 61 } }
    };

    private static Dictionary<int[]?, byte[]> VOB => new ()
    {
        { null , new byte[] { 0x00, 0x00, 0x01, 0xBA, 0x44, 0x00 } }
    };

    private static Dictionary<int[]?, byte[]> M2TS => new ()
    {
        { null , new byte[] { 0x47, 0x40, 0x00, 0x10 } }
    };

    private static Dictionary<int[]?, byte[]> MXF => new ()
    {
        { null , new byte[] { 0x06, 0x0E, 0x2B, 0x34, 0x02, 0x05, 0x01, 0x01, 0x0D, 0x01, 0x02, 0x01, 0x01, 0x02 } }
    };

    //TODO distinguish between mkv
    private static Dictionary<int[]?, byte[]> WEBM => new ()
    {
        { null , new byte[] { 0x1A, 0x45, 0xDF, 0xA3 } }
    };

    private static Dictionary<int[]?, byte[]> GXF => new ()
    {
        { null , new byte[] { 0x47, 0x58, 0x46, 0x30 } }
    };

    private static Dictionary<int[]?, byte[]> FLV => new ()
    {
        { null , new byte[] { 0x46, 0x4C, 0x56, 0x01 } }
    };

    private static Dictionary<int[]?, byte[]> OGG => new ()
    {
        { null , new byte[] { 0x4F, 0x67, 0x67, 0x53 } }
    };

    private static Dictionary<int[]?, byte[]> WMV => new ()
    {
        { null , new byte[] { 0x30, 0x26, 0xB2, 0x75, 0x8E, 0x66, 0xCF, 0x11, 0xA6, 0xD9, 0x00, 0xAA, 0x00, 0x62, 0xCE, 0x6C } }
    };

    private static Dictionary<int[]?, byte[]> BMP => new ()
    {
        { null , new byte[] { 0x42, 0x4D } }
    };

    //TODO distinguish between WMA
    private static Dictionary<int[]?, byte[]> ASF => new ()
    {
        { null , new byte[] { 0x30, 0x26, 0xB2, 0x75, 0x8E, 0x66, 0xCF, 0x11, 0xA6, 0xD9, 0x00, 0xAA, 0x00, 0x62, 0xCE, 0x6C } }
    };

    private static Dictionary<int[]?, byte[]> MP3 => new ()
    {
        { null , new byte[] { 0x49, 0x44, 0x33 } }
    };

    private static Dictionary<int[]?, byte[]> RM => new ()
    {
        { null , new byte[] { 0x2E, 0x52, 0x4D, 0x46 } }
    };

    private static Dictionary<int[]?, byte[]> PSD => new ()
    {
        { null , new byte[] { 0x38, 0x42, 0x50, 0x53 } }
    };

    private static Dictionary<int[]?, byte[]> WEBP => new ()
    {
        { new [] { 4, 5, 6, 7 } , new byte[] { 0x52, 0x49, 0x46, 0x46, 0, 0, 0, 0, 0x57, 0x45, 0x42, 0x50 } },
        { new [] { 4, 5, 6, 7 } , new byte[] { 0x52, 0x49, 0x46, 0x46, 0, 0, 0, 0, 0x56, 0x50, 0x38, 0x58 } }
    };

    private static Dictionary<int[]?, byte[]> WAV => new ()
    {
        { new [] { 4, 5, 6, 7 } , new byte[] { 0x52, 0x49, 0x46, 0x46, 0, 0, 0, 0, 0x57, 0x41, 0x56, 0x45 } },
        { new [] { 4, 5, 6, 7 } , new byte[] { 0x52, 0x49, 0x46, 0x46, 0, 0, 0, 0, 0x41, 0x56, 0x49, 0x20 } },
        { new [] { 4, 5, 6, 7 } , new byte[] { 0x52, 0x49, 0x46, 0x46, 0, 0, 0, 0, 0x66, 0x6D, 0x74, 0x20 } },
        { new [] { 4, 5, 6, 7 } , new byte[] { 0x52, 0x49, 0x46, 0x46, 0, 0, 0, 0, 0x4D, 0x41, 0x43, 0x20 } }
    };

    private static Dictionary<int[]?, byte[]> FLAC => new ()
    {
        { null , new byte[] { 0x66, 0x4C, 0x61, 0x43 } },
        { null , new byte[] { 0x66, 0x4C, 0x61, 0x58 } },
        { null , new byte[] { 0x66, 0x72, 0x65, 0x65 } },
        { null , new byte[] { 0x66, 0x4C, 0x61, 0x54 } }
    };

    private static Dictionary<int[]?, byte[]> AAC => new ()
    {
        { null , new byte[] { 0xFF, 0xF1 } },
        { null , new byte[] { 0xFF, 0xF9 } },
        { null , new byte[] { 0xFF, 0xFA } },
        { null , new byte[] { 0xFF, 0xFB } }
    };

    private static Dictionary<int[]?, byte[]> WMA => new ()
    {
        { null , new byte[] { 0x30, 0x26, 0xB2, 0x75, 0x8E, 0x66, 0xCF, 0x11, 0xA6, 0xD9, 0x00, 0xAA, 0x00, 0x62, 0xCE, 0x6C } },
        { new [] { 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 } , new byte[] { 0x02, 0x00, 0x00, 0x00, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } },
        { new [] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 } , new byte[] { 0xFE, 0xFF, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } }
    };

    /// <summary>
    /// Get file signature in bytes by its format
    /// </summary>
    /// <param name="outputFormatType">File format</param>
    /// <returns>Byte signature</returns>
    /// <exception cref="NotSupportedException">Exception if file format does not have a specific permanent signature or it is not supported</exception>
    public static Dictionary<int[]?, byte[]> GetSignature(this FileFormatType outputFormatType)
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
        if(signature.Length >= 3 && IsJPEG(signature[..3]))
            return FileFormatType.JPG;

        if(signature.Length >= 8 && IsPNG(signature[..8]))
            return FileFormatType.PNG;

        if(signature.Length >= 6 && IsICO(signature[..6]))
            return FileFormatType.ICO;

        if(signature.Length >= 4 && IsTIFF(signature[..4]))
            return FileFormatType.TIFF;

        if(signature.Length >= 11 && Is3GP(signature[..11]))
            return FileFormatType._3GP;

        if(signature.Length >= 8 && IsMP4(signature[..8]))
            return FileFormatType.MP4;

        if(signature.Length >= 16 && IsMOV(signature[..16]))
            return FileFormatType.MOV;

        if(signature.Length >= 4 && IsMKV(signature[..4]))
            return FileFormatType.MKV;

        if(signature.Length >= 6 && IsVOB(signature[..6]))
            return FileFormatType.VOB;

        if(signature.Length >= 4 && IsMPEG(signature[..4]))
            return FileFormatType.MPEG;

        throw new Exception("Unable to determine the format");
    }

    public static bool IsJPEG(byte[] bytes)
    {
        if(bytes.Length < 3)
            throw new ArgumentException("The signature that you are verifying must be at least 3 bytes long");

        for (var i = 0; i < bytes.Length; i++)
        {
            if(bytes[i] != JPG.Values.First()[i])
                return false;
        }

        return true;
    }

    public static bool IsPNG(byte[] bytes)
    {
        if(bytes.Length < 8)
            throw new ArgumentException("The signature that you are verifying must be at least 8 bytes long");

        for (var i = 0; i < bytes.Length; i++)
        {
            if(bytes[i] != PNG.Values.First()[i])
                return false;
        }

        return true;
    }

    public static bool IsICO(byte[] bytes)
    {
        if(bytes.Length < 6)
            throw new ArgumentException("The signature that you are verifying must be at least 6 bytes long");

        for (var i = 0; i < bytes.Length; i++)
        {
            if(bytes[i] != ICO.Values.First()[i])
                return false;
        }

        return true;
    }

    public static bool IsTIFF(byte[] bytes)
    {
        if(bytes.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        var signatures = TIFF.Values.ToArray();

        for (var i = 0; i < bytes.Length; i++)
        {
            if (bytes[i] == signatures[0][i])
                continue;

            for (var y = 0; y < bytes.Length; y++)
            {
                if(bytes[y] != signatures[1][y])
                    return false;
            }

            return true;
        }

        return true;
    }

    public static bool Is3GP(byte[] bytes)
    {
        if(bytes.Length < 11)
            throw new ArgumentException("The signature that you are verifying must be at least 11 bytes long");

        for (var i = 0; i < bytes.Length; i++)
        {
            if(i is 0 or 1 or 2 or 3)
                continue;

            if(bytes[i] != _3GP.Values.First()[i])
                return false;
        }

        return true;
    }

    public static bool IsMP4(byte[] bytes)
    {
        if(bytes.Length < 8)
            throw new ArgumentException("The signature that you are verifying must be at least 8 bytes long");

        for (var i = 0; i < bytes.Length; i++)
        {
            if(i is 0 or 1 or 2 or 3)
                continue;

            if(bytes[i] != MP4.Values.First()[i])
                return false;
        }

        return true;
    }

    public static bool IsMOV(byte[] bytes)
    {
        if(bytes.Length < 16)
            throw new ArgumentException("The signature that you are verifying must be at least 16 bytes long");

        for (var i = 0; i < bytes.Length; i++)
        {
            if(i is 0 or 1 or 2 or 3)
                continue;

            if(bytes[i] != MOV.Values.First()[i])
                return false;
        }

        return true;
    }

    public static bool IsMKV(byte[] bytes)
    {
        if(bytes.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        for (var i = 0; i < bytes.Length; i++)
        {
            if(bytes[i] != MKV.Values.First()[i])
                return false;
        }

        return true;
    }

    public static bool IsAVI(byte[] bytes)
    {
        if(bytes.Length < 16)
            throw new ArgumentException("The signature that you are verifying must be at least 16 bytes long");

        var signatures = AVI.Values.ToArray();

        for (var i = 0; i < bytes.Length; i++)
        {
            if(i is 4 or 5 or 6 or 7)
                continue;

            if (bytes[i] == signatures[0][i])
                continue;

            for (var y = 0; y < bytes.Length; y++)
            {
                if(y is 4 or 5 or 6 or 7)
                    continue;

                if(bytes[y] == signatures[1][y])
                    continue;

                for (var k = 0; k < bytes.Length; k++)
                {
                    if(k is 4 or 5 or 6 or 7)
                        continue;

                    if(bytes[k] != signatures[2][k])
                        return false;
                }

                return true;
            }

            return true;
        }

        return true;
    }

    public static bool IsMPEG(byte[] bytes)
    {
        if(bytes.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        for (var i = 0; i < bytes.Length; i++)
        {
            if(bytes[i] != MPEG.Values.First()[i])
                return false;
        }

        return true;
    }

    public static bool IsGIF(byte[] bytes)
    {
        if(bytes.Length < 6)
            throw new ArgumentException("The signature that you are verifying must be at least 6 bytes long");

        var signatures = GIF.Values.ToArray();

        for (var i = 0; i < bytes.Length; i++)
        {
            if (bytes[i] == signatures[0][i])
                continue;

            for (var y = 0; y < bytes.Length; y++)
            {
                if(bytes[y] != signatures[1][y])
                    return false;
            }

            return true;
        }

        return true;
    }

    public static bool IsVOB(byte[] bytes)
    {
        if(bytes.Length < 6)
            throw new ArgumentException("The signature that you are verifying must be at least 6 bytes long");

        for (var i = 0; i < bytes.Length; i++)
        {
            if(bytes[i] != VOB.Values.First()[i])
                return false;
        }

        return true;
    }

    public static bool IsM2TS(byte[] bytes)
    {
        if(bytes.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        for (var i = 0; i < bytes.Length; i++)
        {
            if(bytes[i] != M2TS.Values.First()[i])
                return false;
        }

        return true;
    }

    public static bool IsMXF(byte[] bytes)
    {
        if(bytes.Length < 14)
            throw new ArgumentException("The signature that you are verifying must be at least 14 bytes long");

        for (var i = 0; i < bytes.Length; i++)
        {
            if(bytes[i] != MXF.Values.First()[i])
                return false;
        }

        return true;
    }

    public static bool IsWEBM(byte[] bytes)
    {
        if(bytes.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        for (var i = 0; i < bytes.Length; i++)
        {
            if(bytes[i] != WEBM.Values.First()[i])
                return false;
        }

        return true;
    }

    public static bool IsGXF(byte[] bytes)
    {
        if(bytes.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        for (var i = 0; i < bytes.Length; i++)
        {
            if(bytes[i] != GXF.Values.First()[i])
                return false;
        }

        return true;
    }

    public static bool IsFLV(byte[] bytes)
    {
        if(bytes.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        for (var i = 0; i < bytes.Length; i++)
        {
            if(bytes[i] != FLV.Values.First()[i])
                return false;
        }

        return true;
    }

    public static bool IsOGG(byte[] bytes)
    {
        if(bytes.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        for (var i = 0; i < bytes.Length; i++)
        {
            if(bytes[i] != OGG.Values.First()[i])
                return false;
        }

        return true;
    }

    public static bool IsWMV(byte[] bytes)
    {
        if(bytes.Length < 16)
            throw new ArgumentException("The signature that you are verifying must be at least 16 bytes long");

        for (var i = 0; i < bytes.Length; i++)
        {
            if(bytes[i] != WMV.Values.First()[i])
                return false;
        }

        if(bytes.Length < 1024)
            return true;

        var aspectRatio = Encoding.Unicode.GetBytes("AspectRatio");
        var windowsMediaVideo = Encoding.Unicode.GetBytes("WindowsMediaVideo");
        var wmv3 = Encoding.ASCII.GetBytes("WMV3");
        var deviceConformanceTemplate = Encoding.Unicode.GetBytes("DeviceConformanceTemplate MP @ML");

        var buffer = new byte[1024];

        var aspectRatioIndex = IndexOf(buffer, aspectRatio);
        var windowsMediaVideoIndex = IndexOf(buffer, windowsMediaVideo);
        var wmv3Index = IndexOf(buffer, wmv3);
        var deviceConformanceTemplateIndex = IndexOf(buffer, deviceConformanceTemplate);

        if (aspectRatioIndex != -1)
            return true;

        if (windowsMediaVideoIndex != -1)
            return true;

        if (wmv3Index != -1)
            return true;

        return deviceConformanceTemplateIndex != -1;
    }

    public static bool IsBMP(byte[] bytes)
    {
        if(bytes.Length < 2)
            throw new ArgumentException("The signature that you are verifying must be at least 2 bytes long");

        for (var i = 0; i < bytes.Length; i++)
        {
            if(bytes[i] != BMP.Values.First()[i])
                return false;
        }

        return true;
    }

    private static int IndexOf(byte[] source, byte[] pattern)
    {
        for (var i = 0; i <= source.Length - pattern.Length; i++)
        {
            var match = !pattern.Where((t, j) => source[i + j] != t).Any();

            if (match)
                return i;
        }

        return -1;
    }

    public static bool IsASF(byte[] bytes)
    {
        if(bytes.Length < 16)
            throw new ArgumentException("The signature that you are verifying must be at least 16 bytes long");

        for (var i = 0; i < bytes.Length; i++)
        {
            if(bytes[i] != ASF.Values.First()[i])
                return false;
        }

        return true;
    }

    public static bool IsMP3(byte[] bytes)
    {
        if(bytes.Length < 3)
            throw new ArgumentException("The signature that you are verifying must be at least 3 bytes long");

        for (var i = 0; i < bytes.Length; i++)
        {
            if(bytes[i] != MP3.Values.First()[i])
                return false;
        }

        return true;
    }

    public static bool IsRM(byte[] bytes)
    {
        if(bytes.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        for (var i = 0; i < bytes.Length; i++)
        {
            if(bytes[i] != RM.Values.First()[i])
                return false;
        }

        return true;
    }

    public static bool IsPSD(byte[] bytes)
    {
        if(bytes.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        for (var i = 0; i < bytes.Length; i++)
        {
            if(bytes[i] != PSD.Values.First()[i])
                return false;
        }

        return true;
    }

    public static bool IsWEBP(byte[] bytes)
    {
        if(bytes.Length < 12)
            throw new ArgumentException("The signature that you are verifying must be at least 12 bytes long");

        var signatures = WEBP.Values.ToArray();

        for (var i = 0; i < bytes.Length; i++)
        {
            if(i is 4 or 5 or 6 or 7)
                continue;

            if (bytes[i] == signatures[0][i])
                continue;

            for (var y = 0; y < bytes.Length; y++)
            {
                if(y is 4 or 5 or 6 or 7)
                    continue;

                if(bytes[y] != signatures[1][y])
                    return false;
            }

            return true;
        }

        return true;
    }

    public static bool IsWAV(byte[] bytes)
    {
        if(bytes.Length < 12)
            throw new ArgumentException("The signature that you are verifying must be at least 12 bytes long");

        var signatures = WAV.Values.ToArray();

        for (var i = 0; i < bytes.Length; i++)
        {
            if(i is 4 or 5 or 6 or 7)
                continue;

            if (bytes[i] == signatures[0][i])
                continue;

            for (var y = 0; y < bytes.Length; y++)
            {
                if(y is 4 or 5 or 6 or 7)
                    continue;

                if(bytes[y] == signatures[1][y])
                    continue;

                for (var k = 0; k < bytes.Length; k++)
                {
                    if(k is 4 or 5 or 6 or 7)
                        continue;

                    if(bytes[k] == signatures[2][k])
                        continue;

                    for (var l = 0; l < bytes.Length; l++)
                    {
                        if(l is 4 or 5 or 6 or 7)
                            continue;

                        if(bytes[l] != signatures[3][l])
                            return false;
                    }

                    return true;
                }

                return true;
            }

            return true;
        }

        return true;
    }

    public static bool IsFLAC(byte[] bytes)
    {
        if(bytes.Length < 4)
            throw new ArgumentException("The signature that you are verifying must be at least 4 bytes long");

        var signatures = FLAC.Values.ToArray();

        for (var i = 0; i < bytes.Length; i++)
        {
            if (bytes[i] == signatures[0][i])
                continue;

            for (var y = 0; y < bytes.Length; y++)
            {
                if(bytes[y] == signatures[1][y])
                    continue;

                for (var k = 0; k < bytes.Length; k++)
                {
                    if(bytes[k] == signatures[2][k])
                        continue;

                    for (var l = 0; l < bytes.Length; l++)
                    {
                        if(bytes[l] != signatures[3][l])
                            return false;
                    }

                    return true;
                }

                return true;
            }

            return true;
        }

        return true;
    }

    public static bool IsAAC(byte[] bytes)
    {
        if(bytes.Length < 2)
            throw new ArgumentException("The signature that you are verifying must be at least 2 bytes long");

        var signatures = AAC.Values.ToArray();

        for (var i = 0; i < bytes.Length; i++)
        {
            if (bytes[i] == signatures[0][i])
                continue;

            for (var y = 0; y < bytes.Length; y++)
            {
                if(bytes[y] == signatures[1][y])
                    continue;

                for (var k = 0; k < bytes.Length; k++)
                {
                    if(bytes[k] == signatures[2][k])
                        continue;

                    for (var l = 0; l < bytes.Length; l++)
                    {
                        if(bytes[l] != signatures[3][l])
                            return false;
                    }

                    return true;
                }

                return true;
            }

            return true;
        }

        return true;
    }

    public static bool IsWMA(byte[] bytes)
    {
        if(bytes.Length < 16)
            throw new ArgumentException("The signature that you are verifying must be at least 16 bytes long");

        var signatures = WMA.Values.ToArray();

        for (var i = 0; i < bytes.Length; i++)
        {
            if (bytes[i] == signatures[0][i])
                continue;

            for (var y = 0; y < bytes.Length; y++)
            {
                if(y is 4 or 5 or 6 or 7 or 8 or 9 or 10 or 11 or 12 or 13 or 14 or 15)
                    continue;

                if(bytes[y] == signatures[1][y])
                    continue;

                for (var k = 0; k < bytes.Length; k++)
                {
                    if(y is 2 or 3 or 4 or 5 or 6 or 7 or 8 or 9 or 10 or 11 or 12 or 13 or 14 or 15)
                        continue;

                    if(bytes[k] != signatures[2][k])
                        return false;
                }

                return true;
            }

            return true;
        }

        return true;
    }
}