using System.Runtime.InteropServices;
using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Enums;
namespace MediaFileProcessor.Extensions;

/// <summary>
/// A class that contains extension methods for working with file data
/// </summary>
public static class FileDataExtensions
{
    /// <summary>
    /// Checks if the specified string value exists in the specified enum type T.
    /// </summary>
    /// <typeparam name="T">The enum type to search for the value in.</typeparam>
    /// <param name="value">The string value to search for.</param>
    /// <returns>True if the value exists in the enum type T, false otherwise.</returns>
    public static bool ExistsInEnum<T>(this string value) where T : Enum
    {
        return Enum.GetNames(typeof(T)).Any(i => "." + i.Replace("_", "").ToLower() == value.ToLower());
    }

    /// <summary>
    /// Converts the specified file name to a stream.
    /// </summary>
    /// <param name="fileName">The file name to convert.</param>
    /// <param name="mode">The mode to open the file stream with. Defaults to FileMode.Open.</param>
    /// <returns>A FileStream representing the specified file name.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the file specified in <paramref name="fileName"/> does not exist.</exception>
    public static FileStream ToFileStream(this string fileName, FileMode mode = FileMode.Open)
    {
        if(!File.Exists(fileName))
            throw new FileNotFoundException($"File not found: {fileName}");

        return new FileStream(fileName, mode);
    }

    /// <summary>
    /// Gets the extension of the specified file name.
    /// </summary>
    /// <param name="fileName">The file name to get the extension for.</param>
    /// <returns>The extension of the specified file name.</returns>
    public static string GetExtension(this string fileName)
    {
        return Path.GetExtension(fileName).Replace(".", "");
    }

    /// <summary>
    /// Converts the specified file name to an array of bytes.
    /// </summary>
    /// <param name="fileName">The file name to convert.</param>
    /// <returns>An array of bytes representing the contents of the file specified in <paramref name="fileName"/>.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the file specified in <paramref name="fileName"/> does not exist.</exception>
    public static byte[] ToBytes(this string fileName)
    {
        if(!File.Exists(fileName))
            throw new FileNotFoundException($"File not found: {fileName}");

        return File.ReadAllBytes(fileName);
    }

    /// <summary>
    /// Converts the specified byte array to a memory stream.
    /// </summary>
    /// <param name="bytes">The byte array to convert.</param>
    /// <returns>A MemoryStream representing the specified byte array.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="bytes"/> is an empty array.</exception>
    public static MemoryStream ToFileStream(this byte[] bytes)
    {
        if(bytes.Length is 0)
            throw new ArgumentException("Byte array is empty");

        return new MemoryStream(bytes);
    }

    /// <summary>
    /// Writes the specified byte array to a file.
    /// </summary>
    /// <param name="bytes">The byte array to write to a file.</param>
    /// <param name="fileName">The name of the file to create or overwrite.</param>
    public static void ToFile(this byte[] bytes, string fileName)
    {
        using (var output = new FileStream(fileName, FileMode.Create))
            output.Write(bytes, 0, bytes.Length);
    }

    /// <summary>
    /// Converts a pipe name to the appropriate pipe directory format for the current operating system.
    /// </summary>
    /// <param name="pipeName">The name of the pipe</param>
    /// <returns>A string representing the pipe directory</returns>
    public static string ToPipeDir(this string pipeName)
    {
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return $@"\\.\pipe\{pipeName}";

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return pipeName;

        // Throw an exception if the operating system cannot be recognized
        throw new Exception("Operating System not recognized");
    }

    /// <summary>
    /// Writes the contents of the specified stream to a file.
    /// </summary>
    /// <param name="stream">The stream to write to a file.</param>
    /// <param name="fileName">The name of the file to create or overwrite.</param>
    public static void ToFile(this Stream stream, string fileName)
    {
        using (var output = new FileStream(fileName, FileMode.Create))
            stream.CopyTo(output);
    }

    /// <summary>
    /// Gets the format type of a file based on its file extension.
    /// </summary>
    /// <param name="fileName">The name of the file to get the format type of.</param>
    /// <returns>The format type of the file.</returns>
    /// <exception cref="Exception">Thrown if the file extension of <paramref name="fileName"/> could not be recognized.</exception>
    public static FileFormatType GetFileFormatType(this string fileName)
    {
        var ext = Path.GetExtension(fileName);

        if (Enum.TryParse(ext.ToUpper().Replace(".", ""), out FileFormatType output))
            return output;

        throw new Exception("The file extension could not be recognized");
    }

    /// <summary>
    /// Concatenates multiple arrays of bytes into a single array of bytes.
    /// </summary>
    /// <param name="onlyNotDefaultArrays">A flag indicating whether only arrays that contain non-zero bytes should be concatenated.</param>
    /// <param name="arrays">The arrays of bytes to concatenate.</param>
    /// <returns>A single array of bytes that is the result of concatenating the input arrays.</returns>
    public static byte[] ConcatByteArrays(bool onlyNotDefaultArrays, params byte[][] arrays)
    {
        if(onlyNotDefaultArrays)
            arrays = arrays.Where(x => x.Any(y => y != 0))
                           .ToArray();

        var z = new byte[arrays.Sum(x => x.Length)];

        var lengthSum = 0;

        foreach (var array in arrays)
        {
            Buffer.BlockCopy(array, 0, z, lengthSum, array.Length * sizeof(byte));
            lengthSum += array.Length * sizeof(byte);
        }

        return z;
    }

    /// <summary>
    /// Searches for the occurrence of a signature within a byte array.
    /// </summary>
    /// <param name="bytes">The array to search within.</param>
    /// <param name="signatures">The signature to search for.</param>
    /// <returns>A tuple representing the start position of the signature within the array and a flag indicating whether the signature was found in its entirety.</returns>
    private static (int?, bool) SearchFileSignature(this byte[] bytes, List<byte[]> signatures)
    {
        if (signatures.Count == 0)
            return (null, false);

        foreach (var signature in signatures)
        {
            var signatureLength = signature.Length;

            if (signatureLength == 0)
                continue;

            var signatureStartPos = 0;
            var startFlag = -1;

            for (var i = 0; i < bytes.Length; i++)
            {
                if ((bytes[i] == signature[signatureStartPos] || (0 == signature[signatureStartPos] && signatureStartPos is 4 or 5)) && startFlag is -1)
                {
                    startFlag = i;
                    signatureStartPos++;

                    continue;
                }

                if(bytes[i] != signature[signatureStartPos] && (signature[signatureStartPos] != 0 && signatureStartPos != 4 || signatureStartPos != 5))
                {
                    startFlag = -1;
                    signatureStartPos = default;

                    continue;
                }

                if(startFlag is not -1 && signatureStartPos == signatureLength - 1)
                    return (startFlag, true);

                signatureStartPos++;
            }

            if(startFlag is not -1)
                return (startFlag, false);
        }

        return (null, false);
    }

    /// <summary>
    /// Gets the list of signatures from the given data.
    /// </summary>
    /// <param name="data">The byte array to search for the signatures.</param>
    /// <param name="signature">The byte array representing the signature to search for.</param>
    /// <returns>A tuple containing a list of byte arrays, a byte array and a byte array.
    /// The list of byte arrays represent the found signatures, the byte array represents the data without the found signatures and the byte array represents previous data (if any).</returns>
    private static (List<byte[]>?, byte[], byte[]?) GetListOfSignature(this byte[] data, List<byte[]> signature)
    {
        // Check if the data is shorter than the signature, in which case there's no match
        if (data.Length < signature.First().Length)
            return (null, data, null);

        var isFullEndSignature = false;

        var listFiles = new List<byte[]>();

        var previosdatas = Array.Empty<byte>();

        do
        {
            // Search for the start of the signature in the data
            (var startFilePos, var isFullStartSignature) = SearchFileSignature(data, signature);

            // If a start signature is found and it's not at position 0
            if(startFilePos.HasValue && startFilePos.Value != 0)
            {
                previosdatas = data[..startFilePos.Value];
                data = data[startFilePos.Value..];
                (startFilePos, isFullStartSignature) = SearchFileSignature(data, signature);
            }

            // If a full start signature is found
            if(isFullStartSignature)
            {
                // Search for the end of the signature in the data
                (var endFilePos, isFullEndSignature) = SearchFileSignature(data[(startFilePos!.Value + signature.First().Length)..], signature);

                // If a full end signature is found
                if(isFullEndSignature)
                {
                    var fileBytes = data[startFilePos.Value..(endFilePos!.Value + signature.First().Length)];
                    listFiles.Add(fileBytes);

                    data = data[(endFilePos.Value + signature.First().Length)..];
                }
            }
        } while (isFullEndSignature);

        // Set previosdatas to null if there's no previous data
        if(previosdatas.Length == 0)
            previosdatas = null;

        // Return the found signatures, data without the signatures and previous data (if any)
        return listFiles.Count == 0 ? (null, data, previosdatas) : (listFiles, data, previosdatas);
    }

    /// <summary>
    /// Extension method to get a `MultiStream` from a `Stream` object by a given file signature.
    /// </summary>
    /// <param name="stream">The input `Stream` object to extract the `MultiStream` from.</param>
    /// <param name="fileSignature">The signature to identify the start of a new file in the input `Stream`.</param>
    /// <returns>A `MultiStream` containing multiple `Stream` objects extracted from the input `Stream`.
    /// Each `Stream` object represents a file extracted based on the given file signature.</returns>
    public static MultiStream GetMultiStreamBySignature(this Stream stream, List<byte[]> fileSignature)
    {
        // buffer to store the read data from the input stream
        var buffer = new byte[16 * 1024];

        // a buffer to store data that could potentially be part of the start of a new file
        byte[]? newFileStartBuffer = null;

        // a memory stream to store data until a new file signature is found
        var ms = new MemoryStream();

        // the `MultiStream` object to return
        var multiStream = new MultiStream();

        // the number of bytes read from the input stream
        int nread;

        // loop to read data from the input stream
        while ((nread = stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            if(multiStream.Count == 364)
            {
                Console.WriteLine("test");
            }

            // the data read from the input stream, concatenated with the new file start buffer if it exists
            var array = newFileStartBuffer != null
                ? ConcatByteArrays(false, newFileStartBuffer, buffer[..nread])
                : buffer[..nread];

            // the list of files found in the current data, as well as the remaining data
            var listFiles = array.GetListOfSignature(fileSignature);

            // check if a new file has started in the current data
            var newFileStarted = array.Length < fileSignature.First().Length
                ? array.SearchFileSignature(fileSignature)
                : array[..fileSignature.First().Length].SearchFileSignature(fileSignature);

            // if a new file signature has been found, add the data stored in the memory stream to the `MultiStream`
            // and reset the memory stream
            if(listFiles.Item3 == null && newFileStarted.Item2 && newFileStarted.Item1!.Value == 0 && ms.Length != 0)
            {
                multiStream.AddStream(new MemoryStream(ms.ToArray()));
                ms = new MemoryStream();
            }

            // reset the new file start buffer
            newFileStartBuffer = null;

            // if there is remaining data, add it to the memory stream or start a new memory stream
            if (listFiles.Item3 != null)
            {
                // check if the remaining data starts with a file signature
                var isSig = listFiles.Item2.SearchFileSignature(fileSignature);

                // if the memory stream contains data and the remaining data starts with a file signature,
                // add the concatenated data to the `MultiStream` and reset the memory stream
                if(ms.Length != 0 && isSig.Item2)
                {
                    var data = ConcatByteArrays(false, ms.ToArray(), listFiles.Item3);
                    multiStream.AddStream(new MemoryStream(data));
                    ms = new MemoryStream();
                }

                // If ms is not empty and the current buffer does not start with a file signature, write to ms
                if(ms.Length != 0 && !isSig.Item2)
                    ms.Write(listFiles.Item3, 0, listFiles.Item3.Length);

                if(isSig is { Item1: { }, Item2: false })
                {
                    newFileStartBuffer = listFiles.Item2;

                    continue;
                }
            }

            // This code block processes a list of files. It checks if `listFiles.Item1` is not null and adds its contents to a `multiStream` object.
            // Then, it searches for the `fileSignature` within `listFiles.Item2` and retrieves its starting position and a flag indicating if the signature is found completely.
            // If the signature is found completely, the code checks if it starts at the beginning of the memory stream and adds the remaining data to `multiStream`.
            // It then checks if there is another signature following the first one and adds the data in between the two signatures to `ms`.
            // If the signature is not found, the entire `listFiles.Item2` is added to `ms`.

            // Check if listFiles.Item1 is not null and add each item as a memory stream to multiStream
            if (listFiles.Item1 is not null)
                foreach (var i in listFiles.Item1)
                    multiStream.AddStream(new MemoryStream(i));

            // Search for file signature in listFiles.Item2 and store start position and full signature found or not in variables
            (var startFilePos, var isFullSignature) = listFiles.Item2.SearchFileSignature(fileSignature);

            // If a full signature is found
            if(isFullSignature)
            {
                // If start position is 0 and ms is not empty
                if(startFilePos!.Value == 0 && ms.Length != 0)
                {
                    // Store data in array and clear ms
                    var data = ms.ToArray();
                    ms = new MemoryStream();

                    // Add data to multiStream as a memory stream
                    multiStream.AddStream(new MemoryStream(data));
                }

                // Check for a new file start buffer
                var checkNewFileStartBuffer = listFiles.Item2[fileSignature.First().Length..].SearchFileSignature(fileSignature);

                // If start position is 0 and a new file start buffer is found
                if(startFilePos.Value == 0 && checkNewFileStartBuffer.Item1 != null)
                {
                    // Write the data up to the new file signature to the memory stream.
                    var data = listFiles.Item2[..(checkNewFileStartBuffer.Item1.Value + fileSignature.First().Length)];
                    ms.Write(data, 0, data.Length);

                    // Update newFileStartBuffer
                    newFileStartBuffer = listFiles.Item2[(checkNewFileStartBuffer.Item1.Value + fileSignature.First().Length)..];

                    continue;
                }

                ms.Write(listFiles.Item2[startFilePos.Value..],
                         0,
                         listFiles.Item2[startFilePos.Value..]
                                  .Length);

                // Continue to the next iteration
                continue;
            }

            // If no start position is found
            // Write data to ms starting from the start position
            if(startFilePos == null)
                ms.Write(listFiles.Item2, 0, listFiles.Item2.Length);
        }

        // Check if the MemoryStream is not empty
        if(ms.Length != 0)
        {
            // Create an array of the data in the MemoryStream
            var data = ms.ToArray();

            // Add the data to the MultiStream
            multiStream.AddStream(new MemoryStream(data));
        }

        // Dispose and close the MemoryStream
        ms.Dispose();
        ms.Close();

        //return result
        return multiStream;
    }

    /// <summary>
    /// A method for extracting multiple streams from a larger stream using a specified file signature.
    /// </summary>
    /// <param name="stream">The larger stream from which to extract smaller streams.</param>
    /// <param name="fileSignature">The byte array used to identify the start of a new stream in the larger stream.</param>
    /// <param name="action">The action to perform on each extracted stream represented as a byte array.</param>
    public static void GetMultiStreamBySignature(this Stream stream, List<byte[]> fileSignature, Action<byte[]> action)
    {
        var buffer = new byte[16 * 1024];
        byte[]? newFileStartBuffer = null;

        var ms = new MemoryStream();

        int nread;

        while ((nread = stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            var array = newFileStartBuffer != null
                ? ConcatByteArrays(false, newFileStartBuffer, buffer[..nread])
                : buffer[..nread];

            var listFiles = array.GetListOfSignature(fileSignature);

            var newFileStarted = array.Length < fileSignature.First().Length
                ? array.SearchFileSignature(fileSignature)
                : array[..fileSignature.First().Length].SearchFileSignature(fileSignature);

            if(listFiles.Item3 == null && newFileStarted.Item2 && newFileStarted.Item1!.Value == 0 && ms.Length != 0)
            {
                action(ms.ToArray());
                ms = new MemoryStream();
            }

            newFileStartBuffer = null;

            if (listFiles.Item3 != null)
            {
                var isSig = listFiles.Item2.SearchFileSignature(fileSignature);

                if(ms.Length != 0 && isSig.Item2)
                {
                    var data = ConcatByteArrays(false, ms.ToArray(), listFiles.Item3);
                    action(data);
                    ms = new MemoryStream();
                }

                if(ms.Length != 0 && !isSig.Item2)
                    ms.Write(listFiles.Item3, 0, listFiles.Item3.Length);

                if(isSig is { Item1: { }, Item2: false })
                {
                    newFileStartBuffer = listFiles.Item2;

                    continue;
                }
            }

            if (listFiles.Item1 is not null)
                foreach (var i in listFiles.Item1)
                    action(i);

            (var startFilePos, var isFullSignature) = listFiles.Item2.SearchFileSignature(fileSignature);

            if(isFullSignature)
            {
                if(startFilePos!.Value == 0 && ms.Length != 0)
                {
                    var data = ms.ToArray();
                    ms = new MemoryStream();
                    action(data);
                }

                var checkNewFileStartBuffer = listFiles.Item2[fileSignature.First().Length..].SearchFileSignature(fileSignature);

                if(startFilePos.Value == 0 && checkNewFileStartBuffer.Item1 != null)
                {
                    var test = listFiles.Item2[..(checkNewFileStartBuffer.Item1.Value + fileSignature.First().Length)];
                    ms.Write(test, 0, test.Length);
                    newFileStartBuffer = listFiles.Item2[(checkNewFileStartBuffer.Item1.Value + fileSignature.First().Length)..];

                    continue;
                }

                ms.Write(listFiles.Item2[startFilePos.Value..],
                         0,
                         listFiles.Item2[startFilePos.Value..]
                                  .Length);

                continue;
            }

            if(startFilePos == null)
                ms.Write(listFiles.Item2, 0, listFiles.Item2.Length);
        }

        if(ms.Length != 0)
        {
            var data = ms.ToArray();
            action(data);
        }

        ms.Dispose();
        ms.Close();
    }
}