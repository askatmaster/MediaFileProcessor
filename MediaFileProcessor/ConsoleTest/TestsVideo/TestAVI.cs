using MediaFileProcessor.Extensions;
using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Enums;
using MediaFileProcessor.Processors;
namespace ConsoleTest.TestsVideo;

public static class TestAVI
{
    private static readonly VideoFileProcessor _videoProcessor = new ();

    public static async Task Test()
    {
        var sample = TestFile.GetPath(FileFormatType.MKV);
        var imageFormat = FileFormatType.JPG;
        var resultPhysicalPath = TestFile.ResultFilePath + $@"TestExtractFrameFromVideo/resultPath.{imageFormat.ToString().ToLower()}";
        var resultStreamPath = TestFile.ResultFilePath + $@"TestExtractFrameFromVideo/resultStream.{imageFormat.ToString().ToLower()}";

        // Test block with physical paths to input and output files
        // await _videoProcessor.ExtractFrameFromVideoAsync(TimeSpan.FromMilliseconds(5000),
        //                                                  new MediaFile(sample),
        //                                                  resultPhysicalPath);

        var result = await _videoProcessor.SetStartMoovAsync(new MediaFile(sample.ToFileStream()), FileFormatType.MKV);

        var resultStream = await _videoProcessor.ExtractFrameFromVideoAsync(TimeSpan.FromMilliseconds(5000),
                                                                            new MediaFile(result.ToArray()),
                                                                            outputFormat: imageFormat);
        resultStream!.ToFile(resultStreamPath);
    }

    public static async Task TestExtractFrameFromVideo()
    {
        var moovStartRequiredFormats = new List<FileFormatType>
        {
            FileFormatType._3GP,
            FileFormatType.ASF,
            FileFormatType.FLV,
            FileFormatType.M4V,
            FileFormatType.MKV,
            FileFormatType.MOV,
            FileFormatType.WMV,
            FileFormatType.MP4
        };

        var supportedVideoFormats = new List<FileFormatType>
        {
            FileFormatType.AVI,
            FileFormatType._3GP,
            FileFormatType.ASF,
            FileFormatType.FLV,
            FileFormatType.M2TS,
            FileFormatType.M4V,
            FileFormatType.MKV,
            FileFormatType.MOV,
            FileFormatType.MP4,
            FileFormatType.MPEG,
            FileFormatType.MXF,
            FileFormatType.RM,
            FileFormatType.VOB,
            FileFormatType.WEBM,
            FileFormatType.WMV,
        };

        var supportedImageFormats = new Dictionary<FileFormatType, Dictionary<FileFormatType, int[]>>
        {
            {
                FileFormatType.JPG, new Dictionary<FileFormatType, int[]>
                {
                    { FileFormatType.AVI, new [] { 18960, 18960 } },
                    { FileFormatType._3GP, new [] { 43032, 43032 } },
                    { FileFormatType.ASF, new [] { 20524, 21378 } },
                    { FileFormatType.FLV, new [] { 14634, 15431 } },
                    { FileFormatType.M2TS, new [] { 12194, 18640 } },
                    { FileFormatType.M4V, new [] { 43032, 43032 } },
                    { FileFormatType.MKV, new [] { 43032, 43032 } },
                    { FileFormatType.MOV, new [] { 43032, 43032 } },
                    { FileFormatType.MP4, new [] { 43032, 43032 } },
                    { FileFormatType.MPEG, new [] { 12402, 12402 } },
                    { FileFormatType.MXF, new [] { 18640, 18757 } },
                    { FileFormatType.RM, new [] { 25178, 25178 } },
                    { FileFormatType.VOB, new [] { 12194, 12200 } },
                    { FileFormatType.WEBM, new [] { 38244, 41444 } },
                    { FileFormatType.WMV, new [] { 20524, 21378 } }
                }
            },
            {
                FileFormatType.PNG, new Dictionary<FileFormatType, int[]>
                {
                    { FileFormatType.AVI, new [] { 140281, 140281 } },
                    { FileFormatType._3GP, new [] { 577232, 577232 } },
                    { FileFormatType.ASF, new [] { 156677, 334231 } },
                    { FileFormatType.FLV, new [] { 143256, 212379 } },
                    { FileFormatType.M2TS, new [] { 152899, 163568 } },
                    { FileFormatType.M4V, new [] { 577232, 577232 } },
                    { FileFormatType.MKV, new [] { 577232, 577232 } },
                    { FileFormatType.MP4, new [] { 577232, 577232 } },
                    { FileFormatType.MOV, new [] { 577232, 577232 } },
                    { FileFormatType.MPEG, new [] { 150161, 150161 } },
                    { FileFormatType.MXF, new [] { 163568, 162582 } },
                    { FileFormatType.RM, new [] { 158984, 158984 } },
                    { FileFormatType.VOB, new [] { 152899, 151980 } },
                    { FileFormatType.WEBM, new [] { 411386, 362748 } },
                    { FileFormatType.WMV, new [] { 156677, 334231 } }
                }
            },
            {
                FileFormatType.TIFF, new Dictionary<FileFormatType, int[]>
                {
                    { FileFormatType.AVI, new [] { 2092250, 2092250 } },
                    { FileFormatType._3GP, new [] { 2092445, 2092445 } },
                    { FileFormatType.ASF, new [] { 2092724, 2092714 } },
                    { FileFormatType.FLV, new [] { 930290, 930272 } },
                    { FileFormatType.M2TS, new [] { 2092583, 2092603 } },
                    { FileFormatType.M4V, new [] { 2092445, 2092445 } },
                    { FileFormatType.MKV, new [] { 2092445, 2092445 } },
                    { FileFormatType.MP4, new [] { 2092445, 2092445 } },
                    { FileFormatType.MOV, new [] { 2092445, 2092445 } },
                    { FileFormatType.MPEG, new [] { 2092585, 2092585 } },
                    { FileFormatType.MXF, new [] { 2092603, 2092572 } },
                    { FileFormatType.RM, new [] { 1550072, 1550072 } },
                    { FileFormatType.VOB, new [] { 2092583, 2092636 } },
                    { FileFormatType.WEBM, new [] { 2092501, 2092717 } },
                    { FileFormatType.WMV, new [] { 2092724, 2092714 } }
                }
            },
            {
                FileFormatType.BMP, new Dictionary<FileFormatType, int[]>
                {
                    { FileFormatType.AVI, new [] { 519478, 519478 } },
                    { FileFormatType._3GP, new [] { 519478, 519478 } },
                    { FileFormatType.ASF, new [] { 519478, 519478 } },
                    { FileFormatType.FLV, new [] { 231478, 231478 } },
                    { FileFormatType.M2TS, new [] { 519478, 519478 } },
                    { FileFormatType.M4V, new [] { 519478, 519478 } },
                    { FileFormatType.MKV, new [] { 519478, 519478 } },
                    { FileFormatType.MP4, new [] { 519478, 519478 } },
                    { FileFormatType.MOV, new [] { 519478, 519478 } },
                    { FileFormatType.MPEG, new [] { 519478, 519478 } },
                    { FileFormatType.MXF, new [] { 519478, 519478 } },
                    { FileFormatType.RM, new [] { 385078, 385078 } },
                    { FileFormatType.VOB, new [] { 519478, 519478 } },
                    { FileFormatType.WEBM, new [] { 519478, 519478 } },
                    { FileFormatType.WMV, new [] { 519478, 519478 } }
                }
            },
            {
                FileFormatType.ICO, new Dictionary<FileFormatType, int[]>
                {
                    { FileFormatType.AVI, new [] { 12918, 12342 } },
                    { FileFormatType._3GP, new [] { 12918, 12342 } },
                    { FileFormatType.ASF, new [] { 12918, 12342 } },
                    { FileFormatType.FLV, new [] { 12918, 12342 } },
                    { FileFormatType.M2TS, new [] { 12918, 12342 } },
                    { FileFormatType.M4V, new [] { 12918, 12342 } },
                    { FileFormatType.MKV, new [] { 12918, 12342 } },
                    { FileFormatType.MP4, new [] { 12918, 12342 } },
                    { FileFormatType.MOV, new [] { 12918, 12342 } },
                    { FileFormatType.MPEG, new [] { 12918, 12342 } },
                    { FileFormatType.MXF, new [] { 12918, 12342 } },
                    { FileFormatType.RM, new [] { 12918, 12342 } },
                    { FileFormatType.VOB, new [] { 12918, 12342 } },
                    { FileFormatType.WEBM, new [] { 12918, 12342 } },
                    { FileFormatType.WMV, new [] { 12918, 12342 } }
                }
            },
            {
                FileFormatType.WEBP, new Dictionary<FileFormatType, int[]>
                {
                    { FileFormatType.AVI, new [] { 15832, 15832 } },
                    { FileFormatType._3GP, new [] { 26928, 26928 } },
                    { FileFormatType.ASF, new [] { 16832, 17096 } },
                    { FileFormatType.FLV, new [] { 11152, 11498 } },
                    { FileFormatType.M2TS, new [] { 15482, 16354 } },
                    { FileFormatType.M4V, new [] { 26928, 26928 } },
                    { FileFormatType.MKV, new [] { 26928, 26928 } },
                    { FileFormatType.MP4, new [] { 26928, 26928 } },
                    { FileFormatType.MOV, new [] { 26928, 26928 } },
                    { FileFormatType.MPEG, new [] { 15604, 15604 } },
                    { FileFormatType.MXF, new [] { 16354, 16196 } },
                    { FileFormatType.RM, new [] { 16676, 16676 } },
                    { FileFormatType.VOB, new [] { 15482, 15452 } },
                    { FileFormatType.WEBM, new [] { 23402, 20340 } },
                    { FileFormatType.WMV, new [] { 16832, 17096 } }
                }
            }
        };

        foreach (var videoFormat in supportedVideoFormats)
        {
            foreach ((var imageFormat, var value) in supportedImageFormats)
            {
                var sample = TestFile.GetPath(videoFormat);
                var resultPhysicalPath = TestFile.ResultFilePath + $@"TestExtractFrameFromVideo/resultPath.{imageFormat.ToString().ToLower()}";
                var resultStreamPath = TestFile.ResultFilePath + $@"TestExtractFrameFromVideo/resultStream.{imageFormat.ToString().ToLower()}";

                // Test block with physical paths to input and output files
                await _videoProcessor.ExtractFrameFromVideoAsync(TimeSpan.FromMilliseconds(5000),
                                                                 new MediaFile(sample),
                                                                 resultPhysicalPath);

                // Block for testing file processing as streams without specifying physical paths
                MemoryStream? ms = null;
                if(moovStartRequiredFormats.Contains(videoFormat))
                    ms = _videoProcessor.SetStartMoovAsync(new MediaFile(sample.ToBytes()), videoFormat).GetAwaiter().GetResult()!;

                var resultStream = await _videoProcessor.ExtractFrameFromVideoAsync(TimeSpan.FromMilliseconds(5000),
                                                                                    new MediaFile(moovStartRequiredFormats.Contains(videoFormat) ? ms!.ToArray()
                                                                                                      : sample.ToBytes()),
                                                                                    outputFormat: imageFormat);
                resultStream!.ToFile(resultStreamPath);

                TestFile.VerifyFileSize(resultPhysicalPath, value.First(x => x.Key == videoFormat).Value[0]);
                TestFile.VerifyFileSize(resultStreamPath, value.First(x => x.Key == videoFormat).Value[1]);
            }

            Thread.Sleep(1000);

            foreach (var filePath in Directory.GetFiles(TestFile.ResultFilePath + @"TestExtractFrameFromVideo/"))
                File.Delete(filePath);
        }
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
                                            resultPhysicalPath);

        //Block for testing file processing as streams without specifying physical paths
        var resultStream = await _videoProcessor.CutVideoAsync(TimeSpan.FromMilliseconds(2000),
                                                               TimeSpan.FromMilliseconds(9000),
                                                               new MediaFile(sample),
                                                               outputFormat: FileFormatType.AVI);
        resultStream!.ToFile(resultStreamPath);

        TestFile.VerifyFileSize(resultPhysicalPath, 271668);
        TestFile.VerifyFileSize(resultStreamPath, 263904);
    }

    public static async Task ConvertVideoToImages()
    {
        var sample = TestFile.GetPath(FileFormatType.AVI);
        var resultPhysicalPath = TestFile.ResultFilePath + @"ConvertVideoToImagesAsync/Path/result%03d.png";

        // //Test block with physical paths to input and output files
        await _videoProcessor.ConvertVideoToImagesAsync(new MediaFile(sample),
                                                        resultPhysicalPath);

        //Block for testing file processing as streams without specifying physical paths
        await using var stream = new FileStream(sample, FileMode.Open);
        var resultStream = await _videoProcessor.ConvertVideoToImagesAsync(new MediaFile(stream), outputFormat: FileFormatType.PNG);
        var count = 1;
        var data = resultStream!.ReadAsDataArray();

        foreach (var bytes in data)
        {
            await using (var output = new FileStream(TestFile.ResultFilePath + @$"ConvertVideoToImagesAsync\Stream\result{count++}.png", FileMode.Create))
                output.Write(bytes, 0, bytes.Length);
        }
    }
}