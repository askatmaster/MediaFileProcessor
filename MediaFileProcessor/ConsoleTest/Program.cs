using ConsoleTest.TestsVideo;
using MediaFileProcessor.Extensions;
using MediaFileProcessor.Models.Enums;
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("StartStream");
Console.ResetColor();

await TestAVI.ConvertVideoToImagesAsync();

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Done");
Console.ResetColor();