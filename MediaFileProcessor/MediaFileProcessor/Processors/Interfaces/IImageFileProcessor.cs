using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Enums;
using MediaFileProcessor.Models.Settings;
namespace MediaFileProcessor.Processors.Interfaces;

public interface IImageFileProcessor
{
    /// <summary>
    /// Executes image processing asynchronously.
    /// </summary>
    /// <param name="settings">The settings used for the processing process.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A `MemoryStream` containing the converted PDF file.</returns>
    Task<MemoryStream?> ExecuteAsync(ImageBaseProcessingSettings settings, CancellationToken cancellationToken);

    /// <summary>
    /// Compresses the image asynchronously.
    /// </summary>
    /// <param name="file">The image file to be compressed.</param>
    /// <param name="inputFormatType">The format of the input image.</param>
    /// <param name="quality">The quality of the output image.</param>
    /// <param name="filterType">The type of filter to be applied to the image.</param>
    /// <param name="thumbnail">The size of the thumbnail to be generated.</param>
    /// <param name="outputFile">The path of the output file.</param>
    /// <param name="outputFormatType">The format of the output image.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task CompressImageAsync(MediaFile file,
                            ImageFormatType inputFormatType,
                            int quality,
                            FilterType filterType,
                            string thumbnail,
                            string outputFile,
                            ImageFormatType outputFormatType,
                            CancellationToken? cancellationToken = null);

    /// <summary>
    /// Compresses an image file as a memory stream.
    /// </summary>
    /// <param name="file">The image file to be compressed.</param>
    /// <param name="inputFormatType">The format of the input image file.</param>
    /// <param name="quality">The quality level of the output image, with 0 being the lowest and 100 being the highest.</param>
    /// <param name="filterType">The type of filter to be applied during the compression process.</param>
    /// <param name="thumbnail">A string that specifies the size and location of a thumbnail to extract from the input image.</param>
    /// <param name="outputFormatType">The format of the output image file.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A memory stream that contains the compressed image data.</returns>
    Task<MemoryStream> CompressImageAsStreamAsync(MediaFile file,
                                                  ImageFormatType inputFormatType,
                                                  int quality,
                                                  FilterType filterType,
                                                  string thumbnail,
                                                  ImageFormatType outputFormatType,
                                                  CancellationToken? cancellationToken = null);

    /// <summary>
    /// Compresses an image to a memory stream as a byte array.
    /// </summary>
    /// <param name="file">The input image file to be compressed.</param>
    /// <param name="inputFormatType">The format of the input image file.</param>
    /// <param name="quality">The quality of the compressed image, where quality value varies between 0 and 100.</param>
    /// <param name="filterType">The filter type to be applied to the compressed image.</param>
    /// <param name="thumbnail">The thumbnail to be generated from the compressed image.</param>
    /// <param name="outputFormatType">The format of the output compressed image.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the asynchronous operation.</param>
    /// <returns>The compressed image as a byte array in memory stream.</returns>
    Task<byte[]> CompressImageAsBytesAsync(MediaFile file,
                                           ImageFormatType inputFormatType,
                                           int quality,
                                           FilterType filterType,
                                           string thumbnail,
                                           ImageFormatType outputFormatType,
                                           CancellationToken? cancellationToken = null);

    /// <summary>
    /// Converts an image from one format to another and saves the result to a specified file.
    /// </summary>
    /// <param name="file">The input image file to be converted.</param>
    /// <param name="inputFormatType">The format of the input image file.</param>
    /// <param name="outputFile">The file path of the converted image.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    Task ConvertImageAsync(MediaFile file, ImageFormatType inputFormatType, string outputFile, CancellationToken? cancellationToken = null);

    /// <summary>
    /// Converts an image and returns the result as a memory stream.
    /// </summary>
    /// <param name="file">The media file to convert</param>
    /// <param name="inputFormatType">The input format of the image</param>
    /// <param name="outputFormat">The desired output format of the image. If null, the output format will be the same as the input format.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the operation</param>
    /// <returns>A memory stream containing the converted image data</returns>
    Task<MemoryStream> ConvertImageAsStreamAsync(MediaFile file, ImageFormatType inputFormatType, ImageFormatType? outputFormat, CancellationToken? cancellationToken = null);

    /// <summary>
    /// Converts the image to a byte array format.
    /// </summary>
    /// <param name="file">The file to be converted.</param>
    /// <param name="inputFormatType">The input format of the file.</param>
    /// <param name="outputFormat">The desired output format of the image. If null, the original format will be kept.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The converted image as a byte array.</returns>
    Task<byte[]> ConvertImageAsBytesAsync(MediaFile file, ImageFormatType inputFormatType, ImageFormatType? outputFormat, CancellationToken? cancellationToken = null);

    /// <summary>
    /// Resizes the image asynchronously.
    /// </summary>
    /// <param name="file">The image file to be resized.</param>
    /// <param name="inputFormatType">The input format of the image file.</param>
    /// <param name="size">The size to which the image should be resized to in the format of "width x height".</param>
    /// <param name="outputFile">The full path and filename of the output file.</param>
    /// <param name="cancellationToken">The optional cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous resize operation.</returns>
    Task ResizeImageAsync(MediaFile file, ImageFormatType inputFormatType, string size, string outputFile, CancellationToken? cancellationToken = null);

    /// <summary>
    /// Resizes an image and returns the result as a memory stream.
    /// </summary>
    /// <param name="file">The input image file.</param>
    /// <param name="inputFormatType">The input image format.</param>
    /// <param name="size">The size to resize the image to, in the format "widthxheight".</param>
    /// <param name="outputFormat">The output image format, if different from the input format. Can be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A memory stream containing the resized image.</returns>
    Task<MemoryStream> ResizeImageAsStreamAsync(MediaFile file,
                                                ImageFormatType inputFormatType,
                                                string size,
                                                ImageFormatType? outputFormat,
                                                CancellationToken? cancellationToken = null);

    /// <summary>
    /// Resizes an image and returns the result as a byte array.
    /// </summary>
    /// <param name="file">The input image file to be resized.</param>
    /// <param name="inputFormatType">The format of the input image file.</param>
    /// <param name="size">The target size of the resized image, in the format "widthxheight".</param>
    /// <param name="outputFormat">The format of the output image, if different from the input format. Can be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A byte array representing the resized image.</returns>
    Task<byte[]> ResizeImageAsBytesAsync(MediaFile file, ImageFormatType inputFormatType, string size, ImageFormatType? outputFormat, CancellationToken? cancellationToken = null);

    /// <summary>
    /// Converts a set of image files into a single GIF image.
    /// </summary>
    /// <param name="file">The set of image files to be converted into a single GIF image.</param>
    /// <param name="delay">The delay between frames in the output GIF image in milliseconds.</param>
    /// <param name="inputFormatType">The format of the input image files.</param>
    /// <param name="outputFile">The file path of the output GIF image. If it is not specified, the result will be returned as a memory stream.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    Task ImagesToGifAsync(MediaFile file, int delay, ImageFormatType inputFormatType, string? outputFile, CancellationToken? cancellationToken = null);

    /// <summary>
    /// Converts multiple images to a GIF image as a MemoryStream.
    /// </summary>
    /// <param name="file">The input MediaFile.</param>
    /// <param name="delay">The delay in milliseconds between each frame of the resulting GIF image.</param>
    /// <param name="inputFormatType">The input image format.</param>
    /// <param name="cancellationToken">A CancellationToken to cancel the asynchronous operation.</param>
    /// <returns>A MemoryStream that contains the resulting GIF image.</returns>
    Task<MemoryStream> ImagesToGifAsStreamAsync(MediaFile file, int delay, ImageFormatType inputFormatType, CancellationToken? cancellationToken = null);

    /// <summary>
    /// Converts multiple images to an animated GIF.
    /// </summary>
    /// <param name="file">The `MediaFile` object representing the image to convert.</param>
    /// <param name="delay">The delay in milliseconds between each image frame in the GIF animation.</param>
    /// <param name="inputFormatType">The input image format.</param>
    /// <param name="cancellationToken">A `CancellationToken` to observe while waiting for the task to complete.</param>
    /// <returns>A `byte[]` containing the animated GIF data.</returns>
    Task<byte[]> ImagesToGifAsBytesAsync(MediaFile file, int delay, ImageFormatType inputFormatType, CancellationToken? cancellationToken = null);
}