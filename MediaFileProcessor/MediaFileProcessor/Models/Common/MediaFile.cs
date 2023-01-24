using MediaFileProcessor.Models.Enums;
namespace MediaFileProcessor.Models.Common;

public class MediaFile
{
    public string? InputFilePath { get;  }

    public Stream? InputFileStream { get;  }

    public MediaFileInputType InputType { get; }

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
                InputFilePath = $" \"{inputArgument}\" ";
                InputType = inputType;

                break;
            case MediaFileInputType.NamedPipe:
                InputFilePath = $" \\\\.\\pipe\\{inputArgument} ";

                break;
            case MediaFileInputType.Stream:
                throw new ArgumentException("Input is not compatible with Stream or Bytes types");
            default:
                throw new ArgumentOutOfRangeException(nameof(inputType), inputType, null);
        }
    }

    public MediaFile(Stream inputFileStream)
    {
        if (!inputFileStream.CanRead)
            throw new Exception("Stream cannot be read");

        InputFileStream = inputFileStream;
        InputType = MediaFileInputType.Stream;
    }

    public MediaFile(byte[] bytes)
    {
        if(bytes.Length is 0)
            throw new ArgumentException("Byte array is empty");

        InputFileStream = new MemoryStream(bytes);
        InputType = MediaFileInputType.Stream;
    }
}