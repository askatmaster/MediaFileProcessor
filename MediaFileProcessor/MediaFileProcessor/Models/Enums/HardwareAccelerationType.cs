namespace MediaFileProcessor.Models.Enums;

public enum HardwareAccelerationType
{
    AUTO = 0,
    DXVA2 = 1,
    VDPAU = 2,
    D3D11VA = 3,
    VAAPI = 4,
    QSV = 5,
    CUDA = 6,
    NVDEC = 7,
    CUVID = 8
}