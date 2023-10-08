using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Enums;
using MediaFileProcessor.Models.Settings;
using MediaFileProcessor.Processors.Interfaces;
namespace MediaFileProcessor.Processors;

/// <summary>
/// This class is responsible for processing Document files.
/// </summary>
public class ImageFileProcessor : IImageFileProcessor
{
    /// <summary>
    /// The name of the convert executable.
    /// </summary>
    private readonly string _convert = "convert";

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="convertExePath">The name of the convert executable.</param>
    public ImageFileProcessor(string convertExePath)
    {
        _convert = convertExePath;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    public ImageFileProcessor() { }

    /// <summary>
    /// The address from which the convert executable can be downloaded.
    /// </summary>
    private const string ZipAddress = "https://imagemagick.org/archive/binaries/ImageMagick-7.1.0-61-portable-Q16-x64.zip";

    /// <inheritdoc />
    public async Task<MemoryStream?> ExecuteAsync(ImageBaseProcessingSettings settings, CancellationToken cancellationToken)
    {
        using var process = new MediaFileProcess(_convert,
                                                 settings.GetProcessArguments(),
                                                 settings,
                                                 settings.GetInputStreams(),
                                                 settings.GetInputPipeNames());

        return await process.ExecuteAsync(cancellationToken);
    }

    //======================================================================================================================================================================

    /// <summary>
    /// Executes the compress image asynchronously.
    /// </summary>
    /// <param name="file">The image file to be compressed.</param>
    /// <param name="inputFormatType">The format of the input image.</param>
    /// <param name="quality">The quality of the output image.</param>
    /// <param name="filterType">The type of filter to be applied to the image.</param>
    /// <param name="thumbnail">The size of the thumbnail to be generated.</param>
    /// <param name="outputFile">The path of the output file.</param>
    /// <param name="outputFormatType">The format of the output image.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the operation.</param>
    /// <returns>A memory stream that contains the compressed image.</returns>
    private async Task<MemoryStream?> ExecuteCompressImageAsync(MediaFile file,
                                                                ImageFormatType inputFormatType,
                                                                int quality,
                                                                FilterType filterType,
                                                                string thumbnail,
                                                                string? outputFile,
                                                                ImageFormatType outputFormatType,
                                                                CancellationToken cancellationToken)
    {
        var settings = new ImageBaseProcessingSettings().Format(inputFormatType)
                                                    .SetInputFiles(file)
                                                    .Quality(quality)
                                                    .Filter(filterType)
                                                    .SamplingFactor("4:2:0")
                                                    .Define("jpeg:dct-method=float")
                                                    .Thumbnail(thumbnail)
                                                    .Format(outputFormatType)
                                                    .SetOutputFileArguments(outputFile);

        return await ExecuteAsync(settings, cancellationToken);
    }

    /// <inheritdoc />
    public async Task CompressImageAsync(MediaFile file,
                                         ImageFormatType inputFormatType,
                                         int quality,
                                         FilterType filterType,
                                         string thumbnail,
                                         string outputFile,
                                         ImageFormatType outputFormatType,
                                         CancellationToken? cancellationToken = null)
    {
        await ExecuteCompressImageAsync(file, inputFormatType, quality, filterType, thumbnail, outputFile, outputFormatType, cancellationToken ?? default);
    }

    /// <inheritdoc />
    public async Task<MemoryStream> CompressImageAsStreamAsync(MediaFile file,
                                                               ImageFormatType inputFormatType,
                                                               int quality,
                                                               FilterType filterType,
                                                               string thumbnail,
                                                               ImageFormatType outputFormatType,
                                                               CancellationToken? cancellationToken = null)
    {
        return (await ExecuteCompressImageAsync(file, inputFormatType, quality, filterType, thumbnail, null, outputFormatType, cancellationToken ?? default))!;
    }

    /// <inheritdoc />
    public async Task<byte[]> CompressImageAsBytesAsync(MediaFile file,
                                                        ImageFormatType inputFormatType,
                                                        int quality,
                                                        FilterType filterType,
                                                        string thumbnail,
                                                        ImageFormatType outputFormatType,
                                                        CancellationToken? cancellationToken = null)
    {
        return (await ExecuteCompressImageAsync(file, inputFormatType, quality, filterType, thumbnail, null, outputFormatType, cancellationToken ?? default))!
            .ToArray();
    }

    //======================================================================================================================================================================

    /// <summary>
    /// Executes the conversion of an image file from one format to another.
    /// </summary>
    /// <param name="file">The file to convert.</param>
    /// <param name="inputFormatType">The format of the input file.</param>
    /// <param name="outputFile">The path to the output file (optional).</param>
    /// <param name="outputFormat">The format of the output file (optional).</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the operation.</param>
    /// <returns>A memory stream containing the output image file.</returns>
    private async Task<MemoryStream?> ExecuteConvertImageAsync(MediaFile file,
                                                               ImageFormatType inputFormatType,
                                                               string? outputFile,
                                                               ImageFormatType? outputFormat,
                                                               CancellationToken cancellationToken)
    {
        var settings = new ImageBaseProcessingSettings().Format(inputFormatType).SetInputFiles(file).Format(outputFormat).SetOutputFileArguments(outputFile);

        return await ExecuteAsync(settings, cancellationToken);
    }

    /// <inheritdoc />
    public async Task ConvertImageAsync(MediaFile file, ImageFormatType inputFormatType, string outputFile, CancellationToken? cancellationToken = null)
    {
        await ExecuteConvertImageAsync(file, inputFormatType, outputFile, null, cancellationToken ?? default);
    }

    /// <inheritdoc />
    public async Task<MemoryStream> ConvertImageAsStreamAsync(MediaFile file,
                                                              ImageFormatType inputFormatType,
                                                              ImageFormatType? outputFormat,
                                                              CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConvertImageAsync(file, inputFormatType, null, outputFormat, cancellationToken ?? default))!;
    }

    /// <inheritdoc />
    public async Task<byte[]> ConvertImageAsBytesAsync(MediaFile file, ImageFormatType inputFormatType, ImageFormatType? outputFormat, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConvertImageAsync(file, inputFormatType, null, outputFormat, cancellationToken ?? default))!.ToArray();
    }

    //======================================================================================================================================================================

    /// <summary>
    /// Executes the resize image operation with the specified parameters asynchronously.
    /// </summary>
    /// <param name="file">The input file.</param>
    /// <param name="inputFormatType">The input format of the image file.</param>
    /// <param name="size">The size to resize the image to in the format of "widthxheight".</param>
    /// <param name="outputFormat">The desired output format for the image. If null, the output format will be the same as the input format.</param>
    /// <param name="outputFile">The path to the output file. If null, the image will not be saved to a file.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is the memory stream containing the resized image data.</returns>
    private async Task<MemoryStream?> ExecuteResizeImageAsync(MediaFile file,
                                                              ImageFormatType inputFormatType,
                                                              string size,
                                                              ImageFormatType? outputFormat,
                                                              string? outputFile,
                                                              CancellationToken cancellationToken)
    {
        var settings = new ImageBaseProcessingSettings().Resize(size)
                                                    .Quality(92)
                                                    .Format(inputFormatType)
                                                    .SetInputFiles(file)
                                                    .Format(outputFormat)
                                                    .SetOutputFileArguments(outputFile);

        return await ExecuteAsync(settings, cancellationToken);
    }

    /// <inheritdoc />
    public async Task ResizeImageAsync(MediaFile file, ImageFormatType inputFormatType, string size, string outputFile, CancellationToken? cancellationToken = null)
    {
        await ExecuteResizeImageAsync(file, inputFormatType, size, null, outputFile, cancellationToken ?? default);
    }

    /// <inheritdoc />
    public async Task<MemoryStream> ResizeImageAsStreamAsync(MediaFile file,
                                                             ImageFormatType inputFormatType,
                                                             string size,
                                                             ImageFormatType? outputFormat,
                                                             CancellationToken? cancellationToken = null)
    {
        return (await ExecuteResizeImageAsync(file, inputFormatType, size, outputFormat, null, cancellationToken ?? default))!;
    }

    /// <inheritdoc />
    public async Task<byte[]> ResizeImageAsBytesAsync(MediaFile file,
                                                      ImageFormatType inputFormatType,
                                                      string size,
                                                      ImageFormatType? outputFormat,
                                                      CancellationToken? cancellationToken = null)
    {
        return (await ExecuteResizeImageAsync(file, inputFormatType, size, outputFormat, null, cancellationToken ?? default))!.ToArray();
    }

    //======================================================================================================================================================================

    /// <summary>
    /// This method converts a series of images into a single animated gif.
    /// </summary>
    /// <param name="file">The input file(s) to be converted into a gif.</param>
    /// <param name="delay">The delay between each frame in the gif in hundredths of a second.</param>
    /// <param name="inputFormatType">The format of the input files.</param>
    /// <param name="outputFile">The file path where the resulting gif will be saved. If set to null, the method returns the gif as a stream.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The resulting gif as a memory stream, or null if outputFile is not null.</returns>
    private async Task<MemoryStream?> ExecuteImagesToGifAsync(MediaFile file, int delay, ImageFormatType inputFormatType, string? outputFile, CancellationToken cancellationToken)
    {
        var settings = new ImageBaseProcessingSettings().Delay(delay)
                                                    .Format(inputFormatType)
                                                    .SetInputFiles(file)
                                                    .Format(FileFormatType.GIF)
                                                    .SetOutputFileArguments(outputFile);

        return await ExecuteAsync(settings, cancellationToken);
    }

    /// <inheritdoc />
    public async Task ImagesToGifAsync(MediaFile file, int delay, ImageFormatType inputFormatType, string? outputFile, CancellationToken? cancellationToken = null)
    {
        await ExecuteImagesToGifAsync(file, delay, inputFormatType, outputFile, cancellationToken ?? default);
    }

    /// <inheritdoc />
    public async Task<MemoryStream> ImagesToGifAsStreamAsync(MediaFile file, int delay, ImageFormatType inputFormatType, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteImagesToGifAsync(file, delay, inputFormatType, null, cancellationToken ?? default))!;
    }

    /// <inheritdoc />
    public async Task<byte[]> ImagesToGifAsBytesAsync(MediaFile file, int delay, ImageFormatType inputFormatType, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteImagesToGifAsync(file, delay, inputFormatType, null, cancellationToken ?? default))!.ToArray();
    }

    /// <summary>
    /// Downloads executable files convert.exe from a remote ZIP archive.
    /// </summary>
    /// <exception cref="Exception">
    /// Thrown when either of the files convert.exe is not found in the ZIP archive.
    /// </exception>
    public static async Task DownloadExecutableFilesAsync()
    {
        var fileName = $"{Guid.NewGuid()}.zip";

        var convertFound = false;

        try
        {
            // Downloads the ZIP archive from the remote location specified by _zipAddress.
            await FileDownloadProcessor.DownloadFileAsync(new Uri(ZipAddress), fileName);

            // Open an existing zip file for reading
            using var zip = ZipFileProcessor.Open(fileName, FileAccess.Read);

            // Read the central directory collection
            var dir = zip.ReadCentralDir();

            // Look for the desired file
            foreach (var entry in dir.Where(entry => Path.GetFileName(entry.FilenameInZip) == "convert.exe"))
            {
                zip.ExtractFile(entry, "convert.exe"); // File found, extract it
                convertFound = true;
            }

            // Check if both the files were found in the ZIP archive.
            if(!convertFound)
                throw new FileNotFoundException("convert.exe not found");
        }
        finally
        {
            // Delete the downloaded ZIP archive after extracting the required files.
            if(File.Exists(fileName))
                File.Delete(fileName);
        }
    }
}