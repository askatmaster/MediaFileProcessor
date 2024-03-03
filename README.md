* [Overview](#overview)
* [FFmpeg instruction](#ffmpeg-instruction)
  * [File Processing Example](#file-processing-example)
  * [Get Video File Info](#get-video-file-info)
  * [Attention!](#attention)
    * [Important Note When Processing MP4 Files!](#important-note-when-processing-mp4-files)
    * [Example "Extract frame from video"](#example-extract-frame-from-video)
* [ImageMagick instruction](#imagemagick-instruction)
    * [An example of image compression in three options (directory path, stream, byte array)](#an-example-of-image-compression-in-three-options-directory-path-stream-byte-array)
* [Pandoc instruction](#pandoc-instruction)
* [Useful Features](#useful-features)
  * [MultiStream](#multistream)
    * [Example of using MultiStream](#example-of-using-multistream)
    * [On-the-fly stream decoding](#on-the-fly-stream-decoding)
    * [To learn more about the decoding process, I advise you to study the source code.](#to-learn-more-about-the-decoding-process-i-advise-you-to-study-the-source-code)
  * [FileDownloadProcessor](#filedownloadprocessor)
  * [ZipFileProcessor](#zipfileprocessor)
  * [MediaFileProcess](#mediafileprocess)
    * [Note on input streams and named pipes:](#note-on-input-streams-and-named-pipes)
  * [FileDataExtensions](#filedataextensions)
  * [FilesSignatureExtensions](#filessignatureextensions)
  * [StreamDecodeExtensions](#streamdecodeextensions)
* [Special thanks to](#special-thanks-to)

# Overview
C# (.NET Standard 2.1) OpenSource library for processing various files (videos, photos, documents, images).

```dotnet add package MediaFileProcessor --version 1.0.3```

This library is a universal wrapper for executable processes in the operating system (Windows/Linux).
The library allows files to communicate with processes through named pipes, streams, byte arrays, and directory paths.
It also has some useful features, such as the ability to decode a stream on the fly and get a set of files from it by their signatures. 

In this version, wrappers are implemented in the libraries over such projects as FFmpeg, ImageMagick and Pandoc.
This library can also be used to interact with third-party processes.

Below the presentation is an instruction for using this library and its description.

After reading the instructions, you can study the source code. it is extensively commented and has a simple architecture.

The first step is to define the data to be processed. The data to be processed is the ```MediaFile``` class.
You can create an instance of this class from a stream, a file path, an array of bytes, a named pipe, a naming pattern:

```csharp 
var fromPath = new MediaFile(@"C:\fileTest.avi");

var fromNamedPipe = new MediaFile("fileTestPipeName");

var fromPipe = new MediaFile("pipeName");

var fs = @"C:\fileTest.avi".ToFileStream();
var fromStream = new MediaFile(fs);

var bytes = @"C:\fileTest.avi".ToBytes();
var fromBytes = new MediaFile(bytes);
```

# FFmpeg instruction

To process video files with FFmpeg, you must have its executable file ffmpeg.exe.
If you don't want to download it yourself, you can use the following code:

```await VideoFileProcessor.DownloadExecutableFilesAsync();```

This code will download the archive from https://www.gyan.dev/ffmpeg/builds/ffmpeg-release-essentials.zip and unzip the required ffmpeg.exe to the root directory.

## File Processing Example

Below is an example of getting a frame from a video.

The ```VideoFileProcessor``` class is responsible for processing video files using ffmpeg.
You should create an instance of it:

```var videoFileProcessor = new VideoFileProcessor();```

Creation through the constructor without parameters implies that the executable files ffmpeg.exe and ffprobe.exe are located in the root folder.

If you have defined executable files in another directory, then you should create an instance of the processor by setting the paths to the executable files through the constructor:
```csharp
var videoFileProcessor = new VideoFileProcessor("pathToFFmpeg.exe", "pathToFFprobe.exe");
```

To specify how a file should be processed, we need to instantiate ```VideoProcessingSettings```.
Next, define the configuration for processing:
```csharp 
var settings = new VideoProcessingSettings();

var mediaFile = new MediaFile("pathToOutputFile");

settings.ReplaceIfExist()                          //Overwrite Output Files Without Prompting.
        .Seek(TimeSpan.FromMilliseconds(47500))    //A frame to start your search with.
        .SetInputFiles(mediaFile)                  //Install Input Files
        .FramesNumber(1)                           //Number of Video Frames to Output
        .Format(FileFormatType.JPG)                //Force Input or Output File Format.
        .SetOutputArguments(@"pathToInputFile");   //Configuring Output Arguments
```
Next, you just need to pass the configuration to the method  ```ExecuteAsync```:

```csharp 
var result = await videoFileProcessor.ExecuteAsync(settings, new CancellationToken());
```
The specified configuration methods will give us the following arguments to start the ffmpeg process:
```-y  -ss 00:00:47.500  -i pathToOutputFile  -frames:v 1  -f image2 pathToInputFile```.
It is necessary to OBSERVE the ORDER of the configurations, because some arguments must be given before the input argument and some after.

## Get Video File Info

```csharp
var videoProcessor = new VideoFileProcessor();

var stream = "pathToOutputFile".ToFileStream();
var data = await videoProcessor.GetVideoInfo(new MediaFile(stream));

var info = JsonConvert.DeserializeObject<VideoFileInfo>(data, _jsonSnakeCaseSerializerSettings)!;
```

## Attention!

When setting the process configuration, you can set the input data using the ```SetInputFiles``` method, which accepts an array of parameters in the form of instances of the ```MediaFile``` class.

You just need to create instances of this class from data presented in any form (path, stream, bytes, pipes, patterns) and pass it to the ```SetInputFiles``` method.
The ```SetOutputArguments``` method is responsible for setting the output file argument. Through this method, you can set the path of the output file, the rtp address of the server for broadcasting, etc.

If this method is not called, it means that the result of processing will be issued to ```StandardOutput``` as a stream. And the ```ExecuteAsync``` method will return the result on the thread.
If you set your own output argument, then ```StandardOutput``` will be empty and ```ExecuteAsync``` will return ```null```.

If you need to set an argument that is not present in the configuration methods, then you can set custom arguments using the ```CustomArguments``` method.

Full code:
```csharp
var mediaFile = new MediaFile(@"pathToOutputFile");

var videoFileProcessor = new VideoFileProcessor();

var settings = new VideoProcessingSettings();

settings.ReplaceIfExist()                        //Overwrite output files without asking.
        .Seek(TimeSpan.FromMilliseconds(47500))  //The frame to begin seeking from.
        .SetInputFiles(mediaFile)                //Set input files
        .FramesNumber(1)                         //Number of video frames to output
        .Format(FileFormatType.JPG)              //Force input or output file format.
        .SetOutputArguments(@"pathToInputFile"); //Setting Output Arguments

var result = await videoFileProcessor.ExecuteAsync(settings, new CancellationToken());
```

### Important Note When Processing MP4 Files!
When processing MP4 video files into a video stream or byte set, use this method.
```csharp
public async Task<MemoryStream?> SetStartMoovAsync(MediaFile file, string? outputFile = null, CancellationToken cancellationToken = default)
```

This method moves the MOOV atom of the MP4 format to the beginning because FFmpeg must know how to process this file when reading files from a stream. 
The MP4 file processing information is usually at the end and FFmpeg cannot process it as a stream.
When moving a MOOV atom, ffmpeg cannot retrieve a file from a stream and output the result as a stream.
To move an atom, it is necessary to have a file physically in the directory, the result of processing must also be written to the directory. If the MP4 file is in your stream form and needs to shift the MOOV atom, and get the result in wanting necessarily in the video stream.
Then this method will create a physical file from your input stream and pass it to FFmpeg for processing and convert the result from the file to a stream and return this stream.
All intermediate files created will then be deleted.

The current version of the library has already implemented some options for processing video files using ffmpeg:

- Extract frame from video
- Trim video
- Convert video to image set frame by frame
- Convert images to video
- Extract audio track from video file
- Convert to another format
- Add Watermark
- Remove sound from video
- Add audio file to video file
- Convert video to Gif animation
- Compress video
- Compress image
- Combine a set of video files into a single video file
- Add subtitles
- Get detailed information on video file metadata

These functions I tested with some of the most requested formats mp4 avi png jpg bmp wav and mpeg
Other formats may require additional settings.
In the future, I'm going to develop the library to possibly install settings under the hood for any format.

### Example "Extract frame from video"
Below is an example of using frame extraction from a video file at a certain timing, provided that the file exists PHYSICALLY in the directory
```csharp
 var videoFileProcessor = new VideoFileProcessor();
 //Test block with physical paths to input and output files
 await videoFileProcessor.ExtractFrameFromVideoAsync(TimeSpan.FromMilliseconds(47500),
                                                     new MediaFile(@"C:\inputFile.avi"),
                                                     @"C:\resultPath.jpg",
                                                     FileFormatType.JPG);
```

Below is an example of using a frame extraction from a video file at a certain timing, provided that we have a file in the video stream
```csharp
//Block for testing file processing as streams without specifying physical paths
await using var stream = new FileStream(@"C:\inputFile.avi", FileMode.Open);
var resultStream = await videoProcessor.GetFrameFromVideoAsStreamAsync(TimeSpan.FromMilliseconds(47500), new MediaFile(stream), null, FileFormatType.JPG);
await using (var output = new FileStream(@"C:\resultPath.jpg", FileMode.Create))
     resultStream.WriteTo(output);
```

All other methods work exactly the same. You can transfer files to the process in any form and receive them in any video.

# ImageMagick instruction

For image processing, ImageMagick uses its class ```ImageFileProcessor``` and its executable convert.exe

To load its executable, you can call the following code
```csharp
await ImageFileProcessor.DownloadExecutableFiles();
```
This code will download the executable file to the root directory from the address https://imagemagick.org/archive/binaries/ImageMagick-7.1.0-61-portable-Q16-x64.zip

All instructions that apply to ffmpeg also apply to ImageMagick.
The ImageMagick handler is the ```ImageFileProcessor``` class
```csharp
var i = new ImageFileProcessor();
var j = new ImageFileProcessor("pathToConvert.exe");
```

The current version of the library already implements some options for image processing using ImageMagick:

-Compress image
-Convert image to another format
-Resize image
-Convert a set of images to Gif animation

### An example of image compression in three options (directory path, stream, byte array)
```csharp
//Test block with physical paths to input and output files
await processor.CompressImageAsync(new MediaFile(_image), ImageFormat.JPG, 60, FilterType.Lanczos, "x1080", @"С:\result.jpg", ImageFormat.JPG);

//Block for testing file processing as streams without specifying physical paths
await using var stream = new FileStream(_image, FileMode.Open);
var resultStream = await processor.CompressImageAsStreamAsync(new MediaFile(stream), ImageFormat.JPG, 60, FilterType.Lanczos, "x1080", ImageFormat.JPG);
await using (var output = new FileStream(@"С:\result.jpg", FileMode.Create))
     resultStream.WriteTo(output);

//Block for testing file processing as bytes without specifying physical paths
var bytes = await File.ReadAllBytesAsync(_image);
var resultBytes = await processor.CompressImageAsBytesAsync(new MediaFile(bytes), ImageFormat.JPG, 60, FilterType.Lanczos, "x1080", ImageFormat.JPG);
await using (var output = new FileStream(@"С:\result.jpg", FileMode.Create))
    output.Write(resultBytes);
```

# Pandoc instruction
The pandoc.exe process, its processor ```DocumentFileProcessor```, is used to process documents.

In the current version of the library, some options for processing documents using pandoc are already implemented:

-convert .docx file to .pdf
```csharp
var file = new MediaFile(@"C:\inputFile.docx");
var processor = new DocumentFileProcessor();
await processor.ConvertDocxToPdf(file, "test.pdf");
```

# Useful Features

## MultiStream
The ```MultiStream``` class is designed to work with a set of streams as a single entity.

If you need to pass multiple files to a single process input stream, the ```MultiStream``` class will help you.
For example, when ffmpeg needs to create a video from a set of images, and these images should be passed as a single stream to the input stream of the process.
```csharp
var stream = new MultiStream();
stream.AddStream(new FileStream(@"С:\inputfile1.jpg", FileMode.Open, FileAccess.Read, FileShare.Read));
stream.AddStream(new FileStream(@"С:\inputfile2.jpg", FileMode.Open, FileAccess.Read, FileShare.Read));
stream.AddStream(new FileStream(@"С:\inputfile3.jpg", FileMode.Open, FileAccess.Read, FileShare.Read));
stream.AddStream(new FileStream(@"С:\inputfile4.jpg", FileMode.Open, FileAccess.Read, FileShare.Read));
stream.AddStream(new FileStream(@"С:\inputfile5.jpg", FileMode.Open, FileAccess.Read, FileShare.Read));
```
Here we create an instance of the ```MultiStream``` class and, through the ```AddStream``` method, add several streams with different files to this one.
Now we can transfer these streams to the process in one stream in one input stream

### Example of using MultiStream
```csharp
var stream = new MultiStream();
var files = new List<string>();
for (var i = 1; i <= 1000; i++)
{
    files.Add($@"C:\image{i:000}.jpg");
}
foreach (var file in files)
{
    stream.AddStream(new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read));
}

//Block for testing file processing as streams without specifying physical paths
stream.Seek(0, SeekOrigin.Begin);
var resultStream = await videoProcessor.ConvertImagesToVideoAsync(new MediaFile(stream), 24, "yuv420p", FileFormatType.AVI);
await using (var output = new FileStream(@"C:\mfptest\results\ConvertImagesToVideoTest\resultStream.avi", FileMode.Create))
{
   resultStream.WriteTo(output);
}
```
We collect a thousand images into one ```MultiStream``` and pass it to the process
The ```MultiStream``` class has a ```ReadAsDataArray``` method to get the contained streams as arrays of bytes,
and ```ReadAsStreamArray``` to get the contained streams as an array of streams.

### On-the-fly stream decoding
When we use the ffmpeg function to split a video file frame by frame into images, it creates a set of images for us in the specified output directory.

But what if we need to get its result to the directory and to the output stream. In this case, it will write all the images obtained from the video file into a single output stream.
As a result, we will have many files in one stream. How can we get these files?
This is where the ```GetMultiStreamBySignature(this Stream stream, byte[] fileSignature)``` extension method comes to the rescue.
This should be called on the stream to be decoded and passed to this method as an argument - the signature of the files to be extracted.
The result of this method will be a ```MultiStream``` containing an array of file poices. 1 stream for 1 file.
And already using its methods ```ReadAsDataArray``` or ```ReadAsStreamArray``` we can get these files as an array of bytes or streams.

### To learn more about the decoding process, I advise you to study the source code.
An illustrative example of stream decoding:
```csharp
//Block for testing file processing as streams without specifying physical paths
await using var stream = new FileStream(_video1, FileMode.Open);
var resultMultiStream = await videoProcessor.ConvertVideoToImagesAsync(new MediaFile(stream), null,  FileFormatType.JPG);
var count = 1;
var data = resultMultiStream.ReadAsDataArray();

foreach (var bytes in data)
{
   await using (var output = new FileStream(@$"C:\result{count++}.jpg", FileMode.Create))
       output.Write(bytes, 0, bytes.Length);
}
```
To get the signature of a particular file format, there is an extension method
```csharp
public static byte[] GetSignature(this FileFormatType outputFormatType)
```

If this extension method does not support defining the signature of the format you need, then let me know and I will fix the defect as quickly as possible.

## FileDownloadProcessor

If you need to download a file, you can use the ```DownloadFile``` static method of the ```FileDownloadProcessor``` class.
This method uses not the outdated WebClient for downloading, but HttpClient and allows you to track the progress of the download as a percentage.

## ZipFileProcessor

The ```ZipFileProcessor``` class is introduced for working with zip archives.

Applications for unpacking downloaded ffmpeg archive and extracting executable files
```csharp
// Open an existing zip file for reading
using(var zip = ZipFileProcessor.Open(fileName, FileAccess.Read))
{
    // Read the central directory collection
    var dir = zip.ReadCentralDir();

    // Look for the desired file
    foreach (var entry in dir)
    {
        if (Path.GetFileName(entry.FilenameInZip) == "ffmpeg.exe")
        {
            zip.ExtractFile(entry, $@"ffmpeg.exe"); // File found, extract it
        }

        if (Path.GetFileName(entry.FilenameInZip) == "ffmpeg.exe")
        {
            zip.ExtractFile(entry, $@"ffprobe.exe"); // File found, extract it
        }
    }
}
```
## MediaFileProcess

Perhaps the main class of this library is the class ```MediaFileProcess```.
It is a universal wrapper for executable processes.

When instantiating it, you must give it the path/name of the executable process, process arguments, classes from  ```BaseProcessingSettings```, input streams, and names of input named pipes.
### Note on input streams and named pipes:
If a process needs to pass multiple threads to different input arguments,
then you should specify the names of the named pipes in the input arguments and pass these names and input streams to the corresponding arguments of the ```MediaFileProcess``` constructor.
This is necessary because in the case of passing to different streams in different input arguments, named pipes are used.
The configuration of the running process itself should be done in the class that derives from ```BaseProcessingSettings```.

```csharp
var inputStreamFile = @"C:\inputFile.txt".ToFileStream();

var settings = new FFmpegProcessingSettings
{
    CreateNoWindow = true,
    UseShellExecute = false,
    EnableRaisingEvents = false,
    WindowStyle = ProcessWindowStyle.Normal,
    ProcessOnExitedHandler = null,
    OutputDataReceivedEventHandler = null,
    ErrorDataReceivedHandler = null
};

var process = new MediaFileProcess("program.exe", "-arg1 value1 -arg2 value2 -arg3 value3", settings, new Stream[] { inputStreamFile } );

var result = await process.ExecuteAsync(new CancellationToken());
```
## FileDataExtensions

The FileDataExtensions file contains a class with the same name, which consists of several extension methods aimed at helping to work with file and byte array data.
Here are some of the methods presented in that class:

* `ToFileStream` - these methods are used to convert a file name to a FileStream with an optional FileMode.

* `ToMemoryStream` - this method converts a byte array to a MemoryStream.

* `ToBytes`- this method converts a file name to a byte array.

* `ToFile` - these methods take either a byte array or a stream and write it to a file with the given name.

* `GetFileFormatType` - this method returns the format type of the file.

* `GetFilesFromByteArray` and variations - these methods are used to get a MultiStream from a byte array.

## FilesSignatureExtensions

The FilesSignatureExtensions file in your project contains a class FilesSignatureExtensions which provides a set of extension methods to interact with various file types and their associated signatures.
These methods are primarily used to detect and handle different types of media files depending on their signature.
Here are some functionalities provided by the extension methods:

* Properties representing byte arrays for various media file signatures like `JPG, PNG, ICO, TIFF, MP4, AVI, GIF, VOB, MP3, WAV`, etc.

* Methods like `FileFormatType GetFormat(this byte[] signature)` and `FileFormatType GetFormat(this ReadOnlySpan<byte> signature)` which return the media file format type based on the byte signature.

* A collection of boolean methods like `IsTS(this byte[] buffer), IsJPEG(this byte[] buffer), IsPNG(this byte[] buffer), IsMOV(this byte[] buffer), etc.,` which determine the type of media file from a byte buffer.

* Methods like `bool CompareByteArrays(byte[] arr1, byte[] arr2)` and `bool CompareByteArrays(byte[] arr1, byte[] arr2, int position)` which compares byte arrays.

* Methods like `string? GetFileFormat(Stream stream)` and `string? GetFileFormat(byte[] fileData)` which return file format based on stream or byte array.

## StreamDecodeExtensions

The StreamDecodeExtensions file contains a class StreamDecodeExtensions that extends certain functionalities to help with stream decoding and handling such as searching for file signatures and generating multi streams based on these signatures.
Here are the methods offered by this class for these functionalities:

* `byte[] ConcatByteArrays(ReadOnlySpan<byte> array1, ReadOnlySpan<byte> array2)`: This method concatenates two byte arrays.

* `(int?, bool) SearchFileSignature(Span<byte> bytes, int startIndex, int endIndex)`: This method is designed to search for a file signature in the span of bytes within specified start and end indices.

* `bool SignaturePositionIgnore(ReadOnlySpan<byte> signature, ref int signatureStartPos)`: This method examines if the signature position should be ignored.

* `List<byte[]>? GetListOfSignature(Span<byte> data)`: This method retrieves a list of signatures from the provided data span.

* `MultiStream GetMultiStreamBySignature(Stream stream, List<byte[]> fileSignature)`: This method generates a MultiStream based on a stream and a list of file signatures.

## FFmpegExtensions

The FFmpegExtensions file contains a static class FFmpegExtensions which provides utility methods for handling FFmpeg-related operations.
Currently, it contains a single extension method:

* `string ToFfmpegDuration(this TimeSpan duration)`: this extension method on TimeSpan instances is used to convert a TimeSpan duration to a string formatted for FFmpeg usage. This method handles both positive and negative durations, returning the appropriate FFmpeg-formatted string representation of the duration.

_**P.S. You can use test files with different formats from the ```testFiles``` folder**_

# Special thanks to

* [JetBrains](https://www.jetbrains.com/), for the Rider IDE opensource license.
* [Syntevo](https://www.syntevo.com/), for the SmartGit opensource license.
* [PostSharp](https://www.postsharp.net/), for the Metalama opensource license.