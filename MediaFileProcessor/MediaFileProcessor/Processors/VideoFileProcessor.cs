using MediaFileProcessor.Extensions;
using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Enums;
using MediaFileProcessor.Models.Settings;
namespace MediaFileProcessor.Processors;

public class VideoFileProcessor
{
    private static readonly string _ffmpeg = "ffmpeg.exe";
    private static readonly string _ffprobe = "ffprobe.exe";

    private static async Task<MemoryStream?> ExecuteAsync(VideoProcessingSettings settings, CancellationToken cancellationToken)
    {
        var processArguments = settings.GetProcessArguments();

        var process = new MediaFileProcess(_ffmpeg, processArguments, settings, settings.GetInputStreams(), settings.IsStandartOutputRedirect, settings.GetInputPipeNames());

        return await process.ExecuteAsync(cancellationToken);
    }

    //======================================================================================================================================================================

    private static async Task<MemoryStream?> ExecuteGetFrameFromVideoAsync(TimeSpan timestamp, MediaFile file, string? outputFile, CancellationToken cancellationToken)
    {
        var settings = new VideoProcessingSettings().ReplaceIfExist()
                                                    .Seek(timestamp)
                                                    .SetInputFiles(file)
                                                    .FramesNumber(1)
                                                    .SetOutputArguments(outputFile);


        return await ExecuteAsync(settings, cancellationToken);
    }

    public static async Task GetFrameFromVideoAsync(TimeSpan timestamp, MediaFile file, string outputFile, CancellationToken? cancellationToken = null)
    {
        await ExecuteGetFrameFromVideoAsync(timestamp, file, outputFile, cancellationToken ?? new CancellationToken());
    }
    public static async Task<MemoryStream> GetFrameFromVideoAsStreamAsync(TimeSpan timestamp, MediaFile file, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteGetFrameFromVideoAsync(timestamp, file, null, cancellationToken ?? new CancellationToken()))!;
    }
    public static async Task<byte[]> GetFrameFromVideoAsBytesAsync(TimeSpan timestamp, MediaFile file, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteGetFrameFromVideoAsync(timestamp, file, null, cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    //======================================================================================================================================================================

    private static async Task<MemoryStream?> ExecuteCutVideoAsync(TimeSpan startTime, TimeSpan endTime, MediaFile file, string? outputFile, CancellationToken cancellationToken)
    {
        var settings = new VideoProcessingSettings().ReplaceIfExist()
                                                    .Seek(startTime)
                                                    .TimePosition(endTime)
                                                    .SetInputFiles(file)
                                                    .CopyAllCodec()
                                                    .MapMetadata()
                                                    .SetOutputArguments(outputFile);

        return await ExecuteAsync(settings, cancellationToken);
    }

    public static async Task CutVideoAsync(TimeSpan startTime, TimeSpan endTime,  MediaFile file, string outputFile, CancellationToken? cancellationToken = null)
    {
        await ExecuteCutVideoAsync(startTime, endTime, file, outputFile, cancellationToken ?? new CancellationToken());
    }
    public static async Task<MemoryStream> CutVideoAsStreamAsync(TimeSpan startTime, TimeSpan endTime, MediaFile file, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteCutVideoAsync(startTime, endTime, file, null, cancellationToken ?? new CancellationToken()))!;
    }
    public static async Task<byte[]> CutVideoAsBytesAsync(TimeSpan startTime, TimeSpan endTime, MediaFile file, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteCutVideoAsync(startTime, endTime, file, null, cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    //======================================================================================================================================================================

    private static async Task<MemoryStream?> ExecuteConvertImagesToVideoAsync(MediaFile file, int frameRate, VideoCodecType videoCodecType, FileFormatType? outputFileFormatType, string? outputFile, CancellationToken cancellationToken)
    {
        var settings = new VideoProcessingSettings().ReplaceIfExist()
                                                    .FrameRate(frameRate)
                                                    .SetInputFiles(file)
                                                    .VideoCodec(videoCodecType)
                                                    .PixelFormat("yuv420p")
                                                    .SetOutputArguments(outputFile);
        if(outputFileFormatType is not null)
            settings.Format(outputFileFormatType.Value);

        return await ExecuteAsync(settings, cancellationToken);
    }

    public static async Task ConvertImagesToVideoAsync(MediaFile file, int frameRate, string outputFile, CancellationToken? cancellationToken = null, VideoCodecType videoCodecType = VideoCodecType.LIBX264)
    {
        await ExecuteConvertImagesToVideoAsync(file, frameRate, videoCodecType, null, outputFile,  cancellationToken ?? new CancellationToken());
    }
    public static async Task<MemoryStream> ConvertImagesToVideoAsStreamAsync(MediaFile file, int frameRate, FileFormatType outputFileFormatType, VideoCodecType videoCodecType = VideoCodecType.LIBX264, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConvertImagesToVideoAsync(file, frameRate, videoCodecType, outputFileFormatType, null, cancellationToken ?? new CancellationToken()))!;
    }
    public static async Task<byte[]> ConvertImagesToVideoAsBytesAsync(MediaFile file, int frameRate, FileFormatType? outputFileFormatType, VideoCodecType videoCodecType = VideoCodecType.LIBX264, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConvertImagesToVideoAsync(file, frameRate, videoCodecType, outputFileFormatType, null, cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    //======================================================================================================================================================================

    private static async Task<MemoryStream?> ExecuteConvertVideoToImagesAsync(MediaFile file, FileFormatType? outputFileFormatType, string? outputImagesPattern, CancellationToken cancellationToken)
    {
        var settings = new VideoProcessingSettings().ReplaceIfExist().SetInputFiles(file).SetOutputArguments(outputImagesPattern);

        switch(outputFileFormatType)
        {
            case FileFormatType.JPG:
            case FileFormatType.JPEG:
                settings.Format(outputFileFormatType.Value);
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

    public static async Task ConvertVideoToImagesAsync(MediaFile file, string outputImagesPattern, CancellationToken? cancellationToken = null)
    {
        await ExecuteConvertVideoToImagesAsync(file, null, outputImagesPattern,  cancellationToken ?? new CancellationToken());
    }
    public static async Task<MultiStream> ConvertVideoToImagesAsStreamAsync(MediaFile file, FileFormatType outputFormatType, CancellationToken? cancellationToken = null)
    {
        var stream = (await ExecuteConvertVideoToImagesAsync(file, outputFormatType, null,  cancellationToken ?? new CancellationToken()))!;

        return stream.GetMultiStreamBySignature(FilesSignatures.GetSignature(outputFormatType));
    }
    public static async Task<byte[][]> ConvertVideoToImagesAsBytesAsync(MediaFile file, FileFormatType outputFormatType, CancellationToken? cancellationToken = null)
    {
        var stream = (await ExecuteConvertVideoToImagesAsync(file, outputFormatType, null,  cancellationToken ?? new CancellationToken()))!;
        return stream.GetMultiStreamBySignature(FilesSignatures.GetSignature(outputFormatType)).ReadAsDataArray();
    }

    //======================================================================================================================================================================

    private static async Task<MemoryStream?> ExecuteGetAudioFromVideoAsync(MediaFile file, FileFormatType outputFileFormatType, AudioSampleRateType audioSampleRateType, int audioChannel, string? output, CancellationToken cancellationToken)
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

    public static async Task GetAudioFromVideoAsync(MediaFile file, FileFormatType outputFileFormatType, string output, int audioChannel = 2, CancellationToken? cancellationToken = null, AudioSampleRateType audioSampleRateType = AudioSampleRateType.Hz44100)
    {
        await ExecuteGetAudioFromVideoAsync(file, outputFileFormatType, audioSampleRateType, audioChannel, output,  cancellationToken ?? new CancellationToken());
    }

    public static async Task<MemoryStream> GetAudioFromVideoAsStreamAsync(MediaFile file, FileFormatType outputFileFormatType, int audioChannel = 2, CancellationToken? cancellationToken = null, AudioSampleRateType audioSampleRateType = AudioSampleRateType.Hz44100)
    {
        return (await ExecuteGetAudioFromVideoAsync(file, outputFileFormatType, audioSampleRateType, audioChannel, null,  cancellationToken ?? new CancellationToken()))!;
    }

    public static async Task<byte[]> GetAudioFromVideoAsBytesAsync(MediaFile file, FileFormatType outputFileFormatType, int audioChannel = 2, CancellationToken? cancellationToken = null, AudioSampleRateType audioSampleRateType = AudioSampleRateType.Hz44100)
    {
        return (await ExecuteGetAudioFromVideoAsync(file, outputFileFormatType, audioSampleRateType, audioChannel, null,  cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    //======================================================================================================================================================================

    private static async Task<MemoryStream?> ExecuteConvertVideoAsync( MediaFile file, string? output, CancellationToken cancellationToken)
    {
        return await ExecuteAsync(new VideoProcessingSettings().ReplaceIfExist().SetInputFiles(file).SetOutputArguments(output), cancellationToken);
    }

    public static async Task ConvertVideoAsync(MediaFile file, string output, CancellationToken? cancellationToken = null)
    {
        await ExecuteConvertVideoAsync(file, output, cancellationToken ?? new CancellationToken());
    }

    public static async Task<MemoryStream> ConvertVideoAsStreamAsync(MediaFile file, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConvertVideoAsync(file, null, cancellationToken ?? new CancellationToken()))!;
    }

    public static async Task<byte[]> ConvertVideoAsBytesAsync(MediaFile file, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConvertVideoAsync(file, null, cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    //======================================================================================================================================================================

    private static async Task<MemoryStream?> ExecuteAddWaterMarkToVideoAsync(MediaFile videoFile, MediaFile watermarkFile, PositionType position, string? output, CancellationToken cancellationToken)
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
                                                    .SetOutputArguments(output);

        return await ExecuteAsync(settings, cancellationToken);
    }

    public static async Task AddWaterMarkToVideoAsync(MediaFile videoFile, MediaFile watermarkFile, PositionType position, string output, CancellationToken? cancellationToken = null)
    {
        await ExecuteAddWaterMarkToVideoAsync(videoFile, watermarkFile, position, output, cancellationToken ?? new CancellationToken());
    }

    public static async Task<MemoryStream> AddWaterMarkToVideoAsStreamAsync(MediaFile videoFile, MediaFile watermarkFile, PositionType position, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteAddWaterMarkToVideoAsync(videoFile, watermarkFile, position, null, cancellationToken ?? new CancellationToken()))!;
    }

    public static async Task<byte[]> AddWaterMarkToVideoAsBytesAsync(MediaFile videoFile, MediaFile watermarkFile, PositionType position, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteAddWaterMarkToVideoAsync(videoFile, watermarkFile, position, null, cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    //======================================================================================================================================================================

    private static async Task<MemoryStream?> ExecuteExtractVideoFromFileAsync(MediaFile file, string? output, CancellationToken cancellationToken)
    {
        var settings = new VideoProcessingSettings().ReplaceIfExist()
                                                    .SetInputFiles(file)
                                                    .MapMetadata()
                                                    .MapArgument("0:v:0")
                                                    .VideoCodec(VideoCodecType.COPY)
                                                    .PixelFormat("yuv420p")
                                                    .SetOutputArguments(output);

        return await ExecuteAsync(settings, cancellationToken);
    }

    public static async Task ExtractVideoFromFileAsync(MediaFile file, string output, CancellationToken? cancellationToken = null)
    {
        await ExecuteExtractVideoFromFileAsync(file, output, cancellationToken ?? new CancellationToken());
    }

    public static async Task<MemoryStream> ExtractVideoFromFileAsStreamAsync(MediaFile file, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteExtractVideoFromFileAsync(file, null, cancellationToken ?? new CancellationToken()))!;
    }

    public static async Task<byte[]> ExtractVideoFromFileAsBytesAsync(MediaFile file, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteExtractVideoFromFileAsync(file, null, cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    //======================================================================================================================================================================

    private static async Task<MemoryStream?> ExecuteAddAudioToVideoAsync(MediaFile audioFile, MediaFile videofile, string? output, CancellationToken cancellationToken)
    {
        var settings = new VideoProcessingSettings().ReplaceIfExist()
                                                    .SetInputFiles(audioFile, videofile)
                                                    .VideoCodec(VideoCodecType.COPY)
                                                    .AudioCodec(AudioCodecType.COPY)
                                                    .SetOutputArguments(output);

        return await ExecuteAsync(settings, cancellationToken);
    }

    public static async Task AddAudioToVideoAsync(MediaFile audioFile, MediaFile videofile, string output, CancellationToken? cancellationToken = null)
    {
        await ExecuteAddAudioToVideoAsync(audioFile, videofile, output, cancellationToken ?? new CancellationToken());
    }

    public static async Task<MemoryStream> AddAudioToVideoAsStreamAsync(MediaFile audioFile, MediaFile videofile, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteAddAudioToVideoAsync(audioFile, videofile, null, cancellationToken ?? new CancellationToken()))!;
    }

    public static async Task<byte[]> AddAudioToVideoAsBytesAsync(MediaFile audioFile, MediaFile videofile, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteAddAudioToVideoAsync(audioFile, videofile, null, cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    //======================================================================================================================================================================

    private static async Task<MemoryStream?> ExecuteConvertVideoToGifAsync(MediaFile file, int fps, int scale, int loop, string? output, CancellationToken cancellationToken)
    {
        var setting = new VideoProcessingSettings().ReplaceIfExist()
                                                   .SetInputFiles(file)
                                                   .CustomArguments($"-vf \"fps={fps},scale={scale}:-1:flags=lanczos,split[s0][s1];[s0]palettegen[p];[s1][p]paletteuse\"")
                                                   .Loop(loop)
                                                   .SetOutputArguments(output);

        return await ExecuteAsync(setting, cancellationToken);
    }

    public static async Task ConvertVideoToGifAsync(MediaFile file, int fps, int scale, int loop, string output, CancellationToken? cancellationToken = null)
    {
        await ExecuteConvertVideoToGifAsync(file, fps, scale, loop, output, cancellationToken ?? new CancellationToken());
    }

    public static async Task<MemoryStream> ConvertVideoToGifAsStreamAsync(MediaFile file, int fps, int scale, int loop, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConvertVideoToGifAsync(file, fps, scale, loop, null, cancellationToken ?? new CancellationToken()))!;
    }

    public static async Task<byte[]> ConvertVideoToGifAsBytesAsync(MediaFile file, int fps, int scale, int loop, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConvertVideoToGifAsync(file, fps, scale, loop, null, cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    //======================================================================================================================================================================

    private static async Task<MemoryStream?> ExecuteCompressVideoAsync(MediaFile file, int crf, string? output, CancellationToken cancellationToken)
    {
        var setting = new VideoProcessingSettings().ReplaceIfExist().SetInputFiles(file).VideoCodec(VideoCodecType.LIBX264).Crf(crf).SetOutputArguments(output);
        return await ExecuteAsync(setting, cancellationToken);
    }

    public static async Task CompressVideoAsync(MediaFile file, int compressionRatio, string output, CancellationToken? cancellationToken = null)
    {
        await ExecuteCompressVideoAsync(file, compressionRatio, output, cancellationToken ?? new CancellationToken());
    }

    public static async Task<MemoryStream> CompressVideoAsStreamAsync(MediaFile file, int compressionRatio, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteCompressVideoAsync(file, compressionRatio, null, cancellationToken ?? new CancellationToken()))!;
    }

    public static async Task<byte[]> CompressVideoAsBytesAsync(MediaFile file, int compressionRatio, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteCompressVideoAsync(file, compressionRatio, null, cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    //======================================================================================================================================================================

    private static async Task<MemoryStream?> ExecuteCompressImageAsync(MediaFile file, int level, string? output, CancellationToken cancellationToken)
    {
        var setting = new VideoProcessingSettings().ReplaceIfExist()
                                                   .HideBanner()
                                                   .SetInputFiles(file)
                                                   .CompressionLevel(level)
                                                   .SetOutputArguments(output);

        return await ExecuteAsync(setting, cancellationToken);
    }

    public static async Task CompressImageAsync(MediaFile file, int level, string output, CancellationToken? cancellationToken = null)
    {
        await ExecuteCompressImageAsync(file, level, output, cancellationToken ?? new CancellationToken());
    }

    public static async Task<MemoryStream> CompressImageAsStreamAsync(MediaFile file, int level, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteCompressImageAsync(file, level, null, cancellationToken ?? new CancellationToken()))!;
    }

    public static async Task<byte[]> CompressImageAsBytesAsync(MediaFile file, int level, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteCompressImageAsync(file, level, null, cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    //======================================================================================================================================================================


    private static async Task CreateMpegts(MediaFile inputFile, string output, CancellationToken cancellationToken)
    {
        var setting = new VideoProcessingSettings().ReplaceIfExist()
                                                   .SetInputFiles(inputFile)
                                                   .CopyAllCodec()
                                                   .VideoBSF("h264_mp4toannexb")
                                                   .Format(FileFormatType.MPEGTS)
                                                   .SetOutputArguments(output);

        await ExecuteAsync(setting, cancellationToken);
    }

    private static async Task<MemoryStream?> ExecuteConcatVideosAsync(MediaFile[] files, string? outputFile, CancellationToken cancellationToken)
    {
        var intermediateFiles = new List<string>();

        for (var i = 0; i < files.Length; i++)
        {
            var outputFileName = $"_{i}{Guid.NewGuid()}.ts";
            intermediateFiles.Add(outputFileName);
            await CreateMpegts(files[i], outputFileName, cancellationToken);
        }

        var strCmd = " -i \"concat:";

        foreach (var i in intermediateFiles)
        {
            strCmd += i;
            strCmd += "|";
        }

        strCmd += strCmd.Remove(strCmd.Length - 1) + "\" ";

        var setting = new VideoProcessingSettings().ReplaceIfExist()
                                                   .CustomArguments(strCmd)
                                                   .CopyAllCodec()
                                                   .AudioBSF("aac_adtstoasc")
                                                   .MovFralgs("+faststart")
                                                   .SetOutputArguments(outputFile);

        var result = await ExecuteAsync(setting, cancellationToken);

        foreach (var file in intermediateFiles)
        {
            if(File.Exists(file))
                File.Delete(file);
        }

        return result;
    }

    public static async Task ConcatVideosAsync(MediaFile[] files, string output, CancellationToken? cancellationToken = null)
    {
        await ExecuteConcatVideosAsync(files, output, cancellationToken ?? new CancellationToken());
    }

    public static async Task<MemoryStream> ConcatVideosAsStreamAsync(MediaFile[] files, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConcatVideosAsync(files, null, cancellationToken ?? new CancellationToken()))!;
    }

    public static async Task<byte[]> ConcatVideosAsBytesAsync(MediaFile[] files, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConcatVideosAsync(files, null, cancellationToken ?? new CancellationToken()))!.ToArray();
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

    private static async Task<MemoryStream?> ExecuteAddHardSubtitlesAsync(MediaFile videoFile, MediaFile subsFile, string language, string? outputFile, CancellationToken cancellationToken)
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

    public static async Task AddHardSubtitlesAsync(MediaFile videoFile, MediaFile subsFile, string language, string outputFile, CancellationToken? cancellationToken = null)
    {
        await ExecuteAddHardSubtitlesAsync(videoFile, subsFile, language, outputFile, cancellationToken ?? new CancellationToken());
    }

    public static async Task<MemoryStream> AddHardSubtitlesAsStreamAsync(MediaFile videoFile, MediaFile subsFile, string language, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteAddHardSubtitlesAsync(videoFile, subsFile, language, null, cancellationToken ?? new CancellationToken()))!;
    }

    public static async Task<byte[]> AddHardSubtitlesAsBytesAsync(MediaFile videoFile, MediaFile subsFile, string language, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteAddHardSubtitlesAsync(videoFile, subsFile, language, null, cancellationToken ?? new CancellationToken()))!.ToArray();
    }
}