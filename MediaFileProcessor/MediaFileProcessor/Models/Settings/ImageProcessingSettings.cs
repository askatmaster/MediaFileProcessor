using MediaFileProcessor.Extensions;
using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Enums;
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
    /// Unsharpen the image
    /// </summary>
    public ImageProcessingSettings UnSharp(string radius)
    {
        _stringBuilder.Append($" -unsharp {radius} ");

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
    /// Set an image attribute
    /// </summary>
    public ImageProcessingSettings SetAttribute(string value)
    {
        _stringBuilder.Append($" -set {value}");

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
    /// Color correct with a color decision list
    /// </summary>
    public ImageProcessingSettings ColorDecisionList(string filename)
    {
        _stringBuilder.Append($" -cdl {filename} ");

        return this;
    }

    /// <summary>
    /// Adaptively blur pixels; decrease effect near edges
    /// </summary>
    public ImageProcessingSettings AdaptiveBlur(string geometry)
    {
        _stringBuilder.Append($" -adaptive-blur {geometry} ");

        return this;
    }

    /// <summary>
    /// Adaptively resize image with data dependent triangulation
    /// </summary>
    public ImageProcessingSettings AdaptiveResize(string geometry)
    {
        _stringBuilder.Append($" -adaptive-resize {geometry} ");

        return this;
    }

    /// <summary>
    /// Adaptively sharpen pixels; increase effect near edges
    /// </summary>
    public ImageProcessingSettings AdaptiveSharpen(string geometry)
    {
        _stringBuilder.Append($" -adaptive-sharpen {geometry} ");

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
    /// Annotate the image with text
    /// </summary>
    public ImageProcessingSettings Annotate(string geometry, string text)
    {
        _stringBuilder.Append($" -annotate {geometry} {text} ");

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
    /// Append an image sequence
    /// </summary>
    public ImageProcessingSettings Append()
    {
        _stringBuilder.Append(" -append ");

        return this;
    }

    /// <summary>
    /// Decipher image with this password
    /// </summary>
    public ImageProcessingSettings Authenticate(string value)
    {
        _stringBuilder.Append($" -authenticate {value} ");

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
    /// Automatically perform image thresholding
    /// </summary>
    public ImageProcessingSettings AutoThreshold(string method)
    {
        _stringBuilder.Append($" -auto-threshold {method} ");

        return this;
    }

    /// <summary>
    /// Background color
    /// </summary>
    public ImageProcessingSettings Background(string color)
    {
        _stringBuilder.Append($" -background {color} ");

        return this;
    }

    /// <summary>
    /// Measure performance
    /// </summary>
    public ImageProcessingSettings Bench(string iterations)
    {
        _stringBuilder.Append($" -bench {iterations} ");

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
    /// Force all pixels below the threshold into black
    /// </summary>
    public ImageProcessingSettings BlackThreshold(string value)
    {
        _stringBuilder.Append($" -black-threshold {value} ");

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
    /// Simulate a scene at nighttime in the moonlight
    /// </summary>
    public ImageProcessingSettings BlueShift(string factor)
    {
        _stringBuilder.Append($" -blue-shift {factor} ");

        return this;
    }

    /// <summary>
    /// Reduce image noise and reduce detail levels
    /// </summary>
    public ImageProcessingSettings Blur(string geometry)
    {
        _stringBuilder.Append($" -blur {geometry} ");

        return this;
    }

    /// <summary>
    /// Surround image with a border of color
    /// </summary>
    public ImageProcessingSettings Border(string geometry)
    {
        _stringBuilder.Append($" -border {geometry} ");

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

    public ImageProcessingSettings BrightnessContrast(string geometry)
    {
        _stringBuilder.Append($" -brightness-contrast {geometry} ");

        return this;
    }

    public ImageProcessingSettings Canny(string geometry)
    {
        _stringBuilder.Append($" -canny {geometry} ");

        return this;
    }
    public ImageProcessingSettings Caption(string text)
    {
        _stringBuilder.Append($" -caption {text} ");

        return this;
    }

    public ImageProcessingSettings Channel(string type)
    {
        _stringBuilder.Append($" -channel {type} ");

        return this;
    }
    public ImageProcessingSettings Charcoal(string radius)
    {
        _stringBuilder.Append($" -charcoal {radius} ");

        return this;
    }
    public ImageProcessingSettings Chop(string geometry)
    {
        _stringBuilder.Append($" -chop {geometry} ");

        return this;
    }
    public ImageProcessingSettings Clahe(string geometry)
    {
        _stringBuilder.Append($" -clahe {geometry} ");

        return this;
    }
    public ImageProcessingSettings Clamp()
    {
        _stringBuilder.Append(" -clamp ");

        return this;
    }
    public ImageProcessingSettings Clip()
    {
        _stringBuilder.Append(" -clip ");

        return this;
    }
    public ImageProcessingSettings ClipMask(string filename)
    {
        _stringBuilder.Append($" -clip-mask {filename} ");

        return this;
    }
    public ImageProcessingSettings ClipPath(string id)
    {
        _stringBuilder.Append($" -clip-path {id} ");

        return this;
    }
    public ImageProcessingSettings Clone(string index)
    {
        _stringBuilder.Append($" -clone {index} ");

        return this;
    }
    public ImageProcessingSettings Clut()
    {
        _stringBuilder.Append(" -clut ");

        return this;
    }
    public ImageProcessingSettings ConnectedComponents(string connectivity)
    {
        _stringBuilder.Append($" -connected-components {connectivity} ");

        return this;
    }
    public ImageProcessingSettings ContrastStretch(string geometry)
    {
        _stringBuilder.Append($" -contrast-stretch {geometry} ");

        return this;
    }
    public ImageProcessingSettings Coalesce()
    {
        _stringBuilder.Append(" -coalesce ");

        return this;
    }
    public ImageProcessingSettings Colorize(string value)
    {
        _stringBuilder.Append($" -colorize {value} ");

        return this;
    }

    public ImageProcessingSettings ColorMatrix(string matrix)
    {
        _stringBuilder.Append($" -color-matrix {matrix} ");

        return this;
    }

    public ImageProcessingSettings Colors(string value)
    {
        _stringBuilder.Append($" -colors {value} ");

        return this;
    }
    public ImageProcessingSettings Colorspace(string type)
    {
        _stringBuilder.Append($" -colorspace {type} ");

        return this;
    }
    public ImageProcessingSettings ColorThreshold(string startColorStopColor)
    {
        _stringBuilder.Append($" -color-threshold {startColorStopColor} ");

        return this;
    }
    public ImageProcessingSettings Combine()
    {
        _stringBuilder.Append(" -combine ");

        return this;
    }
    public ImageProcessingSettings Comment(string text)
    {
        _stringBuilder.Append($" -comment {text} ");

        return this;
    }
    public ImageProcessingSettings Compare()
    {
        _stringBuilder.Append(" -compare ");

        return this;
    }
    public ImageProcessingSettings ComplexOperator()
    {
        _stringBuilder.Append(" -complexoperator ");

        return this;
    }
    public ImageProcessingSettings Compose(string @operator)
    {
        _stringBuilder.Append($" -compose {@operator} ");

        return this;
    }
    public ImageProcessingSettings Composite()
    {
        _stringBuilder.Append(" -composite ");

        return this;
    }
    public ImageProcessingSettings Compress(string type)
    {
        _stringBuilder.Append($" -compress {type} ");

        return this;
    }
    public ImageProcessingSettings Contrast(string level)
    {
        _stringBuilder.Append($" -contrast {level} ");

        return this;
    }
    public ImageProcessingSettings Convolve(string coefficients)
    {
        _stringBuilder.Append($" -convolve {coefficients} ");

        return this;
    }

    public ImageProcessingSettings Copy(string geometry, string offset)
    {
        _stringBuilder.Append($" -copy {geometry} {offset} ");

        return this;
    }
    public ImageProcessingSettings Crop(string geometry)
    {
        _stringBuilder.Append($" -crop {geometry} ");

        return this;
    }
    public ImageProcessingSettings Cycle(string amount)
    {
        _stringBuilder.Append($" -cycle {amount} ");

        return this;
    }
    public ImageProcessingSettings Decipher(string filename)
    {
        _stringBuilder.Append($" -decipher {filename} ");

        return this;
    }
    public ImageProcessingSettings Debug(string events)
    {
        _stringBuilder.Append($" -debug {events} ");

        return this;
    }
    public ImageProcessingSettings Define(string formatOption)
    {
        _stringBuilder.Append($" -define {formatOption} ");

        return this;
    }
    public ImageProcessingSettings Deconstruct()
    {
        _stringBuilder.Append(" -deconstruct ");

        return this;
    }
    public ImageProcessingSettings Delay(int centiseconds)
    {
        _stringBuilder.Append($" -delay {centiseconds} ");

        return this;
    }
    public ImageProcessingSettings Delete(string index)
    {
        _stringBuilder.Append($" -delete {index} ");

        return this;
    }
    public ImageProcessingSettings Density(string geometry)
    {
        _stringBuilder.Append($" -density {geometry} ");

        return this;
    }
    public ImageProcessingSettings Depth(string value)
    {
        _stringBuilder.Append($" -depth {value} ");

        return this;
    }
    public ImageProcessingSettings Despeckle()
    {
        _stringBuilder.Append(" -despeckle ");

        return this;
    }
    public ImageProcessingSettings Direction(string type)
    {
        _stringBuilder.Append($" -direction {type} ");

        return this;
    }
    public ImageProcessingSettings Display(string server)
    {
        _stringBuilder.Append($" -display {server} ");

        return this;
    }
    public ImageProcessingSettings Dispose(string method)
    {
        _stringBuilder.Append($" -dispose {method} ");

        return this;
    }
    public ImageProcessingSettings DistributeCache(string port)
    {
        _stringBuilder.Append($" -distribute-cache {port} ");

        return this;
    }
    public ImageProcessingSettings Distort(string type, string coefficients)
    {
        _stringBuilder.Append($" -distort {type} {coefficients} ");

        return this;
    }
    public ImageProcessingSettings Dither(string method)
    {
        _stringBuilder.Append($" -dither {method} ");

        return this;
    }

    public ImageProcessingSettings Draw(string value)
    {
        _stringBuilder.Append($" -draw {value} ");

        return this;
    }
    public ImageProcessingSettings Duplicate(string count, string indexes)
    {
        _stringBuilder.Append($" -duplicate {count} {indexes} ");

        return this;
    }
    public ImageProcessingSettings Edge(string radius)
    {
        _stringBuilder.Append($" -edge {radius} ");

        return this;
    }
    public ImageProcessingSettings Emboss(string radius)
    {
        _stringBuilder.Append($" -emboss {radius} ");

        return this;
    }
    public ImageProcessingSettings Encipher(string filename)
    {
        _stringBuilder.Append($" -encipher {filename} ");

        return this;
    }
    public ImageProcessingSettings Encoding(string type)
    {
        _stringBuilder.Append($" -encoding {type} ");

        return this;
    }
    public ImageProcessingSettings Endian(string type)
    {
        _stringBuilder.Append($" -endian {type} ");

        return this;
    }
    public ImageProcessingSettings Enhance()
    {
        _stringBuilder.Append(" -enhance ");

        return this;
    }
    public ImageProcessingSettings Equalize()
    {
        _stringBuilder.Append(" -equalize ");

        return this;
    }
    public ImageProcessingSettings Evaluate(string @operator, string value)
    {
        _stringBuilder.Append($" -evaluate {@operator} {value} ");

        return this;
    }
    public ImageProcessingSettings EvaluateSequence(string @operator)
    {
        _stringBuilder.Append($" -evaluate-sequence {@operator} ");

        return this;
    }
    public ImageProcessingSettings Extent(string geometry)
    {
        _stringBuilder.Append($" -extent {geometry}");

        return this;
    }

    public ImageProcessingSettings Extract(string geometry)
    {
        _stringBuilder.Append($" -extract {geometry} ");

        return this;
    }
    public ImageProcessingSettings Family(string name)
    {
        _stringBuilder.Append($" -family {name} ");

        return this;
    }
    public ImageProcessingSettings Features(string distance)
    {
        _stringBuilder.Append($" -features {distance} ");

        return this;
    }
    public ImageProcessingSettings FFT()
    {
        _stringBuilder.Append(" -fft ");

        return this;
    }
    public ImageProcessingSettings Fill(string color)
    {
        _stringBuilder.Append($" -fill {color} ");

        return this;
    }

    public ImageProcessingSettings Flatten()
    {
        _stringBuilder.Append(" -flatten ");

        return this;
    }
    public ImageProcessingSettings Flip()
    {
        _stringBuilder.Append(" -flip ");

        return this;
    }
    public ImageProcessingSettings Floodfill(string geometry, string color)
    {
        _stringBuilder.Append($" -floodfill {geometry} {color} ");

        return this;
    }
    public ImageProcessingSettings Flop()
    {
        _stringBuilder.Append(" -flop ");

        return this;
    }
    public ImageProcessingSettings Font(string name)
    {
        _stringBuilder.Append($" -font {name} ");

        return this;
    }

    public ImageProcessingSettings Frame(string geometry)
    {
        _stringBuilder.Append($" -frame {geometry} ");

        return this;
    }
    public ImageProcessingSettings Function(string name)
    {
        _stringBuilder.Append($" -function {name} ");

        return this;
    }
    public ImageProcessingSettings Fuzz(string distance)
    {
        _stringBuilder.Append($" -fuzz {distance} ");

        return this;
    }
    public ImageProcessingSettings FX(string expression)
    {
        _stringBuilder.Append($" -fx {expression} ");

        return this;
    }

    public ImageProcessingSettings Gamma(string value)
    {
        _stringBuilder.Append($" -gamma {value} ");

        return this;
    }
    public ImageProcessingSettings GaussianBlur(string geometry)
    {
        _stringBuilder.Append($" -gaussian-blur {geometry} ");

        return this;
    }
    public ImageProcessingSettings Generate(string noiseType)
    {
        _stringBuilder.Append($" -generate {noiseType} ");

        return this;
    }
    public ImageProcessingSettings GenerateSignature()
    {
        _stringBuilder.Append(" -generate-signature ");

        return this;
    }
    public ImageProcessingSettings Get(string attribute)
    {
        _stringBuilder.Append($" -get {attribute} ");

        return this;
    }
    public ImageProcessingSettings GlobalThreshold(string geometry)
    {
        _stringBuilder.Append($" -global-threshold {geometry} ");

        return this;
    }
    public ImageProcessingSettings Gradient(string color)
    {
        _stringBuilder.Append($" -gradient {color} ");

        return this;
    }
    public ImageProcessingSettings Graph(string pixel)
    {
        _stringBuilder.Append($" -graph {pixel} ");

        return this;
    }
    public ImageProcessingSettings Grayscale(string method)
    {
        _stringBuilder.Append($" -grayscale {method} ");

        return this;
    }
    public ImageProcessingSettings GreenPrimary(string point)
    {
        _stringBuilder.Append($" -green-primary {point} ");

        return this;
    }
    public ImageProcessingSettings Halftone(string pattern)
    {
        _stringBuilder.Append($" -halftone {pattern} ");

        return this;
    }
    public ImageProcessingSettings HighlightColor(string color)
    {
        _stringBuilder.Append($" -highlight-color {color} ");

        return this;
    }
    public ImageProcessingSettings HighlightStyle(string style)
    {
        _stringBuilder.Append($" -highlight-style {style} ");

        return this;
    }
    public ImageProcessingSettings Histogram()
    {
        _stringBuilder.Append(" -histogram ");

        return this;
    }

    public ImageProcessingSettings HoughLine(string geometry)
    {
        _stringBuilder.Append($" -hough-line {geometry} ");

        return this;
    }
    public ImageProcessingSettings HSL(string type)
    {
        _stringBuilder.Append($" -hsl {type} ");

        return this;
    }
    public ImageProcessingSettings HSB()
    {
        _stringBuilder.Append(" -hsb ");

        return this;
    }
    public ImageProcessingSettings HSLColor()
    {
        _stringBuilder.Append(" -hsl-color ");

        return this;
    }
    public ImageProcessingSettings HWEIGHT(string value)
    {
        _stringBuilder.Append($" -hweight {value} ");

        return this;
    }
    public ImageProcessingSettings IDENTIFY()
    {
        _stringBuilder.Append(" -identify ");

        return this;
    }
    public ImageProcessingSettings IDENTITY()
    {
        _stringBuilder.Append(" -identity ");

        return this;
    }
    public ImageProcessingSettings If(string expression)
    {
        _stringBuilder.Append($" -if {expression} ");

        return this;
    }
    public ImageProcessingSettings Implode(string value)
    {
        _stringBuilder.Append($" -implode {value} ");

        return this;
    }
    public ImageProcessingSettings Import()
    {
        _stringBuilder.Append(" -import ");

        return this;
    }
    public ImageProcessingSettings Indent(string value)
    {
        _stringBuilder.Append($" -indent {value} ");

        return this;
    }
    public ImageProcessingSettings Info()
    {
        _stringBuilder.Append(" -info ");

        return this;
    }
    public ImageProcessingSettings InkCoverage(string value)
    {
        _stringBuilder.Append($" -ink-coverage {value} ");

        return this;
    }
    public ImageProcessingSettings Insert(string index)
    {
        _stringBuilder.Append($" -insert {index} ");

        return this;
    }
    public ImageProcessingSettings Intent(string type)
    {
        _stringBuilder.Append($" -intent {type} ");

        return this;
    }
    public ImageProcessingSettings Interlace(string type)
    {
        _stringBuilder.Append($" -interlace {type} ");

        return this;
    }

    public ImageProcessingSettings Interpolate(string type)
    {
        _stringBuilder.Append($" -interpolate {type} ");

        return this;
    }
    public ImageProcessingSettings Inverse()
    {
        _stringBuilder.Append(" -inverse ");

        return this;
    }
    public ImageProcessingSettings Invert()
    {
        _stringBuilder.Append(" -invert ");

        return this;
    }
    public ImageProcessingSettings Iterations(string value)
    {
        _stringBuilder.Append($" -iterations {value} ");

        return this;
    }
    public ImageProcessingSettings Label(string value)
    {
        _stringBuilder.Append($" -label {value} ");

        return this;
    }
    public ImageProcessingSettings Lat(string value)
    {
        _stringBuilder.Append($" -lat {value} ");

        return this;
    }
    public ImageProcessingSettings Layer(string type)
    {
        _stringBuilder.Append($" -layer {type} ");

        return this;
    }
    public ImageProcessingSettings Level(string value)
    {
        _stringBuilder.Append($" -level {value} ");

        return this;
    }
    public ImageProcessingSettings LevelColors(string value)
    {
        _stringBuilder.Append($" -level-colors {value} ");

        return this;
    }
    public ImageProcessingSettings Limit(string type)
    {
        _stringBuilder.Append($" -limit {type} ");

        return this;
    }
    public ImageProcessingSettings LinearStretch(string value)
    {
        _stringBuilder.Append($" -linear-stretch {value} ");

        return this;
    }
    public ImageProcessingSettings LineSize(string value)
    {
        _stringBuilder.Append($" -line-size {value} ");

        return this;
    }
    public ImageProcessingSettings LiquidRescale(string value)
    {
        _stringBuilder.Append($" -liquid-rescale {value} ");

        return this;
    }
    public ImageProcessingSettings List(string type)
    {
        _stringBuilder.Append($" -list {type} ");

        return this;
    }

    /// <summary>
    /// Applies a matte color to the frame color
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public ImageProcessingSettings MatteColor(string color)
    {
        _stringBuilder.Append($" -mattecolor {color} ");

        return this;
    }

    /// <summary>
    /// Applies a median filter to the image with a given radius
    /// </summary>
    public ImageProcessingSettings Median(string radius)
    {
        _stringBuilder.Append($" -median {radius} ");

        return this;
    }

    /// <summary>
    /// Delineates arbitrarily shaped clusters in the image using mean-shift geometry
    /// </summary>
    public ImageProcessingSettings MeanShift(string geometry)
    {
        _stringBuilder.Append($" -mean-shift {geometry} ");

        return this;
    }

    /// <summary>
    /// Measures differences between images using the specified metric type
    /// </summary>
    public ImageProcessingSettings Metric(string type)
    {
        _stringBuilder.Append($" -metric {type} ");

        return this;
    }

    /// <summary>
    /// Makes each pixel the 'predominant color' of the neighborhood with a given radius
    /// </summary>
    public ImageProcessingSettings Mode(string radius)
    {
        _stringBuilder.Append($" -mode {radius} ");

        return this;
    }

    /// <summary>
    /// Varies the brightness, saturation, and hue of the image by a given value
    /// </summary>
    public ImageProcessingSettings Modulate(string value)
    {
        _stringBuilder.Append($" -modulate {value} ");

        return this;
    }

    /// <summary>
    /// Monitors the progress of the image processing
    /// </summary>
    public ImageProcessingSettings Monitor()
    {
        _stringBuilder.Append(" -monitor ");

        return this;
    }

    /// <summary>
    /// Transforms the image to black and white
    /// </summary>
    public ImageProcessingSettings Monochrome()
    {
        _stringBuilder.Append(" -monochrome ");

        return this;
    }

    /// <summary>
    /// Morphs an image sequence by a given value
    /// </summary>
    public ImageProcessingSettings Morph(string value)
    {
        _stringBuilder.Append($" -morph {value} ");

        return this;
    }

    /// <summary>
    /// Applies a morphology method to the image using a given kernel
    /// </summary>
    public ImageProcessingSettings Morphology(string method, string kernel)
    {
        _stringBuilder.Append($" -morphology {method} {kernel} ");

        return this;
    }

    /// <summary>
    /// Simulates motion blur on the image using a given geometry
    /// </summary>
    public ImageProcessingSettings MotionBlur(string geometry)
    {
        _stringBuilder.Append($" -motion-blur {geometry} ");

        return this;
    }

    /// <summary>
    /// Replaces each pixel with its complementary color
    /// </summary>
    public ImageProcessingSettings Negate()
    {
        _stringBuilder.Append(" -negate ");

        return this;
    }

    /// <summary>
    /// Adds or reduces noise in an image with a given radius
    /// </summary>
    public ImageProcessingSettings Noise(string radius)
    {
        _stringBuilder.Append($" -noise {radius} ");

        return this;
    }

    /// <summary>
    /// Transforms the image to span the full range of colors
    /// </summary>
    public ImageProcessingSettings Normalize()
    {
        _stringBuilder.Append(" -normalize ");

        return this;
    }

    /// <summary>
    /// Changes the specified color to the fill color
    /// </summary>
    /// <param name="color">The color to be changed to the fill color</param>
    public ImageProcessingSettings Opaque(string color)
    {
        _stringBuilder.Append($" -opaque {color} ");

        return this;
    }

    /// <summary>
    /// Applies ordered dithering to the image with a given NxN matrix
    /// </summary>
    /// <param name="nxn">The size of the dithering matrix</param>
    public ImageProcessingSettings OrderedDither(string nxn)
    {
        _stringBuilder.Append($" -ordered-dither {nxn} ");

        return this;
    }

    /// <summary>
    /// Sets the orientation of the image
    /// </summary>
    /// <param name="type">The type of orientation to apply</param>
    public ImageProcessingSettings Orient(string type)
    {
        _stringBuilder.Append($" -orient {type} ");

        return this;
    }

    /// <summary>
    /// Sets the size and location of the image canvas
    /// </summary>
    /// <param name="geometry">The size and location of the canvas</param>
    public ImageProcessingSettings Page(string geometry)
    {
        _stringBuilder.Append($" -page {geometry} ");

        return this;
    }

    /// <summary>
    /// Simulates an oil painting effect on the image with a given radius
    /// </summary>
    /// <param name="radius">The radius of the painting effect</param>
    public ImageProcessingSettings Paint(string radius)
    {
        _stringBuilder.Append($" -paint {radius} ");

        return this;
    }

    /// <summary>
    /// Sets the value of each pixel whose value is less than |epsilon| to -epsilon or epsilon
    /// </summary>
    public ImageProcessingSettings Perceptible()
    {
        _stringBuilder.Append(" -perceptible ");

        return this;
    }

    /// <summary>
    /// Efficiently determine image attributes
    /// </summary>
    public ImageProcessingSettings Ping()
    {
        _stringBuilder.Append(" -ping ");

        return this;
    }

    /// <summary>
    /// Sets the font point size
    /// </summary>
    /// <param name="value">The font point size</param>
    public ImageProcessingSettings Pointsize(string value)
    {
        _stringBuilder.Append($" -pointsize {value} ");

        return this;
    }

    /// <summary>
    /// Simulates a Polaroid picture with a given angle
    /// </summary>
    /// <param name="angle">The angle of the Polaroid effect</param>
    public ImageProcessingSettings Polaroid(string angle)
    {
        _stringBuilder.Append($" -polaroid {angle} ");

        return this;
    }

    /// <summary>
    /// Builds a polynomial from the image sequence and the corresponding terms (coefficients and degree pairs)
    /// </summary>
    /// <param name="terms">The terms of the polynomial</param>
    public ImageProcessingSettings Poly(string terms)
    {
        _stringBuilder.Append($" -poly {terms} ");

        return this;
    }

    /// <summary>
    /// Reduces the image to a limited number of color levels
    /// </summary>
    /// <param name="levels">The number of color levels</param>
    public ImageProcessingSettings Posterize(string levels)
    {
        _stringBuilder.Append($" -posterize {levels} ");

        return this;
    }

    /// <summary>
    /// Sets the maximum number of significant digits to be printed
    /// </summary>
    /// <param name="value">The number of significant digits</param>
    public ImageProcessingSettings Precision(string value)
    {
        _stringBuilder.Append($" -precision {value} ");

        return this;
    }

    /// <summary>
    /// Sets the image preview type
    /// </summary>
    /// <param name="type">The type of preview</param>
    public ImageProcessingSettings Preview(string type)
    {
        _stringBuilder.Append($" -preview {type} ");

        return this;
    }

    /// <summary>
    /// Interprets the given string and prints it to the console
    /// </summary>
    /// <param name="str">The string to be printed</param>
    public ImageProcessingSettings Print(string str)
    {
        _stringBuilder.Append($" -print {str} ");

        return this;
    }

    /// <summary>
    /// Processes the image with a custom image filter
    /// </summary>
    /// <param name="imageFilter">The custom image filter</param>
    public ImageProcessingSettings Process(string imageFilter)
    {
        _stringBuilder.Append($" -process {imageFilter} ");

        return this;
    }

    /// <summary>
    /// Adds, deletes, or applies an image profile
    /// </summary>
    /// <param name="filename">The name of the profile file</param>
    public ImageProcessingSettings Profile(string filename)
    {
        _stringBuilder.Append($" -profile {filename} ");

        return this;
    }

    /// <summary>
    /// Reduces the number of colors in the image in the specified colorspace
    /// </summary>
    /// <param name="colorspace">The colorspace to use for quantization</param>
    public ImageProcessingSettings Quantize(string colorspace)
    {
        _stringBuilder.Append($" -quantize {colorspace} ");

        return this;
    }

    /// <summary>
    /// Suppresses all warning messages
    /// </summary>
    public ImageProcessingSettings Quiet()
    {
        _stringBuilder.Append(" -quiet ");

        return this;
    }

    /// <summary>
    /// Applies a radial blur to the image with a given angle
    /// </summary>
    /// <param name="angle">The angle of the rotational blur</param>
    public ImageProcessingSettings RotationalBlur(string angle)
    {
        _stringBuilder.Append($" -rotational-blur {angle} ");

        return this;
    }

    /// <summary>
    /// Lightens or darkens the edges of the image to create a 3-D effect
    /// </summary>
    /// <param name="value">The value to raise the edges by</param>
    public ImageProcessingSettings Raise(string value)
    {
        _stringBuilder.Append($" -raise {value} ");

        return this;
    }

    /// <summary>
    /// Applies a random threshold to the image within the given range
    /// </summary>
    /// <param name="low">The low threshold value</param>
    /// <param name="high">The high threshold value</param>
    public ImageProcessingSettings RandomThreshold(string low, string high)
    {
        _stringBuilder.Append($" -random-threshold {low} {high} ");

        return this;
    }

    /// <summary>
    /// Performs hard or soft thresholding within a given range of values in the image
    /// </summary>
    /// <param name="lowBlack">The low black threshold value</param>
    /// <param name="lowWhite">The low white threshold value</param>
    /// <param name="highWhite">The high white threshold value</param>
    /// <param name="highBlack">The high black threshold value</param>
    public ImageProcessingSettings RangeThreshold(string lowBlack, string lowWhite, string highWhite, string highBlack)
    {
        _stringBuilder.Append($" -range-threshold {lowBlack} {lowWhite} {highWhite} {highBlack} ");

        return this;
    }

    /// <summary>
    /// Associates a read mask with the image
    /// </summary>
    /// <param name="filename">The name of the read mask file</param>
    public ImageProcessingSettings ReadMask(string filename)
    {
        _stringBuilder.Append($" -read-mask {filename} ");

        return this;
    }

    /// <summary>
    /// Sets the chromaticity red primary point
    /// </summary>
    /// <param name="point">The red primary point</param>
    public ImageProcessingSettings RedPrimary(string point)
    {
        _stringBuilder.Append($" -red-primary {point} ");

        return this;
    }

    /// <summary>
    /// Pays attention to warning messages
    /// </summary>
    public ImageProcessingSettings RegardWarnings()
    {
        _stringBuilder.Append(" -regard-warnings ");

        return this;
    }

    /// <summary>
    /// Applies options to a portion of the image
    /// </summary>
    /// <param name="geometry">The region to apply options to</param>
    public ImageProcessingSettings Region(string geometry)
    {
        _stringBuilder.Append($" -region {geometry} ");

        return this;
    }

    /// <summary>
    /// Transforms the image colors to match a given set of colors
    /// </summary>
    /// <param name="filename">The file containing the set of colors to match</param>
    public ImageProcessingSettings Remap(string filename)
    {
        _stringBuilder.Append($" -remap {filename} ");

        return this;
    }

    /// <summary>
    /// Renders vector graphics
    /// </summary>
    public ImageProcessingSettings Render()
    {
        _stringBuilder.Append(" -render ");

        return this;
    }

    /// <summary>
    /// Sets the size and location of the image canvas
    /// </summary>
    /// <param name="geometry">The size and location of the canvas</param>
    public ImageProcessingSettings Repage(string geometry)
    {
        _stringBuilder.Append($" -repage {geometry} ");

        return this;
    }

    /// <summary>
    /// Changes the resolution of an image
    /// </summary>
    /// <param name="geometry">The new resolution of the image</param>
    public ImageProcessingSettings Resample(string geometry)
    {
        _stringBuilder.Append($" -resample {geometry} ");

        return this;
    }

    /// <summary>
    /// Resizes the image
    /// </summary>
    /// <param name="geometry">The new size of the image</param>
    public ImageProcessingSettings Resize(string geometry)
    {
        _stringBuilder.Append($" -resize {geometry} ");

        return this;
    }

    /// <summary>
    /// Settings remain in effect until the parenthesis boundary
    /// </summary>
    public ImageProcessingSettings RespectParentheses()
    {
        _stringBuilder.Append(" -respect-parentheses ");

        return this;
    }

    /// <summary>
    /// Rolls the image vertically or horizontally
    /// </summary>
    /// <param name="geometry">The direction and amount to roll the image</param>
    public ImageProcessingSettings Roll(string geometry)
    {
        _stringBuilder.Append($" -roll {geometry} ");

        return this;
    }

    /// <summary>
    /// Applies Paeth rotation to the image
    /// </summary>
    /// <param name="degrees">The amount of rotation in degrees</param>
    public ImageProcessingSettings Rotate(string degrees)
    {
        _stringBuilder.Append($" -rotate {degrees} ");

        return this;
    }

    /// <summary>
    /// Scales the image with pixel sampling
    /// </summary>
    /// <param name="geometry">The new size of the image</param>
    public ImageProcessingSettings Sample(string geometry)
    {
        _stringBuilder.Append($" -sample {geometry} ");

        return this;
    }

    /// <summary>
    /// Sets the horizontal and vertical sampling factor
    /// </summary>
    /// <param name="geometry">The horizontal and vertical sampling factor</param>
    public ImageProcessingSettings SamplingFactor(string geometry)
    {
        _stringBuilder.Append($" -sampling-factor {geometry} ");

        return this;
    }

    /// <summary>
    /// Scales the image
    /// </summary>
    /// <param name="geometry">The new size of the image</param>
    public ImageProcessingSettings Scale(string geometry)
    {
        _stringBuilder.Append($" -scale {geometry} ");

        return this;
    }

    /// <summary>
    /// Sets the image scene number
    /// </summary>
    /// <param name="value">The scene number</param>
    public ImageProcessingSettings Scene(string value)
    {
        _stringBuilder.Append($" -scene {value} ");

        return this;
    }

    /// <summary>
    /// Seeds a new sequence of pseudo-random numbers
    /// </summary>
    /// <param name="value">The seed value</param>
    public ImageProcessingSettings Seed(string value)
    {
        _stringBuilder.Append($" -seed {value} ");

        return this;
    }

    /// <summary>
    /// Segments an image
    /// </summary>
    /// <param name="values">The segmentation values</param>
    public ImageProcessingSettings Segment(string values)
    {
        _stringBuilder.Append($" -segment {values} ");

        return this;
    }

    /// <summary>
    /// Selectively blurs pixels within a contrast threshold
    /// </summary>
    /// <param name="geometry">The contrast threshold and blur radius</param>
    public ImageProcessingSettings SelectiveBlur(string geometry)
    {
        _stringBuilder.Append($" -selective-blur {geometry} ");

        return this;
    }

    /// <summary>
    /// Separates an image channel into a grayscale image
    /// </summary>
    public ImageProcessingSettings Separate()
    {
        _stringBuilder.Append(" -separate ");

        return this;
    }

    /// <summary>
    /// Simulates a sepia-toned photo
    /// </summary>
    /// <param name="threshold">The threshold value for the sepia effect</param>
    public ImageProcessingSettings SepiaTone(string threshold)
    {
        _stringBuilder.Append($" -sepia-tone {threshold}% ");

        return this;
    }

    /// <summary>
    /// Sets an image attribute
    /// </summary>
    /// <param name="attribute">The attribute to set</param>
    /// <param name="value">The value of the attribute</param>
    public ImageProcessingSettings SetAttribute(string attribute, string value)
    {
        _stringBuilder.Append($" -set {attribute} {value} ");

        return this;
    }

    /// <summary>
    /// Shades the image using a distant light source
    /// </summary>
    /// <param name="degrees">The angle of the light source</param>
    public ImageProcessingSettings Shade(string degrees)
    {
        _stringBuilder.Append($" -shade {degrees} ");

        return this;
    }

    /// <summary>
    /// Simulates an image shadow
    /// </summary>
    /// <param name="geometry">The offset and blur of the shadow</param>
    public ImageProcessingSettings Shadow(string geometry)
    {
        _stringBuilder.Append($" -shadow {geometry} ");

        return this;
    }

    /// <summary>
    /// Sharpens the image
    /// </summary>
    /// <param name="geometry">The sharpness radius and sigma</param>
    public ImageProcessingSettings Sharpen(string geometry)
    {
        _stringBuilder.Append($" -sharpen {geometry} ");

        return this;
    }

    /// <summary>
    /// Shaves pixels from the image edges
    /// </summary>
    /// <param name="geometry">The number of pixels to shave from the edges</param>
    public ImageProcessingSettings Shave(string geometry)
    {
        _stringBuilder.Append($" -shave {geometry} ");

        return this;
    }

    /// <summary>
    /// Slides one edge of the image along the X or Y axis
    /// </summary>
    /// <param name="geometry">The X or Y offset and angle of the shear</param>
    public ImageProcessingSettings Shear(string geometry)
    {
        _stringBuilder.Append($" -shear {geometry} ");

        return this;
    }

    /// <summary>
    /// Increases the contrast without saturating highlights or shadows
    /// </summary>
    /// <param name="geometry">The contrast and mid-point values</param>
    public ImageProcessingSettings SigmoidalContrast(string geometry)
    {
        _stringBuilder.Append($" -sigmoidal-contrast {geometry} ");

        return this;
    }

    /// <summary>
    /// Smushes an image sequence together
    /// </summary>
    /// <param name="offset">The offset of the smush</param>
    public ImageProcessingSettings Smush(string offset)
    {
        _stringBuilder.Append($" -smush {offset} ");

        return this;
    }

    /// <summary>
    /// Sets the width and height of the image
    /// </summary>
    /// <param name="geometry">The width and height of the image</param>
    public ImageProcessingSettings Size(string geometry)
    {
        _stringBuilder.Append($" -size {geometry} ");

        return this;
    }

    /// <summary>
    /// Simulates a pencil sketch
    /// </summary>
    /// <param name="geometry">The sketch settings</param>
    public ImageProcessingSettings Sketch(string geometry)
    {
        _stringBuilder.Append($" -sketch {geometry} ");

        return this;
    }

    /// <summary>
    /// Negates all pixels above the threshold level
    /// </summary>
    /// <param name="threshold">The threshold level</param>
    public ImageProcessingSettings Solarize(string threshold)
    {
        _stringBuilder.Append($" -solarize {threshold} ");

        return this;
    }

    /// <summary>
    /// Sorts pixels within each scanline in ascending order of intensity
    /// </summary>
    public ImageProcessingSettings SortPixels()
    {
        _stringBuilder.Append(" -sort-pixels ");

        return this;
    }

    /// <summary>
    /// Splices the background color into the image
    /// </summary>
    /// <param name="geometry">The number of pixels to splice and the gravity direction</param>
    public ImageProcessingSettings Splice(string geometry)
    {
        _stringBuilder.Append($" -splice {geometry} ");

        return this;
    }

    /// <summary>
    /// Displaces image pixels by a random amount
    /// </summary>
    /// <param name="radius">The maximum random displacement of pixels</param>
    public ImageProcessingSettings Spread(string radius)
    {
        _stringBuilder.Append($" -spread {radius} ");

        return this;
    }

    /// <summary>
    /// Replaces each pixel with corresponding statistic from the neighborhood
    /// </summary>
    /// <param name="type">The type of statistic to use (e.g. "mean", "median", etc.)</param>
    /// <param name="geometry">The size of the neighborhood</param>
    public ImageProcessingSettings Statistic(string type, string geometry)
    {
        _stringBuilder.Append($" -statistic {type} {geometry} ");

        return this;
    }

    /// <summary>
    /// Strip image of all profiles and comments
    /// </summary>
    public ImageProcessingSettings Strip()
    {
        _stringBuilder.Append(" -strip ");

        return this;
    }

    /// <summary>
    /// Sets the graphic primitive stroke color
    /// </summary>
    /// <param name="color">The stroke color</param>
    public ImageProcessingSettings Stroke(string color)
    {
        _stringBuilder.Append($" -stroke {color} ");

        return this;
    }

    /// <summary>
    /// Sets the graphic primitive stroke width
    /// </summary>
    /// <param name="value">The stroke width</param>
    public ImageProcessingSettings StrokeWidth(string value)
    {
        _stringBuilder.Append($" -strokewidth {value} ");

        return this;
    }

    /// <summary>
    /// Render text with this font stretch
    /// </summary>
    /// <param name="type">The font stretch type (e.g. "condensed", "expanded", etc.)</param>
    public ImageProcessingSettings Stretch(string type)
    {
        _stringBuilder.Append($" -stretch {type} ");

        return this;
    }

    /// <summary>
    /// Render text with this font style
    /// </summary>
    /// <param name="type">The font style type (e.g. "italic", "oblique", etc.)</param>
    public ImageProcessingSettings Style(string type)
    {
        _stringBuilder.Append($" -style {type} ");

        return this;
    }

    /// <summary>
    /// Swap two images in the image sequence
    /// </summary>
    /// <param name="indexes">The indexes of the images to swap</param>
    public ImageProcessingSettings Swap(string indexes)
    {
        _stringBuilder.Append($" -swap {indexes} ");

        return this;
    }

    /// <summary>
    /// Swirl image pixels about the center
    /// </summary>
    /// <param name="degrees">The number of degrees to swirl the pixels</param>
    public ImageProcessingSettings Swirl(string degrees)
    {
        _stringBuilder.Append($" -swirl {degrees} ");

        return this;
    }

    /// <summary>
    /// Synchronize image to storage device
    /// </summary>
    public ImageProcessingSettings Synchronize()
    {
        _stringBuilder.Append(" -synchronize ");

        return this;
    }

    /// <summary>
    /// Tiles the specified texture onto the image background
    /// </summary>
    /// <param name="filename">The name of the texture file</param>
    public ImageProcessingSettings Texture(string filename)
    {
        _stringBuilder.Append($" -texture {filename} ");

        return this;
    }

    /// <summary>
    /// Thresholds the image
    /// </summary>
    /// <param name="value">The threshold value</param>
    public ImageProcessingSettings Threshold(string value)
    {
        _stringBuilder.Append($" -threshold {value} ");

        return this;
    }

    /// <summary>
    /// Creates a thumbnail of the image
    /// </summary>
    /// <param name="geometry">The size of the thumbnail</param>
    public ImageProcessingSettings Thumbnail(string geometry)
    {
        _stringBuilder.Append($" -thumbnail {geometry} ");

        return this;
    }

    /// <summary>
    /// Tiles the specified image when filling a graphic primitive
    /// </summary>
    /// <param name="filename">The name of the image file</param>
    public ImageProcessingSettings Tile(string filename)
    {
        _stringBuilder.Append($" -tile {filename} ");

        return this;
    }

    /// <summary>
    /// Set force output format
    /// </summary>
    public ImageProcessingSettings Format(FileFormatType? format)
    {
        if(format is null)
            return this;

        _stringBuilder.Append($" {format.ToString()
                                        .ToLower()}:");

        return this;
    }

    /// <summary>
    /// Set force output format
    /// </summary>
    public ImageProcessingSettings Format(ImageFormat? format)
    {
        if(format is null)
            return this;

        _stringBuilder.Append($" {format.ToString()
                                        .ToLower()}:");

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
    private string StandartInputRedirectArgument => "- ";

    /// <summary>
    /// Setting Output Arguments
    /// </summary>
    public ImageProcessingSettings SetOutputFileArguments(string? arg)
    {
        OutputFileArguments = arg;

        return this;
    }

    /// <summary>
    /// Sets the transparent color
    /// </summary>
    /// <param name="color">The transparent color</param>
    public ImageProcessingSettings TransparentColor(string color)
    {
        _stringBuilder.Append($" -transparent-color {color} ");

        return this;
    }

    /// <summary>
    /// Flips the image in the vertical direction and rotates 90 degrees
    /// </summary>
    public ImageProcessingSettings Transpose()
    {
        _stringBuilder.Append(" -transpose ");

        return this;
    }

    /// <summary>
    /// Flops the image in the horizontal direction and rotates 270 degrees
    /// </summary>
    public ImageProcessingSettings Transverse()
    {
        _stringBuilder.Append(" -transverse ");

        return this;
    }

    /// <summary>
    /// Trims image edges
    /// </summary>
    public ImageProcessingSettings Trim()
    {
        _stringBuilder.Append(" -trim ");

        return this;
    }

    /// <summary>
    /// Sets the color tree depth
    /// </summary>
    /// <param name="value">The color tree depth</param>
    public ImageProcessingSettings TreeDepth(string value)
    {
        _stringBuilder.Append($" -treedepth {value} ");

        return this;
    }

    /// <summary>
    /// Sets the image type
    /// </summary>
    /// <param name="type">The image type</param>
    public ImageProcessingSettings Type(string type)
    {
        _stringBuilder.Append($" -type {type} ");

        return this;
    }

    /// <summary>
    /// Sets the annotation bounding box color
    /// </summary>
    /// <param name="color">The annotation bounding box color</param>
    public ImageProcessingSettings Undercolor(string color)
    {
        _stringBuilder.Append($" -undercolor {color} ");

        return this;
    }

    /// <summary>
    /// Discards all but one of any pixel color
    /// </summary>
    public ImageProcessingSettings UniqueColors()
    {
        _stringBuilder.Append(" -unique-colors ");

        return this;
    }

    /// <summary>
    /// Sets the units of image resolution
    /// </summary>
    /// <param name="type">The units of image resolution</param>
    public ImageProcessingSettings Units(string type)
    {
        _stringBuilder.Append($" -units {type} ");

        return this;
    }

    /// <summary>
    /// Sharpens the image
    /// </summary>
    /// <param name="geometry">The sharpen geometry</param>
    public ImageProcessingSettings Unsharp(string geometry)
    {
        _stringBuilder.Append($" -unsharp {geometry} ");

        return this;
    }

    /// <summary>
    /// Prints version information
    /// </summary>
    public ImageProcessingSettings Version()
    {
        _stringBuilder.Append(" -version ");

        return this;
    }

    /// <summary>
    /// Enables FlashPix viewing transforms
    /// </summary>
    public ImageProcessingSettings View()
    {
        _stringBuilder.Append(" -view ");

        return this;
    }

    /// <summary>
    /// Softens the edges of the image in vignette style
    /// </summary>
    /// <param name="geometry">The vignette geometry</param>
    public ImageProcessingSettings Vignette(string geometry)
    {
        _stringBuilder.Append($" -vignette {geometry} ");

        return this;
    }

    /// <summary>
    /// Sets the access method for pixels outside the boundaries of the image
    /// </summary>
    /// <param name="method">The access method for pixels outside the boundaries of the image</param>
    public ImageProcessingSettings VirtualPixel(string method)
    {
        _stringBuilder.Append($" -virtual-pixel {method} ");

        return this;
    }

    /// <summary>
    /// Alters an image along a sine wave
    /// </summary>
    /// <param name="geometry">The wave geometry</param>
    public ImageProcessingSettings Wave(string geometry)
    {
        _stringBuilder.Append($" -wave {geometry} ");

        return this;
    }

    /// <summary>
    /// Removes noise from the image using a wavelet transform
    /// </summary>
    /// <param name="threshold">The wavelet denoise threshold</param>
    public ImageProcessingSettings WaveletDenoise(string threshold)
    {
        _stringBuilder.Append($" -wavelet-denoise {threshold} ");

        return this;
    }

    /// <summary>
    /// Renders text with the specified font weight
    /// </summary>
    /// <param name="type">The font weight</param>
    public ImageProcessingSettings Weight(string type)
    {
        _stringBuilder.Append($" -weight {type} ");

        return this;
    }

    /// <summary>
    /// Sets the chromaticity white point
    /// </summary>
    /// <param name="point">The chromaticity white point</param>
    public ImageProcessingSettings WhitePoint(string point)
    {
        _stringBuilder.Append($" -white-point {point} ");

        return this;
    }

    /// <summary>
    /// Forces all pixels above the threshold into white
    /// </summary>
    /// <param name="value">The white threshold value</param>
    public ImageProcessingSettings WhiteThreshold(string value)
    {
        _stringBuilder.Append($" -white-threshold {value} ");

        return this;
    }

    /// <summary>
    /// Sets whether line breaks appear wherever the text would otherwise overflow its content box.
    /// Choose from normal, the default, or break-word.
    /// </summary>
    /// <param name="type">The word break type</param>
    public ImageProcessingSettings WordBreak(string type)
    {
        _stringBuilder.Append($" -word-break {type} ");

        return this;
    }

    /// <summary>
    /// Writes images to the specified file
    /// </summary>
    /// <param name="filename">The file name to write the images to</param>
    public ImageProcessingSettings Write(string filename)
    {
        _stringBuilder.Append($" -write {filename} ");

        return this;
    }

    /// <summary>
    /// Associates a write mask with the image
    /// </summary>
    /// <param name="filename">The file name of the write mask</param>
    public ImageProcessingSettings WriteMask(string filename)
    {
        _stringBuilder.Append($" -write-mask {filename} ");

        return this;
    }

    /// <summary>
    /// Prints detailed information about the image
    /// </summary>
    public ImageProcessingSettings Verbose()
    {
        _stringBuilder.Append(" -verbose ");

        return this;
    }

    /// <summary>
    /// Set input files
    /// </summary>
    public ImageProcessingSettings SetInputFiles(params MediaFile[]? files)
    {
        if(files is null)
            throw new NullReferenceException("'CustomInputs' Arguments must be specified if there are no input files");

        switch(files.Length)
        {
            case 0:
                throw new Exception("No input files");
            case 1:
                _stringBuilder.Append(files[0]
                                          .InputType is MediaFileInputType.Path or MediaFileInputType.Template or MediaFileInputType.NamedPipe
                                          ? files[0]
                                              .InputFilePath!
                                          : StandartInputRedirectArgument);

                SetInputStreams(files);

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

            SetInputStreams(files);

            return this;
        }

        _stringBuilder.Append(files.Aggregate(string.Empty,
                                              (current, file) => current
                                                + " "
                                                + (file.InputType is MediaFileInputType.Path or MediaFileInputType.Template or MediaFileInputType.NamedPipe
                                                      ? file.InputFilePath!
                                                      : SetPipeChannel(Guid.NewGuid()
                                                                           .ToString(),
                                                                       file))));

        SetInputStreams(files);

        return this;
    }

    /// <summary>
    /// Summary arguments to process
    /// </summary>
    public override string GetProcessArguments(bool setOutputArguments = true)
    {
        if(setOutputArguments)
            return _stringBuilder + GetOutputArguments();

        return _stringBuilder.ToString();
    }

    /// <summary>
    /// Get output arguments
    /// </summary>
    private string GetOutputArguments()
    {
        return OutputFileArguments ?? "- ";
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
    public override string[]? GetInputPipeNames()
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

        return pipeName.ToPipeDir();
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
            InputStreams.Add(files.First(x => x.InputType == MediaFileInputType.Stream)
                                  .InputFileStream!);
        }

        if (!(PipeNames?.Count > 0))
            return;

        InputStreams ??= new List<Stream>();
        InputStreams.AddRange(PipeNames.Select(pipeName => pipeName.Value));
    }
}