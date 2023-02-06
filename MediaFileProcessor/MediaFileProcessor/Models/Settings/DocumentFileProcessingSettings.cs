using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Enums;
namespace MediaFileProcessor.Models.Settings;

public class DocumentFileProcessingSettings : ProcessingSettings
{
    /// <summary>
    /// To produce a standalone documen (e.g. a valid HTML file including 'head' and 'body' tags)
    /// </summary>
    public DocumentFileProcessingSettings Standalone()
    {
        _stringBuilder.Append(" -s ");

        return this;
    }

    /// <summary>
    /// The input format can be specified using the -f/--from option
    /// </summary>
    public DocumentFileProcessingSettings From(string format)
    {
        _stringBuilder.Append($" -f {format}");

        return this;
    }

    /// <summary>
    /// The output format using the -t/--to option
    /// </summary>
    public DocumentFileProcessingSettings To(string format)
    {
        _stringBuilder.Append($" -t {format}");

        return this;
    }

    /// <summary>
    /// Specify the user data directory to search for pandoc data files
    /// </summary>
    public DocumentFileProcessingSettings DataDirectory(string directory)
    {
        _stringBuilder.Append($" --data-dir={directory}");

        return this;
    }

    /// <summary>
    /// Specify a set of default option settings
    /// </summary>
    public DocumentFileProcessingSettings DefaultOptionSettings(string file)
    {
        _stringBuilder.Append($" -d {file}");

        return this;
    }

    /// <summary>
    /// Shift heading levels by a positive or negative integer
    /// </summary>
    public DocumentFileProcessingSettings ShiftHeadingLevel(string number)
    {
        _stringBuilder.Append($" --shift-heading-level-by={number}");

        return this;
    }

    /// <summary>
    /// Specify an executable to be used as a filter transforming the pandoc AST after the input is parsed and before the output is written
    /// </summary>
    public DocumentFileProcessingSettings Filter(string program)
    {
        _stringBuilder.Append($" --filter={program}");

        return this;
    }

    /// <summary>
    /// Set the metadata field KEY to the value VAL. A value specified on the command line overrides a value specified in the document using YAML metadata blocks
    /// </summary>
    public DocumentFileProcessingSettings Metadata(string value)
    {
        _stringBuilder.Append($" --metadata={value}");

        return this;
    }

    /// <summary>
    /// Read metadata from the supplied YAML (or JSON) file
    /// </summary>
    public DocumentFileProcessingSettings MetadataFile(string file)
    {
        _stringBuilder.Append($" --metadata-file={file}");

        return this;
    }

    /// <summary>
    /// Preserve tabs instead of converting them to spaces
    /// </summary>
    public DocumentFileProcessingSettings PreserveTabs()
    {
        _stringBuilder.Append(" --preserve-tabs ");

        return this;
    }

    /// <summary>
    /// Parse untranslatable HTML and LaTeX as raw
    /// </summary>
    public DocumentFileProcessingSettings ParseRaw()
    {
        _stringBuilder.Append(" --parse-raw ");

        return this;
    }

    /// <summary>
    /// Normalize the document, including converting it to NFC Unicode normalization form
    /// </summary>
    public DocumentFileProcessingSettings Normalize()
    {
        _stringBuilder.Append(" --normalize ");

        return this;
    }

    /// <summary>
    /// Link to a CSS stylesheet
    /// </summary>
    public DocumentFileProcessingSettings CssUrl(string url)
    {
        _stringBuilder.Append(" --css={url} ");

        return this;
    }

    /// <summary>
    /// Print the default template for FORMAT
    /// </summary>
    public DocumentFileProcessingSettings PrintDefaultTemplate(string format)
    {
        _stringBuilder.Append(" -D {format} ");

        return this;
    }

    /// <summary>
    /// Parse each file individually before combining for multifile documents.
    /// </summary>
    public DocumentFileProcessingSettings FileScope()
    {
        _stringBuilder.Append(" --file-scope ");

        return this;
    }

    /// <summary>
    /// Additional settings that are not currently provided in the wrapper
    /// </summary>
    public DocumentFileProcessingSettings CustomArguments(string arg)
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
    public DocumentFileProcessingSettings SetOutputFileArguments(string? arg)
    {
        OutputFileArguments = arg;

        return this;
    }

    /// <summary>
    /// Set input files
    /// </summary>
    public DocumentFileProcessingSettings SetInputFiles(params MediaFile[]? files)
    {
        if(files is null)
            throw new NullReferenceException("'CustomInputs' Arguments must be specified if there are no input files");

        switch(files.Length)
        {
            case 0:
                throw new Exception("No input files");
            case 1:
                _stringBuilder.Append(files[0]
                                         .InputType is MediaFileInputType.Path or MediaFileInputType.Template or MediaFileInputType.NamedPipe
                                          ? files[0]
                                             .InputFilePath!
                                          : StandartInputRedirectArgument);

                SetInputStreams(files);
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

            SetInputStreams(files);
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
    /// Get output arguments
    /// </summary>
    private string GetOutputArguments()
    {
        return " -o " + (OutputFileArguments ?? " - ");
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
}