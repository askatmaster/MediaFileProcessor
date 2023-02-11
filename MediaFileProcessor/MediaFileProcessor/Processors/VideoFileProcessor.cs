using System.Text;
using MediaFileProcessor.Extensions;
using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Enums;
using MediaFileProcessor.Models.Settings;
namespace MediaFileProcessor.Processors;

/// <summary>
/// This class is responsible for processing video files.
/// </summary>
public class VideoFileProcessor
{
    /// <summary>
    /// The name of the ffmpeg executable.
    /// </summary>
    private static readonly string _ffmpeg = "ffmpeg.exe";

    /// <summary>
    /// The name of the _ffprobe executable.
    /// </summary>
    private static readonly string _ffprobe = "ffprobe.exe";

    /// <summary>
    /// The address from which the ffmpeg and ffprobe executable can be downloaded.
    /// </summary>
    private static readonly string _zipAddress = "https://www.gyan.dev/ffmpeg/builds/ffmpeg-release-essentials.zip";

    /// <summary>
    /// Executes the video processing with the provided settings and cancellation token.
    /// </summary>
    /// <param name="settings">The video processing settings to use for processing the video.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the processing if necessary.</param>
    /// <returns>A MemoryStream representing the processed video, or null if the process was cancelled.</returns>
    public async Task<MemoryStream?> ExecuteAsync(VideoProcessingSettings settings, CancellationToken cancellationToken)
    {
        var process = new MediaFileProcess(_ffmpeg,
                                           settings.GetProcessArguments(),
                                           settings,
                                           settings.GetInputStreams(),
                                           settings.GetInputPipeNames());

        return await process.ExecuteAsync(cancellationToken);
    }

    //======================================================================================================================================================================

    /// <summary>
    /// Executes the process of getting a frame from a video at the specified timestamp.
    /// </summary>
    /// <param name="timestamp">The timestamp at which to extract the frame from the video.</param>
    /// <param name="file">The media file representing the video to extract the frame from.</param>
    /// <param name="outputFile">The output file to save the extracted frame to (optional).</param>
    /// <param name="outputFormat">The format to use for the output file (if specified).</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the process if necessary.</param>
    /// <returns>A MemoryStream representing the extracted frame, or null if the process was cancelled.</returns>
    private async Task<MemoryStream?> ExecuteGetFrameFromVideoAsync(TimeSpan timestamp,
                                                                    MediaFile file,
                                                                    string? outputFile,
                                                                    FileFormatType outputFormat,
                                                                    CancellationToken cancellationToken)
    {
        var settings = new VideoProcessingSettings().ReplaceIfExist()
                                                    .Seek(timestamp)
                                                    .SetInputFiles(file)
                                                    .FramesNumber(1)
                                                    .Format(outputFormat)
                                                    .SetOutputArguments(outputFile);


        return await ExecuteAsync(settings, cancellationToken);
    }

    /// <summary>
    /// Gets a frame from a video at the specified timestamp and saves it to a specified output file.
    /// </summary>
    /// <param name="timestamp">The timestamp at which to extract the frame from the video.</param>
    /// <param name="file">The media file representing the video to extract the frame from.</param>
    /// <param name="outputFile">The output file to save the extracted frame to.</param>
    /// <param name="outputFormat">The format to use for the output file.</param>
    /// <param name="cancellationToken">The optional cancellation token used to cancel the process if necessary.</param>
    public  async Task GetFrameFromVideoAsync(TimeSpan timestamp,
                                              MediaFile file,
                                              string outputFile,
                                              FileFormatType outputFormat,
                                              CancellationToken? cancellationToken = null)
    {
        await ExecuteGetFrameFromVideoAsync(timestamp, file, outputFile, outputFormat, cancellationToken ?? new CancellationToken());
    }

    /// <summary>
    /// Retrieves a frame from a video file at a specified time and returns it as a memory stream.
    /// </summary>
    /// <param name="timestamp">The time at which the frame should be retrieved.</param>
    /// <param name="file">The media file from which the frame should be retrieved.</param>
    /// <param name="outputFormat">The format in which the retrieved frame should be returned.</param>
    /// <param name="cancellationToken">The cancellation token for the operation.</param>
    /// <returns>A memory stream containing the requested frame.</returns>
    public  async Task<MemoryStream> GetFrameFromVideoAsStreamAsync(TimeSpan timestamp,
                                                                    MediaFile file,
                                                                    FileFormatType outputFormat,
                                                                    CancellationToken? cancellationToken = null)
    {
        return (await ExecuteGetFrameFromVideoAsync(timestamp, file, null, outputFormat, cancellationToken ?? new CancellationToken()))!;
    }

    /// <summary>
    /// Retrieves a frame from a video file at a specified time and returns it as a byte array.
    /// </summary>
    /// <param name="timestamp">The time at which the frame should be retrieved.</param>
    /// <param name="file">The media file from which the frame should be retrieved.</param>
    /// <param name="outputFormat">The format in which the retrieved frame should be returned.</param>
    /// <param name="cancellationToken">The cancellation token for the operation.</param>
    /// <returns>A byte array containing the requested frame.</returns>
    public  async Task<byte[]> GetFrameFromVideoAsBytesAsync(TimeSpan timestamp, MediaFile file, FileFormatType outputFormat, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteGetFrameFromVideoAsync(timestamp, file, null, outputFormat, cancellationToken ?? new CancellationToken()))!.ToArray();
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
    private  async Task<MemoryStream?> ExecuteCutVideoAsync(TimeSpan startTime,
                                                            TimeSpan endTime,
                                                            MediaFile file,
                                                            string? outputFile,
                                                            FileFormatType outputFormat,
                                                            CancellationToken cancellationToken)
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

    /// <summary>
    /// Cuts a portion of a video file and saves the result to a specified file.
    /// </summary>
    /// <param name="startTime">The start time of the portion to be cut.</param>
    /// <param name="endTime">The end time of the portion to be cut.</param>
    /// <param name="file">The media file to be cut.</param>
    /// <param name="outputFile">The name of the output file.</param>
    /// <param name="outputFormat">The format in which the cut portion should be saved.</param>
    /// <param name="cancellationToken">The cancellation token for the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public  async Task CutVideoAsync(TimeSpan startTime,
                                     TimeSpan endTime,
                                     MediaFile file,
                                     string outputFile,
                                     FileFormatType outputFormat,
                                     CancellationToken? cancellationToken = null)
    {
        await ExecuteCutVideoAsync(startTime, endTime, file, outputFile, outputFormat, cancellationToken ?? new CancellationToken());
    }

    /// <summary>
    /// Cuts a portion of a video file and returns the result as a memory stream.
    /// </summary>
    /// <param name="startTime">The start time of the portion to be cut.</param>
    /// <param name="endTime">The end time of the portion to be cut.</param>
    /// <param name="file">The media file to be cut.</param>
    /// <param name="outputFormat">The format in which the cut portion should be saved.</param>
    /// <param name="cancellationToken">The cancellation token for the operation.</param>
    /// <returns>A memory stream that contains the cut portion of the video file.</returns>
    public  async Task<MemoryStream> CutVideoAsStreamAsync(TimeSpan startTime,
                                                           TimeSpan endTime,
                                                           MediaFile file,
                                                           FileFormatType outputFormat,
                                                           CancellationToken? cancellationToken = null)
    {
        return (await ExecuteCutVideoAsync(startTime, endTime, file, null, outputFormat, cancellationToken ?? new CancellationToken()))!;
    }

    /// <summary>
    /// Cuts a portion of a video file and returns the result as a byte array.
    /// </summary>
    /// <param name="startTime">The start time of the portion to be cut.</param>
    /// <param name="endTime">The end time of the portion to be cut.</param>
    /// <param name="file">The media file to be cut.</param>
    /// <param name="outputFormat">The format in which the cut portion should be saved.</param>
    /// <param name="cancellationToken">The cancellation token for the operation.</param>
    /// <returns>A byte array that contains the cut portion of the video file.</returns>
    public  async Task<byte[]> CutVideoAsBytesAsync(TimeSpan startTime,
                                                    TimeSpan endTime,
                                                    MediaFile file,
                                                    FileFormatType outputFormat,
                                                    CancellationToken? cancellationToken = null)
    {
        return (await ExecuteCutVideoAsync(startTime, endTime, file, null, outputFormat, cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    //======================================================================================================================================================================

    /// <summary>
    /// Executes the conversion of a set of images to a video file.
    /// </summary>
    /// <param name="file">The file containing the images to be converted into a video.</param>
    /// <param name="frameRate">The number of frames per second in the output video.</param>
    /// <param name="videoCodecType">The type of codec to use for encoding the video.</param>
    /// <param name="outputFileFormatType">The format of the output video file.</param>
    /// <param name="outputFile">The file name and path of the output video.</param>
    /// <param name="pixelFormat">The pixel format of the output video.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A MemoryStream containing the content of the output video, or null if the operation was cancelled.</returns>
    private  async Task<MemoryStream?> ExecuteConvertImagesToVideoAsync(MediaFile file,
                                                                        int frameRate,
                                                                        VideoCodecType videoCodecType,
                                                                        FileFormatType? outputFileFormatType,
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
        if(outputFileFormatType is not null)
            settings.Format(outputFileFormatType.Value);

        return await ExecuteAsync(settings, cancellationToken);
    }

    /// <summary>
    /// Converts images to video and saves it to the specified file path.
    /// </summary>
    /// <param name="file">The file that contains the list of images to be converted to video.</param>
    /// <param name="frameRate">The number of frames per second for the resulting video.</param>
    /// <param name="outputFile">The path to the output video file. The file will be overwritten if it already exists.</param>
    /// <param name="pixelFormat">The pixel format for the output video.</param>
    /// <param name="outputFileFormatType">The format type for the output video file.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to cancel the asynchronous operation.</param>
    /// <param name="videoCodecType">The codec to use for encoding the video. Default is VideoCodecType.LIBX264.</param>
    public  async Task ConvertImagesToVideoAsync(MediaFile file,
                                                 int frameRate,
                                                 string outputFile,
                                                 string pixelFormat,
                                                 FileFormatType outputFileFormatType,
                                                 CancellationToken? cancellationToken = null,
                                                 VideoCodecType videoCodecType = VideoCodecType.LIBX264)
    {
        await ExecuteConvertImagesToVideoAsync(file,
                                               frameRate,
                                               videoCodecType,
                                               outputFileFormatType,
                                               outputFile,
                                               pixelFormat,
                                               cancellationToken ?? new CancellationToken());
    }

    /// <summary>
    /// Converts a set of images into a video.
    /// </summary>
    /// <param name="file">The set of images to be converted into a video.</param>
    /// <param name="frameRate">The frame rate of the output video.</param>
    /// <param name="pixelFormat">The pixel format of the output video.</param>
    /// <param name="outputFileFormatType">The format of the output video.</param>
    /// <param name="videoCodecType">The codec to be used for encoding the video. The default value is VideoCodecType.LIBX264.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A memory stream that contains the data of the converted video.</returns>
    public  async Task<MemoryStream> ConvertImagesToVideoAsStreamAsync(MediaFile file,
                                                                       int frameRate,
                                                                       string pixelFormat,
                                                                       FileFormatType outputFileFormatType,
                                                                       VideoCodecType videoCodecType = VideoCodecType.LIBX264,
                                                                       CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConvertImagesToVideoAsync(file,
                                                       frameRate,
                                                       videoCodecType,
                                                       outputFileFormatType,
                                                       null,
                                                       pixelFormat,
                                                       cancellationToken ?? new CancellationToken()))!;
    }

    /// <summary>
    /// Converts a set of images to a video as a byte array.
    /// </summary>
    /// <param name="file">The input image file.</param>
    /// <param name="frameRate">The number of frames per second in the output video.</param>
    /// <param name="pixelFormat">The desired pixel format for the output video.</param>
    /// <param name="outputFileFormatType">The desired format type of the output video.</param>
    /// <param name="videoCodecType">The desired codec type for the output video (default is VideoCodecType.LIBX264).</param>
    /// <param name="cancellationToken">A cancellation token to stop the task (optional).</param>
    /// <returns>The output video as a byte array.</returns>
    public  async Task<byte[]> ConvertImagesToVideoAsBytesAsync(MediaFile file,
                                                                int frameRate,
                                                                string pixelFormat,
                                                                FileFormatType? outputFileFormatType,
                                                                VideoCodecType videoCodecType = VideoCodecType.LIBX264,
                                                                CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConvertImagesToVideoAsync(file,
                                                       frameRate,
                                                       videoCodecType,
                                                       outputFileFormatType,
                                                       null,
                                                       pixelFormat,
                                                       cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    //======================================================================================================================================================================

    /// <summary>
    /// Converts the video to images and returns the result as a MemoryStream.
    /// </summary>
    /// <param name="file">The video file to convert.</param>
    /// <param name="outputFileFormatType">The output file format type for the images.</param>
    /// <param name="outputImagesPattern">The pattern for the output images file names.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The result of the conversion as a MemoryStream.</returns>
    private  async Task<MemoryStream?> ExecuteConvertVideoToImagesAsync(MediaFile file,
                                                                        FileFormatType? outputFileFormatType,
                                                                        string? outputImagesPattern,
                                                                        CancellationToken cancellationToken)
    {
        var settings = new VideoProcessingSettings().ReplaceIfExist().SetInputFiles(file).SetOutputArguments(outputImagesPattern);

        switch(outputFileFormatType)
        {
            case FileFormatType.JPG:
            case FileFormatType.IMAGE2PIPE:
            case FileFormatType.IMAGE2:
            case FileFormatType.JPEG:
                settings.Format(outputImagesPattern == null ? FileFormatType.IMAGE2PIPE : outputFileFormatType.Value);

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

    /// <summary>
    /// Converts a video file to image files.
    /// </summary>
    /// <param name="file">The input video file</param>
    /// <param name="outputFormatType">The output image format type</param>
    /// <param name="outputImagesPattern">The pattern to name the output images</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation</param>
    public  async Task ConvertVideoToImagesAsync(MediaFile file, FileFormatType outputFormatType, string outputImagesPattern, CancellationToken? cancellationToken = null)
    {
        await ExecuteConvertVideoToImagesAsync(file, outputFormatType, outputImagesPattern,  cancellationToken ?? new CancellationToken());
    }

    /// <summary>
    /// Converts a video file to a multi-stream of images.
    /// </summary>
    /// <param name="file">The input video file</param>
    /// <param name="outputFormatType">The output image format type</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation</param>
    /// <returns>The multi-stream of images</returns>
    public  async Task<MultiStream> ConvertVideoToImagesAsStreamAsync(MediaFile file, FileFormatType outputFormatType, CancellationToken? cancellationToken = null)
    {
        var stream = (await ExecuteConvertVideoToImagesAsync(file, outputFormatType, null,  cancellationToken ?? new CancellationToken()))!;

        return stream.GetMultiStreamBySignature(FilesSignatures.GetSignature(outputFormatType));
    }

    /// <summary>
    /// Converts a video file to a multi-array of image bytes.
    /// </summary>
    /// <param name="file">The input video file</param>
    /// <param name="outputFormatType">The output image format type</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation</param>
    /// <returns>The multi-array of image bytes</returns>
    public  async Task<byte[][]> ConvertVideoToImagesAsBytesAsync(MediaFile file, FileFormatType outputFormatType, CancellationToken? cancellationToken = null)
    {
        var stream = (await ExecuteConvertVideoToImagesAsync(file, outputFormatType, null,  cancellationToken ?? new CancellationToken()))!;

        return stream.GetMultiStreamBySignature(FilesSignatures.GetSignature(outputFormatType)).ReadAsDataArray();
    }

    //======================================================================================================================================================================

    /// <summary>
    /// Extracts audio from a video file.
    /// </summary>
    /// <param name="file">The video file to extract audio from.</param>
    /// <param name="outputFileFormatType">The format to save the extracted audio as.</param>
    /// <param name="audioSampleRateType">The audio sample rate type to use when extracting audio.</param>
    /// <param name="audioChannel">The number of audio channels to extract from the video.</param>
    /// <param name="output">The path to save the extracted audio file to. If `null`, the audio will be returned as a `MemoryStream`.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A `MemoryStream` containing the extracted audio, or `null` if `output` is not `null`.</returns>
    private  async Task<MemoryStream?> ExecuteGetAudioFromVideoAsync(MediaFile file,
                                                                     FileFormatType outputFileFormatType,
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
                                                    .Format(outputFileFormatType)
                                                    .SetOutputArguments(output);

        return await ExecuteAsync(settings, cancellationToken);
    }

    /// <summary>
    /// Extracts audio from a video file and saves it to a specified file path.
    /// </summary>
    /// <param name="file">The video file to extract audio from.</param>
    /// <param name="outputFileFormatType">The format of the output audio file.</param>
    /// <param name="output">The file path where the extracted audio will be saved.</param>
    /// <param name="audioChannel">The number of audio channels to include in the output audio. Default is 2.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to cancel the operation.</param>
    /// <param name="audioSampleRateType">The sample rate of the output audio. Default is 44100 Hz.</param>
    public  async Task GetAudioFromVideoAsync(MediaFile file,
                                              FileFormatType outputFileFormatType,
                                              string output,
                                              int audioChannel = 2,
                                              CancellationToken? cancellationToken = null,
                                              AudioSampleRateType audioSampleRateType = AudioSampleRateType.Hz44100)
    {
        await ExecuteGetAudioFromVideoAsync(file, outputFileFormatType, audioSampleRateType, audioChannel, output,  cancellationToken ?? new CancellationToken());
    }

    /// <summary>
    /// Extracts audio from a video file and returns it as a <see cref="MemoryStream"/>.
    /// </summary>
    /// <param name="file">The video file to extract audio from.</param>
    /// <param name="outputFileFormatType">The format of the output audio file.</param>
    /// <param name="audioChannel">The number of audio channels to include in the output audio. Default is 2.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to cancel the operation.</param>
    /// <param name="audioSampleRateType">The sample rate of the output audio. Default is 44100 Hz.</param>
    /// <returns>A <see cref="MemoryStream"/> containing the extracted audio data.</returns>
    public  async Task<MemoryStream> GetAudioFromVideoAsStreamAsync(MediaFile file,
                                                                    FileFormatType outputFileFormatType,
                                                                    int audioChannel = 2,
                                                                    CancellationToken? cancellationToken = null,
                                                                    AudioSampleRateType audioSampleRateType = AudioSampleRateType.Hz44100)
    {
        return (await ExecuteGetAudioFromVideoAsync(file, outputFileFormatType, audioSampleRateType, audioChannel, null,  cancellationToken ?? new CancellationToken()))!;
    }

    /// <summary>
    /// Extracts audio from a video file and returns it as a byte array.
    /// </summary>
    /// <param name="file">The video file to extract audio from.</param>
    /// <param name="outputFileFormatType">The format of the output audio file.</param>
    /// <param name="audioChannel">The number of audio channels to include in the output audio file. Default is 2.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation. Default is null.</param>
    /// <param name="audioSampleRateType">The audio sample rate of the output audio file. Default is 44100 Hz.</param>
    /// <returns>The extracted audio as a byte array.</returns>
    public  async Task<byte[]> GetAudioFromVideoAsBytesAsync(MediaFile file,
                                                             FileFormatType outputFileFormatType,
                                                             int audioChannel = 2,
                                                             CancellationToken? cancellationToken = null,
                                                             AudioSampleRateType audioSampleRateType = AudioSampleRateType.Hz44100)
    {
        return (await ExecuteGetAudioFromVideoAsync(file, outputFileFormatType, audioSampleRateType, audioChannel, null,  cancellationToken ?? new CancellationToken()))!
            .ToArray();
    }

    //======================================================================================================================================================================

    /// <summary>
    /// Executes the conversion of a video file to the specified output format.
    /// </summary>
    /// <param name="file">The video file to be converted.</param>
    /// <param name="output">The output file path for the converted video. Can be null if output is to be returned as a stream.</param>
    /// <param name="outputFileFormatType">The desired output format for the converted video.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="MemoryStream"/> that contains the converted video, or null if output is specified as a file path.</returns>
    private  async Task<MemoryStream?> ExecuteConvertVideoAsync(MediaFile file, string? output, FileFormatType outputFileFormatType, CancellationToken cancellationToken)
    {
        return await ExecuteAsync(new VideoProcessingSettings().ReplaceIfExist()
                                                               .SetInputFiles(file)
                                                               .MapMetadata()
                                                               .Format(outputFileFormatType)
                                                               .SetOutputArguments(output),
                                  cancellationToken);
    }

    /// <summary>
    /// Converts the given `MediaFile` to the specified `FileFormatType` and saves it to the file at the specified `output` path.
    /// </summary>
    /// <param name="file">The `MediaFile` to be converted.</param>
    /// <param name="output">The path where the converted file will be saved.</param>
    /// <param name="outputFileFormatType">The desired `FileFormatType` of the converted file.</param>
    /// <param name="cancellationToken">An optional `CancellationToken` that can be used to cancel the operation.</param>
    /// <returns>A `Task` representing the asynchronous operation.</returns>
    public  async Task ConvertVideoAsync(MediaFile file, string output, FileFormatType outputFileFormatType, CancellationToken? cancellationToken = null)
    {
        await ExecuteConvertVideoAsync(file, output, outputFileFormatType, cancellationToken ?? new CancellationToken());
    }

    /// <summary>
    /// Converts the given `MediaFile` to the specified `FileFormatType` and returns the result as a `MemoryStream`.
    /// </summary>
    /// <param name="file">The `MediaFile` to be converted.</param>
    /// <param name="outputFileFormatType">The desired `FileFormatType` of the converted file.</param>
    /// <param name="cancellationToken">An optional `CancellationToken` that can be used to cancel the operation.</param>
    /// <returns>A `Task` representing the asynchronous operation that returns the result as a `MemoryStream`.</returns>
    public  async Task<MemoryStream> ConvertVideoAsStreamAsync(MediaFile file, FileFormatType outputFileFormatType, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConvertVideoAsync(file, null, outputFileFormatType, cancellationToken ?? new CancellationToken()))!;
    }

    /// <summary>
    /// Converts the given `MediaFile` to the specified `FileFormatType` and returns the result as a byte array.
    /// </summary>
    /// <param name="file">The `MediaFile` to be converted.</param>
    /// <param name="outputFileFormatType">The desired `FileFormatType` of the converted file.</param>
    /// <param name="cancellationToken">An optional `CancellationToken` that can be used to cancel the operation.</param>
    /// <returns>A `Task` representing the asynchronous operation that returns the result as a byte array.</returns>
    public  async Task<byte[]> ConvertVideoAsBytesAsync(MediaFile file, FileFormatType outputFileFormatType, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConvertVideoAsync(file, null, outputFileFormatType, cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    //======================================================================================================================================================================

    /// <summary>
    /// Executes the task of adding a watermark to the specified video.
    /// </summary>
    /// <param name="videoFile">The video file to add watermark to.</param>
    /// <param name="watermarkFile">The watermark file to be added to the video.</param>
    /// <param name="position">The position of the watermark in the video.</param>
    /// <param name="output">The path and name of the output file.</param>
    /// <param name="outputFileFormatType">The format of the output file.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A MemoryStream object that contains the data of the output file.</returns>
    private  async Task<MemoryStream?> ExecuteAddWaterMarkToVideoAsync(MediaFile videoFile,
                                                                       MediaFile watermarkFile,
                                                                       PositionType position,
                                                                       string? output,
                                                                       FileFormatType outputFileFormatType,
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
                                                    .Format(outputFileFormatType)
                                                    .SetOutputArguments(output);

        return await ExecuteAsync(settings, cancellationToken);
    }

    /// <summary>
    /// Adds a watermark to the given video file and saves the result to the specified output path.
    /// </summary>
    /// <param name="videoFile">The video file to add the watermark to.</param>
    /// <param name="watermarkFile">The watermark file to add to the video.</param>
    /// <param name="position">The position of the watermark in the video.</param>
    /// <param name="output">The path to the output file. If null, the output will not be saved to disk.</param>
    /// <param name="outputFileFormatType">The format of the output file.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    public  async Task AddWaterMarkToVideoAsync(MediaFile videoFile,
                                                MediaFile watermarkFile,
                                                PositionType position,
                                                string output,
                                                FileFormatType outputFileFormatType,
                                                CancellationToken? cancellationToken = null)
    {
        await ExecuteAddWaterMarkToVideoAsync(videoFile, watermarkFile, position, output, outputFileFormatType, cancellationToken ?? new CancellationToken());
    }

    /// <summary>
    /// Adds a watermark to the given video file and returns the result as a MemoryStream.
    /// </summary>
    /// <param name="videoFile">The video file to add the watermark to.</param>
    /// <param name="watermarkFile">The watermark file to add to the video.</param>
    /// <param name="position">The position of the watermark in the video.</param>
    /// <param name="outputFileFormatType">The format of the output file.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>The result of adding the watermark to the video as a MemoryStream.</returns>
    public  async Task<MemoryStream> AddWaterMarkToVideoAsStreamAsync(MediaFile videoFile,
                                                                      MediaFile watermarkFile,
                                                                      PositionType position,
                                                                      FileFormatType outputFileFormatType,
                                                                      CancellationToken? cancellationToken = null)
    {
        return (await ExecuteAddWaterMarkToVideoAsync(videoFile, watermarkFile, position, null, outputFileFormatType, cancellationToken ?? new CancellationToken()))!;
    }

    /// <summary>
    /// Adds a watermark to a video and returns the result as a byte array.
    /// </summary>
    /// <param name="videoFile">The video file to add the watermark to.</param>
    /// <param name="watermarkFile">The watermark file to add to the video.</param>
    /// <param name="position">The position of the watermark on the video.</param>
    /// <param name="outputFileFormatType">The format of the output video file.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
    /// <returns>A byte array containing the result of the operation.</returns>
    public  async Task<byte[]> AddWaterMarkToVideoAsBytesAsync(MediaFile videoFile,
                                                               MediaFile watermarkFile,
                                                               PositionType position,
                                                               FileFormatType outputFileFormatType,
                                                               CancellationToken? cancellationToken = null)
    {
        return (await ExecuteAddWaterMarkToVideoAsync(videoFile, watermarkFile, position, null, outputFileFormatType, cancellationToken ?? new CancellationToken()))!
            .ToArray();
    }

    //======================================================================================================================================================================

    /// <summary>
    /// Extracts the video from a media file.
    /// </summary>
    /// <param name="file">The media file from which the video should be extracted.</param>
    /// <param name="output">The output path where the extracted video should be stored. If this is `null`, the extracted video will be stored in memory.</param>
    /// <param name="outputFileFormatType">The format of the extracted video file.</param>
    /// <param name="videoCodecType">The video codec to be used while extracting the video.</param>
    /// <param name="pixelFormat">The pixel format of the extracted video.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A `MemoryStream` containing the extracted video, or `null` if the `output` parameter was provided.</returns>
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

    /// <summary>
    /// Extracts the video from a media file asynchronously.
    /// </summary>
    /// <param name="file">The media file from which to extract the video.</param>
    /// <param name="output">The output file path for the extracted video.</param>
    /// <param name="outputFileFormatType">The format of the output file.</param>
    /// <param name="videoCodecType">The video codec type to use for the extracted video. Default is VideoCodecType.COPY.</param>
    /// <param name="pixelFormat">The pixel format to use for the extracted video. Default is "yuv420p".</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation. Default is null.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public  async Task ExtractVideoFromFileAsync(MediaFile file,
                                                 string output,
                                                 FileFormatType outputFileFormatType,
                                                 VideoCodecType videoCodecType = VideoCodecType.COPY,
                                                 string pixelFormat = "yuv420p",
                                                 CancellationToken? cancellationToken = null)
    {
        await ExecuteExtractVideoFromFileAsync(file, output, outputFileFormatType, videoCodecType, pixelFormat, cancellationToken ?? new CancellationToken());
    }

    /// <summary>
    /// Extracts the video from a media file as a memory stream asynchronously.
    /// </summary>
    /// <param name="file">The media file from which to extract the video.</param>
    /// <param name="outputFileFormatType">The format of the output file.</param>
    /// <param name="videoCodecType">The video codec type to use for the extracted video. Default is VideoCodecType.COPY.</param>
    /// <param name="pixelFormat">The pixel format to use for the extracted video. Default is "yuv420p".</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation. Default is null.</param>
    /// <returns>A task representing the asynchronous operation, containing the extracted video as a memory stream.</returns>
    public  async Task<MemoryStream> ExtractVideoFromFileAsStreamAsync(MediaFile file,
                                                                       FileFormatType outputFileFormatType,
                                                                       VideoCodecType videoCodecType = VideoCodecType.COPY,
                                                                       string pixelFormat = "yuv420p",
                                                                       CancellationToken? cancellationToken = null)
    {
        return (await ExecuteExtractVideoFromFileAsync(file, null, outputFileFormatType, videoCodecType, pixelFormat, cancellationToken ?? new CancellationToken()))!;
    }

    /// <summary>
    /// Extracts video from a media file as bytes.
    /// </summary>
    /// <param name="file">The media file to extract video from.</param>
    /// <param name="outputFileFormatType">The output file format type of the extracted video.</param>
    /// <param name="videoCodecType">The video codec type to use during the extraction process. Default is VideoCodecType.COPY.</param>
    /// <param name="pixelFormat">The pixel format to use during the extraction process. Default is "yuv420p".</param>
    /// <param name="cancellationToken">A cancellation token to cancel the asynchronous operation. Default is null.</param>
    /// <returns>A byte array that represents the extracted video.</returns>
    public  async Task<byte[]> ExtractVideoFromFileAsBytesAsync(MediaFile file,
                                                                FileFormatType outputFileFormatType,
                                                                VideoCodecType videoCodecType = VideoCodecType.COPY,
                                                                string pixelFormat = "yuv420p",
                                                                CancellationToken? cancellationToken = null)
    {
        return (await ExecuteExtractVideoFromFileAsync(file, null, outputFileFormatType, videoCodecType, pixelFormat, cancellationToken ?? new CancellationToken()))!
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
    /// <param name="outputFileFormatType">The output file format type of the audio-added video.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the asynchronous operation.</param>
    /// <returns>A memory stream that represents the audio-added video.</returns>
    private  async Task<MemoryStream?> ExecuteAddAudioToVideoAsync(MediaFile audioFile,
                                                                   MediaFile videoFile,
                                                                   string? output,
                                                                   VideoCodecType videoCodecType,
                                                                   AudioCodecType audioCodecType,
                                                                   FileFormatType outputFileFormatType,
                                                                   CancellationToken cancellationToken)
    {
        var settings = new VideoProcessingSettings().ReplaceIfExist()
                                                    .SetInputFiles(audioFile, videoFile)
                                                    .VideoCodec(videoCodecType)
                                                    .AudioCodec(audioCodecType)
                                                    .Format(outputFileFormatType)
                                                    .SetOutputArguments(output);

        return await ExecuteAsync(settings, cancellationToken);
    }

    /// <summary>
    /// Adds audio to a video file asynchronously.
    /// </summary>
    /// <param name="audioFile">The audio file to add to the video.</param>
    /// <param name="videoFile">The video file to add the audio to.</param>
    /// <param name="output">The output file path for the audio-added video.</param>
    /// <param name="outputFileFormatType">The output file format type of the audio-added video.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the asynchronous operation. Default is null.</param>
    /// <param name="audioCodecType">The audio codec type to use during the addition process. Default is AudioCodecType.COPY.</param>
    /// <param name="videoCodecType">The video codec type to use during the addition process. Default is VideoCodecType.COPY.</param>
    /// <returns></returns>
    public  async Task AddAudioToVideoAsync(MediaFile audioFile,
                                            MediaFile videoFile,
                                            string output,
                                            FileFormatType outputFileFormatType,
                                            CancellationToken? cancellationToken = null,
                                            AudioCodecType audioCodecType = AudioCodecType.COPY,
                                            VideoCodecType videoCodecType = VideoCodecType.COPY)
    {
        await ExecuteAddAudioToVideoAsync(audioFile,
                                          videoFile,
                                          output,
                                          videoCodecType,
                                          audioCodecType,
                                          outputFileFormatType,
                                          cancellationToken ?? new CancellationToken());
    }

    /// <summary>
    /// Adds audio to a video file and returns the result as a memory stream asynchronously.
    /// </summary>
    /// <param name="audioFile">The audio file to add to the video.</param>
    /// <param name="videoFile">The video file to add the audio to.</param>
    /// <param name="outputFileFormatType">The output file format type of the audio-added video.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the asynchronous operation. Default is null.</param>
    /// <param name="audioCodecType">The audio codec type to use during the addition process. Default is AudioCodecType.COPY.</param>
    /// <param name="videoCodecType">The video codec type to use during the addition process. Default is VideoCodecType.COPY.</param>
    /// <returns>A memory stream that contains the audio-added video.</returns>
    public  async Task<MemoryStream> AddAudioToVideoAsStreamAsync(MediaFile audioFile,
                                                                  MediaFile videoFile,
                                                                  FileFormatType outputFileFormatType,
                                                                  CancellationToken? cancellationToken = null,
                                                                  AudioCodecType audioCodecType = AudioCodecType.COPY,
                                                                  VideoCodecType videoCodecType = VideoCodecType.COPY)
    {
        return (await ExecuteAddAudioToVideoAsync(audioFile,
                                                  videoFile,
                                                  null,
                                                  videoCodecType,
                                                  audioCodecType,
                                                  outputFileFormatType,
                                                  cancellationToken ?? new CancellationToken()))!;
    }

    /// <summary>
    /// Asynchronously adds audio to a video and returns the result as an array of bytes.
    /// </summary>
    /// <param name="audioFile">The audio file to be added to the video.</param>
    /// <param name="videofile">The video file that the audio will be added to.</param>
    /// <param name="outputFileFormatType">The desired output file format type.</param>
    /// <param name="cancellationToken">The optional cancellation token to cancel the operation.</param>
    /// <param name="audioCodecType">The audio codec type, with a default value of AudioCodecType.COPY.</param>
    /// <param name="videoCodecType">The video codec type, with a default value of VideoCodecType.COPY.</param>
    /// <returns>The result of adding audio to the video as an array of bytes.</returns>
    public  async Task<byte[]> AddAudioToVideoAsBytesAsync(MediaFile audioFile,
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

    /// <summary>
    /// Converts a video file to a gif file and saves it to the specified output path.
    /// </summary>
    /// <param name="file">The video file to be converted.</param>
    /// <param name="fps">The number of frames per second for the output gif.</param>
    /// <param name="scale">The scale of the output gif in pixels.</param>
    /// <param name="loop">The number of times the output gif will loop.</param>
    /// <param name="output">The path where the output gif will be saved.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    public async Task ConvertVideoToGifAsync(MediaFile file, int fps, int scale, int loop, string output, CancellationToken? cancellationToken = null)
    {
        await ExecuteConvertVideoToGifAsync(file, fps, scale, loop, output, cancellationToken ?? new CancellationToken());
    }

    /// <summary>
    /// Converts a video file to a gif and returns it as a memory stream.
    /// </summary>
    /// <param name="file">The video file to be converted.</param>
    /// <param name="fps">The number of frames per second for the output gif.</param>
    /// <param name="scale">The scale of the output gif in pixels.</param>
    /// <param name="loop">The number of times the output gif will loop.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A memory stream containing the converted gif file.</returns>
    public async Task<MemoryStream> ConvertVideoToGifAsStreamAsync(MediaFile file, int fps, int scale, int loop, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteConvertVideoToGifAsync(file, fps, scale, loop, null, cancellationToken ?? new CancellationToken()))!;
    }

    /// <summary>
    /// Converts a video file to a gif and returns it as a byte array.
    /// </summary>
    /// <param name="file">The video file to be converted.</param>
    /// <param name="fps">The number of frames per second for the output gif.</param>
    /// <param name="scale">The scale of the output gif in pixels.</param>
    /// <param name="loop">The number of times the output gif will loop.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A byte array containing the converted gif file.</returns>
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
    /// <param name="outputFileFormatType">The desired file format for the output file.</param>
    /// <param name="videoCodecType">The type of video codec to be used for compression.</param>
    /// <param name="cancellationToken">A cancellation token to stop the compression process.</param>
    /// <returns>A memory stream of the compressed video, or `null` if the compression failed.</returns>
    private  async Task<MemoryStream?> ExecuteCompressVideoAsync(MediaFile file,
                                                                 int crf,
                                                                 string? output,
                                                                 FileFormatType outputFileFormatType,
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
                                                   .Format(outputFileFormatType)
                                                   .SetOutputArguments(output);

        return await ExecuteAsync(setting, cancellationToken);
    }

    /// <summary>
    /// Asynchronously compresses the input video file.
    /// </summary>
    /// <param name="file">The input media file to be compressed.</param>
    /// <param name="compressionRatio">The compression ratio to be used for compression.</param>
    /// <param name="output">The desired name of the output file, including the file extension. </param>
    /// <param name="outputFileFormatType">The desired file format for the output file.</param>
    /// <param name="videoCodecType">The type of video codec to be used for compression. Default is H264.</param>
    /// <param name="cancellationToken">An optional cancellation token to stop the compression process.</param>
    public  async Task CompressVideoAsync(MediaFile file,
                                          int compressionRatio,
                                          string output,
                                          FileFormatType outputFileFormatType,
                                          VideoCodecType videoCodecType = VideoCodecType.H264,
                                          CancellationToken? cancellationToken = null)
    {
        await ExecuteCompressVideoAsync(file, compressionRatio, output, outputFileFormatType, videoCodecType, cancellationToken ?? new CancellationToken());
    }

    /// <summary>
    /// Asynchronously compresses the input video file and returns the compressed video as a memory stream.
    /// </summary>
    /// <param name="file">The input media file to be compressed.</param>
    /// <param name="compressionRatio">The compression ratio to be used for compression.</param>
    /// <param name="outputFileFormatType">The desired file format for the output file.</param>
    /// <param name="videoCodecType">The type of video codec to be used for compression. Default is H264.</param>
    /// <param name="cancellationToken">An optional cancellation token to stop the compression process.</param>
    /// <returns>The compressed video as a memory stream.</returns>
    public  async Task<MemoryStream> CompressVideoAsStreamAsync(MediaFile file,
                                                                int compressionRatio,
                                                                FileFormatType outputFileFormatType,
                                                                VideoCodecType videoCodecType = VideoCodecType.H264,
                                                                CancellationToken? cancellationToken = null)
    {
        return (await ExecuteCompressVideoAsync(file, compressionRatio, null, outputFileFormatType, videoCodecType, cancellationToken ?? new CancellationToken()))!;
    }

    /// <summary>
    /// Asynchronously compresses the input video file and returns the compressed video as a byte array.
    /// </summary>
    /// <param name="file">The input media file to be compressed.</param>
    /// <param name="compressionRatio">The compression ratio to be used for compression.</param>
    /// <param name="outputFileFormatType">The desired file format for the output file.</param>
    /// <param name="videoCodecType">The type of video codec to be used for compression. Default is H264.</param>
    /// <param name="cancellationToken">An optional cancellation token to stop the compression process.</param>
    /// <returns>The compressed video as a byte array.</returns>
    public  async Task<byte[]> CompressVideoAsBytesAsync(MediaFile file,
                                                         int compressionRatio,
                                                         FileFormatType outputFileFormatType,
                                                         VideoCodecType videoCodecType = VideoCodecType.H264,
                                                         CancellationToken? cancellationToken = null)
    {
        return (await ExecuteCompressVideoAsync(file, compressionRatio, null, outputFileFormatType, videoCodecType, cancellationToken ?? new CancellationToken()))!
            .ToArray();
    }

    //======================================================================================================================================================================

    /// <summary>
    /// Executes the compression of an image file.
    /// </summary>
    /// <param name="file">The image file to be compressed.</param>
    /// <param name="level">The compression level to be applied to the image file.</param>
    /// <param name="output">The output file path, if specified. If null, the output will be returned as a memory stream.</param>
    /// <param name="outputFileFormatType">The file format type for the output file.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A memory stream containing the compressed image data, or null if the output file path is specified.</returns>
    private  async Task<MemoryStream?> ExecuteCompressImageAsync(MediaFile file,
                                                                 int level,
                                                                 string? output,
                                                                 FileFormatType outputFileFormatType,
                                                                 CancellationToken cancellationToken)
    {
        var setting = new VideoProcessingSettings().ReplaceIfExist()
                                                   .HideBanner()
                                                   .SetInputFiles(file)
                                                   .CompressionLevel(level)
                                                   .Format(outputFileFormatType)
                                                   .SetOutputArguments(output);

        return await ExecuteAsync(setting, cancellationToken);
    }

    /// <summary>
    /// Compresses an image file.
    /// </summary>
    /// <param name="file">The image file to be compressed.</param>
    /// <param name="level">The level of compression to apply to the image. The higher the level, the smaller the size of the output file will be.</param>
    /// <param name="output">The path to the output file, including the file name and extension.</param>
    /// <param name="outputFileFormatType">The format of the output file. Can be either JPEG, PNG, or GIF.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    public  async Task CompressImageAsync(MediaFile file, int level, string output, FileFormatType outputFileFormatType, CancellationToken? cancellationToken = null)
    {
        await ExecuteCompressImageAsync(file, level, output, outputFileFormatType, cancellationToken ?? new CancellationToken());
    }

    /// <summary>
    /// Compresses the image file and returns the compressed image as a <see cref="MemoryStream"/>.
    /// </summary>
    /// <param name="file">The input image file to be compressed.</param>
    /// <param name="level">The compression level to be applied to the image. This value should be between 0 and 100.</param>
    /// <param name="outputFileFormatType">The output format for the compressed image file.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result contains the compressed image as a <see cref="MemoryStream"/>.</returns>
    public  async Task<MemoryStream> CompressImageAsStreamAsync(MediaFile file,
                                                                int level,
                                                                FileFormatType outputFileFormatType,
                                                                CancellationToken? cancellationToken = null)
    {
        return (await ExecuteCompressImageAsync(file, level, null, outputFileFormatType, cancellationToken ?? new CancellationToken()))!;
    }

    /// <summary>
    /// Compresses the image file and returns the compressed image as a byte array.
    /// </summary>
    /// <param name="file">The input image file to be compressed.</param>
    /// <param name="level">The compression level to be applied to the image. This value should be between 0 and 100.</param>
    /// <param name="outputFileFormatType">The output format for the compressed image file.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result contains the compressed image as a byte array.</returns>
    public  async Task<byte[]> CompressImageAsBytesAsync(MediaFile file, int level, FileFormatType outputFileFormatType, CancellationToken? cancellationToken = null)
    {
        return (await ExecuteCompressImageAsync(file, level, null, outputFileFormatType, cancellationToken ?? new CancellationToken()))!.ToArray();
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
    /// <param name="outputFileFormatType">The output file format type.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A memory stream containing the result of the operation.</returns>
    private  async Task<MemoryStream?> ExecuteConcatVideosAsync(MediaFile[] files,
                                                                string? outputFile,
                                                                VideoCodecType videoCodecType,
                                                                AudioCodecType audioCodecType,
                                                                string videoBSF,
                                                                FileFormatType outputFileFormatType,
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
                                                   .Format(outputFileFormatType)
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

    /// <summary>
    /// Concatenates multiple videos into one video.
    /// </summary>
    /// <param name="files">Array of input videos to concatenate</param>
    /// <param name="output">Output file path for the concatenated video</param>
    /// <param name="outputFileFormatType">Format of the output file</param>
    /// <param name="videoBSF">Video bit stream filter to use</param>
    /// <param name="cancellationToken">Cancellation token for cancelling the task</param>
    /// <param name="videoCodecType">Video codec to use for encoding the output video</param>
    /// <param name="audioCodecType">Audio codec to use for encoding the output video</param>
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

    /// <summary>
    /// Concatenates multiple videos into a single MemoryStream.
    /// </summary>
    /// <param name="files">Array of media files to be concatenated</param>
    /// <param name="outputFileFormatType">Output file format type</param>
    /// <param name="videoBSF">The video bitstream filter</param>
    /// <param name="videoCodecType">The type of video codec</param>
    /// <param name="audioCodecType">The type of audio codec</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A memory stream containing the concatenated video</returns>
    public  async Task<MemoryStream> ConcatVideosAsStreamAsync(MediaFile[] files,
                                                               FileFormatType outputFileFormatType,
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
                                               outputFileFormatType,
                                               cancellationToken ?? new CancellationToken()))!;
    }

    /// <summary>
    /// Concatenates multiple videos into a single video file represented as a byte array.
    /// </summary>
    /// <param name="files">An array of MediaFile objects representing the videos to be concatenated.</param>
    /// <param name="outputFileFormatType">The format type for the output video file.</param>
    /// <param name="videoBSF">The video bitstream filter for the output video file.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <param name="videoCodecType">The video codec type for the output video file.</param>
    /// <param name="audioCodecType">The audio codec type for the output video file.</param>
    /// <returns>A byte array representation of the concatenated video file.</returns>
    public  async Task<byte[]> ConcatVideosAsBytesAsync(MediaFile[] files,
                                                        FileFormatType outputFileFormatType,
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
                                               outputFileFormatType,
                                               cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    //======================================================================================================================================================================

    /// <summary>
    /// This method is used to get the information of a video file, such as its size, duration, and bit rate.
    /// </summary>
    /// <param name="videoFile">The video file for which information needs to be retrieved.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the async operation.</param>
    /// <returns>A string that contains the video file's information in JSON format.</returns>
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
    /// <param name="outputFileFormatType">The format type of the output file.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the task.</param>
    /// <returns>A memory stream containing the output video with added subtitles, or `null` if an output file is specified.</returns>
    private  async Task<MemoryStream?> ExecuteAddHardSubtitlesAsync(MediaFile videoFile,
                                                                    MediaFile subsFile,
                                                                    string language,
                                                                    string? outputFile,
                                                                    FileFormatType outputFileFormatType,
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
                                                   .Format(outputFileFormatType)
                                                   .SetOutputArguments(outputFile);

        return await ExecuteAsync(setting, cancellationToken);
    }

    /// <summary>
    /// Asynchronously adds hard subtitles to a video file
    /// </summary>
    /// <param name="videoFile">The video file to add subtitles to</param>
    /// <param name="subsFile">The subtitles file to add to the video</param>
    /// <param name="language">The language of the subtitles</param>
    /// <param name="outputFile">The output file name for the processed video file with added subtitles</param>
    /// <param name="outputFileFormatType">The format type for the output file</param>
    /// <param name="cancellationToken">A CancellationToken that can be used to cancel the operation</param>
    public  async Task AddHardSubtitlesAsync(MediaFile videoFile,
                                             MediaFile subsFile,
                                             string language,
                                             string outputFile,
                                             FileFormatType outputFileFormatType,
                                             CancellationToken? cancellationToken = null)
    {
        await ExecuteAddHardSubtitlesAsync(videoFile, subsFile, language, outputFile, outputFileFormatType, cancellationToken ?? new CancellationToken());
    }

    /// <summary>
    /// Adds hard subtitles to a video file and returns the result as a memory stream.
    /// </summary>
    /// <param name="videoFile">The video file to add hard subtitles to.</param>
    /// <param name="subsFile">The subtitles file to add to the video.</param>
    /// <param name="language">The language of the subtitles.</param>
    /// <param name="outputFileFormatType">The format type of the output file.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A memory stream containing the video file with hard subtitles added.</returns>
    public  async Task<MemoryStream> AddHardSubtitlesAsStreamAsync(MediaFile videoFile,
                                                                   MediaFile subsFile,
                                                                   string language,
                                                                   FileFormatType outputFileFormatType,
                                                                   CancellationToken? cancellationToken = null)
    {
        return (await ExecuteAddHardSubtitlesAsync(videoFile, subsFile, language, null, outputFileFormatType, cancellationToken ?? new CancellationToken()))!;
    }

    /// <summary>
    /// Adds hard subtitles to a video file as a byte array.
    /// </summary>
    /// <param name="videoFile">The video file to add hard subtitles to.</param>
    /// <param name="subsFile">The file containing the hard subtitles to add to the video file.</param>
    /// <param name="language">The language of the hard subtitles.</param>
    /// <param name="outputFileFormatType">The format type of the output video file.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The video file with the added hard subtitles as a byte array.</returns>
    public  async Task<byte[]> AddHardSubtitlesAsBytesAsync(MediaFile videoFile,
                                                            MediaFile subsFile,
                                                            string language,
                                                            FileFormatType outputFileFormatType,
                                                            CancellationToken? cancellationToken = null)
    {
        return (await ExecuteAddHardSubtitlesAsync(videoFile, subsFile, language, null, outputFileFormatType, cancellationToken ?? new CancellationToken()))!.ToArray();
    }

    public static async Task DownloadExecutableFiles()
    {
        var fileName = $"{Guid.NewGuid()}.zip";

        var ffmpegFound = false;
        var ffprobeFound = false;

        try
        {
            await FileDownloadProcessor.DownloadFile(_zipAddress, fileName);

            // Open an existing zip file for reading
            using(var zip = ZipFileProcessor.Open(fileName, FileAccess.Read))
            {
                // Read the central directory collection
                var dir = zip.ReadCentralDir();

                // Look for the desired file
                foreach (var entry in dir)
                {
                    if (Path.GetFileName(entry.FilenameInZip) == _ffmpeg)
                    {
                        zip.ExtractFile(entry, $@"asd/{_ffmpeg}"); // File found, extract it
                        ffmpegFound = true;
                    }

                    if (Path.GetFileName(entry.FilenameInZip) == _ffprobe)
                    {
                        zip.ExtractFile(entry, $@"asd/{_ffprobe}"); // File found, extract it
                        ffprobeFound = true;
                    }
                }
            }

            if(!ffmpegFound)
                throw new Exception($"{_ffmpeg} not found");

            if(!ffprobeFound)
                throw new Exception($"{_ffprobe} not found");
        }
        finally
        {
            if(File.Exists(fileName))
                File.Delete(fileName);
        }
    }
}