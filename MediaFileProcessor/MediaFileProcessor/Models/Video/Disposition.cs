namespace MediaFileProcessor.Models.Video;

/// <summary>
/// Class representing the disposition of media file streams.
/// </summary>
public class Disposition
{
    /// <summary>
    /// Indicates the default stream
    /// </summary>
    public int Default { get; set; }

    /// <summary>
    /// Indicates the dub stream
    /// </summary>
    public int Dub { get; set; }

    /// <summary>
    /// Indicates the original stream
    /// </summary>
    public int Original { get; set; }

    /// <summary>
    /// Indicates the comment stream
    /// </summary>
    public int Comment { get; set; }

    /// <summary>
    /// Indicates the lyrics stream
    /// </summary>
    public int Lyrics { get; set; }

    /// <summary>
    /// Indicates the karaoke stream
    /// </summary>
    public int Karaoke { get; set; }

    /// <summary>
    /// Indicates the forced stream
    /// </summary>
    public int Forced { get; set; }

    /// <summary>
    /// Indicates the hearing impaired stream
    /// </summary>
    public int HearingImpaired { get; set; }

    /// <summary>
    /// Indicates the visual impaired stream
    /// </summary>
    public int VisualImpaired { get; set; }

    /// <summary>
    /// Indicates the clean effects stream
    /// </summary>
    public int CleanEffects { get; set; }

    /// <summary>
    /// Represents the number of attached pictures.
    /// </summary>
    public int AttachedPic { get; set; }

    /// <summary>
    /// Represents the number of timed thumbnails
    /// </summary>
    public int TimedThumbnails { get; set; }

    /// <summary>
    /// Represents the number of captions
    /// </summary>
    public int Captions { get; set; }

    /// <summary>
    /// Represents the number of descriptions
    /// </summary>
    public int Descriptions { get; set; }

    /// <summary>
    /// Represents the number of metadata
    /// </summary>
    public int Metadata { get; set; }

    /// <summary>
    /// Represents the number of dependent items
    /// </summary>
    public int Dependent { get; set; }

    /// <summary>
    /// Represents the number of still images
    /// </summary>
    public int StillImage { get; set; }
}