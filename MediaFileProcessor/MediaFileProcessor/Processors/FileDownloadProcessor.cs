using System.ComponentModel;
using System.Net;
namespace MediaFileProcessor.Processors;

public class FileDownloadProcessor
{
    private readonly string _url;
    private readonly string _fullPathWhereToSave;
    private bool _result  ;
    private readonly SemaphoreSlim _semaphore = new (0);

    private FileDownloadProcessor(string url, string fullPathWhereToSave)
    {
        if (string.IsNullOrEmpty(url))
            throw new ArgumentNullException(nameof(url));
        if (string.IsNullOrEmpty(fullPathWhereToSave))
            throw new ArgumentNullException(nameof(fullPathWhereToSave));

        _url = url;
        _fullPathWhereToSave = fullPathWhereToSave;
    }

    private async Task<bool> StartDownload(int timeout)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_fullPathWhereToSave) ?? throw new DirectoryNotFoundException(nameof(_fullPathWhereToSave)));

            if (File.Exists(_fullPathWhereToSave))
                File.Delete(_fullPathWhereToSave);

            using (var client = new WebClient())
            {
                var ur = new Uri(_url);

                client.DownloadProgressChanged += WebClientDownloadProgressChanged;
                client.DownloadFileCompleted += WebClientDownloadCompleted;
                Console.WriteLine(@"Downloading file:");
                await client.DownloadFileTaskAsync(ur, _fullPathWhereToSave);
                await _semaphore.WaitAsync(timeout);

                return _result && File.Exists(_fullPathWhereToSave);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Was not able to download file!");
            Console.Write(e);

            return false;
        }
        finally
        {
            _semaphore.Dispose();
        }
    }

    private void WebClientDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
    {
        Console.Write($"\r    -->    {e.ProgressPercentage}%");
    }

    private void WebClientDownloadCompleted(object? sender, AsyncCompletedEventArgs args)
    {
        _result = !args.Cancelled;
        if (!_result)
            Console.Write(args.Error.ToString());
        Console.WriteLine(Environment.NewLine + "Download finished!");
        _semaphore.Release();
    }

    public static async Task<bool> DownloadFile(string url, string fullPathWhereToSave, int timeoutInMilliSec)
    {
        return await new FileDownloadProcessor(url, fullPathWhereToSave).StartDownload(timeoutInMilliSec);
    }
}