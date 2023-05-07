using MediaFileProcessor.Models.Enums;
namespace ConsoleTest;

public static class TestFile
{
    private const string BasePath = @"testFiles/";
    public const string ResultFilePath = @"tests/";
    public static string GetPath(FileFormatType formatType)
    {
        var filePath = BasePath + $"sample.{formatType}".ToLowerInvariant().Replace("_", "");

        if(!File.Exists(filePath))
            throw new FileNotFoundException($"{filePath} not found");

        return filePath;
    }

    public static void VerifyFileSize(string path, int expectedSize)
    {
        var fileInfo = new FileInfo(path);

        if(expectedSize != fileInfo.Length)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"ERROR ({path}): The expected size does not correspond to the real one");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{path} file successfully created");
            Console.ResetColor();
        }
    }
}