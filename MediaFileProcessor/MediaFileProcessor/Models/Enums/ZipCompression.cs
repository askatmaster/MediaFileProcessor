namespace MediaFileProcessor.Models.Enums;

/// <summary>
/// Compression method enumeration
/// </summary>
public enum ZipCompression : ushort
{
    /// <summary>
    /// Uncompressed storage
    /// </summary>
    Store = 0,

    /// <summary>
    /// Deflate compression method
    /// </summary>
    Deflate = 8
}