using EstateManagementUI.IntegrationTests.Common;
using Microsoft.Playwright;
using Reqnroll;
using Shared.IntegrationTesting;
using System.Text;

namespace EstateManagementUI.IntegrationTests.Steps;

[Binding]
[Scope(Tag = "fileprocessing")]
public sealed class FileProcessingSteps
{
    private readonly IPage _page;
    private readonly TestingContext _testingContext;

    public FileProcessingSteps(IPage page, TestingContext testingContext)
    {
        _page = page;
        _testingContext = testingContext;
    }

    [When("I open the file processing screen")]
    public Task WhenIOpenTheFileProcessingScreen() => GetHelper().OpenFileProcessingScreenAsync();

    [Then("I should see the file processing heading")]
    public Task ThenIShouldSeeTheFileProcessingHeading() => GetHelper().AssertFileProcessingHeadingVisibleAsync();

    [When("I open the file upload page")]
    public Task WhenIOpenTheFileUploadPage() => GetHelper().OpenFileUploadPageAsync();

    [Then("I should see the file upload page")]
    public Task ThenIShouldSeeTheFileUploadPage() => GetHelper().AssertFileUploadPageVisibleAsync();

    [Then("the upload dropdowns should default to the placeholder option")]
    public Task ThenTheUploadDropdownsShouldDefaultToThePlaceholderOption() => GetHelper().AssertFileUploadDropdownDefaultsAsync();

    [When("I upload a batch topup file")]
    public async Task WhenIUploadABatchTopupFile(DataTable table)
    {
        StringBuilder csv = new();
        foreach (DataTableRow row in table.Rows)
        {
            csv.AppendLine($"{ReqnrollTableHelper.GetStringRowValue(row, "PhoneNumber")},{ReqnrollTableHelper.GetStringRowValue(row, "Amount")}");
        }

        await GetHelper().UploadBatchTopupFileAsync(csv.ToString());
    }

    [Then("I should see the upload success message")]
    public Task ThenIShouldSeeTheUploadSuccessMessage() => GetHelper().AssertFileUploadSuccessMessageVisibleAsync();

    private DashboardPageHelper GetHelper() => new(_page, _testingContext);
}
