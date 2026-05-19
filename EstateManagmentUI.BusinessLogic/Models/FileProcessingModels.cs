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

/*
[ExcludeFromCodeCoverage]
public static class FileProcessingSeedData
{
    public static IReadOnlyList<FileImportLogDetailsModel> ImportLogs { get; } = BuildImportLogs();

    public static IReadOnlyList<FileImportLogDetailsModel> FilterByDate(DateTime? startDate, DateTime? endDate)
    {
        DateTime start = startDate?.Date ?? DateTime.MinValue.Date;
        DateTime end = endDate?.Date ?? DateTime.MaxValue.Date;

        return ImportLogs
            .Where(log => log.ImportLogDate.Date >= start && log.ImportLogDate.Date <= end)
            .OrderByDescending(log => log.ImportLogDate)
            .ToList();
    }

    public static FileImportLogDetailsModel? GetImportLog(Guid fileImportLogId) =>
        ImportLogs.FirstOrDefault(log => log.FileImportLogId == fileImportLogId);

    private static IReadOnlyList<FileImportLogDetailsModel> BuildImportLogs()
    {
        DateTime today = DateTime.Today;

        return
        [
            new FileImportLogDetailsModel
            {
                FileImportLogId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                ImportLogDate = today.AddDays(-1),
                Files = BuildAirtimeImportFiles(today.AddDays(-1))
            },
            new FileImportLogDetailsModel
            {
                FileImportLogId = Guid.Parse("11111111-1111-1111-1111-111111111112"),
                ImportLogDate = today.AddDays(-5),
                Files =
                [
                    new FileProcessingFileModel
                    {
                        FileId = Guid.Parse("33333333-3333-3333-3333-333333333331"),
                        FileName = "settlement-batch.csv",
                        FileProfile = "SettlementFile",
                        DateTimeUploaded = today.AddDays(-5).AddHours(11),
                        UploadedBy = "finance@estate.com",
                        MerchantName = "Northwind Traders",
                        FileLines =
                        [
                            new FileProcessingLineModel { LineNumber = 1, LineContents = "TX-001", LineStatus = FileProcessingLineStatus.Successful },
                            new FileProcessingLineModel { LineNumber = 2, LineContents = "TX-002", LineStatus = FileProcessingLineStatus.Successful },
                            new FileProcessingLineModel { LineNumber = 3, LineContents = "TX-003", LineStatus = FileProcessingLineStatus.Successful },
                            new FileProcessingLineModel { LineNumber = 4, LineContents = "TX-004", LineStatus = FileProcessingLineStatus.Successful },
                        ]
                    }
                ]
            },
            new FileImportLogDetailsModel
            {
                FileImportLogId = Guid.Parse("11111111-1111-1111-1111-111111111113"),
                ImportLogDate = today.AddDays(-12),
                Files =
                [
                    new FileProcessingFileModel
                    {
                        FileId = Guid.Parse("44444444-4444-4444-4444-444444444441"),
                        FileName = "safaricom-topup.csv",
                        FileProfile = "SafaricomTopup",
                        DateTimeUploaded = today.AddDays(-12).AddHours(9).AddMinutes(30),
                        UploadedBy = "ops@estate.com",
                        MerchantName = "Contoso Stores",
                        FileLines =
                        [
                            new FileProcessingLineModel { LineNumber = 1, LineContents = "254701000001,500", LineStatus = FileProcessingLineStatus.Successful },
                            new FileProcessingLineModel { LineNumber = 2, LineContents = "254701000002,500", LineStatus = FileProcessingLineStatus.Failed },
                            new FileProcessingLineModel { LineNumber = 3, LineContents = "254701000003,500", LineStatus = FileProcessingLineStatus.Successful },
                            new FileProcessingLineModel { LineNumber = 4, LineContents = "254701000004,500", LineStatus = FileProcessingLineStatus.Ignored },
                        ]
                    },
                    new FileProcessingFileModel
                    {
                        FileId = Guid.Parse("44444444-4444-4444-4444-444444444442"),
                        FileName = "safaricom-topup-followup.csv",
                        FileProfile = "SafaricomTopup",
                        DateTimeUploaded = today.AddDays(-12).AddHours(14),
                        UploadedBy = "ops@estate.com",
                        MerchantName = "Contoso Stores",
                        FileLines =
                        [
                            new FileProcessingLineModel { LineNumber = 1, LineContents = "254701000005,1000", LineStatus = FileProcessingLineStatus.Successful },
                            new FileProcessingLineModel { LineNumber = 2, LineContents = "254701000006,1000", LineStatus = FileProcessingLineStatus.Successful },
                        ]
                    },
                    new FileProcessingFileModel
                    {
                        FileId = Guid.Parse("44444444-4444-4444-4444-444444444443"),
                        FileName = "safaricom-topup-retry.csv",
                        FileProfile = "SafaricomTopup",
                        DateTimeUploaded = today.AddDays(-12).AddHours(16).AddMinutes(20),
                        UploadedBy = "ops@estate.com",
                        MerchantName = "Contoso Stores",
                        FileLines =
                        [
                            new FileProcessingLineModel { LineNumber = 1, LineContents = "254701000007,250", LineStatus = FileProcessingLineStatus.Ignored },
                            new FileProcessingLineModel { LineNumber = 2, LineContents = "254701000008,250", LineStatus = FileProcessingLineStatus.Successful },
                        ]
                    }
                ]
            }
        ];
    }

    private static List<FileProcessingFileModel> BuildAirtimeImportFiles(DateTime importDate)
    {
        List<FileProcessingFileModel> files = [];

        for (int fileNumber = 1; fileNumber <= 15; fileNumber++)
        {
            files.Add(new FileProcessingFileModel
            {
                FileId = Guid.Parse($"22222222-2222-2222-2222-{fileNumber:000000000000}"),
                FileName = $"airtime-import-{fileNumber:00}.csv",
                FileProfile = "AirtelTopup",
                DateTimeUploaded = importDate.AddHours(8).AddMinutes(15 + (fileNumber - 1) * 10),
                UploadedBy = fileNumber % 2 == 0 ? "john.smith@estate.com" : "jane.doe@estate.com",
                MerchantName = "Acme Retail",
                FileLines = BuildAirtimeImportLines(fileNumber)
            });
        }

        return files;
    }

    private static List<FileProcessingLineModel> BuildAirtimeImportLines(int fileNumber)
    {
        FileProcessingLineStatus secondLineStatus = fileNumber % 3 == 0
            ? FileProcessingLineStatus.Ignored
            : FileProcessingLineStatus.Failed;

        FileProcessingLineStatus thirdLineStatus = fileNumber % 2 == 0
            ? FileProcessingLineStatus.Successful
            : FileProcessingLineStatus.Ignored;

        return
        [
            new FileProcessingLineModel
            {
                LineNumber = 1,
                LineContents = $"2547001{fileNumber:000000},1000",
                LineStatus = FileProcessingLineStatus.Successful
            },
            new FileProcessingLineModel
            {
                LineNumber = 2,
                LineContents = $"2547001{fileNumber:000000},1000",
                LineStatus = secondLineStatus
            },
            new FileProcessingLineModel
            {
                LineNumber = 3,
                LineContents = $"2547001{fileNumber:000000},1000",
                LineStatus = thirdLineStatus
            }
        ];
    }
}
*/