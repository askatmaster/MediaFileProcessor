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
}