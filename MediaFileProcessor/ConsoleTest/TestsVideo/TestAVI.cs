using MediaFileProcessor.Extensions;
using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Enums;
using MediaFileProcessor.Processors;
namespace ConsoleTest.TestsVideo;

public static class TestAVI
{
    private static readonly VideoFileProcessor _videoProcessor = new ();

    public static async Task TestGetFrameFromVideo()
    {
        var sample = TestFile.GetPath(FileFormatType.AVI);
        var resultPhysicalPath = TestFile.ResultFilePath + @"TestGetFrameFromVideo/resultPath.jpg";
        var resultStreamPath = TestFile.ResultFilePath + @"TestGetFrameFromVideo/resultStream.jpg";

        // Test block with physical paths to input and output files
        await _videoProcessor.ExtractFrameFromVideoAsync(TimeSpan.FromMilliseconds(5000), new MediaFile(sample), resultPhysicalPath, FileFormatType.JPG);

        //Block for testing file processing as streams without specifying physical paths
        var resultStream = await _videoProcessor.ExtractFrameFromVideoAsync(TimeSpan.FromMilliseconds(5000),
                                                                            new MediaFile(sample.ToFileStream()),
                                                                            outputFormat: FileFormatType.JPG);
        resultStream!.ToFile(resultStreamPath);

        TestFile.VerifyFileSize(resultPhysicalPath, 18960);
        TestFile.VerifyFileSize(resultStreamPath, 18960);
    }

    public static async Task TestCutVideo()
    {
        var sample = TestFile.GetPath(FileFormatType.AVI);
        var resultPhysicalPath = TestFile.ResultFilePath + @"TestCutVideo/resultPath.avi";
        var resultStreamPath = TestFile.ResultFilePath + @"TestCutVideo/resultStream.avi";

        // Test block with physical paths to input and output files
        await _videoProcessor.CutVideoAsync(TimeSpan.FromMilliseconds(2000),
                                            TimeSpan.FromMilliseconds(9000),
                                            new MediaFile(sample),
                                            resultPhysicalPath,
                                            FileFormatType.AVI);

        //Block for testing file processing as streams without specifying physical paths
        var resultStream = await _videoProcessor.CutVideoAsync(TimeSpan.FromMilliseconds(2000),
                                                               TimeSpan.FromMilliseconds(9000),
                                                               new MediaFile(sample),
                                                               outputFormat: FileFormatType.AVI);
        resultStream!.ToFile(resultStreamPath);

        TestFile.VerifyFileSize(resultPhysicalPath, 271668);
        TestFile.VerifyFileSize(resultStreamPath, 263904);
    }

    public static async Task ConvertVideoToImagesAsync()
    {
        var sample = TestFile.GetPath(FileFormatType.AVI);
        var resultPhysicalPath = TestFile.ResultFilePath + @"ConvertVideoToImagesAsync/Path/result%03d.png";

        // //Test block with physical paths to input and output files
        await _videoProcessor.ConvertVideoToImagesAsync(new MediaFile(sample),
                                                               resultPhysicalPath);

        //Block for testing file processing as streams without specifying physical paths
        await using var stream = new FileStream(sample, FileMode.Open);
        var resultStream = await _videoProcessor.ConvertVideoToImagesAsync(new MediaFile(stream), outputFileFormatType: FileFormatType.PNG);
        var count = 1;
        var data = resultStream!.ReadAsDataArray();

        foreach (var bytes in data)
        {
            await using (var output = new FileStream(TestFile.ResultFilePath + @$"ConvertVideoToImagesAsync\Stream\result{count++}.png", FileMode.Create))
                output.Write(bytes, 0, bytes.Length);
        }
    }
}