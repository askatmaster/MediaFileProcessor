using System.Diagnostics;
using System.IO.Pipes;
using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Enums;
using MediaFileProcessor.Processors;
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("StartStream");
Console.ResetColor();

var videoProcessor = new VideoFileProcessor();
var _video1 = @"test.avi";
var _photo1 =  @"water.png";







// await videoProcessor.AddWaterMarkToVideoAsync(new MediaFile(_video1, MediaFileInputType.Path),
//                                               new MediaFile(_photo1, MediaFileInputType.Path),
//                                               PositionType.UpperLeft,
//                                               @"result.avi",
//                                               FileFormatType.AVI);

Console.WriteLine("PipesAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
await using var stream1 = new FileStream(_video1, FileMode.Open);
await using var stream2 = new FileStream(_photo1, FileMode.Open);
var resultStream = await videoProcessor.AddWaterMarkToVideoAsStreamAsync(new MediaFile(stream1),
                                                                         new MediaFile(stream2),
                                                                         PositionType.UpperLeft,
                                                                         FileFormatType.AVI);
await using (var output = new FileStream(@"result.avi", FileMode.Create))
    resultStream.WriteTo(output);

//Block for testing file processing as bytes without specifying physical paths
// var bytesVideo = await File.ReadAllBytesAsync(_video1);
// var bytesPhoto = await File.ReadAllBytesAsync(_photo1);
// var resultBytes = await videoProcessor.AddWaterMarkToVideoAsBytesAsync(new MediaFile(bytesVideo),
//                                                                        new MediaFile(bytesPhoto),
//                                                                        PositionType.UpperLeft,
//                                                                        FileFormatType.AVI);
// await using (var output = new FileStream(@"C:\mfptest\results\result.avi", FileMode.Create))
//     output.Write(resultBytes);




// var pipeName = "pizda";
//
//
// var task1 = Task.Run(() =>
// {
//     using (var client = new NamedPipeClientStream(".", pipeName, PipeDirection.InOut))
//     {
//         Console.WriteLine("Connecting to server...\n");
//         client.Connect();
//         Console.WriteLine("Connected to server.\n");
//         var reader = new StreamReader(client);
//
//
//         Console.ForegroundColor = ConsoleColor.Yellow;
//         Console.WriteLine("Received from server: " + reader.ReadLine());
//         Console.ResetColor();
//     }
// });
//
// var task2 = Task.Run(() =>
// {
//     using (var server = new NamedPipeServerStream(pipeName, PipeDirection.InOut))
//     {
//         Console.WriteLine("Waiting for connection...\n");
//         server.WaitForConnection();
//         Console.WriteLine("Connected to client.\n");
//         var writer = new StreamWriter(server);
//         writer.WriteLine("Hello from server");
//         writer.Flush();
//     }
// });
//
//
// Task.WaitAll(task1, task2);


// var path = @"\\wsl$\Ubuntu-22.04\tmp";
// var path = @"../tmp";
// Console.WriteLine(path);
// try
// {
//     var files = Directory.GetFiles(path);
//
//     foreach (var file in files)
//         Console.WriteLine(file);
// }
// catch (Exception e)
// {
//     Console.WriteLine("The process failed: {0}", e);
// }







// // Input and output file paths
// var inputFile = @"test.avi";
// var outputFile = "output.mpg";
//
// // Named pipe for input
// var inputPipe = @"\\wsl$\Ubuntu\tmp\outpipe1";
//
// // FFmpeg command to convert AVI to MPG using named pipes
// var ffmpegCommand = $"-i pipe:{inputPipe} -c:v mpeg1video -b:v 1000k -c:a mp2 -b:a 128k {outputFile}";
//
// // Start the FFmpeg process
// using (var process = new Process())
// {
//     process.StartInfo.FileName = "ffmpeg.exe";
//     process.StartInfo.Arguments = ffmpegCommand;
//     process.StartInfo.UseShellExecute = false;
//     process.StartInfo.RedirectStandardOutput = true;
//     process.StartInfo.RedirectStandardError = true;
//     process.Start();
//
//     // Write the input data to the input pipe
//     using (var client = new NamedPipeClientStream(".", inputPipe, PipeDirection.Out))
//     {
//         Console.WriteLine($"Connecting to input pipe {inputPipe}...\n");
//         client.Connect();
//         Console.WriteLine("Connected to input pipe.\n");
//
//         using (var input = File.OpenRead(inputFile))
//             input.CopyTo(client);
//         client.Close();
//     }
//
//     process.WaitForExit();
//
//     // Print the output from FFmpeg
//     Console.WriteLine(process.StandardOutput.ReadToEnd());
//     Console.WriteLine(process.StandardError.ReadToEnd());
// }
//
//
//
// Console.WriteLine("AVI to MPG conversion complete.\n");
















Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Done");
Console.ResetColor();