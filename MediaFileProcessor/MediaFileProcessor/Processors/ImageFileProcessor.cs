using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Enums.MagickImage;
using MediaFileProcessor.Models.Settings;
namespace MediaFileProcessor.Processors;

public class ImageFileProcessor
{
    private static readonly string _convert = "convert.exe";

    private static async Task<MemoryStream?> ExecuteAsync(ImageProcessingSettings settings, CancellationToken cancellationToken)
    {
        var processArguments = settings.GetProcessArguments();

        var process = new MediaFileProcess(_convert, processArguments, settings, settings.GetInputStreams(), settings.IsStandartOutputRedirect, settings.GetInputPipeNames());

        return await process.ExecuteAsync(cancellationToken);
    }

    //======================================================================================================================================================================

    private static async Task<MemoryStream?> ExecuteCompressImageAsync(MediaFile file, string? outputFile, CancellationToken cancellationToken)
    {
        var settings = new ImageProcessingSettings().SetInputFiles(file)
                                                    .Quality(60)
                                                    .Filter(FilterType.Lanczos)
                                                    .SamplingFactor("4:2:0")
                                                    .Define("jpeg:dct-method=float")
                                                    .Thumbnail("x1080")
                                                    .SetOutputFileArguments(outputFile);

        return await ExecuteAsync(settings, cancellationToken);
    }

    public static async Task CompressImageAsync(MediaFile file, string outputFile, CancellationToken? cancellationToken = null)
    {
        await ExecuteCompressImageAsync(file, outputFile, cancellationToken ?? new CancellationToken());
    }

    public static async Task<MemoryStream> CompressImageAsStreamAsync(MediaFile file, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteCompressImageAsync(file, null, cancellationToken ?? new CancellationToken()))!;
    }

    public static async Task<byte[]> CompressImageAsBytesAsync(MediaFile file, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteCompressImageAsync(file, null, cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    //======================================================================================================================================================================

    private static async Task<MemoryStream?> ExecuteConvertImageAsync(MediaFile file, string? outputFile, ImageFormat? outputFormat, CancellationToken cancellationToken)
    {
        var settings = new ImageProcessingSettings().SetInputFiles(file).OutputFormat(outputFormat).SetOutputFileArguments(outputFile);

        return await ExecuteAsync(settings, cancellationToken);
    }

    public static async Task ConvertImageAsync(MediaFile file, string outputFile, CancellationToken? cancellationToken = null)
    {
        await ExecuteConvertImageAsync(file, outputFile, null, cancellationToken ?? new CancellationToken());
    }

    public static async Task<MemoryStream> ConvertImageAsStreamAsync(MediaFile file, ImageFormat? outputFormat, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConvertImageAsync(file, null, outputFormat, cancellationToken ?? new CancellationToken()))!;
    }

    public static async Task<byte[]> ConvertImageAsBytesAsync(MediaFile file, ImageFormat? outputFormat, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConvertImageAsync(file, null, outputFormat, cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    //======================================================================================================================================================================

    private static async Task<MemoryStream?> ExecuteResizeImageAsync(MediaFile file, string size, ImageFormat? outputFormat, string? outputFile, CancellationToken cancellationToken)
    {
        var settings = new ImageProcessingSettings().Resize(size).Quality(92).SetInputFiles(file).OutputFormat(outputFormat).SetOutputFileArguments(outputFile);

        return await ExecuteAsync(settings, cancellationToken);
    }

    public static async Task ResizeImageAsync(MediaFile file, string size, string outputFile, CancellationToken? cancellationToken = null)
    {
        await ExecuteResizeImageAsync(file, size, null, outputFile, cancellationToken ?? new CancellationToken());
    }

    public static async Task<MemoryStream> ResizeImageAsStreamAsync(MediaFile file, string size, ImageFormat? outputFormat, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteResizeImageAsync(file, size, outputFormat, null, cancellationToken ?? new CancellationToken()))!;
    }

    public static async Task<byte[]> ResizeImageAsBytesAsync(MediaFile file, string size, ImageFormat? outputFormat, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteResizeImageAsync(file, size, outputFormat, null, cancellationToken ?? new CancellationToken()))!.ToArray();
    }
}