namespace MediaFileProcessor.Models.Enums;

/// <summary>
/// Audio Filter Type
/// </summary>
public enum AudioFilterType
{
    /// <summary>
    /// Changes the volume of the audio.
    /// Example of use: volume=2 (increases volume by 2 times)
    /// </summary>
    Volume,

    /// <summary>
    /// Applies equalization to audio.
    /// Usage example: equalizer=f=1000:width_type=h:width=200:g=-10 (reduces the frequency of 1000 Hz by 10 dB)
    /// </summary>
    Equalizer,

    /// <summary>
    /// Adds reverb to audio.
    /// Usage example: aecho=0.8:0.9:1000|1800:0.3|0.25 (parameters for easy reverb)
    /// </summary>
    Aecho,

    /// <summary>
    /// Changes the audio playback speed without changing the pitch.
    /// Example of use: atempo=1.5 (increases playback speed by 1.5 times)
    /// </summary>
    Atempo,

    /// <summary>
    /// Changes the pitch of the audio without changing the playback speed.
    /// Example use: asetrate=480001.5 (increases pitch by 1.5 times)
    /// </summary>
    Asetrate,

    /// <summary>
    /// Removes noise from the audio recording.
    /// Usage example: anlmdenoise=s=1 (standard noise removal profile)
    /// </summary>
    Anlmdenoise
}