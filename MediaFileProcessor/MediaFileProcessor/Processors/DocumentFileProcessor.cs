using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Settings;
namespace MediaFileProcessor.Processors;

public class DocumentFileProcessor
{
    private readonly string _pandoc = "pandoc.exe";

    private async Task<MemoryStream?> ExecuteAsync(DocumentFileProcessingSettings settings, CancellationToken cancellationToken)
    {
        var process = new MediaFileProcess(_pandoc,
                                           settings.GetProcessArguments(),
                                           settings,
                                           settings.GetInputStreams(),
                                           settings.IsStandartOutputRedirect,
                                           settings.GetInputPipeNames());

        return await process.ExecuteAsync(cancellationToken);
    }

    private async Task<MemoryStream?> ExecuteConvertDocxToPdf(MediaFile file, string? outputFile, CancellationToken cancellationToken)
    {
        var settings = new DocumentFileProcessingSettings().From("docx")
                                                   .To("pdf")
                                                   .Standalone()
                                                   .SetInputFiles(file)
                                                   .SetOutputFileArguments(outputFile);

        return await ExecuteAsync(settings, cancellationToken);
    }

    public async Task ConvertDocxToPdf(MediaFile file, string? outputFile, CancellationToken? cancellationToken = null)
    {
        await ExecuteConvertDocxToPdf(file, outputFile, cancellationToken ?? new CancellationToken());
    }

    public async Task<MemoryStream> ConvertDocxToPdfAsStream(MediaFile file, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConvertDocxToPdf(file, null, cancellationToken ?? new CancellationToken()))!;
    }

    public async Task<byte[]> ConvertDocxToPdfAsBytes(MediaFile file, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConvertDocxToPdf(file, null, cancellationToken ?? new CancellationToken()))!.ToArray();
    }
}