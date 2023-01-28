using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Enums;
namespace MediaFileProcessor.Extensions;

public static class FileDataExtensions
{
    public static FileFormatType GetFileFormatType(this string fileName)
    {
        var ext = Path.GetExtension(fileName);

        if (Enum.TryParse(ext, out FileFormatType output))
            return output;

        throw new Exception("The file extension could not be recognized");
    }

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

    public static (int?, bool) SearchFileSignature(this byte[] a, byte[] b)
    {
        var signatureLength = b.Length;
        var signatureStartPos = 0;
        var startFlag = -1;

        for (var i = 0; i < a.Length; i++)
        {
            if (a[i] == b[signatureStartPos] && startFlag is -1)
            {
                startFlag = i;
                signatureStartPos++;

                continue;
            }

            if(a[i] != b[signatureStartPos])
            {
                startFlag = -1;
                signatureStartPos = default;

                continue;
            }

            if(startFlag is not -1 && signatureStartPos == signatureLength - 1)
                return (startFlag, true);

            signatureStartPos++;
        }

        return startFlag is -1 ? (null, false) : (startFlag, false);
    }

    public static (List<byte[]>?, byte[], byte[]?) GetListOfSignature(this byte[] data, byte[] signature)
    {
        if (data.Length < signature.Length)
            return (null, data, null);

        var isFullEndSignature = false;

        var listFiles = new List<byte[]>();

        var previosdatas = Array.Empty<byte>();

        do
        {
            (var startFilePos, var isFullStartSignature) = SearchFileSignature(data, signature);

            if(startFilePos.HasValue && startFilePos.Value != 0)
            {
                previosdatas = data[..startFilePos.Value];
                data = data[startFilePos.Value..];
                (startFilePos, isFullStartSignature) = SearchFileSignature(data, signature);
            }

            if(isFullStartSignature)
            {
                (var endFilePos, isFullEndSignature) = SearchFileSignature(data[(startFilePos!.Value + signature.Length)..], signature);

                if(isFullEndSignature)
                {
                    var fileBytes = data[startFilePos.Value..(endFilePos!.Value + signature.Length)];
                    listFiles.Add(fileBytes);

                    data = data[(endFilePos.Value + signature.Length)..];
                }
            }
        } while (isFullEndSignature);

        if(previosdatas.Length == 0)
            previosdatas = null;

        return listFiles.Count == 0 ? (null, data, previosdatas) : (listFiles, data, previosdatas);
    }

    public static MultiStream GetMultiStreamBySignature(this Stream stream, byte[] fileSignature)
    {
        var buffer = new byte[16 * 1024];
        byte[]? newFileStartBuffer = null;

        var ms = new MemoryStream();
        var multiStream = new MultiStream();

        int nread;
        while ((nread = stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            var array = newFileStartBuffer != null
                ? ConcatByteArrays(false, newFileStartBuffer, buffer[..nread])
                : buffer[..nread];

            var listFiles = array.GetListOfSignature(fileSignature);

            var newFileStarted = array.Length < fileSignature.Length
                ? array.SearchFileSignature(fileSignature)
                : array[..fileSignature.Length].SearchFileSignature(fileSignature);

            if(listFiles.Item3 == null && newFileStarted.Item2 && newFileStarted.Item1!.Value == 0 && ms.Length != 0)
            {
                multiStream.AddStream(new MemoryStream(ms.ToArray()));
                ms = new MemoryStream();
            }

            newFileStartBuffer = null;

            if (listFiles.Item3 != null)
            {
                var isSig = listFiles.Item2.SearchFileSignature(fileSignature);

                if(ms.Length != 0 && isSig.Item2)
                {
                    var data = ConcatByteArrays(false, ms.ToArray(), listFiles.Item3);
                    multiStream.AddStream(new MemoryStream(data));
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
                    multiStream.AddStream(new MemoryStream(i));

            (var startFilePos, var isFullSignature) = listFiles.Item2.SearchFileSignature(fileSignature);

            if(isFullSignature)
            {
                if(startFilePos!.Value == 0 && ms.Length != 0)
                {
                    var data = ms.ToArray();
                    ms = new MemoryStream();
                    multiStream.AddStream(new MemoryStream(data));
                }

                var checkNewFileStartBuffer = listFiles.Item2[fileSignature.Length..].SearchFileSignature(fileSignature);

                if(startFilePos.Value == 0 && checkNewFileStartBuffer.Item1 != null)
                {
                    var test = listFiles.Item2[..(checkNewFileStartBuffer.Item1.Value + fileSignature.Length)];
                    ms.Write(test, 0, test.Length);
                    newFileStartBuffer = listFiles.Item2[(checkNewFileStartBuffer.Item1.Value + fileSignature.Length)..];

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
            multiStream.AddStream(new MemoryStream(data));
        }

        ms.Dispose();
        ms.Close();

        return multiStream;
    }
}