using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Enums;
using MediaFileProcessor.Models.Enums.MagickImage;
namespace MediaFileProcessor.Models.Settings;

public class ImageProcessingSettings : ProcessingSettings
{
    /// <summary>
    /// JPEG/MIFF/PNG compression level
    /// </summary>
    public ImageProcessingSettings Quality(int quality)
    {
        _stringBuilder.Append($" -quality {quality} ");

        return this;
    }

    /// <summary>
    /// Use this filter when resizing an image
    /// </summary>
    public ImageProcessingSettings Filter(FilterType filter)
    {
        _stringBuilder.Append($" -filter {filter.ToString()}");

        return this;
    }

    /// <summary>
    /// Horizontal and vertical sampling factor
    /// </summary>
    public ImageProcessingSettings SamplingFactor(string factor)
    {
        _stringBuilder.Append($" -sampling-factor {factor} ");

        return this;
    }

    /// <summary>
    /// define one or more image format options
    /// </summary>
    public ImageProcessingSettings Define(string define)
    {
        _stringBuilder.Append($" -define {define} ");

        return this;
    }

    /// <summary>
    /// Create a thumbnail of the image
    /// </summary>
    public ImageProcessingSettings Thumbnail(string thumbnail)
    {
        _stringBuilder.Append($" -thumbnail {thumbnail} ");

        return this;
    }

    public ImageProcessingSettings CustomInputs(string customInputs)
    {
        _stringBuilder.Append(customInputs);

        return this;
    }

    /// <summary>
    /// apply Paeth rotation to the image
    /// </summary>
    public ImageProcessingSettings Rotate(int rotate)
    {
        _stringBuilder.Append($" -rotate {rotate} ");

        return this;
    }

    /// <summary>
    /// Flip image in the vertical direction
    /// </summary>
    public ImageProcessingSettings Flip()
    {
        _stringBuilder.Append(" -flip ");

        return this;
    }

    /// <summary>
    /// flop image in the horizontal direction
    /// </summary>
    public ImageProcessingSettings Flop()
    {
        _stringBuilder.Append(" -flop ");

        return this;
    }

    /// <summary>
    /// Resize the image
    /// </summary>
    public ImageProcessingSettings Resize(string size)
    {
        _stringBuilder.Append($" -resize {size} ");

        return this;
    }

    /// <summary>
    /// Image depth
    /// </summary>
    public ImageProcessingSettings Depth(int depth)
    {
        _stringBuilder.Append($" -depth {depth} ");

        return this;
    }

    /// <summary>
    /// Horizontal and vertical density of the image
    /// </summary>
    public ImageProcessingSettings Density(int density)
    {
        _stringBuilder.Append($" -density {density} ");

        return this;
    }

    /// <summary>
    /// sharpen the image
    /// </summary>
    public ImageProcessingSettings Sharpen(string radius)
    {
        _stringBuilder.Append($" -sharpen {radius} ");

        return this;
    }

    /// <summary>
    /// Unsharpen the image
    /// </summary>
    public ImageProcessingSettings UnSharp(string radius)
    {
        _stringBuilder.Append($" -unsharp {radius} ");

        return this;
    }

    /// <summary>
    /// Reduce image noise and reduce detail levels
    /// </summary>
    public ImageProcessingSettings GaussianBlur(string radius)
    {
        _stringBuilder.Append($" -gaussian-blur {radius} ");

        return this;
    }

    /// <summary>
    /// Simulate motion blur
    /// </summary>
    public ImageProcessingSettings MotionBlur(string radius)
    {
        _stringBuilder.Append($" -motion-blur {radius} ");

        return this;
    }

    /// <summary>
    /// Simulate radial blur
    /// </summary>
    public ImageProcessingSettings RedialBlur(string angle)
    {
        _stringBuilder.Append($" -radial-blur {angle} ");

        return this;
    }

    /// <summary>
    /// Selectively blur pixels within a contrast threshold
    /// </summary>
    public ImageProcessingSettings SelectiveBlur(string radius)
    {
        _stringBuilder.Append($" -selective-blur {radius} ");

        return this;
    }

    /// <summary>
    /// Transform image to span the full range of colors
    /// </summary>
    public ImageProcessingSettings Normalize()
    {
        _stringBuilder.Append(" -normalize ");

        return this;
    }

    /// <summary>
    /// Enhance or reduce the image contrast
    /// </summary>
    public ImageProcessingSettings Contrast(bool value)
    {
        if(value)
            _stringBuilder.Append(" +contrast ");

        _stringBuilder.Append(" -contrast ");

        return this;
    }

    /// <summary>
    /// Improve brightness / contrast of the image
    /// </summary>
    public ImageProcessingSettings BrightnessContrast(string value)
    {
        _stringBuilder.Append($" -brightness-contrast {value} ");

        return this;
    }

    /// <summary>
    /// Set image colorspace
    /// </summary>
    public ImageProcessingSettings Colorspace(string value)
    {
        _stringBuilder.Append($" -colorspace {value} ");

        return this;
    }

    /// <summary>
    /// Simulate a sepia-toned photo
    /// </summary>
    public ImageProcessingSettings SepiaTone(int percentage)
    {
        _stringBuilder.Append($" -sepia-tone {percentage}% ");

        return this;
    }

    /// <summary>
    /// Replace each pixel with its complementary color
    /// </summary>
    public ImageProcessingSettings Negate()
    {
        _stringBuilder.Append(" -negate ");

        return this;
    }

    /// <summary>
    /// Crop the image
    /// </summary>
    public ImageProcessingSettings Crop(string value)
    {
        _stringBuilder.Append($" -crop {value} ");

        return this;
    }

    /// <summary>
    /// Separate an image channel into a grayscale image
    /// </summary>
    public ImageProcessingSettings Separate()
    {
        _stringBuilder.Append(" -separate ");

        return this;
    }

    /// <summary>
    /// Set an image attribute
    /// </summary>
    public ImageProcessingSettings SetAttribute(string value)
    {
        _stringBuilder.Append($" -set {value}");

        return this;
    }

    /// <summary>
    /// Combine a sequence of images
    /// </summary>
    public ImageProcessingSettings Combine()
    {
        _stringBuilder.Append(" -combine ");

        return this;
    }

    /// <summary>
    /// Compare image
    /// </summary>
    public ImageProcessingSettings Compare()
    {
        _stringBuilder.Append(" -compare ");

        return this;
    }

    /// <summary>
    /// Perform complex mathematics on an image sequence
    /// </summary>
    public ImageProcessingSettings ComplexOperator()
    {
        _stringBuilder.Append(" -complexoperator ");

        return this;
    }

    /// <summary>
    /// Render text with this font
    /// </summary>
    public ImageProcessingSettings Font(string font)
    {
        _stringBuilder.Append($" -font {font}");

        return this;
    }

    /// <summary>
    /// Font point size
    /// </summary>
    public ImageProcessingSettings Pointsize(string size)
    {
        _stringBuilder.Append($" -pointsize {size}");

        return this;
    }

    /// <summary>
    /// Set image composite operator
    /// </summary>
    public ImageProcessingSettings Compose(string value)
    {
        _stringBuilder.Append($" -compose {value}");

        return this;
    }

    /// <summary>
    /// Composite image
    /// </summary>
    public ImageProcessingSettings Composite()
    {
        _stringBuilder.Append($" -composite ");

        return this;
    }

    /// <summary>
    /// Image compression type
    /// </summary>
    public ImageProcessingSettings Compress(string type)
    {
        _stringBuilder.Append($" -compress {type} ");

        return this;
    }

    /// <summary>
    /// Copy pixels from one area of an image to another
    /// </summary>
    public ImageProcessingSettings Copy(string value)
    {
        _stringBuilder.Append($" -copy {value} ");

        return this;
    }

    /// <summary>
    /// Cycle the image colormap
    /// </summary>
    public ImageProcessingSettings Cycle(string value)
    {
        _stringBuilder.Append($" -cycle {value} ");

        return this;
    }

    /// <summary>
    /// Convert cipher pixels to plain
    /// </summary>
    public ImageProcessingSettings Decipher(string filename)
    {
        _stringBuilder.Append($" -decipher {filename} ");

        return this;
    }

    /// <summary>
    /// Color to use when filling a graphic primitive
    /// </summary>
    public ImageProcessingSettings Fill(string color)
    {
        _stringBuilder.Append($" -fill {color}");

        return this;
    }

    /// <summary>
    /// Set box
    /// </summary>
    public ImageProcessingSettings Box(string box)
    {
        _stringBuilder.Append($" -box {box}");

        return this;
    }

    /// <summary>
    /// Annotate the image with text
    /// </summary>
    public ImageProcessingSettings Annotate(string value)
    {
        _stringBuilder.Append($" -annotate {value}");

        return this;
    }

    /// <summary>
    /// Annotate the image with a graphic primitive
    /// </summary>
    public ImageProcessingSettings Draw(string value)
    {
        _stringBuilder.Append($" -draw {value}");

        return this;
    }

    /// <summary>
    /// display the next image after pausing
    /// </summary>
    public ImageProcessingSettings Delay(string value)
    {
        _stringBuilder.Append($" -delay {value}");

        return this;
    }

    /// <summary>
    /// Adaptively blur pixels; decrease effect near edges
    /// </summary>
    public ImageProcessingSettings AdaptiveBlur(string value)
    {
        _stringBuilder.Append($" -adaptive-blur {value}");

        return this;
    }

    /// <summary>
    /// Adaptively sharpen pixels; increase effect near edges
    /// </summary>
    public ImageProcessingSettings AdaptiveSharpen(string value)
    {
        _stringBuilder.Append($" -adaptive-sharpen {value}");

        return this;
    }

    /// <summary>
    /// Join images into a single multi-image file
    /// </summary>
    public ImageProcessingSettings Adjoin()
    {
        _stringBuilder.Append(" -adjoin ");

        return this;
    }

    /// <summary>
    /// Affine transform matrix
    /// </summary>
    public ImageProcessingSettings Affine(string matrix)
    {
        _stringBuilder.Append($" -affine {matrix} ");

        return this;
    }

    /// <summary>
    /// On, activate, off, deactivate, set, opaque, copy", transparent, extract, background, or shape the alpha channel
    /// </summary>
    public ImageProcessingSettings Alpha()
    {
        _stringBuilder.Append(" -alpha ");

        return this;
    }

    /// <summary>
    /// Remove pixel-aliasing
    /// </summary>
    public ImageProcessingSettings Antialias()
    {
        _stringBuilder.Append(" -antialias ");

        return this;
    }

    /// <summary>
    /// decipher image with this password
    /// </summary>
    public ImageProcessingSettings Authenticate(string password)
    {
        _stringBuilder.Append($" -authenticate {password} ");

        return this;
    }

    /// <summary>
    /// Automagically adjust gamma level of image
    /// </summary>
    public ImageProcessingSettings AutoGamma()
    {
        _stringBuilder.Append(" -auto-gamma ");

        return this;
    }

    /// <summary>
    /// Append an image sequence
    /// </summary>
    public ImageProcessingSettings Append()
    {
        _stringBuilder.Append(" -append ");

        return this;
    }

    /// <summary>
    /// Break down an image sequence into constituent parts
    /// </summary>
    public ImageProcessingSettings Deconstruct()
    {
        _stringBuilder.Append(" -deconstruct ");

        return this;
    }

    /// <summary>
    /// Reduce the speckles within an image
    /// </summary>
    public ImageProcessingSettings Despeckle()
    {
        _stringBuilder.Append(" -despeckle ");

        return this;
    }

    /// <summary>
    /// Delete the image from the image sequence
    /// </summary>
    public ImageProcessingSettings Delete(string value)
    {
        _stringBuilder.Append($" -delete {value} ");

        return this;
    }

    /// <summary>
    /// Render text right-to-left or left-to-right
    /// </summary>
    public ImageProcessingSettings Direction(string value)
    {
        _stringBuilder.Append($" -direction {value} ");

        return this;
    }

    /// <summary>
    /// Layer disposal method
    /// </summary>
    public ImageProcessingSettings Dispose(string value)
    {
        _stringBuilder.Append($" -dispose {value} ");

        return this;
    }

    /// <summary>
    /// Distort image
    /// </summary>
    public ImageProcessingSettings Distort(string value)
    {
        _stringBuilder.Append($" -distort {value} ");

        return this;
    }

    /// <summary>
    /// Duplicate an image one or more times
    /// </summary>
    public ImageProcessingSettings Duplicate(string value)
    {
        _stringBuilder.Append($" -duplicate {value} ");

        return this;
    }

    /// <summary>
    /// Apply a filter to detect edges in the image
    /// </summary>
    public ImageProcessingSettings Edge(string value)
    {
        _stringBuilder.Append($" -edge {value} ");

        return this;
    }

    /// <summary>
    /// Emboss an image
    /// </summary>
    public ImageProcessingSettings Emboss(string value)
    {
        _stringBuilder.Append($" -emboss {value} ");

        return this;
    }

    /// <summary>
    /// Convert plain pixels to cipher pixels
    /// </summary>
    public ImageProcessingSettings Encipher(string value)
    {
        _stringBuilder.Append($" -encipher {value} ");

        return this;
    }

    /// <summary>
    /// Text encoding type
    /// </summary>
    public ImageProcessingSettings Encoding(string value)
    {
        _stringBuilder.Append($" -encoding {value} ");

        return this;
    }

    /// <summary>
    /// Endianness (MSB or LSB) of the image
    /// </summary>
    public ImageProcessingSettings Endian(string value)
    {
        _stringBuilder.Append($" -endian {value} ");

        return this;
    }

    /// <summary>
    /// Apply a digital filter to enhance a noisy image
    /// </summary>
    public ImageProcessingSettings Enhance()
    {
        _stringBuilder.Append(" -enhance ");

        return this;
    }

    /// <summary>
    /// Extract area from image
    /// </summary>
    public ImageProcessingSettings Extract(string value)
    {
        _stringBuilder.Append($" -extract {value} ");

        return this;
    }

    /// <summary>
    /// Horizontal and vertical density of the image
    /// </summary>
    public ImageProcessingSettings Density(string value)
    {
        _stringBuilder.Append($" -density {value} ");

        return this;
    }

    /// <summary>
    /// Automagically adjust color levels of image
    /// </summary>
    public ImageProcessingSettings AutoLevel()
    {
        _stringBuilder.Append(" -auto-level ");

        return this;
    }

    /// <summary>
    /// Automagically orient image
    /// </summary>
    public ImageProcessingSettings AutoOrient()
    {
        _stringBuilder.Append(" -auto-orient ");

        return this;
    }

    /// <summary>
    /// automatically perform image thresholding
    /// </summary>
    public ImageProcessingSettings AutoThreshold(string method)
    {
        _stringBuilder.Append($" -auto-threshold {method} ");

        return this;
    }

    /// <summary>
    /// Background color
    /// </summary>
    public ImageProcessingSettings BackgroundColor(string color)
    {
        _stringBuilder.Append($" -background {color} ");

        return this;
    }

    /// <summary>
    /// Add bias when convolving an image
    /// </summary>
    public ImageProcessingSettings Bias(string value)
    {
        _stringBuilder.Append($" -bias {value} ");

        return this;
    }

    /// <summary>
    /// Non-linear, edge-preserving, and noise-reducing smoothing filter
    /// </summary>
    public ImageProcessingSettings BilateralBlur(string geometry)
    {
        _stringBuilder.Append($" -bilateral-blur {geometry} ");

        return this;
    }

    /// <summary>
    /// reduce image noise and reduce detail levels
    /// </summary>
    public ImageProcessingSettings Blur(string geometry)
    {
        _stringBuilder.Append($" -blur {geometry} ");

        return this;
    }

    /// <summary>
    /// Force all pixels below the threshold into black
    /// </summary>
    public ImageProcessingSettings BlackThreshold(string value)
    {
        _stringBuilder.Append($" -black-threshold {value} ");

        return this;
    }

    /// <summary>
    /// Surround image with a border of color
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ImageProcessingSettings Border(string value)
    {
        _stringBuilder.Append($" -border {value} ");

        return this;
    }

    /// <summary>
    /// Border color
    /// </summary>
    public ImageProcessingSettings BorderColor(string color)
    {
        _stringBuilder.Append($" -bordercolor {color} ");

        return this;
    }

    /// <summary>
    /// Chromaticity blue primary point
    /// </summary>
    public ImageProcessingSettings BluePrimary(string point)
    {
        _stringBuilder.Append($" -blue-primary {point} ");

        return this;
    }

    /// <summary>
    /// use a multi-stage algorithm to detect a wide range of edges in the image
    /// </summary>
    public ImageProcessingSettings Canny(string geometry)
    {
        _stringBuilder.Append($" -canny {geometry} ");

        return this;
    }

    /// <summary>
    /// Remove pixels from the image interior
    /// </summary>
    public ImageProcessingSettings Chop(string geometry)
    {
        _stringBuilder.Append($" -chop {geometry} ");

        return this;
    }

    /// <summary>
    /// Contrast limited adaptive histogram equalization
    /// </summary>
    public ImageProcessingSettings Clahe(string geometry)
    {
        _stringBuilder.Append($" -clahe {geometry} ");

        return this;
    }

    /// <summary>
    /// Color correct with a color decision list
    /// </summary>
    public ImageProcessingSettings ColorDecisionList(string filename)
    {
        _stringBuilder.Append($" -cdl {filename} ");

        return this;
    }

    /// <summary>
    /// Clip along the first path from the 8BIM profile
    /// </summary>
    public ImageProcessingSettings Clip()
    {
        _stringBuilder.Append(" -clip ");

        return this;
    }

    /// <summary>
    /// Clip along a named path from the 8BIM profile
    /// </summary>
    public ImageProcessingSettings ClipPath(string id)
    {
        _stringBuilder.Append($" -clip-path {id} ");

        return this;
    }

    /// <summary>
    /// Associate clip mask with the image
    /// </summary>
    public ImageProcessingSettings ClipMask(string filename)
    {
        _stringBuilder.Append($" -clip-mask {filename} ");

        return this;
    }

    /// <summary>
    /// Clone an image
    /// </summary>
    public ImageProcessingSettings Clone(string index)
    {
        _stringBuilder.Append($" -clone {index} ");

        return this;
    }

    /// <summary>
    /// Apply a color lookup table to the image
    /// </summary>
    public ImageProcessingSettings Clut()
    {
        _stringBuilder.Append(" -clut ");

        return this;
    }

    /// <summary>
    /// Merge a sequence of images
    /// </summary>
    public ImageProcessingSettings Coalesce()
    {
        _stringBuilder.Append(" -coalesce ");

        return this;
    }

    /// <summary>
    /// Assign a caption to an image
    /// </summary>
    public ImageProcessingSettings Caption(string value)
    {
        _stringBuilder.Append($" -caption {value} ");

        return this;
    }

    /// <summary>
    /// Colorize the image with the fill color
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ImageProcessingSettings Colorize(string value)
    {
        _stringBuilder.Append($" -colorize {value} ");

        return this;
    }

    /// <summary>
    /// Apply color correction to the image.
    /// </summary>
    public ImageProcessingSettings ColorMatrix(string matrix)
    {
        _stringBuilder.Append($" -color-matrix {matrix} ");

        return this;
    }

    /// <summary>
    /// Preferred number of colors in the image
    /// </summary>
    public ImageProcessingSettings Colors(string value)
    {
        _stringBuilder.Append($" -colors {value} ");

        return this;
    }

    /// <summary>
    /// Connected-components uniquely labeled, choose from 4 or 8 way connectivity
    /// </summary>
    public ImageProcessingSettings ConnectedComponents(string connectivity)
    {
        _stringBuilder.Append($" -connected-components  {connectivity} ");

        return this;
    }

    /// <summary>
    /// Improve the contrast in an image by `stretching' the range of intensity value
    /// </summary>
    public ImageProcessingSettings ContrastStretch(string geometry)
    {
        _stringBuilder.Append($" -contrast-stretch  {geometry} ");

        return this;
    }

    /// <summary>
    /// apply option to select image channels
    /// </summary>
    public ImageProcessingSettings Channel(string type)
    {
        _stringBuilder.Append($" -channel {type} ");

        return this;
    }

    /// <summary>
    /// Simulate a charcoal drawing
    /// </summary>
    public ImageProcessingSettings Charcoal(string radius)
    {
        _stringBuilder.Append($" -charcoal {radius} ");

        return this;
    }

    /// <summary>
    /// Simulate a scene at nighttime in the moonlight
    /// </summary>
    public ImageProcessingSettings BlueShift(string factor)
    {
        _stringBuilder.Append($" -blue-shift {factor} ");

        return this;
    }

    /// <summary>
    /// Set force output format
    /// </summary>
    public ImageProcessingSettings OutputFormat(ImageFormat? format)
    {
        if(format is null)
            return this;

        _stringBuilder.Append($" {format.ToString().ToLower()}:");

        return this;
    }

    /// <summary>
    /// Additional settings that are not currently provided in the wrapper
    /// </summary>
    public ImageProcessingSettings CustomArguments(string arg)
    {
        _stringBuilder.Append(arg);

        return this;
    }

    /// <summary>
    /// Redirect receipt input to stdin
    /// </summary>
    private string StandartInputRedirectArgument => " - ";

    /// <summary>
    /// Setting Output Arguments
    /// </summary>
    public ImageProcessingSettings SetOutputFileArguments(string? arg)
    {
        OutputFileArguments = arg;

        return this;
    }

    /// <summary>
    /// Set input files
    /// </summary>
    public ImageProcessingSettings SetInputFiles(params MediaFile[]? files)
    {
        if(files is null)
            throw new NullReferenceException("'CustomInputs' Arguments must be specified if there are no input files");

        SetInputStreams();

        switch(files.Length)
        {
            case 0:
                throw new Exception("No input files");
            case 1:
                _stringBuilder.Append(files[0].InputType is MediaFileInputType.Path ? files[0].InputFilePath! : StandartInputRedirectArgument);

                return this;
        }

        if(files.Count(x => x.InputType == MediaFileInputType.Stream) <= 1)
        {
            _stringBuilder.Append(files.Aggregate(string.Empty,
                                                  (current, file) =>
                                                      current
                                                    + " "
                                                    + (file.InputType is MediaFileInputType.Path or MediaFileInputType.Template or MediaFileInputType.NamedPipe
                                                          ? file.InputFilePath!
                                                          : StandartInputRedirectArgument)));

            return this;
        }

        _stringBuilder.Append(files.Aggregate(string.Empty,
                                              (current, file) =>
                                                  current
                                                + " "
                                                + (file.InputType is MediaFileInputType.Path or MediaFileInputType.Template or MediaFileInputType.NamedPipe
                                                      ? file.InputFilePath!
                                                      : SetPipeChannel(Guid.NewGuid().ToString(), file))));

        return this;
    }

    /// <summary>
    /// Summary arguments to process
    /// </summary>
    public override string GetProcessArguments()
    {
        return _stringBuilder + GetOutputArgument();
    }

    /// <summary>
    /// Get streams to transfer to a process
    /// </summary>
    public override Stream[]? GetInputStreams()
    {
        return InputStreams?.ToArray();
    }

    /// <summary>
    /// Pipe names for input streams
    /// </summary>
    public string[]? GetInputPipeNames()
    {
        return PipeNames?.Keys.ToArray();
    }

    /// <summary>
    /// If the file is transmitted through a stream then assign a channel name to that stream
    /// </summary>
    private string SetPipeChannel(string pipeName, MediaFile file)
    {
        PipeNames ??= new Dictionary<string, Stream>();
        PipeNames.Add(pipeName, file.InputFileStream!);

        return $@"\\.\pipe\{pipeName}";
    }

    /// <summary>
    /// Set input streams from files
    /// If the input files are streams
    /// </summary>
    private void SetInputStreams(params MediaFile[]? files)
    {
        if(files is null)
            return;

        if(files.Count(x => x.InputType == MediaFileInputType.Stream) == 1)
        {
            InputStreams ??= new List<Stream>();
            InputStreams.Add(files.First(x => x.InputType == MediaFileInputType.Stream).InputFileStream!);
        }

        if (!(PipeNames?.Count > 0))
            return;

        InputStreams ??= new List<Stream>();
        InputStreams.AddRange(PipeNames.Select(pipeName => pipeName.Value));
    }

    /// <summary>
    /// Get output arguments
    /// </summary>
    private string GetOutputArgument()
    {
        return OutputFileArguments ?? " - ";
    }
}