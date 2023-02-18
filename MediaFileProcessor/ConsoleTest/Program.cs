using System.Diagnostics;
using ConsoleTest;
using MediaFileProcessor.Processors;
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("StartStream");
Console.ResetColor();

var videoProcessor = new VideoFileProcessor();
await VideoProcessorTests.AddAudioToVideoTest(videoProcessor);


Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Done");
Console.ResetColor();