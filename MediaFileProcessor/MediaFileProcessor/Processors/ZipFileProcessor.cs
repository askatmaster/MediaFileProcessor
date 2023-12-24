using System.Globalization;
using System.IO.Compression;
using System.Text;
using MediaFileProcessor.Models.Enums;
using MediaFileProcessor.Models.ZipFile;

namespace MediaFileProcessor.Processors;

/// <summary>
/// Unique class for compression/decompression file. Represents a Zip file.
/// </summary>
public sealed class ZipFileProcessor : IDisposable
{
    /// <summary>
    /// True if UTF8 encoding for filename and comments, false if default (CP 437)
    /// </summary>
    public readonly bool EncodeUTF8 = false;

    /// <summary>
    /// Force deflate algorithm even if it inflates the stored file. Off by default.
    /// </summary>
    public readonly bool ForceDeflating = false;

    /// <summary>
    /// List of files to store
    /// </summary>
    private readonly List<ZipFileEntry> _files = new ();

    /// <summary>
    /// Filename of storage file
    /// </summary>
    private string? _fileName;

    /// <summary>
    /// Stream object of storage file
    /// </summary>
    private Stream? _zipFileStream;

    /// <summary>
    /// General comment
    /// </summary>
    private string _comment = string.Empty;

    // Central dir image
    private byte[]? _centralDirImage;

    /// <summary>
    /// Existing files in zip
    /// </summary>
    private long _existingFiles;

    /// <summary>
    /// File access for Open method
    /// </summary>
    private FileAccess _access;

    /// <summary>
    /// leave the stream open after the ZipStorer object is disposed
    /// </summary>
    private bool _leaveOpen;

    /// <summary>
    /// Dispose control
    /// </summary>
    private bool _isDisposed;

    /// <summary>
    /// Static CRC32 Table
    /// </summary>
    private static readonly uint[]? CrcTable;

    /// <summary>
    /// Default filename encoder
    /// </summary>
    private static readonly Encoding DefaultEncoding = Encoding.ASCII;

    /// <summary>
    /// Static constructor. Just invoked once in order to create the CRC32 lookup table.
    /// </summary>
    static ZipFileProcessor()
    {
        // Generate CRC32 table
        CrcTable = new uint[256];

        for (var i = 0;
             i < CrcTable.Length;
             i++)
        {
            var c = (uint)i;

            for (var j = 0;
                 j < 8;
                 j++)
            {
                if ((c & 1) != 0)
                    c = 3_988_292_384 ^ (c >> 1);
                else
                    c >>= 1;
            }

            CrcTable[i] = c;
        }
    }

    /// <summary>
    /// Method to create a new storage file
    /// </summary>
    /// <param name="filename">Full path of Zip file to create</param>
    /// <param name="comment">General comment for Zip file</param>
    /// <returns>A valid ZipStorer object</returns>
    public static ZipFileProcessor Create(string filename, string? comment = null)
    {
        Stream stream = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite);

        var zip = Create(stream, comment);
        zip._comment = comment ?? string.Empty;
        zip._fileName = filename;

        return zip;
    }

    /// <summary>
    /// Method to create a new zip storage in a stream
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="comment"></param>
    /// <param name="leaveOpen">true to leave the stream open after the ZipStorer object is disposed; otherwise, false (default).</param>
    /// <returns>A valid ZipStorer object</returns>
    public static ZipFileProcessor Create(Stream stream, string? comment = null, bool leaveOpen = false)
    {
        var zip = new ZipFileProcessor
        {
            _comment = comment ?? string.Empty,
            _zipFileStream = stream,
            _access = FileAccess.Write,
            _leaveOpen = leaveOpen
        };

        return zip;
    }

    /// <summary>
    /// Method to open an existing storage file
    /// </summary>
    /// <param name="filename">Full path of Zip file to open</param>
    /// <param name="access">File access mode as used in FileStream constructor</param>
    /// <returns>A valid ZipStorer object</returns>
    public static ZipFileProcessor Open(string filename, FileAccess access)
    {
        Stream? stream = null;
        ZipFileProcessor? zip = null;

        try
        {
            stream = new FileStream(filename, FileMode.Open, access == FileAccess.Read ? FileAccess.Read : FileAccess.ReadWrite);

            zip = Open(stream, access);
            zip._fileName = filename;
        }
        catch (Exception)
        {
            stream?.Dispose();

            if (zip == null)
                throw;

            zip.Dispose();

            throw;
        }

        return zip;
    }

    /// <summary>
    /// Method to open an existing storage from stream
    /// </summary>
    /// <param name="stream">Already opened stream with zip contents</param>
    /// <param name="access">File access mode for stream operations</param>
    /// <param name="leaveOpen">true to leave the stream open after the ZipStorer object is disposed; otherwise, false (default).</param>
    /// <returns>A valid ZipStorer object</returns>
    public static ZipFileProcessor Open(Stream stream, FileAccess access, bool leaveOpen = false)
    {
        if (!stream.CanSeek && access != FileAccess.Read)
            throw new InvalidOperationException("Stream cannot seek");

        var zip = new ZipFileProcessor
        {
            _zipFileStream = stream, _access = access, _leaveOpen = leaveOpen
        };

        if (zip.ReadFileInfo())
            return zip;

        if (!leaveOpen)
            zip.Close();

        throw new InvalidDataException();
    }

    /// <summary>
    /// Add full contents of a file into the Zip storage
    /// </summary>
    /// <param name="method">Compression method</param>
    /// <param name="pathname">Full path of file to add to Zip storage</param>
    /// <param name="filenameInZip">Filename and path as desired in Zip directory</param>
    /// <param name="comment">Comment for stored file</param>
    public ZipFileEntry AddFile(ZipCompression method,
                                string pathname,
                                string filenameInZip,
                                string? comment = null)
    {
        if (_access == FileAccess.Read)
            throw new InvalidOperationException("Writing is not alowed");

        using var stream = new FileStream(pathname, FileMode.Open, FileAccess.Read);

        return AddStream(method, filenameInZip, stream, File.GetLastWriteTime(pathname), comment);
    }

    /// <summary>
    /// Add full contents of a stream into the Zip storage
    /// </summary>
    /// <remarks>Same parameters and return value as AddStreamAsync()</remarks>
    public ZipFileEntry AddStream(ZipCompression method,
                                  string filenameInZip,
                                  Stream source,
                                  DateTime modTime,
                                  string? comment = null)
    {
        return Task.Run(() => AddStreamAsync(method, filenameInZip, source, modTime, comment)).Result;
    }

    /// <summary>
    /// Add full contents of a stream into the Zip storage
    /// </summary>
    /// <param name="method">Compression method</param>
    /// <param name="filenameInZip">Filename and path as desired in Zip directory</param>
    /// <param name="source">Stream object containing the data to store in Zip</param>
    /// <param name="modTime">Modification time of the data to store</param>
    /// <param name="comment">Comment for stored file</param>
    public async Task<ZipFileEntry> AddStreamAsync(ZipCompression method,
                                                   string filenameInZip,
                                                   Stream source,
                                                   DateTime modTime,
                                                   string? comment = null)
    {
        if (_access == FileAccess.Read)
            throw new InvalidOperationException("Writing is not alowed");

        // Prepare the fileinfo
        var zfe = new ZipFileEntry
        {
            Method = method,
            EncodeUTF8 = EncodeUTF8,
            FilenameInZip = NormalizedFilename(filenameInZip),
            Comment = comment ?? string.Empty,
            Crc32 = 0,                                     // to be updated later
            HeaderOffset = (uint)_zipFileStream!.Position, // offset within file of the start of this local record
            CreationTime = modTime,
            ModifyTime = modTime,
            AccessTime = modTime
        };

        // Write local header
        WriteLocalHeader(zfe);
        zfe.FileOffset = (uint)_zipFileStream.Position;

        // Write file to zip (store)
        await StoreAsync(zfe, source);

        source.Close();
        UpdateCrcAndSizes(zfe);
        _files.Add(zfe);

        return zfe;
    }

    /// <summary>
    /// Add full contents of a directory into the Zip storage
    /// </summary>
    /// <param name="method">Compression method</param>
    /// <param name="pathname">Full path of directory to add to Zip storage</param>
    /// <param name="pathnameInZip">Path name as desired in Zip directory</param>
    public void AddDirectory(ZipCompression method, string pathname, string pathnameInZip)
    {
        if (_access == FileAccess.Read)
            throw new InvalidOperationException("Writing is not allowed");

        var pos = pathname.LastIndexOf(Path.DirectorySeparatorChar);
        var separator = Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture);

        var foldername = pos >= 0 ? pathname.Remove(0, pos + 1) : pathname;

        if (!string.IsNullOrEmpty(pathnameInZip))
            foldername = pathnameInZip + foldername;

        if (!foldername.EndsWith(separator, StringComparison.CurrentCulture))
            foldername += separator;

        // Process the list of files found in the directory.
        var fileEntries = Directory.GetFiles(pathname);

        foreach (var fileName in fileEntries)
            AddFile(method, fileName, foldername + Path.GetFileName(fileName), "");

        // Recurse into subdirectories of this directory.
        string[] subdirectoryEntries = Directory.GetDirectories(pathname);

        foreach (var subdirectory in subdirectoryEntries)
            AddDirectory(method, subdirectory, foldername);
    }

    /// <summary>
    /// Updates central directory (if pertinent) and close the Zip storage
    /// </summary>
    /// <remarks>This is a required step, unless automatic dispose is used</remarks>
    public void Close()
    {
        if (_access != FileAccess.Read)
        {
            var centralOffset = (uint)_zipFileStream!.Position;
            uint centralSize = 0;

            if (_centralDirImage != null)
                _zipFileStream.Write(_centralDirImage, 0, _centralDirImage.Length);

            foreach (var t in _files)
            {
                var pos = _zipFileStream.Position;
                WriteCentralDirRecord(t);
                centralSize += (uint)(_zipFileStream.Position - pos);
            }

            WriteEndRecord(_centralDirImage != null ? centralSize + (uint)_centralDirImage.Length : centralSize, centralOffset);
        }

        if (_zipFileStream == null || _leaveOpen)
            return;

        _zipFileStream.Flush();
        _zipFileStream.Dispose();
        _zipFileStream = null;
    }

    /// <summary>
    /// Read all the file records in the central directory
    /// </summary>
    /// <returns>List of all entries in directory</returns>
    public List<ZipFileEntry> ReadCentralDir()
    {
        if (_centralDirImage == null)
            throw new InvalidOperationException("Central directory currently does not exist");

        var result = new List<ZipFileEntry>();

        for (var pointer = 0;
             pointer < _centralDirImage.Length;)
        {
            var signature = BitConverter.ToUInt32(_centralDirImage, pointer);

            if (signature != 0x02014b50)
                break;

            var encodeUtf8 = (BitConverter.ToUInt16(_centralDirImage, pointer + 8) & 0x0800) != 0;
            var method = BitConverter.ToUInt16(_centralDirImage, pointer + 10);
            var modifyTime = BitConverter.ToUInt32(_centralDirImage, pointer + 12);
            var crc32 = BitConverter.ToUInt32(_centralDirImage, pointer + 16);
            long comprSize = BitConverter.ToUInt32(_centralDirImage, pointer + 20);
            long fileSize = BitConverter.ToUInt32(_centralDirImage, pointer + 24);
            var filenameSize = BitConverter.ToUInt16(_centralDirImage, pointer + 28);
            var extraSize = BitConverter.ToUInt16(_centralDirImage, pointer + 30);
            var commentSize = BitConverter.ToUInt16(_centralDirImage, pointer + 32);
            var headerOffset = BitConverter.ToUInt32(_centralDirImage, pointer + 42);
            var headerSize = (uint)(46 + filenameSize + extraSize + commentSize);
            var modifyTimeDt = DosTimeToDateTime(modifyTime) ?? DateTime.Now;

            var encoder = encodeUtf8 ? Encoding.UTF8 : DefaultEncoding;

            var zfe = new ZipFileEntry
            {
                Method = (ZipCompression)method,
                FilenameInZip = encoder.GetString(_centralDirImage, pointer + 46, filenameSize),
                FileOffset = GetFileOffset(headerOffset),
                FileSize = fileSize,
                CompressedSize = comprSize,
                HeaderOffset = headerOffset,
                HeaderSize = headerSize,
                Crc32 = crc32,
                ModifyTime = modifyTimeDt,
                CreationTime = modifyTimeDt,
                AccessTime = DateTime.Now
            };

            if (commentSize > 0)
                zfe.Comment = encoder.GetString(_centralDirImage, pointer + 46 + filenameSize + extraSize, commentSize);

            if (extraSize > 0)
                ReadExtraInfo(_centralDirImage, pointer + 46 + filenameSize, zfe);

            result.Add(zfe);
            pointer += 46 + filenameSize + extraSize + commentSize;
        }

        return result;
    }

    /// <summary>
    /// Copy the contents of a stored file into a physical file
    /// </summary>
    /// <param name="zfe">Entry information of file to extract</param>
    /// <param name="filename">Name of file to store uncompressed data</param>
    /// <returns>True if success, false if not.</returns>
    /// <remarks>Unique compression methods are Store and Deflate</remarks>
    public bool ExtractFile(ZipFileEntry zfe, string filename)
    {
        // Make sure the parent directory exist
        var path = Path.GetDirectoryName(filename);

        if (!Directory.Exists(path))
            if (path != null)
                Directory.CreateDirectory(path);

        // Check if it is a directory. If so, do nothing.
        if (Directory.Exists(filename))
            return true;

        bool result;

        using (var output = new FileStream(filename, FileMode.Create, FileAccess.Write))
            result = ExtractFile(zfe, output);

        if (!result)
            return result;

        File.SetCreationTime(filename, zfe.CreationTime);
        File.SetLastWriteTime(filename, zfe.ModifyTime);
        File.SetLastAccessTime(filename, zfe.AccessTime);

        return result;
    }

    /// <summary>
    /// Copy the contents of a stored file into an opened stream
    /// </summary>
    /// <remarks>Same parameters and return value as ExtractFileAsync</remarks>
    public bool ExtractFile(ZipFileEntry zfe, Stream stream)
    {
        return Task.Run(() => ExtractFileAsync(zfe, stream)).Result;
    }

    /// <summary>
    /// Copy the contents of a stored file into an opened stream
    /// </summary>
    /// <param name="zfe">Entry information of file to extract</param>
    /// <param name="stream">Stream to store the uncompressed data</param>
    /// <returns>True if success, false if not.</returns>
    /// <remarks>Unique compression methods are Store and Deflate</remarks>
    public async Task<bool> ExtractFileAsync(ZipFileEntry zfe, Stream stream)
    {
        if (!stream.CanWrite)
            throw new InvalidOperationException("Stream cannot be written");

        // check signature
        var signature = new byte[4];
        _zipFileStream!.Seek(zfe.HeaderOffset, SeekOrigin.Begin);

        await _zipFileStream.ReadAsync(signature, 0, 4);

        if (BitConverter.ToUInt32(signature, 0) != 0x04034b50)
            return false;

        // Select input stream for inflating or just reading
        Stream inStream;

        switch (zfe.Method)
        {
            case ZipCompression.Store:
                inStream = _zipFileStream;

                break;
            case ZipCompression.Deflate:
                inStream = new DeflateStream(_zipFileStream, CompressionMode.Decompress, true);

                break;
            default:
                return false;
        }

        // Buffered copy
        var buffer = new byte[65535];
        _zipFileStream.Seek(zfe.FileOffset, SeekOrigin.Begin);
        var bytesPending = zfe.FileSize;

        while (bytesPending > 0)
        {
            var bytesRead = await inStream.ReadAsync(buffer, 0, (int)Math.Min(bytesPending, buffer.Length));
            await stream.WriteAsync(buffer, 0, bytesRead);

            bytesPending -= (uint)bytesRead;
        }

        await stream.FlushAsync();

        if (zfe.Method == ZipCompression.Deflate)
            await inStream.DisposeAsync();

        return true;
    }

    /// <summary>
    /// Copy the contents of a stored file into a byte array
    /// </summary>
    /// <param name="zfe">Entry information of file to extract</param>
    /// <param name="file">Byte array with uncompressed data</param>
    /// <returns>True if success, false if not.</returns>
    /// <remarks>Unique compression methods are Store and Deflate</remarks>
    public bool ExtractFile(ZipFileEntry zfe, out byte[]? file)
    {
        using var ms = new MemoryStream();

        if (ExtractFile(zfe, ms))
        {
            file = ms.ToArray();

            return true;
        }

        file = null;

        return false;
    }

    /// <summary>
    /// Removes one of many files in storage. It creates a new Zip file.
    /// </summary>
    /// <param name="zip">Reference to the current Zip object</param>
    /// <param name="zfes">List of Entries to remove from storage</param>
    /// <returns>True if success, false if not</returns>
    /// <remarks>This method only works for storage of type FileStream</remarks>
    public static bool RemoveEntries(ref ZipFileProcessor zip, List<ZipFileEntry> zfes)
    {
        if (zip._zipFileStream is not FileStream)
            throw new InvalidOperationException("RemoveEntries is allowed just over streams of type FileStream");

        //Get full list of entries
        var fullList = zip.ReadCentralDir();

        //In order to delete we need to create a copy of the zip file excluding the selected items
        var tempZipName = Path.GetRandomFileName();
        var tempEntryName = Path.GetRandomFileName();

        try
        {
            var tempZip = Create(tempZipName, string.Empty);

            foreach (var zfe in fullList.Where(zfe => !zfes.Contains(zfe)))
            {
                if (zip.ExtractFile(zfe, tempEntryName))
                    tempZip.AddFile(zfe.Method, tempEntryName, zfe.FilenameInZip!, zfe.Comment);
            }

            zip.Close();
            tempZip.Close();

            File.Delete(zip._fileName!);
            File.Move(tempZipName, zip._fileName!);

            zip = Open(zip._fileName!, zip._access);
        }
        catch
        {
            return false;
        }
        finally
        {
            if (File.Exists(tempZipName))
                File.Delete(tempZipName);

            if (File.Exists(tempEntryName))
                File.Delete(tempEntryName);
        }

        return true;
    }

    /// <summary>
    /// Calculate the file offset by reading the corresponding local header
    /// </summary>
    /// <param name="headerOffset">offset</param>
    /// <returns>File offset</returns>
    private uint GetFileOffset(uint headerOffset)
    {
        var buffer = new byte[2];

        _zipFileStream?.Seek(headerOffset + 26, SeekOrigin.Begin);
        _zipFileStream?.Read(buffer, 0, 2);
        var filenameSize = BitConverter.ToUInt16(buffer, 0);
        _zipFileStream?.Read(buffer, 0, 2);
        var extraSize = BitConverter.ToUInt16(buffer, 0);

        return (uint)(30 + filenameSize + extraSize + headerOffset);
    }

    /// <summary>
    /// Local file header:
    /// local file header signature     4 bytes  (0x04034b50)
    /// version needed to extract       2 bytes
    ///     general purpose bit flag        2 bytes
    ///     compression method              2 bytes
    ///     last mod file time              2 bytes
    ///     last mod file date              2 bytes
    ///     crc-32                          4 bytes
    ///     compressed size                 4 bytes
    ///     uncompressed size               4 bytes
    ///     filename length                 2 bytes
    ///     extra field length              2 bytes
    ///     filename (variable size)
    /// extra field (variable size)
    /// </summary>
    /// <param name="zfe"></param>
    private void WriteLocalHeader(ZipFileEntry zfe)
    {
        var pos = _zipFileStream!.Position;
        var encoder = zfe.EncodeUTF8 ? Encoding.UTF8 : DefaultEncoding;
        var encodedFilename = encoder.GetBytes(zfe.FilenameInZip!);
        var extraInfo = CreateExtraInfo(zfe);

        _zipFileStream.Write(new byte[]
                             {
                                 80, 75, 3, 4, 20, 0
                             },
                             0,
                             6); // No extra header

        _zipFileStream.Write(BitConverter.GetBytes((ushort)(zfe.EncodeUTF8 ? 0x0800 : 0)), 0, 2); // filename and comment encoding
        _zipFileStream.Write(BitConverter.GetBytes((ushort)zfe.Method), 0, 2);                    // zipping method
        _zipFileStream.Write(BitConverter.GetBytes(DateTimeToDosTime(zfe.ModifyTime)), 0, 4);     // zipping date and time

        _zipFileStream.Write(new byte[]
                             {
                                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                 0, 0
                             },
                             0,
                             12); // unused CRC, un/compressed size, updated later

        _zipFileStream.Write(BitConverter.GetBytes((ushort)encodedFilename.Length), 0, 2); // filename length
        _zipFileStream.Write(BitConverter.GetBytes((ushort)extraInfo.Length), 0, 2);       // extra length

        _zipFileStream.Write(encodedFilename, 0, encodedFilename.Length);
        _zipFileStream.Write(extraInfo, 0, extraInfo.Length);
        zfe.HeaderSize = (uint)(_zipFileStream.Position - pos);
    }

    /// <summary>
    /// Central directory's File header:
    /// central file header signature   4 bytes  (0x02014b50)
    /// version made by                 2 bytes
    ///     version needed to extract       2 bytes
    ///     general purpose bit flag        2 bytes
    ///     compression method              2 bytes
    ///     last mod file time              2 bytes
    ///     last mod file date              2 bytes
    ///     crc-32                          4 bytes
    ///     compressed size                 4 bytes
    ///     uncompressed size               4 bytes
    ///     filename length                 2 bytes
    ///     extra field length              2 bytes
    ///     file comment length             2 bytes
    ///     disk number start               2 bytes
    /// internal file attributes        2 bytes
    ///     external file attributes        4 bytes
    ///     relative offset of local header 4 bytes
    ///     filename (variable size)
    /// extra field (variable size)
    /// file comment (variable size)
    /// </summary>
    /// <param name="zfe"></param>
    private void WriteCentralDirRecord(ZipFileEntry zfe)
    {
        var encoder = zfe.EncodeUTF8 ? Encoding.UTF8 : DefaultEncoding;
        var encodedFilename = encoder.GetBytes(zfe.FilenameInZip!);
        var encodedComment = encoder.GetBytes(zfe.Comment!);
        var extraInfo = CreateExtraInfo(zfe);

        _zipFileStream!.Write(new byte[]
                              {
                                  80, 75, 1, 2, 23, 0xB, 20, 0
                              },
                              0,
                              8);

        _zipFileStream.Write(BitConverter.GetBytes((ushort)(zfe.EncodeUTF8 ? 0x0800 : 0)), 0, 2); // filename and comment encoding
        _zipFileStream.Write(BitConverter.GetBytes((ushort)zfe.Method), 0, 2);                    // zipping method
        _zipFileStream.Write(BitConverter.GetBytes(DateTimeToDosTime(zfe.ModifyTime)), 0, 4);     // zipping date and time
        _zipFileStream.Write(BitConverter.GetBytes(zfe.Crc32), 0, 4);                             // file CRC
        _zipFileStream.Write(BitConverter.GetBytes(Get32BitSize(zfe.CompressedSize)), 0, 4);      // compressed file size
        _zipFileStream.Write(BitConverter.GetBytes(Get32BitSize(zfe.FileSize)), 0, 4);            // uncompressed file size
        _zipFileStream.Write(BitConverter.GetBytes((ushort)encodedFilename.Length), 0, 2);        // Filename in zip
        _zipFileStream.Write(BitConverter.GetBytes((ushort)extraInfo.Length), 0, 2);              // extra length
        _zipFileStream.Write(BitConverter.GetBytes((ushort)encodedComment.Length), 0, 2);

        _zipFileStream.Write(BitConverter.GetBytes((ushort)0), 0, 2);                      // disk=0
        _zipFileStream.Write(BitConverter.GetBytes((ushort)0), 0, 2);                      // file type: binary
        _zipFileStream.Write(BitConverter.GetBytes((ushort)0), 0, 2);                      // Internal file attributes
        _zipFileStream.Write(BitConverter.GetBytes((ushort)0x8100), 0, 2);                 // External file attributes (normal/readable)
        _zipFileStream.Write(BitConverter.GetBytes(Get32BitSize(zfe.HeaderOffset)), 0, 4); // Offset of header

        _zipFileStream.Write(encodedFilename, 0, encodedFilename.Length);
        _zipFileStream.Write(extraInfo, 0, extraInfo.Length);
        _zipFileStream.Write(encodedComment, 0, encodedComment.Length);
    }

    /// <summary>
    /// Get 32 Bit Size
    /// </summary>
    /// <param name="size">Size</param>
    /// <returns>Size</returns>
    private uint Get32BitSize(long size)
    {
        return size >= 0xFFFFFFFF ? 0xFFFFFFFF : (uint)size;
    }

    /// <summary>
    /// Zip64 end of central directory record
    /// zip64 end of central dir
    ///     signature                       4 bytes  (0x06064b50)
    /// size of zip64 end of central
    /// directory record                8 bytes
    ///     version made by                 2 bytes
    ///     version needed to extract       2 bytes
    ///     number of this disk             4 bytes
    ///     number of the disk with the
    ///     start of the central directory  4 bytes
    ///     total number of entries in the
    ///     central directory on this disk  8 bytes
    ///     total number of entries in the
    ///     central directory               8 bytes
    ///     size of the central directory   8 bytes
    ///     offset of start of central
    /// directory with respect to
    /// the starting disk number        8 bytes
    ///     zip64 extensible data sector    (variable size)
    ///
    /// Zip64 end of central directory locator
    /// zip64 end of central dir locator
    /// signature                       4 bytes  (0x07064b50)
    /// number of the disk with the
    /// start of the zip64 end of
    /// central directory               4 bytes
    ///     relative offset of the zip64
    /// end of central directory record 8 bytes
    ///     total number of disks           4 bytes
    ///     End of central dir record:
    /// end of central dir signature    4 bytes  (0x06054b50)
    /// number of this disk             2 bytes
    ///     number of the disk with the
    ///     start of the central directory  2 bytes
    ///     total number of entries in
    /// the central dir on this disk    2 bytes
    ///     total number of entries in
    /// the central dir                 2 bytes
    ///     size of the central directory   4 bytes
    ///     offset of start of central
    /// directory with respect to
    /// the starting disk number        4 bytes
    ///     zipfile comment length          2 bytes
    ///     zipfile comment (variable size)
    /// </summary>
    /// <param name="_size"></param>
    /// <param name="_offset"></param>
    private void WriteEndRecord(long _size, long _offset)
    {
        var dirOffset = _zipFileStream!.Length;

        // Zip64 end of central directory record
        _zipFileStream.Position = dirOffset;

        _zipFileStream.Write(new byte[]
                             {
                                 80, 75, 6, 6
                             },
                             0,
                             4);

        _zipFileStream.Write(BitConverter.GetBytes((long)44), 0, 8);                      // size of zip64 end of central directory
        _zipFileStream.Write(BitConverter.GetBytes((ushort)45), 0, 2);                    // version made by
        _zipFileStream.Write(BitConverter.GetBytes((ushort)45), 0, 2);                    // version needed to extract
        _zipFileStream.Write(BitConverter.GetBytes((uint)0), 0, 4);                       // current disk
        _zipFileStream.Write(BitConverter.GetBytes((uint)0), 0, 4);                       // start of central directory
        _zipFileStream.Write(BitConverter.GetBytes(_files.Count + _existingFiles), 0, 8); // total number of entries in the central directory in disk
        _zipFileStream.Write(BitConverter.GetBytes(_files.Count + _existingFiles), 0, 8); // total number of entries in the central directory
        _zipFileStream.Write(BitConverter.GetBytes(_size), 0, 8);                         // size of the central directory
        _zipFileStream.Write(BitConverter.GetBytes(_offset), 0, 8);                       // offset of start of central directory with respect to the starting disk number

        // Zip64 end of central directory locator
        _zipFileStream.Write(new byte[]
                             {
                                 80, 75, 6, 7
                             },
                             0,
                             4);

        _zipFileStream.Write(BitConverter.GetBytes((uint)0), 0, 4);   // number of the disk
        _zipFileStream.Write(BitConverter.GetBytes(dirOffset), 0, 8); // relative offset of the zip64 end of central directory record
        _zipFileStream.Write(BitConverter.GetBytes((uint)1), 0, 4);   // total number of disks

        var encoder = EncodeUTF8 ? Encoding.UTF8 : DefaultEncoding;
        var encodedComment = encoder.GetBytes(_comment);

        _zipFileStream.Write(new byte[]
                             {
                                 80, 75, 5, 6, 0, 0, 0, 0
                             },
                             0,
                             8);

        _zipFileStream.Write(new byte[]
                             {
                                 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                                 0xFF, 0xFF
                             },
                             0,
                             12);

        _zipFileStream.Write(BitConverter.GetBytes((ushort)encodedComment.Length), 0, 2);
        _zipFileStream.Write(encodedComment, 0, encodedComment.Length);
    }

    /// <summary>
    /// Copies all the source file into the zip storage
    /// </summary>
    /// <param name="zfe">zfe</param>
    /// <param name="source">source</param>
    /// <returns>Compression type</returns>
    private async Task<ZipCompression> StoreAsync(ZipFileEntry zfe, Stream source)
    {
        var buffer = new byte[16384];
        int bytesRead;
        uint totalRead = 0;

        var posStart = _zipFileStream!.Position;
        var sourceStart = source.CanSeek ? source.Position : 0;

        var outStream = zfe.Method == ZipCompression.Store
                            ? _zipFileStream
                            : new DeflateStream(_zipFileStream, CompressionMode.Compress, true);

        zfe.Crc32 = 0xffffffff;

        do
        {
            bytesRead = await source.ReadAsync(buffer, 0, buffer.Length);

            if (bytesRead > 0)
                await outStream.WriteAsync(buffer, 0, bytesRead);

            for (uint i = 0;
                 i < bytesRead;
                 i++)
                zfe.Crc32 = CrcTable![(zfe.Crc32 ^ buffer[i]) & 0xFF] ^ (zfe.Crc32 >> 8);

            totalRead += (uint)bytesRead;
        } while (bytesRead > 0);

        await outStream.FlushAsync();

        if (zfe.Method == ZipCompression.Deflate)
            await outStream.DisposeAsync();

        zfe.Crc32 ^= 0xFFFFFFFF;
        zfe.FileSize = totalRead;
        zfe.CompressedSize = (uint)(_zipFileStream.Position - posStart);

        // Verify for real compression
        if (zfe.Method != ZipCompression.Deflate || ForceDeflating || !source.CanSeek || zfe.CompressedSize <= zfe.FileSize)
            return zfe.Method;

        // Start operation again with Store algorithm
        zfe.Method = ZipCompression.Store;
        _zipFileStream.Position = posStart;
        _zipFileStream.SetLength(posStart);
        source.Position = sourceStart;

        return await StoreAsync(zfe, source);
    }

    /// <summary>
    /// DOS Date and time:
    /// MS-DOS date. The date is a packed value with the following format. Bits Description
    /// 0-4 Day of the month (131)
    ///     5-8 Month (1 = January, 2 = February, and so on)
    /// 9-15 Year offset from 1980 (add 1980 to get actual year)
    /// MS-DOS time. The time is a packed value with the following format. Bits Description
    /// 0-4 Second divided by 2
    /// 5-10 Minute (059)
    ///     11-15 Hour (023 on a 24-hour clock)
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    private uint DateTimeToDosTime(DateTime dt)
    {
        return (uint)((dt.Second / 2) | (dt.Minute << 5) | (dt.Hour << 11) | (dt.Day << 16) | (dt.Month << 21) | ((dt.Year - 1980) << 25));
    }

    /// <summary>
    /// Time to date time
    /// </summary>
    private DateTime? DosTimeToDateTime(uint dt)
    {
        var year = (int)(dt >> 25) + 1980;
        var month = (int)(dt >> 21) & 15;
        var day = (int)(dt >> 16) & 31;
        var hours = (int)(dt >> 11) & 31;
        var minutes = (int)(dt >> 5) & 63;
        var seconds = (int)(dt & 31) * 2;

        if (month == 0 || day == 0 || year >= 2107)
            return DateTime.Now;

        return new DateTime(year,
                            month,
                            day,
                            hours,
                            minutes,
                            seconds,
                            DateTimeKind.Unspecified);
    }

    /// <summary>
    /// Create extract info
    /// </summary>
    /// <param name="zfe">zfe</param>
    /// <returns>info</returns>
    private byte[] CreateExtraInfo(ZipFileEntry zfe)
    {
        var buffer = new byte[36 + 36];

        BitConverter.GetBytes((ushort)0x0001)
                    .CopyTo(buffer, 0); // ZIP64 Information

        BitConverter.GetBytes((ushort)32)
                    .CopyTo(buffer, 2); // Length

        BitConverter.GetBytes((ushort)1)
                    .CopyTo(buffer, 8); // Tag 1

        BitConverter.GetBytes((ushort)24)
                    .CopyTo(buffer, 10); // Size 1

        BitConverter.GetBytes(zfe.FileSize)
                    .CopyTo(buffer, 12); // MTime

        BitConverter.GetBytes(zfe.CompressedSize)
                    .CopyTo(buffer, 20); // ATime

        BitConverter.GetBytes(zfe.HeaderOffset)
                    .CopyTo(buffer, 28); // CTime

        BitConverter.GetBytes((ushort)0x000A)
                    .CopyTo(buffer, 36); // NTFS FileTime

        BitConverter.GetBytes((ushort)32)
                    .CopyTo(buffer, 38); // Length

        BitConverter.GetBytes((ushort)1)
                    .CopyTo(buffer, 44); // Tag 1

        BitConverter.GetBytes((ushort)24)
                    .CopyTo(buffer, 46); // Size 1

        BitConverter.GetBytes(zfe.ModifyTime.ToFileTime())
                    .CopyTo(buffer, 48); // MTime

        BitConverter.GetBytes(zfe.AccessTime.ToFileTime())
                    .CopyTo(buffer, 56); // ATime

        BitConverter.GetBytes(zfe.CreationTime.ToFileTime())
                    .CopyTo(buffer, 64); // CTime

        return buffer;
    }

    /// <summary>
    /// Read extract info
    /// </summary>
    /// <param name="buffer">buffer</param>
    /// <param name="offset">pffset</param>
    /// <param name="zfe">zfe</param>
    private void ReadExtraInfo(byte[] buffer, int offset, ZipFileEntry zfe)
    {
        if (buffer.Length < 4)
            return;

        var pos = offset;

        while (pos < buffer.Length - 4)
        {
            uint extraId = BitConverter.ToUInt16(buffer, pos);
            uint length = BitConverter.ToUInt16(buffer, pos + 2);

            uint tag;

            uint size;

            switch (extraId)
            {
                // ZIP64 Information
                case 0x0001:
                {
                    tag = BitConverter.ToUInt16(buffer, pos + 8);
                    size = BitConverter.ToUInt16(buffer, pos + 10);

                    if (tag == 1 && size >= 24)
                    {
                        if (zfe.FileSize == 0xFFFFFFFF)
                            zfe.FileSize = BitConverter.ToInt64(buffer, pos + 12);

                        if (zfe.CompressedSize == 0xFFFFFFFF)
                            zfe.CompressedSize = BitConverter.ToInt64(buffer, pos + 20);

                        if (zfe.HeaderOffset == 0xFFFFFFFF)
                            zfe.HeaderOffset = BitConverter.ToInt64(buffer, pos + 28);
                    }

                    break;
                }

                // NTFS FileTime
                case 0x000A:
                {
                    tag = BitConverter.ToUInt16(buffer, pos + 8);
                    size = BitConverter.ToUInt16(buffer, pos + 10);

                    if (tag == 1 && size == 24)
                    {
                        zfe.ModifyTime = DateTime.FromFileTime(BitConverter.ToInt64(buffer, pos + 12));
                        zfe.AccessTime = DateTime.FromFileTime(BitConverter.ToInt64(buffer, pos + 20));
                        zfe.CreationTime = DateTime.FromFileTime(BitConverter.ToInt64(buffer, pos + 28));
                    }

                    break;
                }
            }

            pos += (int)length + 4;
        }
    }

    /// <summary>
    /// The 'magic number' for the CRC is 0xdebb20e3.
    /// The proper CRC pre and post conditioning is used, meaning that the CRC register is
    /// pre-conditioned with all ones (a starting value of 0xffffffff) and the value is post-conditioned by
    /// taking the one's complement of the CRC residual.
    ///     If bit 3 of the general purpose flag is set, this field is set to zero in the local header and the correct
    /// value is put in the data descriptor and in the central directory.
    /// </summary>
    /// <param name="zfe"></param>
    private void UpdateCrcAndSizes(ZipFileEntry zfe)
    {
        var lastPos = _zipFileStream!.Position; // remember position

        _zipFileStream.Position = zfe.HeaderOffset + 8;
        _zipFileStream.Write(BitConverter.GetBytes((ushort)zfe.Method), 0, 2); // zipping method

        _zipFileStream.Position = zfe.HeaderOffset + 14;
        _zipFileStream.Write(BitConverter.GetBytes(zfe.Crc32), 0, 4);                        // Update CRC
        _zipFileStream.Write(BitConverter.GetBytes(Get32BitSize(zfe.CompressedSize)), 0, 4); // Compressed size
        _zipFileStream.Write(BitConverter.GetBytes(Get32BitSize(zfe.FileSize)), 0, 4);       // Uncompressed size

        _zipFileStream.Position = lastPos; // restore position
    }

    /// <summary>
    /// Replaces backslashes with slashes to store in zip header
    /// </summary>
    /// <param name="filename">File name</param>
    private string NormalizedFilename(string filename)
    {
        var validFileName = filename.Replace('\\', '/');

        var pos = validFileName.IndexOf(':');

        if (pos >= 0)
            validFileName = validFileName.Remove(0, pos + 1);

        return validFileName.Trim('/');
    }

    /// <summary>
    /// Reads the end-of-central-directory record
    /// </summary>
    private bool ReadFileInfo()
    {
        if (_zipFileStream!.Length < 22)
            return false;

        _zipFileStream.Seek(0, SeekOrigin.Begin);

        {
            var brCheckHeaderSig = new BinaryReader(_zipFileStream);

            {
                var headerSig = brCheckHeaderSig.ReadUInt32();

                if (headerSig != 0x04034b50)

                    // not PK.. signature header
                    return false;
            }
        }

        try
        {
            _zipFileStream.Seek(-17, SeekOrigin.End);
            var br = new BinaryReader(_zipFileStream);

            do
            {
                _zipFileStream.Seek(-5, SeekOrigin.Current);
                var sig = br.ReadUInt32();

                if (sig != 0x06054b50) continue; // It is central dir

                var dirPosition = _zipFileStream.Position - 4;

                _zipFileStream.Seek(6, SeekOrigin.Current);

                long entries = br.ReadUInt16();
                long centralSize = br.ReadInt32();
                long centralDirOffset = br.ReadUInt32();
                var commentSize = br.ReadUInt16();

                var commentPosition = _zipFileStream.Position;

                if (centralDirOffset == 0xffffffff) // It is a Zip64 file
                {
                    _zipFileStream.Position = dirPosition - 20;

                    sig = br.ReadUInt32();

                    if (sig != 0x07064b50) // Not a Zip64 central dir locator
                        return false;

                    _zipFileStream.Seek(4, SeekOrigin.Current);

                    var dir64Position = br.ReadInt64();
                    _zipFileStream.Position = dir64Position;

                    sig = br.ReadUInt32();

                    if (sig != 0x06064b50) // Not a Zip64 central dir record
                        return false;

                    _zipFileStream.Seek(28, SeekOrigin.Current);
                    entries = br.ReadInt64();
                    centralSize = br.ReadInt64();
                    centralDirOffset = br.ReadInt64();
                }

                // check if comment field is the very last data in file
                if (commentPosition + commentSize != _zipFileStream.Length)
                    return false;

                // Copy entire central directory to a memory buffer
                _existingFiles = entries;
                _centralDirImage = new byte[centralSize];
                _zipFileStream.Seek(centralDirOffset, SeekOrigin.Begin);
                _zipFileStream.Read(_centralDirImage, 0, (int)centralSize);

                // Leave the pointer at the begining of central dir, to append new files
                _zipFileStream.Seek(centralDirOffset, SeekOrigin.Begin);

                return true;
            } while (_zipFileStream.Position > 0);
        }
        catch
        {
            // ignored
        }

        return false;
    }

    /// <summary>
    /// Closes the Zip file stream
    /// </summary>
    public void Dispose()
    {
        Dispose(true);

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Destructor
    /// </summary>
    ~ZipFileProcessor()
    {
        Dispose(false);
    }

    /// <summary>
    /// Dispose
    /// </summary>
    /// <param name="disposing">disposed flag</param>
    private void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
                Close();

            _isDisposed = true;
        }
    }
}