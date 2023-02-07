namespace MediaFileProcessor.Models.Enums;

/// <summary>
/// Enum to represent different types of hardware acceleration.
/// </summary>
public enum HardwareAccelerationType
{
    /// <summary>
    /// Automatically choose the best hardware acceleration method.
    /// </summary>
    AUTO = 0,

    /// <summary>
    /// DirectX Video Acceleration 2 (DXVA2) hardware acceleration.
    /// </summary>
    DXVA2 = 1,

    /// <summary>
    /// Video Decode and Presentation API for Unix (VDPAU) hardware acceleration.
    /// </summary>
    VDPAU = 2,

    /// <summary>
    /// Direct3D 11 Video Acceleration (D3D11VA) hardware acceleration.
    /// </summary>
    D3D11VA = 3,

    /// <summary>
    /// Video Acceleration API (VAAPI) hardware acceleration.
    /// </summary>
    VAAPI = 4,

    /// <summary>
    /// Intel Quick Sync Video (QSV) hardware acceleration.
    /// </summary>
    QSV = 5,

    /// <summary>
    /// NVIDIA CUDA hardware acceleration.
    /// </summary>
    CUDA = 6,

    /// <summary>
    /// NVIDIA NVDEC hardware acceleration.
    /// </summary>
    NVDEC = 7,

    /// <summary>
    /// NVIDIA CUVID hardware acceleration.
    /// </summary>
    CUVID = 8
}