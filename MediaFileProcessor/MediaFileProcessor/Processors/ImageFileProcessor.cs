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
    private static string _convert = "convert";

    public ImageFileProcessor(string convertExePath)
    {
        _convert = convertExePath;
    }

    public ImageFileProcessor() { }

    /// <summary>
    /// The address from which the convert executable can be downloaded.
    /// </summary>
    private static readonly string _zipAddress = "https://imagemagick.org/archive/binaries/ImageMagick-7.1.0-61-portable-Q16-x64.zip";

    /// <summary>
    /// Executes image processing asynchronously.
    /// </summary>
    /// <param name="settings">The settings used for the processing process.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A `MemoryStream` containing the converted PDF file.</returns>
    public async Task<MemoryStream?> ExecuteAsync(ImageProcessingSettings settings, CancellationToken cancellationToken)
    {
        var process = new MediaFileProcess(_convert,
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
    /// <param name="inputFormat">The format of the input image.</param>
    /// <param name="quality">The quality of the output image.</param>
    /// <param name="filterType">The type of filter to be applied to the image.</param>
    /// <param name="thumbnail">The size of the thumbnail to be generated.</param>
    /// <param name="outputFile">The path of the output file.</param>
    /// <param name="outputFormat">The format of the output image.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the operation.</param>
    /// <returns>A memory stream that contains the compressed image.</returns>
    private async Task<MemoryStream?> ExecuteCompressImageAsync(MediaFile file,
                                                                ImageFormat inputFormat,
                                                                int quality,
                                                                FilterType filterType,
                                                                string thumbnail,
                                                                string? outputFile,
                                                                ImageFormat outputFormat,
                                                                CancellationToken cancellationToken)
    {
        var settings = new ImageProcessingSettings().Format(inputFormat)
                                                    .SetInputFiles(file)
                                                    .Quality(quality)
                                                    .Filter(filterType)
                                                    .SamplingFactor("4:2:0")
                                                    .Define("jpeg:dct-method=float")
                                                    .Thumbnail(thumbnail)
                                                    .Format(outputFormat)
                                                    .SetOutputFileArguments(outputFile);

        return await ExecuteAsync(settings, cancellationToken);
    }

    /// <summary>
    /// Compresses the image asynchronously.
    /// </summary>
    /// <param name="file">The image file to be compressed.</param>
    /// <param name="inputFormat">The format of the input image.</param>
    /// <param name="quality">The quality of the output image.</param>
    /// <param name="filterType">The type of filter to be applied to the image.</param>
    /// <param name="thumbnail">The size of the thumbnail to be generated.</param>
    /// <param name="outputFile">The path of the output file.</param>
    /// <param name="outputFormat">The format of the output image.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task CompressImageAsync(MediaFile file,
                                         ImageFormat inputFormat,
                                         int quality,
                                         FilterType filterType,
                                         string thumbnail,
                                         string outputFile,
                                         ImageFormat outputFormat,
                                         CancellationToken? cancellationToken = null)
    {
        await ExecuteCompressImageAsync(file, inputFormat, quality, filterType, thumbnail, outputFile, outputFormat, cancellationToken ?? new CancellationToken());
    }

    /// <summary>
    /// Compresses an image file as a memory stream.
    /// </summary>
    /// <param name="file">The image file to be compressed.</param>
    /// <param name="inputFormat">The format of the input image file.</param>
    /// <param name="quality">The quality level of the output image, with 0 being the lowest and 100 being the highest.</param>
    /// <param name="filterType">The type of filter to be applied during the compression process.</param>
    /// <param name="thumbnail">A string that specifies the size and location of a thumbnail to extract from the input image.</param>
    /// <param name="outputFormat">The format of the output image file.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A memory stream that contains the compressed image data.</returns>
    public async Task<MemoryStream> CompressImageAsStreamAsync(MediaFile file,
                                                               ImageFormat inputFormat,
                                                               int quality,
                                                               FilterType filterType,
                                                               string thumbnail,
                                                               ImageFormat outputFormat,
                                                               CancellationToken? cancellationToken = null)
    {
        return (await ExecuteCompressImageAsync(file, inputFormat, quality, filterType, thumbnail, null, outputFormat, cancellationToken ?? new CancellationToken()))!;
    }

    /// <summary>
    /// Compresses an image to a memory stream as a byte array.
    /// </summary>
    /// <param name="file">The input image file to be compressed.</param>
    /// <param name="inputFormat">The format of the input image file.</param>
    /// <param name="quality">The quality of the compressed image, where quality value varies between 0 and 100.</param>
    /// <param name="filterType">The filter type to be applied to the compressed image.</param>
    /// <param name="thumbnail">The thumbnail to be generated from the compressed image.</param>
    /// <param name="outputFormat">The format of the output compressed image.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the asynchronous operation.</param>
    /// <returns>The compressed image as a byte array in memory stream.</returns>
    public async Task<byte[]> CompressImageAsBytesAsync(MediaFile file,
                                                        ImageFormat inputFormat,
                                                        int quality,
                                                        FilterType filterType,
                                                        string thumbnail,
                                                        ImageFormat outputFormat,
                                                        CancellationToken? cancellationToken = null)
    {
        return (await ExecuteCompressImageAsync(file, inputFormat, quality, filterType, thumbnail, null, outputFormat, cancellationToken ?? new CancellationToken()))!
            .ToArray();
    }

    //======================================================================================================================================================================

    /// <summary>
    /// Executes the conversion of an image file from one format to another.
    /// </summary>
    /// <param name="file">The file to convert.</param>
    /// <param name="inputFormat">The format of the input file.</param>
    /// <param name="outputFile">The path to the output file (optional).</param>
    /// <param name="outputFormat">The format of the output file (optional).</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the operation.</param>
    /// <returns>A memory stream containing the output image file.</returns>
    private async Task<MemoryStream?> ExecuteConvertImageAsync(MediaFile file,
                                                               ImageFormat inputFormat,
                                                               string? outputFile,
                                                               ImageFormat? outputFormat,
                                                               CancellationToken cancellationToken)
    {
        var settings = new ImageProcessingSettings().Format(inputFormat).SetInputFiles(file).Format(outputFormat).SetOutputFileArguments(outputFile);

        return await ExecuteAsync(settings, cancellationToken);
    }

    /// <summary>
    /// Converts an image from one format to another and saves the result to a specified file.
    /// </summary>
    /// <param name="file">The input image file to be converted.</param>
    /// <param name="inputFormat">The format of the input image file.</param>
    /// <param name="outputFile">The file path of the converted image.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    public async Task ConvertImageAsync(MediaFile file, ImageFormat inputFormat, string outputFile, CancellationToken? cancellationToken = null)
    {
        await ExecuteConvertImageAsync(file, inputFormat, outputFile, null, cancellationToken ?? new CancellationToken());
    }

    /// <summary>
    /// Converts an image and returns the result as a memory stream.
    /// </summary>
    /// <param name="file">The media file to convert</param>
    /// <param name="inputFormat">The input format of the image</param>
    /// <param name="outputFormat">The desired output format of the image. If null, the output format will be the same as the input format.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the operation</param>
    /// <returns>A memory stream containing the converted image data</returns>
    public async Task<MemoryStream> ConvertImageAsStreamAsync(MediaFile file,
                                                              ImageFormat inputFormat,
                                                              ImageFormat? outputFormat,
                                                              CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConvertImageAsync(file, inputFormat, null, outputFormat, cancellationToken ?? new CancellationToken()))!;
    }

    /// <summary>
    /// Converts the image to a byte array format.
    /// </summary>
    /// <param name="file">The file to be converted.</param>
    /// <param name="inputFormat">The input format of the file.</param>
    /// <param name="outputFormat">The desired output format of the image. If null, the original format will be kept.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The converted image as a byte array.</returns>
    public async Task<byte[]> ConvertImageAsBytesAsync(MediaFile file, ImageFormat inputFormat, ImageFormat? outputFormat, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConvertImageAsync(file, inputFormat, null, outputFormat, cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    //======================================================================================================================================================================

    /// <summary>
    /// Executes the resize image operation with the specified parameters asynchronously.
    /// </summary>
    /// <param name="file">The input file.</param>
    /// <param name="inputFormat">The input format of the image file.</param>
    /// <param name="size">The size to resize the image to in the format of "widthxheight".</param>
    /// <param name="outputFormat">The desired output format for the image. If null, the output format will be the same as the input format.</param>
    /// <param name="outputFile">The path to the output file. If null, the image will not be saved to a file.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is the memory stream containing the resized image data.</returns>
    private async Task<MemoryStream?> ExecuteResizeImageAsync(MediaFile file,
                                                              ImageFormat inputFormat,
                                                              string size,
                                                              ImageFormat? outputFormat,
                                                              string? outputFile,
                                                              CancellationToken cancellationToken)
    {
        var settings = new ImageProcessingSettings().Resize(size)
                                                    .Quality(92)
                                                    .Format(inputFormat)
                                                    .SetInputFiles(file)
                                                    .Format(outputFormat)
                                                    .SetOutputFileArguments(outputFile);

        return await ExecuteAsync(settings, cancellationToken);
    }

    /// <summary>
    /// Resizes the image asynchronously.
    /// </summary>
    /// <param name="file">The image file to be resized.</param>
    /// <param name="inputFormat">The input format of the image file.</param>
    /// <param name="size">The size to which the image should be resized to in the format of "width x height".</param>
    /// <param name="outputFile">The full path and filename of the output file.</param>
    /// <param name="cancellationToken">The optional cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous resize operation.</returns>
    public async Task ResizeImageAsync(MediaFile file, ImageFormat inputFormat, string size, string outputFile, CancellationToken? cancellationToken = null)
    {
        await ExecuteResizeImageAsync(file, inputFormat, size, null, outputFile, cancellationToken ?? new CancellationToken());
    }

    /// <summary>
    /// Resizes an image and returns the result as a memory stream.
    /// </summary>
    /// <param name="file">The input image file.</param>
    /// <param name="inputFormat">The input image format.</param>
    /// <param name="size">The size to resize the image to, in the format "widthxheight".</param>
    /// <param name="outputFormat">The output image format, if different from the input format. Can be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A memory stream containing the resized image.</returns>
    public async Task<MemoryStream> ResizeImageAsStreamAsync(MediaFile file,
                                                             ImageFormat inputFormat,
                                                             string size,
                                                             ImageFormat? outputFormat,
                                                             CancellationToken? cancellationToken = null)
    {
        return (await ExecuteResizeImageAsync(file, inputFormat, size, outputFormat, null, cancellationToken ?? new CancellationToken()))!;
    }

    /// <summary>
    /// Resizes an image and returns the result as a byte array.
    /// </summary>
    /// <param name="file">The input image file to be resized.</param>
    /// <param name="inputFormat">The format of the input image file.</param>
    /// <param name="size">The target size of the resized image, in the format "widthxheight".</param>
    /// <param name="outputFormat">The format of the output image, if different from the input format. Can be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A byte array representing the resized image.</returns>
    public async Task<byte[]> ResizeImageAsBytesAsync(MediaFile file,
                                                      ImageFormat inputFormat,
                                                      string size,
                                                      ImageFormat? outputFormat,
                                                      CancellationToken? cancellationToken = null)
    {
        return (await ExecuteResizeImageAsync(file, inputFormat, size, outputFormat, null, cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    //======================================================================================================================================================================

    /// <summary>
    /// This method converts a series of images into a single animated gif.
    /// </summary>
    /// <param name="file">The input file(s) to be converted into a gif.</param>
    /// <param name="delay">The delay between each frame in the gif in hundredths of a second.</param>
    /// <param name="inputFormat">The format of the input files.</param>
    /// <param name="outputFile">The file path where the resulting gif will be saved. If set to null, the method returns the gif as a stream.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The resulting gif as a memory stream, or null if outputFile is not null.</returns>
    private async Task<MemoryStream?> ExecuteImagesToGifAsync(MediaFile file, int delay, ImageFormat inputFormat, string? outputFile, CancellationToken cancellationToken)
    {
        var settings = new ImageProcessingSettings().Delay(delay)
                                                    .Format(inputFormat)
                                                    .SetInputFiles(file)
                                                    .Format(FileFormatType.GIF)
                                                    .SetOutputFileArguments(outputFile);

        return await ExecuteAsync(settings, cancellationToken);
    }

    /// <summary>
    /// Converts a set of image files into a single GIF image.
    /// </summary>
    /// <param name="file">The set of image files to be converted into a single GIF image.</param>
    /// <param name="delay">The delay between frames in the output GIF image in milliseconds.</param>
    /// <param name="inputFormat">The format of the input image files.</param>
    /// <param name="outputFile">The file path of the output GIF image. If it is not specified, the result will be returned as a memory stream.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    public async Task ImagesToGifAsync(MediaFile file, int delay, ImageFormat inputFormat, string? outputFile, CancellationToken? cancellationToken = null)
    {
        await ExecuteImagesToGifAsync(file, delay, inputFormat, outputFile, cancellationToken ?? new CancellationToken());
    }

    /// <summary>
    /// Converts multiple images to a GIF image as a MemoryStream.
    /// </summary>
    /// <param name="file">The input MediaFile.</param>
    /// <param name="delay">The delay in milliseconds between each frame of the resulting GIF image.</param>
    /// <param name="inputFormat">The input image format.</param>
    /// <param name="cancellationToken">A CancellationToken to cancel the asynchronous operation.</param>
    /// <returns>A MemoryStream that contains the resulting GIF image.</returns>
    public async Task<MemoryStream> ImagesToGifAsStreamAsync(MediaFile file, int delay, ImageFormat inputFormat, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteImagesToGifAsync(file, delay, inputFormat, null, cancellationToken ?? new CancellationToken()))!;
    }

    /// <summary>
    /// Converts multiple images to an animated GIF.
    /// </summary>
    /// <param name="file">The `MediaFile` object representing the image to convert.</param>
    /// <param name="delay">The delay in milliseconds between each image frame in the GIF animation.</param>
    /// <param name="inputFormat">The input image format.</param>
    /// <param name="cancellationToken">A `CancellationToken` to observe while waiting for the task to complete.</param>
    /// <returns>A `byte[]` containing the animated GIF data.</returns>
    public async Task<byte[]> ImagesToGifAsBytesAsync(MediaFile file, int delay, ImageFormat inputFormat, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteImagesToGifAsync(file, delay, inputFormat, null, cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    /// <summary>
    /// Downloads executable files convert.exe from a remote ZIP archive.
    /// </summary>
    /// <exception cref="Exception">
    /// Thrown when either of the files convert.exe is not found in the ZIP archive.
    /// </exception>
    public static async Task DownloadExecutableFiles()
    {
        var fileName = $"{Guid.NewGuid()}.zip";

        var convertFound = false;

        try
        {
            // Downloads the ZIP archive from the remote location specified by _zipAddress.
            await FileDownloadProcessor.DownloadFile(_zipAddress, fileName);

            // Open an existing zip file for reading
            using(var zip = ZipFileProcessor.Open(fileName, FileAccess.Read))
            {
                // Read the central directory collection
                var dir = zip.ReadCentralDir();

                // Look for the desired file
                foreach (var entry in dir)
                {
                    if (Path.GetFileName(entry.FilenameInZip) == "convert.exe")
                    {
                        zip.ExtractFile(entry, "convert.exe"); // File found, extract it
                        convertFound = true;
                    }
                }

                // Check if both the files were found in the ZIP archive.
                if(!convertFound)
                    throw new Exception("convert.exe not found");
            }
        }
        finally
        {
            // Delete the downloaded ZIP archive after extracting the required files.
            if(File.Exists(fileName))
                File.Delete(fileName);
        }
    }
}