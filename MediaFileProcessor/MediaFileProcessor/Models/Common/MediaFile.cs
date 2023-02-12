using MediaFileProcessor.Extensions;
using MediaFileProcessor.Models.Enums;
namespace MediaFileProcessor.Models.Common;

/// <summary>
/// The `MediaFile` class represents a media file with either file path, named pipe, stream or byte array as input.
/// </summary>
public class MediaFile
{
    /// <summary>
    /// The file path of the media file.
    /// </summary>
    public string? InputFilePath { get;  }

    /// <summary>
    /// The stream of the media file.
    /// </summary>
    public Stream? InputFileStream { get;  }

    /// <summary>
    /// The input type of the media file.
    /// </summary>
    public MediaFileInputType InputType { get; }

    /// <summary>
    /// Initializes a new instance of the `MediaFile` class with file path or template input.
    /// </summary>
    /// <param name="inputArgument">The file path or template of the media file.</param>
    /// <param name="inputType">The input type of the media file.</param>
    public MediaFile(string inputArgument, MediaFileInputType inputType)
    {
        switch(inputType)
        {
            case MediaFileInputType.Path when !File.Exists(inputArgument):
                throw new FileNotFoundException($"File not found: {inputArgument}");
            case MediaFileInputType.Path:
                InputFilePath = $"{inputArgument} ";
                InputType = inputType;

                break;
            case MediaFileInputType.Template:
                InputFilePath = $"{inputArgument} ";
                InputType = inputType;

                break;
            case MediaFileInputType.NamedPipe:
                InputFilePath = $" {inputArgument.ToPipeDir()} ";

                break;
            case MediaFileInputType.Stream:
                throw new ArgumentException("Input is not compatible with Stream or Bytes types");
            default:
                throw new ArgumentOutOfRangeException(nameof(inputType), inputType, null);
        }
    }

    /// <summary>
    /// Initializes a new instance of the `MediaFile` class with stream input.
    /// </summary>
    /// <param name="inputFileStream">The stream of the media file.</param>
    public MediaFile(Stream inputFileStream)
    {
        if (!inputFileStream.CanRead)
            throw new Exception("Stream cannot be read");

        InputFileStream = inputFileStream;
        InputType = MediaFileInputType.Stream;
    }

    /// <summary>
    /// Initializes a new instance of the `MediaFile` class with byte array input.
    /// </summary>
    /// <param name="bytes">The byte array of the media file.</param>
    public MediaFile(byte[] bytes)
    {
        if(bytes.Length is 0)
            throw new ArgumentException("Byte array is empty");

        InputFileStream = new MemoryStream(bytes);
        InputType = MediaFileInputType.Stream;
    }
}