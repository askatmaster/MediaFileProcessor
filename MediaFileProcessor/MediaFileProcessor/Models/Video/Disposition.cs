namespace MediaFileProcessor.Models.Video;

public class Disposition
{
    public int Default { get; set; }

    public int Dub { get; set; }

    public int Original { get; set; }

    public int Comment { get; set; }

    public int Lyrics { get; set; }

    public int Karaoke { get; set; }

    public int Forced { get; set; }

    public int HearingImpaired { get; set; }

    public int VisualImpaired { get; set; }

    public int CleanEffects { get; set; }

    public int AttachedPic { get; set; }

    public int TimedThumbnails { get; set; }

    public int Captions { get; set; }

    public int Descriptions { get; set; }

    public int Metadata { get; set; }

    public int Dependent { get; set; }

    public int StillImage { get; set; }
}