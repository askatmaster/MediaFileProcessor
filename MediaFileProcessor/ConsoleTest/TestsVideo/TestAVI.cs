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
        var resultBytesPath = TestFile.ResultFilePath + @"TestGetFrameFromVideo/resultBytes.jpg";
        var resultStreamPath = TestFile.ResultFilePath + @"TestGetFrameFromVideo/resultStream.jpg";

        // Test block with physical paths to input and output files
        await _videoProcessor.GetFrameFromVideoAsync(TimeSpan.FromMilliseconds(5000),
                                                     new MediaFile(sample, MediaFileInputType.Path),
                                                     resultPhysicalPath,
                                                     FileFormatType.JPG);

        //Block for testing file processing as bytes without specifying physical paths
        var resultBytes = await _videoProcessor.GetFrameFromVideoAsBytesAsync(TimeSpan.FromMilliseconds(5000),
                                                                              new MediaFile(sample.ToBytes()),
                                                                              FileFormatType.JPG);
        resultBytes.ToFile(resultBytesPath);

        //Block for testing file processing as streams without specifying physical paths
        var resultStream = await _videoProcessor.GetFrameFromVideoAsStreamAsync(TimeSpan.FromMilliseconds(5000),
                                                                                new MediaFile(sample.ToFileStream()),
                                                                                FileFormatType.JPG);
        resultStream.ToFile(resultStreamPath);

        TestFile.VerifyFileSize(resultPhysicalPath, 18960);
        TestFile.VerifyFileSize(resultBytesPath, 18960);
        TestFile.VerifyFileSize(resultStreamPath, 18960);
    }
}