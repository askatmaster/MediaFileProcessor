using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Enums;
using MediaFileProcessor.Processors;
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Start");
Console.ResetColor();

var _video1 =  @"C:\mfptest\test.avi";
var _video2 =  @"C:\mfptest\sample-30s.mp4";
var _photo1 =  @"C:\mfptest\water.png";
var _audio1 =  @"C:\mfptest\sample.mp3";

var videoProcessor = new VideoFileProcessor();




















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