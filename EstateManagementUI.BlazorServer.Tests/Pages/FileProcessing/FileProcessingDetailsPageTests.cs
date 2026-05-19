using Bunit;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BusinessLogic.Requests;
using Moq;
using Shouldly;
using SimpleResults;
using FileProcessingDetails = EstateManagementUI.BlazorServer.Components.Pages.FileProcessing.Details;

namespace EstateManagementUI.BlazorServer.Tests.Pages.FileProcessing;

public class FileProcessingDetailsPageTests : BaseTest
{
    [Fact]
    public void FileProcessingDetails_RendersFilesAndLines()
    {
        List<FileImportLogDetailsModel> filteredLogs = new() {
            new() {
                FileImportLogId = FileProcessingSeedData.ImportLogs[0].FileImportLogId,
                ImportLogDate = FileProcessingSeedData.ImportLogs[0].ImportLogDateTime,
                Files = FileProcessingSeedData.ImportLogs[0].FileDetailsList.Select(f => new FileProcessingFileModel
                {
                    FileName = f.FileName,
                    FileId = f.FileId,
                    FileLines = f.FileLines.Select(line => new FileProcessingLineModel
                    {
                        LineNumber = line.LineNumber,
                        LineContents = line.LineContents,
                        LineStatus = line.LineStatus switch {
                            "S" => FileProcessingLineStatus.Successful,
                            "F" => FileProcessingLineStatus.Failed,
                            _ => FileProcessingLineStatus.Ignored
                        }
                    }).ToList()
                }).ToList()
            }
        };

        this.FileProcessingUIService.Setup(f => f.GetImportLog(It.IsAny<CorrelationId>(),
                It.IsAny<Guid>(), It.IsAny<Guid?>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(filteredLogs[0]));

        var log = FileProcessingSeedData.ImportLogs[0];

        var cut = RenderComponent<FileProcessingDetails>(parameters => parameters.Add(p => p.ImportLogId, log.FileImportLogId));

        cut.WaitForAssertion(() =>
        {
            cut.Markup.ShouldContain("Files for import log");
            cut.Markup.ShouldContain(log.FileDetailsList[0].FileName);
            cut.Markup.ShouldContain("Line Number");
            cut.Markup.ShouldContain("Successful");
            cut.Markup.ShouldContain("Failed");
            cut.Markup.ShouldContain("Ignored");
            cut.Markup.ShouldContain("Ignored lines");
            cut.Markup.ShouldContain("Showing 1-5 of 15 files");
            cut.FindAll("details").Count.ShouldBe(5);
            cut.FindAll("details").Any(detail => detail.HasAttribute("open")).ShouldBeFalse();
        });
    }

    [Fact]
    public void FileProcessingDetails_PaginatesTheFileList()
    {
        List<FileImportLogDetailsModel> filteredLogs = new() {
            new() {
                FileImportLogId = FileProcessingSeedData.ImportLogs[0].FileImportLogId,
                ImportLogDate = FileProcessingSeedData.ImportLogs[0].ImportLogDateTime,
                Files = FileProcessingSeedData.ImportLogs[0].FileDetailsList.Select(f => new FileProcessingFileModel
                {
                    FileName = f.FileName,
                    FileId = f.FileId,
                    FileLines = f.FileLines.Select(line => new FileProcessingLineModel
                    {
                        LineNumber = line.LineNumber,
                        LineContents = line.LineContents,
                        LineStatus = line.LineStatus switch {
                            "S" => FileProcessingLineStatus.Successful,
                            "F" => FileProcessingLineStatus.Failed,
                            _ => FileProcessingLineStatus.Ignored
                        }
                    }).ToList()
                }).ToList()
            }
        };

        this.FileProcessingUIService.Setup(f => f.GetImportLog(It.IsAny<CorrelationId>(),
                It.IsAny<Guid>(), It.IsAny<Guid?>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(filteredLogs[0]));

        var log = FileProcessingSeedData.ImportLogs[0];

        var cut = RenderComponent<FileProcessingDetails>(parameters => parameters.Add(p => p.ImportLogId, log.FileImportLogId));

        cut.FindAll("button").Single(button => button.TextContent.Contains("Next")).Click();

        cut.WaitForAssertion(() =>
        {
            cut.Markup.ShouldContain("Showing 6-10 of 15 files");
            cut.Markup.ShouldContain(log.FileDetailsList[5].FileName);
            cut.Markup.ShouldNotContain(log.FileDetailsList[0].FileName);
            cut.FindAll("details").Count.ShouldBe(5);
        });
    }

    [Fact]
    public void FileProcessingDetails_FileNameFilter_FiltersTheVisibleFiles()
    {
        List<FileImportLogDetailsModel> filteredLogs = new() {
            new() {
                FileImportLogId = FileProcessingSeedData.ImportLogs[0].FileImportLogId,
                ImportLogDate = FileProcessingSeedData.ImportLogs[0].ImportLogDateTime,
                Files = FileProcessingSeedData.ImportLogs[0].FileDetailsList.Select(f => new FileProcessingFileModel
                {
                    FileName = f.FileName,
                    FileId = f.FileId,
                    FileLines = f.FileLines.Select(line => new FileProcessingLineModel
                    {
                        LineNumber = line.LineNumber,
                        LineContents = line.LineContents,
                        LineStatus = line.LineStatus switch {
                            "S" => FileProcessingLineStatus.Successful,
                            "F" => FileProcessingLineStatus.Failed,
                            _ => FileProcessingLineStatus.Ignored
                        }
                    }).ToList()
                }).ToList()
            }
        };

        this.FileProcessingUIService.Setup(f => f.GetImportLog(It.IsAny<CorrelationId>(),
                It.IsAny<Guid>(), It.IsAny<Guid?>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(filteredLogs[0]));

        var log = FileProcessingSeedData.ImportLogs[0];

        var cut = RenderComponent<FileProcessingDetails>(parameters => parameters.Add(p => p.ImportLogId, log.FileImportLogId));

        cut.Find("#file-name-filter").Change(log.FileDetailsList[6].FileName);

        cut.WaitForAssertion(() =>
        {
            cut.Markup.ShouldContain("Showing 1-1 of 1 files");
            cut.Markup.ShouldContain(log.FileDetailsList[6].FileName);
            cut.FindAll("details").Count.ShouldBe(1);
        });
    }

    [Fact]
    public void FileProcessingDetails_BackButton_NavigatesToIndex()
    {
        List<FileImportLogDetailsModel> filteredLogs = new() {
            new() {
                FileImportLogId = FileProcessingSeedData.ImportLogs[0].FileImportLogId,
                ImportLogDate = FileProcessingSeedData.ImportLogs[0].ImportLogDateTime,
                Files = new List<FileProcessingFileModel>()
            }
        };

        this.FileProcessingUIService.Setup(f => f.GetImportLog(It.IsAny<CorrelationId>(),
                It.IsAny<Guid>(), It.IsAny<Guid?>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(filteredLogs[0]));

        var log = FileProcessingSeedData.ImportLogs[0];

        var cut = RenderComponent<FileProcessingDetails>(parameters => parameters.Add(p => p.ImportLogId, log.FileImportLogId));

        cut.FindAll("button").Single(button => button.TextContent.Contains("Back")).Click();

        _fakeNavigationManager.LastUri.ShouldBe("http://localhost/file-processing");
    }
}
