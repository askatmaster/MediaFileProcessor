using MediaFileProcessor.Extensions;
using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Enums;
using MediaFileProcessor.Models.Video;
using MediaFileProcessor.Processors;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
namespace ConsoleTest;

/// <summary>
/// Test methods for VideoProcessor
/// </summary>
public class VideoProcessorTests
{
    /// <summary>
    /// Test data
    /// </summary>
    private static readonly  string _video1 = @"C:\mfptest\test.avi";
    private static readonly  string _video12 =  @"C:\mfptest\test2.avi";
    private static readonly  string _video2 =  @"C:\mfptest\sample-30s.mp4";
    private static readonly  string _photo1 =  @"C:\mfptest\water.png";
    private static readonly  string _audio1 =  @"C:\mfptest\sample.mp3";
    private static readonly  string _subsEn =  @"C:\mfptest\subtitle.en.srt";
    private static readonly  string _subsRU =  @"C:\mfptest\subtitle.ru.srt";

    /// <summary>
    /// Settings for json convert
    /// </summary>
    private static readonly JsonSerializerSettings _jsonSnakeCaseSerializerSettings = new()
    {
        ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new SnakeCaseNamingStrategy()
        },
        Formatting = Formatting.Indented
    };

    public static async Task GetFrameFromVideoTest(VideoFileProcessor videoProcessor)
    {
        //Test block with physical paths to input and output files
        await videoProcessor.GetFrameFromVideoAsync(TimeSpan.FromMilliseconds(27500),
                                                    new MediaFile(_video1, MediaFileInputType.Path),
                                                    @"C:\mfptest\results\result.jpg",
                                                    FileFormatType.JPG);

        //Block for testing file processing as streams without specifying physical paths
        await using var stream = new FileStream(_video1, FileMode.Open);
        var resultStream = await videoProcessor.GetFrameFromVideoAsStreamAsync(TimeSpan.FromMilliseconds(47500), new MediaFile(stream), FileFormatType.JPG);
        await using (var output = new FileStream(@"C:\mfptest\results\result.jpg", FileMode.Create))
            resultStream.WriteTo(output);

        //Block for testing file processing as bytes without specifying physical paths
        var bytes = await File.ReadAllBytesAsync(_video1);
        var resultBytes = await videoProcessor.GetFrameFromVideoAsBytesAsync(TimeSpan.FromMilliseconds(47500), new MediaFile(bytes), FileFormatType.JPG);
        await using (var output = new FileStream(@"C:\mfptest\results\result.jpg", FileMode.Create))
            output.Write(resultBytes);
    }

    public static async Task CutVideoTest(VideoFileProcessor videoProcessor)
    {
        //Test block with physical paths to input and output files
        await videoProcessor.CutVideoAsync(TimeSpan.FromMilliseconds(27500),
                                           TimeSpan.FromMilliseconds(47500),
                                           new MediaFile(_video1, MediaFileInputType.Path),
                                           @"C:\mfptest\results\result.avi",
                                           FileFormatType.AVI);

        //Block for testing file processing as streams without specifying physical paths
        await using var stream = new FileStream(_video1, FileMode.Open);
        var resultStream = await videoProcessor.CutVideoAsStreamAsync(TimeSpan.FromMilliseconds(27500),
                                                                      TimeSpan.FromMilliseconds(47500),
                                                                      new MediaFile(stream),
                                                                      FileFormatType.AVI);
        await using (var output = new FileStream(@"C:\mfptest\results\result.avi", FileMode.Create))
            resultStream.WriteTo(output);

        //Block for testing file processing as bytes without specifying physical paths
        var bytes = await File.ReadAllBytesAsync(_video1);
        var resultBytes = await videoProcessor.CutVideoAsBytesAsync(TimeSpan.FromMilliseconds(27500),
                                                                    TimeSpan.FromMilliseconds(47500),
                                                                    new MediaFile(bytes),
                                                                    FileFormatType.AVI);
        await using (var output = new FileStream(@"C:\mfptest\results\result.avi", FileMode.Create))
            output.Write(resultBytes, 0, resultBytes.Length);
    }

    public static async Task ConvertVideoToImagesTest(VideoFileProcessor videoProcessor)
    {
        //Test block with physical paths to input and output files
        await videoProcessor.ConvertVideoToImagesAsync(new MediaFile(_video1, MediaFileInputType.Path),
                                                       FileFormatType.JPG,
                                                       @"C:\mfptest\results\result%03d.jpg");

        //Block for testing file processing as streams without specifying physical paths
        await using var stream = new FileStream(_video1, FileMode.Open);
        var resultStream = await videoProcessor.ConvertVideoToImagesAsStreamAsync(new MediaFile(stream), FileFormatType.JPG);
        var count = 1;
        var data = resultStream.ReadAsDataArray();

        foreach (var bytes in data)
        {
            await using (var output = new FileStream(@$"C:\mfptest\results\result{count++}.jpg", FileMode.Create))
                output.Write(bytes, 0, bytes.Length);
        }

        //Block for testing file processing as bytes without specifying physical paths
        var bytes1 = await File.ReadAllBytesAsync(_video1);
        var resultBytes = await videoProcessor.ConvertVideoToImagesAsBytesAsync(new MediaFile(bytes1), FileFormatType.JPG);
        var count1 = 1;

        foreach (var bytesData in resultBytes)
        {
            await using (var output = new FileStream(@$"C:\mfptest\results\result{count1++}.jpg", FileMode.Create))
                output.Write(bytesData, 0, bytesData.Length);
        }
    }

    public static async Task ConvertImagesToVideoTest(VideoFileProcessor videoProcessor)
    {
        var stream = new MultiStream();
        var files = new List<string>();
        for (var i = 1;
             i <= 4390;
             i++)
            files.Add($@"C:\mfptest\results2\result{i:000}.jpg");
        foreach (var file in files)
            stream.AddStream(new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read));

        //Test block with physical paths to input and output files
        await videoProcessor.ConvertImagesToVideoAsync(new MediaFile(stream), 24, @"C:\mfptest\results\result.avi", "yuv420p", FileFormatType.AVI);

        //Block for testing file processing as streams without specifying physical paths
        var resultStream = await videoProcessor.ConvertImagesToVideoAsStreamAsync(new MediaFile(stream), 24, "yuv420p", FileFormatType.AVI);
        await using (var output = new FileStream(@"C:\mfptest\results\result.avi", FileMode.Create))
            resultStream.WriteTo(output);

        //Block for testing file processing as bytes without specifying physical paths
        var files1 = new List<byte[]>();
        for (var i = 1;
             i <= 4390;
             i++)
            files1.Add(await File.ReadAllBytesAsync($@"C:\mfptest\results2\result{i:000}.jpg"));
        var resultBytes = await videoProcessor.ConvertImagesToVideoAsBytesAsync(new MediaFile(FileDataExtensions.ConcatByteArrays(false, files1.ToArray())),
                                                                                24,
                                                                                "yuv420p",
                                                                                FileFormatType.AVI);
        await using (var output = new FileStream(@"C:\mfptest\results\result.avi", FileMode.Create))
            output.Write(resultBytes, 0, resultBytes.Length);
    }

    public static async Task GetAudioFromVideoTest(VideoFileProcessor videoProcessor)
    {
        //Test block with physical paths to input and output files
        await videoProcessor.GetAudioFromVideoAsync(new MediaFile(_video1, MediaFileInputType.Path),
                                                    FileFormatType.MP3,
                                                    @"C:\mfptest\results\result.mp3");

        //Block for testing file processing as streams without specifying physical paths
        await using var stream = new FileStream(_video1, FileMode.Open);
        var resultStream = await videoProcessor.GetAudioFromVideoAsStreamAsync(new MediaFile(stream),
                                                                               FileFormatType.MP3);
        await using (var output = new FileStream(@"C:\mfptest\results\result.mp3", FileMode.Create))
            resultStream.WriteTo(output);

        //Block for testing file processing as bytes without specifying physical paths
        var bytes = await File.ReadAllBytesAsync(_video1);
        var resultBytes = await videoProcessor.GetAudioFromVideoAsBytesAsync(new MediaFile(bytes), FileFormatType.MP3);
        await using (var output = new FileStream(@"C:\mfptest\results\result.mp3", FileMode.Create))
            output.Write(resultBytes);
    }

    public static async Task ConvertVideoTest(VideoFileProcessor videoProcessor)
    {
        //Test block with physical paths to input and output files
        await videoProcessor.ConvertVideoAsync(new MediaFile(_video1, MediaFileInputType.Path),
                                               @"C:\mfptest\results\result.mp4",
                                               FileFormatType.MP4);

        //Block for testing file processing as streams without specifying physical paths
        await using var stream = new FileStream(_video1, FileMode.Open);
        var resultStream = await videoProcessor.ConvertVideoAsStreamAsync(new MediaFile(stream),
                                                                          FileFormatType.MPEG);
        await using (var output = new FileStream(@"C:\mfptest\results\result.mpg", FileMode.Create))
            resultStream.WriteTo(output);

        //Block for testing file processing as bytes without specifying physical paths
        var bytes = await File.ReadAllBytesAsync(_video1);
        var resultBytes = await videoProcessor.ConvertVideoAsBytesAsync(new MediaFile(bytes), FileFormatType.MPEG);
        await using (var output = new FileStream(@"C:\mfptest\results\result.mpg", FileMode.Create))
            output.Write(resultBytes);
    }

    public static async Task AddWaterMarkToVideoTest(VideoFileProcessor videoProcessor)
    {
        //Test block with physical paths to input and output files
        await videoProcessor.AddWaterMarkToVideoAsync(new MediaFile(_video1, MediaFileInputType.Path),
                                                      new MediaFile(_photo1, MediaFileInputType.Path),
                                                      PositionType.UpperLeft,
                                                      @"C:\mfptest\results\result.avi",
                                                      FileFormatType.AVI);

        //Block for testing file processing as streams without specifying physical paths
        await using var stream1 = new FileStream(_video1, FileMode.Open);
        await using var stream2 = new FileStream(_photo1, FileMode.Open);
        var resultStream = await videoProcessor.AddWaterMarkToVideoAsStreamAsync(new MediaFile(stream1),
                                                                                 new MediaFile(stream2),
                                                                                 PositionType.UpperLeft,
                                                                                 FileFormatType.AVI);
        await using (var output = new FileStream(@"C:\mfptest\results\result.avi", FileMode.Create))
            resultStream.WriteTo(output);

        //Block for testing file processing as bytes without specifying physical paths
        var bytesVideo = await File.ReadAllBytesAsync(_video1);
        var bytesPhoto = await File.ReadAllBytesAsync(_photo1);
        var resultBytes = await videoProcessor.AddWaterMarkToVideoAsBytesAsync(new MediaFile(bytesVideo),
                                                                               new MediaFile(bytesPhoto),
                                                                               PositionType.UpperLeft,
                                                                               FileFormatType.AVI);
        await using (var output = new FileStream(@"C:\mfptest\results\result.avi", FileMode.Create))
            output.Write(resultBytes);
    }

    public static async Task ExtractVideoFromFileTest(VideoFileProcessor videoProcessor)
    {
        //Test block with physical paths to input and output files
        await videoProcessor.ExtractVideoFromFileAsync(new MediaFile(_video1, MediaFileInputType.Path), @"C:\mfptest\results\result.avi", FileFormatType.AVI);

        //Block for testing file processing as streams without specifying physical paths
        await using var stream = new FileStream(_video1, FileMode.Open);
        var resultStream = await videoProcessor.ExtractVideoFromFileAsStreamAsync(new MediaFile(stream),
                                                                                  FileFormatType.AVI);
        await using (var output = new FileStream(@"C:\mfptest\results\result.avi", FileMode.Create))
            resultStream.WriteTo(output);

        //Block for testing file processing as bytes without specifying physical paths
        var bytes = await File.ReadAllBytesAsync(_video1);
        var resultBytes = await videoProcessor.ExtractVideoFromFileAsBytesAsync(new MediaFile(bytes), FileFormatType.AVI);
        await using (var output = new FileStream(@"C:\mfptest\results\result.avi", FileMode.Create))
            output.Write(resultBytes);
    }

    public static async Task AddAudioToVideoTest(VideoFileProcessor videoProcessor)
    {
        //Test block with physical paths to input and output files
        await videoProcessor.AddAudioToVideoAsync(new MediaFile(_audio1, MediaFileInputType.Path),
                                                  new MediaFile(_video1, MediaFileInputType.Path),
                                                  @"C:\mfptest\results\result.avi",
                                                  FileFormatType.AVI);

        //Block for testing file processing as streams without specifying physical paths
        await using var streamA = new FileStream(_audio1, FileMode.Open);
        await using var streamV = new FileStream(_video1, FileMode.Open);
        var resultStream = await videoProcessor.AddAudioToVideoAsStreamAsync(new MediaFile(streamA),
                                                                             new MediaFile(streamV),
                                                                             FileFormatType.AVI);
        await using (var output = new FileStream(@"C:\mfptest\results\result.avi", FileMode.Create))
            resultStream.WriteTo(output);

        //Block for testing file processing as bytes without specifying physical paths
        var bytesA = await File.ReadAllBytesAsync(_audio1);
        var bytesV = await File.ReadAllBytesAsync(_video1);
        var resultBytes = await videoProcessor.AddAudioToVideoAsBytesAsync(new MediaFile(bytesA), new MediaFile(bytesV), FileFormatType.AVI);
        await using (var output = new FileStream(@"C:\mfptest\results\result.avi", FileMode.Create))
            output.Write(resultBytes);
    }

    public static async Task ConvertVideoToGifTest(VideoFileProcessor videoProcessor)
    {
        //Test block with physical paths to input and output files
        await videoProcessor.ConvertVideoToGifAsync(new MediaFile(_video1, MediaFileInputType.Path), 5, 320, 0, @"C:\mfptest\results\result.gif");

        //Block for testing file processing as streams without specifying physical paths
        await using var stream = new FileStream(_video1, FileMode.Open);
        var resultStream = await videoProcessor.ConvertVideoToGifAsStreamAsync(new MediaFile(stream), 5, 320, 0);
        await using (var output = new FileStream(@"C:\mfptest\results\result.gif", FileMode.Create))
            resultStream.WriteTo(output);

        //Block for testing file processing as bytes without specifying physical paths
        var bytes = await File.ReadAllBytesAsync(_video1);
        var resultBytes = await videoProcessor.ConvertVideoToGifAsBytesAsync(new MediaFile(bytes), 5, 320, 0);
        await using (var output = new FileStream(@"C:\mfptest\results\result.gif", FileMode.Create))
            output.Write(resultBytes);
    }

    public static async Task CompressVideoTest(VideoFileProcessor videoProcessor)
    {
        //Test block with physical paths to input and output files
        await videoProcessor.CompressVideoAsync(new MediaFile(_video1, MediaFileInputType.Path), 20, @"C:\mfptest\results\result.avi", FileFormatType.AVI);

        //Block for testing file processing as streams without specifying physical paths
        await using var stream = new FileStream(_video1, FileMode.Open);
        var resultStream = await videoProcessor.CompressVideoAsStreamAsync(new MediaFile(stream), 20, FileFormatType.AVI);
        await using (var output = new FileStream(@"C:\mfptest\results\result.avi", FileMode.Create))
            resultStream.WriteTo(output);

        //Block for testing file processing as bytes without specifying physical paths
        var bytes = await File.ReadAllBytesAsync(_video1);
        var resultBytes = await videoProcessor.CompressVideoAsBytesAsync(new MediaFile(bytes), 20, FileFormatType.AVI);
        await using (var output = new FileStream(@"C:\mfptest\results\result.avi", FileMode.Create))
            output.Write(resultBytes);
    }

    public static async Task CompressImageTest(VideoFileProcessor videoProcessor)
    {
        //Test block with physical paths to input and output files
        await videoProcessor.CompressImageAsync(new MediaFile(_photo1, MediaFileInputType.Path), 100, @"C:\mfptest\results\result.jpg", FileFormatType.JPG);

        //Block for testing file processing as streams without specifying physical paths
        await using var stream = new FileStream(_photo1, FileMode.Open);
        var resultStream = await videoProcessor.CompressImageAsStreamAsync(new MediaFile(stream), 100, FileFormatType.JPG);
        await using (var output = new FileStream(@"C:\mfptest\results\result.jpg", FileMode.Create))
            resultStream.WriteTo(output);

        //Block for testing file processing as bytes without specifying physical paths
        var bytes = await File.ReadAllBytesAsync(_photo1);
        var resultBytes = await videoProcessor.CompressImageAsBytesAsync(new MediaFile(bytes), 100, FileFormatType.JPG);
        await using (var output = new FileStream(@"C:\mfptest\results\result.jpg", FileMode.Create))
            output.Write(resultBytes);
    }

    public static async Task ConcatVideosTest(VideoFileProcessor videoProcessor)
    {
        //Test block with physical paths to input and output files
        await videoProcessor.ConcatVideosAsync(new [] { new MediaFile(_video1, MediaFileInputType.Path), new MediaFile(_video2, MediaFileInputType.Path) },
                                               @"C:\mfptest\results\result.avi",
                                               FileFormatType.AVI);


        //Block for testing file processing as streams without specifying physical paths
        await using var stream1 = new FileStream(_video1, FileMode.Open);
        await using var stream2 = new FileStream(_video2, FileMode.Open);
        var resultStream = await videoProcessor.ConcatVideosAsStreamAsync(new [] { new MediaFile(stream1), new MediaFile(stream2) },
                                                                          FileFormatType.AVI);
        await using (var output = new FileStream(@"C:\mfptest\results\result.avi", FileMode.Create))
            resultStream.WriteTo(output);

        //Block for testing file processing as bytes without specifying physical paths
        var bytes1 = await File.ReadAllBytesAsync(_video1);
        var bytes2 = await File.ReadAllBytesAsync(_video2);
        var resultBytes = await videoProcessor.ConcatVideosAsBytesAsync(new [] { new MediaFile(bytes1), new MediaFile(bytes2) }, FileFormatType.AVI);
        await using (var output = new FileStream(@"C:\mfptest\results\result.avi", FileMode.Create))
            output.Write(resultBytes);
    }

    public static async Task AddSubsTest(VideoFileProcessor videoProcessor)
    {
        //Test block with physical paths to input and output files
        await videoProcessor.AddHardSubtitlesAsync(new MediaFile(_video2, MediaFileInputType.Path),
                                                   new MediaFile(_subsEn, MediaFileInputType.Path),
                                                   @"eng",
                                                   @"C:\mfptest\results\result.mp4",
                                                   FileFormatType.MP4);

        //Block for testing file processing as streams without specifying physical paths
        await using var stream1 = new FileStream(_video2, FileMode.Open);
        await using var stream2 = new FileStream(_subsRU, FileMode.Open);
        var resultStream = await videoProcessor.AddHardSubtitlesAsStreamAsync(new MediaFile(stream1),
                                                                              new MediaFile(stream2),
                                                                              @"rus",
                                                                              FileFormatType.MP4);
        await using (var output = new FileStream(@"C:\mfptest\results\result.mp4", FileMode.Create))
            resultStream.WriteTo(output);

        //Block for testing file processing as bytes without specifying physical paths
        var bytes1 = await File.ReadAllBytesAsync(_video2);
        var bytes2 = await File.ReadAllBytesAsync(_subsRU);
        var resultBytes = await videoProcessor.AddHardSubtitlesAsBytesAsync(new MediaFile(bytes1),
                                                                            new MediaFile(bytes2),
                                                                            @"rus",
                                                                            FileFormatType.MP4);
        await using (var output = new FileStream(@"C:\mfptest\results\result.mp4", FileMode.Create))
            output.Write(resultBytes);
    }

    public static async Task GetVideoInfoTest(VideoFileProcessor videoProcessor)
    {
        var stream = new FileStream(_video1, FileMode.Open);
        var data = await videoProcessor.GetVideoInfo(new MediaFile(stream));
        var result = JsonConvert.DeserializeObject<VideoFileInfo>(data, _jsonSnakeCaseSerializerSettings);
    }
}