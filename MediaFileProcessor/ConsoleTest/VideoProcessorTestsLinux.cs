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
public class VideoProcessorTestsLinux
{
    /// <summary>
    /// Test data
    /// </summary>
    private static readonly  string _video1 = @"test.avi";
    private static readonly  string _photo1 =  @"water.png";
    private static readonly  string _video12 =  @"test2.avi";
    private static readonly  string _video2 =  @"sample-30s.mp4";
    private static readonly  string _audio1 =  @"sample.mp3";
    private static readonly  string _subsEn =  @"subtitle.en.srt";
    private static readonly  string _subsRU =  @"subtitle.ru.srt";

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

    public static void RunAllTests()
    {
        var videoProcessor = new VideoFileProcessor();

        Console.WriteLine("ConvertImagesToVideoTest");
        ConvertImagesToVideoTest(videoProcessor).GetAwaiter().GetResult();

        Console.WriteLine("GetAudioFromVideoTest");
        GetAudioFromVideoTest(videoProcessor).GetAwaiter().GetResult();

        Console.WriteLine("ConvertVideoTest");
        ConvertVideoTest(videoProcessor).GetAwaiter().GetResult();

        Console.WriteLine("AddWaterMarkToVideoTest");
        AddWaterMarkToVideoTest(videoProcessor).GetAwaiter().GetResult();

        Console.WriteLine("ExtractVideoFromFileTest");
        ExtractVideoFromFileTest(videoProcessor).GetAwaiter().GetResult();

        Console.WriteLine("AddAudioToVideoTest");
        AddAudioToVideoTest(videoProcessor).GetAwaiter().GetResult();

        Console.WriteLine("ConvertVideoToGifTest");
        ConvertVideoToGifTest(videoProcessor).GetAwaiter().GetResult();

        Console.WriteLine("CompressVideoTest");
        CompressVideoTest(videoProcessor).GetAwaiter().GetResult();

        Console.WriteLine("CompressImageTest");
        CompressImageTest(videoProcessor).GetAwaiter().GetResult();

        Console.WriteLine("ConcatVideosTest");
        ConcatVideosTest(videoProcessor).GetAwaiter().GetResult();

        Console.WriteLine("AddSubsTest");
        AddSubsTest(videoProcessor).GetAwaiter().GetResult();

        Console.WriteLine("GetVideoInfoTest");
        var data = GetVideoInfoTest(videoProcessor).GetAwaiter().GetResult();
    }

    public static async Task ConvertImagesToVideoTest(VideoFileProcessor videoProcessor)
    {
        // var stream = new MultiStream();
        // var files = new List<string>();
        // for (var i = 1;
        //      i <= 3600;
        //      i++)
        //     files.Add($@"results2/result{i:000}.jpg");
        // foreach (var file in files)
        //     stream.AddStream(new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read));
        //
        // //Test block with physical paths to input and output files
        // videoProcessor.ConvertImagesToVideoAsync(new MediaFile(stream),
        //                                          24,
        //                                          @"results\ConvertImagesToVideoTest\resultPath.avi",
        //                                          "yuv420p",
        //                                          FileFormatType.AVI)
        //               .GetAwaiter()
        //               .GetResult();
        //
        // //Block for testing file processing as bytes without specifying physical paths
        // var files1 = new List<byte[]>();
        // for (var i = 1;
        //      i <= 3600;
        //      i++)
        //     files1.Add(await File.ReadAllBytesAsync($@"results2/result{i:000}.jpg"));
        // var resultBytes = await videoProcessor.ConvertImagesToVideoAsBytesAsync(new MediaFile(FileDataExtensions.ConcatByteArrays(false, files1.ToArray())),
        //                                                                         24,
        //                                                                         "yuv420p",
        //                                                                         FileFormatType.AVI);
        // await using (var output = new FileStream(@"results\ConvertImagesToVideoTest\resultBytes.avi", FileMode.Create))
        //     output.Write(resultBytes, 0, resultBytes.Length);
        //
        // //Block for testing file processing as streams without specifying physical paths
        // stream.Seek(0, SeekOrigin.Begin);
        // var resultStream = await videoProcessor.ConvertImagesToVideoAsStreamAsync(new MediaFile(stream), 24, "yuv420p", FileFormatType.AVI);
        // await using (var output = new FileStream(@"results\ConvertImagesToVideoTest\resultStream.avi", FileMode.Create))
        //     resultStream.WriteTo(output);
    }

    public static async Task GetAudioFromVideoTest(VideoFileProcessor videoProcessor)
    {
        //Test block with physical paths to input and output files
        await videoProcessor.GetAudioFromVideoAsync(new MediaFile(_video1),
                                                    FileFormatType.MP3,
                                                    @"results\GetAudioFromVideoTest\resultPath.mp3");

        //Block for testing file processing as bytes without specifying physical paths
        var bytes = await File.ReadAllBytesAsync(_video1);
        var resultBytes = await videoProcessor.GetAudioFromVideoAsBytesAsync(new MediaFile(bytes), FileFormatType.MP3);
        await using (var output = new FileStream(@"results\GetAudioFromVideoTest\resultBytes.mp3", FileMode.Create))
            output.Write(resultBytes);

        //Block for testing file processing as streams without specifying physical paths
        await using var stream = new FileStream(_video1, FileMode.Open);
        var resultStream = await videoProcessor.GetAudioFromVideoAsStreamAsync(new MediaFile(stream),
                                                                               FileFormatType.MP3);
        await using (var output = new FileStream(@"results\GetAudioFromVideoTest\resultStream.mp3", FileMode.Create))
            resultStream.WriteTo(output);
    }

    public static async Task ConvertVideoTest(VideoFileProcessor videoProcessor)
    {
        //Test block with physical paths to input and output files
        await videoProcessor.ConvertVideoAsync(new MediaFile(_video1),
                                               @"results\ConvertVideoTest\resultPath.mp4",
                                               FileFormatType.MP4);

        //Block for testing file processing as bytes without specifying physical paths
        var bytes = await File.ReadAllBytesAsync(_video1);
        var resultBytes = await videoProcessor.ConvertVideoAsBytesAsync(new MediaFile(bytes), FileFormatType.MPEG);
        await using (var output = new FileStream(@"results\ConvertVideoTest\resultBytes.mpg", FileMode.Create))
            output.Write(resultBytes);

        //Block for testing file processing as streams without specifying physical paths
        await using var stream = new FileStream(_video1, FileMode.Open);
        var resultStream = await videoProcessor.ConvertVideoAsStreamAsync(new MediaFile(stream),
                                                                          FileFormatType.MPEG);
        await using (var output = new FileStream(@"results\ConvertVideoTest\resultStream.mpg", FileMode.Create))
            resultStream.WriteTo(output);
    }

    public static async Task AddWaterMarkToVideoTest(VideoFileProcessor videoProcessor)
    {
        //Test block with physical paths to input and output files
        await videoProcessor.AddWaterMarkToVideoAsync(new MediaFile(_video1),
                                                      new MediaFile(_photo1),
                                                      PositionType.UpperLeft,
                                                      @"results\AddWaterMarkToVideoTest\resultPath.avi",
                                                      FileFormatType.AVI);

        //Block for testing file processing as bytes without specifying physical paths
        var bytesVideo = await File.ReadAllBytesAsync(_video1);
        var bytesPhoto = await File.ReadAllBytesAsync(_photo1);
        var resultBytes = await videoProcessor.AddWaterMarkToVideoAsBytesAsync(new MediaFile(bytesVideo),
                                                                               new MediaFile(bytesPhoto),
                                                                               PositionType.UpperLeft,
                                                                               FileFormatType.AVI);
        await using (var output = new FileStream(@"results\AddWaterMarkToVideoTest\resultBytes.avi", FileMode.Create))
            output.Write(resultBytes);

        //Block for testing file processing as streams without specifying physical paths
        await using var stream1 = new FileStream(_video1, FileMode.Open);
        await using var stream2 = new FileStream(_photo1, FileMode.Open);
        var resultStream = await videoProcessor.AddWaterMarkToVideoAsStreamAsync(new MediaFile(stream1),
                                                                                 new MediaFile(stream2),
                                                                                 PositionType.UpperLeft,
                                                                                 FileFormatType.AVI);
        await using (var output = new FileStream(@"results\AddWaterMarkToVideoTest\resultStream.avi", FileMode.Create))
            resultStream.WriteTo(output);
    }

    public static async Task ExtractVideoFromFileTest(VideoFileProcessor videoProcessor)
    {
        //Test block with physical paths to input and output files
        await videoProcessor.ExtractVideoFromFileAsync(new MediaFile(_video1),
                                                       @"results\ExtractVideoFromFileTest\resultPath.avi",
                                                       FileFormatType.AVI);

        //Block for testing file processing as bytes without specifying physical paths
        var bytes = await File.ReadAllBytesAsync(_video1);
        var resultBytes = await videoProcessor.ExtractVideoFromFileAsBytesAsync(new MediaFile(bytes), FileFormatType.AVI);
        await using (var output = new FileStream(@"results\ExtractVideoFromFileTest\resultBytes.avi", FileMode.Create))
            output.Write(resultBytes);

        //Block for testing file processing as streams without specifying physical paths
        await using var stream = new FileStream(_video1, FileMode.Open);
        var resultStream = await videoProcessor.ExtractVideoFromFileAsStreamAsync(new MediaFile(stream),
                                                                                  FileFormatType.AVI);
        await using (var output = new FileStream(@"results\ExtractVideoFromFileTest\resultStream.avi", FileMode.Create))
            resultStream.WriteTo(output);
    }

    public static async Task AddAudioToVideoTest(VideoFileProcessor videoProcessor)
    {
        //Test block with physical paths to input and output files
        await videoProcessor.AddAudioToVideoAsync(new MediaFile(_audio1),
                                                  new MediaFile(_video1),
                                                  @"results\AddAudioToVideoTest\resultPath.avi",
                                                  FileFormatType.AVI);

        //Block for testing file processing as bytes without specifying physical paths
        var bytesA = await File.ReadAllBytesAsync(_audio1);
        var bytesV = await File.ReadAllBytesAsync(_video1);
        var resultBytes = await videoProcessor.AddAudioToVideoAsBytesAsync(new MediaFile(bytesA), new MediaFile(bytesV), FileFormatType.AVI);
        await using (var output = new FileStream(@"results\AddAudioToVideoTest\resultBytes.avi", FileMode.Create))
            output.Write(resultBytes);

        //Block for testing file processing as streams without specifying physical paths
        await using var streamA = new FileStream(_audio1, FileMode.Open);
        await using var streamV = new FileStream(_video1, FileMode.Open);
        var resultStream = await videoProcessor.AddAudioToVideoAsStreamAsync(new MediaFile(streamA),
                                                                             new MediaFile(streamV),
                                                                             FileFormatType.AVI);
        await using (var output = new FileStream(@"results\AddAudioToVideoTest\resultStream.avi", FileMode.Create))
            resultStream.WriteTo(output);
    }

    public static async Task ConvertVideoToGifTest(VideoFileProcessor videoProcessor)
    {
        //Test block with physical paths to input and output files
        await videoProcessor.ConvertVideoToGifAsync(new MediaFile(_video1),
                                                    5,
                                                    320,
                                                    0,
                                                    @"results\ConvertVideoToGifTest\resultPath.gif");

        //Block for testing file processing as bytes without specifying physical paths
        var bytes = await File.ReadAllBytesAsync(_video1);
        var resultBytes = await videoProcessor.ConvertVideoToGifAsBytesAsync(new MediaFile(bytes), 5, 320, 0);
        await using (var output = new FileStream(@"results\ConvertVideoToGifTest\resultBytes.gif", FileMode.Create))
            output.Write(resultBytes);

        //Block for testing file processing as streams without specifying physical paths
        await using var stream = new FileStream(_video1, FileMode.Open);
        var resultStream = await videoProcessor.ConvertVideoToGifAsStreamAsync(new MediaFile(stream), 5, 320, 0);
        await using (var output = new FileStream(@"results\ConvertVideoToGifTest\resultStream.gif", FileMode.Create))
            resultStream.WriteTo(output);
    }

    public static async Task CompressVideoTest(VideoFileProcessor videoProcessor)
    {
        //Test block with physical paths to input and output files
        await videoProcessor.CompressVideoAsync(new MediaFile(_video1),
                                                20,
                                                @"results\CompressVideoTest\resultPath.avi",
                                                FileFormatType.AVI);

        //Block for testing file processing as bytes without specifying physical paths
        var bytes = await File.ReadAllBytesAsync(_video1);
        var resultBytes = await videoProcessor.CompressVideoAsBytesAsync(new MediaFile(bytes), 20, FileFormatType.AVI);
        await using (var output = new FileStream(@"results\CompressVideoTest\resultBytes.avi", FileMode.Create))
            output.Write(resultBytes);

        //Block for testing file processing as streams without specifying physical paths
        await using var stream = new FileStream(_video1, FileMode.Open);
        var resultStream = await videoProcessor.CompressVideoAsStreamAsync(new MediaFile(stream), 20, FileFormatType.AVI);
        await using (var output = new FileStream(@"results\CompressVideoTest\resultStream.avi", FileMode.Create))
            resultStream.WriteTo(output);
    }

    public static async Task CompressImageTest(VideoFileProcessor videoProcessor)
    {
        //Test block with physical paths to input and output files
        await videoProcessor.CompressImageAsync(new MediaFile(_photo1),
                                                100,
                                                @"results\CompressImageTest\resultPath.jpg",
                                                FileFormatType.JPG);


        //Block for testing file processing as bytes without specifying physical paths
        var bytes = await File.ReadAllBytesAsync(_photo1);
        var resultBytes = await videoProcessor.CompressImageAsBytesAsync(new MediaFile(bytes), 100, FileFormatType.JPG);
        await using (var output = new FileStream(@"results\CompressImageTest\resultBytes.jpg", FileMode.Create))
            output.Write(resultBytes);

        //Block for testing file processing as streams without specifying physical paths
        await using var stream = new FileStream(_photo1, FileMode.Open);
        var resultStream = await videoProcessor.CompressImageAsStreamAsync(new MediaFile(stream), 100, FileFormatType.JPG);
        await using (var output = new FileStream(@"results\CompressImageTest\resultStream.jpg", FileMode.Create))
            resultStream.WriteTo(output);
    }

    public static async Task ConcatVideosTest(VideoFileProcessor videoProcessor)
    {
        //Test block with physical paths to input and output files
        await videoProcessor.ConcatVideosAsync(new [] { new MediaFile(_video1), new MediaFile(_video12) },
                                               @"results\ConcatVideosTest\resultPath.avi",
                                               FileFormatType.AVI);

        //Block for testing file processing as bytes without specifying physical paths
        var bytes1 = await File.ReadAllBytesAsync(_video1);
        var bytes2 = await File.ReadAllBytesAsync(_video12);
        var resultBytes = await videoProcessor.ConcatVideosAsBytesAsync(new [] { new MediaFile(bytes1), new MediaFile(bytes2) }, FileFormatType.AVI);
        await using (var output = new FileStream(@"results\ConcatVideosTest\resultBytes.avi", FileMode.Create))
            output.Write(resultBytes);

        //Block for testing file processing as streams without specifying physical paths
        await using var stream1 = new FileStream(_video1, FileMode.Open);
        await using var stream2 = new FileStream(_video12, FileMode.Open);
        var resultStream = await videoProcessor.ConcatVideosAsStreamAsync(new [] { new MediaFile(stream1), new MediaFile(stream2) },
                                                                          FileFormatType.AVI);
        await using (var output = new FileStream(@"results\ConcatVideosTest\resultStream.avi", FileMode.Create))
            resultStream.WriteTo(output);
    }

    public static async Task AddSubsTest(VideoFileProcessor videoProcessor)
    {
        //Test block with physical paths to input and output files
        await videoProcessor.AddHardSubtitlesAsync(new MediaFile(_video2),
                                                   new MediaFile(_subsEn),
                                                   @"eng",
                                                   @"results\result.mp4",
                                                   FileFormatType.MP4);

        //Block for testing file processing as streams without specifying physical paths
        await using var stream1 = new FileStream(_video2, FileMode.Open);
        await using var stream2 = new FileStream(_subsRU, FileMode.Open);
        var resultStream = await videoProcessor.AddHardSubtitlesAsStreamAsync(new MediaFile(stream1),
                                                                              new MediaFile(stream2),
                                                                              @"rus",
                                                                              FileFormatType.MP4);
        await using (var output = new FileStream(@"results\result.mp4", FileMode.Create))
            resultStream.WriteTo(output);

        //Block for testing file processing as bytes without specifying physical paths
        var bytes1 = await File.ReadAllBytesAsync(_video2);
        var bytes2 = await File.ReadAllBytesAsync(_subsRU);
        var resultBytes = await videoProcessor.AddHardSubtitlesAsBytesAsync(new MediaFile(bytes1),
                                                                            new MediaFile(bytes2),
                                                                            @"rus",
                                                                            FileFormatType.MP4);
        await using (var output = new FileStream(@"results\result.mp4", FileMode.Create))
            output.Write(resultBytes);
    }

    public static async Task<VideoFileInfo> GetVideoInfoTest(VideoFileProcessor videoProcessor)
    {
        var stream = new FileStream(_video1, FileMode.Open);
        var data = await videoProcessor.GetVideoInfo(new MediaFile(stream));
        return JsonConvert.DeserializeObject<VideoFileInfo>(data, _jsonSnakeCaseSerializerSettings)!;
    }
}