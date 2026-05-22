using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.Models;
public enum FileProcessingLineStatus
{
    Successful,
    Failed,
    Ignored
}

public class FileProcessingModels {
    [ExcludeFromCodeCoverage]
    public sealed class FileProcessingLineModel {
        public int LineNumber { get; set; }
        public string LineContents { get; set; } = string.Empty;
        public FileProcessingLineStatus LineStatus { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public sealed class FileProcessingFileModel {
        public Guid FileId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileProfile { get; set; } = string.Empty;
        public DateTime DateTimeUploaded { get; set; }
        public Guid UploadedById { get; set; }
        public string UploadedBy { get; set; } = string.Empty;
        public Guid MerchantId { get; set; }
        public string MerchantName { get; set; } = string.Empty;
        public List<FileProcessingLineModel> FileLines { get; set; } = [];

        public int TotalLines => this.FileLines.Count;
        public int SuccessfulLines => this.FileLines.Count(line => line.LineStatus == FileProcessingLineStatus.Successful);
        public int FailedLines => this.FileLines.Count(line => line.LineStatus == FileProcessingLineStatus.Failed);
        public int IgnoredLines => this.FileLines.Count(line => line.LineStatus == FileProcessingLineStatus.Ignored);
    }

    [ExcludeFromCodeCoverage]
    public sealed class FileImportLogDetailsModel {
        public Guid FileImportLogId { get; set; }
        public DateTime ImportLogDate { get; set; }
        public List<FileProcessingFileModel> Files { get; set; } = [];

        public int NumberOfFilesProcessed => this.Files.Count;
        public int TotalFileLines => this.Files.Sum(file => file.TotalLines);
        public int SuccessfulLines => this.Files.Sum(file => file.SuccessfulLines);
        public int FailedLines => this.Files.Sum(file => file.FailedLines);
        public int IgnoredLines => this.Files.Sum(file => file.IgnoredLines);
    }
}