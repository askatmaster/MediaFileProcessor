using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Enums;
using MediaFileProcessor.Processors;
namespace ConsoleTest;

/// <summary>
/// Test methods for ImageProcessor
/// </summary>
public class ImageProcessorTests
{
    /// <summary>
    /// Test image
    /// </summary>
    private static readonly string _image =  @"G:\MagickImageFile\testImage.jpg";

    /// <summary>
    /// Test Compress image
    /// </summary>
    public static async Task CompressImageTest(ImageFileProcessor processor)
    {
        //Test block with physical paths to input and output files
        await processor.CompressImageAsync(new MediaFile(_image), ImageFormatType.JPG, 60, FilterType.Lanczos, "x1080", @"G:\MagickImageFile\result.jpg", ImageFormatType.JPG);

        //Block for testing file processing as streams without specifying physical paths
        await using var stream = new FileStream(_image, FileMode.Open);
        var resultStream = await processor.CompressImageAsStreamAsync(new MediaFile(stream), ImageFormatType.JPG, 60, FilterType.Lanczos, "x1080", ImageFormatType.JPG);
        await using (var output = new FileStream(@"G:\MagickImageFile\result.jpg", FileMode.Create))
            resultStream.WriteTo(output);

        //Block for testing file processing as bytes without specifying physical paths
        var bytes = await File.ReadAllBytesAsync(_image);
        var resultBytes = await processor.CompressImageAsBytesAsync(new MediaFile(bytes), ImageFormatType.JPG, 60, FilterType.Lanczos, "x1080", ImageFormatType.JPG);
        await using (var output = new FileStream(@"G:\MagickImageFile\result.jpg", FileMode.Create))
            output.Write(resultBytes);
    }
}