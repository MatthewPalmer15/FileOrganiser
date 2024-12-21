var sourcePath = @"D:\\BACKUP\\";
var destinationPath = @"D:\\PhoneBackup\\";


if (!Directory.Exists(sourcePath))
    return;

var filePaths = Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories);
foreach (var filePath in filePaths)
{
    if (!File.Exists(filePath)) continue;

    var file = new FileInfo(filePath);
    if (string.IsNullOrWhiteSpace(file.Extension))
        continue;

    var fileDestinationPath = $"{destinationPath}/{file.LastWriteTime.Year}/{file.LastWriteTime:MMMM}/";

    if (file.Extension.ToLower() is ".jpg" or ".jpeg" or ".png" or ".gif" or ".bmp" or ".tiff")
    {
        fileDestinationPath += "Images/";
    }
    else if (file.Extension.ToLower() is ".mp4" or ".avi" or ".mkv" or ".mov" or ".flv" or ".webm")
    {
        fileDestinationPath += "Videos/";
    }
    else if (file.Extension.ToLower() is ".mp3" or ".wav" or ".flac" or ".aac" or ".ogg" or ".wma" or ".m4a")
    {
        fileDestinationPath += "Audio/";
    }
    else if (file.Extension.ToLower() is ".exe" or ".bat" or ".cmd" or ".sh" or ".bash" or ".app" or ".msi" or ".apk")
    {
        fileDestinationPath += "Installers/";
    }
    else if (file.Extension.ToLower() is ".txt" or ".pdf" or ".docx" or ".xlsx" or ".pptx")
    {
        fileDestinationPath += "Documents/";
    }

    if (!Directory.Exists(fileDestinationPath))
        Directory.CreateDirectory(fileDestinationPath);

    fileDestinationPath += file.Name;
    try
    {
        var fileAttributes = File.GetAttributes(filePath);

        if (((fileAttributes & FileAttributes.System) != FileAttributes.System) || ((fileAttributes & FileAttributes.Hidden) != FileAttributes.Hidden))
        {
            File.Move(filePath, fileDestinationPath);
            Console.WriteLine($"File moved: {filePath} => {fileDestinationPath}");
        }
        else
        {
            Console.WriteLine($"Cannot move system file: {filePath}");
        }
    }
    catch (IOException ex)
    {
        Console.WriteLine($"Error moving file {filePath}: {ex.Message}");
    }
}