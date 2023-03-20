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
    /// File format. If null then the format could not be determined
    /// </summary>
    public FileFormatType? FormatType { get; }

    /// <summary>
    /// The stream of the media file.
    /// </summary>
    public Stream? InputFileStream { get;  }

    /// <summary>
    /// The input type of the media file.
    /// </summary>
    public MediaFileInputType InputType { get; }

    /// <summary>
    /// Initializes a new instance of the `MediaFile` class with file Path or template NamedPipe.
    /// </summary>
    /// <param name="inputArgument">The file path or template of the media file.</param>
    public MediaFile(string inputArgument)
    {
        var fileExtension = inputArgument.GetExtension();


        if(fileExtension != null)
        {
            InputType = MediaFileInputType.Path;
            InputFilePath = $"{inputArgument} ";
            FormatType = inputArgument.GetFileFormatType();
        }
        else
        {
            InputFilePath = $" {inputArgument.ToPipeDir()} ";
            InputType = MediaFileInputType.NamedPipe;
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

        var buffer = new byte[2024];
        var read = inputFileStream.Read(buffer, 0, buffer.Length);
        if(read > 0)
            FormatType = buffer.GetFormat();
        inputFileStream.Seek(0, SeekOrigin.Begin);
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

        FormatType = bytes.GetFormat();
        InputFileStream = new MemoryStream(bytes);
        InputType = MediaFileInputType.Stream;
    }
}