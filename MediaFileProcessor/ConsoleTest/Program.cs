using MediaFileProcessor.Processors;
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("StartStream");
Console.ResetColor();


await VideoFileProcessor.DownloadExecutableFiles();


Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Done");
Console.ResetColor();