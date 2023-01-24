using MediaFileProcessor.Models.Enums;
namespace MediaFileProcessor.Models.Common;

public static class FilesSignatures
{
    public static byte[] JPG => new byte[] { 0xFF, 0xD8, 0xFF };

    public static byte[] PNG => new byte[] { 0x89, 0x50, 0x4e, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };

    public static byte[] GetSignature(FileFormatType outputFormatType)
    {
        return outputFormatType switch
        {
            FileFormatType.JPEG => JPG,
            FileFormatType.PNG => PNG,
            _ => throw new NotSupportedException("This format does not have a signature")
        };
    }
}