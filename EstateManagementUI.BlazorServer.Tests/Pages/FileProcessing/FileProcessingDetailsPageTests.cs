using Bunit;
using EstateManagementUI.BlazorServer.Models;
using Shouldly;
using FileProcessingDetails = EstateManagementUI.BlazorServer.Components.Pages.FileProcessing.Details;

namespace EstateManagementUI.BlazorServer.Tests.Pages.FileProcessing;

public class FileProcessingDetailsPageTests : BaseTest
{
    [Fact]
    public void FileProcessingDetails_RendersFilesAndLines()
    {
        var log = FileProcessingSeedData.ImportLogs[0];

        var cut = RenderComponent<FileProcessingDetails>(parameters => parameters.Add(p => p.ImportLogId, log.FileImportLogId));

        cut.WaitForAssertion(() =>
        {
            cut.Markup.ShouldContain("Files for import log");
            cut.Markup.ShouldContain(log.Files[0].FileName);
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
        var log = FileProcessingSeedData.ImportLogs[0];

        var cut = RenderComponent<FileProcessingDetails>(parameters => parameters.Add(p => p.ImportLogId, log.FileImportLogId));

        cut.FindAll("button").Single(button => button.TextContent.Contains("Next")).Click();

        cut.WaitForAssertion(() =>
        {
            cut.Markup.ShouldContain("Showing 6-10 of 15 files");
            cut.Markup.ShouldContain(log.Files[5].FileName);
            cut.Markup.ShouldNotContain(log.Files[0].FileName);
            cut.FindAll("details").Count.ShouldBe(5);
        });
    }

    [Fact]
    public void FileProcessingDetails_BackButton_NavigatesToIndex()
    {
        var log = FileProcessingSeedData.ImportLogs[0];

        var cut = RenderComponent<FileProcessingDetails>(parameters => parameters.Add(p => p.ImportLogId, log.FileImportLogId));

        cut.FindAll("button").Single(button => button.TextContent.Contains("Back")).Click();

        _fakeNavigationManager.LastUri.ShouldBe("http://localhost/file-processing");
    }
}
