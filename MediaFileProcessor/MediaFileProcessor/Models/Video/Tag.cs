namespace MediaFileProcessor.Models.Video;

/// <summary>
/// Represents a tag information for a media file
/// </summary>
public class Tag
{
    /// <summary>
    /// Represents the creation time of the tag
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// Represents the language of the tag in string format
    /// </summary>
    public string? Language { get; set; }

    /// <summary>
    /// Represents the handler name of the tag in string format
    /// </summary>
    public string? HandlerName { get; set; }

    /// <summary>
    /// Represents the vendor ID of the tag in string format
    /// </summary>
    public string? VendorId { get; set; }
}