using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Enums;

namespace MediaFileProcessor.Extensions;

/// <summary>
/// A class that contains extension methods for working with file data
/// </summary>
internal class StreamDecodeExtensions
{
    /// <summary>
    /// Length of the file signature.
    /// </summary>
    private int _signatureLength;

    /// <summary>
    /// List of signatures to search for.
    /// </summary>
    private List<ReadOnlyMemory<byte>>? _signatures;

    /// <summary>
    /// Start index of the previous DataItem3.
    /// </summary>
    private int _previosdatasItem3Start;

    /// <summary>
    /// End index of the previous DataItem3.
    /// </summary>
    private int _previosdatasItem3End;

    /// <summary>
    /// Start index of DataItem2.
    /// </summary>
    private int _dataItem2Start;

    /// <summary>
    /// End index of DataItem2.
    /// </summary>
    private int _dataItem2End;

    /// <summary>
    /// Concatenates two byte arrays into a single byte array.
    /// </summary>
    /// <param name="array1">The first byte array.</param>
    /// <param name="array2">The second byte array.</param>
    /// <returns>The concatenated byte array.</returns>
    private byte[] ConcatByteArrays(ReadOnlySpan<byte> array1, ReadOnlySpan<byte> array2)
    {
        var totalSize = array1.Length + array2.Length;

        var result = new byte[totalSize];

        array1.CopyTo(result);
        array2.CopyTo(result.AsSpan(array1.Length));

        return result;
    }

    /// <summary>
    /// Searches for a file signature within a given byte span.
    /// </summary>
    /// <param name="bytes">The byte span to search within.</param>
    /// <param name="startIndex">The start index of the search.</param>
    /// <param name="endIndex">The end index of the search.</param>
    /// <returns>A tuple containing the start index of the found signature and a boolean indicating if the entire signature was found.</returns>
    private (int?, bool) SearchFileSignature(Span<byte> bytes, int startIndex, int endIndex)
    {
        if (_signatures is null)
            throw new InvalidOperationException("Signatures is null");

        if (_signatures.Count == 0)
            return (null, false);

        foreach (var signature in _signatures)
        {
            if (_signatureLength == 0)
                continue;

            var signatureStartPos = 0;
            var startFlag = -1;

            for (var i = startIndex; i < endIndex; i++)
            {
                if ((bytes[i] == signature.Span[signatureStartPos] || SignaturePositionIgnore(signature.Span, ref signatureStartPos)) && startFlag is -1)
                {
                    startFlag = i;
                    signatureStartPos++;

                    continue;
                }

                if (bytes[i] != signature.Span[signatureStartPos] && !SignaturePositionIgnore(signature.Span, ref signatureStartPos))
                {
                    startFlag = -1;
                    signatureStartPos = default;

                    continue;
                }

                if (startFlag is not -1 && signatureStartPos == _signatureLength - 1)
                    return (startFlag, true);

                signatureStartPos++;
            }

            if (startFlag is not -1)
                return (startFlag, false);
        }

        return (null, false);
    }

    /// <summary>
    /// Searches for a file signature within a given byte span.
    /// </summary>
    /// <param name="bytes">The byte span to search within.</param>
    /// <param name="startIndex">The start index of the search. This is a reference parameter.</param>
    /// <param name="endIndex">The end index of the search. This is a reference parameter.</param>
    /// <returns>A tuple containing the start index of the found signature and a boolean indicating if the entire signature was found.</returns>
    private (int?, bool) SearchFileSignature(Span<byte> bytes, ref int startIndex, ref int endIndex)
    {
        if (_signatures is null)
            throw new InvalidOperationException("Signatures is null");

        if (_signatures.Count == 0)
            return (null, false);

        foreach (var signature in _signatures)
        {
            if (_signatureLength == 0)
                continue;

            var signatureStartPos = 0;
            var startFlag = -1;

            for (var i = startIndex; i < endIndex; i++)
            {
                if ((bytes[i] == signature.Span[signatureStartPos] || SignaturePositionIgnore(signature.Span, ref signatureStartPos)) && startFlag is -1)
                {
                    startFlag = i;
                    signatureStartPos++;

                    continue;
                }

                if (bytes[i] != signature.Span[signatureStartPos] && !SignaturePositionIgnore(signature.Span, ref signatureStartPos))
                {
                    startFlag = -1;
                    signatureStartPos = default;

                    continue;
                }

                if (startFlag is not -1 && signatureStartPos == _signatureLength - 1)
                    return (startFlag, true);

                signatureStartPos++;
            }

            if (startFlag is not -1)
                return (startFlag, false);
        }

        return (null, false);
    }

    /// <summary>
    /// Searches for a file signature within the entire byte span.
    /// </summary>
    /// <param name="bytes">The byte span to search within.</param>
    /// <returns>A tuple containing the start index of the found signature and a boolean indicating if the entire signature was found.</returns>
    private (int?, bool) SearchFileSignature(Span<byte> bytes)
    {
        if (_signatures is null)
            throw new InvalidOperationException("Signatures is null");

        if (_signatures.Count == 0)
            return (null, false);

        foreach (var signature in _signatures)
        {
            if (_signatureLength == 0)
                continue;

            var signatureStartPos = 0;
            var startFlag = -1;

            for (var i = 0; i < bytes.Length; i++)
            {
                if ((bytes[i] == signature.Span[signatureStartPos] || SignaturePositionIgnore(signature.Span, ref signatureStartPos)) && startFlag is -1)
                {
                    startFlag = i;
                    signatureStartPos++;

                    continue;
                }

                if (bytes[i] != signature.Span[signatureStartPos] && !SignaturePositionIgnore(signature.Span, ref signatureStartPos))
                {
                    startFlag = -1;
                    signatureStartPos = default;

                    continue;
                }

                if (startFlag is not -1 && signatureStartPos == _signatureLength - 1)
                    return (startFlag, true);

                signatureStartPos++;
            }

            if (startFlag is not -1)
                return (startFlag, false);
        }

        return (null, false);
    }

    /// <summary>
    /// Determines whether a given position in a file signature should be ignored during the search.
    /// </summary>
    /// <param name="signature">The file signature in a byte span format.</param>
    /// <param name="signatureStartPos">The current start position within the signature. This is a reference parameter.</param>
    /// <returns>A boolean indicating whether the current position should be ignored or not.</returns>
    private bool SignaturePositionIgnore(ReadOnlySpan<byte> signature, ref int signatureStartPos)
    {
        var format = signature.GetFormat();

        switch (format)
        {
            case FileFormatType.JPG:
                return 0 == signature[signatureStartPos] && signatureStartPos is 4 or 5;
            case FileFormatType._3GP or FileFormatType.MP4 or FileFormatType.MOV:
                return 0 == signature[signatureStartPos] && signatureStartPos is 0 or 1 or 2 or 3;
            case FileFormatType.AVI or FileFormatType.WEBP or FileFormatType.WAV:
                return 0 == signature[signatureStartPos] && signatureStartPos is 4 or 5 or 6 or 7;
            case FileFormatType.PNG
                 or FileFormatType.ICO
                 or FileFormatType.TIFF
                 or FileFormatType.MKV
                 or FileFormatType.MPEG
                 or FileFormatType.GIF
                 or FileFormatType.FLAC
                 or FileFormatType.VOB
                 or FileFormatType.M2TS
                 or FileFormatType.MXF
                 or FileFormatType.WEBM
                 or FileFormatType.GXF
                 or FileFormatType.FLV
                 or FileFormatType.AAC
                 or FileFormatType.OGG
                 or FileFormatType.WMV
                 or FileFormatType.BMP
                 or FileFormatType.ASF
                 or FileFormatType.MP3
                 or FileFormatType.RM
                 or FileFormatType.PSD:
            case FileFormatType.WMA when signature[0] == 0x30
                                         && signature[1] == 0x26
                                         && signature[2] == 0xB2
                                         && signature[3] == 0x75
                                         && signature[4] == 0x8E
                                         && signature[5] == 0x66
                                         && signature[6] == 0xCF
                                         && signature[7] == 0x11
                                         && signature[8] == 0xA6
                                         && signature[9] == 0xD9
                                         && signature[10] == 0x00
                                         && signature[11] == 0xAA
                                         && signature[12] == 0x00
                                         && signature[13] == 0x62
                                         && signature[14] == 0xCE
                                         && signature[15] == 0x6C:
                return false;
            case FileFormatType.WMA when signature[0] == 0x02 && signature[1] == 0x00 && signature[2] == 0x00 && signature[3] == 0x00:
                return 0 == signature[signatureStartPos] && signatureStartPos > 3;
            case FileFormatType.WMA when signature[0] == 0xFE && signature[1] == 0xFF:
                return 0 == signature[signatureStartPos] && signatureStartPos > 1;
            case FileFormatType.WMA:
                return false;
            default:
                return false;
        }
    }

    /// <summary>
    /// Searches for file signatures in a given byte span and returns a list of byte arrays representing the found files.
    /// </summary>
    /// <param name="data">The byte span containing the data to search within.</param>
    /// <returns>
    /// A list of byte arrays, each representing a found file based on its signature, or null if no files are found 
    /// or if the data length is less than the signature length.
    /// </returns>
    private List<byte[]>? GetListOfSignature(Span<byte> data)
    {
        if (data.Length < _signatureLength)
        {
            _previosdatasItem3Start = -1;
            _previosdatasItem3End = -1;

            _dataItem2Start = 0;
            _dataItem2End = data.Length;

            return null;
        }

        _dataItem2End = data.Length;

        var isFullEndSignature = false;

        var listFiles = new List<byte[]>();

        do
        {
            var (startFilePos, isFullStartSignature) = SearchFileSignature(data, ref _dataItem2Start, ref _dataItem2End);

            if (startFilePos.HasValue && startFilePos.Value != 0)
            {
                _previosdatasItem3Start = 0;
                _previosdatasItem3End = startFilePos.Value;

                _dataItem2Start = startFilePos.Value;
                _dataItem2End = data.Length;

                (startFilePos, isFullStartSignature) = SearchFileSignature(data, ref _dataItem2Start, ref _dataItem2End);
            }

            if (isFullStartSignature)
            {
                (var endFilePos, isFullEndSignature) = SearchFileSignature(data, startFilePos!.Value + _signatureLength, data.Length);

                if (isFullEndSignature)
                {
                    var fileBytes = data.Slice(startFilePos.Value, endFilePos!.Value - startFilePos.Value)
                                        .ToArray();

                    listFiles.Add(fileBytes);

                    _dataItem2Start = endFilePos.Value;
                    _dataItem2End = data.Length;
                }
            }
        } while (isFullEndSignature);

        if (_previosdatasItem3Start == -1 || _previosdatasItem3End == 0)
        {
            _previosdatasItem3Start = -1;
            _previosdatasItem3End = -1;
        }

        return listFiles.Count == 0 ? null : listFiles;
    }

    /// <summary>
    /// Reads an input stream and identifies embedded streams based on their file signatures.
    /// </summary>
    /// <param name="stream">The input stream to read.</param>
    /// <param name="fileSignature">A list of byte arrays representing the file signatures to search for.</param>
    /// <returns>
    /// A MultiStream object containing the identified embedded streams.
    /// </returns>
    /// <exception cref="InvalidOperationException">Thrown if no file signatures are provided.</exception>
    public MultiStream GetMultiStreamBySignature(Stream stream, List<byte[]> fileSignature)
    {
        _signatureLength = fileSignature[0].Length;

        _signatures = fileSignature.Select(b => new ReadOnlyMemory<byte>(b)).ToList();

        var buffer = new byte[16 * 1024];

        byte[]? newFileStartBuffer = null;

        var ms = new MemoryStream();

        var multiStream = new MultiStream();

        int nread;

        while ((nread = stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            var array = newFileStartBuffer != null ? ConcatByteArrays(newFileStartBuffer, buffer.AsSpan(0, nread)) : buffer.AsSpan(0, nread);
            var listFiles = GetListOfSignature(array);

            var newFileStarted = array.Length < _signatureLength
                                     ? SearchFileSignature(array, 0, array.Length)
                                     : SearchFileSignature(array, 0, _signatureLength);

            if (_previosdatasItem3End == -1 && newFileStarted.Item2 && newFileStarted.Item1!.Value == 0 && ms.Length != 0)
            {
                multiStream.AddStream(new MemoryStream(ms.ToArray()));
                ms = new MemoryStream();
            }

            newFileStartBuffer = null;

            if (_previosdatasItem3End != -1)
            {
                var isSig = SearchFileSignature(array[_dataItem2Start.._dataItem2End]);

                if (ms.Length != 0 && isSig.Item2)
                {
                    var data = ConcatByteArrays(ms.ToArray(), array[_previosdatasItem3Start.._previosdatasItem3End]);
                    multiStream.AddStream(new MemoryStream(data));
                    ms = new MemoryStream();
                }

                if (ms.Length != 0 && !isSig.Item2)
                    ms.Write(array[_previosdatasItem3Start.._previosdatasItem3End].ToArray(),
                             0,
                             array[_previosdatasItem3Start.._previosdatasItem3End].Length);

                if (isSig is {Item1: not null, Item2: false})
                {
                    newFileStartBuffer = array[_dataItem2Start.._dataItem2End]
                        .ToArray();

                    _dataItem2Start = default;
                    _dataItem2End = default;
                    _previosdatasItem3Start = default;
                    _previosdatasItem3End = default;

                    continue;
                }
            }

            if (listFiles is not null)
                foreach (var i in listFiles)
                    multiStream.AddStream(new MemoryStream(i));

            var (startFilePos, isFullSignature) = SearchFileSignature(array[_dataItem2Start.._dataItem2End]);

            if (isFullSignature)
            {
                if (startFilePos!.Value == 0 && ms.Length != 0)
                {
                    var data = ms.ToArray();
                    ms = new MemoryStream();

                    multiStream.AddStream(new MemoryStream(data));
                }

                var checkNewFileStartBuffer = SearchFileSignature(array[(_dataItem2Start + _signatureLength).._dataItem2End]);

                if (startFilePos.Value == 0 && checkNewFileStartBuffer.Item1 != null)
                {
                    ms.Write(array[_dataItem2Start..(_dataItem2End - checkNewFileStartBuffer.Item1.Value + _signatureLength)].ToArray(),
                             0,
                             array[_dataItem2Start..(_dataItem2End - checkNewFileStartBuffer.Item1.Value + _signatureLength)].Length);

                    newFileStartBuffer = array[(checkNewFileStartBuffer.Item1.Value + _signatureLength + _dataItem2Start).._dataItem2End].ToArray();
                }
                else
                {
                    ms.Write(array[_dataItem2Start.._dataItem2End].ToArray(),
                             0,
                             array[_dataItem2Start.._dataItem2End].Length);
                }
            }
            else
            {
                ms.Write(array[_dataItem2Start.._dataItem2End].ToArray(),
                         0,
                         array[_dataItem2Start.._dataItem2End].Length);
            }

            _dataItem2Start = default;
            _dataItem2End = default;
            _previosdatasItem3Start = default;
            _previosdatasItem3End = default;
        }

        if (ms.Length != 0)
            multiStream.AddStream(new MemoryStream(ms.ToArray()));

        return multiStream;
    }
}