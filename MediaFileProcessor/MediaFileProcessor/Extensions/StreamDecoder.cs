using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Enums;
namespace MediaFileProcessor.Extensions;

/// <summary>
/// A class that contains extension methods for working with file data
/// </summary>
public class StreamDecoder
{
    private int signatureLength;
    private List<ReadOnlyMemory<byte>>? signatures;
    private int previosdatasItem3start;
    private int previosdatasItem3end;
    private int dataItem2start;
    private int dataItem2end;

    private byte[] ConcatByteArrays(ReadOnlySpan<byte> array1, ReadOnlySpan<byte> array2)
    {
        var totalSize = array1.Length + array2.Length;

        var result = new byte[totalSize];

        array1.CopyTo(result);
        array2.CopyTo(result.AsSpan(array1.Length));

        return result;
    }

    private (int?, bool) SearchFileSignature(Span<byte> bytes, int startIndex, int endIndex)
    {
        if(signatures is null)
            throw new NullReferenceException("Signatures is null");

        if (signatures.Count == 0)
            return (null, false);

        foreach (var signature in signatures)
        {
            if (signatureLength == 0)
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

                if (startFlag is not -1 && signatureStartPos == signatureLength - 1)
                    return (startFlag, true);

                signatureStartPos++;
            }

            if (startFlag is not -1)
                return (startFlag, false);
        }

        return (null, false);
    }

    private (int?, bool) SearchFileSignature(Span<byte> bytes, ref int startIndex, ref int endIndex)
    {
        if(signatures is null)
            throw new NullReferenceException("Signatures is null");

        if (signatures.Count == 0)
            return (null, false);

        foreach (var signature in signatures)
        {
            if (signatureLength == 0)
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

                if (startFlag is not -1 && signatureStartPos == signatureLength - 1)
                    return (startFlag, true);

                signatureStartPos++;
            }

            if (startFlag is not -1)
                return (startFlag, false);
        }

        return (null, false);
    }

    private (int?, bool) SearchFileSignature(Span<byte> bytes)
    {
        if(signatures is null)
            throw new NullReferenceException("Signatures is null");

        if (signatures.Count == 0)
            return (null, false);

        foreach (var signature in signatures)
        {
            if (signatureLength == 0)
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

                if (startFlag is not -1 && signatureStartPos == signatureLength - 1)
                    return (startFlag, true);

                signatureStartPos++;
            }

            if (startFlag is not -1)
                return (startFlag, false);
        }

        return (null, false);
    }

    private bool SignaturePositionIgnore(ReadOnlySpan<byte> signature, ref int signatureStartPos)
    {
        var format = signature.GetFormat();

        switch(format)
        {
            case FileFormatType.JPG:
                return 0 == signature[signatureStartPos] && signatureStartPos is 4 or 5;
            case FileFormatType._3GP or FileFormatType.MP4 or FileFormatType.MOV:
                return 0 == signature[signatureStartPos] && signatureStartPos is 0 or 1 or 2 or 3;
            case FileFormatType.AVI or FileFormatType.WEBP or FileFormatType.WAV:
                return 0 == signature[signatureStartPos] && signatureStartPos is 4 or 5 or 6 or 7;
            case FileFormatType.PNG or FileFormatType.ICO or FileFormatType.TIFF or FileFormatType.MKV or FileFormatType.MPEG or FileFormatType.GIF or FileFormatType.FLAC
                 or FileFormatType.VOB or FileFormatType.M2TS or FileFormatType.MXF or FileFormatType.WEBM or FileFormatType.GXF or FileFormatType.FLV
                 or FileFormatType.AAC or FileFormatType.OGG or FileFormatType.WMV or FileFormatType.BMP or FileFormatType.ASF or FileFormatType.MP3 or FileFormatType.RM
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

    private List<byte[]>? GetListOfSignature(Span<byte> data)
    {
        if (data.Length < signatureLength)
        {
            previosdatasItem3start = -1;
            previosdatasItem3end = -1;

            dataItem2start = 0;
            dataItem2end = data.Length;

            return null;
        }

        dataItem2end = data.Length;

        var isFullEndSignature = false;

        var listFiles = new List<byte[]>();

        do
        {
            (var startFilePos, var isFullStartSignature) = SearchFileSignature(data, ref dataItem2start, ref dataItem2end);

            if (startFilePos.HasValue && startFilePos.Value != 0)
            {
                previosdatasItem3start = 0;
                previosdatasItem3end = startFilePos.Value;

                dataItem2start = startFilePos.Value;
                dataItem2end = data.Length;

                (startFilePos, isFullStartSignature) = SearchFileSignature(data, ref dataItem2start, ref dataItem2end);
            }

            if (isFullStartSignature)
            {
                (var endFilePos, isFullEndSignature) = SearchFileSignature(data, startFilePos!.Value + signatureLength, data.Length);

                if (isFullEndSignature)
                {
                    var fileBytes = data.Slice(startFilePos.Value, endFilePos!.Value - startFilePos.Value).ToArray();
                    listFiles.Add(fileBytes);


                    dataItem2start = endFilePos.Value;
                    dataItem2end = data.Length;
                }
            }
        } while (isFullEndSignature);

        if (previosdatasItem3start == -1 || previosdatasItem3end == 0)
        {
            previosdatasItem3start = -1;
            previosdatasItem3end = -1;
        }

        return listFiles.Count == 0 ? null : listFiles;
    }

    public MultiStream GetMultiStreamBySignatureTest1(Stream stream, List<byte[]> fileSignature)
    {
        signatureLength = fileSignature.First().Length;
        signatures = fileSignature.Select(b => new ReadOnlyMemory<byte>(b)).ToList();

        var buffer = new byte[16 * 1024];

        byte[]? newFileStartBuffer = null;

        var ms = new MemoryStream();

        var multiStream = new MultiStream();

        int nread;

        while ((nread = stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            var array = newFileStartBuffer != null ? ConcatByteArrays(newFileStartBuffer, buffer.AsSpan(0, nread)) : buffer.AsSpan(0, nread);
            var listFiles = GetListOfSignature(array);

            var newFileStarted = array.Length < signatureLength ? SearchFileSignature(array, 0, array.Length)
                : SearchFileSignature(array, 0, signatureLength);

            if (previosdatasItem3end == -1 && newFileStarted.Item2 && newFileStarted.Item1!.Value == 0 && ms.Length != 0)
            {
                multiStream.AddStream(new MemoryStream(ms.ToArray()));
                ms = new MemoryStream();
            }

            newFileStartBuffer = null;

            if (previosdatasItem3end != -1)
            {
                var isSig = SearchFileSignature(array[dataItem2start..dataItem2end]);

                if (ms.Length != 0 && isSig.Item2)
                {
                    var data = ConcatByteArrays(ms.ToArray(), array[previosdatasItem3start..previosdatasItem3end]);
                    multiStream.AddStream(new MemoryStream(data));
                    ms = new MemoryStream();
                }

                if (ms.Length != 0 && !isSig.Item2)
                    ms.Write(array[previosdatasItem3start..previosdatasItem3end].ToArray(), 0, array[previosdatasItem3start..previosdatasItem3end].Length);

                if (isSig is { Item1: { }, Item2: false })
                {
                    newFileStartBuffer = array[dataItem2start..dataItem2end].ToArray();

                    dataItem2start = default;
                    dataItem2end = default;
                    previosdatasItem3start = default;
                    previosdatasItem3end = default;

                    continue;
                }
            }

            if (listFiles is not null)
                foreach (var i in listFiles)
                    multiStream.AddStream(new MemoryStream(i));

            (var startFilePos, var isFullSignature) = SearchFileSignature(array[dataItem2start..dataItem2end]);

            if (isFullSignature)
            {
                if (startFilePos!.Value == 0 && ms.Length != 0)
                {
                    var data = ms.ToArray();
                    ms = new MemoryStream();

                    multiStream.AddStream(new MemoryStream(data));
                }

                var checkNewFileStartBuffer = SearchFileSignature(array[(dataItem2start + signatureLength)..dataItem2end]);

                if (startFilePos.Value == 0 && checkNewFileStartBuffer.Item1 != null)
                {
                    ms.Write(array[dataItem2start..(dataItem2end - checkNewFileStartBuffer.Item1.Value + signatureLength)].ToArray(),
                             0,
                             array[dataItem2start..(dataItem2end - checkNewFileStartBuffer.Item1.Value + signatureLength)].Length);

                    newFileStartBuffer = array[(checkNewFileStartBuffer.Item1.Value + signatureLength + dataItem2start)..dataItem2end].ToArray();
                }
                else
                {
                    ms.Write(array[dataItem2start..dataItem2end].ToArray(), 0, array[dataItem2start..dataItem2end].Length);
                }
            }
            else
            {
                ms.Write(array[dataItem2start..dataItem2end].ToArray(), 0, array[dataItem2start..dataItem2end].Length);
            }

            dataItem2start = default;
            dataItem2end = default;
            previosdatasItem3start = default;
            previosdatasItem3end = default;
        }

        if (ms.Length != 0)
            multiStream.AddStream(new MemoryStream(ms.ToArray()));

        return multiStream;
    }
}