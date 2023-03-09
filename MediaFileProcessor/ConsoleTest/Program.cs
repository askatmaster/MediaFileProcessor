﻿using System.Text;
using ConsoleTest;
using ConsoleTest.TestsVideo;
using MediaFileProcessor.Extensions;
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

var sample = TestFile.GetPath(FileFormatType.AVI);
var header = sample.ToBytes()[..20];

// byte[] aspectRatio = Encoding.Unicode.GetBytes("AspectRatio");
// byte[] windowsMediaVideo = Encoding.Unicode.GetBytes("WindowsMediaVideo");
// byte[] wmv3 = Encoding.ASCII.GetBytes("WMV3");
// byte[] deviceConformanceTemplate = Encoding.Unicode.GetBytes("DeviceConformanceTemplate MP @ML");
//
// using (FileStream fs = new FileStream(sample, FileMode.Open, FileAccess.Read))
// {
//     byte[] buffer = new byte[1024];
//     int bytesRead = fs.Read(buffer, 0, buffer.Length);
//
//     while (bytesRead > 0)
//     {
//         int aspectRatioIndex = IndexOf(buffer, aspectRatio);
//         int windowsMediaVideoIndex = IndexOf(buffer, windowsMediaVideo);
//         int wmv3Index = IndexOf(buffer, wmv3);
//         int deviceConformanceTemplateIndex = IndexOf(buffer, deviceConformanceTemplate);
//
//         if (aspectRatioIndex != -1)
//         {
//             Console.WriteLine("AspectRatio found at index {0}", aspectRatioIndex);
//         }
//         if (windowsMediaVideoIndex != -1)
//         {
//             Console.WriteLine("WindowsMediaVideo found at index {0}", windowsMediaVideoIndex);
//         }
//         if (wmv3Index != -1)
//         {
//             Console.WriteLine("WMV3 found at index {0}", wmv3Index);
//         }
//         if (deviceConformanceTemplateIndex != -1)
//         {
//             Console.WriteLine("DeviceConformanceTemplate MP @ML found at index {0}", deviceConformanceTemplateIndex);
//         }
//
//         if (aspectRatioIndex != -1 && windowsMediaVideoIndex != -1 && wmv3Index != -1 && deviceConformanceTemplateIndex != -1)
//         {
//             break;
//         }
//
//         bytesRead = fs.Read(buffer, 0, buffer.Length);
//     }
// }
// int IndexOf(byte[] source, byte[] pattern)
// {
//     for (int i = 0; i <= source.Length - pattern.Length; i++)
//     {
//         bool match = true;
//
//         for (int j = 0; j < pattern.Length; j++)
//         {
//             if (source[i + j] != pattern[j])
//             {
//                 match = false;
//                 break;
//             }
//         }
//
//         if (match)
//         {
//             return i;
//         }
//     }
//
//     return -1;
// }

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Done");
Console.ResetColor();