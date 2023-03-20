using System.Text;
using MediaFileProcessor.Extensions;
using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Enums;
using MediaFileProcessor.Models.Settings;
using MediaFileProcessor.Processors.Interfaces;
namespace MediaFileProcessor.Processors;

/// <summary>
/// This class is responsible for processing video files.
/// </summary>
public class VideoFileProcessor : IVideoFileProcessor
{
    public VideoFileProcessor() { }

    public VideoFileProcessor(string ffmpegExePath, string ffprobeExePath)
    {
        _ffmpeg = ffmpegExePath;
        _ffprobe = ffprobeExePath;
    }

    /// <summary>
    /// The name of the ffmpeg executable.
    /// </summary>
    private static string _ffmpeg = "ffmpeg";

    /// <summary>
    /// The name of the _ffprobe executable.
    /// </summary>
    private static string _ffprobe = "ffprobe";

    /// <summary>
    /// The address from which the ffmpeg and ffprobe executable can be downloaded.
    /// </summary>
    private static readonly string _zipAddress = "https://www.gyan.dev/ffmpeg/builds/ffmpeg-release-essentials.zip";

    /// <inheritdoc />
    public async Task<MemoryStream?> ExecuteAsync(VideoProcessingSettings settings, CancellationToken cancellationToken)
    {
        using(var process = new MediaFileProcess(_ffmpeg, settings.GetProcessArguments(), settings, settings.GetInputStreams(), settings.GetInputPipeNames()))
            return  await process.ExecuteAsync(cancellationToken);
    }

    //======================================================================================================================================================================

    /// <summary>
    /// Executes the process of getting a frame from a video at the specified timestamp.
    /// Supported image formats: WEBP, JPG, PNG, TIFF, BMP, ICO
    /// </summary>
    /// <param name="timestamp">The timestamp at which to extract the frame from the video.</param>
    /// <param name="file">The media file representing the video to extract the frame from.</param>
    /// <param name="outputFile">The output file to save the extracted frame to (optional).</param>
    /// <param name="outputFormat">The format to use for the output file (if specified).</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the process if necessary.</param>
    /// <returns>A MemoryStream representing the extracted frame, or null if the process was cancelled.</returns>
    public async Task<MemoryStream?> ExtractFrameFromVideoAsync(TimeSpan timestamp,
                                                                MediaFile file,
                                                                string? outputFile = null,
                                                                FileFormatType? outputFormat = null,
                                                                CancellationToken? cancellationToken = null)
    {
        var settings = new VideoProcessingSettings().ReplaceIfExist().Seek(timestamp).SetInputFiles(file).FramesNumber(1).SetOutputArguments(outputFile);

        if(outputFile is null && outputFormat is null)
            throw new Exception("If the outputFile is not specified then the outputFormat must be indicated necessarily");

        outputFormat ??= outputFile!.GetFileFormatType();

        switch(outputFormat)
        {
            case FileFormatType.WEBP:
            case FileFormatType.JPG:
            case FileFormatType.IMAGE2PIPE:
            case FileFormatType.IMAGE2:
                settings.Format(outputFormat.Value);

                break;
            case FileFormatType.PNG:
                if(outputFile == null)
                {
                    settings.VideoCodec(VideoCodecType.PNG);
                    settings.Format(FileFormatType.RAWVIDEO);
                }

                break;
            case FileFormatType.TIFF:
                settings.PixelFormat("rgba");

                if(outputFile == null)
                {
                    settings.VideoCodec(VideoCodecType.TIFF);
                    settings.Format(FileFormatType.RAWVIDEO);
                }

                break;
            case FileFormatType.BMP:
                settings.PixelFormat("bgr8");

                if(outputFile == null)
                {
                    settings.VideoCodec(VideoCodecType.BMP);
                    settings.Format(FileFormatType.RAWVIDEO);
                }

                break;
            case FileFormatType.ICO:
                settings.VideoSize(VideoSizeType.CUSTOM, 64, 64);

                if(outputFile == null)
                {
                    settings.VideoCodec(VideoCodecType.BMP);
                    settings.Format(FileFormatType.RAWVIDEO);
                }

                break;
            default:
                throw new NotSupportedException("This output format is not supported");
        }

        return await ExecuteAsync(settings, cancellationToken ?? new CancellationToken());
    }

    /// <summary>
    /// This method moves the MOOV atom of the MP4 format to the beginning because FFmpeg must know how to process this file when reading files from a stream.
    /// The MP4 file processing information is usually at the end and FFmpeg cannot process it as a stream.
    /// When moving a MOOV atom, ffmpeg cannot retrieve a file from a stream and output the result as a stream.
    /// To move an atom, it is necessary to have a file physically in the directory, the result of processing must also be written to the directory.
    /// If the MP4 file is in your stream form and needs to shift the MOOV atom, and get the result in wanting necessarily in the video stream.
    /// Then this method will create a physical file from your input stream and pass it to FFmpeg for processing and convert the result from the file to a stream and return this stream.
    /// All intermediate files created will then be deleted.
    /// </summary>
    /// <param name="file">Входной файл</param>
    /// <param name="fileFormatType"></param>
    /// <param name="outputFile">
    /// The output file. If outputFile is specified, the result will be generated from it.
    /// If the outputFile is null, the result will be retrieved as a stream.
    /// </param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>If outputFile is specified, null will be returned. And if outputfile is not specified the stream containing the resulting file will be returned</returns>
    public async Task<MemoryStream?> SetStartMoovAsync(MediaFile file,
                                                       FileFormatType fileFormatType,
                                                       string? outputFile = null,
                                                       CancellationToken cancellationToken = default)
    {
        if(file.InputType is MediaFileInputType.NamedPipe)
            throw new ArgumentException("NamedPipe input is not sopported in current version.");

        string? fileName = null;
        string? resultFileName = null;

        try
        {
            var settings = new VideoProcessingSettings().ReplaceIfExist();

            if(file.InputType is MediaFileInputType.Stream)
            {
                fileName = @$"{Guid.NewGuid()}.mp4";
                using (var output = new FileStream(fileName, FileMode.Create))
                    await file.InputFileStream!.CopyToAsync(output);
            }

            if(outputFile is null)
                resultFileName = @$"{Guid.NewGuid()}.mp4";

            settings.SetInputFiles(fileName is not null ? new MediaFile(fileName) : file);

            settings.MovFralgs("faststart").SetOutputArguments(resultFileName ?? outputFile);

            switch(fileFormatType)
            {
                case FileFormatType.MP4 or FileFormatType.M4V or FileFormatType.MKV or FileFormatType.MOV or FileFormatType._3GP:
                    settings.AudioCodec(AudioCodecType.COPY).VideoCodec(VideoCodecType.COPY);

                    break;
                case FileFormatType.WMV:
                    settings.VideoCodec(VideoCodecType.LIBX264).AudioCodec(AudioCodecType.AAC).Strict("experimental").AudioBitRate(192);

                    break;
                default:
                    settings.VideoCodec(VideoCodecType.LIBX264).VideoCodecPreset(VideoCodecPresetType.ULTRAFAST).Crf(23).AudioCodec(AudioCodecType.COPY);

                    break;
            }

            if(outputFile is null)
            {
                await ExecuteAsync(settings, cancellationToken);

                return new MemoryStream(File.ReadAllBytes(resultFileName!));
            }
            else
            {
                await ExecuteAsync(settings, cancellationToken);

                return null;
            }
        }
        finally
        {
            if(fileName is not null && File.Exists(resultFileName))
                File.Delete(fileName);

            if(resultFileName is not null && File.Exists(resultFileName))
                File.Delete(resultFileName);
        }
    }

    //======================================================================================================================================================================

    /// <summary>
    /// Cuts a portion of a video file and returns the result as a memory stream.
    /// </summary>
    /// <param name="startTime">The start time of the portion to be cut.</param>
    /// <param name="endTime">The end time of the portion to be cut.</param>
    /// <param name="file">The media file to be cut.</param>
    /// <param name="outputFile">The name of the output file, if any.</param>
    /// <param name="outputFormat">The format in which the cut portion should be returned.</param>
    /// <param name="cancellationToken">The cancellation token for the operation.</param>
    /// <returns>A memory stream containing the cut portion of the video file, or `null` if an output file was specified.</returns>
    public  async Task<MemoryStream?> CutVideoAsync(TimeSpan startTime,
                                                    TimeSpan endTime,
                                                    MediaFile file,
                                                    string? outputFile = null,
                                                    FileFormatType? outputFormat = null,
                                                    CancellationToken? cancellationToken = null)
    {
        var settings = new VideoProcessingSettings().ReplaceIfExist().SetInputFiles(file).Seek(startTime).TimePosition(endTime).SetOutputArguments(outputFile);

        if(outputFile is null && outputFormat is null)
            throw new Exception("If the outputFile is not specified then the outputFormat must be indicated necessarily");

        outputFormat = outputFile?.GetFileFormatType() ?? outputFormat;

        switch(outputFormat)
        {
            case FileFormatType.RM:
                settings.VideoCodec(VideoCodecType.RV10).Crf(30);

                break;
            case FileFormatType.WMV:
                settings.VideoCodec(VideoCodecType.WMV1).Crf(30);

                break;
            case FileFormatType.WEBM or FileFormatType.AVI:
                settings.CopyAllCodec();

                break;
            default:
                settings.VideoCodec(VideoCodecType.LIBX264).Crf(30);

                break;
        }

        if(outputFile is null)
            switch(outputFormat)
            {
                case FileFormatType._3GP or FileFormatType.ASF or FileFormatType.MOV or FileFormatType.MP4:
                    settings.Format(FileFormatType.MPEG);

                    break;
                case FileFormatType.M2TS:
                    settings.Format(FileFormatType.MPEGTS);

                    break;
                case FileFormatType.MKV:
                    settings.Format("matroska");

                    break;
                case FileFormatType.RM:
                    settings.Format(outputFormat.Value);

                    break;
                case FileFormatType.WMV:
                    settings.Format(FileFormatType.ASF);

                    break;
                default:
                    settings.Format(outputFormat!.Value);

                    break;
            }

        return await ExecuteAsync(settings, cancellationToken ?? new CancellationToken());
    }

    //======================================================================================================================================================================

    /// <summary>
    /// Executes the conversion of a set of images to a video file.
    /// </summary>
    /// <param name="file">The file containing the images to be converted into a video.</param>
    /// <param name="frameRate">The number of frames per second in the output video.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <param name="outputFile">The file name and path of the output video.</param>
    /// <param name="outputFormat">The format of the output video file.</param>
    /// <returns>A MemoryStream containing the content of the output video, or null if the operation was cancelled.</returns>
    public async Task<MemoryStream?> ConvertImagesToVideoAsync(MediaFile file,
                                                                 int frameRate,
                                                                 string? outputFile = null,
                                                                 FileFormatType? outputFormat = null,
                                                                 CancellationToken? cancellationToken = null)
    {
        if(outputFile is null && outputFormat is null)
            throw new Exception("If the outputFile is not specified then the outputFormat must be indicated necessarily");

        outputFormat ??= outputFile!.GetFileFormatType();

        var settings = new VideoProcessingSettings().ReplaceIfExist()
                                                    .FrameRate(frameRate)
                                                    .SetInputFiles(file)
                                                    .SetOutputArguments(outputFile);
        outputFormat = outputFile?.GetFileFormatType() ?? outputFormat;

        switch(outputFormat)
        {
            case FileFormatType.RM:
                settings.VideoSize(VideoSizeType.HD720);
                settings.VideoCodec(VideoCodecType.RV10);
                break;
            case FileFormatType.WMV:
                settings.VideoCodec(VideoCodecType.WMV1);

                break;
            case FileFormatType.WEBM:
                settings.VideoCodec(VideoCodecType.LIBVPX);

                break;
            case FileFormatType.M4V:
                settings.VideoSize(VideoSizeType.CIF);
                settings.VideoCodec(VideoCodecType.MPEG4);

                break;
            default:
                settings.VideoCodec(VideoCodecType.LIBX264);

                break;
        }

        if(outputFile is null)
            switch(outputFormat)
            {
                case FileFormatType._3GP or FileFormatType.ASF or FileFormatType.MOV or FileFormatType.MP4:
                    settings.Format(FileFormatType.MPEG);

                    break;
                case FileFormatType.M2TS:
                    settings.Format(FileFormatType.MPEGTS);

                    break;
                case FileFormatType.MKV:
                    settings.Format("matroska");

                    break;
                case FileFormatType.RM:
                    settings.Format(outputFormat.Value);

                    break;
                case FileFormatType.WMV:
                    settings.Format(FileFormatType.ASF);

                    break;
                case FileFormatType.M4V:
                    settings.KeyFrame(30);
                    settings.Format(FileFormatType.M4V);

                    break;
                default:
                    settings.Format(outputFormat!.Value);

                    break;
            }

        return await ExecuteAsync(settings, cancellationToken ?? new CancellationToken());
    }

    //======================================================================================================================================================================

    /// <summary>
    /// Converts the video to images and returns the result as a MemoryStream.
    /// </summary>
    /// <param name="file">The video file to convert.</param>
    /// <param name="outputFormat">The output file format type for the images.</param>
    /// <param name="outputImagesPattern">The pattern for the output images file names.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The result of the conversion as a MemoryStream.</returns>
    public  async Task<MultiStream?> ConvertVideoToImagesAsync(MediaFile file,
                                                               string? outputImagesPattern = null,
                                                               FileFormatType? outputFormat = null,
                                                               CancellationToken? cancellationToken = null)
    {
        var settings = new VideoProcessingSettings().ReplaceIfExist().SetInputFiles(file).SetOutputArguments(outputImagesPattern);

        if(outputImagesPattern is null && outputFormat is null)
            throw new Exception("If the outputImagesPattern is not specified then the outputFormat must be indicated necessarily");

        outputFormat ??= outputImagesPattern!.GetFileFormatType();

        switch(outputFormat)
        {
            case FileFormatType.WEBP:
                settings.VideoCodec(VideoCodecType.LIBWEBP);

                if(outputImagesPattern == null)
                    settings.Format(FileFormatType.IMAGE2PIPE);

                break;
            case FileFormatType.JPG:
            case FileFormatType.IMAGE2PIPE:
            case FileFormatType.IMAGE2:
                settings.Format(outputImagesPattern == null ? FileFormatType.IMAGE2PIPE : outputFormat.Value);

                break;
            case FileFormatType.PNG:
                if(outputImagesPattern == null)
                {
                    settings.VideoCodec(VideoCodecType.PNG);
                    settings.Format(FileFormatType.RAWVIDEO);
                }

                break;
            case FileFormatType.TIFF:
                settings.PixelFormat("rgba");

                if(outputImagesPattern == null)
                {
                    settings.VideoCodec(VideoCodecType.TIFF);
                    settings.Format(FileFormatType.RAWVIDEO);
                }

                break;
            case FileFormatType.BMP:
                settings.PixelFormat("bgr8");

                if(outputImagesPattern == null)
                {
                    settings.VideoCodec(VideoCodecType.BMP);
                    settings.Format(FileFormatType.RAWVIDEO);
                }

                break;
            default:
                throw new NotSupportedException("This output format is not supported");
        }

        var stream = await ExecuteAsync(settings, cancellationToken ?? new CancellationToken());

        return stream?.GetMultiStreamBySignature(outputFormat.Value.GetSignature());
    }

    //======================================================================================================================================================================

    /// <summary>
    /// Extracts audio from a video file.
    /// </summary>
    /// <param name="file">The video file to extract audio from.</param>
    /// <param name="outputFormat">The format to save the extracted audio as.</param>
    /// <param name="outputFile">The path to save the extracted audio file to. If `null`, the audio will be returned as a `MemoryStream`.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A `MemoryStream` containing the extracted audio, or `null` if `output` is not `null`.</returns>
    public async Task<MemoryStream?> ExtractAudioFromVideoAsync(MediaFile file,
                                                                string? outputFile = null,
                                                                FileFormatType? outputFormat = null,
                                                                CancellationToken? cancellationToken = null)
    {
        if(outputFile is null && outputFormat is null)
            throw new Exception("If the outputFile is not specified then the outputFormat must be indicated necessarily");

        outputFormat ??= outputFile!.GetFileFormatType();

        var settings = new VideoProcessingSettings().ReplaceIfExist()
                                                    .SetInputFiles(file)
                                                    .DeleteVideo()
                                                    .AudioSampleRate(AudioSampleRateType.Hz44100)
                                                    .AudioChannel(2)
                                                    .SetOutputArguments(outputFile);
        switch(outputFormat)
        {
            case FileFormatType.AAC:
                settings.AudioCodec(AudioCodecType.AAC);

                break;
            case FileFormatType.FLAC:
                settings.AudioCodec(AudioCodecType.FLAC);

                break;
            case FileFormatType.WMA:
                settings.AudioCodec(AudioCodecType.WMAV2);

                break;
            case FileFormatType.MP3:
                settings.AudioCodec(AudioCodecType.MP3);

                break;
            case FileFormatType.M4A:
                settings.AudioCodec(AudioCodecType.AAC);

                break;
            default:
                settings.Format(outputFormat.Value);

                break;
        }

        if(outputFile is null)
            switch(outputFormat)
            {
                case FileFormatType.AAC:
                    settings.Format("adts");

                    break;
                case FileFormatType.FLAC:
                    settings.Format(outputFormat.Value);

                    break;
                case FileFormatType.WMA:
                    settings.Format(FileFormatType.ASF);

                    break;
                case FileFormatType.M4A:
                    settings.Format("adts");

                    break;
                default:
                    settings.Format(outputFormat.Value);

                    break;
            }

        return await ExecuteAsync(settings, cancellationToken ?? new CancellationToken());
    }

    //======================================================================================================================================================================

    /// <summary>
    /// Executes the conversion of a video file to the specified output format.
    /// </summary>
    /// <param name="file">The video file to be converted.</param>
    /// <param name="outputFile">The output file path for the converted video. Can be null if output is to be returned as a stream.</param>
    /// <param name="outputFormat">The desired output format for the converted video.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="MemoryStream"/> that contains the converted video, or null if output is specified as a file path.</returns>
    public async Task<MemoryStream?> ConvertVideoAsync(MediaFile file,
                                                       string? outputFile = null,
                                                       FileFormatType? outputFormat = null,
                                                       CancellationToken? cancellationToken = null)
    {
        if(outputFile is null && outputFormat is null)
            throw new Exception("If the outputFile is not specified then the outputFormat must be indicated necessarily");

        outputFormat ??= outputFile!.GetFileFormatType();

        var settings = new VideoProcessingSettings().ReplaceIfExist().SetInputFiles(file).MapMetadata().SetOutputArguments(outputFile);

        switch(outputFormat)
        {
            case FileFormatType.RM:
                settings.VideoSize(VideoSizeType.HD720);
                settings.VideoCodec(VideoCodecType.RV10);
                break;
            case FileFormatType.WMV:
                settings.VideoCodec(VideoCodecType.WMV1);

                break;
            case FileFormatType.WEBM:
                settings.VideoCodec(VideoCodecType.LIBVPX);

                break;
            case FileFormatType.M4V:
                settings.VideoSize(VideoSizeType.CIF);
                settings.VideoCodec(VideoCodecType.MPEG4);

                break;
            case FileFormatType._3GP:
                settings.VideoCodec(VideoCodecType.LIBX264);
                if(outputFile is not null)
                    settings.Format(FileFormatType.MPEG);

                break;
        }

        if(outputFile is null)
            switch(outputFormat)
            {
                case FileFormatType._3GP or FileFormatType.ASF or FileFormatType.MOV or FileFormatType.MP4:
                    settings.Format(FileFormatType.MPEG);

                    break;
                case FileFormatType.M2TS:
                    settings.Format(FileFormatType.MPEGTS);

                    break;
                case FileFormatType.MKV:
                    settings.Format("matroska");

                    break;
                case FileFormatType.RM:
                    settings.Format(outputFormat.Value);

                    break;
                case FileFormatType.WMV:
                    settings.Format(FileFormatType.ASF);

                    break;
                case FileFormatType.M4V:
                    settings.KeyFrame(30);
                    settings.Format(FileFormatType.M4V);

                    break;
                default:
                    settings.Format(outputFormat.Value);

                    break;
            }

        return await ExecuteAsync(settings, cancellationToken ?? new CancellationToken());
    }

    //======================================================================================================================================================================

    /// <summary>
    /// Executes the task of adding a watermark to the specified video.
    /// </summary>
    /// <param name="videoFile">The video file to add watermark to.</param>
    /// <param name="watermarkFile">The watermark file to be added to the video.</param>
    /// <param name="position">The position of the watermark in the video.</param>
    /// <param name="outputFile">The path and name of the output file.</param>
    /// <param name="outputFormat">The format of the output file.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A MemoryStream object that contains the data of the output file.</returns>
    public async Task<MemoryStream?> AddWaterMarkToVideoAsync(MediaFile videoFile,
                                                              MediaFile watermarkFile,
                                                              PositionType position,
                                                              string? outputFile = null,
                                                              FileFormatType? outputFormat = null,
                                                              CancellationToken? cancellationToken = null)
    {
        if(outputFile is null && outputFormat is null)
            throw new Exception("If the outputFile is not specified then the outputFormat must be indicated necessarily");

        outputFormat ??= outputFile!.GetFileFormatType();

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
                                                    .Format(outputFormat.Value)
                                                    .SetOutputArguments(outputFile);

        return await ExecuteAsync(settings, cancellationToken ?? new CancellationToken());
    }

    //======================================================================================================================================================================

    /// <summary>
    /// Extracts the video from a media file.
    /// </summary>
    /// <param name="file">The media file from which the video should be extracted.</param>
    /// <param name="outputFile">The output path where the extracted video should be stored. If this is `null`, the extracted video will be stored in memory.</param>
    /// <param name="outputFormat">The format of the extracted video file.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A `MemoryStream` containing the extracted video, or `null` if the `output` parameter was provided.</returns>
    public async Task<MemoryStream?> ExtractVideoFromFileAsync(MediaFile file,
                                                               string? outputFile = null,
                                                               FileFormatType? outputFormat = null,
                                                               CancellationToken? cancellationToken = null)
    {
        if(outputFile is null && outputFormat is null)
            throw new Exception("If the outputFile is not specified then the outputFormat must be indicated necessarily");

        outputFormat ??= outputFile!.GetFileFormatType();

        var settings = new VideoProcessingSettings().ReplaceIfExist()
                                                    .SetInputFiles(file)
                                                    .MapMetadata()
                                                    .MapArgument("0:v:0")
                                                    .VideoCodec(VideoCodecType.COPY)
                                                    .PixelFormat("yuv420p")
                                                    .Format(outputFormat.Value)
                                                    .SetOutputArguments(outputFile);

        return await ExecuteAsync(settings, cancellationToken ?? new CancellationToken());
    }

    //======================================================================================================================================================================

    /// <summary>
    /// Adds audio to a video file.
    /// </summary>
    /// <param name="audioFile">The audio file to add to the video.</param>
    /// <param name="videoFile">The video file to add the audio to.</param>
    /// <param name="outputFile">The output file path for the audio-added video. Default is null.</param>
    /// <param name="outputFormat">The output file format type of the audio-added video.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the asynchronous operation.</param>
    /// <returns>A memory stream that represents the audio-added video.</returns>
    public async Task<MemoryStream?> AddAudioToVideoAsync(MediaFile audioFile,
                                                          MediaFile videoFile,
                                                          string? outputFile = null,
                                                          FileFormatType? outputFormat = null,
                                                          CancellationToken? cancellationToken = null)
    {
        if(outputFile is null && outputFormat is null)
            throw new Exception("If the outputFile is not specified then the outputFormat must be indicated necessarily");

        outputFormat ??= outputFile!.GetFileFormatType();

        var settings = new VideoProcessingSettings().ReplaceIfExist()
                                                    .SetInputFiles(audioFile, videoFile)
                                                    .VideoCodec(VideoCodecType.COPY)
                                                    .AudioCodec(AudioCodecType.COPY)
                                                    .Format(outputFormat.Value)
                                                    .SetOutputArguments(outputFile);

        return await ExecuteAsync(settings, cancellationToken ?? new CancellationToken());
    }

    //======================================================================================================================================================================

    /// <summary>
    /// Converts a video file to a GIF using the given parameters.
    /// </summary>
    /// <param name="file">The video file to be converted.</param>
    /// <param name="fps">The number of frames per second in the output GIF.</param>
    /// <param name="scale">The scaling factor to be applied to the video before converting it to a GIF.</param>
    /// <param name="loop">The number of times the output GIF should loop. Set to 0 for infinite looping.</param>
    /// <param name="outputFile">The path to the output file. Set to null if the result should be returned as a stream.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the task.</param>
    /// <returns>The resulting GIF as a memory stream, or null if an output file was specified.</returns>
    public async Task<MemoryStream?> ConvertVideoToGifAsync(MediaFile file, int fps, int scale, int loop, string? outputFile = null, CancellationToken? cancellationToken = null)
    {
        var setting = new VideoProcessingSettings().ReplaceIfExist()
                                                   .SetInputFiles(file)
                                                   .CustomArguments($"-vf \"fps={fps},scale={scale}:-1:flags=lanczos,split[s0][s1];[s0]palettegen[p];[s1][p]paletteuse\"")
                                                   .Loop(loop)
                                                   .Format(FileFormatType.GIF)
                                                   .SetOutputArguments(outputFile);

        return await ExecuteAsync(setting, cancellationToken ?? new CancellationToken());
    }

    //======================================================================================================================================================================

    /// <summary>
    /// Asynchronously executes the compression of the input video file.
    /// </summary>
    /// <param name="file">The input media file to be compressed.</param>
    /// <param name="compressionRatio">The constant rate factor used for compression.</param>
    /// <param name="output">The desired name of the output file, including the file extension.
    ///                     If `null`, the original file name will be used.</param>
    /// <param name="cancellationToken">A cancellation token to stop the compression process.</param>
    /// <returns>A memory stream of the compressed video, or `null` if the compression failed.</returns>
    public async Task<MemoryStream?> CompressVideoAsync(MediaFile file,
                                                          int compressionRatio,
                                                          string? output = null,
                                                          CancellationToken? cancellationToken = null)
    {
        var setting = new VideoProcessingSettings().ReplaceIfExist()
                                                   .SetInputFiles(file)
                                                   .VideoCodec(VideoCodecType.H264)
                                                   .Crf(compressionRatio)
                                                   .VideoCodecPreset(VideoCodecPresetType.SLOW)
                                                   .VideoBitRate(500)
                                                   .MaxRate(500)
                                                   .BufSize(1000)
                                                   .VideoSize(VideoSizeType.CUSTOM, -2, 720)
                                                   .Format("outputFormat") //TODO get format from input
                                                   .SetOutputArguments(output);

        return await ExecuteAsync(setting, cancellationToken ?? new CancellationToken());
    }

    //======================================================================================================================================================================

    /// <summary>
    /// Executes the compression of an image file.
    /// </summary>
    /// <param name="file">The image file to be compressed.</param>
    /// <param name="level">The compression level to be applied to the image file.</param>
    /// <param name="outputFile">The output file path, if specified. If null, the output will be returned as a memory stream.</param>
    /// <param name="outputFormat">The file format type for the output file.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A memory stream containing the compressed image data, or null if the output file path is specified.</returns>
    public async Task<MemoryStream?> CompressImageAsync(MediaFile file,
                                                        int level,
                                                        string? outputFile = null,
                                                        FileFormatType? outputFormat = null,
                                                        CancellationToken? cancellationToken = null)
    {
        if(outputFile is null && outputFormat is null)
            throw new Exception("If the outputFile is not specified then the outputFormat must be indicated necessarily");

        outputFormat ??= outputFile!.GetFileFormatType();

        var setting = new VideoProcessingSettings().ReplaceIfExist()
                                                   .HideBanner()
                                                   .SetInputFiles(file)
                                                   .CompressionLevel(level)
                                                   .Format(outputFormat.Value)
                                                   .SetOutputArguments(outputFile);

        return await ExecuteAsync(setting, cancellationToken ?? new CancellationToken());
    }

    //======================================================================================================================================================================

    /// <summary>
    /// Creates an MPEG-TS file from the input media file.
    /// </summary>
    /// <param name="inputFile">The input media file to be used for creating the MPEG-TS file.</param>
    /// <param name="output">The path of the output MPEG-TS file to be created.</param>
    /// <param name="videoCodecType">The type of video codec to be used for the output file.</param>
    /// <param name="audioCodecType">The type of audio codec to be used for the output file.</param>
    /// <param name="videoBSF">The video bitstream filter to be used for the output file.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task CreateMpegts(MediaFile inputFile,
                                    string output,
                                    VideoCodecType videoCodecType,
                                    AudioCodecType audioCodecType,
                                    string videoBSF,
                                    CancellationToken cancellationToken)
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

    /// <summary>
    /// Executes the concat videos asynchronous.
    /// </summary>
    /// <param name="files">The files to be concatenated.</param>
    /// <param name="outputFile">The output file path.</param>
    /// <param name="outputFormat">The output file format type.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A memory stream containing the result of the operation.</returns>
    public async Task<MemoryStream?> ConcatVideosAsync(MediaFile[] files,
                                                       string? outputFile = null,
                                                       FileFormatType? outputFormat = null,
                                                       CancellationToken? cancellationToken = null)
    {
        if(outputFile is null && outputFormat is null)
            throw new Exception("If the outputFile is not specified then the outputFormat must be indicated necessarily");

        outputFormat ??= outputFile!.GetFileFormatType();

        var intermediateFiles = new List<string>();

        // Create intermediate mpegts files for each of the input files
        for (var i = 0; i < files.Length; i++)
        {
            var outputFileName = $"_{i}{Guid.NewGuid()}.ts";
            intermediateFiles.Add(outputFileName);
            await CreateMpegts(files[i], outputFileName, VideoCodecType.LIBX264, AudioCodecType.AAC, "h264_mp4toannexb", cancellationToken ?? default);
        }

        // Concatenate the intermediate files using the 'concat' argument in FFmpeg
        var strCmd = " -i \"concat:";

        foreach (var i in intermediateFiles)
        {
            strCmd += i;
            strCmd += "|";
        }

        strCmd = strCmd.Remove(strCmd.Length - 1) + "\" ";

        // Set the output format and execute the FFmpeg command
        var setting = new VideoProcessingSettings().ReplaceIfExist()
                                                   .CustomArguments(strCmd)
                                                   .CopyAllCodec()
                                                   .AudioBSF("aac_adtstoasc")
                                                   .MovFralgs("+faststart")
                                                   .Format(outputFormat.Value)
                                                   .SetOutputArguments(outputFile);

        var result = await ExecuteAsync(setting, cancellationToken ?? default);

        // Delete the intermediate files
        foreach (var file in intermediateFiles)
        {
            if(File.Exists(file))
                File.Delete(file);
        }

        return result;
    }

    //======================================================================================================================================================================

    /// <inheritdoc />
    public async Task<string> GetVideoInfo(MediaFile videoFile, CancellationToken? cancellationToken = null)
    {
        VideoProcessingSettings? settings;

        if(videoFile.InputFilePath != null)
        {
            settings = new VideoProcessingSettings().CustomArguments("-v panic -print_format json=c=1 -show_streams -show_entries "
                                                                   + $"format=size,duration,bit_rate:format_tags=creation_time {videoFile.InputFilePath}");
        }
        else
        {
            settings = new VideoProcessingSettings().CustomArguments("-v panic -print_format json=c=1 -show_streams -show_entries "
                                                                   + "format=size,duration,bit_rate:format_tags=creation_time -");

            settings.SetInputStreams(videoFile.InputFileStream!);
        }

        var process = new MediaFileProcess(_ffprobe, settings.GetProcessArguments(false), settings, settings.GetInputStreams());

        var result = await process.ExecuteAsync(cancellationToken ?? new CancellationToken());

        return Encoding.UTF8.GetString(result!.ToArray());
    }

    //======================================================================================================================================================================

    /// <summary>
    /// Executes the process of adding hard subtitles to a video file.
    /// </summary>
    /// <param name="videoFile">The video file to add subtitles to.</param>
    /// <param name="subsFile">The subtitles file to add to the video.</param>
    /// <param name="language">The language of the subtitles to add to the video.</param>
    /// <param name="outputFile">The output file path. If not specified, the output will be returned as a stream.</param>
    /// <param name="outputFormat">The format type of the output file.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the task.</param>
    /// <returns>A memory stream containing the output video with added subtitles, or `null` if an output file is specified.</returns>
    public async Task<MemoryStream?> AddSubtitlesAsync(MediaFile videoFile,
                                                       MediaFile subsFile,
                                                       string language,
                                                       string? outputFile = null,
                                                       FileFormatType? outputFormat = null,
                                                       CancellationToken? cancellationToken = null)
    {
        if(outputFile is null && outputFormat is null)
            throw new Exception("If the outputFile is not specified then the outputFormat must be indicated necessarily");

        outputFormat ??= outputFile!.GetFileFormatType();

        var setting = new VideoProcessingSettings().ReplaceIfExist()
                                                   .SetInputFiles(videoFile, subsFile)
                                                   .MapArgument("0")
                                                   .MapArgument("1")
                                                   .CopyAllCodec()
                                                   .SubtitlesCodec("mov_text")
                                                   .MetaData("s:s:1")
                                                   .Language(language)
                                                   .Format(outputFormat.Value)
                                                   .SetOutputArguments(outputFile);

        return await ExecuteAsync(setting, cancellationToken ?? new CancellationToken());
    }

    /// <summary>
    /// Downloads executable files ffmpeg.exe and ffprobe.exe from a remote ZIP archive.
    /// </summary>
    /// <exception cref="Exception">
    /// Thrown when either of the files ffmpeg.exe or ffprobe.exe is not found in the ZIP archive.
    /// </exception>
    public static async Task DownloadExecutableFiles()
    {
        var fileName = $"{Guid.NewGuid()}.zip";

        var ffmpegFound = false;
        var ffprobeFound = false;

        try
        {
            // Downloads the ZIP archive from the remote location specified by _zipAddress.
            await FileDownloadProcessor.DownloadFile(_zipAddress, fileName);

            // Open an existing zip file for reading
            using(var zip = ZipFileProcessor.Open(fileName, FileAccess.Read))
            {
                // Read the central directory collection
                var dir = zip.ReadCentralDir();

                // Look for the desired files ffmpeg.exe and ffprobe.exe.
                foreach (var entry in dir)
                {
                    if (Path.GetFileName(entry.FilenameInZip) == "ffmpeg.exe")
                    {
                        zip.ExtractFile(entry, "ffmpeg.exe"); // File found, extract it
                        ffmpegFound = true;
                    }

                    if (Path.GetFileName(entry.FilenameInZip) == "ffprobe.exe")
                    {
                        zip.ExtractFile(entry, "ffprobe.exe"); // File found, extract it
                        ffprobeFound = true;
                    }
                }
            }

            // Check if both the files were found in the ZIP archive.
            if(!ffmpegFound)
                throw new Exception("ffmpeg.exe not found");

            if(!ffprobeFound)
                throw new Exception("ffprobe.exe not found");
        }
        finally
        {
            // Delete the downloaded ZIP archive after extracting the required files.
            if(File.Exists(fileName))
                File.Delete(fileName);
        }
    }
}