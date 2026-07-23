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

        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Upload File"));
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
            cut.Markup.ShouldContain("Merchant is required.");
            cut.Markup.ShouldContain("File profile is required.");
            cut.Markup.ShouldContain("A file is required.");
        });
    }

    [Fact]
    public void FileProcessingUpload_SubmitWithProfileButWithoutFile_ShowsFileValidationErrorOnly()
    {
        var cut = RenderComponent<FileProcessingUpload>();
        cut.WaitForAssertion(() => cut.Find("select[name='MerchantId']").Change("33333333-3333-3333-3333-333333333333"));
        cut.WaitForAssertion(() => cut.Find("select[name='FileProfile']").Change("22222222-2222-2222-2222-222222222222"));

        cut.Find("form").Submit();

        cut.WaitForAssertion(() =>
        {
            cut.Markup.ShouldContain("A file is required.");
            cut.Markup.ShouldNotContain("File profile is required.");
        });
    }
}
