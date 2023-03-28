using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Enums;
namespace MediaFileProcessor.Extensions;

/// <summary>
/// A class that contains extension methods for working with file data
/// </summary>
public static class FileDataExtensionsTest
{
    /// <summary>
    /// Gets the format type of a file based on its file extension.
    /// </summary>
    /// <param name="fileName">The name of the file to get the format type of.</param>
    /// <returns>The format type of the file.</returns>
    /// <exception cref="Exception">Thrown if the file extension of <paramref name="fileName"/> could not be recognized.</exception>
    public static FileFormatType GetFileFormatTypeTest(this string fileName)
    {
        var ext = Path.GetExtension(fileName);

        if(char.IsDigit(ext[1]))
            ext = "_" + ext;

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
    public static byte[] ConcatByteArraysTest(bool onlyNotDefaultArrays, params byte[][] arrays)
    {
        // Вычисляем общий размер результирующего массива.
        var totalSize = 0;
        for (var i = 0; i < arrays.Length; i++)
        {
            if (!onlyNotDefaultArrays || arrays[i].Length != 0)
                totalSize += arrays[i].Length;
        }

        // Создаем результирующий массив и объединяем данные в него.
        var result = new byte[totalSize];
        var offset = 0;
        for (var i = 0; i < arrays.Length; i++)
        {
            if (onlyNotDefaultArrays && arrays[i].Length == 0)
                continue;

            Buffer.BlockCopy(arrays[i], 0, result, offset, arrays[i].Length);
            offset += arrays[i].Length;
        }

        return result;
    }

    /// <summary>
    /// Searches for the occurrence of a signature within a byte array.
    /// </summary>
    /// <param name="bytes">The array to search within.</param>
    /// <param name="signatures">The signature to search for.</param>
    /// <returns>A tuple representing the start position of the signature within the array and a flag indicating whether the signature was found in its entirety.</returns>
    private static (int?, bool) SearchFileSignatureTest(this ArraySegment<byte> bytes, ArraySegment<byte>[] signatures)
    {
        if (signatures.Length == 0)
            return (null, false);

        foreach (var signature in signatures)
        {
            var signatureLength = signature.Count;

            if (signatureLength == 0)
                continue;

            var signatureStartPos = 0;
            var startFlag = -1;

            for (var i = bytes.Offset; i < bytes.Count + bytes.Offset; i++)
            {
                if ((bytes.Array![i] == signature.Array![signatureStartPos + signature.Offset] || SignaturePositionIgnoreTest(signature, signatureStartPos)) && startFlag is -1)
                {
                    startFlag = i;
                    signatureStartPos++;

                    continue;
                }

                if(bytes.Array![i] != signature.Array[signatureStartPos + signature.Offset] && !SignaturePositionIgnoreTest(signature, signatureStartPos))
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

    private static bool SignaturePositionIgnoreTest(ArraySegment<byte> signature, int signatureStartPos)
    {
        if(signature.Array == null)
            return false;

        var format = signature.Array.GetFormat();

        switch(format)
        {
            case FileFormatType.JPG:
                return 0 == signature.Array[signatureStartPos + signature.Offset] && signatureStartPos is 4 or 5;
            case FileFormatType._3GP or FileFormatType.MP4 or FileFormatType.MOV:
                return 0 == signature.Array[signatureStartPos + signature.Offset] && signatureStartPos is 0 or 1 or 2 or 3;
            case FileFormatType.AVI or FileFormatType.WEBP or FileFormatType.WAV:
                return 0 == signature.Array[signatureStartPos + signature.Offset] && signatureStartPos is 4 or 5 or 6 or 7;
            case FileFormatType.PNG or FileFormatType.ICO or FileFormatType.TIFF or FileFormatType.MKV or FileFormatType.MPEG or FileFormatType.GIF
                 or FileFormatType.FLAC or FileFormatType.VOB or FileFormatType.M2TS or FileFormatType.MXF or FileFormatType.WEBM or FileFormatType.GXF
                 or FileFormatType.FLV or FileFormatType.AAC or FileFormatType.OGG or FileFormatType.WMV or FileFormatType.BMP or FileFormatType.ASF
                 or FileFormatType.MP3 or FileFormatType.RM or FileFormatType.PSD:
            case FileFormatType.WMA when signature.Array[signature.Offset + 0] == 0x30
             && signature.Array[signature.Offset + 1] == 0x26
             && signature.Array[signature.Offset + 2] == 0xB2
             && signature.Array[signature.Offset + 3] == 0x75
             && signature.Array[signature.Offset + 4] == 0x8E
             && signature.Array[signature.Offset + 5] == 0x66
             && signature.Array[signature.Offset + 6] == 0xCF
             && signature.Array[signature.Offset + 7] == 0x11
             && signature.Array[signature.Offset + 8] == 0xA6
             && signature.Array[signature.Offset + 9] == 0xD9
             && signature.Array[signature.Offset + 10] == 0x00
             && signature.Array[signature.Offset + 11] == 0xAA
             && signature.Array[signature.Offset + 12] == 0x00
             && signature.Array[signature.Offset + 13] == 0x62
             && signature.Array[signature.Offset + 14] == 0xCE
             && signature.Array[signature.Offset + 15] == 0x6C:
                return false;
            case FileFormatType.WMA when signature.Array[signature.Offset + 0] == 0x02
             && signature.Array[signature.Offset + 1] == 0x00
             && signature.Array[signature.Offset + 2] == 0x00
             && signature.Array[signature.Offset + 3] == 0x00:
                return 0 == signature.Array[signature.Offset + signatureStartPos] && signatureStartPos > 3;
            case FileFormatType.WMA when signature.Array[signature.Offset + 0] == 0xFE
             && signature.Array[signature.Offset + 1] == 0xFF:
                return 0 == signature.Array[signature.Offset + signatureStartPos] && signatureStartPos > 1;
            case FileFormatType.WMA:
                return false;
            default:
                return false;
        }
    }

    /// <summary>
    /// Gets the list of signatures from the given data.
    /// </summary>
    /// <param name="data">The byte array to search for the signatures.</param>
    /// <param name="signature">The byte array representing the signature to search for.</param>
    /// <returns>A tuple containing a list of byte arrays, a byte array and a byte array.
    /// The list of byte arrays represent the found signatures, the byte array represents the data without the found signatures and the byte array represents previous data (if any).</returns>
    private static (List<byte[]>?, byte[], byte[]?) GetListOfSignatureTest(this byte[] data, List<byte[]> signature)
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
            (var startFilePos, var isFullStartSignature) = SearchFileSignatureTest(data, signature);

            // If a start signature is found and it's not at position 0
            if(startFilePos.HasValue && startFilePos.Value != 0)
            {
                previosdatas = data[..startFilePos.Value];
                data = data[startFilePos.Value..];
                (startFilePos, isFullStartSignature) = SearchFileSignatureTest(data, signature);
            }

            // If a full start signature is found
            if(isFullStartSignature)
            {
                // Search for the end of the signature in the data
                (var endFilePos, isFullEndSignature) = SearchFileSignatureTest(data[(startFilePos!.Value + signature.First().Length)..], signature);

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
    public static MultiStream GetMultiStreamBySignatureTest(this Stream stream, List<byte[]> fileSignature)
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
            // the data read from the input stream, concatenated with the new file start buffer if it exists
            var array = newFileStartBuffer != null
                ? ConcatByteArraysTest(false, newFileStartBuffer, buffer[..nread])
                : buffer[..nread];

            // the list of files found in the current data, as well as the remaining data
            var listFiles = array.GetListOfSignatureTest(fileSignature);

            // check if a new file has started in the current data
            var newFileStarted = array.Length < fileSignature.First().Length
                ? array.SearchFileSignatureTest(fileSignature)
                : array[..fileSignature.First().Length].SearchFileSignatureTest(fileSignature);

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
                var isSig = listFiles.Item2.SearchFileSignatureTest(fileSignature);

                // if the memory stream contains data and the remaining data starts with a file signature,
                // add the concatenated data to the `MultiStream` and reset the memory stream
                if(ms.Length != 0 && isSig.Item2)
                {
                    var data = ConcatByteArraysTest(false, ms.ToArray(), listFiles.Item3);
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
            (var startFilePos, var isFullSignature) = listFiles.Item2.SearchFileSignatureTest(fileSignature);

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
                var checkNewFileStartBuffer = listFiles.Item2[fileSignature.First().Length..].SearchFileSignatureTest(fileSignature);

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
}