using System.Collections;
namespace MediaFileProcessor.Models.Common;

public sealed class MultiStream : Stream
{
    private readonly ArrayList streamList = new ();
    private long position;
    public override bool CanRead => true;
    public override bool CanSeek => true;
    public override bool CanWrite => false;

    public override long Length
    {
        get
        {
            return streamList.Cast<Stream>()
                             .Sum(stream => stream.Length);
        }
    }

    public override long Position
    {
        get => position;
        set => Seek(value, SeekOrigin.Begin);
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        var len = Length;

        switch(origin)
        {
            case SeekOrigin.Begin:
                position = offset;

                break;
            case SeekOrigin.Current:
                position += offset;

                break;
            case SeekOrigin.End:
                position = len - offset;

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(origin), origin, null);
        }

        if(position > len)
            position = len;
        else if(position < 0)
            position = 0;

        return position;
    }

    public byte[][] ReadAsDataArray()
    {
        var list = new List<byte[]>(streamList.Count);

        foreach(Stream stream in streamList)
        {
            var buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);
            list.Add(buffer);
            stream.Seek(0, SeekOrigin.Begin);
        }

        return list.ToArray();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        long len = 0;
        var result = 0;
        var buf_pos = offset;

        foreach(Stream stream in streamList)
        {
            if(position < len + stream.Length)
            {
                stream.Position = position - len;
                var bytesRead = stream.Read(buffer, buf_pos, count);
                result += bytesRead;
                buf_pos += bytesRead;
                position += bytesRead;

                if(bytesRead < count)
                    count -= bytesRead;
                else
                    break;
            }
            len += stream.Length;
        }

        return result;
    }

    public void AddStream(Stream stream)
    {
        streamList.Add(stream);
    }

    public override void SetLength(long value) { }

    public override void Flush() { }

    public override void Write(byte[] buffer, int offset, int count) { }
}