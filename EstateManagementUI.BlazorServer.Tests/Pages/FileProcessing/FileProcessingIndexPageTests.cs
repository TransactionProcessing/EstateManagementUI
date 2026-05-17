using Bunit;
using EstateManagementUI.BlazorServer.Models;
using Shouldly;
using FileProcessingIndex = EstateManagementUI.BlazorServer.Components.Pages.FileProcessing.Index;

namespace EstateManagementUI.BlazorServer.Tests.Pages.FileProcessing;

public class FileProcessingIndexPageTests : BaseTest
{
    [Fact]
    public void FileProcessingIndex_RendersSummaryAndLogs()
    {
        var cut = RenderComponent<FileProcessingIndex>();

        cut.WaitForAssertion(() =>
        {
            cut.Markup.ShouldContain("File Processing");
            cut.Markup.ShouldContain("Upload File");
            cut.Markup.ShouldContain("Total number of files");
            cut.Markup.ShouldContain("Total number of file lines");
            cut.Markup.ShouldContain("Success rate");
            cut.Markup.ShouldContain(FileProcessingSeedData.ImportLogs[0].ImportLogDate.ToString("dd MMM yyyy"));
        });
    }

    [Fact]
    public void FileProcessingIndex_FilterByDate_UpdatesTheDisplayedLogs()
    {
        var cut = RenderComponent<FileProcessingIndex>();

        var targetDate = FileProcessingSeedData.ImportLogs[1].ImportLogDate.Date;
        var dateInputs = cut.FindAll("input[type='date']");
        dateInputs[0].Change(targetDate.ToString("yyyy-MM-dd"));
        dateInputs[1].Change(targetDate.ToString("yyyy-MM-dd"));

        cut.FindAll("button").Single(button => button.TextContent.Contains("Apply filter")).Click();

        cut.WaitForAssertion(() =>
        {
            cut.Markup.ShouldContain(FileProcessingSeedData.ImportLogs[1].ImportLogDate.ToString("dd MMM yyyy"));
            cut.Markup.ShouldNotContain(FileProcessingSeedData.ImportLogs[0].ImportLogDate.ToString("dd MMM yyyy"));
        });
    }

    [Fact]
    public void FileProcessingIndex_RowClick_NavigatesToImportLogDetails()
    {
        var cut = RenderComponent<FileProcessingIndex>();
        var firstLog = FileProcessingSeedData.ImportLogs[0];

        cut.FindAll("button").First(button => button.TextContent.Contains("View details")).Click();

        _fakeNavigationManager.LastUri.ShouldBe($"http://localhost/file-processing/import-log/{firstLog.FileImportLogId}");
    }

    [Fact]
    public void FileProcessingIndex_UploadButton_NavigatesToUploadScreen()
    {
        var cut = RenderComponent<FileProcessingIndex>();

        cut.FindAll("button").First(button => button.TextContent.Contains("Upload File")).Click();

        _fakeNavigationManager.LastUri.ShouldBe("http://localhost/file-processing/upload");
    }
}
