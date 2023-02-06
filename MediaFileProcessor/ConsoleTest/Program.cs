using System.Diagnostics;
using System.IO.Pipes;
using System.Text;
using ConsoleTest;
using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Enums;
using MediaFileProcessor.Models.Settings;
using MediaFileProcessor.Processors;
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("StartStream");
Console.ResetColor();

var processor = new DocumentFileProcessor();
// var _video1 = @"test.avi";
// var _photo1 =  @"water.png";
var _docx =  @"TestDoc.docx";


// await processor.ConvertDocxToPdf(new MediaFile(_docx, MediaFileInputType.Path), "test.pdf");
await processor.ConvertDocxToPdfAsStream(new MediaFile(new FileStream(_docx, FileMode.Open)));

// await videoProcessor.AddWaterMarkToVideoAsync(new MediaFile(_video1, MediaFileInputType.Path),
//                                               new MediaFile(_photo1, MediaFileInputType.Path),
//                                               PositionType.UpperLeft,
//                                               @"result.avi",
//                                               FileFormatType.AVI);

// await using var stream1 = new FileStream(_video1, FileMode.Open);
// await using var stream2 = new FileStream(_photo1, FileMode.Open);
// await videoProcessor.AddWaterMarkToVideoAsync(new MediaFile(stream1), new MediaFile(stream2), PositionType.UpperLeft, @"result.avi", FileFormatType.AVI);


// await videoProcessor.GetFrameFromVideoAsync(TimeSpan.FromMilliseconds(27500),
//                                             new MediaFile(stream1),
//                                             @"result.jpg",
//                                             FileFormatType.JPG);

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


// var path = @"\\wsl$\Ubuntu-20.04\tmp";
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


// using (var pipeStream = new NamedPipeServerStream("/tmp/outpipe1"))
// {
//     Console.WriteLine($"[Server] Pipe Created, the current process ID is {Environment.ProcessId.ToString()}");
//
//     //wait for a connection from another process
//     pipeStream.WaitForConnection();
//     Console.WriteLine("[Server] Pipe connection established");
//
//     using (var sr = new StreamReader(pipeStream))
//     {
//         //wait for message to arrive from the pipe, when message arrive print date/time and the message to the console.
//         while(sr.ReadLine() is { } message)
//             Console.WriteLine($"{DateTime.Now}: {message}");
//     }
// }
// Console.WriteLine("Connection lost");

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






// using var fs = File.OpenWrite("/tmp/outpipe1");
// fs.Write("Hello Askhat Bro"u8);



// var pipes = new [] { "outpipe1", "outpipe2" };


// Process.Start(new ProcessStartInfo("mkfifo")
// {
//     Arguments = $"/net7.0/{pipes[0]}"
// });
//
// Process.Start(new ProcessStartInfo("mkfifo")
// {
//     Arguments = $"/net7.0/{pipes[1]}"
// });

// var pipes = new [] { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() };
//
// var inputFile = @"test.avi";
// var inputFile2 = @"sample.mp3";
// var outputFile = @"outputFileStream.avi";
//
// var streams = new [] { new FileStream(inputFile, FileMode.Open), new FileStream(inputFile2, FileMode.Open)  };
//
// try
// {
//     using var process = new Process
//     {
//         StartInfo = new ProcessStartInfo
//         {
//             RedirectStandardInput = true,
//             RedirectStandardOutput = true,
//             UseShellExecute = false,
//             CreateNoWindow = true,
//             Arguments = $"-y -i - -i {pipes[0]} -c:v copy -c:a copy -f avi -",
//             // Arguments = $"-y -nostdin -thread_queue_size 16384 -i outpipe2 -thread_queue_size 8192 -i outpipe1 -c:v copy -c:a copy -f avi {outputFile} > /dev/null 2>&1",
//             // Arguments = "-y -ss 00:00:27.500 -i outpipe1.avi -frames:v 1 -f image2 result%03d.jpg",
//             FileName = "ffmpeg"
//         },
//         EnableRaisingEvents = true
//     };
//
//     process.Start();
//
//     var inputTask1 = Task.Run(() =>
//     {
//         Console.WriteLine("CopyToStart_" + pipes[0]);
//         var fs = File.OpenWrite($"{pipes[0]}");
//         streams[0].CopyTo(fs);
//         Console.WriteLine("CopyToEnd_" + pipes[0]);
//     });
//
//     // var inputTask2 = Task.Run(() =>
//     // {
//     //     Console.WriteLine("CopyToStart" + pipes[1]);
//     //     var fs = File.OpenWrite($"{pipes[1]}");
//     //     streams[1].CopyTo(fs);
//     //     Console.WriteLine("CopyToEnd" + pipes[1]);
//     // });
//
//     var inputTaskSI = Task.Run(() =>
//     {
//         Console.WriteLine("StandartInputStart_" + pipes[1]);
//         streams[1].CopyTo(process.StandardInput.BaseStream);
//         process.StandardInput.Flush();
//         process.StandardInput.Close();
//         Console.WriteLine("StandartInputEnd_" + pipes[1]);
//     });
//
//     var outputTask = Task.Run(() =>
//     {
//         Console.WriteLine("OUTPUTSTREAM");
//         using (var output = new FileStream(outputFile, FileMode.Create))
//             process.StandardOutput.BaseStream.CopyTo(output);
//     });
//
//     Task.WaitAll(inputTask1, inputTaskSI, outputTask);
//
//     process.WaitForExit();
// }
// finally
// {
//     foreach (var fs in streams)
//         fs.Dispose();
//
//     foreach (var pipeFile in pipes)
//         File.Delete(pipeFile);
// }



Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Done");
Console.ResetColor();