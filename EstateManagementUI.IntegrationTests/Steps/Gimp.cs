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

        //[Then(@"I am presented the make merchant deposit screen")]
        //public async Task ThenIAmPresentedTheMakeMerchantDepositScreen()
        //{
        //    await this.UiHelpers.VerifyOnTheMakeMerchantDepositScreen();
        //}

        //[Then(@"I am presented the merchant details screen for '(.*)'")]
        //public async Task ThenIAmPresentedTheMerchantDetailsScreenFor(String merchantName)
        //{
        //    await this.UiHelpers.VerifyOnTheTheMerchantDetailsScreen(merchantName);
        //}

        //[Then(@"I am presented the new contract screen")]
        //public async Task ThenIAmPresentedTheNewContractScreen()
        //{
        //    await this.UiHelpers.VerifyOnTheNewContractScreen();
        //}

        //[Then(@"I am presented the new merchant screen")]
        //public async Task ThenIAmPresentedTheNewMerchantScreen()
        //{
        //    await this.UiHelpers.VerifyOnTheNewMerchantScreen();
        //}

        //[Then(@"I am presented the new operator screen")]
        //public async Task ThenIAmPresentedTheNewOperatorScreen()
        //{
        //    await this.UiHelpers.VerifyOnTheNewOperatorScreen();
        //}

        //[Then(@"I am presented the new product screen")]
        //public async Task ThenIAmPresentedTheNewProductScreen()
        //{
        //    await this.UiHelpers.VerifyOnTheNewProductScreen();
        //}

        //[Then(@"I am presented the new transaction fee screen")]
        //public async Task ThenIAmPresentedTheNewTransactionFeeScreen()
        //{
        //    await this.UiHelpers.VerifyOnTheNewTransactionFeeScreen();
        //}

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

        //[Then(@"I am presented with the Products List Screen")]
        //public async Task ThenIAmPresentedWithTheProductsListScreen()
        //{
        //    await this.UiHelpers.VerifyOnTheProductsListScreen();
        //}

        //[Then(@"I am presented with the Transaction Fee List Screen")]
        //public async Task ThenIAmPresentedWithTheTransactionFeeListScreen()
        //{
        //    await this.UiHelpers.VerifyOnTheTransactionFeeListScreen();
        //}

        [Then(@"My Estate Details will be shown")]
        public async Task ThenMyEstateDetailsWillBeShown(DataTable table)
        {
            DataTableRow tableRow = table.Rows.Single();
            String estateName = ReqnrollTableHelper.GetStringRowValue(tableRow, "EstateName").Replace("[id]", this.TestingContext.DockerHelper.TestId.ToString("N"));
            //String estateReference = ReqnrollTableHelper.GetStringRowValue(tableRow, "EstateReference");
            await this.UiHelpers.VerifyTheCorrectEstateDetailsAreDisplayed(estateName);
        }

        //[Then(@"the available balance for the merchant should be (.*)")]
        //public async Task ThenTheAvailableBalanceForTheMerchantShouldBe(Decimal availableBalance)
        //{
        //    await this.UiHelpers.VerifyTheAvailableBalanceIsDisplayed(availableBalance);
        //}

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
        
        //[Then(@"the following fee details are in the list")]
        //public async Task ThenTheFollowingFeeDetailsAreInTheList(DataTable table)
        //{
        //    List<String> feeDescriptions = new List<String>();
        //    foreach (DataTableRow tableRow in table.Rows)
        //    {
        //        feeDescriptions.Add(ReqnrollTableHelper.GetStringRowValue(tableRow, "Description"));
        //    }

        //    await this.UiHelpers.VerifyTheFeeDetailsAreInTheList(feeDescriptions);
        //}

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
            List<(String, String, String)> operatorsList = new List<(String, String, String)>();
            foreach (DataTableRow tableRow in table.Rows)
            {
                operatorsList.Add((ReqnrollTableHelper.GetStringRowValue(tableRow, "OperatorName"),
                    ReqnrollTableHelper.GetStringRowValue(tableRow, "RequireCustomMerchantNumber"),
                    ReqnrollTableHelper.GetStringRowValue(tableRow, "RequireCustomTerminalNumber")));   
            }

            await this.UiHelpers.VerifyOperatorDetailsAreInTheList(operatorsList);
        }

        //[Then(@"the following product details are in the list")]
        //public async Task ThenTheFollowingProductDetailsAreInTheList(DataTable table)
        //{
        //    List<String> productsList = new List<String>();
        //    foreach (DataTableRow tableRow in table.Rows)
        //    {
        //        productsList.Add(ReqnrollTableHelper.GetStringRowValue(tableRow, "ProductName"));
        //    }

        //    await this.UiHelpers.VerifyProductDetailsAreInTheList(productsList);
        //}

        //[Then(@"the merchants settlement schedule is '([^']*)'")]
        //public async Task ThenTheMerchantsSettlementScheduleIs(String settlementSchedule)
        //{
        //    await this.UiHelpers.VerifyMerchantsSettlementSchedule(settlementSchedule);
        //}

        //[When(@"I click the Add New Contract button")]
        //public async Task WhenIClickTheAddNewContractButton()
        //{
        //    await this.UiHelpers.ClickAddNewContractButton();
        //}

        //[When(@"I click the Add New Merchant button")]
        //public async Task WhenIClickTheAddNewMerchantButton()
        //{
        //    await this.UiHelpers.ClickAddNewMerchantButton();
        //}

        //[When(@"I click the Add New Operator button")]
        //public async Task WhenIClickTheAddNewOperatorButton()
        //{
        //    await this.UiHelpers.ClickAddNewOperatorButton();
        //}

        //[When(@"I click the Add New Product button")]
        //public async Task WhenIClickTheAddNewProductButton()
        //{
        //    await this.UiHelpers.ClickAddNewProductButton();
        //}

        //[When(@"I click the Add New Transaction Fee button")]
        //public async Task WhenIClickTheAddNewTransactionFeeButton()
        //{
        //    await this.UiHelpers.ClickAddNewTransactionFeeButton();
        //}

        //[When(@"I click the Create Contract button")]
        //public async Task WhenIClickTheCreateContractButton()
        //{
        //    await this.UiHelpers.ClickTheCreateContractButton();
        //}

        //[When(@"I click the Create Merchant button")]
        //public async Task WhenIClickTheCreateMerchantButton()
        //{
        //    await this.UiHelpers.ClickTheCreateMerchantButton();
        //}

        //[When(@"I click the Create Operator button")]
        //public async Task WhenIClickTheCreateOperatorButton()
        //{
        //    await this.UiHelpers.ClickTheCreateOperatorButton();
        //}

        //[When(@"I click the Create Product button")]
        //public async Task WhenIClickTheCreateProductButton()
        //{
        //    await this.UiHelpers.ClickTheCreateProductButton();
        //}

        //[When(@"I click the Create Transaction Fee button")]
        //public async Task WhenIClickTheCreateTransactionFeeButton()
        //{
        //    await this.UiHelpers.ClickTheCreateTransactionFeeButton();
        //}

        //[When(@"I click the Make Deposit button for '(.*)' from the merchant list")]
        //public async Task WhenIClickTheMakeDepositButtonForFromTheMerchantList(String merchantName)
        //{
        //    await this.UiHelpers.ClickTheMakeDepositButtonForTheMerchant(merchantName);
        //}

        //[When(@"I click the Products Link for '(.*)'")]
        //public async Task WhenIClickTheProductsLinkFor(String contractDescription)
        //{
        //    await this.UiHelpers.ClickTheProductsLinkForContract(contractDescription);
        //}

        //[When(@"I click the Transaction Fees Link for '(.*)'")]
        //public async Task WhenIClickTheTransactionFeesLinkFor(String productName)
        //{
        //    await this.UiHelpers.ClickTheTransactionFeesLinkForTheProduct(productName);
        //}

        //[When(@"I enter the following new contract details")]
        //public async Task WhenIEnterTheFollowingNewContractDetails(DataTable table)
        //{
        //    DataTableRow tableRow = table.Rows.Single();

        //    String contractDescription = ReqnrollTableHelper.GetStringRowValue(tableRow, "ContractDescription");
        //    String operatorName = ReqnrollTableHelper.GetStringRowValue(tableRow, "OperatorName").Replace("[id]", this.TestingContext.DockerHelper.TestId.ToString("N"));

        //    await this.UiHelpers.EnterContractDetails(contractDescription,
        //                                                                operatorName);
        //}

        //[When(@"I enter the following new merchant details")]
        //public async Task WhenIEnterTheFollowingNewMerchantDetails(DataTable table)
        //{
        //    DataTableRow tableRow = table.Rows.Single();

        //    String merchantName = ReqnrollTableHelper.GetStringRowValue(tableRow, "MerchantName");
        //    String addressLine1 = ReqnrollTableHelper.GetStringRowValue(tableRow, "AddressLine1");
        //    String town = ReqnrollTableHelper.GetStringRowValue(tableRow, "Town");
        //    String region = ReqnrollTableHelper.GetStringRowValue(tableRow, "Region");
        //    String postCode = ReqnrollTableHelper.GetStringRowValue(tableRow, "PostCode");
        //    String country = ReqnrollTableHelper.GetStringRowValue(tableRow, "Country");
        //    String contactName = ReqnrollTableHelper.GetStringRowValue(tableRow, "ContactName");
        //    String contactEmail = ReqnrollTableHelper.GetStringRowValue(tableRow, "ContactEmail");
        //    String contactPhoneNumber = ReqnrollTableHelper.GetStringRowValue(tableRow, "ContactPhoneNumber");
        //    String settlementSchedule = ReqnrollTableHelper.GetStringRowValue(tableRow, "SettlementSchedule");

        //    await this.UiHelpers.EnterMerchantDetails(merchantName,
        //                                                                addressLine1,
        //                                                                town,
        //                                                                region,
        //                                                                postCode,
        //                                                                country,
        //                                                                contactName,
        //                                                                contactEmail,
        //                                                                contactPhoneNumber,
        //                                                                settlementSchedule);
        //}

        //[When(@"I enter the following new operator details")]
        //public async Task WhenIEnterTheFollowingNewOperatorDetails(DataTable table)
        //{
        //    DataTableRow tableRow = table.Rows.Single();

        //    String operatorName = ReqnrollTableHelper.GetStringRowValue(tableRow, "OperatorName");
        //    await this.UiHelpers.EnterOperatorDetails(operatorName);
        //}

        //[When(@"I enter the following new product details")]
        //public async Task WhenIEnterTheFollowingNewProductDetails(DataTable table)
        //{
        //    DataTableRow productDetails = table.Rows.Single();
        //    String productName = ReqnrollTableHelper.GetStringRowValue(productDetails, "ProductName");
        //    String displayText = ReqnrollTableHelper.GetStringRowValue(productDetails, "DisplayText");
        //    String productValue = ReqnrollTableHelper.GetStringRowValue(productDetails, "Value");
        //    String productType = ReqnrollTableHelper.GetStringRowValue(productDetails, "ProductType");

        //    await this.UiHelpers.EnterProductDetails(productName, displayText, productValue, productType);
        //}

        //[When(@"I enter the following new transaction fee details")]
        //public async Task WhenIEnterTheFollowingNewTransactionFeeDetails(DataTable table)
        //{
        //    DataTableRow productDetails = table.Rows.Single();
        //    String description = ReqnrollTableHelper.GetStringRowValue(productDetails, "Description");
        //    String calculationType = ReqnrollTableHelper.GetStringRowValue(productDetails, "CalculationType");
        //    String feeType = ReqnrollTableHelper.GetStringRowValue(productDetails, "FeeType");
        //    String feeValue = ReqnrollTableHelper.GetStringRowValue(productDetails, "Value");

        //    await this.UiHelpers.EnterTransactionFeeDetails(description, calculationType, feeType, feeValue);
        //}

        [When(@"I login with the username '(.*)' and password '(.*)'")]
        public async Task WhenILoginWithTheUsernameAndPassword(String userName, String password)
        {

            String username = userName.Replace("[id]", this.TestingContext.DockerHelper.TestId.ToString("N"));
            await this.UiHelpers.Login(username, password);
        }

        //[When(@"I make the following deposit")]
        //public async Task WhenIMakeTheFollowingDeposit(DataTable table)
        //{
        //    DataTableRow depositDetails = table.Rows.Single();
        //    Decimal depositAmount = ReqnrollTableHelper.GetDecimalValue(depositDetails, "DepositAmount");
        //    String depositDateString = ReqnrollTableHelper.GetStringRowValue(depositDetails, "DepositDate");
        //    DateTime depositDate = ReqnrollTableHelper.GetDateForDateString(depositDateString, DateTime.Now);
        //    String depositReference = ReqnrollTableHelper.GetStringRowValue(depositDetails, "DepositReference");

        //    await this.UiHelpers.MakeMerchantDeposit(depositDate, depositAmount, depositReference);
        //}

        //[When(@"I select '(.*)' from the merchant list")]
        //public async Task WhenISelectFromTheMerchantList(String merchantName)
        //{
        //    await this.UiHelpers.ClickTheMerchantLinkForMerchant(merchantName);
        //}

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

        [Then("the Add New Merchant Dialog is displayed")]
        public async Task ThenTheAddNewMerchantDialogIsDisplayed()
        {
            await this.UiHelpers.VerifyOnTheMerchantDialog();
        }

        [When("I click on the Edit Operator Button for {string}")]
        public async Task WhenIClickOnTheEditOperatorButtonFor(string operatorName)
        {
            await this.UiHelpers.ClickTheEditOperatorButton(operatorName);
        }

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

        [Then("the Add New Operator Dialog is displayed")]
        public async Task ThenTheAddNewOperatorDialogIsDisplayed() {
            await this.UiHelpers.VerifyOnTheOperatorDialog();
        }

        [Then("the Edit Operator Dialog is displayed")]
        public async Task ThenTheEditOperatorDialogIsDisplayed()
        {
            await this.UiHelpers.VerifyOnTheOperatorDialog();
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
            await this.UiHelpers.ClickTheSaveNewOperatorButton();
        }

        [When("click the Save Merchant button")]
        public async Task WhenClickTheSaveMerchantButton()
        {
            await this.UiHelpers.ClickTheSaveNewMerchantButton();
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
