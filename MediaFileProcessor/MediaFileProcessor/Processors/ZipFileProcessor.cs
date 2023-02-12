using System.IO.Compression;
using System.Text;
namespace MediaFileProcessor.Processors;

/// <summary>
/// Unique class for compression/decompression file. Represents a Zip file.
/// </summary>
public class ZipFileProcessor : IDisposable
{
    /// <summary>
    /// Compression method enumeration
    /// </summary>
    public enum Compression : ushort
    {
        /// <summary>Uncompressed storage</summary>
        Store = 0,
        /// <summary>Deflate compression method</summary>
        Deflate = 8
    }

    /// <summary>
    /// Represents an entry in Zip file directory
    /// </summary>
    public class ZipFileEntry
    {
        /// <summary>Compression method</summary>
        public Compression Method;
        /// <summary>Full path and filename as stored in Zip</summary>
        public string? FilenameInZip;
        /// <summary>Original file size</summary>
        public long FileSize;
        /// <summary>Compressed file size</summary>
        public long CompressedSize;
        /// <summary>Offset of header information inside Zip storage</summary>
        public long HeaderOffset;
        /// <summary>Offset of file inside Zip storage</summary>
        public long FileOffset;
        /// <summary>Size of header information</summary>
        public uint HeaderSize;
        /// <summary>32-bit checksum of entire file</summary>
        public uint Crc32;
        /// <summary>Last modification time of file</summary>
        public DateTime ModifyTime;
        /// <summary>Creation time of file</summary>
        public DateTime CreationTime;
        /// <summary>Last access time of file</summary>
        public DateTime AccessTime;
        /// <summary>User comment for file</summary>
        public string? Comment;
        /// <summary>True if UTF8 encoding for filename and comments, false if default (CP 437)</summary>
        public bool EncodeUTF8;

        /// <summary>Overriden method</summary>
        /// <returns>Filename in Zip</returns>
        public override string? ToString()
        {
            return FilenameInZip;
        }
    }

    #region Public fields
    /// <summary>True if UTF8 encoding for filename and comments, false if default (CP 437)</summary>
    public bool EncodeUTF8 = false;
    /// <summary>Force deflate algotithm even if it inflates the stored file. Off by default.</summary>
    public bool ForceDeflating = false;
    #endregion

    #region Private fields
    // List of files to store
    private readonly List<ZipFileEntry> Files = new ();

    // Filename of storage file
    private string? FileName;

    // Stream object of storage file
    private Stream? ZipFileStream;

    // General comment
    private string Comment = string.Empty;

    // Central dir image
    private byte[]? CentralDirImage;

    // Existing files in zip
    private long ExistingFiles;

    // File access for Open method
    private FileAccess Access;

    // leave the stream open after the ZipStorer object is disposed
    private bool leaveOpen;

    // Dispose control
    private bool isDisposed;

    // Static CRC32 Table
    private static uint[]? CrcTable;

    // Default filename encoder
    private static Encoding DefaultEncoding = Encoding.ASCII;
    #endregion

    #region Public methods
    // Static constructor. Just invoked once in order to create the CRC32 lookup table.
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
                    c = 3988292384 ^ (c >> 1);
                else
                    c >>= 1;
            }
            CrcTable[i] = c;
        }
    }

    /// <summary>
    /// Method to create a new storage file
    /// </summary>
    /// <param name="_filename">Full path of Zip file to create</param>
    /// <param name="_comment">General comment for Zip file</param>
    /// <returns>A valid ZipStorer object</returns>
    public static ZipFileProcessor Create(string _filename, string? _comment = null)
    {
        Stream stream = new FileStream(_filename, FileMode.Create, FileAccess.ReadWrite);

        var zip = Create(stream, _comment);
        zip.Comment = _comment ?? string.Empty;
        zip.FileName = _filename;

        return zip;
    }

    /// <summary>
    /// Method to create a new zip storage in a stream
    /// </summary>
    /// <param name="_stream"></param>
    /// <param name="_comment"></param>
    /// <param name="_leaveOpen">true to leave the stream open after the ZipStorer object is disposed; otherwise, false (default).</param>
    /// <returns>A valid ZipStorer object</returns>
    public static ZipFileProcessor Create(Stream _stream, string? _comment = null, bool _leaveOpen = false)
    {
        var zip = new ZipFileProcessor
        {
            Comment = _comment ?? string.Empty,
            ZipFileStream = _stream,
            Access = FileAccess.Write,
            leaveOpen = _leaveOpen
        };

        return zip;
    }

    /// <summary>
    /// Method to open an existing storage file
    /// </summary>
    /// <param name="_filename">Full path of Zip file to open</param>
    /// <param name="_access">File access mode as used in FileStream constructor</param>
    /// <returns>A valid ZipStorer object</returns>
    public static ZipFileProcessor Open(string _filename, FileAccess _access)
    {
        Stream? stream = null;
        ZipFileProcessor? zip = null;

        try
        {
            stream = new FileStream(_filename, FileMode.Open, _access == FileAccess.Read ? FileAccess.Read : FileAccess.ReadWrite);

            zip = Open(stream, _access);
            zip.FileName = _filename;
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
    /// <param name="_stream">Already opened stream with zip contents</param>
    /// <param name="_access">File access mode for stream operations</param>
    /// <param name="_leaveOpen">true to leave the stream open after the ZipStorer object is disposed; otherwise, false (default).</param>
    /// <returns>A valid ZipStorer object</returns>
    public static ZipFileProcessor Open(Stream _stream, FileAccess _access, bool _leaveOpen = false)
    {
        if (!_stream.CanSeek && _access != FileAccess.Read)
            throw new InvalidOperationException("Stream cannot seek");

        var zip = new ZipFileProcessor
        {
            ZipFileStream = _stream,
            Access = _access,
            leaveOpen = _leaveOpen
        };

        if (zip.ReadFileInfo())
            return zip;

        if (!_leaveOpen)
            zip.Close();

        throw new InvalidDataException();
    }

    /// <summary>
    /// Add full contents of a file into the Zip storage
    /// </summary>
    /// <param name="_method">Compression method</param>
    /// <param name="_pathname">Full path of file to add to Zip storage</param>
    /// <param name="_filenameInZip">Filename and path as desired in Zip directory</param>
    /// <param name="_comment">Comment for stored file</param>
    public ZipFileEntry AddFile(Compression _method, string _pathname, string _filenameInZip, string? _comment = null)
    {
        if (Access == FileAccess.Read)
            throw new InvalidOperationException("Writing is not alowed");

        using (var stream = new FileStream(_pathname, FileMode.Open, FileAccess.Read))
            return AddStream(_method, _filenameInZip, stream, File.GetLastWriteTime(_pathname), _comment);
    }

    /// <summary>
    /// Add full contents of a stream into the Zip storage
    /// </summary>
    /// <remarks>Same parameters and return value as AddStreamAsync()</remarks>
    public ZipFileEntry AddStream(Compression _method, string _filenameInZip, Stream _source, DateTime _modTime, string? _comment = null)
    {
#if NOASYNC
            return this.AddStreamAsync(_method, _filenameInZip, _source, _modTime, _comment);
#else
        return Task.Run(() => AddStreamAsync(_method, _filenameInZip, _source, _modTime, _comment))
                   .Result;
#endif
    }

    /// <summary>
    /// Add full contents of a stream into the Zip storage
    /// </summary>
    /// <param name="_method">Compression method</param>
    /// <param name="_filenameInZip">Filename and path as desired in Zip directory</param>
    /// <param name="_source">Stream object containing the data to store in Zip</param>
    /// <param name="_modTime">Modification time of the data to store</param>
    /// <param name="_comment">Comment for stored file</param>
#if NOASYNC
        private ZipFileEntry
#else
    public async Task<ZipFileEntry>
#endif
        AddStreamAsync(Compression _method, string _filenameInZip, Stream _source, DateTime _modTime, string? _comment = null)
    {
        if (Access == FileAccess.Read)
            throw new InvalidOperationException("Writing is not alowed");

        // Prepare the fileinfo
        var zfe = new ZipFileEntry
        {
            Method = _method,
            EncodeUTF8 = EncodeUTF8,
            FilenameInZip = NormalizedFilename(_filenameInZip),
            Comment = _comment ?? string.Empty,
            Crc32 = 0,                                    // to be updated later
            HeaderOffset = (uint)ZipFileStream!.Position, // offset within file of the start of this local record
            CreationTime = _modTime,
            ModifyTime = _modTime,
            AccessTime = _modTime
        };

        // Write local header
        WriteLocalHeader(zfe);
        zfe.FileOffset = (uint)ZipFileStream.Position;

        // Write file to zip (store)
#if NOASYNC
                Store(zfe, _source);
#else
        await Store(zfe, _source);
#endif

        _source.Close();
        UpdateCrcAndSizes(zfe);
        Files.Add(zfe);

        return zfe;
    }

    /// <summary>
    /// Add full contents of a directory into the Zip storage
    /// </summary>
    /// <param name="_method">Compression method</param>
    /// <param name="_pathname">Full path of directory to add to Zip storage</param>
    /// <param name="_pathnameInZip">Path name as desired in Zip directory</param>
    public void AddDirectory(Compression _method, string _pathname, string _pathnameInZip)
    {
        if (Access == FileAccess.Read)
            throw new InvalidOperationException("Writing is not allowed");

        var pos = _pathname.LastIndexOf(Path.DirectorySeparatorChar);
        var separator = Path.DirectorySeparatorChar.ToString();

        var foldername = pos >= 0 ? _pathname.Remove(0, pos + 1) : _pathname;

        if (!string.IsNullOrEmpty(_pathnameInZip))
            foldername = _pathnameInZip + foldername;

        if (!foldername.EndsWith(separator, StringComparison.CurrentCulture))
            foldername += separator;

        // this.AddStream(_method, foldername, null, File.GetLastWriteTime(_pathname), _comment);

        // Process the list of files found in the directory.
        var fileEntries = Directory.GetFiles(_pathname);

        foreach (var fileName in fileEntries)
            AddFile(_method, fileName, foldername + Path.GetFileName(fileName), "");

        // Recurse into subdirectories of this directory.
        string[] subdirectoryEntries = Directory.GetDirectories(_pathname);

        foreach (var subdirectory in subdirectoryEntries)
            AddDirectory(_method, subdirectory, foldername);
    }

    /// <summary>
    /// Updates central directory (if pertinent) and close the Zip storage
    /// </summary>
    /// <remarks>This is a required step, unless automatic dispose is used</remarks>
    public void Close()
    {
        if (Access != FileAccess.Read)
        {
            var centralOffset = (uint)ZipFileStream!.Position;
            uint centralSize = 0;

            if (CentralDirImage != null)
                ZipFileStream.Write(CentralDirImage, 0, CentralDirImage.Length);

            foreach(var t in Files)
            {
                var pos = ZipFileStream.Position;
                WriteCentralDirRecord(t);
                centralSize += (uint)(ZipFileStream.Position - pos);
            }

            if (CentralDirImage != null)
                WriteEndRecord(centralSize + (uint)CentralDirImage.Length, centralOffset);
            else
                WriteEndRecord(centralSize, centralOffset);
        }

        if (ZipFileStream != null && !leaveOpen)
        {
            ZipFileStream.Flush();
            ZipFileStream.Dispose();
            ZipFileStream = null;
        }
    }

    /// <summary>
    /// Read all the file records in the central directory
    /// </summary>
    /// <returns>List of all entries in directory</returns>
    public List<ZipFileEntry> ReadCentralDir()
    {
        if (CentralDirImage == null)
            throw new InvalidOperationException("Central directory currently does not exist");

        var result = new List<ZipFileEntry>();

        for (var pointer = 0;
             pointer < CentralDirImage.Length; )
        {
            var signature = BitConverter.ToUInt32(CentralDirImage, pointer);

            if (signature != 0x02014b50)
                break;

            var encodeUTF8 = (BitConverter.ToUInt16(CentralDirImage, pointer + 8) & 0x0800) != 0;
            var method = BitConverter.ToUInt16(CentralDirImage, pointer + 10);
            var modifyTime = BitConverter.ToUInt32(CentralDirImage, pointer + 12);
            var crc32 = BitConverter.ToUInt32(CentralDirImage, pointer + 16);
            long comprSize = BitConverter.ToUInt32(CentralDirImage, pointer + 20);
            long fileSize = BitConverter.ToUInt32(CentralDirImage, pointer + 24);
            var filenameSize = BitConverter.ToUInt16(CentralDirImage, pointer + 28);
            var extraSize = BitConverter.ToUInt16(CentralDirImage, pointer + 30);
            var commentSize = BitConverter.ToUInt16(CentralDirImage, pointer + 32);
            var headerOffset = BitConverter.ToUInt32(CentralDirImage, pointer + 42);
            var headerSize = (uint)( 46 + filenameSize + extraSize + commentSize);
            var modifyTimeDT = DosTimeToDateTime(modifyTime) ?? DateTime.Now;

            var encoder = encodeUTF8 ? Encoding.UTF8 : DefaultEncoding;

            var zfe = new ZipFileEntry
            {
                Method = (Compression)method,
                FilenameInZip = encoder.GetString(CentralDirImage, pointer + 46, filenameSize),
                FileOffset = GetFileOffset(headerOffset),
                FileSize = fileSize,
                CompressedSize = comprSize,
                HeaderOffset = headerOffset,
                HeaderSize = headerSize,
                Crc32 = crc32,
                ModifyTime = modifyTimeDT,
                CreationTime = modifyTimeDT,
                AccessTime = DateTime.Now
            };

            if (commentSize > 0)
                zfe.Comment = encoder.GetString(CentralDirImage, pointer + 46 + filenameSize + extraSize, commentSize);

            if (extraSize > 0)
                ReadExtraInfo(CentralDirImage, pointer + 46 + filenameSize, zfe);

            result.Add(zfe);
            pointer += 46 + filenameSize + extraSize + commentSize;
        }

        return result;
    }

    /// <summary>
    /// Copy the contents of a stored file into a physical file
    /// </summary>
    /// <param name="_zfe">Entry information of file to extract</param>
    /// <param name="_filename">Name of file to store uncompressed data</param>
    /// <returns>True if success, false if not.</returns>
    /// <remarks>Unique compression methods are Store and Deflate</remarks>
    public bool ExtractFile(ZipFileEntry _zfe, string _filename)
    {
        // Make sure the parent directory exist
        var path = Path.GetDirectoryName(_filename);

        if (!Directory.Exists(path))
            if (path != null)
                Directory.CreateDirectory(path);

        // Check if it is a directory. If so, do nothing.
        if (Directory.Exists(_filename))
            return true;

        bool result;

        using(var output = new FileStream(_filename, FileMode.Create, FileAccess.Write))
            result = ExtractFile(_zfe, output);

        if (result)
        {
            File.SetCreationTime(_filename, _zfe.CreationTime);
            File.SetLastWriteTime(_filename, _zfe.ModifyTime);
            File.SetLastAccessTime(_filename, _zfe.AccessTime);
        }

        return result;
    }

    /// <summary>
    /// Copy the contents of a stored file into an opened stream
    /// </summary>
    /// <remarks>Same parameters and return value as ExtractFileAsync</remarks>
    public bool ExtractFile(ZipFileEntry _zfe, Stream _stream)
    {
#if NOASYNC
            return this.ExtractFileAsync(_zfe, _stream);
#else
        return Task.Run(() => ExtractFileAsync(_zfe, _stream))
                   .Result;
#endif
    }

    /// <summary>
    /// Copy the contents of a stored file into an opened stream
    /// </summary>
    /// <param name="_zfe">Entry information of file to extract</param>
    /// <param name="_stream">Stream to store the uncompressed data</param>
    /// <returns>True if success, false if not.</returns>
    /// <remarks>Unique compression methods are Store and Deflate</remarks>
#if NOASYNC
        private bool
#else
    public async Task<bool>
#endif
        ExtractFileAsync(ZipFileEntry _zfe, Stream _stream)
    {
        if (!_stream.CanWrite)
            throw new InvalidOperationException("Stream cannot be written");

        // check signature
        var signature = new byte[4];
        ZipFileStream!.Seek(_zfe.HeaderOffset, SeekOrigin.Begin);

#if NOASYNC
                this.ZipFileStream.Read(signature, 0, 4);
#else
        await ZipFileStream.ReadAsync(signature, 0, 4);
#endif

        if (BitConverter.ToUInt32(signature, 0) != 0x04034b50)
            return false;

        // Select input stream for inflating or just reading
        Stream inStream;

        switch(_zfe.Method)
        {
            case Compression.Store:
                inStream = ZipFileStream;

                break;
            case Compression.Deflate:
                inStream = new DeflateStream(ZipFileStream, CompressionMode.Decompress, true);

                break;
            default:
                return false;
        }

        // Buffered copy
        var buffer = new byte[65535];
        ZipFileStream.Seek(_zfe.FileOffset, SeekOrigin.Begin);
        var bytesPending = _zfe.FileSize;

        while (bytesPending > 0)
        {
#if NOASYNC
                    int bytesRead = inStream.Read(buffer, 0, (int)Math.Min(bytesPending, buffer.Length));
                    _stream.Write(buffer, 0, bytesRead);
#else
            var bytesRead = await inStream.ReadAsync(buffer, 0, (int)Math.Min(bytesPending, buffer.Length));
            await _stream.WriteAsync(buffer, 0, bytesRead);
#endif

            bytesPending -= (uint)bytesRead;
        }
        _stream.Flush();

        if (_zfe.Method == Compression.Deflate)
            inStream.Dispose();

        return true;
    }

    /// <summary>
    /// Copy the contents of a stored file into a byte array
    /// </summary>
    /// <param name="_zfe">Entry information of file to extract</param>
    /// <param name="_file">Byte array with uncompressed data</param>
    /// <returns>True if success, false if not.</returns>
    /// <remarks>Unique compression methods are Store and Deflate</remarks>
    public bool ExtractFile(ZipFileEntry _zfe, out byte[]? _file)
    {
        using (var ms = new MemoryStream())
        {
            if (ExtractFile(_zfe, ms))
            {
                _file = ms.ToArray();

                return true;
            }

            _file = null;

            return false;
        }
    }

    /// <summary>
    /// Removes one of many files in storage. It creates a new Zip file.
    /// </summary>
    /// <param name="_zip">Reference to the current Zip object</param>
    /// <param name="_zfes">List of Entries to remove from storage</param>
    /// <returns>True if success, false if not</returns>
    /// <remarks>This method only works for storage of type FileStream</remarks>
    public static bool RemoveEntries(ref ZipFileProcessor _zip, List<ZipFileEntry> _zfes)
    {
        if (_zip.ZipFileStream is not FileStream)
            throw new InvalidOperationException("RemoveEntries is allowed just over streams of type FileStream");

        //Get full list of entries
        var fullList = _zip.ReadCentralDir();

        //In order to delete we need to create a copy of the zip file excluding the selected items
        var tempZipName = Path.GetTempFileName();
        var tempEntryName = Path.GetTempFileName();

        try
        {
            var tempZip = Create(tempZipName, string.Empty);

            foreach (var zfe in fullList)
            {
                if (!_zfes.Contains(zfe))
                    if (_zip.ExtractFile(zfe, tempEntryName))
                        tempZip.AddFile(zfe.Method, tempEntryName, zfe.FilenameInZip!, zfe.Comment);
            }

            _zip.Close();
            tempZip.Close();

            File.Delete(_zip.FileName!);
            File.Move(tempZipName, _zip.FileName!);

            _zip = Open(_zip.FileName!, _zip.Access);
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
    #endregion

    #region Private methods
    // Calculate the file offset by reading the corresponding local header
    private uint GetFileOffset(uint _headerOffset)
    {
        var buffer = new byte[2];

        ZipFileStream?.Seek(_headerOffset + 26, SeekOrigin.Begin);
        ZipFileStream?.Read(buffer, 0, 2);
        var filenameSize = BitConverter.ToUInt16(buffer, 0);
        ZipFileStream?.Read(buffer, 0, 2);
        var extraSize = BitConverter.ToUInt16(buffer, 0);

        return (uint)(30 + filenameSize + extraSize + _headerOffset);
    }

    /* Local file header:
        local file header signature     4 bytes  (0x04034b50)
        version needed to extract       2 bytes
        general purpose bit flag        2 bytes
        compression method              2 bytes
        last mod file time              2 bytes
        last mod file date              2 bytes
        crc-32                          4 bytes
        compressed size                 4 bytes
        uncompressed size               4 bytes
        filename length                 2 bytes
        extra field length              2 bytes
        filename (variable size)
        extra field (variable size)
    */
    private void WriteLocalHeader(ZipFileEntry _zfe)
    {
        var pos = ZipFileStream!.Position;
        var encoder = _zfe.EncodeUTF8 ? Encoding.UTF8 : DefaultEncoding;
        var encodedFilename = encoder.GetBytes(_zfe.FilenameInZip!);
        var extraInfo = CreateExtraInfo(_zfe);

        ZipFileStream.Write(new byte[] { 80, 75, 3, 4, 20, 0 }, 0, 6);                            // No extra header
        ZipFileStream.Write(BitConverter.GetBytes((ushort)(_zfe.EncodeUTF8 ? 0x0800 : 0)), 0, 2); // filename and comment encoding
        ZipFileStream.Write(BitConverter.GetBytes((ushort)_zfe.Method), 0, 2);                    // zipping method
        ZipFileStream.Write(BitConverter.GetBytes(DateTimeToDosTime(_zfe.ModifyTime)), 0, 4);     // zipping date and time
        ZipFileStream.Write(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 0, 12);            // unused CRC, un/compressed size, updated later
        ZipFileStream.Write(BitConverter.GetBytes((ushort)encodedFilename.Length), 0, 2);         // filename length
        ZipFileStream.Write(BitConverter.GetBytes((ushort)extraInfo.Length), 0, 2);               // extra length

        ZipFileStream.Write(encodedFilename, 0, encodedFilename.Length);
        ZipFileStream.Write(extraInfo, 0, extraInfo.Length);
        _zfe.HeaderSize = (uint)(ZipFileStream.Position - pos);
    }

    /* Central directory's File header:
        central file header signature   4 bytes  (0x02014b50)
        version made by                 2 bytes
        version needed to extract       2 bytes
        general purpose bit flag        2 bytes
        compression method              2 bytes
        last mod file time              2 bytes
        last mod file date              2 bytes
        crc-32                          4 bytes
        compressed size                 4 bytes
        uncompressed size               4 bytes
        filename length                 2 bytes
        extra field length              2 bytes
        file comment length             2 bytes
        disk number start               2 bytes
        internal file attributes        2 bytes
        external file attributes        4 bytes
        relative offset of local header 4 bytes
        filename (variable size)
        extra field (variable size)
        file comment (variable size)
    */
    private void WriteCentralDirRecord(ZipFileEntry _zfe)
    {
        var encoder = _zfe.EncodeUTF8 ? Encoding.UTF8 : DefaultEncoding;
        var encodedFilename = encoder.GetBytes(_zfe.FilenameInZip);
        var encodedComment = encoder.GetBytes(_zfe.Comment);
        var extraInfo = CreateExtraInfo(_zfe);

        ZipFileStream!.Write(new byte[] { 80, 75, 1, 2, 23, 0xB, 20, 0 }, 0, 8);
        ZipFileStream.Write(BitConverter.GetBytes((ushort)(_zfe.EncodeUTF8 ? 0x0800 : 0)), 0, 2); // filename and comment encoding
        ZipFileStream.Write(BitConverter.GetBytes((ushort)_zfe.Method), 0, 2);                    // zipping method
        ZipFileStream.Write(BitConverter.GetBytes(DateTimeToDosTime(_zfe.ModifyTime)), 0, 4);     // zipping date and time
        ZipFileStream.Write(BitConverter.GetBytes(_zfe.Crc32), 0, 4);                             // file CRC
        ZipFileStream.Write(BitConverter.GetBytes(get32bitSize(_zfe.CompressedSize)), 0, 4);      // compressed file size
        ZipFileStream.Write(BitConverter.GetBytes(get32bitSize(_zfe.FileSize)), 0, 4);            // uncompressed file size
        ZipFileStream.Write(BitConverter.GetBytes((ushort)encodedFilename.Length), 0, 2);         // Filename in zip
        ZipFileStream.Write(BitConverter.GetBytes((ushort)extraInfo.Length), 0, 2);               // extra length
        ZipFileStream.Write(BitConverter.GetBytes((ushort)encodedComment.Length), 0, 2);

        ZipFileStream.Write(BitConverter.GetBytes((ushort)0), 0, 2);                       // disk=0
        ZipFileStream.Write(BitConverter.GetBytes((ushort)0), 0, 2);                       // file type: binary
        ZipFileStream.Write(BitConverter.GetBytes((ushort)0), 0, 2);                       // Internal file attributes
        ZipFileStream.Write(BitConverter.GetBytes((ushort)0x8100), 0, 2);                  // External file attributes (normal/readable)
        ZipFileStream.Write(BitConverter.GetBytes(get32bitSize(_zfe.HeaderOffset)), 0, 4); // Offset of header

        ZipFileStream.Write(encodedFilename, 0, encodedFilename.Length);
        ZipFileStream.Write(extraInfo, 0, extraInfo.Length);
        ZipFileStream.Write(encodedComment, 0, encodedComment.Length);
    }

    private uint get32bitSize(long size)
    {
        return size >= 0xFFFFFFFF ? 0xFFFFFFFF : (uint)size;
    }

    /* 
    Zip64 end of central directory record
        zip64 end of central dir 
        signature                       4 bytes  (0x06064b50)
        size of zip64 end of central
        directory record                8 bytes
        version made by                 2 bytes
        version needed to extract       2 bytes
        number of this disk             4 bytes
        number of the disk with the 
        start of the central directory  4 bytes
        total number of entries in the
        central directory on this disk  8 bytes
        total number of entries in the
        central directory               8 bytes
        size of the central directory   8 bytes
        offset of start of central
        directory with respect to
        the starting disk number        8 bytes
        zip64 extensible data sector    (variable size)        
    
    Zip64 end of central directory locator
        zip64 end of central dir locator 
        signature                       4 bytes  (0x07064b50)
        number of the disk with the
        start of the zip64 end of 
        central directory               4 bytes
        relative offset of the zip64
        end of central directory record 8 bytes
        total number of disks           4 bytes
    End of central dir record:
        end of central dir signature    4 bytes  (0x06054b50)
        number of this disk             2 bytes
        number of the disk with the
        start of the central directory  2 bytes
        total number of entries in
        the central dir on this disk    2 bytes
        total number of entries in
        the central dir                 2 bytes
        size of the central directory   4 bytes
        offset of start of central
        directory with respect to
        the starting disk number        4 bytes
        zipfile comment length          2 bytes
        zipfile comment (variable size)
    */
    private void WriteEndRecord(long _size, long _offset)
    {
        var dirOffset = ZipFileStream!.Length;

        // Zip64 end of central directory record
        ZipFileStream.Position = dirOffset;
        ZipFileStream.Write(new byte[] { 80, 75, 6, 6 }, 0, 4);
        ZipFileStream.Write(BitConverter.GetBytes((long)44), 0, 8);                    // size of zip64 end of central directory
        ZipFileStream.Write(BitConverter.GetBytes((ushort)45), 0, 2);                  // version made by
        ZipFileStream.Write(BitConverter.GetBytes((ushort)45), 0, 2);                  // version needed to extract
        ZipFileStream.Write(BitConverter.GetBytes((uint)0), 0, 4);                     // current disk
        ZipFileStream.Write(BitConverter.GetBytes((uint)0), 0, 4);                     // start of central directory
        ZipFileStream.Write(BitConverter.GetBytes(Files.Count + ExistingFiles), 0, 8); // total number of entries in the central directory in disk
        ZipFileStream.Write(BitConverter.GetBytes(Files.Count + ExistingFiles), 0, 8); // total number of entries in the central directory
        ZipFileStream.Write(BitConverter.GetBytes(_size), 0, 8);                       // size of the central directory
        ZipFileStream.Write(BitConverter.GetBytes(_offset), 0, 8);                     // offset of start of central directory with respect to the starting disk number

        // Zip64 end of central directory locator
        ZipFileStream.Write(new byte[] { 80, 75, 6, 7 }, 0, 4);
        ZipFileStream.Write(BitConverter.GetBytes((uint)0), 0, 4);   // number of the disk
        ZipFileStream.Write(BitConverter.GetBytes(dirOffset), 0, 8); // relative offset of the zip64 end of central directory record
        ZipFileStream.Write(BitConverter.GetBytes((uint)1), 0, 4);   // total number of disks

        var encoder = EncodeUTF8 ? Encoding.UTF8 : DefaultEncoding;
        var encodedComment = encoder.GetBytes(Comment);

        ZipFileStream.Write(new byte[] { 80, 75, 5, 6, 0, 0, 0, 0 }, 0, 8);
        ZipFileStream.Write(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, 0, 12);
        ZipFileStream.Write(BitConverter.GetBytes((ushort)encodedComment.Length), 0, 2);
        ZipFileStream.Write(encodedComment, 0, encodedComment.Length);
    }

    // Copies all the source file into the zip storage
#if NOASYNC
        private Compression
#else
    private async Task<Compression>
#endif
        Store(ZipFileEntry _zfe, Stream _source)
    {
        var buffer = new byte[16384];
        int bytesRead;
        uint totalRead = 0;
        Stream outStream;

        var posStart = ZipFileStream!.Position;
        var sourceStart = _source.CanSeek ? _source.Position : 0;

        if (_zfe.Method == Compression.Store)
            outStream = ZipFileStream;
        else
            outStream = new DeflateStream(ZipFileStream, CompressionMode.Compress, true);

        _zfe.Crc32 = 0 ^ 0xffffffff;

        do
        {
#if NOASYNC
                bytesRead = _source.Read(buffer, 0, buffer.Length);
                if (bytesRead > 0)
                    outStream.Write(buffer, 0, bytesRead);
#else
            bytesRead = await _source.ReadAsync(buffer, 0, buffer.Length);
            if (bytesRead > 0)
                await outStream.WriteAsync(buffer, 0, bytesRead);
#endif

            for (uint i = 0;
                 i < bytesRead;
                 i++)
                _zfe.Crc32 = CrcTable![(_zfe.Crc32 ^ buffer[i]) & 0xFF] ^ (_zfe.Crc32 >> 8);

            totalRead += (uint)bytesRead;
        } while (bytesRead > 0);

        outStream.Flush();

        if (_zfe.Method == Compression.Deflate)
            outStream.Dispose();

        _zfe.Crc32 ^= 0xFFFFFFFF;
        _zfe.FileSize = totalRead;
        _zfe.CompressedSize = (uint)(ZipFileStream.Position - posStart);

        // Verify for real compression
        if (_zfe.Method == Compression.Deflate && !ForceDeflating && _source.CanSeek && _zfe.CompressedSize > _zfe.FileSize)
        {
            // Start operation again with Store algorithm
            _zfe.Method = Compression.Store;
            ZipFileStream.Position = posStart;
            ZipFileStream.SetLength(posStart);
            _source.Position = sourceStart;

#if NOASYNC
                    return this.Store(_zfe, _source);
#else
            return await Store(_zfe, _source);
#endif
        }

        return _zfe.Method;
    }

    /* DOS Date and time:
        MS-DOS date. The date is a packed value with the following format. Bits Description 
            0-4 Day of the month (131) 
            5-8 Month (1 = January, 2 = February, and so on) 
            9-15 Year offset from 1980 (add 1980 to get actual year) 
        MS-DOS time. The time is a packed value with the following format. Bits Description 
            0-4 Second divided by 2 
            5-10 Minute (059) 
            11-15 Hour (023 on a 24-hour clock) 
    */
    private uint DateTimeToDosTime(DateTime _dt)
    {
        return (uint)((_dt.Second / 2) | (_dt.Minute << 5) | (_dt.Hour << 11) | (_dt.Day << 16) | (_dt.Month << 21) | ((_dt.Year - 1980) << 25));
    }

    private DateTime? DosTimeToDateTime(uint _dt)
    {
        var year = (int)(_dt >> 25) + 1980;
        var month = (int)(_dt >> 21) & 15;
        var day = (int)(_dt >> 16) & 31;
        var hours = (int)(_dt >> 11) & 31;
        var minutes = (int)(_dt >> 5) & 63;
        var seconds = (int)(_dt & 31) * 2;

        if (month == 0 || day == 0 || year >= 2107)
            return DateTime.Now;

        return new DateTime(year, month, day, hours, minutes, seconds);
    }

    private byte[] CreateExtraInfo(ZipFileEntry _zfe)
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
        BitConverter.GetBytes(_zfe.FileSize)
                    .CopyTo(buffer, 12); // MTime
        BitConverter.GetBytes(_zfe.CompressedSize)
                    .CopyTo(buffer, 20); // ATime
        BitConverter.GetBytes(_zfe.HeaderOffset)
                    .CopyTo(buffer, 28); // CTime

        BitConverter.GetBytes((ushort)0x000A)
                    .CopyTo(buffer, 36); // NTFS FileTime
        BitConverter.GetBytes((ushort)32)
                    .CopyTo(buffer, 38); // Length
        BitConverter.GetBytes((ushort)1)
                    .CopyTo(buffer, 44); // Tag 1
        BitConverter.GetBytes((ushort)24)
                    .CopyTo(buffer, 46); // Size 1
        BitConverter.GetBytes(_zfe.ModifyTime.ToFileTime())
                    .CopyTo(buffer, 48); // MTime
        BitConverter.GetBytes(_zfe.AccessTime.ToFileTime())
                    .CopyTo(buffer, 56); // ATime
        BitConverter.GetBytes(_zfe.CreationTime.ToFileTime())
                    .CopyTo(buffer, 64); // CTime

        return buffer;
    }

    private void ReadExtraInfo(byte[] buffer, int offset, ZipFileEntry _zfe)
    {
        if (buffer.Length < 4)
            return;

        var pos = offset;
        uint tag,
            size;

        while (pos < buffer.Length - 4)
        {
            uint extraId = BitConverter.ToUInt16(buffer, pos);
            uint length = BitConverter.ToUInt16(buffer, pos + 2);

            switch(extraId)
            {
                // ZIP64 Information
                case 0x0001:
                {
                    tag = BitConverter.ToUInt16(buffer, pos + 8);
                    size = BitConverter.ToUInt16(buffer, pos + 10);

                    if (tag == 1 && size >= 24)
                    {
                        if (_zfe.FileSize == 0xFFFFFFFF)
                            _zfe.FileSize = BitConverter.ToInt64(buffer, pos + 12);
                        if (_zfe.CompressedSize == 0xFFFFFFFF)
                            _zfe.CompressedSize = BitConverter.ToInt64(buffer, pos + 20);
                        if (_zfe.HeaderOffset == 0xFFFFFFFF)
                            _zfe.HeaderOffset = BitConverter.ToInt64(buffer, pos + 28);
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
                        _zfe.ModifyTime = DateTime.FromFileTime(BitConverter.ToInt64(buffer, pos + 12));
                        _zfe.AccessTime = DateTime.FromFileTime(BitConverter.ToInt64(buffer, pos + 20));
                        _zfe.CreationTime = DateTime.FromFileTime(BitConverter.ToInt64(buffer, pos + 28));
                    }

                    break;
                }
            }

            pos += (int)length + 4;
        }
    }

    /* CRC32 algorithm
      The 'magic number' for the CRC is 0xdebb20e3.  
      The proper CRC pre and post conditioning is used, meaning that the CRC register is
      pre-conditioned with all ones (a starting value of 0xffffffff) and the value is post-conditioned by
      taking the one's complement of the CRC residual.
      If bit 3 of the general purpose flag is set, this field is set to zero in the local header and the correct
      value is put in the data descriptor and in the central directory.
    */
    private void UpdateCrcAndSizes(ZipFileEntry _zfe)
    {
        var lastPos = ZipFileStream!.Position; // remember position

        ZipFileStream.Position = _zfe.HeaderOffset + 8;
        ZipFileStream.Write(BitConverter.GetBytes((ushort)_zfe.Method), 0, 2); // zipping method

        ZipFileStream.Position = _zfe.HeaderOffset + 14;
        ZipFileStream.Write(BitConverter.GetBytes(_zfe.Crc32), 0, 4);                        // Update CRC
        ZipFileStream.Write(BitConverter.GetBytes(get32bitSize(_zfe.CompressedSize)), 0, 4); // Compressed size
        ZipFileStream.Write(BitConverter.GetBytes(get32bitSize(_zfe.FileSize)), 0, 4);       // Uncompressed size

        ZipFileStream.Position = lastPos; // restore position
    }

    // Replaces backslashes with slashes to store in zip header
    private string NormalizedFilename(string _filename)
    {
        var filename = _filename.Replace('\\', '/');

        var pos = filename.IndexOf(':');
        if (pos >= 0)
            filename = filename.Remove(0, pos + 1);

        return filename.Trim('/');
    }

    // Reads the end-of-central-directory record
    private bool ReadFileInfo()
    {
        if (ZipFileStream!.Length < 22)
            return false;

        ZipFileStream.Seek(0, SeekOrigin.Begin);
        {
            var brCheckHeaderSig = new BinaryReader(ZipFileStream);
            {
                var headerSig = brCheckHeaderSig.ReadUInt32();

                if (headerSig != 0x04034b50)

                    // not PK.. signature header
                    return false;
            }
        }

        try
        {
            ZipFileStream.Seek(-17, SeekOrigin.End);
            var br = new BinaryReader(ZipFileStream);

            do
            {
                ZipFileStream.Seek(-5, SeekOrigin.Current);
                var sig = br.ReadUInt32();

                if (sig == 0x06054b50) // It is central dir
                {
                    var dirPosition = ZipFileStream.Position - 4;

                    ZipFileStream.Seek(6, SeekOrigin.Current);

                    long entries = br.ReadUInt16();
                    long centralSize = br.ReadInt32();
                    long centralDirOffset = br.ReadUInt32();
                    var commentSize = br.ReadUInt16();

                    var commentPosition = ZipFileStream.Position;

                    if (centralDirOffset == 0xffffffff) // It is a Zip64 file
                    {
                        ZipFileStream.Position = dirPosition - 20;

                        sig = br.ReadUInt32();

                        if (sig != 0x07064b50) // Not a Zip64 central dir locator
                            return false;

                        ZipFileStream.Seek(4, SeekOrigin.Current);

                        var dir64Position = br.ReadInt64();
                        ZipFileStream.Position = dir64Position;

                        sig = br.ReadUInt32();

                        if (sig != 0x06064b50) // Not a Zip64 central dir record
                            return false;

                        ZipFileStream.Seek(28, SeekOrigin.Current);
                        entries = br.ReadInt64();
                        centralSize = br.ReadInt64();
                        centralDirOffset = br.ReadInt64();
                    }

                    // check if comment field is the very last data in file
                    if (commentPosition + commentSize != ZipFileStream.Length)
                        return false;

                    // Copy entire central directory to a memory buffer
                    ExistingFiles = entries;
                    CentralDirImage = new byte[centralSize];
                    ZipFileStream.Seek(centralDirOffset, SeekOrigin.Begin);
                    ZipFileStream.Read(CentralDirImage, 0, (int)centralSize);

                    // Leave the pointer at the begining of central dir, to append new files
                    ZipFileStream.Seek(centralDirOffset, SeekOrigin.Begin);

                    return true;
                }
            } while (ZipFileStream.Position > 0);
        }
        catch
        {
            // ignored
        }

        return false;
    }
    #endregion

    #region IDisposable implementation
    /// <summary>
    /// Closes the Zip file stream
    /// </summary>
    public void Dispose()
    {
        Dispose(true);

        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!isDisposed)
        {
            if (disposing)
                Close();

            isDisposed = true;
        }
    }
    #endregion
}