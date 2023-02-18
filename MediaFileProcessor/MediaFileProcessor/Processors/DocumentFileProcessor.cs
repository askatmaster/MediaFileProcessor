using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Settings;
using MediaFileProcessor.Processors.Interfaces;
namespace MediaFileProcessor.Processors;

/// <summary>
/// This class is responsible for converting Document files.
/// </summary>
public class DocumentFileProcessor : IDocumentFileProcessor
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

    /// <inheritdoc />
    public async Task<MemoryStream?> ExecuteAsync(DocumentFileProcessingSettings settings, CancellationToken cancellationToken)
    {
        using(var process = new MediaFileProcess(_pandoc,
                                                 settings.GetProcessArguments(),
                                                 settings,
                                                 settings.GetInputStreams(),
                                                 settings.GetInputPipeNames()))
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

    /// <inheritdoc />
    public async Task ConvertDocxToPdf(MediaFile file, string? outputFile, CancellationToken? cancellationToken = null)
    {
        await ExecuteConvertDocxToPdf(file, outputFile, cancellationToken ?? new CancellationToken());
    }

    /// <inheritdoc />
    public async Task<MemoryStream> ConvertDocxToPdfAsStream(MediaFile file, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConvertDocxToPdf(file, null, cancellationToken ?? new CancellationToken()))!;
    }

    /// <inheritdoc />
    public async Task<byte[]> ConvertDocxToPdfAsBytes(MediaFile file, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConvertDocxToPdf(file, null, cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    /// <summary>
    /// Downloads executable files pandoc.exe from a remote ZIP archive.
    /// </summary>
    /// <exception cref="Exception">
    /// Thrown when either of the files pandoc.exe is not found in the ZIP archive.
    /// </exception>
    public static async Task DownloadExecutableFiles()
    {
        var fileName = $"{Guid.NewGuid()}.zip";
        var pandocFound = false;

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
            // Delete the downloaded ZIP archive after extracting the required files.
            if(File.Exists(fileName))
                File.Delete(fileName);
        }
    }
}