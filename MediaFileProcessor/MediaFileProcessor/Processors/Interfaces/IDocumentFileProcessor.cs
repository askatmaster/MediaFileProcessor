using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Settings;
namespace MediaFileProcessor.Processors.Interfaces;

public interface IDocumentFileProcessor
{
    Task<MemoryStream?> ExecuteAsync(DocumentFileProcessingSettings settings, CancellationToken cancellationToken);

    Task ConvertDocxToPdf(MediaFile file, string? outputFile, CancellationToken? cancellationToken = null);

    Task<MemoryStream> ConvertDocxToPdfAsStream(MediaFile file, CancellationToken? cancellationToken = null);

    Task<byte[]> ConvertDocxToPdfAsBytes(MediaFile file, CancellationToken? cancellationToken = null);
}