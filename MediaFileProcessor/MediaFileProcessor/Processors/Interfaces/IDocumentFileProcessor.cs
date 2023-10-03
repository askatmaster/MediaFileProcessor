using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Settings;
namespace MediaFileProcessor.Processors.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IDocumentFileProcessor
{
    /// <summary>
    /// Executes the conversion of the document file to PDF asynchronously.
    /// </summary>
    /// <param name="settings">The settings used for the conversion process.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A `MemoryStream` containing the converted PDF file.</returns>
    Task<MemoryStream?> ExecuteAsync(DocumentFileBaseProcessingSettings settings, CancellationToken cancellationToken);

    /// <summary>
    /// Converts a .docx file to a PDF file and saves it to disk asynchronously.
    /// </summary>
    /// <param name="file">The .docx file to be converted.</param>
    /// <param name="outputFile">The file name of the converted PDF file.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    Task ConvertDocxToPdf(MediaFile file, string? outputFile, CancellationToken? cancellationToken = null);

    /// <summary>
    /// Converts the DOCX file to a PDF file as a stream.
    /// </summary>
    /// <param name="file">The media file to be converted.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The memory stream that contains the converted PDF file.</returns>
    Task<MemoryStream> ConvertDocxToPdfAsStream(MediaFile file, CancellationToken? cancellationToken = null);

    /// <summary>
    /// Converts the DOCX file to a PDF file as a byte array.
    /// </summary>
    /// <param name="file">The media file to be converted.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The byte array that contains the converted PDF file.</returns>
    Task<byte[]> ConvertDocxToPdfAsBytes(MediaFile file, CancellationToken? cancellationToken = null);
}