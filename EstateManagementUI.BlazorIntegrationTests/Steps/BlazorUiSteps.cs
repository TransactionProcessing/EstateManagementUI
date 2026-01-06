using Reqnroll;
using System;
using System.Threading.Tasks;
using Shared.IntegrationTesting;
using EstateManagementUI.BlazorIntegrationTests.Common;
using EstateManagementUI.IntegrationTests.Common;
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

        [When(@"I login with the username '(.*)' and password '(.*)'")]
        public async Task WhenILoginWithTheUsernameAndPassword(String userName, String password)
        {
            String username = userName.Replace("[id]", this.TestingContext.DockerHelper.TestId.ToString("N"));
            await this.UiHelpers.Login(username, password);
        }

        [When("I click on the New Operator Button")]
        public async Task WhenIClickOnTheNewOperatorButton()
        {
            await this.UiHelpers.ClickTheNewOperatorButton();
        }

        [When("I click on the New Merchant Button")]
        public async Task WhenIClickOnTheNewMerchantButton()
        {
            await this.UiHelpers.ClickTheNewMerchantButton();
        }

        [Then("the Edit Merchant Screen is displayed")]
        public async Task ThenTheEditMerchantScreenIsDisplayed()
        {
            await this.UiHelpers.VerifyOnTheEditMerchantScreen();
        }

        [Then("the Make Deposit Screen is displayed")]
        public async Task ThenTheMakeDepositScreenIsDisplayed()
        {
            await this.UiHelpers.VerifyOnTheMakeDepositScreen();
        }

        [When("I click on the Operators tab")]
        public async Task WhenIClickOnTheOperatorsTab()
        {
            await this.UiHelpers.ClickOnTheMerchantOperatorsTab();
        }

        [When("I click on the Contracts tab")]
        public async Task WhenIClickOnTheContractsTab()
        {
            await this.UiHelpers.ClickOnTheMerchantContractsTab();
        }

        [When("I click on the Devices tab")]
        public async Task WhenIClickOnTheDevicesTab()
        {
            await this.UiHelpers.ClickOnTheMerchantDevicesTab();
        }

        [When("I click on the Add Device Button")]
        public async Task WhenIClickOnTheAddDeviceButton()
        {
            await this.UiHelpers.ClickTheAddDeviceButton();
        }

        [Then("I am presented with the Merchants Operator List Screen")]
        public async Task ThenIAmPresentedWithTheMerchantsOperatorListScreen()
        {
            await this.UiHelpers.VerifyOnMerchantOperatorsTab();
        }

        [Then("I am presented with the Merchants Contract List Screen")]
        public async Task ThenIAmPresentedWithTheMerchantsContractListScreen()
        {
            await this.UiHelpers.VerifyOnMerchantContractsTab();
        }

        [Then("I am presented with the Merchants Device List Screen")]
        public async Task ThenIAmPresentedWithTheMerchantsDeviceListScreen()
        {
            await this.UiHelpers.VerifyOnMerchantDevicesTab();
        }

        [Then("the View Merchant Screen is displayed")]
        public async Task ThenTheViewMerchantScreenIsDisplayed()
        {
            await this.UiHelpers.VerifyOnTheViewMerchantScreen();
        }

        [When("click the Assign Operator button")]
        public async Task WhenClickTheAssignOperatorButton()
        {
            await this.UiHelpers.ClickTheAssignOperatorButton();
        }

        [Then("the Add New Merchant Screen is displayed")]
        public async Task ThenTheAddNewMerchantScreenIsDisplayed()
        {
            await this.UiHelpers.VerifyOnTheNewMerchantScreen();
        }

        [When("I click on the Edit Operator Button for {string}")]
        public async Task WhenIClickOnTheEditOperatorButtonFor(string operatorName)
        {
            await this.UiHelpers.ClickTheEditOperatorButton(operatorName);
        }

        [When("I click on the Edit Merchant Button for {string}")]
        public async Task WhenIClickOnTheEditMerchantButtonFor(string merchantName)
        {
            await this.UiHelpers.ClickTheEditMerchantButton(merchantName);
        }

        [When("I click on the Make Deposit Button for {string}")]
        public async Task WhenIClickOnTheMakeDepositButtonFor(string merchantName)
        {
            await this.UiHelpers.ClickTheMakeDepositButtonFor(merchantName);
        }

        [Then("the Edit Operator Screen is displayed")]
        public async Task ThenTheEditOperatorScreenIsDisplayed()
        {
            await this.UiHelpers.VerifyOnTheEditOperatorScreen();
        }

        [Then("the New Operator Screen is displayed")]
        public async Task ThenTheNewOperatorScreenIsDisplayed()
        {
            await this.UiHelpers.VerifyOnTheNewOperatorScreen();
        }

        [Then(@"the following merchants details are in the list")]
        public async Task ThenTheFollowingMerchantsDetailsAreInTheList(DataTable table)
        {
            List<MerchantDetails> merchantDetailsList = new List<MerchantDetails>();
            foreach (DataTableRow tableRow in table.Rows)
            {
                MerchantDetails m = new MerchantDetails(
                    tableRow["MerchantName"],
                    tableRow["SettlementSchedule"],
                    tableRow["ContactName"],
                    tableRow["AddressLine1"],
                    tableRow["Town"]);
                merchantDetailsList.Add(m);
            }

            await this.UiHelpers.VerifyMerchantDetailsAreInTheList(merchantDetailsList);
        }

        [Then(@"the following operator details are in the list")]
        public async Task ThenTheFollowingOperatorDetailsAreInTheList(DataTable table)
        {
            List<(String, String, String, String)> operatorsList = new();
            foreach (DataTableRow tableRow in table.Rows)
            {
                operatorsList.Add((ReqnrollTableHelper.GetStringRowValue(tableRow, "OperatorName"),
                    ReqnrollTableHelper.GetStringRowValue(tableRow, "RequireCustomMerchantNumber"),
                    ReqnrollTableHelper.GetStringRowValue(tableRow, "RequireCustomTerminalNumber"),
                    null));
            }

            await this.UiHelpers.VerifyOperatorDetailsAreInTheList("operatorList", operatorsList);
        }

        [Then("the following operators are displayed in the list")]
        public async Task ThenTheFollowingOperatorsAreDisplayedInTheList(DataTable table)
        {
            List<(String, String, String, String)> operatorsList = new();
            foreach (DataTableRow tableRow in table.Rows)
            {
                operatorsList.Add((ReqnrollTableHelper.GetStringRowValue(tableRow, "OperatorName"),
                    ReqnrollTableHelper.GetStringRowValue(tableRow, "MerchantNumber"),
                    ReqnrollTableHelper.GetStringRowValue(tableRow, "TerminalNumber"),
                    ReqnrollTableHelper.GetStringRowValue(tableRow, "IsDeleted")));
            }

            await this.UiHelpers.VerifyOperatorDetailsAreInTheList("merchantOperatorList", operatorsList);
        }

        [Then("the following contracts are displayed in the list")]
        public async Task ThenTheFollowingContractsAreDisplayedInTheList(DataTable dataTable)
        {
            List<(String, String)> contractsList = new();
            foreach (DataTableRow tableRow in dataTable.Rows)
            {
                contractsList.Add((ReqnrollTableHelper.GetStringRowValue(tableRow, "ContractName"),
                    ReqnrollTableHelper.GetStringRowValue(tableRow, "IsDeleted")));
            }

            await this.UiHelpers.VerifyContractDetailsAreInTheList("merchantContractList", contractsList);
        }

        [When("I click on the New Contract Button")]
        public async Task WhenIClickOnTheNewContractButton()
        {
            await this.UiHelpers.ClickAddNewContractButton();
        }

        [Then("the New Contract Screen is displayed")]
        public async Task ThenTheNewContractScreenIsDisplayed()
        {
            await this.UiHelpers.VerifyOnTheNewContractScreen();
        }

        [When("I enter the following details for the new Merchant")]
        public async Task WhenIEnterTheFollowingDetailsForTheNewMerchant(DataTable dataTable)
        {
            DataTableRow row = dataTable.Rows.Single();
            
            await this.UiHelpers.FillInNewMerchantForm(
                ReqnrollTableHelper.GetStringRowValue(row, "MerchantName"),
                ReqnrollTableHelper.GetStringRowValue(row, "SettlementSchedule"),
                ReqnrollTableHelper.GetStringRowValue(row, "AddressLine1"),
                ReqnrollTableHelper.GetStringRowValue(row, "Town"),
                ReqnrollTableHelper.GetStringRowValue(row, "Region"),
                ReqnrollTableHelper.GetStringRowValue(row, "Country"),
                ReqnrollTableHelper.GetStringRowValue(row, "ContactName"),
                ReqnrollTableHelper.GetStringRowValue(row, "EmailAddress"));
        }

        [When("click the Save Merchant button")]
        public async Task WhenClickTheSaveMerchantButton()
        {
            await this.UiHelpers.ClickTheSaveMerchantButton();
        }

        [When("I enter the following details for the updated Merchant")]
        public async Task WhenIEnterTheFollowingDetailsForTheUpdatedMerchant(DataTable dataTable)
        {
            foreach (DataTableRow row in dataTable.Rows)
            {
                String tab = ReqnrollTableHelper.GetStringRowValue(row, "Tab");
                String field = ReqnrollTableHelper.GetStringRowValue(row, "Field");
                String value = ReqnrollTableHelper.GetStringRowValue(row, "Value");
                
                await this.UiHelpers.UpdateMerchantField(tab, field, value);
            }
        }

        [When("I enter the following details for the deposit")]
        public async Task WhenIEnterTheFollowingDetailsForTheDeposit(DataTable dataTable)
        {
            DataTableRow row = dataTable.Rows.Single();
            
            await this.UiHelpers.FillInDepositForm(
                ReqnrollTableHelper.GetStringRowValue(row, "Amount"),
                ReqnrollTableHelper.GetStringRowValue(row, "Date"),
                ReqnrollTableHelper.GetStringRowValue(row, "Reference"));
        }

        [When("click the Make Deposit button")]
        public async Task WhenClickTheMakeDepositButton()
        {
            await this.UiHelpers.ClickTheMakeDepositButton();
        }

        [When("I click on the View Merchant Button for {string}")]
        public async Task WhenIClickOnTheViewMerchantButtonFor(string merchantName)
        {
            await this.UiHelpers.ClickTheViewMerchantButton(merchantName);
        }
    }
}
