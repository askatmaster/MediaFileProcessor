using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Settings;
namespace MediaFileProcessor.Processors;

/// <summary>
/// This class is responsible for converting Document files.
/// </summary>
public class DocumentFileProcessor
{
    /// <summary>
    /// The name of the pandoc executable.
    /// </summary>
    private static string _pandoc = "pandoc";

    public DocumentFileProcessor(string pandocExePath)
    {
        _pandoc = pandocExePath;
    }

    public DocumentFileProcessor() { }

    /// <summary>
    /// The address from which the pandoc executable can be downloaded.
    /// </summary>
    private static readonly string _zipAddress = "https://github.com/jgm/pandoc/releases/download/3.0.1/pandoc-3.0.1-windows-x86_64.zip";

    /// <summary>
    /// Executes the conversion of the document file to PDF asynchronously.
    /// </summary>
    /// <param name="settings">The settings used for the conversion process.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A `MemoryStream` containing the converted PDF file.</returns>
    private async Task<MemoryStream?> ExecuteAsync(DocumentFileProcessingSettings settings, CancellationToken cancellationToken)
    {
        var process = new MediaFileProcess(_pandoc,
                                           settings.GetProcessArguments(),
                                           settings,
                                           settings.GetInputStreams(),
                                           settings.GetInputPipeNames());

        return await process.ExecuteAsync(cancellationToken);
    }

    /// <summary>
    /// Converts a .docx file to a PDF file asynchronously.
    /// </summary>
    /// <param name="file">The .docx file to be converted.</param>
    /// <param name="outputFile">The file name of the converted PDF file. If `null`, the PDF file is not saved to disk.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A `MemoryStream` containing the converted PDF file.</returns>
    private async Task<MemoryStream?> ExecuteConvertDocxToPdf(MediaFile file, string? outputFile, CancellationToken cancellationToken)
    {
        var settings = new DocumentFileProcessingSettings().From("docx").To("pdf").Standalone().SetInputFiles(file).SetOutputFileArguments(outputFile);

        return await ExecuteAsync(settings, cancellationToken);
    }

    /// <summary>
    /// Converts a .docx file to a PDF file and saves it to disk asynchronously.
    /// </summary>
    /// <param name="file">The .docx file to be converted.</param>
    /// <param name="outputFile">The file name of the converted PDF file.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    public async Task ConvertDocxToPdf(MediaFile file, string? outputFile, CancellationToken? cancellationToken = null)
    {
        await ExecuteConvertDocxToPdf(file, outputFile, cancellationToken ?? new CancellationToken());
    }

    /// <summary>
    /// Converts the DOCX file to a PDF file as a stream.
    /// </summary>
    /// <param name="file">The media file to be converted.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The memory stream that contains the converted PDF file.</returns>
    public async Task<MemoryStream> ConvertDocxToPdfAsStream(MediaFile file, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConvertDocxToPdf(file, null, cancellationToken ?? new CancellationToken()))!;
    }

    /// <summary>
    /// Converts the DOCX file to a PDF file as a byte array.
    /// </summary>
    /// <param name="file">The media file to be converted.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The byte array that contains the converted PDF file.</returns>
    public async Task<byte[]> ConvertDocxToPdfAsBytes(MediaFile file, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConvertDocxToPdf(file, null, cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    public static async Task DownloadExecutableFiles()
    {
        var fileName = $"{Guid.NewGuid()}.zip";
        var pandocFound = false;

        try
        {
            await FileDownloadProcessor.DownloadFile(_zipAddress, fileName);

            // Open an existing zip file for reading
            using(var zip = ZipFileProcessor.Open(fileName, FileAccess.Read))
            {
                // Read the central directory collection
                var dir = zip.ReadCentralDir();

                // Look for the desired file
                foreach (var entry in dir)
                {
                    if (Path.GetFileName(entry.FilenameInZip) == "pandoc.exe")
                    {
                        zip.ExtractFile(entry, "pandoc.exe"); // File found, extract it}
                        pandocFound = true;
                    }
                }

                if(!pandocFound)
                    throw new Exception("pandoc.exe not found");
            }
        }
        finally
        {
            if(File.Exists(fileName))
                File.Delete(fileName);
        }
    }
}