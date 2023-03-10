using System.Text;
using ConsoleTest;
using ConsoleTest.TestsVideo;
using MediaFileProcessor.Extensions;
using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Enums;

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("StartStream");
Console.ResetColor();

// var directoryPath = @"C:\Projects\MediaFileProcessor\MediaFileProcessor\ConsoleTest\bin\Debug\net7.0\tests\ConvertVideoToImages\Path";
// var directoryStream = @"C:\Projects\MediaFileProcessor\MediaFileProcessor\ConsoleTest\bin\Debug\net7.0\tests\ConvertVideoToImages\Stream";
// var fileSizesPath = new Dictionary<string, long>();
// var fileSizesStream = new Dictionary<string, long>();
//
// foreach (var filePath in Directory.GetFiles(directoryPath))
// {
//     var fileInfo = new FileInfo(filePath);
//     fileSizesPath[fileInfo.Name] = fileInfo.Length;
// }
//
// foreach (var filePath in Directory.GetFiles(directoryStream))
// {
//     var fileInfo = new FileInfo(filePath);
//     fileSizesStream[fileInfo.Name] = fileInfo.Length;
// }
//
// var pos = 1;
//
// foreach (var pair in fileSizesPath)
// {
//     if(pair.Value != fileSizesStream[pair.Key])
//     {
//         Console.ForegroundColor = ConsoleColor.Red;
//         Console.WriteLine($"{pair.Value} : {fileSizesStream[pair.Key]} : {pair.Key}");
//         Console.ResetColor();
//     }
//     else
//     {
//         Console.WriteLine($"{pair.Value} : {fileSizesStream[pair.Key]}");
//
//     }
// }

// await TestAVI.TestExtractFrameFromVideo();
// await TestAVI.ConvertVideoToImages();
// await TestAVI.TestCutVideo();

//=================================================================================================================

var sample = TestFile.GetPath(FileFormatType.AVI);
var header = sample.ToBytes()[..1024];
Console.WriteLine(header.IsAVI());

//=================================================================================================================


Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Done");
Console.ResetColor();