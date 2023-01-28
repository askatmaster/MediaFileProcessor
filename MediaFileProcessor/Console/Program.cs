using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Enums;
using MediaFileProcessor.Processors;
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Start");
Console.ResetColor();

var video1 =  @"C:\mfptest\test.avi";

var videoProcessor = new VideoFileProcessor();

// await videoProcessor.GetFrameFromVideoAsync(TimeSpan.FromMilliseconds(27500),
//                                             new MediaFile(video1, MediaFileInputType.Path),
//                                             @"C:\mfptest\results\result.jpg");

// await using var stream = new FileStream(video1, FileMode.Open);
// var resultStream = await videoProcessor.GetFrameFromVideoAsStreamAsync(TimeSpan.FromMilliseconds(47500), new MediaFile(stream));
// await using (var output = new FileStream(@"C:\mfptest\results\result.jpg", FileMode.Create))
//     resultStream.WriteTo(output);

// var bytes = File.ReadAllBytes(video1);
// var resultBytes = await videoProcessor.GetFrameFromVideoAsBytesAsync(TimeSpan.FromMilliseconds(47500), new MediaFile(bytes), FileFormatType.JPG);
// await using (var output = new FileStream(@"C:\mfptest\results\result.jpg", FileMode.Create))
//     output.Write(resultBytes);


//============================================================================================================================================================================================================================================

// await videoProcessor.CutVideoAsync(TimeSpan.FromMilliseconds(27500),
//                                    TimeSpan.FromMilliseconds(47500),
//                                    new MediaFile(video1, MediaFileInputType.Path),
//                                    @"C:\mfptest\results\result.avi");

// await using var stream = new FileStream(video1, FileMode.Open);
// var resultStream = await videoProcessor.CutVideoAsStreamAsync(TimeSpan.FromMilliseconds(27500),
//                                                              TimeSpan.FromMilliseconds(47500),
//                                                              new MediaFile(stream));
// await using (var output = new FileStream(@"C:\mfptest\results\result.avi", FileMode.Create))
//     resultStream.WriteTo(output);

// var bytes = File.ReadAllBytes(video1);
// var resultBytes = await videoProcessor.CutVideoAsBytesAsync(TimeSpan.FromMilliseconds(27500), TimeSpan.FromMilliseconds(47500), new MediaFile(bytes));
// await using (var output = new FileStream(@"C:\mfptest\results\result.avi", FileMode.Create))
//     output.Write(resultBytes, 0, resultBytes.Length);

//============================================================================================================================================================================================================================================

// await videoProcessor.ConvertVideoToImagesAsync(new MediaFile(video1, MediaFileInputType.Path),
//                                                FileFormatType.JPG,
//                                                @"C:\mfptest\results\result%03d.jpg");

// await using var stream = new FileStream(video1, FileMode.Open);
// var resultStream = await videoProcessor.ConvertVideoToImagesAsStreamAsync(new MediaFile(stream), FileFormatType.JPG);
// var count = 1;
// var data = resultStream.ReadAsDataArray();
// foreach (var bytes in data)
// {
//     await using (var output = new FileStream(@$"C:\mfptest\results\result{count++}.jpg", FileMode.Create))
//         output.Write(bytes, 0, bytes.Length);
// }

// var bytes = File.ReadAllBytes(video1);
// var resultBytes = await videoProcessor.ConvertVideoToImagesAsBytesAsync(new MediaFile(bytes), FileFormatType.JPG);
// var count = 1;
// foreach (var bytesData in resultBytes)
// {
//     await using (var output = new FileStream(@$"C:\mfptest\results\result{count++}.jpg", FileMode.Create))
//         output.Write(bytesData, 0, bytesData.Length);
// }

//============================================================================================================================================================================================================================================

var stream = new MultiStream();


var files = new List<string>();
for (var i = 1; i <= 4390; i++)
    files.Add($@"C:\mfptest\results2\result{i:000}.jpg");


foreach (var file in files)
    stream.AddStream(new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read));

// await videoProcessor.ConvertImagesToVideoAsync(new MediaFile(stream), 24, @"C:\mfptest\results\result.avi", "yuv420p", FileFormatType.AVI);

var resultStream = await videoProcessor.ConvertImagesToVideoAsStreamAsync(new MediaFile(stream), 24, "yuv420p", FileFormatType.AVI);
await using (var output = new FileStream(@"C:\mfptest\results\result.avi", FileMode.Create))
    resultStream.WriteTo(output);



























// var resultStream = await VideoFileProcessor.ConvertImagesToVideoAsStreamAsync(new MediaFile(stream), 5, FileFormatType.MPEG);
// await using (var output = new FileStream(@"tests\result.mpg", FileMode.Create))
//     resultStream.WriteTo(output);


//============================================================================================================================================================================================================================================

// await VideoFileProcessor.ConvertVideoToImagesAsync(new MediaFile(@"tests\test.avi", @"tests\qwe\image%03d.jpg"));

// await using var stream = new FileStream(@"G:\videofiles\sample.avi", FileMode.Open);
// var resultStream = await VideoFileProcessor.ConvertVideoToImagesAsStreamAsync(new MediaFile(stream), FileFormatType.JPG);
// await using (var output = new FileStream(@"tests\result.jpg", FileMode.Create))
//     resultStream.WriteTo(output);

//============================================================================================================================================================================================================================================

// await VideoFileProcessor.GetAudioFromVideoAsync(new MediaFile(@"tests\test.avi", @"tests\exsound.mp3"), FileFormatType.MP3);

// await using var stream = new FileStream(@"tests\test.avi", FileMode.Open);
// var resultStream = await VideoFileProcessor.GetAudioFromVideoAsStreamAsync(new MediaFile(@"tests\test.avi"), FileFormatType.MP3);
// await using (var output = new FileStream(@"tests\exsound.mp3", FileMode.Create))
//     resultStream.WriteTo(output);

// var bytes = File.ReadAllBytes(@"tests\test.avi");
// var resultBytes = await VideoFileProcessor.GetAudioFromVideoAsBytesAsync(new MediaFile(bytes), FileFormatType.MP3);
// await using (var output = new FileStream(@"tests\exsound.mp3", FileMode.Create))
//     output.Write(resultBytes, 0, resultBytes.Length);


// await ImageFileProcessor.CompressImageAsync(new MediaFile(@"G:\MagickImageFile\testImage.jpg", MediaFileInputType.Path), @"G:\MagickImageFile\image.jpg");

// await using var stream = new FileStream(@"G:\MagickImageFile\testImage.jpg", FileMode.Open);
// var resultStream = await ImageFileProcessor.CompressImageAsStreamAsync(new MediaFile(stream));
// await using (var output = new FileStream(@"G:\MagickImageFile\image.jpg", FileMode.Create))
//     resultStream.WriteTo(output);

// var bytes = File.ReadAllBytes(@"G:\MagickImageFile\testImage.jpg");
// var resultBytes = await ImageFileProcessor.CompressImageAsBytesAsync(new MediaFile(bytes));
// await using (var output = new FileStream(@"G:\MagickImageFile\image.jpg", FileMode.Create))
//     output.Write(resultBytes, 0, resultBytes.Length);



































Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Done");
Console.ResetColor();