using System.Diagnostics;
using System.Text;
namespace MediaFileProcessor.Models.Settings;

public abstract class ProcessingSettings
{
    protected readonly StringBuilder _stringBuilder = new ();

    /// <summary>
    /// Use process standart error
    /// </summary>
    public bool RedirectStandardError => ErrorDataReceivedHandler is not null;

    public bool CreateNoWindow { get; set; } = true;

    public bool UseShellExecute { get; set; }

    public bool EnableRaisingEvents { get; set; } = true;

    public ProcessWindowStyle WindowStyle { get; set; } = ProcessWindowStyle.Hidden;

    public EventHandler? ProcessOnExitedHandler { get; set; }

    public DataReceivedEventHandler? OutputDataReceivedEventHandler { get; set; }

    public DataReceivedEventHandler? ErrorDataReceivedHandler { get; set; }

    /// <summary>
    /// Arguments for Specifying the Output Data Format
    /// </summary>
    protected string? OutputFileArguments { get; set; }

    public bool IsStandartOutputRedirect => OutputFileArguments == null || OutputFileArguments.Trim() == " - ";

    internal Dictionary<string, Stream>? PipeNames { get; set; }

    protected List<Stream>? InputStreams { get; set; }

    public abstract string GetProcessArguments(bool setOutputArguments = true);

    public abstract Stream[]? GetInputStreams();

    public abstract string[]? GetInputPipeNames();
}