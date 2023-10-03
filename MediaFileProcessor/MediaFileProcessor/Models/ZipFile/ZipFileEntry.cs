using MediaFileProcessor.Models.Enums;

namespace MediaFileProcessor.Models.ZipFile;

/// <summary>
/// Represents an entry in Zip file directory
/// </summary>
public class ZipFileEntry
{
    /// <summary>
    /// Compression method
    /// </summary>
    public ZipCompression Method;

    /// <summary>
    /// Full path and filename as stored in Zip
    /// </summary>
    public string? FilenameInZip;

    /// <summary>
    /// Original file size
    /// </summary>
    public long FileSize;

    /// <summary>
    /// Compressed file size
    /// </summary>
    public long CompressedSize;

    /// <summary>
    /// Offset of header information inside Zip storage
    /// </summary>
    public long HeaderOffset;

    /// <summary>
    /// Offset of file inside Zip
    /// </summary>
    public long FileOffset;

    /// <summary>
    /// Size of header information
    /// </summary>
    public uint HeaderSize;

    /// <summary>
    /// 32-bit checksum of entire file
    /// </summary>
    public uint Crc32;

    /// <summary>
    /// Last modification time of file
    /// </summary>
    public DateTime ModifyTime;

    /// <summary>
    /// Creation time of file
    /// </summary>
    public DateTime CreationTime;

    /// <summary>
    /// Last access time of file
    /// </summary>
    public DateTime AccessTime;

    /// <summary>
    /// User comment for file
    /// </summary>
    public string? Comment;

    /// <summary>
    /// True if UTF8 encoding for filename and comments, false if default (CP 437)
    /// </summary>
    public bool EncodeUTF8;

    /// <summary>
    /// Overriden method
    /// </summary>
    /// <returns>Filename in Zip</returns>
    public override string? ToString()
    {
        return FilenameInZip;
    }
}