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
                settings.VideoCodec(VideoCodecType.PNG);
                settings.Format(FileFormatType.RAWVIDEO);

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

            if(fileFormatType is FileFormatType.MP4 or FileFormatType.M4V or FileFormatType.MKV or FileFormatType.MOV or FileFormatType._3GP)
                settings.AudioCodec(AudioCodecType.COPY).VideoCodec(VideoCodecType.COPY);
            else
                settings.VideoCodec(VideoCodecType.LIBX264).VideoCodecPreset(VideoCodecPresetType.ULTRAFAST).Crf(23).AudioCodec(AudioCodecType.COPY);

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
        var settings = new VideoProcessingSettings().ReplaceIfExist().SetInputFiles(file).Seek(startTime).TimePosition(endTime).VideoCodec(VideoCodecType.LIBX264).Crf(30).SetOutputArguments(outputFile);

        switch(outputFile)
        {
            case null when outputFormat is null:
                throw new Exception("If the outputFile is not specified then the outputFormat must be indicated necessarily");
            case null:
                if (outputFormat is FileFormatType._3GP or FileFormatType.ASF)
                    settings.Format(FileFormatType.MPEG);
                else if (outputFormat is FileFormatType.M2TS)
                {
                    settings.Format(FileFormatType.MPEGTS);
                }
                else
                    settings.Format(outputFormat.Value);


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
    /// <param name="videoCodecType">The type of codec to use for encoding the video.</param>
    /// <param name="outputFormat">The format of the output video file.</param>
    /// <param name="outputFile">The file name and path of the output video.</param>
    /// <param name="pixelFormat">The pixel format of the output video.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A MemoryStream containing the content of the output video, or null if the operation was cancelled.</returns>
    private  async Task<MemoryStream?> ExecuteConvertImagesToVideoAsync(MediaFile file,
                                                                        int frameRate,
                                                                        VideoCodecType videoCodecType,
                                                                        FileFormatType? outputFormat,
                                                                        string? outputFile,
                                                                        string pixelFormat,
                                                                        CancellationToken cancellationToken)
    {
        var settings = new VideoProcessingSettings().ReplaceIfExist()
                                                    .FrameRate(frameRate)
                                                    .SetInputFiles(file)
                                                    .VideoCodec(videoCodecType)
                                                    .PixelFormat(pixelFormat)
                                                    .SetOutputArguments(outputFile);
        if(outputFormat is not null)
            settings.Format(outputFormat.Value);

        return await ExecuteAsync(settings, cancellationToken);
    }

    /// <inheritdoc />
    public async Task ConvertImagesToVideoAsync(MediaFile file,
                                                int frameRate,
                                                string outputFile,
                                                string pixelFormat,
                                                FileFormatType outputFormat,
                                                CancellationToken? cancellationToken = null,
                                                VideoCodecType videoCodecType = VideoCodecType.LIBX264)
    {
        await ExecuteConvertImagesToVideoAsync(file,
                                               frameRate,
                                               videoCodecType,
                                               outputFormat,
                                               outputFile,
                                               pixelFormat,
                                               cancellationToken ?? new CancellationToken());
    }

    /// <inheritdoc />
    public async Task<MemoryStream> ConvertImagesToVideoAsStreamAsync(MediaFile file,
                                                                      int frameRate,
                                                                      string pixelFormat,
                                                                      FileFormatType outputFormat,
                                                                      VideoCodecType videoCodecType = VideoCodecType.LIBX264,
                                                                      CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConvertImagesToVideoAsync(file,
                                                       frameRate,
                                                       videoCodecType,
                                                       outputFormat,
                                                       null,
                                                       pixelFormat,
                                                       cancellationToken ?? new CancellationToken()))!;
    }

    /// <inheritdoc />
    public async Task<byte[]> ConvertImagesToVideoAsBytesAsync(MediaFile file,
                                                               int frameRate,
                                                               string pixelFormat,
                                                               FileFormatType? outputFormat,
                                                               VideoCodecType videoCodecType = VideoCodecType.LIBX264,
                                                               CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConvertImagesToVideoAsync(file,
                                                       frameRate,
                                                       videoCodecType,
                                                       outputFormat,
                                                       null,
                                                       pixelFormat,
                                                       cancellationToken ?? new CancellationToken()))!.ToArray();
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
            case FileFormatType.JPG:
            case FileFormatType.IMAGE2PIPE:
            case FileFormatType.IMAGE2:
                settings.Format(outputImagesPattern == null ? FileFormatType.IMAGE2PIPE : outputFormat.Value);

                break;
            case FileFormatType.PNG:
                settings.VideoCodec(VideoCodecType.PNG);
                settings.Format(FileFormatType.RAWVIDEO);

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
            case FileFormatType.ICO:
                settings.VideoSize(VideoSizeType.CUSTOM, 64, 64);

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
    /// <param name="audioSampleRateType">The audio sample rate type to use when extracting audio.</param>
    /// <param name="audioChannel">The number of audio channels to extract from the video.</param>
    /// <param name="output">The path to save the extracted audio file to. If `null`, the audio will be returned as a `MemoryStream`.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A `MemoryStream` containing the extracted audio, or `null` if `output` is not `null`.</returns>
    private  async Task<MemoryStream?> ExecuteGetAudioFromVideoAsync(MediaFile file,
                                                                     FileFormatType outputFormat,
                                                                     AudioSampleRateType audioSampleRateType,
                                                                     int audioChannel,
                                                                     string? output,
                                                                     CancellationToken cancellationToken)
    {
        var settings = new VideoProcessingSettings().ReplaceIfExist()
                                                    .SetInputFiles(file)
                                                    .DeleteVideo()
                                                    .AudioSampleRate(audioSampleRateType)
                                                    .AudioChannel(audioChannel)
                                                    .Format(outputFormat)
                                                    .SetOutputArguments(output);

        return await ExecuteAsync(settings, cancellationToken);
    }

    /// <inheritdoc />
    public async Task GetAudioFromVideoAsync(MediaFile file,
                                             FileFormatType outputFormat,
                                             string output,
                                             int audioChannel = 2,
                                             CancellationToken? cancellationToken = null,
                                             AudioSampleRateType audioSampleRateType = AudioSampleRateType.Hz44100)
    {
        await ExecuteGetAudioFromVideoAsync(file, outputFormat, audioSampleRateType, audioChannel, output,  cancellationToken ?? new CancellationToken());
    }

    /// <inheritdoc />
    public async Task<MemoryStream> GetAudioFromVideoAsStreamAsync(MediaFile file,
                                                                   FileFormatType outputFormat,
                                                                   int audioChannel = 2,
                                                                   CancellationToken? cancellationToken = null,
                                                                   AudioSampleRateType audioSampleRateType = AudioSampleRateType.Hz44100)
    {
        return (await ExecuteGetAudioFromVideoAsync(file, outputFormat, audioSampleRateType, audioChannel, null,  cancellationToken ?? new CancellationToken()))!;
    }

    /// <inheritdoc />
    public async Task<byte[]> GetAudioFromVideoAsBytesAsync(MediaFile file,
                                                            FileFormatType outputFormat,
                                                            int audioChannel = 2,
                                                            CancellationToken? cancellationToken = null,
                                                            AudioSampleRateType audioSampleRateType = AudioSampleRateType.Hz44100)
    {
        return (await ExecuteGetAudioFromVideoAsync(file, outputFormat, audioSampleRateType, audioChannel, null,  cancellationToken ?? new CancellationToken()))!
            .ToArray();
    }

    //======================================================================================================================================================================

    /// <summary>
    /// Executes the conversion of a video file to the specified output format.
    /// </summary>
    /// <param name="file">The video file to be converted.</param>
    /// <param name="output">The output file path for the converted video. Can be null if output is to be returned as a stream.</param>
    /// <param name="outputFormat">The desired output format for the converted video.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="MemoryStream"/> that contains the converted video, or null if output is specified as a file path.</returns>
    private  async Task<MemoryStream?> ExecuteConvertVideoAsync(MediaFile file, string? output, FileFormatType outputFormat, CancellationToken cancellationToken)
    {
        return await ExecuteAsync(new VideoProcessingSettings().ReplaceIfExist()
                                                               .SetInputFiles(file)
                                                               .MapMetadata()
                                                               .Format(outputFormat)
                                                               .SetOutputArguments(output),
                                  cancellationToken);
    }

    /// <inheritdoc />
    public async Task ConvertVideoAsync(MediaFile file, string output, FileFormatType outputFormat, CancellationToken? cancellationToken = null)
    {
        await ExecuteConvertVideoAsync(file, output, outputFormat, cancellationToken ?? new CancellationToken());
    }

    /// <inheritdoc />
    public async Task<MemoryStream> ConvertVideoAsStreamAsync(MediaFile file, FileFormatType outputFormat, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConvertVideoAsync(file, null, outputFormat, cancellationToken ?? new CancellationToken()))!;
    }

    /// <inheritdoc />
    public async Task<byte[]> ConvertVideoAsBytesAsync(MediaFile file, FileFormatType outputFormat, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConvertVideoAsync(file, null, outputFormat, cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    //======================================================================================================================================================================

    /// <summary>
    /// Executes the task of adding a watermark to the specified video.
    /// </summary>
    /// <param name="videoFile">The video file to add watermark to.</param>
    /// <param name="watermarkFile">The watermark file to be added to the video.</param>
    /// <param name="position">The position of the watermark in the video.</param>
    /// <param name="output">The path and name of the output file.</param>
    /// <param name="outputFormat">The format of the output file.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A MemoryStream object that contains the data of the output file.</returns>
    private  async Task<MemoryStream?> ExecuteAddWaterMarkToVideoAsync(MediaFile videoFile,
                                                                       MediaFile watermarkFile,
                                                                       PositionType position,
                                                                       string? output,
                                                                       FileFormatType outputFormat,
                                                                       CancellationToken cancellationToken)
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
                                                    .Format(outputFormat)
                                                    .SetOutputArguments(output);

        return await ExecuteAsync(settings, cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddWaterMarkToVideoAsync(MediaFile videoFile,
                                               MediaFile watermarkFile,
                                               PositionType position,
                                               string output,
                                               FileFormatType outputFormat,
                                               CancellationToken? cancellationToken = null)
    {
        await ExecuteAddWaterMarkToVideoAsync(videoFile, watermarkFile, position, output, outputFormat, cancellationToken ?? new CancellationToken());
    }

    /// <inheritdoc />
    public async Task<MemoryStream> AddWaterMarkToVideoAsStreamAsync(MediaFile videoFile,
                                                                     MediaFile watermarkFile,
                                                                     PositionType position,
                                                                     FileFormatType outputFormat,
                                                                     CancellationToken? cancellationToken = null)
    {
        return (await ExecuteAddWaterMarkToVideoAsync(videoFile, watermarkFile, position, null, outputFormat, cancellationToken ?? new CancellationToken()))!;
    }

    /// <inheritdoc />
    public async Task<byte[]> AddWaterMarkToVideoAsBytesAsync(MediaFile videoFile,
                                                              MediaFile watermarkFile,
                                                              PositionType position,
                                                              FileFormatType outputFormat,
                                                              CancellationToken? cancellationToken = null)
    {
        return (await ExecuteAddWaterMarkToVideoAsync(videoFile, watermarkFile, position, null, outputFormat, cancellationToken ?? new CancellationToken()))!
            .ToArray();
    }

    //======================================================================================================================================================================

    /// <summary>
    /// Extracts the video from a media file.
    /// </summary>
    /// <param name="file">The media file from which the video should be extracted.</param>
    /// <param name="output">The output path where the extracted video should be stored. If this is `null`, the extracted video will be stored in memory.</param>
    /// <param name="outputFormat">The format of the extracted video file.</param>
    /// <param name="videoCodecType">The video codec to be used while extracting the video.</param>
    /// <param name="pixelFormat">The pixel format of the extracted video.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A `MemoryStream` containing the extracted video, or `null` if the `output` parameter was provided.</returns>
    private  async Task<MemoryStream?> ExecuteExtractVideoFromFileAsync(MediaFile file,
                                                                        string? output,
                                                                        FileFormatType outputFormat,
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
                                                    .Format(outputFormat)
                                                    .SetOutputArguments(output);

        return await ExecuteAsync(settings, cancellationToken);
    }

    /// <inheritdoc />
    public async Task ExtractVideoFromFileAsync(MediaFile file,
                                                string output,
                                                FileFormatType outputFormat,
                                                VideoCodecType videoCodecType = VideoCodecType.COPY,
                                                string pixelFormat = "yuv420p",
                                                CancellationToken? cancellationToken = null)
    {
        await ExecuteExtractVideoFromFileAsync(file, output, outputFormat, videoCodecType, pixelFormat, cancellationToken ?? new CancellationToken());
    }

    /// <inheritdoc />
    public async Task<MemoryStream> ExtractVideoFromFileAsStreamAsync(MediaFile file,
                                                                      FileFormatType outputFormat,
                                                                      VideoCodecType videoCodecType = VideoCodecType.COPY,
                                                                      string pixelFormat = "yuv420p",
                                                                      CancellationToken? cancellationToken = null)
    {
        return (await ExecuteExtractVideoFromFileAsync(file, null, outputFormat, videoCodecType, pixelFormat, cancellationToken ?? new CancellationToken()))!;
    }

    /// <inheritdoc />
    public async Task<byte[]> ExtractVideoFromFileAsBytesAsync(MediaFile file,
                                                               FileFormatType outputFormat,
                                                               VideoCodecType videoCodecType = VideoCodecType.COPY,
                                                               string pixelFormat = "yuv420p",
                                                               CancellationToken? cancellationToken = null)
    {
        return (await ExecuteExtractVideoFromFileAsync(file, null, outputFormat, videoCodecType, pixelFormat, cancellationToken ?? new CancellationToken()))!
            .ToArray();
    }

    //======================================================================================================================================================================

    /// <summary>
    /// Adds audio to a video file.
    /// </summary>
    /// <param name="audioFile">The audio file to add to the video.</param>
    /// <param name="videoFile">The video file to add the audio to.</param>
    /// <param name="output">The output file path for the audio-added video. Default is null.</param>
    /// <param name="videoCodecType">The video codec type to use during the addition process.</param>
    /// <param name="audioCodecType">The audio codec type to use during the addition process.</param>
    /// <param name="outputFormat">The output file format type of the audio-added video.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the asynchronous operation.</param>
    /// <returns>A memory stream that represents the audio-added video.</returns>
    private  async Task<MemoryStream?> ExecuteAddAudioToVideoAsync(MediaFile audioFile,
                                                                   MediaFile videoFile,
                                                                   string? output,
                                                                   VideoCodecType videoCodecType,
                                                                   AudioCodecType audioCodecType,
                                                                   FileFormatType outputFormat,
                                                                   CancellationToken cancellationToken)
    {
        var settings = new VideoProcessingSettings().ReplaceIfExist()
                                                    .SetInputFiles(audioFile, videoFile)
                                                    .VideoCodec(videoCodecType)
                                                    .AudioCodec(audioCodecType)
                                                    .Format(outputFormat)
                                                    .SetOutputArguments(output);

        return await ExecuteAsync(settings, cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddAudioToVideoAsync(MediaFile audioFile,
                                           MediaFile videoFile,
                                           string output,
                                           FileFormatType outputFormat,
                                           CancellationToken? cancellationToken = null,
                                           AudioCodecType audioCodecType = AudioCodecType.COPY,
                                           VideoCodecType videoCodecType = VideoCodecType.COPY)
    {
        await ExecuteAddAudioToVideoAsync(audioFile,
                                          videoFile,
                                          output,
                                          videoCodecType,
                                          audioCodecType,
                                          outputFormat,
                                          cancellationToken ?? new CancellationToken());
    }

    /// <inheritdoc />
    public async Task<MemoryStream> AddAudioToVideoAsStreamAsync(MediaFile audioFile,
                                                                 MediaFile videoFile,
                                                                 FileFormatType outputFormat,
                                                                 CancellationToken? cancellationToken = null,
                                                                 AudioCodecType audioCodecType = AudioCodecType.COPY,
                                                                 VideoCodecType videoCodecType = VideoCodecType.COPY)
    {
        return (await ExecuteAddAudioToVideoAsync(audioFile,
                                                  videoFile,
                                                  null,
                                                  videoCodecType,
                                                  audioCodecType,
                                                  outputFormat,
                                                  cancellationToken ?? new CancellationToken()))!;
    }

    /// <inheritdoc />
    public async Task<byte[]> AddAudioToVideoAsBytesAsync(MediaFile audioFile,
                                                          MediaFile videofile,
                                                          FileFormatType outputFormat,
                                                          CancellationToken? cancellationToken = null,
                                                          AudioCodecType audioCodecType = AudioCodecType.COPY,
                                                          VideoCodecType videoCodecType = VideoCodecType.COPY)
    {
        return (await ExecuteAddAudioToVideoAsync(audioFile,
                                                  videofile,
                                                  null,
                                                  videoCodecType,
                                                  audioCodecType,
                                                  outputFormat,
                                                  cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    //======================================================================================================================================================================

    /// <summary>
    /// Converts a video file to a GIF using the given parameters.
    /// </summary>
    /// <param name="file">The video file to be converted.</param>
    /// <param name="fps">The number of frames per second in the output GIF.</param>
    /// <param name="scale">The scaling factor to be applied to the video before converting it to a GIF.</param>
    /// <param name="loop">The number of times the output GIF should loop. Set to 0 for infinite looping.</param>
    /// <param name="output">The path to the output file. Set to null if the result should be returned as a stream.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the task.</param>
    /// <returns>The resulting GIF as a memory stream, or null if an output file was specified.</returns>
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

    /// <inheritdoc />
    public async Task ConvertVideoToGifAsync(MediaFile file, int fps, int scale, int loop, string output, CancellationToken? cancellationToken = null)
    {
        await ExecuteConvertVideoToGifAsync(file, fps, scale, loop, output, cancellationToken ?? new CancellationToken());
    }

    /// <inheritdoc />
    public async Task<MemoryStream> ConvertVideoToGifAsStreamAsync(MediaFile file, int fps, int scale, int loop, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConvertVideoToGifAsync(file, fps, scale, loop, null, cancellationToken ?? new CancellationToken()))!;
    }

    /// <inheritdoc />
    public async Task<byte[]> ConvertVideoToGifAsBytesAsync(MediaFile file, int fps, int scale, int loop, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConvertVideoToGifAsync(file, fps, scale, loop, null, cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    //======================================================================================================================================================================

    /// <summary>
    /// Asynchronously executes the compression of the input video file.
    /// </summary>
    /// <param name="file">The input media file to be compressed.</param>
    /// <param name="crf">The constant rate factor used for compression.</param>
    /// <param name="output">The desired name of the output file, including the file extension.
    ///                     If `null`, the original file name will be used.</param>
    /// <param name="outputFormat">The desired file format for the output file.</param>
    /// <param name="videoCodecType">The type of video codec to be used for compression.</param>
    /// <param name="cancellationToken">A cancellation token to stop the compression process.</param>
    /// <returns>A memory stream of the compressed video, or `null` if the compression failed.</returns>
    private  async Task<MemoryStream?> ExecuteCompressVideoAsync(MediaFile file,
                                                                 int crf,
                                                                 string? output,
                                                                 FileFormatType outputFormat,
                                                                 VideoCodecType videoCodecType,
                                                                 CancellationToken cancellationToken)
    {
        var setting = new VideoProcessingSettings().ReplaceIfExist()
                                                   .SetInputFiles(file)
                                                   .VideoCodec(videoCodecType)
                                                   .Crf(crf)
                                                   .VideoCodecPreset(VideoCodecPresetType.SLOW)
                                                   .VideoBitRate(500)
                                                   .MaxRate(500)
                                                   .BufSize(1000)
                                                   .VideoSize(VideoSizeType.CUSTOM, -2, 720)
                                                   .Format(outputFormat)
                                                   .SetOutputArguments(output);

        return await ExecuteAsync(setting, cancellationToken);
    }

    /// <inheritdoc />
    public async Task CompressVideoAsync(MediaFile file,
                                         int compressionRatio,
                                         string output,
                                         FileFormatType outputFormat,
                                         VideoCodecType videoCodecType = VideoCodecType.H264,
                                         CancellationToken? cancellationToken = null)
    {
        await ExecuteCompressVideoAsync(file, compressionRatio, output, outputFormat, videoCodecType, cancellationToken ?? new CancellationToken());
    }

    /// <inheritdoc />
    public async Task<MemoryStream> CompressVideoAsStreamAsync(MediaFile file,
                                                               int compressionRatio,
                                                               FileFormatType outputFormat,
                                                               VideoCodecType videoCodecType = VideoCodecType.H264,
                                                               CancellationToken? cancellationToken = null)
    {
        return (await ExecuteCompressVideoAsync(file, compressionRatio, null, outputFormat, videoCodecType, cancellationToken ?? new CancellationToken()))!;
    }

    /// <inheritdoc />
    public async Task<byte[]> CompressVideoAsBytesAsync(MediaFile file,
                                                        int compressionRatio,
                                                        FileFormatType outputFormat,
                                                        VideoCodecType videoCodecType = VideoCodecType.H264,
                                                        CancellationToken? cancellationToken = null)
    {
        return (await ExecuteCompressVideoAsync(file, compressionRatio, null, outputFormat, videoCodecType, cancellationToken ?? new CancellationToken()))!
            .ToArray();
    }

    //======================================================================================================================================================================

    /// <summary>
    /// Executes the compression of an image file.
    /// </summary>
    /// <param name="file">The image file to be compressed.</param>
    /// <param name="level">The compression level to be applied to the image file.</param>
    /// <param name="output">The output file path, if specified. If null, the output will be returned as a memory stream.</param>
    /// <param name="outputFormat">The file format type for the output file.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A memory stream containing the compressed image data, or null if the output file path is specified.</returns>
    private  async Task<MemoryStream?> ExecuteCompressImageAsync(MediaFile file,
                                                                 int level,
                                                                 string? output,
                                                                 FileFormatType outputFormat,
                                                                 CancellationToken cancellationToken)
    {
        var setting = new VideoProcessingSettings().ReplaceIfExist()
                                                   .HideBanner()
                                                   .SetInputFiles(file)
                                                   .CompressionLevel(level)
                                                   .Format(outputFormat)
                                                   .SetOutputArguments(output);

        return await ExecuteAsync(setting, cancellationToken);
    }

    /// <inheritdoc />
    public async Task CompressImageAsync(MediaFile file, int level, string output, FileFormatType outputFormat, CancellationToken? cancellationToken = null)
    {
        await ExecuteCompressImageAsync(file, level, output, outputFormat, cancellationToken ?? new CancellationToken());
    }

    /// <inheritdoc />
    public async Task<MemoryStream> CompressImageAsStreamAsync(MediaFile file,
                                                               int level,
                                                               FileFormatType outputFormat,
                                                               CancellationToken? cancellationToken = null)
    {
        return (await ExecuteCompressImageAsync(file, level, null, outputFormat, cancellationToken ?? new CancellationToken()))!;
    }

    /// <inheritdoc />
    public async Task<byte[]> CompressImageAsBytesAsync(MediaFile file, int level, FileFormatType outputFormat, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteCompressImageAsync(file, level, null, outputFormat, cancellationToken ?? new CancellationToken()))!.ToArray();
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
    private  async Task CreateMpegts(MediaFile inputFile,
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
    /// <param name="videoCodecType">The video codec type to be used.</param>
    /// <param name="audioCodecType">The audio codec type to be used.</param>
    /// <param name="videoBSF">The video bitstream filter to be applied.</param>
    /// <param name="outputFormat">The output file format type.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A memory stream containing the result of the operation.</returns>
    private  async Task<MemoryStream?> ExecuteConcatVideosAsync(MediaFile[] files,
                                                                string? outputFile,
                                                                VideoCodecType videoCodecType,
                                                                AudioCodecType audioCodecType,
                                                                string videoBSF,
                                                                FileFormatType outputFormat,
                                                                CancellationToken cancellationToken)
    {
        var intermediateFiles = new List<string>();

        // Create intermediate mpegts files for each of the input files
        for (var i = 0; i < files.Length; i++)
        {
            var outputFileName = $"_{i}{Guid.NewGuid()}.ts";
            intermediateFiles.Add(outputFileName);
            await CreateMpegts(files[i], outputFileName, videoCodecType, audioCodecType, videoBSF, cancellationToken);
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
                                                   .Format(outputFormat)
                                                   .SetOutputArguments(outputFile);

        var result = await ExecuteAsync(setting, cancellationToken);

        // Delete the intermediate files
        foreach (var file in intermediateFiles)
        {
            if(File.Exists(file))
                File.Delete(file);
        }

        return result;
    }

    /// <inheritdoc />
    public async Task ConcatVideosAsync(MediaFile[] files,
                                        string output,
                                        FileFormatType outputFormat,
                                        string videoBSF = "h264_mp4toannexb",
                                        CancellationToken? cancellationToken = null,
                                        VideoCodecType videoCodecType = VideoCodecType.LIBX264,
                                        AudioCodecType audioCodecType = AudioCodecType.AAC)
    {
        await ExecuteConcatVideosAsync(files, output, videoCodecType, audioCodecType, videoBSF, outputFormat, cancellationToken ?? new CancellationToken());
    }

    /// <inheritdoc />
    public async Task<MemoryStream> ConcatVideosAsStreamAsync(MediaFile[] files,
                                                              FileFormatType outputFormat,
                                                              string videoBSF = "h264_mp4toannexb",
                                                              VideoCodecType videoCodecType = VideoCodecType.LIBX264,
                                                              AudioCodecType audioCodecType = AudioCodecType.AAC,
                                                              CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConcatVideosAsync(files,
                                               null,
                                               videoCodecType,
                                               audioCodecType,
                                               videoBSF,
                                               outputFormat,
                                               cancellationToken ?? new CancellationToken()))!;
    }

    /// <inheritdoc />
    public async Task<byte[]> ConcatVideosAsBytesAsync(MediaFile[] files,
                                                       FileFormatType outputFormat,
                                                       string videoBSF = "h264_mp4toannexb",
                                                       CancellationToken? cancellationToken = null,
                                                       VideoCodecType videoCodecType = VideoCodecType.LIBX264,
                                                       AudioCodecType audioCodecType = AudioCodecType.AAC)
    {
        return (await ExecuteConcatVideosAsync(files,
                                               null,
                                               videoCodecType,
                                               audioCodecType,
                                               videoBSF,
                                               outputFormat,
                                               cancellationToken ?? new CancellationToken()))!.ToArray();
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
    private  async Task<MemoryStream?> ExecuteAddHardSubtitlesAsync(MediaFile videoFile,
                                                                    MediaFile subsFile,
                                                                    string language,
                                                                    string? outputFile,
                                                                    FileFormatType outputFormat,
                                                                    CancellationToken cancellationToken)
    {
        var setting = new VideoProcessingSettings().ReplaceIfExist()
                                                   .SetInputFiles(videoFile, subsFile)
                                                   .MapArgument("0")
                                                   .MapArgument("1")
                                                   .CopyAllCodec()
                                                   .SubtitlesCodec("mov_text")
                                                   .MetaData("s:s:1")
                                                   .Language(language)
                                                   .Format(outputFormat)
                                                   .SetOutputArguments(outputFile);

        return await ExecuteAsync(setting, cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddHardSubtitlesAsync(MediaFile videoFile,
                                            MediaFile subsFile,
                                            string language,
                                            string outputFile,
                                            FileFormatType outputFormat,
                                            CancellationToken? cancellationToken = null)
    {
        await ExecuteAddHardSubtitlesAsync(videoFile, subsFile, language, outputFile, outputFormat, cancellationToken ?? new CancellationToken());
    }

    /// <inheritdoc />
    public async Task<MemoryStream> AddHardSubtitlesAsStreamAsync(MediaFile videoFile,
                                                                  MediaFile subsFile,
                                                                  string language,
                                                                  FileFormatType outputFormat,
                                                                  CancellationToken? cancellationToken = null)
    {
        return (await ExecuteAddHardSubtitlesAsync(videoFile, subsFile, language, null, outputFormat, cancellationToken ?? new CancellationToken()))!;
    }

    /// <inheritdoc />
    public async Task<byte[]> AddHardSubtitlesAsBytesAsync(MediaFile videoFile,
                                                           MediaFile subsFile,
                                                           string language,
                                                           FileFormatType outputFormat,
                                                           CancellationToken? cancellationToken = null)
    {
        return (await ExecuteAddHardSubtitlesAsync(videoFile, subsFile, language, null, outputFormat, cancellationToken ?? new CancellationToken()))!.ToArray();
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