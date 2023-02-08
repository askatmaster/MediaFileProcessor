namespace MediaFileProcessor.Models.Video;

/// <summary>
/// Represents a crop area with its X, Y, Width and Height values.
/// </summary>
public class CropArea
{
    /// <summary>
    /// The X coordinate of the top-left corner of the crop area.
    /// </summary>
    public int X { get; set; }

    /// <summary>
    /// The Y coordinate of the top-left corner of the crop area.
    /// </summary>
    public int Y { get; set; }

    /// <summary>
    /// The width of the crop area.
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// The height of the crop area.
    /// </summary>
    public int Height { get; set; }
}