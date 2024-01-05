using System.Diagnostics;
using System.Text;
namespace MediaFileProcessor.Models.Settings;

/// <summary>
/// Abstract class that provides basic properties and methods for processing settings
/// </summary>
public abstract class BaseProcessingSettings
{
    /// <summary>
    /// A string builder to store the process arguments
    /// </summary>
    protected readonly StringBuilder _stringBuilder = new ();

    /// <summary>
    /// Property that indicates whether to use the process standard error
    /// </summary>
    public bool RedirectStandardError => ErrorDataReceivedHandler is not null;

    /// <summary>
    /// Property that indicates whether to create a console window for the process
    /// </summary>
    public bool CreateNoWindow { get; set; } = true;

    /// <summary>
    /// Property that indicates whether to use shell execute for the process
    /// </summary>
    public bool UseShellExecute { get; set; }

    /// <summary>
    /// Property that indicates whether to raise events for the process
    /// </summary>
    public bool EnableRaisingEvents { get; set; } = true;

    /// <summary>
    /// Property that sets the window style for the process
    /// </summary>
    public ProcessWindowStyle WindowStyle { get; set; } = ProcessWindowStyle.Hidden;

    /// <summary>
    /// Property that sets the process exit handler
    /// </summary>
    public EventHandler? ProcessOnExitedHandler { get; set; }

    /// <summary>
    /// Property that sets the output data received event handler
    /// </summary>
    public DataReceivedEventHandler? OutputDataReceivedEventHandler { get; set; }

    /// <summary>
    /// Property to hold a reference to the ErrorDataReceivedHandler event handler.
    /// </summary>
    public DataReceivedEventHandler? ErrorDataReceivedHandler { get; set; }

    /// <summary>
    /// Arguments for Specifying the Output Data Format
    /// </summary>
    protected string? OutputFileArguments { get; set; }

    /// <summary>
    /// Property to determine if standard output is redirected or not.
    /// </summary>
    public bool IsStandartOutputRedirect => OutputFileArguments == null || OutputFileArguments.Trim() == "-";

    /// <summary>
    /// Property to hold a dictionary of pipe names and their associated streams.
    /// </summary>
    internal Dictionary<string, Stream>? PipeNames { get; set; }

    /// <summary>
    /// Property to hold a list of input streams.
    /// </summary>
    protected List<Stream>? InputStreams { get; set; }

    /// <summary>
    /// Abstract method to get process arguments.
    /// </summary>
    /// <param name="setOutputArguments">Determines whether output arguments should be set or not. Default is true.</param>
    /// <returns>A string representing the process arguments.</returns>
    public virtual string GetProcessArguments(bool setOutputArguments = true)
    {
        return _stringBuilder.ToString();
    }

    /// <summary>
    /// Abstract method to get input streams.
    /// </summary>
    /// <returns>An array of input streams.</returns>
    public virtual Stream[]? GetInputStreams()
    {
        return InputStreams?.ToArray();
    }

    /// <summary>
    /// Abstract method to get input pipe names.
    /// </summary>
    /// <returns>An array of input pipe names.</returns>
    public virtual string[]? GetInputPipeNames()
    {
        return PipeNames?.Keys.ToArray();
    }
}