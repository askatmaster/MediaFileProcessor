namespace MediaFileProcessor.Models.Video;

public class StreamInfo
{
    public int Index { get; set; }

    public string? CodecName { get; set; }

    public string? CodecLongName { get; set; }

    public string? Profile { get; set; }

    public string? CodecType { get; set; }

    public string? CodecTagString { get; set; }

    public string? CodecTag { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public int CodedWidth { get; set; }

    public int CodedHeight { get; set; }

    public int ClosedCaptions { get; set; }

    public int FilmGrain { get; set; }

    public int HasBFrames { get; set; }

    public string? SampleAspectRatio { get; set; }

    public string? DisplayAspectRatio { get; set; }

    public string? PixFmt { get; set; }

    public int Level { get; set; }

    public string? ColorRange { get; set; }

    public string? ColorSpace { get; set; }

    public string? ColorTransfer { get; set; }

    public string? ColorPrimaries { get; set; }

    public string? ChromaLocation { get; set; }

    public string? FieldOrder { get; set; }

    public int Refs { get; set; }

    public string? IsAvc { get; set; }

    public string? NalLengthSize { get; set; }

    public string? Id { get; set; }

    public string? RFrameRate { get; set; }

    public string? AvgFrameRate { get; set; }

    public string? TimeBase { get; set; }

    public int StartPts { get; set; }

    public string? StartTime { get; set; }

    public int DurationTs { get; set; }

    public string? Duration { get; set; }

    public string? BitRate { get; set; }

    public string? BitsPerRawSample { get; set; }

    public string? NbFrames { get; set; }

    public int ExtradataSize { get; set; }

    public Disposition? Disposition { get; set; }

    public Tag? Tag { get; set; }

    public List<SideDataList>? SideDataList { get; set; }

    public string? SampleFmt { get; set; }

    public string? SampleRate { get; set; }

    public int? Channels { get; set; }

    public string? ChannelLayout { get; set; }

    public int? BitsPerSample { get; set; }
}