namespace MediaFileProcessor.Models.Video;

/// <summary>
/// Represents the information of a stream in a media file
/// </summary>
public class StreamInfo
{
    /// <summary>
    /// Represents the index of the stream
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// Represents the codec name of the stream in string format
    /// </summary>
    public string? CodecName { get; set; }

    /// <summary>
    /// Represents the codec long name of the stream in string format
    /// </summary>
    public string? CodecLongName { get; set; }

    /// <summary>
    /// Represents the profile of the stream in string format
    /// </summary>
    public string? Profile { get; set; }

    /// <summary>
    /// Represents the codec type of the stream in string format
    /// </summary>
    public string? CodecType { get; set; }

    /// <summary>
    /// Represents the codec tag string of the stream in string format
    /// </summary>
    public string? CodecTagString { get; set; }

    /// <summary>
    /// Represents the codec tag of the stream in string format
    /// </summary>
    public string? CodecTag { get; set; }

    /// <summary>
    /// Represents the width of the stream in pixels
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// Represents the height of the stream in pixels
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// Represents the coded width of the stream in pixels
    /// </summary>
    public int CodedWidth { get; set; }

    /// <summary>
    /// Represents the coded height of the stream in pixels
    /// </summary>
    public int CodedHeight { get; set; }

    /// <summary>
    /// Represents the number of closed captions in the video.
    /// </summary>
    public int ClosedCaptions { get; set; }

    /// <summary>
    /// Represents the presence of film grain effect in the video.
    /// </summary>
    public int FilmGrain { get; set; }

    /// <summary>
    /// Indicates if the video has B frames.
    /// </summary>
    public int HasBFrames { get; set; }

    /// <summary>
    /// Represents the sample aspect ratio of the video.
    /// </summary>
    public string? SampleAspectRatio { get; set; }

    /// <summary>
    /// Represents the display aspect ratio of the video.
    /// </summary>
    public string? DisplayAspectRatio { get; set; }

    /// <summary>
    /// Represents the pixel format of the video.
    /// </summary>
    public string? PixFmt { get; set; }

    /// <summary>
    /// Represents the level of the video.
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// Represents the color range of the video.
    /// </summary>
    public string? ColorRange { get; set; }

    /// <summary>
    /// Represents the color space of the video.
    /// </summary>
    public string? ColorSpace { get; set; }

    /// <summary>
    /// Represents the color transfer characteristic of the video.
    /// </summary>
    public string? ColorTransfer { get; set; }

    /// <summary>
    /// Represents the color primaries of the video.
    /// </summary>
    public string? ColorPrimaries { get; set; }

    /// <summary>
    /// Represents the chroma location of the video.
    /// </summary>
    public string? ChromaLocation { get; set; }

    /// <summary>
    /// Represents the field order of the video.
    /// </summary>
    public string? FieldOrder { get; set; }

    /// <summary>
    /// Represents the number of reference frames in the video.
    /// </summary>
    public int Refs { get; set; }

    /// <summary>
    /// Indicates if the video is in AVC format.
    /// </summary>
    public string? IsAvc { get; set; }

    /// <summary>
    /// The size of the NAL unit, expressed as the number of bytes of the prefix that determine the size of each NAL unit.
    /// </summary>
    public string? NalLengthSize { get; set; }

    /// <summary>
    /// The identifier of the stream.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// The real frame rate of the stream.
    /// </summary>
    public string? RFrameRate { get; set; }

    /// <summary>
    /// The average frame rate of the stream.
    /// </summary>
    public string? AvgFrameRate { get; set; }

    /// <summary>
    /// The time base of the stream.
    /// </summary>
    public string? TimeBase { get; set; }

    /// <summary>
    /// The presentation timestamp of the first frame of the stream, in stream time base.
    /// </summary>
    public int StartPts { get; set; }

    /// <summary>
    /// The start time of the stream, expressed as a string.
    /// </summary>
    public string? StartTime { get; set; }

    /// <summary>
    /// The duration of the stream, expressed in stream time base units.
    /// </summary>
    public int DurationTs { get; set; }

    /// <summary>
    /// The duration of the stream, expressed as a string.
    /// </summary>
    public string? Duration { get; set; }

    /// <summary>
    /// The bit rate of the stream, expressed as a string.
    /// </summary>
    public string? BitRate { get; set; }

    /// <summary>
    /// The number of bits per raw audio sample.
    /// </summary>
    public string? BitsPerRawSample { get; set; }

    /// <summary>
    /// The number of frames in the stream.
    /// </summary>
    public string? NbFrames { get; set; }

    /// <summary>
    /// The size of the extra data in the stream.
    /// </summary>
    public int ExtradataSize { get; set; }

    /// <summary>
    /// The disposition of the stream, if any.
    /// </summary>
    public Disposition? Disposition { get; set; }

    /// <summary>
    /// The tag associated with the stream, if any.
    /// </summary>
    public Tag? Tag { get; set; }

    /// <summary>
    /// A list of side data in the stream, if any.
    /// </summary>
    public List<SideDataList>? SideDataList { get; set; }

    /// <summary>
    /// The sample format of the stream.
    /// </summary>
    public string? SampleFmt { get; set; }

    /// <summary>
    /// The sample rate of the stream.
    /// </summary>
    public string? SampleRate { get; set; }

    /// <summary>
    /// The number of channels in the stream.
    /// </summary>
    public int? Channels { get; set; }

    /// <summary>
    /// The channel layout of the stream.
    /// </summary>
    public string? ChannelLayout { get; set; }

    /// <summary>
    /// The number of bits per sample in the stream.
    /// </summary>
    public int? BitsPerSample { get; set; }
}