using System.Collections;
namespace MediaFileProcessor.Models.Common;

/// <summary>
/// The `MultiStream` class is a concrete implementation of the `Stream` abstract class.
/// It allows multiple streams to be combined into one virtual stream that can be read from.
/// </summary>
public sealed class MultiStream : Stream
{
    public MultiStream() { }

    public MultiStream(Stream[] streams)
    {
        streamList.AddRange(streams);
    }

    public MultiStream(List<Stream> streams)
    {
        streamList.AddRange(streams);
    }

    public MultiStream(IEnumerable<Stream> streams)
    {
        streamList.AddRange(streams.ToArray());
    }

    public MultiStream(ICollection<Stream> streams)
    {
        streamList.AddRange(streams.ToArray());
    }

    /// <summary>
    /// A list of streams to be combined into the virtual stream.
    /// </summary>
    private readonly ArrayList streamList = new ();

    /// <summary>
    /// The current position within the virtual stream.
    /// </summary>
    private long position;

    /// <summary>
    /// Gets a value indicating whether the current stream supports reading.
    /// </summary>
    /// <returns>
    /// Always returns `true` as the `MultiStream` class supports reading.
    /// </returns>
    public override bool CanRead => true;

    /// <summary>
    /// Gets a value indicating whether the current stream supports seeking.
    /// </summary>
    /// <returns>
    /// Always returns `true` as the `MultiStream` class supports seeking.
    /// </returns>
    public override bool CanSeek => true;

    /// <summary>
    /// Gets a value indicating whether the current stream supports writing.
    /// </summary>
    /// <returns>
    /// Always returns `false` as the `MultiStream` class does not support writing.
    /// </returns>
    public override bool CanWrite => false;

    public int Count => streamList.Count;

    /// <summary>
    /// Gets the length of the virtual stream, which is the sum of the lengths of all streams in the `streamList`.
    /// </summary>
    /// <returns>
    /// The length of the virtual stream as a `long`.
    /// </returns>
    public override long Length
    {
        get
        {
            return streamList.Cast<Stream>()
                             .Sum(stream => stream.Length);
        }
    }

    /// <summary>
    /// Gets or sets the position within the virtual stream.
    /// </summary>
    /// <returns>
    /// The current position within the virtual stream as a `long`.
    /// </returns>
    public override long Position
    {
        get => position;
        set => Seek(value, SeekOrigin.Begin);
    }

    /// <summary>
    /// Changes the position within the virtual stream.
    /// </summary>
    /// <param name="offset">A `long` offset to move the position by.</param>
    /// <param name="origin">The origin from which to calculate the new position.
    /// Must be either `SeekOrigin.Begin`, `SeekOrigin.Current`, or `SeekOrigin.End`.</param>
    /// <returns>
    /// The new position within the virtual stream as a `long`.
    /// </returns>
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

    /// <summary>
    /// Reads all the data in the multiple streams and returns it as an array of byte arrays.
    /// </summary>
    /// <returns>An array of byte arrays containing the data of the multiple streams</returns>
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

    /// <summary>
    /// Reads all the data in the multiple streams and returns it as an arrays.
    /// </summary>
    /// <returns>An array of Stream containing the data of the multiple streams</returns>
    public Stream[] ReadAsStreamArray()
    {
        var list = new List<Stream>(streamList.Count);

        foreach(Stream stream in streamList)
        {
            stream.Seek(0, SeekOrigin.Begin);
            list.Add(stream);
        }

        return list.ToArray();
    }

    /// <summary>
    /// Reads a specified number of bytes from the multiple streams into a buffer, starting at a specified index.
    /// </summary>
    /// <param name="buffer">The buffer to read the data into</param>
    /// <param name="offset">The starting index in the buffer</param>
    /// <param name="count">The number of bytes to read</param>
    /// <returns>The total number of bytes read into the buffer</returns>
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

    /// <summary>
    /// Adds a new stream to the multiple streams list.
    /// </summary>
    /// <param name="stream">The stream to add</param>
    public void AddStream(Stream stream)
    {
        streamList.Add(stream);
    }

    /// <summary>
    /// Sets the length of the multiple streams.
    /// WARNING!!! This method cannot be implemented in this class
    /// </summary>
    /// <param name="value">The length to set</param>
    /// <exception cref="NotImplementedException">This method is not implemented in this class</exception>
    public override void SetLength(long value) { }

    /// <summary>
    /// Clears all buffers for the multiple streams and causes any buffered data to be written to the underlying devices.
    /// </summary>
    public override void Flush()
    {
        for (var i = 0; i < streamList.Count; i++)
            ((Stream)streamList[i]).Flush();
    }

    /// <summary>
    /// Writes a specified number of bytes to the multiple streams from a buffer, starting at a specified index.
    /// WARNING!!! This method cannot be implemented in this class
    /// </summary>
    public override void Write(byte[] buffer, int offset, int count) { }
}