using Bunit;
using EstateManagementUI.BlazorServer.Factories;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BusinessLogic.Client;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using Moq;
using Shouldly;
using SimpleResults;
using FileProcessingIndex = EstateManagementUI.BlazorServer.Components.Pages.FileProcessing.Index;

namespace EstateManagementUI.BlazorServer.Tests.Pages.FileProcessing;

public class FileProcessingIndexPageTests : BaseTest
{
    [Fact]
    public void FileProcessingIndex_RendersSummaryAndLogs()
    {
        List<FileImportLogDetailsModel> filteredLogs = new() {
            new() {
                FileImportLogId = FileProcessingSeedData.ImportLogs[0].FileImportLogId,
                ImportLogDate = FileProcessingSeedData.ImportLogs[0].ImportLogDateTime,
                Files = new List<FileProcessingFileModel>()
            }
        };

        this.FileProcessingUIService.Setup(f => f.GetImportLogList(It.IsAny<CorrelationId>(),
                It.IsAny<Guid>(), It.IsAny<Guid?>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(Result.Success(filteredLogs));

        var cut = RenderComponent<FileProcessingIndex>();

        cut.WaitForAssertion(() =>
        {
            cut.Markup.ShouldContain("File Processing");
            cut.Markup.ShouldContain("Upload File");
            cut.Markup.ShouldContain("Total number of files");
            cut.Markup.ShouldContain("Total number of file lines");
            cut.Markup.ShouldContain("Success rate");
            cut.Markup.ShouldContain(FileProcessingSeedData.ImportLogs[0].ImportLogDateTime.ToString("dd MMM yyyy"));
        });
    }

    [Fact]
    public void FileProcessingIndex_FilterByDate_UpdatesTheDisplayedLogs() {

        List<FileImportLogDetailsModel> filteredLogs = new() {
            new() {
                FileImportLogId = FileProcessingSeedData.ImportLogs[1].FileImportLogId,
                ImportLogDate = FileProcessingSeedData.ImportLogs[1].ImportLogDateTime,
                Files = new List<FileProcessingFileModel>()
            }
        };

        this.FileProcessingUIService.Setup(f => f.GetImportLogList(It.IsAny<CorrelationId>(), 
            It.IsAny<Guid>(), It.IsAny<Guid?>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(Result.Success(filteredLogs));
        
        var cut = RenderComponent<FileProcessingIndex>();

        var targetDate = FileProcessingSeedData.ImportLogs[1].ImportLogDateTime.Date;
        // Use InvokeAsync to ensure DOM updates and event handler registration
        // happen synchronously with the test renderer. Re-query inputs after
        // each render to avoid stale element references (unknown event handler IDs).
        cut.InvokeAsync(() => cut.FindAll("input[type='date']")[0].Change(targetDate.ToString("yyyy-MM-dd")));
        var dateInputs = cut.FindAll("input[type='date']");
        cut.InvokeAsync(() => dateInputs[1].Change(targetDate.ToString("yyyy-MM-dd")));

        cut.InvokeAsync(() => cut.FindAll("button").Single(button => button.TextContent.Contains("Apply filter")).Click());

        cut.WaitForAssertion(() =>
        {
            cut.Markup.ShouldContain(FileProcessingSeedData.ImportLogs[1].ImportLogDateTime.ToString("dd MMM yyyy"));
            cut.Markup.ShouldNotContain(FileProcessingSeedData.ImportLogs[0].ImportLogDateTime.ToString("dd MMM yyyy"));
        });
    }

    [Fact]
    public void FileProcessingIndex_RowClick_NavigatesToImportLogDetails()
    {
        List<FileImportLogDetailsModel> filteredLogs = new() {
            new() {
                FileImportLogId = FileProcessingSeedData.ImportLogs[0].FileImportLogId,
                ImportLogDate = FileProcessingSeedData.ImportLogs[0].ImportLogDateTime,
                Files = new List<FileProcessingFileModel>()
            }
        };

        this.FileProcessingUIService.Setup(f => f.GetImportLogList(It.IsAny<CorrelationId>(),
                It.IsAny<Guid>(), It.IsAny<Guid?>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(Result.Success(filteredLogs));

        var cut = RenderComponent<FileProcessingIndex>();
        var firstLog = FileProcessingSeedData.ImportLogs[0];

        cut.FindAll("button").First(button => button.TextContent.Contains("View details")).Click();

        _fakeNavigationManager.LastUri.ShouldBe($"http://localhost/file-processing/import-log/{firstLog.FileImportLogId}");
    }

    [Fact]
    public void FileProcessingIndex_UploadButton_NavigatesToUploadScreen()
    {
        List<FileImportLogDetailsModel> filteredLogs = new() {
            new() {
                FileImportLogId = FileProcessingSeedData.ImportLogs[0].FileImportLogId,
                ImportLogDate = FileProcessingSeedData.ImportLogs[0].ImportLogDateTime,
                Files = new List<FileProcessingFileModel>()
            }
        };

        this.FileProcessingUIService.Setup(f => f.GetImportLogList(It.IsAny<CorrelationId>(),
                It.IsAny<Guid>(), It.IsAny<Guid?>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(Result.Success(filteredLogs));

        var cut = RenderComponent<FileProcessingIndex>();

        cut.FindAll("button").First(button => button.TextContent.Contains("Upload File")).Click();

        _fakeNavigationManager.LastUri.ShouldBe("http://localhost/file-processing/upload");
    }
}
