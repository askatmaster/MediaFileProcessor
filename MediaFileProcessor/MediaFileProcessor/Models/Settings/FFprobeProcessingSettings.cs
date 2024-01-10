using MediaFileProcessor.Models.Enums;

namespace MediaFileProcessor.Models.Settings;

/// <summary>
/// Represents the FFprobe processing settings.
/// </summary>
public class FFprobeProcessingSettings : BaseProcessingSettings
{
    /// <summary>
    /// Show license.
    /// </summary>
    public FFprobeProcessingSettings ShowLicense()
    {
        _stringBuilder.Append(" -L ");

        return this;
    }
    
    /// <summary>
    /// Show help. An optional parameter may be specified to print help about a specific item.
    /// If no argument is specified, only basic (non advanced) tool options are shown.
    /// </summary>
    public FFprobeProcessingSettings Help(HelpOptionType? optionType)
    {
        _stringBuilder.Append($" -h {optionType} ");

        return this;
    }
    
    /// <summary>
    /// Show version.
    /// </summary>
    public FFprobeProcessingSettings Version()
    {
        _stringBuilder.Append(" -version ");

        return this;
    }
    
    /// <summary>
    /// Show the build configuration, one option per line.
    /// </summary>
    public FFprobeProcessingSettings Buildconf()
    {
        _stringBuilder.Append(" -buildconf ");

        return this;
    }
    
    /// <summary>
    /// Show available formats (including devices).
    /// </summary>
    public FFprobeProcessingSettings Formats()
    {
        _stringBuilder.Append(" -formats ");

        return this;
    }
    
    /// <summary>
    /// Show available demuxers.
    /// </summary>
    public FFprobeProcessingSettings Demuxers()
    {
        _stringBuilder.Append(" -demuxers ");

        return this;
    }
    
    /// <summary>
    /// Show available muxers.
    /// </summary>
    public FFprobeProcessingSettings Muxers()
    {
        _stringBuilder.Append(" -muxers ");
        return this;
    }

    /// <summary>
    /// Show available devices.
    /// </summary>
    public FFprobeProcessingSettings Devices()
    {
        _stringBuilder.Append(" -devices ");
        return this;
    }

    /// <summary>
    /// Show all codecs known to libavcodec.
    /// Note that the term ’codec’ is used throughout this documentation as a shortcut for what is more correctly called a media bitstream format.
    /// </summary>
    public FFprobeProcessingSettings Codecs()
    {
        _stringBuilder.Append(" -codecs ");
        return this;
    }

    /// <summary>
    /// Show available decoders.
    /// </summary>
    public FFprobeProcessingSettings Decoders()
    {
        _stringBuilder.Append(" -decoders ");
        return this;
    }

    /// <summary>
    /// Show all available encoders.
    /// </summary>
    public FFprobeProcessingSettings Encoders()
    {
        _stringBuilder.Append(" -encoders ");
        return this;
    }

    /// <summary>
    /// Show available bitstream filters.
    /// </summary>
    public FFprobeProcessingSettings BitstreamFilters()
    {
        _stringBuilder.Append(" -bsfs ");
        return this;
    }

    /// <summary>
    /// Show available protocols.
    /// </summary>
    public FFprobeProcessingSettings Protocols()
    {
        _stringBuilder.Append(" -protocols ");
        return this;
    }

    /// <summary>
    /// Show available libavfilter filters.
    /// </summary>
    public FFprobeProcessingSettings Filters()
    {
        _stringBuilder.Append(" -filters ");
        return this;
    }

    /// <summary>
    /// Show available pixel formats.
    /// </summary>
    public FFprobeProcessingSettings PixelFormats()
    {
        _stringBuilder.Append(" -pix_fmts ");
        return this;
    }

    /// <summary>
    /// Show available sample formats.
    /// </summary>
    public FFprobeProcessingSettings SampleFormats()
    {
        _stringBuilder.Append(" -sample_fmts ");
        return this;
    }

    /// <summary>
    /// Show channel names and standard channel layouts.
    /// </summary>
    public FFprobeProcessingSettings Layouts()
    {
        _stringBuilder.Append(" -layouts ");
        return this;
    }

    /// <summary>
    /// Show stream dispositions.
    /// </summary>
    public FFprobeProcessingSettings Dispositions()
    {
        _stringBuilder.Append(" -dispositions ");
        return this;
    }

    /// <summary>
    /// Show recognized color names.
    /// </summary>
    /// <returns></returns>
    public FFprobeProcessingSettings Colors()
    {
        _stringBuilder.Append(" -colors ");
        return this;
    }

    /// <summary>
    /// Sets the log level for FFprobe processing.
    /// </summary>
    /// <param name="level">The log level to be set.</param>
    public FFprobeProcessingSettings LogLevel(string level)
    {
        _stringBuilder.Append($" -loglevel {level} ");
        return this;
    }

    /// <summary>
    /// Sets the log level with a flag and a level.
    /// </summary>
    /// <param name="flag">The log level flag.</param>
    /// <param name="level">The log level.</param>
    public FFprobeProcessingSettings LogLevelWithFlag(string flag, string level)
    {
        _stringBuilder.Append($" -loglevel {flag}+{level} ");
        return this;
    }

    /// <summary>
    /// Adds the repeat flag only to the FFprobe processing settings.
    /// </summary>
    public FFprobeProcessingSettings AddRepeatFlagOnly()
    {
        _stringBuilder.Append(" -loglevel +repeat ");
        return this;
    }

    /// <summary>
    /// Adds the '-report' option to FFprobe processing settings.
    /// </summary>
    public FFprobeProcessingSettings Report()
    {
        _stringBuilder.Append(" -report ");
        return this;
    }

    /// <summary>
    /// Appends the "-hide_banner" flag to the string builder.
    /// </summary>
    public FFprobeProcessingSettings HideBanner()
    {
        _stringBuilder.Append(" -hide_banner ");
        return this;
    }

    /// <summary>
    /// Sets the CPU flags for FFprobe processing settings.
    /// </summary>
    /// <param name="flag">The CPU flag value.</param>
    public FFprobeProcessingSettings Сpuflags(CpuFlags flag)
    {
        _stringBuilder.Append($" -cpuflags {flag.ToString().ToLowerInvariant().Replace("_", string.Empty, StringComparison.InvariantCulture)}");
        return this;
    }
    
    /// <summary>
    /// These options can be set for any container, codec or device.
    /// Generic options are listed under AVFormatContext options for containers/devices and under AVCodecContext options for codecs.
    /// </summary>
    public FFprobeProcessingSettings Generic()
    {
        _stringBuilder.Append(" -generic ");
        return this;
    }
    
    /// <summary>
    /// These options are specific to the given container, device or codec.
    /// Private options are listed under their corresponding containers/devices/codecs.
    /// </summary>
    public FFprobeProcessingSettings Private()
    {
        _stringBuilder.Append(" -private ");
        return this;
    }
    
    /// <summary>
    /// Force displayed width.
    /// </summary>
    public FFprobeProcessingSettings Width()
    {
        _stringBuilder.Append(" -x ");
        return this;
    }
    
    /// <summary>
    /// Force displayed height.
    /// </summary>
    public FFprobeProcessingSettings Height()
    {
        _stringBuilder.Append(" -y ");
        return this;
    }
    
    /// <summary>
    /// Start in fullscreen mode.
    /// </summary>
    public FFprobeProcessingSettings Fullscreen()
    {
        _stringBuilder.Append(" -fs ");
        return this;
    }
    
    /// <summary>
    /// Disable audio.
    /// </summary>
    public FFprobeProcessingSettings DisableAudio()
    {
        _stringBuilder.Append(" -an ");
        return this;
    }
    
    /// <summary>
    /// Disable subtitles.
    /// </summary>
    public FFprobeProcessingSettings DisableSubtitles()
    {
        _stringBuilder.Append(" -sn ");
        return this;
    }
    
    /// <summary>
    /// Seek to pos.
    /// </summary>
    public FFprobeProcessingSettings SeekToPos(string pos)
    {
        _stringBuilder.Append($" -ss {pos} ");
        return this;
    }

    /// <summary>
    /// Play duration seconds of audio/video
    /// </summary>
    public FFprobeProcessingSettings PlayDuration(string duration)
    {
        _stringBuilder.Append($" -t {duration} ");
        return this;
    }

    /// <summary>
    /// Seek by bytes.
    /// </summary>
    public FFprobeProcessingSettings SeekByBytes()
    {
        _stringBuilder.Append(" -bytes ");
        return this;
    }

    /// <summary>
    /// Set custom interval for seeking.
    /// </summary>
    public FFprobeProcessingSettings SetSeekInterval(string interval)
    {
        _stringBuilder.Append($" -seek_interval {interval} ");
        return this;
    }

    /// <summary>
    /// Disable graphical display.
    /// </summary>
    public FFprobeProcessingSettings DisableDisplay()
    {
        _stringBuilder.Append(" -nodisp ");
        return this;
    }
    
    /// <summary>
    /// Borderless window.
    /// </summary>
    public FFprobeProcessingSettings BorderlessWindow()
    {
        _stringBuilder.Append(" -noborder ");
        return this;
    }

    /// <summary>
    /// Window always on top.
    /// </summary>
    public FFprobeProcessingSettings WindowAlwaysOnTop()
    {
        _stringBuilder.Append(" -alwaysontop ");
        return this;
    }

    /// <summary>
    /// Set the startup volume.
    /// </summary>
    public FFprobeProcessingSettings Volume(int value)
    {
        _stringBuilder.Append($" -volume {value} ");
        return this;
    }

    /// <summary>
    /// Force format.
    /// </summary>
    public FFprobeProcessingSettings ForceFormat(string fmt)
    {
        _stringBuilder.Append($" -f {fmt} ");
        return this;
    }

    /// <summary>
    /// Set window title.
    /// </summary>
    public FFprobeProcessingSettings WindowTitle(string title)
    {
        _stringBuilder.Append($" -window_title \"{title}\" ");
        return this;
    }

    /// <summary>
    /// Set the x position for the left of the window.
    /// </summary>
    public FFprobeProcessingSettings WindowLeft(string left)
    {
        _stringBuilder.Append($" -left {left} ");
        return this;
    }

    /// <summary>
    /// Set the y position for the top of the window.
    /// </summary>
    public FFprobeProcessingSettings WindowTop(string top)
    {
        _stringBuilder.Append($" -top {top} ");
        return this;
    }

    /// <summary>
    /// Loops movie playback number times.
    /// </summary>
    public FFprobeProcessingSettings ReplayLoop(string numberOfLoops)
    {
        _stringBuilder.Append($" -loop {numberOfLoops} ");
        return this;
    }
    
    /// <summary>
    /// Set the show mode to use.
    /// </summary>
    public FFprobeProcessingSettings ShowMode(string mode)
    {
        _stringBuilder.Append($" -showmode {mode} ");
        return this;
    }

    /// <summary>
    /// Create the filtergraph specified by filtergraph and use it to filter the video stream.
    /// </summary>
    public FFprobeProcessingSettings VideoFiltergraph(string filtergraph)
    {
        _stringBuilder.Append($" -vf \"{filtergraph}\" ");
        return this;
    }

    /// <summary>
    /// Filtergraph is a description of the filtergraph to apply to the input audio.
    /// </summary>
    public FFprobeProcessingSettings AudioFiltergraph(string filtergraph)
    {
        _stringBuilder.Append($" -af \"{filtergraph}\" ");
        return this;
    }

    /// <summary>
    /// Read input_url.
    /// </summary>
    public FFprobeProcessingSettings InputUrl(string inputUrl)
    {
        _stringBuilder.Append($" -i \"{inputUrl}\" ");
        return this;
    }
    
        /// <summary>
    /// Print several playback statistics.
    /// </summary>
    public FFprobeProcessingSettings ShowStats()
    {
        _stringBuilder.Append(" -stats ");
        return this;
    }

    /// <summary>
    /// Non-spec-compliant optimizations.
    /// </summary>
    public FFprobeProcessingSettings FastEnable()
    {
        _stringBuilder.Append(" -fast ");
        return this;
    }

    /// <summary>
    /// Generate pts.
    /// </summary>
    public FFprobeProcessingSettings GenPts()
    {
        _stringBuilder.Append(" -genpts ");
        return this;
    }

    /// <summary>
    /// Set the master clock to audio, video or external.
    /// </summary>
    public FFprobeProcessingSettings SyncType(string type)
    {
        _stringBuilder.Append($" -sync {type} ");
        return this;
    }

    /// <summary>
    /// Select the desired audio stream using the given stream specifier.
    /// </summary>
    public FFprobeProcessingSettings AudioStreamSpecifier(string specifier)
    {
        _stringBuilder.Append($" -ast {specifier} ");
        return this;
    }

    /// <summary>
    /// Select the desired video stream using the given stream specifier.
    /// </summary>
    public FFprobeProcessingSettings VideoStreamSpecifier(string specifier)
    {
        _stringBuilder.Append($" -vst {specifier} ");
        return this;
    }

    /// <summary>
    /// Select the desired subtitle stream using the given stream specifier.
    /// </summary>
    public FFprobeProcessingSettings SubtitleStreamSpecifier(string specifier)
    {
        _stringBuilder.Append($" -sst {specifier} ");
        return this;
    }

    /// <summary>
    /// Exit when video is done playing.
    /// </summary>
    public FFprobeProcessingSettings AutoExit()
    {
        _stringBuilder.Append(" -autoexit ");
        return this;
    }

    /// <summary>
    /// Exit if any key is pressed.
    /// </summary>
    public FFprobeProcessingSettings ExitOnKeydown()
    {
        _stringBuilder.Append(" -exitonkeydown ");
        return this;
    }
    
        /// <summary>
    /// Exit if any mouse button is pressed.
    /// </summary>
    public FFprobeProcessingSettings ExitOnMousedown()
    {
        _stringBuilder.Append(" -exitonmousedown ");
        return this;
    }

    /// <summary>
    /// Force a specific decoder implementation.
    /// </summary>
    public FFprobeProcessingSettings Codec(string mediaSpecifier, string codecName)
    {
        _stringBuilder.Append($" -codec:{mediaSpecifier} {codecName} ");
        return this;
    }

    /// <summary>
    /// Force a specific audio decoder.
    /// </summary>
    public FFprobeProcessingSettings AudioCodec(string codecName)
    {
        _stringBuilder.Append($" -acodec {codecName} ");
        return this;
    }

    /// <summary>
    /// Force a specific video decoder.
    /// </summary>
    public FFprobeProcessingSettings VideoCodec(string codecName)
    {
        _stringBuilder.Append($" -vcodec {codecName} ");
        return this;
    }

    /// <summary>
    /// Force a specific subtitle decoder.
    /// </summary>
    public FFprobeProcessingSettings SubtitleCodec(string codecName)
    {
        _stringBuilder.Append($" -scodec {codecName} ");
        return this;
    }

    /// <summary>
    /// Automatically rotate the video.
    /// </summary>
    public FFprobeProcessingSettings Autorotate()
    {
        _stringBuilder.Append(" -autorotate ");
        return this;
    }

    /// <summary>
    /// Drop video frames if video is out of sync.
    /// </summary>
    public FFprobeProcessingSettings FrameDrop()
    {
        _stringBuilder.Append(" -framedrop ");
        return this;
    }

    /// <summary>
    /// Do not limit the input buffer size.
    /// </summary>
    public FFprobeProcessingSettings Infbuf()
    {
        _stringBuilder.Append(" -infbuf ");
        return this;
    }

    /// <summary>
    /// Defines how many threads are used to process a filter pipeline.
    /// </summary>
    public FFprobeProcessingSettings FilterThreads(string nbThreads)
    {
        _stringBuilder.Append($" -filter_threads {nbThreads} ");
        return this;
    }

    /// <summary>
    /// Use vulkan renderer.
    /// </summary>
    public FFprobeProcessingSettings EnableVulkan()
    {
        _stringBuilder.Append(" -enable_vulkan ");
        return this;
    }

    /// <summary>
    /// Vulkan configuration.
    /// </summary>
    public FFprobeProcessingSettings VulkanParams(string parameters)
    {
        _stringBuilder.Append($" -vulkan_params {parameters} ");
        return this;
    }

    /// <summary>
    /// Use HW accelerated decoding.
    /// </summary>
    public FFprobeProcessingSettings Hwaccel()
    {
        _stringBuilder.Append(" -hwaccel ");
        return this;
    }
}