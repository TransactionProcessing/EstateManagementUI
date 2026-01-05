using Reqnroll;
using System;
using System.Threading.Tasks;
using Shared.IntegrationTesting;
using EstateManagementUI.BlazorIntegrationTests.Common;
using Microsoft.Playwright;
using Reqnroll.BoDi;

namespace EstateManagementUI.BlazorIntegrationTests.Steps
{
    [Binding]
    [Scope(Tag = "uigeneral")]
    public class BlazorUiSteps
    {
        private readonly TestingContext TestingContext;
        private readonly BlazorUiHelpers UiHelpers;

        public BlazorUiSteps(ScenarioContext scenarioContext, TestingContext testingContext, IObjectContainer container)
        {
            var page = scenarioContext.ScenarioContainer.Resolve<IPage>(scenarioContext.ScenarioInfo.Title.Replace(" ", ""));

            this.TestingContext = testingContext;
            this.UiHelpers = new BlazorUiHelpers(page, this.TestingContext.DockerHelper.EstateManagementUiPort);
        }

        [Given(@"I am on the application home page")]
        public async Task GivenIAmOnTheApplicationHomePage()
        {
            await this.UiHelpers.NavigateToHomePage();
        }

        [Given(@"I click on the My Contracts sidebar option")]
        public async Task GivenIClickOnTheMyContractsSidebarOption()
        {
            await this.UiHelpers.ClickContractsSidebarOption();
        }

        [Given(@"I click on the My Estate sidebar option")]
        public async Task GivenIClickOnTheMyEstateSidebarOption()
        {
            await this.UiHelpers.ClickMyEstateSidebarOption();
        }

        [Given(@"I click on the My Merchants sidebar option")]
        public async Task GivenIClickOnTheMyMerchantsSidebarOption()
        {
            await this.UiHelpers.ClickMyMerchantsSidebarOption();
        }

        [Given(@"I click on the My Operators sidebar option")]
        public async Task GivenIClickOnTheMyOperatorsSidebarOption()
        {
            await this.UiHelpers.ClickMyOperatorsSidebarOption();
        }

        [Given(@"I click on the Sign In Button")]
        public async Task GivenIClickOnTheSignInButton()
        {
            await this.UiHelpers.ClickOnTheSignInButton();
        }

        [Then(@"I am presented with a login screen")]
        public async Task ThenIAmPresentedWithALoginScreen()
        {
            await this.UiHelpers.VerifyOnTheLoginScreen();
        }

        [Then(@"I am presented with the Contracts List Screen")]
        public async Task ThenIAmPresentedWithTheContractsListScreen()
        {
            await this.UiHelpers.VerifyOnTheContractsListScreen();
        }

        [Then("the Contract Products List Screen is displayed")]
        public async Task ThenTheContractProductsListScreenIsDisplayed()
        {
            await this.UiHelpers.VerifyOnTheContractProductsListScreen();
        }

        [Then("the Contract Products Transaction Fees List Screen is displayed")]
        public async Task ThenTheContractProductsTransactionFeesListScreenIsDisplayed()
        {
            await this.UiHelpers.VerifyOnTheContractProductsFeesListScreen();
        }

        [Then(@"I am presented with the Estate Administrator Dashboard")]
        public async Task ThenIAmPresentedWithTheEstateAdministratorDashboard()
        {
            await this.UiHelpers.VerifyOnTheDashboard();
        }

        [When("I click the Save Product Button")]
        public async Task WhenIClickTheSaveProductButton()
        {
            await this.UiHelpers.ClickTheSaveProductButton();
        }

        [Then(@"I am presented with the View Estate Page")]
        public async Task ThenIamPresentedWithTheViewEstatePage()
        {
            await this.UiHelpers.VerifyOnTheEstateDetailsScreen();
        }

        [Then(@"I am presented with the Merchants List Screen")]
        public async Task ThenIAmPresentedWithTheMerchantsListScreen()
        {
            await this.UiHelpers.VerifyOnTheMerchantsListScreen();
        }

        [Then(@"I am presented with the Operators List Screen")]
        public async Task ThenIAmPresentedWithTheOperatorsListScreen()
        {
            await this.UiHelpers.VerifyOnTheOperatorsListScreen();
        }

        [Then(@"My Estate Details will be shown")]
        public async Task ThenMyEstateDetailsWillBeShown(DataTable table)
        {
            DataTableRow tableRow = table.Rows.Single();
            String estateName = ReqnrollTableHelper.GetStringRowValue(tableRow, "EstateName").Replace("[id]", this.TestingContext.DockerHelper.TestId.ToString("N"));
            await this.UiHelpers.VerifyTheCorrectEstateDetailsAreDisplayed(estateName);
        }

        [Then(@"the following contract details are in the list")]
        public async Task ThenTheFollowingContractDetailsAreInTheList(DataTable table)
        {
            List<(String, String, Int32)> contractDescriptions = new List<(String, String, Int32)>();
            foreach (DataTableRow tableRow in table.Rows)
            {
                contractDescriptions.Add((ReqnrollTableHelper.GetStringRowValue(tableRow, "Description"),
                    ReqnrollTableHelper.GetStringRowValue(tableRow, "OperatorName"),
                    ReqnrollTableHelper.GetIntValue(tableRow, "Products")));
            }

            await this.UiHelpers.VerifyTheContractDetailsAreInTheList(contractDescriptions);
        }
    }
}
