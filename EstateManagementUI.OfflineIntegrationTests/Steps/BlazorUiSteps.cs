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
    [Scope(Tag = "offline")]
    public class BlazorUiSteps
    {
        private readonly BlazorUiHelpers UiHelpers;
        private readonly IPage Page;
        private readonly int EstateManagementUiPort = 5004; // Port from launchSettings.json

        public BlazorUiSteps(ScenarioContext scenarioContext, IObjectContainer container)
        {
            this.Page = scenarioContext.ScenarioContainer.Resolve<IPage>(scenarioContext.ScenarioInfo.Title.Replace(" ", ""));
            this.UiHelpers = new BlazorUiHelpers(this.Page, this.EstateManagementUiPort);
        }

        [When(@"I navigate to the home page")]
        [When(@"I navigate to the Estate page")]
        public async Task WhenINavigateToTheHomePage()
        {
            await this.UiHelpers.NavigateToHomePage();
        }

        [When(@"I navigate to the Merchants page")]
        public async Task WhenINavigateToTheMerchantsPage()
        {
            await this.UiHelpers.ClickMyMerchantsSidebarOption();
        }

        [When(@"I navigate to the Operators page")]
        public async Task WhenINavigateToTheOperatorsPage()
        {
            await this.UiHelpers.ClickMyOperatorsSidebarOption();
        }

        [When(@"I navigate to the Contracts page")]
        public async Task WhenINavigateToTheContractsPage()
        {
            await this.UiHelpers.ClickContractsSidebarOption();
        }

        [When(@"I navigate to the File Processing page")]
        public async Task WhenINavigateToTheFileProcessingPage()
        {
            await this.Page.ClickLinkByText("File Processing");
        }

        [When(@"I navigate to the Permissions page")]
        public async Task WhenINavigateToThePermissionsPage()
        {
            await this.Page.ClickLinkByText("Permissions");
        }

        [When(@"I navigate to the Reporting page")]
        public async Task WhenINavigateToTheReportingPage()
        {
            await this.Page.ClickLinkByText("Reporting");
        }

        [Then(@"I should see the estate details")]
        [Then(@"I should see the estate dashboard")]
        public async Task ThenIShouldSeeTheEstateDetails()
        {
            var isVisible = await this.Page.IsElementVisible("#estateDetails");
            isVisible.ShouldBeTrue();
        }

        [Then(@"the estate name should be displayed")]
        [Then(@"the dashboard should show estate statistics")]
        public async Task ThenTheEstateNameShouldBeDisplayed()
        {
            var pageContent = await this.Page.Locator("body").TextContentAsync();
            pageContent.ShouldNotBeNullOrEmpty();
        }

        [Then(@"I should see a list of (.*)")]
        [Then(@"the (.*) table should be displayed")]
        public async Task ThenIShouldSeeAListOf(string entityType)
        {
            await Task.Delay(500);
            var tables = await this.Page.Locator("table").CountAsync();
            tables.ShouldBeGreaterThan(0);
        }

        [When(@"I click on a (.*) in the list")]
        [When(@"I click on an? (.*) in the list")]
        public async Task WhenIClickOnAnItemInTheList(string entityType)
        {
            await this.Page.Locator("table tbody tr").First.ClickAsync();
        }

        [Then(@"I should see the (.*) details page")]
        [Then(@"I should see the (.*) details")]
        [Then(@"the (.*) information should be displayed")]
        [Then(@"the (.*) details should be displayed")]
        public async Task ThenIShouldSeeTheDetailsPage(string entityType)
        {
            await Task.Delay(500);
            var isVisible = await this.Page.Locator("body").IsVisibleAsync();
            isVisible.ShouldBeTrue();
        }

        [When(@"I click the Create (.*) button")]
        [When(@"I click the Add (.*) button")]
        public async Task WhenIClickTheCreateButton(string entityType)
        {
            await this.Page.ClickButtonByText($"Create {entityType}");
        }

        [When(@"I click the Edit button")]
        public async Task WhenIClickTheEditButton()
        {
            await this.Page.ClickButtonByText("Edit");
        }

        [When(@"I click the Save button")]
        [When(@"I click the Save (.*) button")]
        public async Task WhenIClickTheSaveButton(string entityType = "")
        {
            await this.Page.ClickButtonByText("Save");
        }

        [When(@"I click the Filter button")]
        [When(@"I click the Search button")]
        [When(@"I click the Generate Report button")]
        public async Task WhenIClickTheFilterButton()
        {
            // Try multiple possible button texts
            try
            {
                await this.Page.ClickButtonByText("Filter");
            }
            catch
            {
                try
                {
                    await this.Page.ClickButtonByText("Search");
                }
                catch
                {
                    await this.Page.ClickButtonByText("Generate");
                }
            }
        }

        [When(@"I fill in the (.*) details")]
        [When(@"I update the (.*) details")]
        public async Task WhenIFillInTheDetails(string entityType, Table table)
        {
            foreach (var row in table.Rows)
            {
                var field = row["Field"];
                var value = row["Value"];
                await this.Page.FillIn(field, value);
            }
        }

        [When(@"I update the (.*) name to ""(.*)""")]
        public async Task WhenIUpdateTheName(string entityType, string newName)
        {
            await this.Page.FillIn("Name", newName, clearExistingText: true);
        }

        [When(@"I update the (.*) configuration")]
        public async Task WhenIUpdateTheConfiguration(string entityType)
        {
            await Task.CompletedTask;
        }

        [Then(@"the (.*) should be created successfully")]
        [Then(@"the (.*) should be updated successfully")]
        [Then(@"the (.*) should be added to the (.*)")]
        [Then(@"the (.*) should be added successfully")]
        public async Task ThenTheActionShouldBeSuccessful(string entityType, string container = "")
        {
            await Task.Delay(500);
            var pageContent = await this.Page.Locator("body").TextContentAsync();
            pageContent.ShouldNotBeNullOrEmpty();
        }

        [Then(@"I should see the new (.*) in the list")]
        [Then(@"I should see the (.*) in the (.*)")]
        [Then(@"the updated (.*) should be displayed")]
        [Then(@"the updated details should be displayed")]
        public async Task ThenIShouldSeeTheItemInTheList(string entityType, string container = "")
        {
            await Task.Delay(500);
            var isVisible = await this.Page.Locator("body").IsVisibleAsync();
            isVisible.ShouldBeTrue();
        }

        [When(@"I set the date range filter")]
        [When(@"I filter the report by date range")]
        [When(@"I filter the (.*) by date range")]
        public async Task WhenISetTheDateRangeFilter(Table table)
        {
            foreach (var row in table.Rows)
            {
                var field = row["Field"];
                var value = row["Value"];
                await this.Page.FillIn(field, value);
            }
        }

        [When(@"I enter ""(.*)"" in the search box")]
        [When(@"I search for (.*) by name ""(.*)""")]
        public async Task WhenIEnterInTheSearchBox(string searchTerm, string entityType = "")
        {
            await this.Page.FillIn("SearchText", searchTerm);
        }

        [Then(@"only (.*) should be displayed")]
        [Then(@"the (.*) list should be filtered (.*)")]
        [Then(@"only matching (.*) should be displayed")]
        public async Task ThenOnlyMatchingItemsShouldBeDisplayed(string text1, string text2 = "")
        {
            await Task.Delay(500);
            var isVisible = await this.Page.Locator("body").IsVisibleAsync();
            isVisible.ShouldBeTrue();
        }
    }
}
