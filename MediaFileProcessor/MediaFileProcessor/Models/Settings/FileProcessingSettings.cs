using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Enums;
namespace MediaFileProcessor.Models.Settings;

public class FileProcessingSettings : ProcessingSettings
{
    /// <summary>
    /// To produce a standalone documen (e.g. a valid HTML file including 'head' and 'body' tags)
    /// </summary>
    public FileProcessingSettings Standalone()
    {
        _stringBuilder.Append(" -s ");

        return this;
    }

    /// <summary>
    /// The input format can be specified using the -f/--from option
    /// </summary>
    public FileProcessingSettings From(string format)
    {
        _stringBuilder.Append($" -f {format}");

        return this;
    }

    /// <summary>
    /// Set outputFile
    /// </summary>
    public FileProcessingSettings Output(string output)
    {
        _stringBuilder.Append($" -o {output}");

        return this;
    }

    /// <summary>
    /// The output format using the -t/--to option
    /// </summary>
    public FileProcessingSettings To(string format)
    {
        _stringBuilder.Append($" -t {format}");

        return this;
    }

    /// <summary>
    /// Specify the user data directory to search for pandoc data files
    /// </summary>
    public FileProcessingSettings DataDirectory(string directory)
    {
        _stringBuilder.Append($" --data-dir={directory}");

        return this;
    }

    /// <summary>
    /// Specify a set of default option settings
    /// </summary>
    public FileProcessingSettings DefaultOptionSettings(string file)
    {
        _stringBuilder.Append($" -d {file}");

        return this;
    }

    /// <summary>
    /// Shift heading levels by a positive or negative integer
    /// </summary>
    public FileProcessingSettings ShiftHeadingLevel(string number)
    {
        _stringBuilder.Append($" --shift-heading-level-by={number}");

        return this;
    }

    /// <summary>
    /// Specify an executable to be used as a filter transforming the pandoc AST after the input is parsed and before the output is written
    /// </summary>
    public FileProcessingSettings Filter(string program)
    {
        _stringBuilder.Append($" --filter={program}");

        return this;
    }

    /// <summary>
    /// Set the metadata field KEY to the value VAL. A value specified on the command line overrides a value specified in the document using YAML metadata blocks
    /// </summary>
    public FileProcessingSettings Metadata(string value)
    {
        _stringBuilder.Append($" --metadata={value}");

        return this;
    }

    /// <summary>
    /// Read metadata from the supplied YAML (or JSON) file
    /// </summary>
    public FileProcessingSettings MetadataFile(string file)
    {
        _stringBuilder.Append($" --metadata-file={file}");

        return this;
    }

    /// <summary>
    /// Preserve tabs instead of converting them to spaces
    /// </summary>
    public FileProcessingSettings PreserveTabs()
    {
        _stringBuilder.Append(" --preserve-tabs ");

        return this;
    }

    /// <summary>
    /// Parse each file individually before combining for multifile documents.
    /// </summary>
    public FileProcessingSettings FileScope()
    {
        _stringBuilder.Append(" --file-scope ");

        return this;
    }

    /// <summary>
    /// Additional settings that are not currently provided in the wrapper
    /// </summary>
    public FileProcessingSettings CustomArguments(string arg)
    {
        _stringBuilder.Append(arg);

        return this;
    }

    /// <summary>
    /// Redirect receipt input to stdin
    /// </summary>
    private string StandartInputRedirectArgument => " - ";

    /// <summary>
    /// Setting Output Arguments
    /// </summary>
    public FileProcessingSettings SetOutputFileArguments(string? arg)
    {
        OutputFileArguments = arg;

        return this;
    }

    /// <summary>
    /// Set input files
    /// </summary>
    public FileProcessingSettings SetInputFiles(params MediaFile[]? files)
    {
        if(files is null)
            throw new NullReferenceException("'CustomInputs' Arguments must be specified if there are no input files");

        SetInputStreams();

        switch(files.Length)
        {
            case 0:
                throw new Exception("No input files");
            case 1:
                _stringBuilder.Append(files[0]
                                         .InputType is MediaFileInputType.Path
                                          ? files[0]
                                             .InputFilePath!
                                          : StandartInputRedirectArgument);

                return this;
        }

        if(files.Count(x => x.InputType == MediaFileInputType.Stream) <= 1)
        {
            _stringBuilder.Append(files.Aggregate(string.Empty,
                                                  (current, file) =>
                                                      current
                                                    + " "
                                                    + (file.InputType is MediaFileInputType.Path or MediaFileInputType.Template or MediaFileInputType.NamedPipe
                                                          ? file.InputFilePath!
                                                          : StandartInputRedirectArgument)));

            return this;
        }

        _stringBuilder.Append(files.Aggregate(string.Empty,
                                              (current, file) => current
                                                               + " "
                                                               + (file.InputType is MediaFileInputType.Path or MediaFileInputType.Template or MediaFileInputType.NamedPipe
                                                                     ? file.InputFilePath!
                                                                     : SetPipeChannel(Guid.NewGuid()
                                                                                          .ToString(),
                                                                                      file))));

        return this;
    }

    /// <summary>
    /// Summary arguments to process
    /// </summary>
    public override string GetProcessArguments()
    {
        return _stringBuilder + GetOutputArgument();
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
    public string[]? GetInputPipeNames()
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

        return $@"\\.\pipe\{pipeName}";
    }

    /// <summary>
    /// Set input streams from files
    /// If the input files are streams
    /// </summary>
    private void SetInputStreams(params MediaFile[]? files)
    {
        if(files is null)
            return;

        if(files.Count(x => x.InputType == MediaFileInputType.Stream) == 1)
        {
            InputStreams ??= new List<Stream>();
            InputStreams.Add(files.First(x => x.InputType == MediaFileInputType.Stream)
                                  .InputFileStream!);
        }

        if (!(PipeNames?.Count > 0))
            return;

        InputStreams ??= new List<Stream>();
        InputStreams.AddRange(PipeNames.Select(pipeName => pipeName.Value));
    }

    /// <summary>
    /// Get output arguments
    /// </summary>
    private string GetOutputArgument()
    {
        return OutputFileArguments ?? " - ";
    }
}