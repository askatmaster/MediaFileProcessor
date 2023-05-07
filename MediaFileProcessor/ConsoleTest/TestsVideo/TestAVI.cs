using MediaFileProcessor.Extensions;
using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Enums;
using MediaFileProcessor.Processors;
namespace ConsoleTest.TestsVideo;

public static class TestAVI
{
    private static readonly VideoFileProcessor _videoProcessor = new ();
    private static readonly List<FileFormatType> moovStartRequiredFormats = new()
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

    private static readonly List<FileFormatType> supportedVideoFormats = new()
    {
        FileFormatType.AVI,
        // FileFormatType._3GP,
        // FileFormatType.ASF,
        // FileFormatType.FLV,
        // FileFormatType.M2TS,
        // FileFormatType.M4V, //TODO fix add audio
        // FileFormatType.MKV,
        // FileFormatType.MOV,
        // FileFormatType.MP4,
        // FileFormatType.MPEG,
        // FileFormatType.MXF,
        // FileFormatType.RM,
        // FileFormatType.VOB,
        // FileFormatType.WEBM,
        // FileFormatType.GXF,
        // FileFormatType.TS,
        // FileFormatType.WMV
    };

    private static readonly List<FileFormatType> supportedAudioFormats = new()
    {
        FileFormatType.AAC,
        // FileFormatType.FLAC,
        // FileFormatType.MP3,
        // FileFormatType.OGG,
        // FileFormatType.WAV,
        // FileFormatType.WMA,
        // FileFormatType.M4A
    };

    // public static async Task Test()
    // {
    //     var sample = TestFile.GetPath(FileFormatType.MKV);
    //     var imageFormat = FileFormatType.JPG;
    //     var resultPhysicalPath = TestFile.ResultFilePath + $@"TestExtractFrameFromVideo/resultPath.{imageFormat.ToString().ToLowerInvariant()}";
    //     var resultStreamPath = TestFile.ResultFilePath + $@"TestExtractFrameFromVideo/resultStream.{imageFormat.ToString().ToLowerInvariant()}";
    //
    //     // Test block with physical paths to input and output files
    //     // await _videoProcessor.ExtractFrameFromVideoAsync(TimeSpan.FromMilliseconds(5000),
    //     //                                                  new MediaFile(sample),
    //     //                                                  resultPhysicalPath);
    //
    //     var result = await _videoProcessor.SetStartMoovAsync(new MediaFile(sample.ToFileStream()), FileFormatType.MKV);
    //
    //     var resultStream = await _videoProcessor.ExtractFrameFromVideoAsync(TimeSpan.FromMilliseconds(5000),
    //                                                                         new MediaFile(result.ToArray()),
    //                                                                         outputFormat: imageFormat);
    //     resultStream!.ToFile(resultStreamPath);
    // }

    public static async Task TestExtractFrameFromVideo()
    {
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
                    { FileFormatType.WMV, new [] { 5502, 5871 } }
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
                    { FileFormatType.WMV, new [] { 52543, 60562 } }
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
                    { FileFormatType.WMV, new [] { 520484, 520476 } }
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
                    { FileFormatType.WMV, new [] { 130678, 130678 } }
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
                    { FileFormatType.WMV, new [] { 2708, 2762 } }
                }
            }
        };

        foreach (var videoFormat in supportedVideoFormats)
        {
            foreach ((var imageFormat, var value) in supportedImageFormats)
            {
                var sample = TestFile.GetPath(videoFormat);
                var resultPhysicalPath = TestFile.ResultFilePath + $@"TestExtractFrameFromVideo/resultPath.{imageFormat.ToString().ToLowerInvariant()}";
                var resultStreamPath = TestFile.ResultFilePath + $@"TestExtractFrameFromVideo/resultStream.{imageFormat.ToString().ToLowerInvariant()}";

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

            Thread.Sleep(500);

            foreach (var filePath in Directory.GetFiles(TestFile.ResultFilePath + @"TestExtractFrameFromVideo/"))
                File.Delete(filePath);
        }
    }

    public static async Task TestCutVideo()
    {
        var expectedFiles = new Dictionary<FileFormatType, int[]>
        {
            { FileFormatType.AVI, new [] { 271668, 263904 } },
            { FileFormatType._3GP, new [] { 224077, 225280 } },
            { FileFormatType.ASF, new [] { 359245, 370688 } },
            { FileFormatType.FLV, new [] { 172335, 141344 } },
            { FileFormatType.M2TS, new [] { 356352, 345544 } },
            { FileFormatType.M4V, new [] { 224203, 220885 } },
            { FileFormatType.MKV, new [] { 223155, 223230 } },
            { FileFormatType.MP4, new [] { 224195, 225280 } },
            { FileFormatType.MOV, new [] { 224142, 225280 } },
            { FileFormatType.MPEG, new [] { 303104, 303104 } },
            { FileFormatType.MXF, new [] { 477753, 486457 } },
            { FileFormatType.RM, new [] { 911963, 911963 } },
            { FileFormatType.VOB, new [] { 292864, 290816 } },
            { FileFormatType.WEBM, new [] { 154923, 154887 } },
            { FileFormatType.WMV, new [] { 369087, 401103 } }
        };

        foreach (var videoFormat in supportedVideoFormats)
        {
            var sample = TestFile.GetPath(videoFormat);
            var resultPhysicalPath = TestFile.ResultFilePath + @$"TestCutVideo/resultPath.{videoFormat.ToString().Replace("_", "").ToLowerInvariant()}";
            var resultStreamPath = TestFile.ResultFilePath + @$"TestCutVideo/resultStream.{videoFormat.ToString().Replace("_", "").ToLowerInvariant()}";

            // Test block with physical paths to input and output files
            await _videoProcessor.CutVideoAsync(TimeSpan.FromMilliseconds(2000),
                                                TimeSpan.FromMilliseconds(9000),
                                                new MediaFile(sample),
                                                resultPhysicalPath);

            //Block for testing file processing as streams without specifying physical paths
            MemoryStream? ms = null;
            if(moovStartRequiredFormats.Contains(videoFormat))
                ms = _videoProcessor.SetStartMoovAsync(new MediaFile(sample.ToBytes()), videoFormat).GetAwaiter().GetResult()!;

            var resultStream = await _videoProcessor.CutVideoAsync(TimeSpan.FromMilliseconds(2000),
                                                                   TimeSpan.FromMilliseconds(9000),
                                                                   new MediaFile(moovStartRequiredFormats.Contains(videoFormat) ? ms!.ToArray()
                                                                                     : sample.ToBytes()),
                                                                   outputFormat: videoFormat);
            resultStream!.ToFile(resultStreamPath);

            TestFile.VerifyFileSize(resultPhysicalPath, expectedFiles.First(x => x.Key == videoFormat).Value[0]);
            TestFile.VerifyFileSize(resultStreamPath, expectedFiles.First(x => x.Key == videoFormat).Value[1]);
        }

        Thread.Sleep(500);

        foreach (var filePath in Directory.GetFiles(TestFile.ResultFilePath + @"TestCutVideo/"))
            File.Delete(filePath);
    }

    public static async Task ConvertVideoToImages()
    {
        var supportedImageFormats = new Dictionary<FileFormatType, Dictionary<FileFormatType, int[]>>
        {
            {
                FileFormatType.JPG, new Dictionary<FileFormatType, int[]>
                {
                    { FileFormatType.AVI, new [] { 41148, 41148 } },
                    { FileFormatType._3GP, new [] { 39364, 43032 } },
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
                    { FileFormatType.WMV, new [] { 5502, 5871 } }
                }
            }
            // {
            //     FileFormatType.PNG, new Dictionary<FileFormatType, int[]>
            //     {
            //         { FileFormatType.AVI, new [] { 388742, 388742 } },
            //         { FileFormatType._3GP, new [] { 577232, 577232 } },
            //         { FileFormatType.ASF, new [] { 156677, 334231 } },
            //         { FileFormatType.FLV, new [] { 143256, 212379 } },
            //         { FileFormatType.M2TS, new [] { 152899, 163568 } },
            //         { FileFormatType.M4V, new [] { 577232, 577232 } },
            //         { FileFormatType.MKV, new [] { 577232, 577232 } },
            //         { FileFormatType.MP4, new [] { 577232, 577232 } },
            //         { FileFormatType.MOV, new [] { 577232, 577232 } },
            //         { FileFormatType.MPEG, new [] { 150161, 150161 } },
            //         { FileFormatType.MXF, new [] { 163568, 162582 } },
            //         { FileFormatType.RM, new [] { 158984, 158984 } },
            //         { FileFormatType.VOB, new [] { 152899, 151980 } },
            //         { FileFormatType.WEBM, new [] { 411386, 362748 } },
            //         { FileFormatType.WMV, new [] { 52543, 60562 } }
            //     }
            // },
            // {
            //     FileFormatType.TIFF, new Dictionary<FileFormatType, int[]>
            //     {
            //         { FileFormatType.AVI, new [] { 2092581, 2092581 } },
            //         { FileFormatType._3GP, new [] { 2092445, 2092445 } },
            //         { FileFormatType.ASF, new [] { 2092724, 2092714 } },
            //         { FileFormatType.FLV, new [] { 930290, 930272 } },
            //         { FileFormatType.M2TS, new [] { 2092583, 2092603 } },
            //         { FileFormatType.M4V, new [] { 2092445, 2092445 } },
            //         { FileFormatType.MKV, new [] { 2092445, 2092445 } },
            //         { FileFormatType.MP4, new [] { 2092445, 2092445 } },
            //         { FileFormatType.MOV, new [] { 2092445, 2092445 } },
            //         { FileFormatType.MPEG, new [] { 2092585, 2092585 } },
            //         { FileFormatType.MXF, new [] { 2092603, 2092572 } },
            //         { FileFormatType.RM, new [] { 1550072, 1550072 } },
            //         { FileFormatType.VOB, new [] { 2092583, 2092636 } },
            //         { FileFormatType.WEBM, new [] { 2092501, 2092717 } },
            //         { FileFormatType.WMV, new [] { 520484, 520476 } }
            //     }
            // },
            // {
            //     FileFormatType.BMP, new Dictionary<FileFormatType, int[]>
            //     {
            //         { FileFormatType.AVI, new [] { 519478, 519478 } },
            //         { FileFormatType._3GP, new [] { 519478, 519478 } },
            //         { FileFormatType.ASF, new [] { 519478, 519478 } },
            //         { FileFormatType.FLV, new [] { 231478, 231478 } },
            //         { FileFormatType.M2TS, new [] { 519478, 519478 } },
            //         { FileFormatType.M4V, new [] { 519478, 519478 } },
            //         { FileFormatType.MKV, new [] { 519478, 519478 } },
            //         { FileFormatType.MP4, new [] { 519478, 519478 } },
            //         { FileFormatType.MOV, new [] { 519478, 519478 } },
            //         { FileFormatType.MPEG, new [] { 519478, 519478 } },
            //         { FileFormatType.MXF, new [] { 519478, 519478 } },
            //         { FileFormatType.RM, new [] { 385078, 385078 } },
            //         { FileFormatType.VOB, new [] { 519478, 519478 } },
            //         { FileFormatType.WEBM, new [] { 519478, 519478 } },
            //         { FileFormatType.WMV, new [] { 130678, 130678 } }
            //     }
            // },
            // {
            //     FileFormatType.WEBP, new Dictionary<FileFormatType, int[]>
            //     {
            //         { FileFormatType.AVI, new [] { 27014, 27014 } },
            //         { FileFormatType._3GP, new [] { 26928, 26928 } },
            //         { FileFormatType.ASF, new [] { 16832, 17096 } },
            //         { FileFormatType.FLV, new [] { 11152, 11498 } },
            //         { FileFormatType.M2TS, new [] { 15482, 16354 } },
            //         { FileFormatType.M4V, new [] { 26928, 26928 } },
            //         { FileFormatType.MKV, new [] { 26928, 26928 } },
            //         { FileFormatType.MP4, new [] { 26928, 26928 } },
            //         { FileFormatType.MOV, new [] { 26928, 26928 } },
            //         { FileFormatType.MPEG, new [] { 15604, 15604 } },
            //         { FileFormatType.MXF, new [] { 16354, 16196 } },
            //         { FileFormatType.RM, new [] { 16676, 16676 } },
            //         { FileFormatType.VOB, new [] { 15482, 15452 } },
            //         { FileFormatType.WEBM, new [] { 23402, 20340 } },
            //         { FileFormatType.WMV, new [] { 2708, 2762 } }
            //     }
            // }
        };

        foreach (var videoFormat in supportedVideoFormats)
        {
            foreach ((var imageFormat, var value) in supportedImageFormats)
            {
                var sample = TestFile.GetPath(videoFormat);

                var resultPhysicalPath = TestFile.ResultFilePath + $@"ConvertVideoToImages/Path/result%03d.{imageFormat.ToString().Replace("_", "").ToLowerInvariant()}";

                // Test block with physical paths to input and output files
                await _videoProcessor.ConvertVideoToImagesAsync(new MediaFile(sample),
                                                                resultPhysicalPath);

                //Block for testing file processing as streams without specifying physical paths
                MemoryStream? ms = null;
                if(moovStartRequiredFormats.Contains(videoFormat))
                    ms = _videoProcessor.SetStartMoovAsync(new MediaFile(sample.ToBytes()), videoFormat).GetAwaiter().GetResult()!;

                var resultStream = await _videoProcessor.ConvertVideoToImagesAsync(new MediaFile(moovStartRequiredFormats.Contains(videoFormat) ? ms!.ToArray()
                                                                                                     : sample.ToBytes()),
                                                                                   outputFormat: imageFormat);
                var count = 1;
                var data = resultStream!.ReadAsDataArray();

                foreach (var bytes in data)
                {
                    await using (var output = new FileStream(TestFile.ResultFilePath + @$"ConvertVideoToImages\Stream\result{count++:000}.{imageFormat.ToString().Replace("_", "").ToLowerInvariant()}", FileMode.Create))
                        output.Write(bytes, 0, bytes.Length);
                }
            }

            Thread.Sleep(500);

            foreach (var filePath in Directory.GetFiles(TestFile.ResultFilePath + @"ConvertVideoToImages\Stream"))
                File.Delete(filePath);
        }
    }


    public static async Task ConvertImagesToVideo()
    {
        var supportedImageFormats = new Dictionary<FileFormatType, Dictionary<FileFormatType, int[]>>
        {
            {
                FileFormatType.JPG, new Dictionary<FileFormatType, int[]>
                {
                    { FileFormatType.AVI, new [] { 41148, 41148 } },
                    { FileFormatType._3GP, new [] { 39364, 43032 } },
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
                    { FileFormatType.WMV, new [] { 5502, 5871 } }
                }
            }
            // {
            //     FileFormatType.PNG, new Dictionary<FileFormatType, int[]>
            //     {
            //         { FileFormatType.AVI, new [] { 388742, 388742 } },
            //         { FileFormatType._3GP, new [] { 577232, 577232 } },
            //         { FileFormatType.ASF, new [] { 156677, 334231 } },
            //         { FileFormatType.FLV, new [] { 143256, 212379 } },
            //         { FileFormatType.M2TS, new [] { 152899, 163568 } },
            //         { FileFormatType.M4V, new [] { 577232, 577232 } },
            //         { FileFormatType.MKV, new [] { 577232, 577232 } },
            //         { FileFormatType.MP4, new [] { 577232, 577232 } },
            //         { FileFormatType.MOV, new [] { 577232, 577232 } },
            //         { FileFormatType.MPEG, new [] { 150161, 150161 } },
            //         { FileFormatType.MXF, new [] { 163568, 162582 } },
            //         { FileFormatType.RM, new [] { 158984, 158984 } },
            //         { FileFormatType.VOB, new [] { 152899, 151980 } },
            //         { FileFormatType.WEBM, new [] { 411386, 362748 } },
            //         { FileFormatType.WMV, new [] { 52543, 60562 } }
            //     }
            // },
            // {
            //     FileFormatType.TIFF, new Dictionary<FileFormatType, int[]>
            //     {
            //         { FileFormatType.AVI, new [] { 2092581, 2092581 } },
            //         { FileFormatType._3GP, new [] { 2092445, 2092445 } },
            //         { FileFormatType.ASF, new [] { 2092724, 2092714 } },
            //         { FileFormatType.FLV, new [] { 930290, 930272 } },
            //         { FileFormatType.M2TS, new [] { 2092583, 2092603 } },
            //         { FileFormatType.M4V, new [] { 2092445, 2092445 } },
            //         { FileFormatType.MKV, new [] { 2092445, 2092445 } },
            //         { FileFormatType.MP4, new [] { 2092445, 2092445 } },
            //         { FileFormatType.MOV, new [] { 2092445, 2092445 } },
            //         { FileFormatType.MPEG, new [] { 2092585, 2092585 } },
            //         { FileFormatType.MXF, new [] { 2092603, 2092572 } },
            //         { FileFormatType.RM, new [] { 1550072, 1550072 } },
            //         { FileFormatType.VOB, new [] { 2092583, 2092636 } },
            //         { FileFormatType.WEBM, new [] { 2092501, 2092717 } },
            //         { FileFormatType.WMV, new [] { 520484, 520476 } }
            //     }
            // },
            // {
            //     FileFormatType.BMP, new Dictionary<FileFormatType, int[]>
            //     {
            //         { FileFormatType.AVI, new [] { 519478, 519478 } },
            //         { FileFormatType._3GP, new [] { 519478, 519478 } },
            //         { FileFormatType.ASF, new [] { 519478, 519478 } },
            //         { FileFormatType.FLV, new [] { 231478, 231478 } },
            //         { FileFormatType.M2TS, new [] { 519478, 519478 } },
            //         { FileFormatType.M4V, new [] { 519478, 519478 } },
            //         { FileFormatType.MKV, new [] { 519478, 519478 } },
            //         { FileFormatType.MP4, new [] { 519478, 519478 } },
            //         { FileFormatType.MOV, new [] { 519478, 519478 } },
            //         { FileFormatType.MPEG, new [] { 519478, 519478 } },
            //         { FileFormatType.MXF, new [] { 519478, 519478 } },
            //         { FileFormatType.RM, new [] { 385078, 385078 } },
            //         { FileFormatType.VOB, new [] { 519478, 519478 } },
            //         { FileFormatType.WEBM, new [] { 519478, 519478 } },
            //         { FileFormatType.WMV, new [] { 130678, 130678 } }
            //     }
            // },
            // {
            //     FileFormatType.WEBP, new Dictionary<FileFormatType, int[]>
            //     {
            //         { FileFormatType.AVI, new [] { 27014, 27014 } },
            //         { FileFormatType._3GP, new [] { 26928, 26928 } },
            //         { FileFormatType.ASF, new [] { 16832, 17096 } },
            //         { FileFormatType.FLV, new [] { 11152, 11498 } },
            //         { FileFormatType.M2TS, new [] { 15482, 16354 } },
            //         { FileFormatType.M4V, new [] { 26928, 26928 } },
            //         { FileFormatType.MKV, new [] { 26928, 26928 } },
            //         { FileFormatType.MP4, new [] { 26928, 26928 } },
            //         { FileFormatType.MOV, new [] { 26928, 26928 } },
            //         { FileFormatType.MPEG, new [] { 15604, 15604 } },
            //         { FileFormatType.MXF, new [] { 16354, 16196 } },
            //         { FileFormatType.RM, new [] { 16676, 16676 } },
            //         { FileFormatType.VOB, new [] { 15482, 15452 } },
            //         { FileFormatType.WEBM, new [] { 23402, 20340 } },
            //         { FileFormatType.WMV, new [] { 2708, 2762 } }
            //     }
            // }
        };

        foreach (var videoFormat in supportedVideoFormats)
        {
            foreach ((var imageFormat, var value) in supportedImageFormats)
            {
                var stream = new MultiStream();
                var files = new List<string>();
                for (var i = 1; i <= 400; i++)
                    files.Add(TestFile.ResultFilePath + $@"ConvertVideoToImages\Stream2\result{i:000}.{imageFormat.ToString().Replace("_", "").ToLowerInvariant()}");

                var resultPhysicalPath = TestFile.ResultFilePath + @$"ConvertImagesToVideo\resultPath.{videoFormat.ToString().Replace("_", "").ToLowerInvariant()}";
                var resultStreamPath = TestFile.ResultFilePath + @$"ConvertImagesToVideo\resultStream.{videoFormat.ToString().Replace("_", "").ToLowerInvariant()}";

                foreach (var file in files)
                    stream.AddStream(new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read));

                //Test block with physical paths to input and output files
                _videoProcessor.ConvertImagesToVideoAsync(new MediaFile(stream),
                                                         24,
                                                         outputFile: resultPhysicalPath)
                              .GetAwaiter()
                              .GetResult();


                var stream1 = new MultiStream();
                var files1 = new List<string>();
                for (var i = 1; i <= 400; i++)
                    files1.Add(TestFile.ResultFilePath + $@"ConvertVideoToImages\Stream2\result{i:000}.{imageFormat.ToString().Replace("_", "").ToLowerInvariant()}");

                foreach (var file in files1)
                    stream1.AddStream(new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read));

                var resultStream = await _videoProcessor.ConvertImagesToVideoAsync(new MediaFile(stream1), 24, outputFormat: videoFormat);
                await using (var output = new FileStream(resultStreamPath, FileMode.Create))
                    resultStream!.WriteTo(output);
            }
        }
    }

    public static async Task ExtractAudioFromVideo()
    {
        foreach (var videoFormat in supportedVideoFormats)
        {
            foreach (var audioFormat in supportedAudioFormats)
            {
                var sample = TestFile.GetPath(videoFormat);
                var resultPhysicalPath = TestFile.ResultFilePath + $@"ExtractAudioFromVideo/resultPath.{audioFormat.ToString().ToLowerInvariant()}";
                var resultStreamPath = TestFile.ResultFilePath + $@"ExtractAudioFromVideo/resultStream.{audioFormat.ToString().ToLowerInvariant()}";

                // Test block with physical paths to input and output files
                await _videoProcessor.ExtractAudioFromVideoAsync(new MediaFile(sample),
                                                                 resultPhysicalPath);

                // Block for testing file processing as streams without specifying physical paths
                MemoryStream? ms = null;
                if(moovStartRequiredFormats.Contains(videoFormat))
                    ms = _videoProcessor.SetStartMoovAsync(new MediaFile(sample.ToBytes()), videoFormat).GetAwaiter().GetResult()!;

                var resultStream = await _videoProcessor.ExtractAudioFromVideoAsync(new MediaFile(moovStartRequiredFormats.Contains(videoFormat) ? ms!.ToArray()
                                                                                                      : sample.ToBytes()),
                                                                                    outputFormat: audioFormat);
                resultStream!.ToFile(resultStreamPath);
            }
        }
    }

    public static async Task ConvertVideo()
    {
        List<FileFormatType> results = new()
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
            FileFormatType.WMV
        };

        foreach (var videoFormat in supportedVideoFormats)
        {
            foreach (var outputFormat in results)
            {
                var sample = TestFile.GetPath(videoFormat);
                var resultPhysicalPath = TestFile.ResultFilePath + $@"ConvertVideo/resultPath.{outputFormat.ToString().Replace("_", "").ToLowerInvariant()}";
                var resultStreamPath = TestFile.ResultFilePath + $@"ConvertVideo/resultStream.{outputFormat.ToString().Replace("_", "").ToLowerInvariant()}";

                // Test block with physical paths to input and output files
                await _videoProcessor.ConvertVideoAsync(new MediaFile(sample), resultPhysicalPath);

                // Block for testing file processing as streams without specifying physical paths
                MemoryStream? ms = null;
                if(moovStartRequiredFormats.Contains(videoFormat))
                    ms = _videoProcessor.SetStartMoovAsync(new MediaFile(sample.ToBytes()), videoFormat).GetAwaiter().GetResult()!;

                var resultStream = await _videoProcessor.ConvertVideoAsync(new MediaFile(moovStartRequiredFormats.Contains(videoFormat) ? ms!.ToArray() : sample.ToBytes()),
                                                                           outputFormat: outputFormat);
                resultStream!.ToFile(resultStreamPath);
            }
        }
    }

    public static async Task AddWaterMarkToVideoAsync()
    {
        foreach (var videoFormat in supportedVideoFormats)
        {
            var sample = TestFile.GetPath(videoFormat);
            var png = TestFile.GetPath(FileFormatType.TIFF);
            var resultPhysicalPath = TestFile.ResultFilePath + $@"AddWaterMarkToVideoAsync/resultPath.{videoFormat.ToString().Replace("_", "").ToLowerInvariant()}";
            var resultStreamPath = TestFile.ResultFilePath + $@"AddWaterMarkToVideoAsync/resultStream.{videoFormat.ToString().Replace("_", "").ToLowerInvariant()}";

            // Test block with physical paths to input and output files
            await _videoProcessor.AddWaterMarkToVideoAsync(new MediaFile(sample), new MediaFile(png), PositionType.Up, resultPhysicalPath);

            // Block for testing file processing as streams without specifying physical paths
            MemoryStream? ms = null;
            if(moovStartRequiredFormats.Contains(videoFormat))
                ms = _videoProcessor.SetStartMoovAsync(new MediaFile(sample.ToBytes()), videoFormat).GetAwaiter().GetResult()!;

            var resultStream = await _videoProcessor.AddWaterMarkToVideoAsync(new MediaFile(moovStartRequiredFormats.Contains(videoFormat) ? ms!.ToArray() : sample.ToBytes()),
                                                                              new MediaFile(png.ToBytes()),
                                                                              PositionType.Up,
                                                                              outputFormat: videoFormat);
            resultStream!.ToFile(resultStreamPath);
        }
    }

    public static async Task ExtractVideoFromFileAsync()
    {
        foreach (var videoFormat in supportedVideoFormats)
        {
            var sample = TestFile.GetPath(videoFormat);
            var resultPhysicalPath = TestFile.ResultFilePath + $@"ExtractVideoFromFileAsync/resultPath.{videoFormat.ToString().Replace("_", "").ToLowerInvariant()}";
            var resultStreamPath = TestFile.ResultFilePath + $@"ExtractVideoFromFileAsync/resultStream.{videoFormat.ToString().Replace("_", "").ToLowerInvariant()}";

            // Test block with physical paths to input and output files
            await _videoProcessor.ExtractVideoFromFileAsync(new MediaFile(sample), resultPhysicalPath);

            // Block for testing file processing as streams without specifying physical paths
            MemoryStream? ms = null;
            if(moovStartRequiredFormats.Contains(videoFormat))
                ms = _videoProcessor.SetStartMoovAsync(new MediaFile(sample.ToBytes()), videoFormat).GetAwaiter().GetResult()!;

            var resultStream = await _videoProcessor.ExtractVideoFromFileAsync(new MediaFile(moovStartRequiredFormats.Contains(videoFormat) ? ms!.ToArray() : sample.ToBytes()),
                                                                               outputFormat: videoFormat);
            resultStream!.ToFile(resultStreamPath);
        }
    }

    public static async Task AddAudioToVideoAsync()
    {
        foreach (var videoFormat in supportedVideoFormats)
        {
            var sample = TestFile.GetPath(videoFormat);
            var audio = TestFile.GetPath(FileFormatType.AAC);
            var resultPhysicalPath = TestFile.ResultFilePath + $@"AddAudioToVideoAsync/resultPath.{videoFormat.ToString().Replace("_", "").ToLowerInvariant()}";
            var resultStreamPath = TestFile.ResultFilePath + $@"AddAudioToVideoAsync/resultStream.{videoFormat.ToString().Replace("_", "").ToLowerInvariant()}";

            // Test block with physical paths to input and output files
            await _videoProcessor.AddAudioToVideoAsync(new MediaFile(audio), new MediaFile(sample), resultPhysicalPath);

            // Block for testing file processing as streams without specifying physical paths
            MemoryStream? ms = null;
            if(moovStartRequiredFormats.Contains(videoFormat))
                ms = _videoProcessor.SetStartMoovAsync(new MediaFile(sample.ToBytes()), videoFormat).GetAwaiter().GetResult()!;

            var resultStream = await _videoProcessor.AddAudioToVideoAsync(new MediaFile(audio.ToFileStream()),
                                                                          new MediaFile(moovStartRequiredFormats.Contains(videoFormat) ? ms!.ToArray() : sample.ToBytes()),
                                                                          outputFormat: videoFormat);
            resultStream!.ToFile(resultStreamPath);
        }
    }

    public static async Task ConvertVideoToGifAsync()
    {
        foreach (var videoFormat in supportedVideoFormats)
        {
            var sample = TestFile.GetPath(videoFormat);
            var audio = TestFile.GetPath(FileFormatType.AAC);
            var resultPhysicalPath = TestFile.ResultFilePath + $@"ConvertVideoToGifAsync/resultPath.{FileFormatType.GIF.ToString().Replace("_", "").ToLowerInvariant()}";
            var resultStreamPath = TestFile.ResultFilePath + $@"ConvertVideoToGifAsync/resultStream.{FileFormatType.GIF.ToString().Replace("_", "").ToLowerInvariant()}";

            // Test block with physical paths to input and output files
            await _videoProcessor.ConvertVideoToGifAsync(new MediaFile(sample), 5, 320, 0, resultPhysicalPath);

            // Block for testing file processing as streams without specifying physical paths
            MemoryStream? ms = null;
            if(moovStartRequiredFormats.Contains(videoFormat))
                ms = _videoProcessor.SetStartMoovAsync(new MediaFile(sample.ToBytes()), videoFormat).GetAwaiter().GetResult()!;

            var resultStream = await _videoProcessor.ConvertVideoToGifAsync(new MediaFile(moovStartRequiredFormats.Contains(videoFormat) ? ms!.ToArray() : sample.ToBytes()),
                                                                            5,
                                                                            320,
                                                                            0);
            resultStream!.ToFile(resultStreamPath);
        }
    }

    public static async Task CompressVideoAsync()
    {
        foreach (var videoFormat in supportedVideoFormats)
        {
            var sample = TestFile.GetPath(videoFormat);
            var resultPhysicalPath = TestFile.ResultFilePath + $@"CompressVideoAsync/resultPath.{videoFormat.ToString().Replace("_", "").ToLowerInvariant()}";
            var resultStreamPath = TestFile.ResultFilePath + $@"CompressVideoAsync/resultStream.{videoFormat.ToString().Replace("_", "").ToLowerInvariant()}";

            // Test block with physical paths to input and output files
            await _videoProcessor.CompressVideoAsync(new MediaFile(sample), 50, resultPhysicalPath);

            // Block for testing file processing as streams without specifying physical paths
            MemoryStream? ms = null;
            if(moovStartRequiredFormats.Contains(videoFormat))
                ms = _videoProcessor.SetStartMoovAsync(new MediaFile(sample.ToBytes()), videoFormat).GetAwaiter().GetResult()!;

            var resultStream = await _videoProcessor.CompressVideoAsync(new MediaFile(moovStartRequiredFormats.Contains(videoFormat) ? ms!.ToArray() : sample.ToBytes()),
                                                                        50);
            resultStream!.ToFile(resultStreamPath);
        }
    }

    public static async Task ConcatVideosAsync()
    {
        List<FileFormatType> results = new()
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
            FileFormatType.WMV
        };

        foreach (var videoFormat in supportedVideoFormats)
        {
            var sample = TestFile.GetPath(videoFormat);
            var resultPhysicalPath = TestFile.ResultFilePath + $@"ConcatVideosAsync/resultPath.{videoFormat.ToString().Replace("_", "").ToLowerInvariant()}";
            var resultStreamPath = TestFile.ResultFilePath + $@"ConcatVideosAsync/resultStream.{videoFormat.ToString().Replace("_", "").ToLowerInvariant()}";

            var files = new List<MediaFile>();

            // foreach (var i in results)
            //     files.Add(new MediaFile(TestFile.GetPath(i)));

            // Test block with physical paths to input and output files
            // await _videoProcessor.ConcatVideosAsync(files.ToArray(), resultPhysicalPath);

            // Block for testing file processing as streams without specifying physical paths

            foreach (var i in results)
            {
                MemoryStream? ms = null;
                if(moovStartRequiredFormats.Contains(i))
                    ms = _videoProcessor.SetStartMoovAsync(new MediaFile(TestFile.GetPath(i).ToBytes()), i).GetAwaiter().GetResult()!;

                files.Add(new MediaFile(moovStartRequiredFormats.Contains(i) ? ms!.ToArray() : TestFile.GetPath(i).ToBytes()));
            }

            var resultStream = await _videoProcessor.ConcatVideosAsync(files.ToArray(), outputFormat: videoFormat);
            resultStream!.ToFile(resultStreamPath);
        }
    }
}