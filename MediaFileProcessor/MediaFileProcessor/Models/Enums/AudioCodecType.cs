namespace MediaFileProcessor.Models.Enums;

/// <summary>
/// Enumeration for the supported audio codec types.
/// </summary>
public enum AudioCodecType
{
    /// <summary>
    /// The audio codec type for copying the original audio without encoding or compression.
    /// </summary>
    /// <remarks>
    /// This option can be useful when the original audio quality is already high,
    /// and you want to maintain the original quality without any further processing.
    /// </remarks>
    COPY = 0,

    /// <summary>
    /// The audio codec type for MP3 encoding.
    /// </summary>
    /// <remarks>
    /// MP3 is a popular audio format that uses lossy compression to reduce file size,
    /// while maintaining good audio quality for most applications.
    /// </remarks>
    MP3 = 1,

    /// <summary>
    /// The audio codec type for WMA encoding.
    /// </summary>
    /// <remarks>
    /// WMA is a proprietary audio format developed by Microsoft,
    /// and it offers both lossless and lossy compression options.
    /// </remarks>
    WMA = 2,

    /// <summary>
    /// The audio codec type for WAV encoding.
    /// </summary>
    /// <remarks>
    /// WAV is a standard audio format for storing uncompressed audio data,
    /// and it is commonly used for storing high-quality audio recordings.
    /// </remarks>
    WAV = 3,

    /// <summary>
    /// The audio codec type for AAC encoding.
    /// </summary>
    /// <remarks>
    /// AAC is a popular audio format that uses lossy compression,
    /// and it is widely used for storing audio on portable devices and streaming audio over the internet.
    /// </remarks>
    AAC = 4,

    /// <summary>
    /// The audio codec type for FLAC encoding.
    /// </summary>
    /// <remarks>
    /// FLAC is a lossless audio format that is commonly used for storing high-quality audio recordings,
    /// and it provides efficient compression without sacrificing audio quality.
    /// </remarks>
    FLAC = 5,

    /// <summary>
    /// The audio codec type for ALAC encoding.
    /// </summary>
    /// <remarks>
    /// ALAC is a lossless audio format developed by Apple,
    /// and it is commonly used for storing high-quality audio recordings on Apple devices.
    /// </remarks>
    ALAC = 6,

    /// <summary>
    /// The audio codec type for WMA encoding.
    /// </summary>
    /// <remarks>
    /// WMA is a proprietary audio format developed by Microsoft,
    /// and it offers both lossless and lossy compression options.
    /// </remarks>
    WMAV2 = 7,

    /// <summary>
    /// AC3 (Audio Codec 3), also known as Dolby Digital, is a lossy audio compression format developed by Dolby Laboratories.
    /// It is widely used in surround sound systems, such as those in movie theaters and home theaters.
    /// AC3 supports multiple audio channels, making it suitable for multi-channel audio setups (e.g., 5.1 or 7.1 surround sound).
    /// </summary>
    AC3 = 8,

    /// <summary>
    /// The pcm_s16le audio codec in FFmpeg represents uncompressed Pulse Code Modulation (PCM) audio data with a signed 16-bit little-endian format.
    /// PCM is a method used to digitally represent analog signals, and it is the standard form of digital audio in computers, compact discs, digital telephony, and other digital audio applications.
    /// Here's a brief explanation of the pcm_s16le codec:
    /// PCM (Pulse Code Modulation): A technique used to convert analog audio signals into digital data without any compression.
    /// S16: Refers to a signed 16-bit integer format, meaning each audio sample is represented by a 16-bit value, allowing for a dynamic range of approximately 96 decibels (dB).
    /// LE (Little-Endian): Represents the byte order of the 16-bit values, where the least significant byte comes first.
    /// </summary>
    PCM_S16LE = 9,


    /// <summary>
    /// libopus is an FFmpeg audio codec that allows you to encode or decode audio streams using the Opus audio format.
    /// Opus is a versatile, open, and royalty-free audio codec designed for a wide range of applications, from low-latency real-time communication to high-quality audio streaming.
    /// It is well-suited for both speech and music, and it provides low-latency and high compression efficiency.
    /// </summary>
    LIBOPUS = 10
}