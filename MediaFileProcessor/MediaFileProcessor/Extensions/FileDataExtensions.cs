using System.Globalization;
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
    public static bool ExistsInEnum<T>(this string value)
        where T : Enum
    {
        return Array.Exists(Enum.GetNames(typeof(T)),
                            @enum => "." + @enum.Replace("_", "", StringComparison.InvariantCultureIgnoreCase).ToLowerInvariant() == value.ToLowerInvariant());
    }

    /// <summary>
    /// Converts the specified file name to a stream.
    /// </summary>
    /// <param name="fileName">The file name to convert.</param>
    /// <returns>A FileStream representing the specified file name.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the file specified in <paramref name="fileName"/> does not exist.</exception>
    public static FileStream ToFileStream(this string fileName)
    {
        if (!File.Exists(fileName))
            throw new FileNotFoundException($"File not found: {fileName}");

        return new FileStream(fileName, FileMode.Open);
    }

    /// <summary>
    /// Converts the specified file name to a stream.
    /// </summary>
    /// <param name="fileName">The file name to convert.</param>
    /// <param name="mode">The mode to open the file stream with. Defaults to FileMode.Open.</param>
    /// <returns>A FileStream representing the specified file name.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the file specified in <paramref name="fileName"/> does not exist.</exception>
    public static FileStream ToFileStream(this string fileName, FileMode mode)
    {
        if (!File.Exists(fileName))
            throw new FileNotFoundException($"File not found: {fileName}");

        return new FileStream(fileName, mode);
    }

    /// <summary>
    /// Converts the specified byte array to a memory stream.
    /// </summary>
    /// <param name="bytes">The byte array to convert.</param>
    /// <returns>A MemoryStream representing the specified byte array.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="bytes"/> is an empty array.</exception>
    public static MemoryStream ToMemoryStream(this byte[] bytes)
    {
        if (bytes.Length is 0)
            throw new ArgumentException("Byte array is empty");

        return new MemoryStream(bytes);
    }

    /// <summary>
    /// Gets the extension of the specified file name.
    /// </summary>
    /// <param name="fileName">The file name to get the extension for.</param>
    /// <returns>The extension of the specified file name.</returns>
    public static string? GetExtension(this string fileName)
    {
        return Path.GetExtension(fileName).Replace(".", "", StringComparison.InvariantCultureIgnoreCase);
    }

    /// <summary>
    /// Converts the specified file name to an array of bytes.
    /// </summary>
    /// <param name="fileName">The file name to convert.</param>
    /// <returns>An array of bytes representing the contents of the file specified in <paramref name="fileName"/>.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the file specified in <paramref name="fileName"/> does not exist.</exception>
    public static byte[] ToBytes(this string fileName)
    {
        if (!File.Exists(fileName))
            throw new FileNotFoundException($"File not found: {fileName}");

        return File.ReadAllBytes(fileName);
    }

    /// <summary>
    /// Writes the specified byte array to a file.
    /// </summary>
    /// <param name="bytes">The byte array to write to a file.</param>
    /// <param name="fileName">The name of the file to create or overwrite.</param>
    public static void ToFile(this byte[] bytes, string fileName)
    {
        using var output = new FileStream(fileName, FileMode.Create);

        output.Write(bytes, 0, bytes.Length);
    }

    /// <summary>
    /// Writes the contents of the specified stream to a file.
    /// </summary>
    /// <param name="stream">The stream to write to a file.</param>
    /// <param name="fileName">The name of the file to create or overwrite.</param>
    public static void ToFile(this Stream stream, string fileName)
    {
        using var output = new FileStream(fileName, FileMode.Create);

        stream.CopyTo(output);
    }

    /// <summary>
    /// Converts a pipe name to the appropriate pipe directory format for the current operating system.
    /// </summary>
    /// <param name="pipeName">The name of the pipe</param>
    /// <returns>A string representing the pipe directory</returns>
    public static string ToPipeDir(this string pipeName)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return $@"\\.\pipe\{pipeName}";

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return pipeName;

        // Throw an exception if the operating system cannot be recognized
        throw new NotSupportedException("Operating System not supported");
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

        if (char.IsDigit(ext[1]))
            ext = "_" + ext;

        if (Enum.TryParse(ext.ToUpper(CultureInfo.InvariantCulture)
                             .Replace(".", "", StringComparison.InvariantCultureIgnoreCase),
                          out FileFormatType output))
            return output;

        throw new InvalidDataException("The file extension could not be recognized");
    }

    /// <summary>
    /// Concatenates multiple arrays of bytes into a single array of bytes.
    /// </summary>
    /// <param name="onlyNotDefaultArrays">A flag indicating whether only arrays that contain non-zero bytes should be concatenated.</param>
    /// <param name="arrays">The arrays of bytes to concatenate.</param>
    /// <returns>A single array of bytes that is the result of concatenating the input arrays.</returns>
    public static byte[] ConcatByteArrays(bool onlyNotDefaultArrays, params byte[][] arrays)
    {
        if (onlyNotDefaultArrays)
            arrays = arrays.Where(array => Array.Exists(array, y => y != 0))
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
    /// Concatenates two byte arrays into one.
    /// </summary>
    /// <param name="array1">The first byte array.</param>
    /// <param name="array2">The second byte array.</param>
    /// <returns>A new byte array containing elements from both input arrays.</returns>
    public static byte[] ConcatByteArrays(ReadOnlySpan<byte> array1, ReadOnlySpan<byte> array2)
    {
        // Calculate the total size for the resulting array
        var totalSize = array1.Length + array2.Length;

        // Create the resulting array
        var result = new byte[totalSize];

        // Copy the data from the first array
        array1.CopyTo(result);

        // Copy the data from the second array, starting from the position after the last element of the first array
        array2.CopyTo(result.AsSpan(array1.Length));

        // Return the resulting array
        return result;
    }

    /// <summary>
    /// Concatenates two byte arrays into one.
    /// </summary>
    /// <param name="array1">The first byte array.</param>
    /// <param name="array2">The second byte array.</param>
    /// <returns>A new byte array containing elements from both input arrays.</returns>
    public static byte[] ConcatByteArrays(byte[] array1, byte[] array2)
    {
        // Calculate the total size for the resulting array
        var totalSize = array1.Length + array2.Length;

        // Create the resulting array
        var result = new byte[totalSize];

        // Copy the data from the first array
        array1.CopyTo((Span<byte>)result);

        // Copy the data from the second array, starting from the position after the last element of the first array
        array2.CopyTo(result.AsSpan(array1.Length));

        // Return the resulting array
        return result;
    }

    /// <summary>
    /// Finds the first occurrence of any given byte array within a span of bytes.
    /// </summary>
    /// <param name="span">The source span of bytes in which to perform the search.</param>
    /// <param name="byteArrays">The collection of byte arrays to search for within the source span.</param>
    /// <returns>Returns the first matched byte array found in the source span. Returns null if no array is found.</returns>
    public static byte[]? FindFirstOccurrence(Span<byte> span, IEnumerable<byte[]> byteArrays)
    {
        // Variable to keep track of the minimum position at which an array was found
        var minLength = int.MaxValue;

        // The array that was found
        byte[]? matchedArray = null;

        // Iterate over all the byte arrays to search for
        foreach (var byteArray in byteArrays)
        {
            // Temporary variable to store the position at which the array is found
            var tempPos = -1;

            // Search for the byte array within the source span
            for (var i = 0; i <= span.Length - byteArray.Length; i++)
            {
                var isMatch = true;

                // Compare each byte in the array
                for (var j = 0; j < byteArray.Length; j++)
                {
                    if (span[i + j] != byteArray[j])
                    {
                        isMatch = false;

                        break;
                    }
                }

                // If a match is found, store its position
                if (isMatch)
                {
                    tempPos = i;

                    break;
                }
            }

            // If no array is found or if it's found later than the current minimum, continue searching
            if (tempPos == -1 || tempPos >= minLength)
                continue;

            // Update the minimum position and store the found array
            minLength = tempPos;
            matchedArray = byteArray;
        }

        // Return the found array or null
        return matchedArray;
    }

    /// <summary>
    /// Retrieves files from a byte array based on the specified file format and performs an action on each file's byte array.
    /// </summary>
    /// <param name="array">The source byte array containing the files.</param>
    /// <param name="format">The file format type used to identify the files.</param>
    /// <param name="action">The action to be performed on each file's byte array.</param>
    /// <returns>Returns a MultiStream containing the files found in the source byte array.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the signature for the specified file format is not found.</exception>
    public static MultiStream GetFilesFromByteArray(byte[] array,
                                                    FileFormatType format,
                                                    Action<byte[]> action)
    {
        // Obtain the signatures for the specified file format
        var signatures = format.GetSignature();

        // Create a span from the source byte array
        var span = array.AsSpan(0, array.Length - 1);

        // Find the first occurrence of any of the signatures in the span
        var signature = FindFirstOccurrence(span, signatures);

        // Throw exception if the signature is not found
        if (signature is null)
            throw new KeyNotFoundException($"{format.ToString()} signature not found");

        // List to store the starting indices of each file
        var indices = new List<int>();

        // Create a MultiStream to hold the files
        var ms = new MultiStream();

        // Loop through the source byte array to find matches of the signature
        for (var i = 0; i < array.Length - signature.Length; i++)
        {
            var matchFound = true;

            // Compare each byte for a match
            for (var j = 0; j < signature.Length; j++)
            {
                if (array[i + j] != signature[j])
                {
                    matchFound = false;

                    break;
                }
            }

            // If a match is found
            if (matchFound)
            {
                // Perform the action on the previous file if this is not the first file
                if (indices.Count != 0)
                    action(array[indices[^1]..i]);

                // Add the starting index to the list
                indices.Add(i);
            }
        }

        // Perform the action on the last file
        action(array[indices[^1]..]);

        // Return the MultiStream
        return ms;
    }

    /// <summary>
    /// Retrieves files from a byte array based on a specified signature and adds them to a MultiStream.
    /// </summary>
    /// <param name="array">The source byte array containing the files.</param>
    /// <param name="signature">The byte array signature used to identify the files.</param>
    /// <returns>Returns a MultiStream containing the files found in the source byte array based on the given signature.</returns>
    public static MultiStream GetFilesFromByteArray(byte[] array, byte[] signature)
    {
        // List to store the starting indices of each file
        var indices = new List<int>();

        // Create a MultiStream to hold the files
        var ms = new MultiStream();

        // Loop through the source byte array to find matches of the signature
        for (var i = 0; i < array.Length - signature.Length; i++)
        {
            var matchFound = true;

            // Compare each byte for a match
            for (var j = 0; j < signature.Length; j++)
            {
                if (array[i + j] != signature[j])
                {
                    matchFound = false;

                    break;
                }
            }

            // If a match is found
            if (matchFound)
            {
                // Add the previous file to the MultiStream if this is not the first file
                if (indices.Count != 0)
                    ms.AddStream(new MemoryStream(array[indices[^1]..i]));

                // Add the starting index to the list
                indices.Add(i);
            }
        }

        // Add the last file to the MultiStream
        ms.AddStream(new MemoryStream(array[indices[^1]..]));

        // Return the MultiStream
        return ms;
    }

    /// <summary>
    /// Retrieves files from a byte array based on a specified file format type and adds them to a MultiStream.
    /// </summary>
    /// <param name="array">The source byte array containing the files.</param>
    /// <param name="format">The file format type used to identify the files based on their signature.</param>
    /// <returns>Returns a MultiStream containing the files found in the source byte array based on the given format type.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the signature for the specified file format is not found.</exception>
    public static MultiStream GetFilesFromByteArray(byte[] array, FileFormatType format)
    {
        // Get the signature associated with the specified file format
        var signatures = format.GetSignature();

        // Create a span from the byte array
        var span = array.AsSpan(0, array.Length - 1);

        // Find the first occurrence of the signature in the span
        var signature = FindFirstOccurrence(span, signatures);

        // Throw exception if the signature is not found
        if (signature is null)
            throw new KeyNotFoundException($"{format.ToString()} signature not found");

        // List to store the starting indices of each found file
        var indices = new List<int>();

        // Create a MultiStream to hold the found files
        var ms = new MultiStream();

        // Loop through the array to find matches for the signature
        for (var i = 0; i < array.Length - signature.Length; i++)
        {
            var matchFound = true;

            // Check each byte for a match
            for (var j = 0; j < signature.Length; j++)
            {
                if (array[i + j] != signature[j])
                {
                    matchFound = false;

                    break;
                }
            }

            // If a match is found
            if (matchFound)
            {
                // Add the previous file to the MultiStream if this is not the first file
                if (indices.Count != 0)
                    ms.AddStream(new MemoryStream(array[indices[^1]..i]));

                // Add the starting index to the list
                indices.Add(i);
            }
        }

        // Add the last found file to the MultiStream
        ms.AddStream(new MemoryStream(array[indices[^1]..]));

        // Return the MultiStream
        return ms;
    }

    /// <summary>
    /// Retrieves files from a byte array based on a specified signature, performs an action on each file's byte array, and adds them to a MultiStream.
    /// </summary>
    /// <param name="array">The source byte array containing the files.</param>
    /// <param name="signature">The byte array signature used to identify the files.</param>
    /// <param name="action">The action to be performed on each file's byte array.</param>
    /// <returns>Returns a MultiStream containing the files found in the source byte array based on the given signature.</returns>
    public static MultiStream GetFilesFromByteArray(byte[] array,
                                                    byte[] signature,
                                                    Action<byte[]> action)
    {
        // List to store the starting indices of each found file
        var indices = new List<int>();

        // Create a MultiStream to hold the found files
        var ms = new MultiStream();

        // Loop through the source byte array to find matches for the signature
        for (var i = 0; i < array.Length - signature.Length; i++)
        {
            var matchFound = true;

            // Compare each byte for a match
            for (var j = 0; j < signature.Length; j++)
            {
                if (array[i + j] != signature[j])
                {
                    matchFound = false;

                    break;
                }
            }

            // If a match is found
            if (matchFound)
            {
                // Perform the action on the previous file if this is not the first file
                if (indices.Count != 0)
                    action(array[indices[^1]..i]);

                // Add the starting index to the list
                indices.Add(i);
            }
        }

        // Perform the action on the last file
        action(array[indices[^1]..]);

        // Return the MultiStream
        return ms;
    }

    /// <summary>
    /// Retrieves files from a MemoryStream based on a specified file format type, performs an action on each file's byte array, and adds them to a MultiStream.
    /// </summary>
    /// <param name="stream">The source MemoryStream containing the files.</param>
    /// <param name="format">The file format type used to identify the files based on their signature.</param>
    /// <param name="action">The action to be performed on each file's byte array.</param>
    /// <returns>Returns a MultiStream containing the files found in the source MemoryStream based on the given format type.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the signature for the specified file format is not found.</exception>
    public static MultiStream GetFilesFromStream(MemoryStream stream,
                                                 FileFormatType format,
                                                 Action<byte[]> action)
    {
        // Convert the MemoryStream to a byte array
        var arrayToSearch = stream.ToArray();

        // Get the signature associated with the specified file format
        var signatures = format.GetSignature();

        // Create a span from the byte array
        var span = arrayToSearch.AsSpan(0, arrayToSearch.Length - 1);

        // Find the first occurrence of the signature in the span
        var signature = FindFirstOccurrence(span, signatures);

        // Throw exception if the signature is not found
        if (signature is null)
            throw new KeyNotFoundException($"{format.ToString()} signature not found");

        // List to store the starting indices of each found file
        var indices = new List<int>();

        // Create a MultiStream to hold the found files
        var ms = new MultiStream();

        // Loop through the array to find matches for the signature
        for (var i = 0; i < arrayToSearch.Length - signature.Length; i++)
        {
            var matchFound = true;

            // Check each byte for a match
            for (var j = 0; j < signature.Length; j++)
            {
                if (arrayToSearch[i + j] != signature[j])
                {
                    matchFound = false;

                    break;
                }
            }

            // If a match is found
            if (matchFound)
            {
                // Perform the action on the previous file if this is not the first file
                if (indices.Count != 0)
                    action(arrayToSearch[indices[^1]..i]);

                // Add the starting index to the list
                indices.Add(i);
            }
        }

        // Perform the action on the last file
        action(arrayToSearch[indices[^1]..]);

        // Return the MultiStream
        return ms;
    }

    /// <summary>
    /// Retrieves files from a MemoryStream based on a specified file format type and adds them to a MultiStream.
    /// </summary>
    /// <param name="stream">The source MemoryStream containing the files.</param>
    /// <param name="format">The file format type used to identify the files based on their signature.</param>
    /// <returns>Returns a MultiStream containing the files found in the source MemoryStream based on the given format type.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the signature for the specified file format is not found.</exception>
    public static MultiStream GetFilesFromStream(MemoryStream stream, FileFormatType format)
    {
        // Convert the MemoryStream to a byte array
        var arrayToSearch = stream.ToArray();

        // Get the signature associated with the specified file format
        var signatures = format.GetSignature();

        // Create a span from the byte array
        var span = arrayToSearch.AsSpan(0, arrayToSearch.Length - 1);

        // Find the first occurrence of the signature in the span
        var signature = FindFirstOccurrence(span, signatures);

        // Throw exception if the signature is not found
        if (signature is null)
            throw new KeyNotFoundException($"{format.ToString()} signature not found");

        // List to store the starting indices of each found file
        var indices = new List<int>();

        // Create a MultiStream to hold the found files
        var ms = new MultiStream();

        // Loop through the array to find matches for the signature
        for (var i = 0; i < arrayToSearch.Length - signature.Length; i++)
        {
            var matchFound = true;

            // Check each byte for a match
            for (var j = 0; j < signature.Length; j++)
            {
                if (arrayToSearch[i + j] != signature[j])
                {
                    matchFound = false;

                    break;
                }
            }

            // If a match is found
            if (matchFound)
            {
                // Add the previous file to the MultiStream if this is not the first file
                if (indices.Count != 0)
                    ms.AddStream(new MemoryStream(arrayToSearch[indices[^1]..i]));

                // Add the starting index to the list
                indices.Add(i);
            }
        }

        // Add the last found file to the MultiStream
        ms.AddStream(new MemoryStream(arrayToSearch[indices[^1]..]));

        // Return the MultiStream
        return ms;
    }

    /// <summary>
    /// Retrieves files from a MemoryStream based on a specified signature and adds them to a MultiStream.
    /// </summary>
    /// <param name="stream">The source MemoryStream containing the files.</param>
    /// <param name="signature">The byte array signature used to identify the files.</param>
    /// <returns>Returns a MultiStream containing the files found in the source MemoryStream based on the given signature.</returns>
    public static MultiStream GetFilesFromStream(MemoryStream stream, byte[] signature)
    {
        // Convert the MemoryStream to a byte array
        var arrayToSearch = stream.ToArray();

        // List to store the starting indices of each found file
        var indices = new List<int>();

        // Create a MultiStream to hold the found files
        var ms = new MultiStream();

        // Loop through the array to find matches for the signature
        for (var i = 0; i < arrayToSearch.Length - signature.Length; i++)
        {
            var matchFound = true;

            // Check each byte for a match
            for (var j = 0; j < signature.Length; j++)
            {
                if (arrayToSearch[i + j] != signature[j])
                {
                    matchFound = false;

                    break;
                }
            }

            // If a match is found
            if (matchFound)
            {
                // Add the previous file to the MultiStream if this is not the first file
                if (indices.Count != 0)
                    ms.AddStream(new MemoryStream(arrayToSearch[indices[^1]..i]));

                // Add the starting index to the list
                indices.Add(i);
            }
        }

        // Add the last found file to the MultiStream
        ms.AddStream(new MemoryStream(arrayToSearch[indices[^1]..]));

        // Return the MultiStream
        return ms;
    }

    /// <summary>
    /// Retrieves files from a MemoryStream based on a specified signature, performs an action on each file's byte array, and adds them to a MultiStream.
    /// </summary>
    /// <param name="stream">The source MemoryStream containing the files.</param>
    /// <param name="signature">The byte array signature used to identify the files.</param>
    /// <param name="action">The action to be performed on each file's byte array.</param>
    /// <returns>Returns a MultiStream containing the files found in the source MemoryStream based on the given signature.</returns>
    public static MultiStream GetFilesFromStream(MemoryStream stream,
                                                 byte[] signature,
                                                 Action<byte[]> action)
    {
        // Convert the MemoryStream to a byte array
        var arrayToSearch = stream.ToArray();

        // List to store the starting indices of each found file
        var indices = new List<int>();

        // Create a MultiStream to hold the found files
        var ms = new MultiStream();

        // Loop through the array to find matches for the signature
        for (var i = 0; i < arrayToSearch.Length - signature.Length; i++)
        {
            var matchFound = true;

            // Check each byte for a match
            for (var j = 0; j < signature.Length; j++)
            {
                if (arrayToSearch[i + j] != signature[j])
                {
                    matchFound = false;

                    break;
                }
            }

            // If a match is found
            if (matchFound)
            {
                // Perform the action on the previous file if this is not the first file
                if (indices.Count != 0)
                    action(arrayToSearch[indices[^1]..i]);

                // Add the starting index to the list
                indices.Add(i);
            }
        }

        // Perform the action on the last file
        action(arrayToSearch[indices[^1]..]);

        // Return the MultiStream
        return ms;
    }
}