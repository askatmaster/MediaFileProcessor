using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Enums;
using MediaFileProcessor.Models.Settings;
namespace MediaFileProcessor.Processors;

public class ImageFileProcessor
{
    private readonly string _convert = "convert.exe";

    private async Task<MemoryStream?> ExecuteAsync(ImageProcessingSettings settings, CancellationToken cancellationToken)
    {
        var process = new MediaFileProcess(_convert,
                                           settings.GetProcessArguments(),
                                           settings,
                                           settings.GetInputStreams(),
                                           settings.IsStandartOutputRedirect,
                                           settings.GetInputPipeNames());

        return await process.ExecuteAsync(cancellationToken);
    }

    //======================================================================================================================================================================

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

    private async Task<MemoryStream?> ExecuteConvertImageAsync(MediaFile file,
                                                               ImageFormat inputFormat,
                                                               string? outputFile,
                                                               ImageFormat? outputFormat,
                                                               CancellationToken cancellationToken)
    {
        var settings = new ImageProcessingSettings().Format(inputFormat).SetInputFiles(file).Format(outputFormat).SetOutputFileArguments(outputFile);

        return await ExecuteAsync(settings, cancellationToken);
    }

    public async Task ConvertImageAsync(MediaFile file, ImageFormat inputFormat, string outputFile, CancellationToken? cancellationToken = null)
    {
        await ExecuteConvertImageAsync(file, inputFormat, outputFile, null, cancellationToken ?? new CancellationToken());
    }

    public async Task<MemoryStream> ConvertImageAsStreamAsync(MediaFile file,
                                                              ImageFormat inputFormat,
                                                              ImageFormat? outputFormat,
                                                              CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConvertImageAsync(file, inputFormat, null, outputFormat, cancellationToken ?? new CancellationToken()))!;
    }

    public async Task<byte[]> ConvertImageAsBytesAsync(MediaFile file, ImageFormat inputFormat, ImageFormat? outputFormat, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConvertImageAsync(file, inputFormat, null, outputFormat, cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    //======================================================================================================================================================================

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

    public async Task ResizeImageAsync(MediaFile file, ImageFormat inputFormat, string size, string outputFile, CancellationToken? cancellationToken = null)
    {
        await ExecuteResizeImageAsync(file, inputFormat, size, null, outputFile, cancellationToken ?? new CancellationToken());
    }

    public async Task<MemoryStream> ResizeImageAsStreamAsync(MediaFile file,
                                                             ImageFormat inputFormat,
                                                             string size,
                                                             ImageFormat? outputFormat,
                                                             CancellationToken? cancellationToken = null)
    {
        return (await ExecuteResizeImageAsync(file, inputFormat, size, outputFormat, null, cancellationToken ?? new CancellationToken()))!;
    }

    public async Task<byte[]> ResizeImageAsBytesAsync(MediaFile file,
                                                      ImageFormat inputFormat,
                                                      string size,
                                                      ImageFormat? outputFormat,
                                                      CancellationToken? cancellationToken = null)
    {
        return (await ExecuteResizeImageAsync(file, inputFormat, size, outputFormat, null, cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    //======================================================================================================================================================================

    private async Task<MemoryStream?> ExecuteImagesToGifAsync(MediaFile file, int delay, ImageFormat inputFormat, string? outputFile, CancellationToken cancellationToken)
    {
        var settings = new ImageProcessingSettings().Delay(delay)
                                                    .Format(inputFormat)
                                                    .SetInputFiles(file)
                                                    .Format(FileFormatType.GIF)
                                                    .SetOutputFileArguments(outputFile);

        return await ExecuteAsync(settings, cancellationToken);
    }

    public async Task ImagesToGifAsync(MediaFile file, int delay, ImageFormat inputFormat, string? outputFile, CancellationToken? cancellationToken = null)
    {
        await ExecuteImagesToGifAsync(file, delay, inputFormat, outputFile, cancellationToken ?? new CancellationToken());
    }
    public async Task<MemoryStream> ImagesToGifAsStreamAsync(MediaFile file, int delay, ImageFormat inputFormat, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteImagesToGifAsync(file, delay, inputFormat, null, cancellationToken ?? new CancellationToken()))!;
    }

    public async Task<byte[]> ImagesToGifAsBytesAsync(MediaFile file, int delay, ImageFormat inputFormat, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteImagesToGifAsync(file, delay, inputFormat, null, cancellationToken ?? new CancellationToken()))!.ToArray();
    }

}