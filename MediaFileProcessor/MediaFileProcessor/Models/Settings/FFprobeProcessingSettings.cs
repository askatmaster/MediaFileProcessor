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
}