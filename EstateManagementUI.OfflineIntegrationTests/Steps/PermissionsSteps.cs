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
    [Scope(Tag = "permissions")]
    public class PermissionsSteps
    {
        private readonly int EstateManagementUiPort = 5004; // Port from launchSettings.json
        private readonly BlazorUiHelpers UiHelpers;
        private readonly IPage Page;

        public PermissionsSteps(ScenarioContext scenarioContext, IObjectContainer container)
        {
            this.Page = scenarioContext.ScenarioContainer.Resolve<IPage>(scenarioContext.ScenarioInfo.Title.Replace(" ", ""));
            
            this.UiHelpers = new BlazorUiHelpers(this.Page, this.EstateManagementUiPort);
        }

        [When(@"I click on the Permissions Sidebar option")]
        public async Task WhenIClickOnThePermissionsSidebarOption()
        {
            await this.Page.ClickLinkByText("Permissions");
        }

        [Then(@"I am presented with the Permissions List screen")]
        public async Task ThenIAmPresentedWithThePermissionsListScreen()
        {
            var title = await this.Page.TitleAsync();
            title.ShouldContain("Permissions");
        }

        [Then(@"the permissions list should be displayed")]
        public async Task ThenThePermissionsListShouldBeDisplayed()
        {
            var isVisible = await this.Page.IsElementVisible("#permissionsList");
            isVisible.ShouldBeTrue();
        }

        [When(@"I click on a permission entry")]
        public async Task WhenIClickOnAPermissionEntry()
        {
            // Click the first permission entry in the list
            await this.Page.Locator("table tbody tr").First.ClickAsync();
        }

        [Then(@"I am presented with the Permission Details screen")]
        public async Task ThenIAmPresentedWithThePermissionDetailsScreen()
        {
            var title = await this.Page.TitleAsync();
            title.ShouldContain("Permission Details");
        }

        [Then(@"the permission details should be displayed")]
        public async Task ThenThePermissionDetailsShouldBeDisplayed()
        {
            var isVisible = await this.Page.IsElementVisible("#permissionDetails");
            isVisible.ShouldBeTrue();
        }

        [When(@"I click the Edit Permission button")]
        public async Task WhenIClickTheEditPermissionButton()
        {
            await this.Page.ClickButtonByText("Edit");
        }

        [Then(@"I am presented with the Edit Permission screen")]
        public async Task ThenIAmPresentedWithTheEditPermissionScreen()
        {
            var title = await this.Page.TitleAsync();
            title.ShouldContain("Edit Permission");
        }

        [When(@"I update the permission details")]
        public async Task WhenIUpdateThePermissionDetails(Table table)
        {
            foreach (var row in table.Rows)
            {
                var field = row["Field"];
                var value = row["Value"];

                await this.Page.FillIn(field, value, clearExistingText: true);
            }
        }

        [When(@"I click the Save Permission button")]
        public async Task WhenIClickTheSavePermissionButton()
        {
            await this.Page.ClickButtonByText("Save");
        }

        [Then(@"the permission should be updated successfully")]
        public async Task ThenThePermissionShouldBeUpdatedSuccessfully()
        {
            // Wait for success message or redirect
            await Task.Delay(1000);
            var pageContent = await this.Page.Locator("body").TextContentAsync();
            pageContent.ShouldContain("successfully", Case.Insensitive);
        }

        [When(@"I search for permissions by name '(.*)'")]
        public async Task WhenISearchForPermissionsByName(string searchText)
        {
            await this.Page.FillIn("SearchText", searchText);
            await this.Page.ClickButtonByText("Search");
        }

        [Then(@"the permissions list should show results containing '(.*)'")]
        public async Task ThenThePermissionsListShouldShowResultsContaining(string expectedText)
        {
            var pageContent = await this.Page.Locator("#permissionsList").TextContentAsync();
            pageContent.ShouldContain(expectedText, Case.Insensitive);
        }

        [When(@"I filter permissions by role '(.*)'")]
        public async Task WhenIFilterPermissionsByRole(string roleName)
        {
            await this.Page.SelectDropDownItemByText("RoleFilter", roleName);
            await this.Page.ClickButtonByText("Filter");
        }

        [Then(@"the permissions list should show only permissions for role '(.*)'")]
        public async Task ThenThePermissionsListShouldShowOnlyPermissionsForRole(string roleName)
        {
            // Verify filtered results
            await this.Page.WaitForElement("#permissionsList");
            var pageContent = await this.Page.Locator("#permissionsList").TextContentAsync();
            pageContent.ShouldContain(roleName);
        }
    }
}
