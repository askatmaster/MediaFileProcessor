using MediaFileProcessor.Extensions;
using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Enums;
using MediaFileProcessor.Models.Settings;
namespace MediaFileProcessor.Processors;

public class VideoFileProcessor
{
    private readonly string _ffmpeg = "ffmpeg.exe";
    private readonly string _ffprobe = "ffprobe.exe";

    private async Task<MemoryStream?> ExecuteAsync(VideoProcessingSettings settings, CancellationToken cancellationToken)
    {
        var processArguments = settings.GetProcessArguments();

        var process = new MediaFileProcess(_ffmpeg, processArguments, settings, settings.GetInputStreams(), settings.IsStandartOutputRedirect, settings.GetInputPipeNames());

        return await process.ExecuteAsync(cancellationToken);
    }

    //======================================================================================================================================================================

    private async Task<MemoryStream?> ExecuteGetFrameFromVideoAsync(TimeSpan timestamp, MediaFile file, string? outputFile, FileFormatType outputFormat, CancellationToken cancellationToken)
    {
        var settings = new VideoProcessingSettings().ReplaceIfExist()
                                                    .Seek(timestamp)
                                                    .SetInputFiles(file)
                                                    .FramesNumber(1)
                                                    .Format(outputFormat)
                                                    .SetOutputArguments(outputFile);


        return await ExecuteAsync(settings, cancellationToken);
    }

    public  async Task GetFrameFromVideoAsync(TimeSpan timestamp, MediaFile file, string outputFile, FileFormatType outputFormat, CancellationToken? cancellationToken = null)
    {
        await ExecuteGetFrameFromVideoAsync(timestamp, file, outputFile, outputFormat, cancellationToken ?? new CancellationToken());
    }
    public  async Task<MemoryStream> GetFrameFromVideoAsStreamAsync(TimeSpan timestamp, MediaFile file, FileFormatType outputFormat, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteGetFrameFromVideoAsync(timestamp, file, null, outputFormat, cancellationToken ?? new CancellationToken()))!;
    }
    public  async Task<byte[]> GetFrameFromVideoAsBytesAsync(TimeSpan timestamp, MediaFile file, FileFormatType outputFormat, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteGetFrameFromVideoAsync(timestamp, file, null, outputFormat, cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    //======================================================================================================================================================================

    private  async Task<MemoryStream?> ExecuteCutVideoAsync(TimeSpan startTime, TimeSpan endTime, MediaFile file, string? outputFile, FileFormatType outputFormat, CancellationToken cancellationToken)
    {
        var settings = new VideoProcessingSettings().ReplaceIfExist()
                                                    .SetInputFiles(file)
                                                    .Seek(startTime)
                                                    .TimePosition(endTime)
                                                    .CopyAllCodec()
                                                    .Format(outputFormat)
                                                    .SetOutputArguments(outputFile);

        return await ExecuteAsync(settings, cancellationToken);
    }

    public  async Task CutVideoAsync(TimeSpan startTime, TimeSpan endTime,  MediaFile file, string outputFile, FileFormatType outputFormat, CancellationToken? cancellationToken = null)
    {
        await ExecuteCutVideoAsync(startTime, endTime, file, outputFile, outputFormat, cancellationToken ?? new CancellationToken());
    }
    public  async Task<MemoryStream> CutVideoAsStreamAsync(TimeSpan startTime, TimeSpan endTime, MediaFile file, FileFormatType outputFormat, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteCutVideoAsync(startTime, endTime, file, null, outputFormat, cancellationToken ?? new CancellationToken()))!;
    }
    public  async Task<byte[]> CutVideoAsBytesAsync(TimeSpan startTime, TimeSpan endTime, MediaFile file, FileFormatType outputFormat, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteCutVideoAsync(startTime, endTime, file, null, outputFormat, cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    //======================================================================================================================================================================

    private  async Task<MemoryStream?> ExecuteConvertImagesToVideoAsync(MediaFile file, int frameRate, VideoCodecType videoCodecType, FileFormatType? outputFileFormatType, string? outputFile, string pixelFormat, CancellationToken cancellationToken)
    {
        var settings = new VideoProcessingSettings().ReplaceIfExist()
                                                    .FrameRate(frameRate)
                                                    .SetInputFiles(file)
                                                    .VideoCodec(videoCodecType)
                                                    .PixelFormat(pixelFormat)
                                                    .SetOutputArguments(outputFile);
        if(outputFileFormatType is not null)
            settings.Format(outputFileFormatType.Value);

        return await ExecuteAsync(settings, cancellationToken);
    }

    public  async Task ConvertImagesToVideoAsync(MediaFile file, int frameRate, string outputFile, string pixelFormat, FileFormatType outputFileFormatType, CancellationToken? cancellationToken = null, VideoCodecType videoCodecType = VideoCodecType.LIBX264)
    {
        await ExecuteConvertImagesToVideoAsync(file, frameRate, videoCodecType, outputFileFormatType, outputFile, pixelFormat,  cancellationToken ?? new CancellationToken());
    }
    public  async Task<MemoryStream> ConvertImagesToVideoAsStreamAsync(MediaFile file, int frameRate, string pixelFormat, FileFormatType outputFileFormatType, VideoCodecType videoCodecType = VideoCodecType.LIBX264, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConvertImagesToVideoAsync(file, frameRate, videoCodecType, outputFileFormatType, null, pixelFormat, cancellationToken ?? new CancellationToken()))!;
    }
    public  async Task<byte[]> ConvertImagesToVideoAsBytesAsync(MediaFile file, int frameRate, string pixelFormat, FileFormatType? outputFileFormatType, VideoCodecType videoCodecType = VideoCodecType.LIBX264, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConvertImagesToVideoAsync(file, frameRate, videoCodecType, outputFileFormatType, null, pixelFormat, cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    //======================================================================================================================================================================

    private  async Task<MemoryStream?> ExecuteConvertVideoToImagesAsync(MediaFile file, FileFormatType? outputFileFormatType, string? outputImagesPattern, CancellationToken cancellationToken)
    {
        var settings = new VideoProcessingSettings().ReplaceIfExist().SetInputFiles(file).SetOutputArguments(outputImagesPattern);

        switch(outputFileFormatType)
        {
            case FileFormatType.JPG:
            case FileFormatType.IMAGE2PIPE:
            case FileFormatType.IMAGE2:
            case FileFormatType.JPEG:
                settings.Format(FileFormatType.IMAGE2PIPE);
                break;
            case FileFormatType.PNG:
                settings.VideoCodec(VideoCodecType.PNG);
                settings.Format(FileFormatType.RAWVIDEO);
                break;
            case FileFormatType.BMP:
                settings.VideoCodec(VideoCodecType.PNG);
                settings.Format(FileFormatType.RAWVIDEO);
                break;
            default:
                throw new NotSupportedException("This output format is not supported");
        }

        return await ExecuteAsync(settings, cancellationToken);
    }

    public  async Task ConvertVideoToImagesAsync(MediaFile file, FileFormatType outputFormatType, string outputImagesPattern, CancellationToken? cancellationToken = null)
    {
        await ExecuteConvertVideoToImagesAsync(file, outputFormatType, outputImagesPattern,  cancellationToken ?? new CancellationToken());
    }
    public  async Task<MultiStream> ConvertVideoToImagesAsStreamAsync(MediaFile file, FileFormatType outputFormatType, CancellationToken? cancellationToken = null)
    {
        var stream = (await ExecuteConvertVideoToImagesAsync(file, outputFormatType, null,  cancellationToken ?? new CancellationToken()))!;

        return stream.GetMultiStreamBySignature(FilesSignatures.GetSignature(outputFormatType));
    }
    public  async Task<byte[][]> ConvertVideoToImagesAsBytesAsync(MediaFile file, FileFormatType outputFormatType, CancellationToken? cancellationToken = null)
    {
        var stream = (await ExecuteConvertVideoToImagesAsync(file, outputFormatType, null,  cancellationToken ?? new CancellationToken()))!;
        return stream.GetMultiStreamBySignature(FilesSignatures.GetSignature(outputFormatType)).ReadAsDataArray();
    }

    //======================================================================================================================================================================

    private  async Task<MemoryStream?> ExecuteGetAudioFromVideoAsync(MediaFile file, FileFormatType outputFileFormatType, AudioSampleRateType audioSampleRateType, int audioChannel, string? output, CancellationToken cancellationToken)
    {
        var settings = new VideoProcessingSettings().ReplaceIfExist()
                                                    .SetInputFiles(file)
                                                    .DeleteVideo()
                                                    .AudioSampleRate(audioSampleRateType)
                                                    .AudioChannel(audioChannel)
                                                    .Format(outputFileFormatType)
                                                    .SetOutputArguments(output);

        return await ExecuteAsync(settings, cancellationToken);
    }

    public  async Task GetAudioFromVideoAsync(MediaFile file, FileFormatType outputFileFormatType, string output, int audioChannel = 2, CancellationToken? cancellationToken = null, AudioSampleRateType audioSampleRateType = AudioSampleRateType.Hz44100)
    {
        await ExecuteGetAudioFromVideoAsync(file, outputFileFormatType, audioSampleRateType, audioChannel, output,  cancellationToken ?? new CancellationToken());
    }

    public  async Task<MemoryStream> GetAudioFromVideoAsStreamAsync(MediaFile file, FileFormatType outputFileFormatType, int audioChannel = 2, CancellationToken? cancellationToken = null, AudioSampleRateType audioSampleRateType = AudioSampleRateType.Hz44100)
    {
        return (await ExecuteGetAudioFromVideoAsync(file, outputFileFormatType, audioSampleRateType, audioChannel, null,  cancellationToken ?? new CancellationToken()))!;
    }

    public  async Task<byte[]> GetAudioFromVideoAsBytesAsync(MediaFile file, FileFormatType outputFileFormatType, int audioChannel = 2, CancellationToken? cancellationToken = null, AudioSampleRateType audioSampleRateType = AudioSampleRateType.Hz44100)
    {
        return (await ExecuteGetAudioFromVideoAsync(file, outputFileFormatType, audioSampleRateType, audioChannel, null,  cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    //======================================================================================================================================================================

    private  async Task<MemoryStream?> ExecuteConvertVideoAsync( MediaFile file, string? output, FileFormatType outputFileFormatType, CancellationToken cancellationToken)
    {
        return await ExecuteAsync(new VideoProcessingSettings().ReplaceIfExist()
                                                               .SetInputFiles(file)
                                                               .MapMetadata()
                                                               .Format(outputFileFormatType)
                                                               .SetOutputArguments(output),
                                  cancellationToken);
    }

    public  async Task ConvertVideoAsync(MediaFile file, string output, FileFormatType outputFileFormatType, CancellationToken? cancellationToken = null)
    {
        await ExecuteConvertVideoAsync(file, output, outputFileFormatType, cancellationToken ?? new CancellationToken());
    }

    public  async Task<MemoryStream> ConvertVideoAsStreamAsync(MediaFile file, FileFormatType outputFileFormatType, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConvertVideoAsync(file, null, outputFileFormatType, cancellationToken ?? new CancellationToken()))!;
    }

    public  async Task<byte[]> ConvertVideoAsBytesAsync(MediaFile file, FileFormatType outputFileFormatType, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConvertVideoAsync(file, null, outputFileFormatType, cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    //======================================================================================================================================================================

    private  async Task<MemoryStream?> ExecuteAddWaterMarkToVideoAsync(MediaFile videoFile, MediaFile watermarkFile, PositionType position, string? output, FileFormatType outputFileFormatType, CancellationToken cancellationToken)
    {
        var positionArgumant = string.Empty;

        positionArgumant += position switch
        {
            PositionType.Bottom => "(main_w-overlay_w)/2:main_h-overlay_h",
            PositionType.Center => "x=(main_w-overlay_w)/2:y=(main_h-overlay_h)/2",
            PositionType.BottomLeft => "5:main_h-overlay_h",
            PositionType.UpperLeft => "5:5",
            PositionType.BottomRight => "(main_w-overlay_w):main_h-overlay_h",
            PositionType.UpperRight => "(main_w-overlay_w):5",
            PositionType.Left => "5:(main_h-overlay_h)/2",
            PositionType.Right => "(main_w-overlay_w-5):(main_h-overlay_h)/2",
            PositionType.Up => "(main_w-overlay_w)/2:5",
            _ => throw new ArgumentOutOfRangeException(nameof(position), position, null)
        };

        var settings = new VideoProcessingSettings().ReplaceIfExist()
                                                    .SetInputFiles(videoFile, watermarkFile)
                                                    .FilterComplexArgument(positionArgumant)
                                                    .Format(outputFileFormatType)
                                                    .SetOutputArguments(output);

        return await ExecuteAsync(settings, cancellationToken);
    }

    public  async Task AddWaterMarkToVideoAsync(MediaFile videoFile, MediaFile watermarkFile, PositionType position, string output, FileFormatType outputFileFormatType, CancellationToken? cancellationToken = null)
    {
        await ExecuteAddWaterMarkToVideoAsync(videoFile, watermarkFile, position, output, outputFileFormatType, cancellationToken ?? new CancellationToken());
    }

    public  async Task<MemoryStream> AddWaterMarkToVideoAsStreamAsync(MediaFile videoFile, MediaFile watermarkFile, PositionType position, FileFormatType outputFileFormatType, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteAddWaterMarkToVideoAsync(videoFile, watermarkFile, position, null, outputFileFormatType, cancellationToken ?? new CancellationToken()))!;
    }

    public  async Task<byte[]> AddWaterMarkToVideoAsBytesAsync(MediaFile videoFile, MediaFile watermarkFile, PositionType position, FileFormatType outputFileFormatType, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteAddWaterMarkToVideoAsync(videoFile, watermarkFile, position, null, outputFileFormatType, cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    //======================================================================================================================================================================

    private  async Task<MemoryStream?> ExecuteExtractVideoFromFileAsync(MediaFile file,
                                                                        string? output,
                                                                        FileFormatType outputFileFormatType,
                                                                        VideoCodecType videoCodecType,
                                                                        string pixelFormat,
                                                                        CancellationToken cancellationToken)
    {
        var settings = new VideoProcessingSettings().ReplaceIfExist()
                                                    .SetInputFiles(file)
                                                    .MapMetadata()
                                                    .MapArgument("0:v:0")
                                                    .VideoCodec(videoCodecType)
                                                    .PixelFormat(pixelFormat)
                                                    .Format(outputFileFormatType)
                                                    .SetOutputArguments(output);

        return await ExecuteAsync(settings, cancellationToken);
    }

    public  async Task ExtractVideoFromFileAsync(MediaFile file, string output, FileFormatType outputFileFormatType, VideoCodecType videoCodecType = VideoCodecType.COPY, string pixelFormat = "yuv420p", CancellationToken? cancellationToken = null)
    {
        await ExecuteExtractVideoFromFileAsync(file, output, outputFileFormatType, videoCodecType, pixelFormat, cancellationToken ?? new CancellationToken());
    }

    public  async Task<MemoryStream> ExtractVideoFromFileAsStreamAsync(MediaFile file, FileFormatType outputFileFormatType, VideoCodecType videoCodecType = VideoCodecType.COPY, string pixelFormat = "yuv420p", CancellationToken? cancellationToken = null)
    {
        return (await ExecuteExtractVideoFromFileAsync(file, null, outputFileFormatType, videoCodecType, pixelFormat, cancellationToken ?? new CancellationToken()))!;
    }

    public  async Task<byte[]> ExtractVideoFromFileAsBytesAsync(MediaFile file, FileFormatType outputFileFormatType, VideoCodecType videoCodecType = VideoCodecType.COPY, string pixelFormat = "yuv420p", CancellationToken? cancellationToken = null)
    {
        return (await ExecuteExtractVideoFromFileAsync(file, null, outputFileFormatType, videoCodecType, pixelFormat, cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    //======================================================================================================================================================================

    private  async Task<MemoryStream?> ExecuteAddAudioToVideoAsync(MediaFile audioFile, MediaFile videofile, string? output, VideoCodecType videoCodecType, AudioCodecType audioCodecType, FileFormatType outputFileFormatType, CancellationToken cancellationToken)
    {
        var settings = new VideoProcessingSettings().ReplaceIfExist()
                                                    .SetInputFiles(audioFile, videofile)
                                                    .VideoCodec(videoCodecType)
                                                    .AudioCodec(audioCodecType)
                                                    .Format(outputFileFormatType)
                                                    .SetOutputArguments(output);

        return await ExecuteAsync(settings, cancellationToken);
    }

    public  async Task AddAudioToVideoAsync(MediaFile audioFile,
                                            MediaFile videofile,
                                            string output,
                                            FileFormatType outputFileFormatType,
                                            CancellationToken? cancellationToken = null,
                                            AudioCodecType audioCodecType = AudioCodecType.COPY,
                                            VideoCodecType videoCodecType = VideoCodecType.COPY)
    {
        await ExecuteAddAudioToVideoAsync(audioFile, videofile, output, videoCodecType, audioCodecType, outputFileFormatType, cancellationToken ?? new CancellationToken());
    }

    public  async Task<MemoryStream> AddAudioToVideoAsStreamAsync(MediaFile audioFile,
                                                                  MediaFile videofile,
                                                                  FileFormatType outputFileFormatType,
                                                                  CancellationToken? cancellationToken = null,
                                                                  AudioCodecType audioCodecType = AudioCodecType.COPY,
                                                                  VideoCodecType videoCodecType = VideoCodecType.COPY)
    {
        return (await ExecuteAddAudioToVideoAsync(audioFile,
                                                  videofile,
                                                  null,
                                                  videoCodecType,
                                                  audioCodecType,
                                                  outputFileFormatType,
                                                  cancellationToken ?? new CancellationToken()))!;
    }

    public  async Task<byte[]> AddAudioToVideoAsBytesAsync(MediaFile audioFile,
                                                           MediaFile videofile,
                                                           FileFormatType outputFileFormatType,
                                                           CancellationToken? cancellationToken = null,
                                                           AudioCodecType audioCodecType = AudioCodecType.COPY,
                                                           VideoCodecType videoCodecType = VideoCodecType.COPY)
    {
        return (await ExecuteAddAudioToVideoAsync(audioFile, videofile, null, videoCodecType, audioCodecType, outputFileFormatType, cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    //======================================================================================================================================================================

    private  async Task<MemoryStream?> ExecuteConvertVideoToGifAsync(MediaFile file, int fps, int scale, int loop, string? output, CancellationToken cancellationToken)
    {
        var setting = new VideoProcessingSettings().ReplaceIfExist()
                                                   .SetInputFiles(file)
                                                   .CustomArguments($"-vf \"fps={fps},scale={scale}:-1:flags=lanczos,split[s0][s1];[s0]palettegen[p];[s1][p]paletteuse\"")
                                                   .Loop(loop)
                                                   .Format(FileFormatType.GIF)
                                                   .SetOutputArguments(output);

        return await ExecuteAsync(setting, cancellationToken);
    }

    public  async Task ConvertVideoToGifAsync(MediaFile file, int fps, int scale, int loop, string output, CancellationToken? cancellationToken = null)
    {
        await ExecuteConvertVideoToGifAsync(file, fps, scale, loop, output, cancellationToken ?? new CancellationToken());
    }

    public  async Task<MemoryStream> ConvertVideoToGifAsStreamAsync(MediaFile file, int fps, int scale, int loop, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConvertVideoToGifAsync(file, fps, scale, loop, null, cancellationToken ?? new CancellationToken()))!;
    }

    public  async Task<byte[]> ConvertVideoToGifAsBytesAsync(MediaFile file, int fps, int scale, int loop, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConvertVideoToGifAsync(file, fps, scale, loop, null, cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    //======================================================================================================================================================================

    private  async Task<MemoryStream?> ExecuteCompressVideoAsync(MediaFile file, int crf, string? output, FileFormatType outputFileFormatType, VideoCodecType videoCodecType, CancellationToken cancellationToken)
    {
        var setting = new VideoProcessingSettings().ReplaceIfExist()
                                                   .SetInputFiles(file)
                                                   .VideoCodec(videoCodecType)
                                                   .Crf(crf)
                                                   .Format(outputFileFormatType)
                                                   .SetOutputArguments(output);
        return await ExecuteAsync(setting, cancellationToken);
    }

    public  async Task CompressVideoAsync(MediaFile file, int compressionRatio, string output, FileFormatType outputFileFormatType, VideoCodecType videoCodecType = VideoCodecType.LIBX264, CancellationToken? cancellationToken = null)
    {
        await ExecuteCompressVideoAsync(file, compressionRatio, output, outputFileFormatType, videoCodecType, cancellationToken ?? new CancellationToken());
    }

    public  async Task<MemoryStream> CompressVideoAsStreamAsync(MediaFile file, int compressionRatio, FileFormatType outputFileFormatType, VideoCodecType videoCodecType = VideoCodecType.LIBX264, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteCompressVideoAsync(file, compressionRatio, null, outputFileFormatType, videoCodecType, cancellationToken ?? new CancellationToken()))!;
    }

    public  async Task<byte[]> CompressVideoAsBytesAsync(MediaFile file, int compressionRatio, FileFormatType outputFileFormatType, VideoCodecType videoCodecType = VideoCodecType.LIBX264, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteCompressVideoAsync(file, compressionRatio, null, outputFileFormatType, videoCodecType, cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    //======================================================================================================================================================================

    private  async Task<MemoryStream?> ExecuteCompressImageAsync(MediaFile file, int level, string? output, FileFormatType outputFileFormatType, CancellationToken cancellationToken)
    {
        var setting = new VideoProcessingSettings().ReplaceIfExist()
                                                   .HideBanner()
                                                   .SetInputFiles(file)
                                                   .CompressionLevel(level)
                                                   .Format(outputFileFormatType)
                                                   .SetOutputArguments(output);

        return await ExecuteAsync(setting, cancellationToken);
    }

    public  async Task CompressImageAsync(MediaFile file, int level, string output, FileFormatType outputFileFormatType, CancellationToken? cancellationToken = null)
    {
        await ExecuteCompressImageAsync(file, level, output, outputFileFormatType, cancellationToken ?? new CancellationToken());
    }

    public  async Task<MemoryStream> CompressImageAsStreamAsync(MediaFile file, int level, FileFormatType outputFileFormatType, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteCompressImageAsync(file, level, null, outputFileFormatType, cancellationToken ?? new CancellationToken()))!;
    }

    public  async Task<byte[]> CompressImageAsBytesAsync(MediaFile file, int level, FileFormatType outputFileFormatType, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteCompressImageAsync(file, level, null, outputFileFormatType, cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    //======================================================================================================================================================================


    private  async Task CreateMpegts(MediaFile inputFile, string output, VideoCodecType videoCodecType, AudioCodecType audioCodecType, string videoBSF, CancellationToken cancellationToken)
    {
        var setting = new VideoProcessingSettings().ReplaceIfExist()
                                                   .CustomArguments("-fflags +genpts")
                                                   .SetInputFiles(inputFile)
                                                   .VideoCodec(videoCodecType)
                                                   .VideoBSF(videoBSF)
                                                   .AudioCodec(audioCodecType)
                                                   .Format(FileFormatType.MPEGTS)
                                                   .SetOutputArguments(output);

        await ExecuteAsync(setting, cancellationToken);
    }

    private  async Task<MemoryStream?> ExecuteConcatVideosAsync(MediaFile[] files, string? outputFile, VideoCodecType videoCodecType, AudioCodecType audioCodecType, string videoBSF, FileFormatType outputFileFormatType, CancellationToken cancellationToken)
    {
        var intermediateFiles = new List<string>();

        for (var i = 0; i < files.Length; i++)
        {
            var outputFileName = $"_{i}{Guid.NewGuid()}.ts";
            intermediateFiles.Add(outputFileName);
            await CreateMpegts(files[i], outputFileName, videoCodecType, audioCodecType, videoBSF, cancellationToken);
        }

        var strCmd = " -i \"concat:";

        foreach (var i in intermediateFiles)
        {
            strCmd += i;
            strCmd += "|";
        }

        strCmd = strCmd.Remove(strCmd.Length - 1) + "\" ";

        var setting = new VideoProcessingSettings().ReplaceIfExist()
                                                   .CustomArguments(strCmd)
                                                   .CopyAllCodec()
                                                   .AudioBSF("aac_adtstoasc")
                                                   .MovFralgs("+faststart")
                                                   .Format(outputFileFormatType)
                                                   .SetOutputArguments(outputFile);

        var result = await ExecuteAsync(setting, cancellationToken);

        foreach (var file in intermediateFiles)
        {
            if(File.Exists(file))
                File.Delete(file);
        }

        return result;
    }

    public  async Task ConcatVideosAsync(MediaFile[] files,
                                         string output,
                                         FileFormatType outputFileFormatType,
                                         string videoBSF = "h264_mp4toannexb",
                                         CancellationToken? cancellationToken = null,
                                         VideoCodecType videoCodecType = VideoCodecType.LIBX264,
                                         AudioCodecType audioCodecType = AudioCodecType.AAC)
    {
        await ExecuteConcatVideosAsync(files, output, videoCodecType, audioCodecType, videoBSF, outputFileFormatType, cancellationToken ?? new CancellationToken());
    }

    public  async Task<MemoryStream> ConcatVideosAsStreamAsync(MediaFile[] files,
                                                               FileFormatType outputFileFormatType,
                                                               string videoBSF = "h264_mp4toannexb",
                                                               VideoCodecType videoCodecType = VideoCodecType.LIBX264,
                                                               AudioCodecType audioCodecType = AudioCodecType.AAC,
                                                               CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConcatVideosAsync(files, null, videoCodecType, audioCodecType, videoBSF, outputFileFormatType, cancellationToken ?? new CancellationToken()))!;
    }

    public  async Task<byte[]> ConcatVideosAsBytesAsync(MediaFile[] files,
                                                        FileFormatType outputFileFormatType,
                                                        string videoBSF = "h264_mp4toannexb",
                                                        CancellationToken? cancellationToken = null,
                                                        VideoCodecType videoCodecType = VideoCodecType.LIBX264,
                                                        AudioCodecType audioCodecType = AudioCodecType.AAC)
    {
        return (await ExecuteConcatVideosAsync(files, null, videoCodecType, audioCodecType, videoBSF, outputFileFormatType, cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    //======================================================================================================================================================================

    public async Task<string> GetVideoInfo(string videoFile, CancellationToken? cancellationToken = null)
    {
        var settings = new VideoProcessingSettings().CustomArguments("-v panic -print_format json=c=1 -show_streams -show_entries "
                                                                   + $"format=size,duration,bit_rate:format_tags=creation_time {videoFile}");

        var process = new MediaFileProcess(_ffprobe, settings.GetProcessArguments(), settings, redirectOutputToStream: true);

        var result = await process.ExecuteAsync(cancellationToken ?? new CancellationToken());

        var reader = new StreamReader(result!);

        var output = await reader.ReadToEndAsync();

        return output;
    }

    //======================================================================================================================================================================

    private  async Task<MemoryStream?> ExecuteAddHardSubtitlesAsync(MediaFile videoFile, MediaFile subsFile, string language, string? outputFile, CancellationToken cancellationToken)
    {
        var setting = new VideoProcessingSettings().ReplaceIfExist()
                                                   .SetInputFiles(videoFile, subsFile)
                                                   .MapArgument("0")
                                                   .MapArgument("1")
                                                   .CopyAllCodec()
                                                   .SubtitlesCodec("mov_text")
                                                   .MetaData("s:s:1")
                                                   .Language(language)
                                                   .SetOutputArguments(outputFile);

        return await ExecuteAsync(setting, cancellationToken);
    }

    public  async Task AddHardSubtitlesAsync(MediaFile videoFile, MediaFile subsFile, string language, string outputFile, CancellationToken? cancellationToken = null)
    {
        await ExecuteAddHardSubtitlesAsync(videoFile, subsFile, language, outputFile, cancellationToken ?? new CancellationToken());
    }

    public  async Task<MemoryStream> AddHardSubtitlesAsStreamAsync(MediaFile videoFile, MediaFile subsFile, string language, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteAddHardSubtitlesAsync(videoFile, subsFile, language, null, cancellationToken ?? new CancellationToken()))!;
    }

    public  async Task<byte[]> AddHardSubtitlesAsBytesAsync(MediaFile videoFile, MediaFile subsFile, string language, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteAddHardSubtitlesAsync(videoFile, subsFile, language, null, cancellationToken ?? new CancellationToken()))!.ToArray();
    }
}