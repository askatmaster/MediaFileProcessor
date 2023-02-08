namespace MediaFileProcessor.Models.Video;

/// <summary>
/// Represents a list of side data for a media file
/// </summary>
public class SideDataList
{
    /// <summary>
    /// Represents the type of the side data in string format
    /// </summary>
    public string? SideDataType { get; set; }

    /// <summary>
    /// Represents the display matrix of the side data in string format
    /// </summary>
    public string? Displaymatrix { get; set; }

    /// <summary>
    /// Represents the rotation of the side data
    /// </summary>
    public int Rotation { get; set; }
}