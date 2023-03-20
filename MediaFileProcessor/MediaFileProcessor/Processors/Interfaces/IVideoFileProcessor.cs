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
    /// Converts images to video and saves it to the specified file path.
    /// </summary>
    /// <param name="file">The file that contains the list of images to be converted to video.</param>
    /// <param name="frameRate">The number of frames per second for the resulting video.</param>
    /// <param name="outputFile">The path to the output video file. The file will be overwritten if it already exists.</param>
    /// <param name="outputFormat">The format type for the output video file.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to cancel the asynchronous operation.</param>
    Task<MemoryStream?> ConvertImagesToVideoAsync(MediaFile file,
                                                  int frameRate,
                                                  string? outputFile = null,
                                                  FileFormatType? outputFormat = null,
                                                  CancellationToken? cancellationToken = null);

    /// <summary>
    /// Extracts audio from a video file and saves it to a specified file path.
    /// </summary>
    /// <param name="file">The video file to extract audio from.</param>
    /// <param name="outputFormat">The format of the output audio file.</param>
    /// <param name="outputFile">The file path where the extracted audio will be saved.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to cancel the operation.</param>
    Task<MemoryStream?> ExtractAudioFromVideoAsync(MediaFile file,
                                                   string? outputFile = null,
                                                   FileFormatType? outputFormat = null,
                                                   CancellationToken? cancellationToken = null);

    /// <summary>
    /// Converts the given `MediaFile` to the specified `FileFormatType` and saves it to the file at the specified `output` path.
    /// </summary>
    /// <param name="file">The `MediaFile` to be converted.</param>
    /// <param name="outputFile">The path where the converted file will be saved.</param>
    /// <param name="outputFormat">The desired `FileFormatType` of the converted file.</param>
    /// <param name="cancellationToken">An optional `CancellationToken` that can be used to cancel the operation.</param>
    /// <returns>A `Task` representing the asynchronous operation.</returns>
    Task<MemoryStream?> ConvertVideoAsync(MediaFile file,
                                          string? outputFile = null,
                                          FileFormatType? outputFormat = null,
                                          CancellationToken? cancellationToken = null);

    /// <summary>
    /// Adds a watermark to the given video file and saves the result to the specified output path.
    /// </summary>
    /// <param name="videoFile">The video file to add the watermark to.</param>
    /// <param name="watermarkFile">The watermark file to add to the video.</param>
    /// <param name="position">The position of the watermark in the video.</param>
    /// <param name="outputFile">The path to the output file. If null, the output will not be saved to disk.</param>
    /// <param name="outputFormat">The format of the output file.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    Task<MemoryStream?> AddWaterMarkToVideoAsync(MediaFile videoFile,
                                                 MediaFile watermarkFile,
                                                 PositionType position,
                                                 string? outputFile = null,
                                                 FileFormatType? outputFormat = null,
                                                 CancellationToken? cancellationToken = null);

    /// <summary>
    /// Extracts the video from a media file asynchronously.
    /// </summary>
    /// <param name="file">The media file from which to extract the video.</param>
    /// <param name="outputFile">The output file path for the extracted video.</param>
    /// <param name="outputFormat">The format of the output file.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation. Default is null.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<MemoryStream?> ExtractVideoFromFileAsync(MediaFile file,
                                                  string? outputFile = null,
                                                  FileFormatType? outputFormat = null,
                                                  CancellationToken? cancellationToken = null);

    /// <summary>
    /// Adds audio to a video file asynchronously.
    /// </summary>
    /// <param name="audioFile">The audio file to add to the video.</param>
    /// <param name="videoFile">The video file to add the audio to.</param>
    /// <param name="outputFile">The output file path for the audio-added video.</param>
    /// <param name="outputFormat">The output file format type of the audio-added video.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the asynchronous operation. Default is null.</param>
    Task<MemoryStream?> AddAudioToVideoAsync(MediaFile audioFile,
                                             MediaFile videoFile,
                                             string? outputFile = null,
                                             FileFormatType? outputFormat = null,
                                             CancellationToken? cancellationToken = null);

    /// <summary>
    /// Converts a video file to a gif file and saves it to the specified output path.
    /// </summary>
    /// <param name="file">The video file to be converted.</param>
    /// <param name="fps">The number of frames per second for the output gif.</param>
    /// <param name="scale">The scale of the output gif in pixels.</param>
    /// <param name="loop">The number of times the output gif will loop.</param>
    /// <param name="outputFile">The path where the output gif will be saved.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    Task<MemoryStream?> ConvertVideoToGifAsync(MediaFile file, int fps, int scale, int loop, string? outputFile = null, CancellationToken? cancellationToken = null);

    /// <summary>
    /// Asynchronously compresses the input video file.
    /// </summary>
    /// <param name="file">The input media file to be compressed.</param>
    /// <param name="compressionRatio">The compression ratio to be used for compression.</param>
    /// <param name="output">The desired name of the output file, including the file extension. </param>
    /// <param name="cancellationToken">An optional cancellation token to stop the compression process.</param>
    Task<MemoryStream?> CompressVideoAsync(MediaFile file,
                                           int compressionRatio,
                                           string? output = null,
                                           CancellationToken? cancellationToken = null);

    /// <summary>
    /// Compresses an image file.
    /// </summary>
    /// <param name="file">The image file to be compressed.</param>
    /// <param name="level">The level of compression to apply to the image. The higher the level, the smaller the size of the output file will be.</param>
    /// <param name="outputFile">The path to the output file, including the file name and extension.</param>
    /// <param name="outputFormat">The format of the output file. Can be either JPEG, PNG, or GIF.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    Task<MemoryStream?> CompressImageAsync(MediaFile file,
                                           int level,
                                           string? outputFile = null,
                                           FileFormatType? outputFormat = null,
                                           CancellationToken? cancellationToken = null);

    /// <summary>
    /// Concatenates multiple videos into one video.
    /// </summary>
    /// <param name="files">Array of input videos to concatenate</param>
    /// <param name="outputFile">Output file path for the concatenated video</param>
    /// <param name="outputFormat">Format of the output file</param>
    /// <param name="cancellationToken">Cancellation token for cancelling the task</param>
    Task<MemoryStream?> ConcatVideosAsync(MediaFile[] files,
                                          string? outputFile = null,
                                          FileFormatType? outputFormat = null,
                                          CancellationToken? cancellationToken = null);

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
    /// <param name="outputFormat">The format type for the output file</param>
    /// <param name="cancellationToken">A CancellationToken that can be used to cancel the operation</param>
    Task<MemoryStream?> AddSubtitlesAsync(MediaFile videoFile,
                                          MediaFile subsFile,
                                          string language,
                                          string? outputFile = null,
                                          FileFormatType? outputFormat = null,
                                          CancellationToken? cancellationToken = null);
}
