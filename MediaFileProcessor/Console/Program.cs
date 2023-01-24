using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Enums;
using MediaFileProcessor.Processors;
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Start");
Console.ResetColor();


// var resultStream = await VideoFileProcessor.GetFrameFromVideoAsStreamAsync(TimeSpan.FromMilliseconds(47500), @"tests\test.avi");
// await using (var output = new FileStream(@"tests\result.jpg", FileMode.Create))
//     resultStream.WriteTo(output);

// await using var stream = new FileStream(@"tests\test.avi", FileMode.Open);
// var resultStream = await VideoFileProcessor.GetFrameFromVideoAsStreamAsync(TimeSpan.FromMilliseconds(47500), stream);
// await using (var output = new FileStream(@"tests\result.jpg", FileMode.Create))
//     resultStream.WriteTo(output);

// var bytes = File.ReadAllBytes(@"tests\test.avi");
// var resultStream = await VideoFileProcessor.GetFrameFromVideoAsStreamAsync(TimeSpan.FromMilliseconds(47500), bytes);
// await using (var output = new FileStream(@"tests\result.jpg", FileMode.Create))
//     resultStream.WriteTo(output);


//============================================================================================================================================================================================================================================


// var resultBytes = await VideoFileProcessor.GetFrameFromVideoAsBytesAsync(TimeSpan.FromMilliseconds(47500), @"tests\test.avi");
// await using (var output = new FileStream(@"tests\result.jpg", FileMode.Create))
//     output.Write(resultBytes, 0, resultBytes.Length);

// await using var stream = new FileStream(@"tests\test.avi", FileMode.Open);
// var resultBytes = await VideoFileProcessor.GetFrameFromVideoAsBytesAsync(TimeSpan.FromMilliseconds(47500), stream);
// await using (var output = new FileStream(@"tests\result.jpg", FileMode.Create))
//     output.Write(resultBytes, 0, resultBytes.Length);

// var bytes = File.ReadAllBytes(@"tests\test.avi");
// var resultBytes = await VideoFileProcessor.GetFrameFromVideoAsBytesAsync(TimeSpan.FromMilliseconds(47500), bytes);
// await using (var output = new FileStream(@"tests\result.jpg", FileMode.Create))
//     output.Write(resultBytes, 0, resultBytes.Length);


//============================================================================================================================================================================================================================================

// await VideoFileProcessor.CutVideoAsync(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(20), @"tests\test.avi", @"tests\result.avi");

//============================================================================================================================================================================================================================================

// var stream = new MultiStream();
//
//
// var files = new List<string>();
// for (var i = 1; i < 110; i++)
//     files.Add($@"G:\videofiles\images\weapon1\HQW {i:0000}.jpg");
//
//
// foreach (var file in files)
//     stream.AddStream(new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read));

// await VideoFileProcessor.ConvertImagesToVideoAsync(new MediaFile(stream, @"tests\result.mpg"), 5);

// var resultStream = await VideoFileProcessor.ConvertImagesToVideoAsStreamAsync(new MediaFile(stream), 5);
// await using (var output = new FileStream(@"tests\result.mpg", FileMode.Create))
//     resultStream.WriteTo(output);

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

var bytes = File.ReadAllBytes(@"G:\MagickImageFile\testImage.jpg");
var resultBytes = await ImageFileProcessor.CompressImageAsBytesAsync(new MediaFile(bytes));
await using (var output = new FileStream(@"G:\MagickImageFile\image.jpg", FileMode.Create))
    output.Write(resultBytes, 0, resultBytes.Length);



































Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Done");
Console.ResetColor();