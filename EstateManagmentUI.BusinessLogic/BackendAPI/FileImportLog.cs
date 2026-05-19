using EstateManagementUI.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using EstateManagementUI.BusinessLogic.BackendAPI;
using static EstateManagementUI.BusinessLogic.Models.FileProcessingModels;

namespace EstateManagementUI.BusinessLogic.BackendAPI
{
    public class FileImportLog
    {
        public Guid FileImportLogId { get; set; }
        public DateTime ImportLogDateTime { get; set; }
        public List<FileDetails> FileDetailsList { get; set; }  
    }

    public class FileDetails {
        public Guid FileId { get; set; }
        public string FileName { get; set; }
        public string FileProfile { get; set; } 
        public DateTime DateTimeUploaded { get; set; }
        public Guid UserId { get; set; }
        public string UploadedBy { get; set; }
        public Guid MerchantId { get; set; }
        public string MerchantName { get; set; } 
        public List<FileLine> FileLines { get; set; } = new List<FileLine>();
    }

    public sealed class FileLine
    {
        public int LineNumber { get; set; }
        public string LineContents { get; set; }
        public String LineStatus { get; set; }
    }
}

[ExcludeFromCodeCoverage]
public static class FileProcessingSeedData
{
    public static IReadOnlyList<FileImportLog> ImportLogs { get; } = BuildImportLogs();

    public static IReadOnlyList<FileImportLog> FilterByDate(DateTime? startDate, DateTime? endDate)
    {
        DateTime start = startDate?.Date ?? DateTime.MinValue.Date;
        DateTime end = endDate?.Date ?? DateTime.MaxValue.Date;

        return ImportLogs
            .Where(log => log.ImportLogDateTime.Date >= start && log.ImportLogDateTime.Date <= end)
            .OrderByDescending(log => log.ImportLogDateTime)
            .ToList();
    }

    public static FileImportLog? GetImportLog(Guid fileImportLogId) =>
        ImportLogs.FirstOrDefault(log => log.FileImportLogId == fileImportLogId);

    private static IReadOnlyList<FileImportLog> BuildImportLogs()
    {
        DateTime today = DateTime.Today;

        return
        [
            new FileImportLog
            {
                FileImportLogId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                ImportLogDateTime = today.AddDays(-1),
                FileDetailsList = BuildAirtimeImportFiles(today.AddDays(-1))
            },
            new FileImportLog
            {
                FileImportLogId = Guid.Parse("11111111-1111-1111-1111-111111111112"),
                ImportLogDateTime = today.AddDays(-5),
                FileDetailsList =
                [
                    new FileDetails()
                    {
                        FileId = Guid.Parse("33333333-3333-3333-3333-333333333331"),
                        FileName = "settlement-batch.csv",
                        FileProfile = "SettlementFile",
                        DateTimeUploaded = today.AddDays(-5).AddHours(11),
                        UploadedBy = "finance@estate.com",
                        MerchantName = "Northwind Traders",
                        FileLines =
                        [
                            new FileLine { LineNumber = 1, LineContents = "TX-001", LineStatus = "S" },
                            new FileLine { LineNumber = 2, LineContents = "TX-002", LineStatus = "S" },
                            new FileLine { LineNumber = 3, LineContents = "TX-003", LineStatus = "S" },
                            new FileLine { LineNumber = 4, LineContents = "TX-004", LineStatus = "S" },
                        ]
                    }
                ]
            },
            new FileImportLog
            {
                FileImportLogId = Guid.Parse("11111111-1111-1111-1111-111111111113"),
                ImportLogDateTime = today.AddDays(-12),
                FileDetailsList =
                [
                    new FileDetails
                    {
                        FileId = Guid.Parse("44444444-4444-4444-4444-444444444441"),
                        FileName = "safaricom-topup.csv",
                        FileProfile = "SafaricomTopup",
                        DateTimeUploaded = today.AddDays(-12).AddHours(9).AddMinutes(30),
                        UploadedBy = "ops@estate.com",
                        MerchantName = "Contoso Stores",
                        FileLines =
                        [
                            new FileLine { LineNumber = 1, LineContents = "254701000001,500", LineStatus = "S" },
                            new FileLine { LineNumber = 2, LineContents = "254701000002,500", LineStatus = "F" },
                            new FileLine { LineNumber = 3, LineContents = "254701000003,500", LineStatus = "S" },
                            new FileLine { LineNumber = 4, LineContents = "254701000004,500", LineStatus = "I" },
                        ]
                    },
                    new FileDetails
                    {
                        FileId = Guid.Parse("44444444-4444-4444-4444-444444444442"),
                        FileName = "safaricom-topup-followup.csv",
                        FileProfile = "SafaricomTopup",
                        DateTimeUploaded = today.AddDays(-12).AddHours(14),
                        UploadedBy = "ops@estate.com",
                        MerchantName = "Contoso Stores",
                        FileLines =
                        [
                            new FileLine { LineNumber = 1, LineContents = "254701000005,1000", LineStatus = "S" },
                            new FileLine { LineNumber = 2, LineContents = "254701000006,1000", LineStatus = "S" },
                        ]
                    },
                    new FileDetails
                    {
                        FileId = Guid.Parse("44444444-4444-4444-4444-444444444443"),
                        FileName = "safaricom-topup-retry.csv",
                        FileProfile = "SafaricomTopup",
                        DateTimeUploaded = today.AddDays(-12).AddHours(16).AddMinutes(20),
                        UploadedBy = "ops@estate.com",
                        MerchantName = "Contoso Stores",
                        FileLines =
                        [
                            new FileLine { LineNumber = 1, LineContents = "254701000007,250", LineStatus = "I" },
                            new FileLine { LineNumber = 2, LineContents = "254701000008,250", LineStatus = "S" },
                        ]
                    }
                ]
            }
        ];
    }

    private static List<FileDetails> BuildAirtimeImportFiles(DateTime importDate)
    {
        List<FileDetails> files = [];

        for (int fileNumber = 1; fileNumber <= 15; fileNumber++)
        {
            files.Add(new FileDetails
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

    private static List<FileLine> BuildAirtimeImportLines(int fileNumber)
    {
        FileProcessingLineStatus secondLineStatus = fileNumber % 3 == 0
            ? FileProcessingLineStatus.Ignored
            : FileProcessingLineStatus.Failed;

        FileProcessingLineStatus thirdLineStatus = fileNumber % 2 == 0
            ? FileProcessingLineStatus.Successful
            : FileProcessingLineStatus.Ignored;

        return
        [
            new FileLine
            {
                LineNumber = 1,
                LineContents = $"2547001{fileNumber:000000},1000",
                LineStatus = "S"
            },
            new FileLine
            {
                LineNumber = 2,
                LineContents = $"2547001{fileNumber:000000},1000",
                LineStatus = secondLineStatus == FileProcessingLineStatus.Ignored ? "I" : "F"
            },
            new FileLine
            {
                LineNumber = 3,
                LineContents = $"2547001{fileNumber:000000},1000",
                LineStatus = thirdLineStatus == FileProcessingLineStatus.Successful ? "S" : "I"
            }
        ];
    }
}