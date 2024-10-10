using Reqnroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.IntegrationTesting;
using EstateManagement.IntegrationTesting.Helpers;
using EstateManagementUI.IntegrationTests.Common;
using OpenQA.Selenium;
using Reqnroll.BoDi;
using SecurityService.IntegrationTesting.Helpers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EstateManagementUI.IntegrationTests.Steps
{
    [Binding]
    [Scope(Tag = "uigeneral")]
    public class EstateManagementUiSteps
    {
        private readonly TestingContext TestingContext;
        private readonly EstateManagementUiHelpers UiHelpers;

        public EstateManagementUiSteps(ScenarioContext scenarioContext, TestingContext testingContext, IObjectContainer container)
        {
            var webDriver = scenarioContext.ScenarioContainer.Resolve<IWebDriver>(scenarioContext.ScenarioInfo.Title.Replace(" ", ""));

            this.TestingContext = testingContext;
            this.UiHelpers = new EstateManagementUiHelpers(webDriver, this.TestingContext.DockerHelper.EstateManagementUiPort);
        }

        [Given(@"I am on the application home page")]
        public void GivenIAmOnTheApplicationHomePage()
        {
            this.UiHelpers.NavigateToHomePage();
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
        public async Task GivenIClickOnTheMyOperatorsSidebarOption() {
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
            //String estateReference = ReqnrollTableHelper.GetStringRowValue(tableRow, "EstateReference");
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
        
        [Then(@"the following merchants details are in the list")]
        public async Task ThenTheFollowingMerchantsDetailsAreInTheList(DataTable table)
        {
            List<MerchantDetails> merchantDetailsList = new List<MerchantDetails>();
            foreach (DataTableRow tableRow in table.Rows) {
                MerchantDetails m = new MerchantDetails(tableRow["MerchantName"], tableRow["SettlementSchedule"],
                    tableRow["ContactName"], tableRow["AddressLine1"], tableRow["Town"]);
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

            await this.UiHelpers.VerifyOperatorDetailsAreInTheList("operatorsList", operatorsList);
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

        [When("I click on the Add Operator Button")]
        public async Task WhenIClickOnTheAddOperatorButton()
        {
            await this.UiHelpers.ClickTheAddOperatorButton();
        }

        [Then("the Assign Operator Dialog will be displayed")]
        public async Task ThenTheAssignOperatorDialogWillBeDisplayed() {
            await this.UiHelpers.VerifyAssignOperatorDialogIsDisplayed();
        }

        [When("I enter the following details for the Operator")]
        public async Task WhenIEnterTheFollowingDetailsForTheOperator(DataTable dataTable)
        {
            List<(String, String, String)> operatorsList = new List<(String, String, String)>();
            foreach (DataTableRow tableRow in dataTable.Rows)
            {
                operatorsList.Add((ReqnrollTableHelper.GetStringRowValue(tableRow, "OperatorName"),
                    ReqnrollTableHelper.GetStringRowValue(tableRow, "MerchantNumber"),
                    ReqnrollTableHelper.GetStringRowValue(tableRow, "TerminalNumber")));
            }

            foreach ((String, String, String) @operator in operatorsList) {
                await this.UiHelpers.EnterOperatorDetails(@operator.Item1, @operator.Item2, @operator.Item3);
            }
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

        [When("I click on the Operators tab")]
        public async Task WhenIClickOnTheOperatorsTab() {
            await this.UiHelpers.ClickOnTheMerchantOperatorsTab();
        }

        [Then("I am presented with the Merchants Operator List Screen")]
        public async  Task ThenIAmPresentedWithTheMerchantsOperatorListScreen() {
            await this.UiHelpers.VerifyOnMerchantOperatorsTab();
        }

        [Then("the View Merchant Screen is displayed")]
        public async Task ThenTheViewMerchantScreenIsDisplayed()
        {
            await this.UiHelpers.VerifyOnTheViewMerchantScreen();
        }

        [When("click the Assign Operator button")]
        public async Task WhenClickTheAssignOperatorButton() {
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

        [When("I click on the Remove Operator for {string}")]
        public async Task WhenIClickOnTheRemoveOperatorFor(string operatorName) {
            await this.UiHelpers.ClickTheRemoveOperatorButton(operatorName);
        }


        [When("I click on the View Merchant Button for {string}")]
        public async Task  WhenIClickOnTheViewMerchantButtonFor(string merchantName)
        {
            await this.UiHelpers.ClickTheViewMerchantButton(merchantName);
        }

        [When("I enter the following details for the updated Merchant")]
        public async Task WhenIEnterTheFollowingDetailsForTheUpdatedMerchant(DataTable dataTable) {
            List<MerchantUpdate> updates = new List<MerchantUpdate>();
            foreach (DataTableRow? row in dataTable.Rows) {
                updates.Add(new MerchantUpdate(ReqnrollTableHelper.GetStringRowValue(row, "Tab"), ReqnrollTableHelper.GetStringRowValue(row, "Field"), ReqnrollTableHelper.GetStringRowValue(row, "Value")));
            }

            await this.UiHelpers.EnterMerchantUpdateDetails(updates);
        }

        public record MerchantUpdate(String tab, String field, String value);

        [When("I enter the following details for the new Merchant")]
        public async Task WhenIEnterTheFollowingDetailsForTheNewMerchant(DataTable dataTable)
        {
            DataTableRow merchantDetails = dataTable.Rows.Single();
            String merchantName = ReqnrollTableHelper.GetStringRowValue(merchantDetails, "MerchantName");
            String addressLine1 = ReqnrollTableHelper.GetStringRowValue(merchantDetails, "AddressLine1");
            String town = ReqnrollTableHelper.GetStringRowValue(merchantDetails, "Town");
            String region = ReqnrollTableHelper.GetStringRowValue(merchantDetails, "Region");
            String country = ReqnrollTableHelper.GetStringRowValue(merchantDetails, "Country");
            String contactName = ReqnrollTableHelper.GetStringRowValue(merchantDetails, "ContactName");
            String contactEmail = ReqnrollTableHelper.GetStringRowValue(merchantDetails, "EmailAddress");
            String settlementSchedule = ReqnrollTableHelper.GetStringRowValue(merchantDetails, "SettlementSchedule");

            await this.UiHelpers.EnterMerchantDetails(merchantName, addressLine1, town, region, null, country, contactName,
                contactEmail, null, settlementSchedule);
        }

        [Then("the Add New Operator Screen is displayed")]
        public async Task ThenTheAddNewOperatorScreenIsDisplayed() {
            await this.UiHelpers.VerifyOnTheNewOperatorScreen();
        }

        [Then("the Edit Operator Screen is displayed")]
        public async Task ThenTheEditOperatorScreenIsDisplayed()
        {
            await this.UiHelpers.VerifyOnTheEditOperatorScreen();
        }
        
        [When("I enter the following details for the new Operator")]
        [When("I enter the following new details for the Operator")]
        public async Task WhenIEnterTheFollowingDetailsForTheNewOperator(DataTable dataTable)
        {
            DataTableRow operatorDetails = dataTable.Rows.Single();
            String operatorName= ReqnrollTableHelper.GetStringRowValue(operatorDetails, "OperatorName");
            String requireCustomMerchantNumberString = ReqnrollTableHelper.GetStringRowValue(operatorDetails, "RequireCustomMerchantNumber");
            String requireCustomTerminalNumberString = ReqnrollTableHelper.GetStringRowValue(operatorDetails, "RequireCustomTerminalNumber");

            // Translate the boolean flags
            Boolean requireCustomMerchantNumber = requireCustomMerchantNumberString switch {
                "Yes" => true,
                _ => false
            };

            Boolean requireCustomTerminalNumber = requireCustomTerminalNumberString switch
            {
                "Yes" => true,
                _ => false
            };


            await this.UiHelpers.EnterOperatorDetails(operatorName, requireCustomMerchantNumber,
                requireCustomTerminalNumber);
        }

        [When("click the Save Operator button")]
        public async Task WhenClickTheSaveOperatorButton() {
            await this.UiHelpers.ClickTheSaveOperatorButton();
        }

        [When("click the Save Merchant button")]
        public async Task WhenClickTheSaveMerchantButton()
        {
            await this.UiHelpers.ClickTheSaveMerchantButton();
        }

        [When("I click on the View Products Button for {string}")]
        public async Task WhenIClickOnTheViewProductsButtonFor(string contractName) {
            await this.UiHelpers.ClickTheViewContractProductsButton(contractName);
        }

        [When("I click on the View Fees Button for {string}")]
        public async Task WhenIClickOnTheViewFeesButtonFor(string productName)
        {
            await this.UiHelpers.ClickTheViewContractProductFeesButton(productName);
        }


        [Then("the following contract product details are in the list")]
        public async Task ThenTheFollowingContractProductDetailsAreInTheList(DataTable dataTable)
        {
            List<(String, String, String,String)> contractProductsDescriptions = new List<(String, String, String, String)>();
            foreach (DataTableRow tableRow in dataTable.Rows)
            {
                contractProductsDescriptions.Add((ReqnrollTableHelper.GetStringRowValue(tableRow, "ProductName"),
                    ReqnrollTableHelper.GetStringRowValue(tableRow, "ProductType"),
                    ReqnrollTableHelper.GetStringRowValue(tableRow, "DisplayText"),
                    ReqnrollTableHelper.GetStringRowValue(tableRow, "Value")));
            }

            await this.UiHelpers.VerifyTheContractProductDetailsAreInTheList(contractProductsDescriptions);
        }


    }
    public record MerchantDetails(String MerchantName, String SettlementSchedule,String ContactName, String AddressLine1, String Town);
}
