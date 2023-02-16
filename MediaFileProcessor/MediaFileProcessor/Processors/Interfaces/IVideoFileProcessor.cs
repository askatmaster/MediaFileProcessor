using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Enums;
using MediaFileProcessor.Models.Settings;
namespace MediaFileProcessor.Processors.Interfaces;

public interface IVideoFileProcessor
{
    /// <summary>
    /// Executes the video processing with the provided settings and cancellation token.
    /// </summary>
    /// <param name="settings">The video processing settings to use for processing the video.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the processing if necessary.</param>
    /// <returns>A MemoryStream representing the processed video, or null if the process was cancelled.</returns>
    Task<MemoryStream?> ExecuteAsync(VideoProcessingSettings settings, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a frame from a video at the specified timestamp and saves it to a specified output file.
    /// </summary>
    /// <param name="timestamp">The timestamp at which to extract the frame from the video.</param>
    /// <param name="file">The media file representing the video to extract the frame from.</param>
    /// <param name="outputFile">The output file to save the extracted frame to.</param>
    /// <param name="outputFormat">The format to use for the output file.</param>
    /// <param name="cancellationToken">The optional cancellation token used to cancel the process if necessary.</param>
    Task GetFrameFromVideoAsync(TimeSpan timestamp, MediaFile file, string outputFile, FileFormatType outputFormat, CancellationToken? cancellationToken = null);

    /// <summary>
    /// Retrieves a frame from a video file at a specified time and returns it as a memory stream.
    /// </summary>
    /// <param name="timestamp">The time at which the frame should be retrieved.</param>
    /// <param name="file">The media file from which the frame should be retrieved.</param>
    /// <param name="outputFormat">The format in which the retrieved frame should be returned.</param>
    /// <param name="cancellationToken">The cancellation token for the operation.</param>
    /// <returns>A memory stream containing the requested frame.</returns>
    Task<MemoryStream> GetFrameFromVideoAsStreamAsync(TimeSpan timestamp, MediaFile file, FileFormatType outputFormat, CancellationToken? cancellationToken = null);

    /// <summary>
    /// Retrieves a frame from a video file at a specified time and returns it as a byte array.
    /// </summary>
    /// <param name="timestamp">The time at which the frame should be retrieved.</param>
    /// <param name="file">The media file from which the frame should be retrieved.</param>
    /// <param name="outputFormat">The format in which the retrieved frame should be returned.</param>
    /// <param name="cancellationToken">The cancellation token for the operation.</param>
    /// <returns>A byte array containing the requested frame.</returns>
    Task<byte[]> GetFrameFromVideoAsBytesAsync(TimeSpan timestamp, MediaFile file, FileFormatType outputFormat, CancellationToken? cancellationToken = null);

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
    Task CutVideoAsync(TimeSpan startTime, TimeSpan endTime, MediaFile file, string outputFile, FileFormatType outputFormat, CancellationToken? cancellationToken = null);

    /// <summary>
    /// Cuts a portion of a video file and returns the result as a memory stream.
    /// </summary>
    /// <param name="startTime">The start time of the portion to be cut.</param>
    /// <param name="endTime">The end time of the portion to be cut.</param>
    /// <param name="file">The media file to be cut.</param>
    /// <param name="outputFormat">The format in which the cut portion should be saved.</param>
    /// <param name="cancellationToken">The cancellation token for the operation.</param>
    /// <returns>A memory stream that contains the cut portion of the video file.</returns>
    Task<MemoryStream> CutVideoAsStreamAsync(TimeSpan startTime,
                                             TimeSpan endTime,
                                             MediaFile file,
                                             FileFormatType outputFormat,
                                             CancellationToken? cancellationToken = null);

    /// <summary>
    /// Cuts a portion of a video file and returns the result as a byte array.
    /// </summary>
    /// <param name="startTime">The start time of the portion to be cut.</param>
    /// <param name="endTime">The end time of the portion to be cut.</param>
    /// <param name="file">The media file to be cut.</param>
    /// <param name="outputFormat">The format in which the cut portion should be saved.</param>
    /// <param name="cancellationToken">The cancellation token for the operation.</param>
    /// <returns>A byte array that contains the cut portion of the video file.</returns>
    Task<byte[]> CutVideoAsBytesAsync(TimeSpan startTime, TimeSpan endTime, MediaFile file, FileFormatType outputFormat, CancellationToken? cancellationToken = null);

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
    Task ConvertImagesToVideoAsync(MediaFile file,
                                   int frameRate,
                                   string outputFile,
                                   string pixelFormat,
                                   FileFormatType outputFileFormatType,
                                   CancellationToken? cancellationToken = null,
                                   VideoCodecType videoCodecType = VideoCodecType.LIBX264);

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
    Task<MemoryStream> ConvertImagesToVideoAsStreamAsync(MediaFile file,
                                                         int frameRate,
                                                         string pixelFormat,
                                                         FileFormatType outputFileFormatType,
                                                         VideoCodecType videoCodecType = VideoCodecType.LIBX264,
                                                         CancellationToken? cancellationToken = null);

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
    Task<byte[]> ConvertImagesToVideoAsBytesAsync(MediaFile file,
                                                  int frameRate,
                                                  string pixelFormat,
                                                  FileFormatType? outputFileFormatType,
                                                  VideoCodecType videoCodecType = VideoCodecType.LIBX264,
                                                  CancellationToken? cancellationToken = null);

    /// <summary>
    /// Converts a video file to image files.
    /// </summary>
    /// <param name="file">The input video file</param>
    /// <param name="outputFormatType">The output image format type</param>
    /// <param name="outputImagesPattern">The pattern to name the output images</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation</param>
    Task ConvertVideoToImagesAsync(MediaFile file, FileFormatType outputFormatType, string outputImagesPattern, CancellationToken? cancellationToken = null);

    /// <summary>
    /// Converts a video file to a multi-stream of images.
    /// </summary>
    /// <param name="file">The input video file</param>
    /// <param name="outputFormatType">The output image format type</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation</param>
    /// <returns>The multi-stream of images</returns>
    Task<MultiStream> ConvertVideoToImagesAsStreamAsync(MediaFile file, FileFormatType outputFormatType, CancellationToken? cancellationToken = null);

    /// <summary>
    /// Converts a video file to a multi-array of image bytes.
    /// </summary>
    /// <param name="file">The input video file</param>
    /// <param name="outputFormatType">The output image format type</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation</param>
    /// <returns>The multi-array of image bytes</returns>
    Task<byte[][]> ConvertVideoToImagesAsBytesAsync(MediaFile file, FileFormatType outputFormatType, CancellationToken? cancellationToken = null);

    /// <summary>
    /// Extracts audio from a video file and saves it to a specified file path.
    /// </summary>
    /// <param name="file">The video file to extract audio from.</param>
    /// <param name="outputFileFormatType">The format of the output audio file.</param>
    /// <param name="output">The file path where the extracted audio will be saved.</param>
    /// <param name="audioChannel">The number of audio channels to include in the output audio. Default is 2.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to cancel the operation.</param>
    /// <param name="audioSampleRateType">The sample rate of the output audio. Default is 44100 Hz.</param>
    Task GetAudioFromVideoAsync(MediaFile file,
                                FileFormatType outputFileFormatType,
                                string output,
                                int audioChannel = 2,
                                CancellationToken? cancellationToken = null,
                                AudioSampleRateType audioSampleRateType = AudioSampleRateType.Hz44100);

    /// <summary>
    /// Extracts audio from a video file and returns it as a <see cref="MemoryStream"/>.
    /// </summary>
    /// <param name="file">The video file to extract audio from.</param>
    /// <param name="outputFileFormatType">The format of the output audio file.</param>
    /// <param name="audioChannel">The number of audio channels to include in the output audio. Default is 2.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to cancel the operation.</param>
    /// <param name="audioSampleRateType">The sample rate of the output audio. Default is 44100 Hz.</param>
    /// <returns>A <see cref="MemoryStream"/> containing the extracted audio data.</returns>
    Task<MemoryStream> GetAudioFromVideoAsStreamAsync(MediaFile file,
                                                      FileFormatType outputFileFormatType,
                                                      int audioChannel = 2,
                                                      CancellationToken? cancellationToken = null,
                                                      AudioSampleRateType audioSampleRateType = AudioSampleRateType.Hz44100);

    /// <summary>
    /// Extracts audio from a video file and returns it as a byte array.
    /// </summary>
    /// <param name="file">The video file to extract audio from.</param>
    /// <param name="outputFileFormatType">The format of the output audio file.</param>
    /// <param name="audioChannel">The number of audio channels to include in the output audio file. Default is 2.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation. Default is null.</param>
    /// <param name="audioSampleRateType">The audio sample rate of the output audio file. Default is 44100 Hz.</param>
    /// <returns>The extracted audio as a byte array.</returns>
    Task<byte[]> GetAudioFromVideoAsBytesAsync(MediaFile file,
                                               FileFormatType outputFileFormatType,
                                               int audioChannel = 2,
                                               CancellationToken? cancellationToken = null,
                                               AudioSampleRateType audioSampleRateType = AudioSampleRateType.Hz44100);

    /// <summary>
    /// Converts the given `MediaFile` to the specified `FileFormatType` and saves it to the file at the specified `output` path.
    /// </summary>
    /// <param name="file">The `MediaFile` to be converted.</param>
    /// <param name="output">The path where the converted file will be saved.</param>
    /// <param name="outputFileFormatType">The desired `FileFormatType` of the converted file.</param>
    /// <param name="cancellationToken">An optional `CancellationToken` that can be used to cancel the operation.</param>
    /// <returns>A `Task` representing the asynchronous operation.</returns>
    Task ConvertVideoAsync(MediaFile file, string output, FileFormatType outputFileFormatType, CancellationToken? cancellationToken = null);

    /// <summary>
    /// Converts the given `MediaFile` to the specified `FileFormatType` and returns the result as a `MemoryStream`.
    /// </summary>
    /// <param name="file">The `MediaFile` to be converted.</param>
    /// <param name="outputFileFormatType">The desired `FileFormatType` of the converted file.</param>
    /// <param name="cancellationToken">An optional `CancellationToken` that can be used to cancel the operation.</param>
    /// <returns>A `Task` representing the asynchronous operation that returns the result as a `MemoryStream`.</returns>
    Task<MemoryStream> ConvertVideoAsStreamAsync(MediaFile file, FileFormatType outputFileFormatType, CancellationToken? cancellationToken = null);

    /// <summary>
    /// Converts the given `MediaFile` to the specified `FileFormatType` and returns the result as a byte array.
    /// </summary>
    /// <param name="file">The `MediaFile` to be converted.</param>
    /// <param name="outputFileFormatType">The desired `FileFormatType` of the converted file.</param>
    /// <param name="cancellationToken">An optional `CancellationToken` that can be used to cancel the operation.</param>
    /// <returns>A `Task` representing the asynchronous operation that returns the result as a byte array.</returns>
    Task<byte[]> ConvertVideoAsBytesAsync(MediaFile file, FileFormatType outputFileFormatType, CancellationToken? cancellationToken = null);

    /// <summary>
    /// Adds a watermark to the given video file and saves the result to the specified output path.
    /// </summary>
    /// <param name="videoFile">The video file to add the watermark to.</param>
    /// <param name="watermarkFile">The watermark file to add to the video.</param>
    /// <param name="position">The position of the watermark in the video.</param>
    /// <param name="output">The path to the output file. If null, the output will not be saved to disk.</param>
    /// <param name="outputFileFormatType">The format of the output file.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    Task AddWaterMarkToVideoAsync(MediaFile videoFile,
                                  MediaFile watermarkFile,
                                  PositionType position,
                                  string output,
                                  FileFormatType outputFileFormatType,
                                  CancellationToken? cancellationToken = null);

    /// <summary>
    /// Adds a watermark to the given video file and returns the result as a MemoryStream.
    /// </summary>
    /// <param name="videoFile">The video file to add the watermark to.</param>
    /// <param name="watermarkFile">The watermark file to add to the video.</param>
    /// <param name="position">The position of the watermark in the video.</param>
    /// <param name="outputFileFormatType">The format of the output file.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>The result of adding the watermark to the video as a MemoryStream.</returns>
    Task<MemoryStream> AddWaterMarkToVideoAsStreamAsync(MediaFile videoFile,
                                                        MediaFile watermarkFile,
                                                        PositionType position,
                                                        FileFormatType outputFileFormatType,
                                                        CancellationToken? cancellationToken = null);

    /// <summary>
    /// Adds a watermark to a video and returns the result as a byte array.
    /// </summary>
    /// <param name="videoFile">The video file to add the watermark to.</param>
    /// <param name="watermarkFile">The watermark file to add to the video.</param>
    /// <param name="position">The position of the watermark on the video.</param>
    /// <param name="outputFileFormatType">The format of the output video file.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
    /// <returns>A byte array containing the result of the operation.</returns>
    Task<byte[]> AddWaterMarkToVideoAsBytesAsync(MediaFile videoFile,
                                                 MediaFile watermarkFile,
                                                 PositionType position,
                                                 FileFormatType outputFileFormatType,
                                                 CancellationToken? cancellationToken = null);

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
    Task ExtractVideoFromFileAsync(MediaFile file,
                                   string output,
                                   FileFormatType outputFileFormatType,
                                   VideoCodecType videoCodecType = VideoCodecType.COPY,
                                   string pixelFormat = "yuv420p",
                                   CancellationToken? cancellationToken = null);

    /// <summary>
    /// Extracts the video from a media file as a memory stream asynchronously.
    /// </summary>
    /// <param name="file">The media file from which to extract the video.</param>
    /// <param name="outputFileFormatType">The format of the output file.</param>
    /// <param name="videoCodecType">The video codec type to use for the extracted video. Default is VideoCodecType.COPY.</param>
    /// <param name="pixelFormat">The pixel format to use for the extracted video. Default is "yuv420p".</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation. Default is null.</param>
    /// <returns>A task representing the asynchronous operation, containing the extracted video as a memory stream.</returns>
    Task<MemoryStream> ExtractVideoFromFileAsStreamAsync(MediaFile file,
                                                         FileFormatType outputFileFormatType,
                                                         VideoCodecType videoCodecType = VideoCodecType.COPY,
                                                         string pixelFormat = "yuv420p",
                                                         CancellationToken? cancellationToken = null);

    /// <summary>
    /// Extracts video from a media file as bytes.
    /// </summary>
    /// <param name="file">The media file to extract video from.</param>
    /// <param name="outputFileFormatType">The output file format type of the extracted video.</param>
    /// <param name="videoCodecType">The video codec type to use during the extraction process. Default is VideoCodecType.COPY.</param>
    /// <param name="pixelFormat">The pixel format to use during the extraction process. Default is "yuv420p".</param>
    /// <param name="cancellationToken">A cancellation token to cancel the asynchronous operation. Default is null.</param>
    /// <returns>A byte array that represents the extracted video.</returns>
    Task<byte[]> ExtractVideoFromFileAsBytesAsync(MediaFile file,
                                                  FileFormatType outputFileFormatType,
                                                  VideoCodecType videoCodecType = VideoCodecType.COPY,
                                                  string pixelFormat = "yuv420p",
                                                  CancellationToken? cancellationToken = null);

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
    Task AddAudioToVideoAsync(MediaFile audioFile,
                              MediaFile videoFile,
                              string output,
                              FileFormatType outputFileFormatType,
                              CancellationToken? cancellationToken = null,
                              AudioCodecType audioCodecType = AudioCodecType.COPY,
                              VideoCodecType videoCodecType = VideoCodecType.COPY);

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
    Task<MemoryStream> AddAudioToVideoAsStreamAsync(MediaFile audioFile,
                                                    MediaFile videoFile,
                                                    FileFormatType outputFileFormatType,
                                                    CancellationToken? cancellationToken = null,
                                                    AudioCodecType audioCodecType = AudioCodecType.COPY,
                                                    VideoCodecType videoCodecType = VideoCodecType.COPY);

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
    Task<byte[]> AddAudioToVideoAsBytesAsync(MediaFile audioFile,
                                             MediaFile videofile,
                                             FileFormatType outputFileFormatType,
                                             CancellationToken? cancellationToken = null,
                                             AudioCodecType audioCodecType = AudioCodecType.COPY,
                                             VideoCodecType videoCodecType = VideoCodecType.COPY);

    /// <summary>
    /// Converts a video file to a gif file and saves it to the specified output path.
    /// </summary>
    /// <param name="file">The video file to be converted.</param>
    /// <param name="fps">The number of frames per second for the output gif.</param>
    /// <param name="scale">The scale of the output gif in pixels.</param>
    /// <param name="loop">The number of times the output gif will loop.</param>
    /// <param name="output">The path where the output gif will be saved.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    Task ConvertVideoToGifAsync(MediaFile file, int fps, int scale, int loop, string output, CancellationToken? cancellationToken = null);

    /// <summary>
    /// Converts a video file to a gif and returns it as a memory stream.
    /// </summary>
    /// <param name="file">The video file to be converted.</param>
    /// <param name="fps">The number of frames per second for the output gif.</param>
    /// <param name="scale">The scale of the output gif in pixels.</param>
    /// <param name="loop">The number of times the output gif will loop.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A memory stream containing the converted gif file.</returns>
    Task<MemoryStream> ConvertVideoToGifAsStreamAsync(MediaFile file, int fps, int scale, int loop, CancellationToken? cancellationToken = null);

    /// <summary>
    /// Converts a video file to a gif and returns it as a byte array.
    /// </summary>
    /// <param name="file">The video file to be converted.</param>
    /// <param name="fps">The number of frames per second for the output gif.</param>
    /// <param name="scale">The scale of the output gif in pixels.</param>
    /// <param name="loop">The number of times the output gif will loop.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A byte array containing the converted gif file.</returns>
    Task<byte[]> ConvertVideoToGifAsBytesAsync(MediaFile file, int fps, int scale, int loop, CancellationToken? cancellationToken = null);

    /// <summary>
    /// Asynchronously compresses the input video file.
    /// </summary>
    /// <param name="file">The input media file to be compressed.</param>
    /// <param name="compressionRatio">The compression ratio to be used for compression.</param>
    /// <param name="output">The desired name of the output file, including the file extension. </param>
    /// <param name="outputFileFormatType">The desired file format for the output file.</param>
    /// <param name="videoCodecType">The type of video codec to be used for compression. Default is H264.</param>
    /// <param name="cancellationToken">An optional cancellation token to stop the compression process.</param>
    Task CompressVideoAsync(MediaFile file,
                            int compressionRatio,
                            string output,
                            FileFormatType outputFileFormatType,
                            VideoCodecType videoCodecType = VideoCodecType.H264,
                            CancellationToken? cancellationToken = null);

    /// <summary>
    /// Asynchronously compresses the input video file and returns the compressed video as a memory stream.
    /// </summary>
    /// <param name="file">The input media file to be compressed.</param>
    /// <param name="compressionRatio">The compression ratio to be used for compression.</param>
    /// <param name="outputFileFormatType">The desired file format for the output file.</param>
    /// <param name="videoCodecType">The type of video codec to be used for compression. Default is H264.</param>
    /// <param name="cancellationToken">An optional cancellation token to stop the compression process.</param>
    /// <returns>The compressed video as a memory stream.</returns>
    Task<MemoryStream> CompressVideoAsStreamAsync(MediaFile file,
                                                  int compressionRatio,
                                                  FileFormatType outputFileFormatType,
                                                  VideoCodecType videoCodecType = VideoCodecType.H264,
                                                  CancellationToken? cancellationToken = null);

    /// <summary>
    /// Asynchronously compresses the input video file and returns the compressed video as a byte array.
    /// </summary>
    /// <param name="file">The input media file to be compressed.</param>
    /// <param name="compressionRatio">The compression ratio to be used for compression.</param>
    /// <param name="outputFileFormatType">The desired file format for the output file.</param>
    /// <param name="videoCodecType">The type of video codec to be used for compression. Default is H264.</param>
    /// <param name="cancellationToken">An optional cancellation token to stop the compression process.</param>
    /// <returns>The compressed video as a byte array.</returns>
    Task<byte[]> CompressVideoAsBytesAsync(MediaFile file,
                                           int compressionRatio,
                                           FileFormatType outputFileFormatType,
                                           VideoCodecType videoCodecType = VideoCodecType.H264,
                                           CancellationToken? cancellationToken = null);

    /// <summary>
    /// Compresses an image file.
    /// </summary>
    /// <param name="file">The image file to be compressed.</param>
    /// <param name="level">The level of compression to apply to the image. The higher the level, the smaller the size of the output file will be.</param>
    /// <param name="output">The path to the output file, including the file name and extension.</param>
    /// <param name="outputFileFormatType">The format of the output file. Can be either JPEG, PNG, or GIF.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    Task CompressImageAsync(MediaFile file, int level, string output, FileFormatType outputFileFormatType, CancellationToken? cancellationToken = null);

    /// <summary>
    /// Compresses the image file and returns the compressed image as a <see cref="MemoryStream"/>.
    /// </summary>
    /// <param name="file">The input image file to be compressed.</param>
    /// <param name="level">The compression level to be applied to the image. This value should be between 0 and 100.</param>
    /// <param name="outputFileFormatType">The output format for the compressed image file.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result contains the compressed image as a <see cref="MemoryStream"/>.</returns>
    Task<MemoryStream> CompressImageAsStreamAsync(MediaFile file, int level, FileFormatType outputFileFormatType, CancellationToken? cancellationToken = null);

    /// <summary>
    /// Compresses the image file and returns the compressed image as a byte array.
    /// </summary>
    /// <param name="file">The input image file to be compressed.</param>
    /// <param name="level">The compression level to be applied to the image. This value should be between 0 and 100.</param>
    /// <param name="outputFileFormatType">The output format for the compressed image file.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result contains the compressed image as a byte array.</returns>
    Task<byte[]> CompressImageAsBytesAsync(MediaFile file, int level, FileFormatType outputFileFormatType, CancellationToken? cancellationToken = null);

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
    Task ConcatVideosAsync(MediaFile[] files,
                           string output,
                           FileFormatType outputFileFormatType,
                           string videoBSF = "h264_mp4toannexb",
                           CancellationToken? cancellationToken = null,
                           VideoCodecType videoCodecType = VideoCodecType.LIBX264,
                           AudioCodecType audioCodecType = AudioCodecType.AAC);

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
    Task<MemoryStream> ConcatVideosAsStreamAsync(MediaFile[] files,
                                                 FileFormatType outputFileFormatType,
                                                 string videoBSF = "h264_mp4toannexb",
                                                 VideoCodecType videoCodecType = VideoCodecType.LIBX264,
                                                 AudioCodecType audioCodecType = AudioCodecType.AAC,
                                                 CancellationToken? cancellationToken = null);

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
    Task<byte[]> ConcatVideosAsBytesAsync(MediaFile[] files,
                                          FileFormatType outputFileFormatType,
                                          string videoBSF = "h264_mp4toannexb",
                                          CancellationToken? cancellationToken = null,
                                          VideoCodecType videoCodecType = VideoCodecType.LIBX264,
                                          AudioCodecType audioCodecType = AudioCodecType.AAC);

    /// <summary>
    /// This method is used to get the information of a video file, such as its size, duration, and bit rate.
    /// </summary>
    /// <param name="videoFile">The video file for which information needs to be retrieved.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the async operation.</param>
    /// <returns>A string that contains the video file's information in JSON format.</returns>
    Task<string> GetVideoInfo(MediaFile videoFile, CancellationToken? cancellationToken = null);

    /// <summary>
    /// Asynchronously adds hard subtitles to a video file
    /// </summary>
    /// <param name="videoFile">The video file to add subtitles to</param>
    /// <param name="subsFile">The subtitles file to add to the video</param>
    /// <param name="language">The language of the subtitles</param>
    /// <param name="outputFile">The output file name for the processed video file with added subtitles</param>
    /// <param name="outputFileFormatType">The format type for the output file</param>
    /// <param name="cancellationToken">A CancellationToken that can be used to cancel the operation</param>
    Task AddHardSubtitlesAsync(MediaFile videoFile,
                               MediaFile subsFile,
                               string language,
                               string outputFile,
                               FileFormatType outputFileFormatType,
                               CancellationToken? cancellationToken = null);

    /// <summary>
    /// Adds hard subtitles to a video file and returns the result as a memory stream.
    /// </summary>
    /// <param name="videoFile">The video file to add hard subtitles to.</param>
    /// <param name="subsFile">The subtitles file to add to the video.</param>
    /// <param name="language">The language of the subtitles.</param>
    /// <param name="outputFileFormatType">The format type of the output file.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A memory stream containing the video file with hard subtitles added.</returns>
    Task<MemoryStream> AddHardSubtitlesAsStreamAsync(MediaFile videoFile,
                                                     MediaFile subsFile,
                                                     string language,
                                                     FileFormatType outputFileFormatType,
                                                     CancellationToken? cancellationToken = null);

    /// <summary>
    /// Adds hard subtitles to a video file as a byte array.
    /// </summary>
    /// <param name="videoFile">The video file to add hard subtitles to.</param>
    /// <param name="subsFile">The file containing the hard subtitles to add to the video file.</param>
    /// <param name="language">The language of the hard subtitles.</param>
    /// <param name="outputFileFormatType">The format type of the output video file.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The video file with the added hard subtitles as a byte array.</returns>
    Task<byte[]> AddHardSubtitlesAsBytesAsync(MediaFile videoFile,
                                              MediaFile subsFile,
                                              string language,
                                              FileFormatType outputFileFormatType,
                                              CancellationToken? cancellationToken = null);
}
