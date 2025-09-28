using FileOrganiser.Enums;

namespace FileOrganiser.Extensions;

public static class FileInfoExtensions
{
    public static FileTypeEnum GetFileType(this FileInfo? fileInfo)
    {
        if (fileInfo == null) return FileTypeEnum.Unknown;

        return fileInfo.Extension.ToSafeString().ToLower() switch
        {
            ".jpg" or ".jpeg" or ".png" or ".gif" or ".bmp" or ".tiff" => FileTypeEnum.Image,
            ".mp4" or ".avi" or ".mkv" or ".mov" or ".flv" or ".webm" => FileTypeEnum.Video,
            ".mp3" or ".wav" or ".flac" or ".aac" or ".ogg" or ".wma" or ".m4a" => FileTypeEnum.Audio,
            ".exe" or ".bat" or ".cmd" or ".sh" or ".bash" or ".app" or ".msi" or ".apk" => FileTypeEnum.Executables,
            ".txt" or ".pdf" or ".docx" or ".xlsx" or ".pptx" => FileTypeEnum.Document,
            ".zip" => FileTypeEnum.Zip,
            _ => FileTypeEnum.Unknown
        };
    }
}
