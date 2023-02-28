using ConsoleTest.TestsVideo;
using MediaFileProcessor.Extensions;
using MediaFileProcessor.Models.Enums;
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("StartStream");
Console.ResetColor();

// await TestAVI.TestGetFrameFromVideo();

var filePath = @"tests/TestGetFrameFromVideo/resultStream.JPG";

var fileExtension = Path.GetExtension(filePath);

Console.WriteLine("File extension: " + fileExtension);

if(fileExtension.ExistsInEnum<FileFormatType>())
    Console.WriteLine("good");

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Done");
Console.ResetColor();