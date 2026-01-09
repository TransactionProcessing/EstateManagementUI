using Reqnroll;
using System;
using System.Threading.Tasks;
using EstateManagementUI.OfflineIntegrationTests.Common;
using Microsoft.Playwright;
using Reqnroll.BoDi;
using Shouldly;

namespace EstateManagementUI.OfflineIntegrationTests.Steps
{
    [Binding]
    [Scope(Tag = "fileprocessing")]
    public class FileProcessingSteps
    {
        private readonly BlazorUiHelpers UiHelpers;
        private readonly IPage Page;
        private readonly int EstateManagementUiPort = 5004; // Port from launchSettings.json

        public FileProcessingSteps(ScenarioContext scenarioContext, IObjectContainer container)
        {
            this.Page = scenarioContext.ScenarioContainer.Resolve<IPage>(scenarioContext.ScenarioInfo.Title.Replace(" ", ""));
            this.UiHelpers = new BlazorUiHelpers(this.Page, this.EstateManagementUiPort);
        }

        [When(@"I click on the File Processing Sidebar option")]
        public async Task WhenIClickOnTheFileProcessingSidebarOption()
        {
            await this.Page.ClickLinkByText("File Processing");
        }

        [Then(@"I am presented with the File Processing List screen")]
        public async Task ThenIAmPresentedWithTheFileProcessingListScreen()
        {
            var title = await this.Page.TitleAsync();
            title.ShouldContain("File Processing");
        }

        [Then(@"the File Processing list should be displayed")]
        public async Task ThenTheFileProcessingListShouldBeDisplayed()
        {
            var isVisible = await this.Page.IsElementVisible("#fileProcessingList");
            isVisible.ShouldBeTrue();
        }

        [When(@"I click on a file processing entry")]
        public async Task WhenIClickOnAFileProcessingEntry()
        {
            // Click the first file processing entry in the list
            await this.Page.Locator("table tbody tr").First.ClickAsync();
        }

        [Then(@"I am presented with the File Processing Details screen")]
        public async Task ThenIAmPresentedWithTheFileProcessingDetailsScreen()
        {
            var title = await this.Page.TitleAsync();
            title.ShouldContain("File Processing Details");
        }

        [Then(@"the file processing details should be displayed")]
        public async Task ThenTheFileProcessingDetailsShouldBeDisplayed()
        {
            var isVisible = await this.Page.IsElementVisible("#fileProcessingDetails");
            isVisible.ShouldBeTrue();
        }

        [When(@"I filter file processing by date range from '(.*)' to '(.*)'")]
        public async Task WhenIFilterFileProcessingByDateRange(string startDate, string endDate)
        {
            await this.Page.FillIn("StartDate", startDate);
            await this.Page.FillIn("EndDate", endDate);
            await this.Page.ClickButtonByText("Filter");
        }

        [Then(@"the file processing list should be filtered by the date range")]
        public async Task ThenTheFileProcessingListShouldBeFilteredByTheDateRange()
        {
            // Verify the list has been updated
            await this.Page.WaitForElement("#fileProcessingList");
            var isVisible = await this.Page.IsElementVisible("#fileProcessingList");
            isVisible.ShouldBeTrue();
        }

        [When(@"I search for file processing by name '(.*)'")]
        public async Task WhenISearchForFileProcessingByName(string fileName)
        {
            await this.Page.FillIn("SearchText", fileName);
            await this.Page.ClickButtonByText("Search");
        }

        [Then(@"the file processing list should show results for '(.*)'")]
        public async Task ThenTheFileProcessingListShouldShowResultsFor(string fileName)
        {
            var pageContent = await this.Page.Locator("body").TextContentAsync();
            pageContent.ShouldContain(fileName);
        }
    }
}
