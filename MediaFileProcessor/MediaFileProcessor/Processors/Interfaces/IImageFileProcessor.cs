using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Enums;
using MediaFileProcessor.Models.Settings;
namespace MediaFileProcessor.Processors.Interfaces;

public interface IImageFileProcessor
{
    Task<MemoryStream?> ExecuteAsync(ImageProcessingSettings settings, CancellationToken cancellationToken);

    Task CompressImageAsync(MediaFile file,
                            ImageFormat inputFormat,
                            int quality,
                            FilterType filterType,
                            string thumbnail,
                            string outputFile,
                            ImageFormat outputFormat,
                            CancellationToken? cancellationToken = null);

    Task<MemoryStream> CompressImageAsStreamAsync(MediaFile file,
                                                  ImageFormat inputFormat,
                                                  int quality,
                                                  FilterType filterType,
                                                  string thumbnail,
                                                  ImageFormat outputFormat,
                                                  CancellationToken? cancellationToken = null);

    Task<byte[]> CompressImageAsBytesAsync(MediaFile file,
                                           ImageFormat inputFormat,
                                           int quality,
                                           FilterType filterType,
                                           string thumbnail,
                                           ImageFormat outputFormat,
                                           CancellationToken? cancellationToken = null);

    Task ConvertImageAsync(MediaFile file, ImageFormat inputFormat, string outputFile, CancellationToken? cancellationToken = null);

    Task<MemoryStream> ConvertImageAsStreamAsync(MediaFile file, ImageFormat inputFormat, ImageFormat? outputFormat, CancellationToken? cancellationToken = null);

    Task<byte[]> ConvertImageAsBytesAsync(MediaFile file, ImageFormat inputFormat, ImageFormat? outputFormat, CancellationToken? cancellationToken = null);

    Task ResizeImageAsync(MediaFile file, ImageFormat inputFormat, string size, string outputFile, CancellationToken? cancellationToken = null);

    Task<MemoryStream> ResizeImageAsStreamAsync(MediaFile file,
                                                ImageFormat inputFormat,
                                                string size,
                                                ImageFormat? outputFormat,
                                                CancellationToken? cancellationToken = null);

    Task<byte[]> ResizeImageAsBytesAsync(MediaFile file, ImageFormat inputFormat, string size, ImageFormat? outputFormat, CancellationToken? cancellationToken = null);

    Task ImagesToGifAsync(MediaFile file, int delay, ImageFormat inputFormat, string? outputFile, CancellationToken? cancellationToken = null);

    Task<MemoryStream> ImagesToGifAsStreamAsync(MediaFile file, int delay, ImageFormat inputFormat, CancellationToken? cancellationToken = null);

    Task<byte[]> ImagesToGifAsBytesAsync(MediaFile file, int delay, ImageFormat inputFormat, CancellationToken? cancellationToken = null);
}