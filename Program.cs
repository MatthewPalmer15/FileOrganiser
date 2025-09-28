using FileOrganiser.Enums;
using FileOrganiser.Extensions;
using System.IO.Compression;

var sourcePath = @"D:\Files\Pending";
var destinationPath = @"D:\Files\Completed";

if (!Directory.Exists(sourcePath))
    return;

// EXTRACT ZIP FILES FIRST.
foreach (var zipPath in Directory.EnumerateFiles(sourcePath, "*.zip", SearchOption.AllDirectories))
{
    try
    {
        var attrs = File.GetAttributes(zipPath);
        if ((attrs & FileAttributes.System) == FileAttributes.System || (attrs & FileAttributes.Hidden) == FileAttributes.Hidden)
        {
            Console.WriteLine($"Skipping system/hidden zip: {zipPath}");
            continue;
        }

        var parentDir = Path.GetDirectoryName(zipPath)!;
        var extractDirName = Path.GetFileNameWithoutExtension(zipPath);
        var extractDir = Path.Combine(parentDir, extractDirName);

        // If it's already extracted (folder exists and not empty), skip.
        if (Directory.Exists(extractDir) && Directory.EnumerateFileSystemEntries(extractDir).Any())
        {
            Console.WriteLine($"Already extracted: {zipPath}");
            continue;
        }

        Directory.CreateDirectory(extractDir);
        ZipFile.ExtractToDirectory(zipPath, extractDir, overwriteFiles: true);
        Console.WriteLine($"Extracted: {zipPath} => {extractDir}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error extracting {zipPath}: {ex.Message}");
    }
}


var filePaths = Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories);
foreach (var filePath in filePaths)
{
    if (!File.Exists(filePath)) continue;

    var file = new FileInfo(filePath);
    if (string.IsNullOrWhiteSpace(file.Extension))
        continue;

    var fileType = file.GetFileType();

    var destinationSubFolder = fileType switch
    {
        FileTypeEnum.Image => "Images/",
        FileTypeEnum.Video => "Videos/",
        FileTypeEnum.Audio => "Audios/",
        FileTypeEnum.Executables => "Executables",
        FileTypeEnum.Document => "Documents/",
        FileTypeEnum.Zip => "Zip/",
        FileTypeEnum.Unknown => "Other/",
        _ => throw new InvalidOperationException("File type is not supported.")
    };

    var fileDestinationPath = $"{destinationPath}/{destinationSubFolder}/{file.CreationTimeUtc.Year}/{file.CreationTimeUtc:MMMM}/";
    if (!Directory.Exists(fileDestinationPath))
        Directory.CreateDirectory(fileDestinationPath);

    fileDestinationPath += file.Name;
    try
    {
        var fileAttributes = File.GetAttributes(filePath);

        var isSystemFile = (fileAttributes & FileAttributes.System) == FileAttributes.System;
        var isHiddenFile = (fileAttributes & FileAttributes.Hidden) == FileAttributes.Hidden;

        if (isHiddenFile || isSystemFile)
        {
            Console.WriteLine($"Cannot move system file: {filePath}");
            continue;
        }

        File.Move(filePath, fileDestinationPath);
    }
    catch (IOException ex)
    {
        Console.WriteLine($"Error moving file {filePath}: {ex.Message}");
    }
}