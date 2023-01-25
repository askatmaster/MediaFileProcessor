namespace MediaFileProcessor.Processors;

public class FileDownloadProcessor
{
    public static async Task DownloadFile(string url, string fileName)
    {
        using (var client = new HttpClient())
        {
            // Add a user agent header in case the requested URI contains a query.
            client.DefaultRequestHeaders.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

            using (var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            {
                using (var content = response.Content)
                {
                    // Get the total size of the file
                    var totalBytes = content.Headers.ContentLength.GetValueOrDefault();

                    using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        // Get the stream of the content
                        using (var contentStream = await content.ReadAsStreamAsync())
                        {
                            // Read the content stream
                            var buffer = new byte[8192];
                            int bytesRead;
                            long bytesReceived = 0;

                            while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                            {
                                // Write the data to the file
                                await fileStream.WriteAsync(buffer, 0, bytesRead);
                                bytesReceived += bytesRead;

                                // Calculate the download progress in percentages
                                var percentage = (double)bytesReceived / totalBytes * 100;

                                // Round the percentage to the nearest tenth
                                percentage = Math.Round(percentage, 1);

                                // Set the cursor position to the beginning of the line
                                Console.SetCursorPosition(0, Console.CursorTop);

                                // Print the download progress percentage to the console
                                Console.Write(percentage + "%");
                            }
                        }
                    }
                }
            }
        }
    }
}