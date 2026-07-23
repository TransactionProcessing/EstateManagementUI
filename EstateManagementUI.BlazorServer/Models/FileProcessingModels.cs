using System.Diagnostics.CodeAnalysis;
using EstateManagementUI.BusinessLogic.BackendAPI;

namespace EstateManagementUI.BlazorServer.Models;

//[ExcludeFromCodeCoverage]
public enum FileProcessingLineStatus
{
    Successful,
    Failed,
    Ignored
}

[ExcludeFromCodeCoverage]
public sealed class FileProcessingLineModel
{
    public int LineNumber { get; set; }
    public string LineContents { get; set; } = string.Empty;
    public FileProcessingLineStatus LineStatus { get; set; }
}

[ExcludeFromCodeCoverage]
public sealed class FileProcessingFileModel
{
    public Guid FileId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileProfile { get; set; } = string.Empty;
    public DateTime DateTimeUploaded { get; set; }
    public string UploadedBy { get; set; } = string.Empty;
    public string MerchantName { get; set; } = string.Empty;
    public List<FileProcessingLineModel> FileLines { get; set; } = [];

    public int TotalLines => FileLines.Count;
    public int SuccessfulLines => FileLines.Count(line => line.LineStatus == FileProcessingLineStatus.Successful);
    public int FailedLines => FileLines.Count(line => line.LineStatus == FileProcessingLineStatus.Failed);
    public int IgnoredLines => FileLines.Count(line => line.LineStatus == FileProcessingLineStatus.Ignored);
}

[ExcludeFromCodeCoverage]
public sealed class FileImportLogDetailsModel
{
    public Guid FileImportLogId { get; set; }
    public DateTime ImportLogDate { get; set; }
    public List<FileProcessingFileModel> Files { get; set; } = [];

    public int NumberOfFilesProcessed => Files.Count;
    public int TotalFileLines => Files.Sum(file => file.TotalLines);
    public int SuccessfulLines => Files.Sum(file => file.SuccessfulLines);
    public int FailedLines => Files.Sum(file => file.FailedLines);
    public int IgnoredLines => Files.Sum(file => file.IgnoredLines);
}

[ExcludeFromCodeCoverage]
public sealed class FileProfileDropDownModel
{
    public Guid FileProfileId { get; set; }
    public string Name { get; set; } = string.Empty;
}
