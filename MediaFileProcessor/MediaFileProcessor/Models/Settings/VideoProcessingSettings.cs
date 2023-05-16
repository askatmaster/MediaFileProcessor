using MediaFileProcessor.Extensions;
using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Enums;
using MediaFileProcessor.Models.Video;
namespace MediaFileProcessor.Models.Settings;

public class VideoProcessingSettings : ProcessingSettings
{
    /// <summary>
    /// Overwrite output files without asking.
    /// </summary>
    public VideoProcessingSettings ReplaceIfExist()
    {
        _stringBuilder.Append(" -y ");

        return this;
    }

    /// <summary>
    /// Suppress printing banner.
    /// All FFmpeg tools will normally show a copyright notice, build options and library versions. This option can be used to suppress printing this information.
    /// </summary>
    public VideoProcessingSettings HideBanner()
    {
        _stringBuilder.Append(" -hide_banner ");

        return this;
    }

    /// <summary>
    /// On a multi-core computer, determines how many threads will be consumed for processing depending on the codec. By default, it will consume the optimal number of threads.
    /// </summary>
    public VideoProcessingSettings Threads(int threads)
    {
        _stringBuilder.Append($" -threads {threads} ");

        return this;
    }

    /// <summary>
    /// Enable to skip writing the name inside a hdlr box. Default is false.
    /// </summary>
    public VideoProcessingSettings EmptyHdlrName(bool value)
    {
        _stringBuilder.Append($" -empty_hdlr_name {value} ");

        return this;
    }

    /// <summary>
    /// Hardware Acceleration Output Format - Force HWAccel if selected
    /// </summary>
    public VideoProcessingSettings HardwareAcceleration(HardwareAccelerationType hardwareAcceleration, bool hardwareAccelerationOutputFormatCopy)
    {
        _stringBuilder.Append($" -hwaccel {hardwareAcceleration.ToString().ToLowerInvariant()} ");
        if(hardwareAccelerationOutputFormatCopy)
            _stringBuilder.Append($" -hwaccel_output_format {HardwareAccelerationType.CUDA.ToString().ToLowerInvariant()} ");

        return this;
    }

    /// <summary>
    /// Audio bitrate
    /// </summary>
    public VideoProcessingSettings AudioBitRate(int bitRate)
    {
        _stringBuilder.Append($" -ab {bitRate}k ");

        return this;
    }

    /// <summary>
    /// Audio bitrate
    /// </summary>
    public VideoProcessingSettings AudioBitRate(AudioBitrateType bitRate)
    {
        _stringBuilder.Append($" -ab {bitRate.ToString().Replace("_", "")} ");

        return this;
    }

    /// <summary>
    /// Audio sample rate
    /// Set the audio sampling frequency. For output streams it is set by default to the frequency of the corresponding input stream.
    /// For input streams this option only makes sense for audio grabbing devices and raw demuxers and is mapped to the corresponding demuxer options.
    /// </summary>
    public VideoProcessingSettings AudioSampleRate(AudioSampleRateType audioSampleRate)
    {
        _stringBuilder.Append($" -ar {audioSampleRate.ToString().Replace("Hz", "")} ");

        return this;
    }

    /// <summary>
    /// Audio codec
    /// </summary>
    public VideoProcessingSettings AudioCodec(AudioCodecType audioCodec)
    {
        _stringBuilder.Append($" -c:a {audioCodec.ToString().ToLowerInvariant()} ");

        return this;
    }

    /// <summary>
    /// Set the subtitle codec
    /// </summary>
    public VideoProcessingSettings SubtitlesCodec(string arg)
    {
        _stringBuilder.Append($" -c:s {arg} ");

        return this;
    }

    /// <summary>
    /// Set the subtitle codec
    /// </summary>
    public VideoProcessingSettings SubtitlesCodec(SubtitleCodecType arg)
    {
        _stringBuilder.Append($" -c:s {arg.ToString().ToLowerInvariant()} ");

        return this;
    }

    /// <summary>
    /// Delete audio from video
    /// </summary>
    public VideoProcessingSettings DeleteAudio()
    {
        _stringBuilder.Append(" -an ");

        return this;
    }

    /// <summary>
    /// Dump video coding statistics to vstats_HHMMSS.log.
    /// </summary>
    public VideoProcessingSettings VStats()
    {
        _stringBuilder.Append(" -vstats ");

        return this;
    }

    /// <summary>
    /// Calculate PSNR of compressed frames. This option is deprecated, pass the PSNR flag to the encoder instead, using -flags +psnr.
    /// </summary>
    public VideoProcessingSettings PSNR()
    {
        _stringBuilder.Append(" -psnr ");

        return this;
    }

    /// <summary>
    /// When used as an input option (before -i), limit the duration of data read from the input file.
    /// When used as an output option (before an output url), stop writing the output after its duration reaches duration.
    /// -to and -t are mutually exclusive and -t has priority
    /// </summary>
    public VideoProcessingSettings MaxDuration(TimeSpan duration)
    {
        _stringBuilder.Append($" -t {duration} ");

        return this;
    }

    /// <summary>
    /// Stop writing the output or reading the input at position. position must be a time duration specification
    /// -to and -t are mutually exclusive and -t has priority
    /// </summary>
    public VideoProcessingSettings TimePosition(TimeSpan position)
    {
        _stringBuilder.Append($" -to {position.TotalSeconds:00:00:00.000} ".Replace(",", "."));

        return this;
    }

    /// <summary>
    /// The frame to begin seeking from.
    /// </summary>
    public VideoProcessingSettings Seek(TimeSpan seek)
    {
        _stringBuilder.Append($" -ss {seek.TotalSeconds:00:00:00.000} ".Replace(",", "."));

        return this;
    }

    /// <summary>
    /// Like the -ss (Seek) option but relative to the "end of file". That is negative values are earlier in the file, 0 is at EOF
    /// </summary>
    public VideoProcessingSettings SeekOf(TimeSpan seek)
    {
        _stringBuilder.Append($" -sseof {seek.TotalSeconds:00:00:00.000} ".Replace(",", "."));

        return this;
    }

    /// <summary>
    /// Sync audio
    /// </summary>
    public VideoProcessingSettings ASync(AudioSyncMethodType index)
    {
        _stringBuilder.Append($" -async {index.ToString().ToLowerInvariant()}");

        return this;
    }

    /// <summary>
    /// Sync video
    /// </summary>
    public VideoProcessingSettings VSync(VideoSyncMethodType index)
    {
        _stringBuilder.Append($" -vsync {index.ToString().ToLowerInvariant()}");

        return this;
    }

    /// <summary>
    /// Set the input time offset
    /// </summary>
    public VideoProcessingSettings InputTimeOffset(TimeSpan offset)
    {
        _stringBuilder.Append($" -itsoffset {offset.TotalSeconds:00:00:00.000} ".Replace(",", "."));

        return this;
    }

    /// <summary>
    /// Rescale input timestamps. scale should be a floating point number
    /// </summary>
    public VideoProcessingSettings InputTimestampScale(TimeSpan scale)
    {
        _stringBuilder.Append($" -itsscale {scale.TotalSeconds:00:00:00.000} ".Replace(",", "."));

        return this;
    }

    /// <summary>
    /// Set the recording timestamp in the container
    /// </summary>
    public VideoProcessingSettings Timestamp(TimeSpan timestamp)
    {
        _stringBuilder.Append($" -timestamp {timestamp.TotalSeconds:00:00:00.000} ".Replace(",", "."));

        return this;
    }

    /// <summary>
    /// As an input option, blocks all data streams of a file from being filtered or being automatically selected or mapped for any output (-dn)
    /// </summary>
    public VideoProcessingSettings BlockAllDataStreams()
    {
        _stringBuilder.Append(" -dn ");

        return this;
    }

    /// <summary>
    /// Fix subtitles durations. For each subtitle, wait for the next packet in the same stream and adjust the duration of the first to avoid overlap.
    /// This is necessary with some subtitles codecs, especially DVB subtitles, because the duration in the original packet is only a rough estimate and the end is actually marked by an empty subtitle frame.
    /// Failing to use this option when necessary can result in exaggerated durations or muxing failures due to non-monotonic timestamps.
    /// Note that this option will delay the output of all data until the next subtitle packet is decoded: it may increase memory consumption and latency a lot
    /// </summary>
    public VideoProcessingSettings FixSubDuration()
    {
        _stringBuilder.Append(" -fix_sub_duration");

        return this;
    }

    /// <summary>
    /// Set the number of data frames to output. This is an obsolete alias for -frames:d, which you should use instead
    /// </summary>
    public VideoProcessingSettings DataFramesOutput(int number)
    {
        _stringBuilder.Append($" -dframes {number}");

        return this;
    }

    /// <summary>
    /// Set the number of audio frames to output. This is an obsolete alias for -frames:a, which you should use instead.
    /// </summary>
    public VideoProcessingSettings AudioFramesOutput(int number)
    {
        _stringBuilder.Append($" -aframes {number}");

        return this;
    }

    /// <summary>
    /// Sets the keyframe interval to 30 frames.
    /// This means that every 30th frame will be a keyframe, which allows for better seeking and compression efficiency
    /// </summary>
    public VideoProcessingSettings KeyFrame(int frameNum)
    {
        _stringBuilder.Append($" -g {frameNum}");

        return this;
    }

    /// <summary>
    /// Create the filtergraph specified by filtergraph and use it to filter the stream.
    /// This is an alias for -filter:a
    /// </summary>
    public VideoProcessingSettings AudioFilterGraph(string value)
    {
        _stringBuilder.Append($" -af {value}");

        return this;
    }

    /// <summary>
    /// Create the filtergraph specified by filtergraph and use it to filter the stream.
    /// This is an alias for -filter:a
    /// </summary>
    public VideoProcessingSettings AudioFilterGraph(AudioFilterType value)
    {
        _stringBuilder.Append($" -af {value.ToString().ToLowerInvariant()}");

        return this;
    }

    /// <summary>
    /// Stop writing to the stream after framecount frames
    /// </summary>
    public VideoProcessingSettings Frames(string value, string? stream = null)
    {
        _stringBuilder.Append($" -frames{stream} {value}");

        return this;
    }

    /// <summary>
    /// Use fixed quality scale (VBR). The meaning of q/qscale is codec-dependent.
    /// If qscale is used without a stream_specifier then it applies only to the video stream, this is to maintain compatibility with previous behavior
    /// and as specifying the same codec specific value to 2 different codecs that is audio and video generally is not what is intended when no stream_specifier is used
    /// </summary>
    public VideoProcessingSettings QualityScale(string value, string? stream = null)
    {
        _stringBuilder.Append($" -qscale{stream} {value}");

        return this;
    }

    /// <summary>
    /// Create the filtergraph specified by filtergraph and use it to filter the stream
    /// </summary>
    public VideoProcessingSettings Filter(string value, string? stream = null)
    {
        _stringBuilder.Append($" -filter{stream} {value}");

        return this;
    }

    /// <summary>
    /// Set maximum frame rate (Hz value, fraction or abbreviation)
    /// </summary>
    public VideoProcessingSettings FpsMax(string value, string? stream = null)
    {
        _stringBuilder.Append($" -fpsmax{stream} {value}");

        return this;
    }

    /// <summary>
    /// Set the audio quality (codec-specific, VBR).
    /// For libmp3lame (MP3) and libvorbis (OGG), the -aq value defines the audio quality in the range from 0 (best quality) to 9 (smallest file size).
    /// For the libfdk_aac (AAC) codec, the value determines the bitrate in bits per second. However,
    /// this is not a complete list of possible values for all codecs, and each codec can have its own quality settings.
    /// </summary>
    public VideoProcessingSettings AudioQuality(string value)
    {
        _stringBuilder.Append($" -aq {value}");

        return this;
    }

    /// <summary>
    /// Sets the disposition for a stream
    /// By default, the disposition is copied from the input stream, unless the output stream this option applies to is fed by a complex filtergraph - in that case the disposition is unset by default
    /// </summary>
    public VideoProcessingSettings Disposition(string value, string? stream = null)
    {
        _stringBuilder.Append($" -disposition{stream} {value}");

        return this;
    }

    /// <summary>
    /// Set frame size.
    /// As an input option, this is a shortcut for the video_size private option,
    /// recognized by some demuxers for which the frame size is either not stored
    /// in the file or is configurable – e.g. raw video or video grabbers
    /// </summary>
    public VideoProcessingSettings FrameSize(string value, string? stream = null)
    {
        _stringBuilder.Append($" -s{stream} {value}");

        return this;
    }

    /// <summary>
    /// Force audio tag/fourcc
    /// </summary>
    public VideoProcessingSettings AudioTagOrFourcc(string value)
    {
        _stringBuilder.Append($" -atag {value}");

        return this;
    }

    /// <summary>
    /// Set the audio sample format
    /// </summary>
    public VideoProcessingSettings AudioSampleFormat(string value, string? stream = null)
    {
        _stringBuilder.Append($" -sample_fmt{stream} {value}");

        return this;
    }

    /// <summary>
    /// Set the number of audio frames to output
    /// </summary>
    public VideoProcessingSettings AudioFramesOutput(string value)
    {
        _stringBuilder.Append($" -frames:a {value}");

        return this;
    }

    /// <summary>
    /// Enable interaction on standard input. On by default unless standard input is used as an input.
    /// To explicitly disable interaction you need to specify -nostdin
    /// </summary>
    public VideoProcessingSettings StdIn()
    {
        _stringBuilder.Append(" -stdin ");

        return this;
    }

    /// <summary>
    /// Disable interaction on standard input.
    /// </summary>
    public VideoProcessingSettings NoStdIn()
    {
        _stringBuilder.Append(" -nostdin ");

        return this;
    }

    /// <summary>
    /// Specify target file type (vcd, svcd, dvd, dv, dv50). type may be prefixed with pal-, ntsc- or film- to use the corresponding standard.
    /// All the format options (bitrate, codecs, buffer sizes) are then set automatically
    /// </summary>
    /// <param name="target">Predefined audio and video options for various file formats</param>
    /// <param name="targetStandard">Predefined standard</param>
    public VideoProcessingSettings Target(TargetType target, TargetStandardType? targetStandard)
    {
        _stringBuilder.Append(" -target ");

        _stringBuilder.Append(targetStandard is not null ? $" {targetStandard.ToString().ToLowerInvariant()}-{target.ToString().ToLowerInvariant()} "
                                  : $" {target.ToString().ToLowerInvariant()} ");

        return this;
    }

    /// <summary>
    /// Video aspect ratios
    /// </summary>
    public VideoProcessingSettings VideoAspectRatio(VideoAspectRatioType ratioType)
    {
        _stringBuilder.Append($" -aspect {ratioType.ToString()[1..].Replace("_", ":")} ");

        return this;
    }

    /// <summary>
    /// Video bit rate in kbit/s
    /// </summary>
    public VideoProcessingSettings VideoBitRate(int bitRate)
    {
        _stringBuilder.Append($" -b:v {bitRate}k ");

        return this;
    }

    /// <summary>
    /// This sets the maximum video bitrate.
    /// </summary>
    public VideoProcessingSettings MaxRate(int bitRate)
    {
        _stringBuilder.Append($" -maxrate {bitRate}k ");

        return this;
    }

    /// <summary>
    /// Channel audio
    /// </summary>
    public VideoProcessingSettings AudioChannel(int channel)
    {
        _stringBuilder.Append($" -ac {channel} ");

        return this;
    }

    /// <summary>
    /// Video frame rate
    /// </summary>
    public VideoProcessingSettings VideoFps(int fps)
    {
        _stringBuilder.Append($" -r {fps} ");

        return this;
    }

    /// <summary>
    /// This specifies the H.264 level to use. In this case, we're using level 3.0, which is also widely supported by mobile devices
    /// </summary>
    public VideoProcessingSettings Level(double level)
    {
        _stringBuilder.Append($" -level {level} ");

        return this;
    }

    /// <summary>
    /// Pixel format. Available formats can be gathered via `ffmpeg -pix_fmts`.
    /// </summary>
    public VideoProcessingSettings PixelFormat(string format)
    {
        _stringBuilder.Append($" -pix_fmt {format} ");

        return this;
    }

    /// <summary>
    /// Pixel format. Available formats can be gathered via `ffmpeg -pix_fmts`.
    /// </summary>
    public VideoProcessingSettings PixelFormat(PixelFormatType formatType)
    {
        _stringBuilder.Append($" -pix_fmt {formatType.ToString().ToLowerInvariant()} ");

        return this;
    }

    /// <summary>
    /// Video sizes
    /// </summary>
    public VideoProcessingSettings VideoSize(VideoSizeType videoSize, int? witdh = null, int? height = null)
    {
        if(videoSize is VideoSizeType.CUSTOM)
        {
            if(witdh is null || height is null)
                throw new Exception("When choosing a custom value, you need to specify dimensions");

            _stringBuilder.Append($" -vf \"scale={witdh}:{height}\" ");
        }
        else
        {
            var size = videoSize.ToString().ToLowerInvariant();
            if (size.StartsWith("_"))
                size = size.Replace("_", "");
            if (size.Contains("_"))
                size = size.Replace("_", "-");

            _stringBuilder.Append($" -s {size} ");
        }

        return this;
    }

    /// <summary>
    /// Video codec
    /// </summary>
    public VideoProcessingSettings VideoCodec(VideoCodecType codec)
    {
        _stringBuilder.Append($" -c:v {codec.ToString().ToLowerInvariant()} ");

        return this;
    }

    /// <summary>
    /// Codec Preset (Tested for -vcodec libx264)
    /// </summary>
    public VideoProcessingSettings VideoCodecPreset(VideoCodecPresetType preset)
    {
        _stringBuilder.Append($" -preset {preset.ToString().ToLowerInvariant()} ");

        return this;
    }

    /// <summary>
    /// Codec Profile (Tested for -vcodec libx264)
    /// Specifies wheter or not to use a H.264 Profile
    /// </summary>
    public VideoProcessingSettings VideoCodecProfile(VideoCodecProfileType codecProfile)
    {
        _stringBuilder.Append($" -profile:v {codecProfile} ");

        return this;
    }

    /// <summary>
    /// Force input or output file format. The format is normally auto detected for input files and guessed from the file extension for output files, so this option is not needed in most cases
    /// </summary>
    public VideoProcessingSettings Format(FileFormatType formatType)
    {
        var format = formatType is FileFormatType.JPG ? "image2" : formatType.ToString().ToLowerInvariant();
        if (format.StartsWith("_"))
            format = format.Replace("_", "");

        _stringBuilder.Append(formatType is FileFormatType.TS ? " -f mpegts " : $" -f {format} ");

        return this;
    }

    /// <summary>
    /// Set Format. If you want to install a format that is not provided
    /// </summary>
    public VideoProcessingSettings Format(string? format)
    {
        if(format != null)
            _stringBuilder.Append($" -f {format} ");

        return this;
    }

    /// <summary>
    /// enables experimental AAC audio encoding, which is required for some older versions of ffmpeg.
    /// </summary>
    public VideoProcessingSettings Strict(FFmpegStrictness value)
    {
        _stringBuilder.Append($" -strict {value.ToString().ToLowerInvariant()} ");

        return this;
    }

    /// <summary>
    /// Video Speed Up / Down using setpts filter
    /// </summary>
    public VideoProcessingSettings VideoTimeScale(double scale)
    {
        _stringBuilder.Append($" -filter:v \"setpts = {scale.ToString().Replace(",", ".")} * PTS\" ");

        return this;
    }

    /// <summary>
    /// Map Metadata from INput to Output
    /// </summary>
    public VideoProcessingSettings MapMetadata()
    {
        _stringBuilder.Append(" -map_metadata 0 ");

        return this;
    }

    /// <summary>
    /// Enable bitexact mode for (de)muxer and (de/en)coder
    /// </summary>
    public VideoProcessingSettings BitExact()
    {
        _stringBuilder.Append(" -bitexact ");

        return this;
    }

    /// <summary>
    /// Set the maximum demux-decode delay.
    /// </summary>
    public VideoProcessingSettings MuxDelay(int seconds)
    {
        _stringBuilder.Append($" -muxdelay {seconds} ");

        return this;
    }

    /// <summary>
    /// Set the initial demux-decode delay.
    /// </summary>
    public VideoProcessingSettings MuxPreload(int seconds)
    {
        _stringBuilder.Append($" -muxpreload {seconds} ");

        return this;
    }

    /// <summary>
    /// The -map option is used to select which threads from the input(s) should be included in the output(s).
    /// This -map parameter can also be used to exclude certain threads with negative mapping.
    /// </summary>
    public VideoProcessingSettings MapArgument(string map)
    {
        _stringBuilder.Append($" -map {map} ");

        return this;
    }

    /// <summary>
    /// Set a metadata key/value pair
    /// </summary>
    public VideoProcessingSettings MetaData(string arg)
    {
        _stringBuilder.Append($" -metadata:{arg} ");

        return this;
    }

    /// <summary>
    /// Read with native frame rate. Perfect for streaming.
    /// Example (-metadata title="my title")
    /// </summary>
    public VideoProcessingSettings ReadNativeFrameRate()
    {
        _stringBuilder.Append(" -re ");

        return this;
    }

    /// <summary>
    /// Video framerate
    /// </summary>
    public VideoProcessingSettings FrameRate(int rate)
    {
        _stringBuilder.Append($" -framerate {rate} ");

        return this;
    }

    /// <summary>
    /// Specifies an optional rectangle from the source video to crop
    /// </summary>
    public VideoProcessingSettings CropArea(CropArea area)
    {
        _stringBuilder.Append($" -filter:v \"crop={area.Width}:{area.Height}:{area.X}:{area.Y}\" ");

        return this;
    }

    /// <summary>
    /// Number of video frames to output
    /// </summary>
    public VideoProcessingSettings FramesNumber(int num)
    {
        _stringBuilder.Append($" -frames:v {num} ");

        return this;
    }

    /// <summary>
    /// Copy input stream to output
    /// </summary>
    public VideoProcessingSettings CopyAllCodec()
    {
        _stringBuilder.Append(" -c copy ");

        return this;
    }

    /// <summary>
    /// Skip inclusion of video stream
    /// </summary>
    public VideoProcessingSettings DeleteVideo()
    {
        _stringBuilder.Append(" -vn ");

        return this;
    }

    /// <summary>
    /// Set the file size limit, expressed in bytes. No further chunk of bytes is written after the limit is exceeded.
    /// The size of the output file is slightly more than the requested file size
    /// </summary>
    public VideoProcessingSettings FileSize(string size)
    {
        _stringBuilder.Append($" -fs {size} ");

        return this;
    }

    /// <summary>
    /// Skip inclusion of subtitle stream
    /// </summary>
    public VideoProcessingSettings DeleteSubtitles()
    {
        _stringBuilder.Append(" -sn ");

        return this;
    }

    /// <summary>
    /// Skip inclusion of data stream
    /// </summary>
    public VideoProcessingSettings DeleteData()
    {
        _stringBuilder.Append(" -dn ");

        return this;
    }

    /// <summary>
    /// Complex filtergraphs are configured with the -filter_complex option
    /// </summary>
    public VideoProcessingSettings FilterComplexArgument()
    {
        _stringBuilder.Append(" -dn ");

        return this;
    }

    /// <summary>
    /// Set the number of times to loop the output. Use -1 for no loop, 0 for looping indefinitely (default)
    /// </summary>
    public VideoProcessingSettings FilterComplexArgument(string arg)
    {
        _stringBuilder.Append($" -filter_complex \"overlay={arg}\" ");

        return this;
    }

    /// <summary>
    /// Constant Rate Factor
    /// The values will depend on which encoder you're using
    /// For x264 your valid range is 0-51
    /// </summary>
    public VideoProcessingSettings Crf(int crf)
    {
        _stringBuilder.Append($" -crf {crf} ");

        return this;
    }

    /// <summary>
    /// Video BitStream Filter
    /// </summary>
    public VideoProcessingSettings VideoBSF(VideoBitstreamFilter bsf)
    {
        _stringBuilder.Append($" -bsf:v {bsf.ToString().ToLowerInvariant()} ");

        return this;
    }

    /// <summary>
    /// Subtitle BitStream Filter
    /// </summary>
    public VideoProcessingSettings SubtitleBSF(SubtitleBitstreamFilterType bsf)
    {
        _stringBuilder.Append($" -bsf:s {bsf.ToString().ToLowerInvariant()} ");

        return this;
    }

    /// <summary>
    /// Set the size of the canvas used to render subtitles
    /// </summary>
    public VideoProcessingSettings CanvasSize(string size)
    {
        _stringBuilder.Append($" -canvas_size {size} ");

        return this;
    }

    /// <summary>
    /// Audio BitStream Filter
    /// </summary>
    public VideoProcessingSettings AudioBSF(AudioBitstreamFilterType bsf)
    {
        _stringBuilder.Append($" -bsf:a {bsf.ToString().ToLowerInvariant()} ");

        return this;
    }

    /// <summary>
    /// Start a new fragment at each video keyframe
    /// </summary>
    public VideoProcessingSettings MovFralgs(MovFlagsType flag)
    {
        _stringBuilder.Append($" -movflags +{flag.ToString().ToLowerInvariant()} ");

        return this;
    }

    /// <summary>
    /// Output file compression level. 0-100
    /// </summary>
    public VideoProcessingSettings CompressionLevel(int lvl)
    {
        _stringBuilder.Append($" -compression_level {lvl} ");

        return this;
    }

    /// <summary>
    /// Set language
    /// </summary>
    public VideoProcessingSettings Language(LanguageType lng)
    {
        _stringBuilder.Append($" language={lng.ToString().ToLowerInvariant()} ");

        return this;
    }

    /// <summary>
    /// Set physical density of pixels, in dots per inch, unset by default
    /// </summary>
    public VideoProcessingSettings Dpi(int value)
    {
        _stringBuilder.Append($" -dpi {value} ");

        return this;
    }

    /// <summary>
    /// For lossy encoding, this controls image quality. For lossless encoding, this controls the effort and time spent in compression. Range is 0 to 100. Default is 75.
    /// </summary>
    public VideoProcessingSettings Quality(int value)
    {
        //-q:v
        _stringBuilder.Append($" -quality {value} ");

        return this;
    }

    /// <summary>
    /// Enables/Disables use of lossless mode. Default is 0.
    /// </summary>
    public VideoProcessingSettings Lossless(bool value)
    {
        _stringBuilder.Append($" -lossless {value} ");

        return this;
    }

    /// <summary>
    /// This option enables or disables accurate seeking in input files with the -ss option. It is enabled by default, so seeking is accurate when transcoding.
    /// Use -noaccurate_seek to disable it, which may be useful e.g. when copying some streams and transcoding the others
    /// </summary>
    public VideoProcessingSettings AccurateSeek()
    {
        _stringBuilder.Append(" -accurate_seek ");

        return this;
    }

    /// <summary>
    /// Allows discarding specific streams or frames from streams. Any input stream can be fully discarded,
    /// using value all whereas selective discarding of frames from a stream occurs at the demuxer and is not supported by all demuxers.
    /// </summary>
    public VideoProcessingSettings Discard()
    {
        _stringBuilder.Append(" -discard ");

        return this;
    }

    /// <summary>
    /// Limit input read speed.
    ///Its value is a floating-point positive number which represents the maximum duration of media, in seconds, that should be ingested in one second of wallclock time.
    /// Default value is zero and represents no imposed limitation on speed of ingestion. Value 1 represents real-time speed and is equivalent to -re.
    /// Mainly used to simulate a capture device or live input stream (e.g. when reading from a file).
    /// Should not be used with a low value when input is an actual capture device or live stream as it may cause packet loss.
    /// It is useful for when flow speed of output packets is important, such as live streaming.
    /// </summary>
    public VideoProcessingSettings Readrate(string speed)
    {
        _stringBuilder.Append($" -readrate {speed} ");

        return this;
    }

    /// <summary>
    /// Limit input read speed.
    ///Its value is a floating-point positive number which represents the maximum duration of media, in seconds, that should be ingested in one second of wallclock time.
    /// Default value is zero and represents no imposed limitation on speed of ingestion. Value 1 represents real-time speed and is equivalent to -re.
    /// Mainly used to simulate a capture device or live input stream (e.g. when reading from a file).
    /// Should not be used with a low value when input is an actual capture device or live stream as it may cause packet loss.
    /// It is useful for when flow speed of output packets is important, such as live streaming.
    /// </summary>
    public VideoProcessingSettings Readrate(ReadRateType speed)
    {
        _stringBuilder.Append($" -readrate {(int)speed}");

        return this;
    }

    /// <summary>
    /// Set the number of times to loop the output. Use -1 for no loop, 0 for looping indefinitely (default)
    /// </summary>
    public VideoProcessingSettings Loop(int loop)
    {
        _stringBuilder.Append($" -loop {loop} ");

        return this;
    }

    /// <summary>
    /// Set thread safe
    /// </summary>
    public VideoProcessingSettings Safe(int safe)
    {
        _stringBuilder.Append($" -safe {safe} ");

        return this;
    }

    /// <summary>
    /// Read input at native frame rate. This is equivalent to setting -readrate 1
    /// </summary>
    public VideoProcessingSettings NativeFrameRateInput()
    {
        _stringBuilder.Append(" -re ");

        return this;
    }

    /// <summary>
    /// Dump each input packet to stderr.
    /// </summary>
    public VideoProcessingSettings Dump()
    {
        _stringBuilder.Append(" -dump ");

        return this;
    }

    /// <summary>
    /// When dumping packets, also dump the payload.
    /// </summary>
    public VideoProcessingSettings Hex()
    {
        _stringBuilder.Append(" -hex ");

        return this;
    }

    /// <summary>
    /// Set frame rate
    /// As an input option, ignore any timestamps stored in the file and instead generate timestamps assuming constant frame rate fps.
    /// This is not the same as the -framerate option used for some input formats like image2 or v4l2 (it used to be the same in older versions of FFmpeg).
    /// If in doubt use -framerate instead of the input option -r.
    /// </summary>
    /// <param name="fps">Frame per second</param>
    /// <param name="stream">stream_specifier</param>
    public VideoProcessingSettings Rate(int fps, string? stream = null)
    {
        _stringBuilder.Append($" -r{stream} {fps}");

        return this;
    }

    /// <summary>
    /// Do not overwrite output files, and exit immediately if a specified output file already exists
    /// </summary>
    public VideoProcessingSettings GetOutputIfExist()
    {
        _stringBuilder.Append(" -n ");

        return this;
    }

    /// <summary>
    /// Assign a new stream-id value to an output stream. This option should be specified prior to the output filename to which it applies.
    /// For the situation where multiple output files exist, a streamid may be reassigned to a different value
    /// </summary>
    public VideoProcessingSettings StreamId(string value)
    {
        _stringBuilder.Append($" -streamid {value}");

        return this;
    }

    /// <summary>
    /// Set number of times input stream shall be looped. Loop 0 means no loop, loop -1 means infinite loop
    /// </summary>
    public VideoProcessingSettings StreamLoop(int value)
    {
        _stringBuilder.Append($" -stream_loop {value}");

        return this;
    }

    /// <summary>
    /// Set max memory used for buffering kb
    /// </summary>
    public VideoProcessingSettings BufSize(int size)
    {
        _stringBuilder.Append($" -bufsize {size}k");

        return this;
    }

    /// <summary>
    /// Set max memory used for buffering real-time frames
    /// </summary>
    public VideoProcessingSettings Rtbufsize(int size)
    {
        _stringBuilder.Append($" -rtbufsize {size}k");

        return this;
    }

    /// <summary>
    /// KMS CRTC ID to define the capture source. The first active plane on the given CRTC will be used.
    /// </summary>
    public VideoProcessingSettings CRTC(int id)
    {
        _stringBuilder.Append($" -crtc_id {id}");

        return this;
    }

    /// <summary>
    /// Set maximum size of buffer for incoming data, in frames. For DV, this is an exact value. For HDV, it is not frame exact, since HDV does not have a fixed frame size.
    /// </summary>
    public VideoProcessingSettings IncomingDataBuffer(int value)
    {
        _stringBuilder.Append($" -dvbuffer {value}");

        return this;
    }

    /// <summary>
    /// Override autodetection of DV/HDV.
    /// This should only be used if auto detection does not work, or if usage of a different device type should be prohibited.
    /// Treating a DV device as HDV (or vice versa) will not work and result in undefined behavior. The values auto, dv and hdv are supported.
    /// </summary>
    public VideoProcessingSettings DvDevice(DvType type)
    {
        _stringBuilder.Append($" -dvtype {type.ToString().ToLowerInvariant()}");

        return this;
    }

    /// <summary>
    /// Select the capture device by specifying its GUID. Capturing will only be performed from the specified device and fails if no device with the given GUID is found.
    /// This is useful to select the input if multiple devices are connected at the same time.
    /// Look at /sys/bus/firewire/devices to find out the GUIDs.
    /// </summary>
    public VideoProcessingSettings CaptureDeviceGuid(string guid)
    {
        _stringBuilder.Append($" -dvguid {guid}");

        return this;
    }

    /// <summary>
    /// The -max_muxing_queue_size option in FFmpeg is used to specify the maximum number of packets that can be buffered while muxing (combining audio and video streams into a container format).
    /// If the muxing queue overflows, the conversion process might fail, and you may see an error message like "Too many packets buffered for output stream" in the console output.
    /// The default value for -max_muxing_queue_size is 1000. In some cases,
    /// especially when working with high-resolution video or high frame rate video, you might need to increase this value to prevent the muxing queue from overflowing.
    /// </summary>
    public VideoProcessingSettings MaxMuxingQueueSize(int size)
    {
        _stringBuilder.Append($" -max_muxing_queue_size {size}");

        return this;
    }

    /// <summary>
    /// Setting Output Arguments
    /// </summary>
    public VideoProcessingSettings SetOutputArguments(string? arg)
    {
        OutputFileArguments = arg;

        return this;
    }

    /// <summary>
    /// Additional settings that are not currently provided in the wrapper
    /// </summary>
    public VideoProcessingSettings CustomArguments(string arg)
    {
        _stringBuilder.Append(arg);

        return this;
    }

    /// <summary>
    /// Redirect receipt input to stdin
    /// </summary>
    private string StandartInputRedirectArgument => " -i - ";

    /// <summary>
    /// Set input files
    /// </summary>
    public VideoProcessingSettings SetInputFiles(params MediaFile[]? files)
    {
        // Check if the input files are specified
        if(files is null || files.Length == 0)
            throw new NullReferenceException("'CustomInputs' Arguments must be specified if there are no input files");

        // If the number of input files is 0, throw an exception
        switch(files.Length)
        {
            case 0:
                throw new Exception("No input files");

            // If there is only one input file
            case 1:
                // Check the type of input file (Path, Template or NamedPipe)
                // and append the file path to the string builder
                _stringBuilder.Append(files[0].InputType is MediaFileInputType.Path or MediaFileInputType.NamedPipe ? " -i " + files[0].InputFilePath!
                                          : StandartInputRedirectArgument);
                SetInputStreams(files);

                return this;
        }

        // If there is only one stream type among the input files
        if(files.Count(x => x.InputType == MediaFileInputType.Stream) <= 1)
        {
            // Aggregate the input file paths (or the standard input redirect argument) into a single string
            // and append it to the string builder
            _stringBuilder.Append(files.Aggregate(string.Empty,
                                                  (current, file) =>
                                                      current
                                                    + " "
                                                    + (file.InputType is MediaFileInputType.Path or MediaFileInputType.NamedPipe ? " -i " + file.InputFilePath!
                                                          : StandartInputRedirectArgument)));

            // Set input streams for the files
            SetInputStreams(files);

            return this;
        }

        // If there are multiple stream types among the input files
        _stringBuilder.Append(files.Aggregate(string.Empty,
                                              (current, file) =>
                                                  current
                                                + " "
                                                + (file.InputType is MediaFileInputType.Path or MediaFileInputType.NamedPipe ? " -i " + file.InputFilePath!
                                                      : SetPipeChannel(Guid.NewGuid().ToString(), file))));

        // Set input streams for the files
        SetInputStreams(files);

        return this;
    }

    /// <summary>
    /// Summary arguments to process
    /// </summary>
    public override string GetProcessArguments(bool setOutputArguments = true)
    {
        if(setOutputArguments)
            return _stringBuilder + GetOutputArguments();

        return _stringBuilder.ToString();
    }

    /// <summary>
    /// Get streams to transfer to a process
    /// </summary>
    public override Stream[]? GetInputStreams()
    {
        return InputStreams?.ToArray();
    }

    /// <summary>
    /// Pipe names for input streams
    /// </summary>
    public override string[]? GetInputPipeNames()
    {
        return PipeNames?.Keys.ToArray();
    }

    /// <summary>
    /// If the file is transmitted through a stream then assign a channel name to that stream
    /// </summary>
    private string SetPipeChannel(string pipeName, MediaFile file)
    {
        PipeNames ??= new Dictionary<string, Stream>();
        PipeNames.Add(pipeName, file.InputFileStream!);

        return $@"-i {pipeName.ToPipeDir()}";
    }

    /// <summary>
    /// Set input streams from files
    /// If the input files are streams
    /// </summary>
    private void SetInputStreams(params MediaFile[] files)
    {
        if(files.Length is 0)
            return;


        if(files.Count(x => x.InputType == MediaFileInputType.Stream) == 1)
        {
            InputStreams ??= new List<Stream>();
            InputStreams.Add(files.First(x => x.InputType == MediaFileInputType.Stream).InputFileStream!);
        }

        if (!(PipeNames?.Count > 0))
            return;

        InputStreams ??= new List<Stream>();
        InputStreams.AddRange(PipeNames.Select(pipeName => pipeName.Value));
    }

    public void SetInputStreams(params Stream[] streams)
    {
        if(streams.Length is 0)
            return;

        InputStreams ??= new List<Stream>();
        InputStreams.AddRange(streams);
    }

    /// <summary>
    /// Get output arguments
    /// </summary>
    private string GetOutputArguments()
    {
        return OutputFileArguments ?? " - ";
    }
}