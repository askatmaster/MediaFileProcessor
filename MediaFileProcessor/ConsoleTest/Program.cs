using System.Diagnostics;
using System.IO.Pipes;
using System.Text;
using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Enums;
using MediaFileProcessor.Models.Settings;
using MediaFileProcessor.Processors;
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("StartStream");
Console.ResetColor();

// var videoProcessor = new VideoFileProcessor();
// var _video1 = @"test.avi";
// var _photo1 =  @"water.png";


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




// var startInfo = new ProcessStartInfo("mkfifo")
// {
//     Arguments = "/tmp/outpipe1"
// };
// Process.Start(startInfo);



// using var fs = File.OpenWrite("/tmp/outpipe1");
// fs.Write("Hello Askhat Bro"u8);



var inputFile = @"test.avi";
var inputFile2 = @"sample.mp3";
var outputFile = @"outputFileStream.avi";

var pipes = new [] { "outpipe1", "outpipe2" };

var streams = new [] { new FileStream(inputFile, FileMode.Open), new FileStream(inputFile2, FileMode.Open) };

try
{
    using var process = new Process
    {
        StartInfo = new ProcessStartInfo
        {
            RedirectStandardInput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            Arguments = $"-y -thread_queue_size 8192 -i outpipe1 -thread_queue_size 8192 -i outpipe2 -c:v copy -c:a copy -f avi {outputFile}",
            FileName = "ffmpeg"
        },
        EnableRaisingEvents = true
    };

    process.Start();

    // var tasks = new Task[2];

    for (var i = 0; i < 2; i++)
    {
        var pipe = pipes[i];
        var fs = File.OpenWrite($"{pipe}");

        try
        {
            Console.WriteLine("CopyToStart" + pipe);
            streams[i].CopyTo(fs);
            Console.WriteLine("CopyToEnd" + pipe);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);

            // ignored
        }
        // var i1 = i;
        // tasks[i] = Task.Run(() =>
        // {
        //     try
        //     {
        //         
        //     }
        //     catch (Exception)
        //     {
        //         Console.WriteLine("ERROR " + pipes[i1]);
        //
        //         // ignored
        //     }
        // });
    }

    // Task.WaitAll(tasks);

    process.WaitForExit();
}
finally
{
    foreach (var fs in streams)
        fs.Dispose();
}






Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Done");
Console.ResetColor();