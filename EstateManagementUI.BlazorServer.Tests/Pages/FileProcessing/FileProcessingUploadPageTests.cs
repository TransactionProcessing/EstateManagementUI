using Bunit;
using Shouldly;
using FileProcessingUpload = EstateManagementUI.BlazorServer.Components.Pages.FileProcessing.Upload;

namespace EstateManagementUI.BlazorServer.Tests.Pages.FileProcessing;

public class FileProcessingUploadPageTests : BaseTest
{
    [Fact]
    public void FileProcessingUpload_RendersRequiredInputs()
    {
        var cut = RenderComponent<FileProcessingUpload>();

        cut.Markup.ShouldContain("Upload File");
        cut.Find("select[name='FileProfile']").ShouldNotBeNull();
        cut.Find("input[type='file']").ShouldNotBeNull();
    }

    [Fact]
    public void FileProcessingUpload_SubmitWithoutProfileAndFile_ShowsValidationErrors()
    {
        var cut = RenderComponent<FileProcessingUpload>();

        cut.Find("form").Submit();

        cut.WaitForAssertion(() =>
        {
            cut.Markup.ShouldContain("File profile is required.");
            cut.Markup.ShouldContain("A file is required.");
        });
    }

    [Fact]
    public void FileProcessingUpload_SubmitWithProfileButWithoutFile_ShowsFileValidationErrorOnly()
    {
        var cut = RenderComponent<FileProcessingUpload>();
        cut.Find("select[name='FileProfile']").Change("SafaricomTopup");

        cut.Find("form").Submit();

        cut.WaitForAssertion(() =>
        {
            cut.Markup.ShouldContain("A file is required.");
            cut.Markup.ShouldNotContain("File profile is required.");
        });
    }
}
