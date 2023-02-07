namespace MediaFileProcessor.Models.Enums;

/// <summary>
/// Enum to represent different types of media file inputs.
/// </summary>
public enum MediaFileInputType
{
    /// <summary>
    /// Input media file is specified by its file path.
    /// </summary>
    Path = 0,

    /// <summary>
    /// Input media file is specified by a stream.
    /// </summary>
    Stream = 1,

    /// <summary>
    /// Input media file is specified by a template.
    /// </summary>
    Template = 2,

    /// <summary>
    /// Input media file is specified by a named pipe.
    /// </summary>
    NamedPipe = 3
}