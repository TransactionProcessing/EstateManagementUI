using Reqnroll;
using EstateManagementUI.IntegrationTests.Common;
using Microsoft.Playwright;
using Reqnroll.BoDi;

namespace EstateManagementUI.IntegrationTests.Steps
{
    [Binding]
    [Scope(Tag = "playwright")]
    public class PlaywrightDashboardSteps
    {
        private readonly TestingContext TestingContext;
        private readonly PlaywrightEstateManagementUiHelpers UiHelpers;

        public PlaywrightDashboardSteps(ScenarioContext scenarioContext, TestingContext testingContext, IObjectContainer container)
        {
            var page = scenarioContext.ScenarioContainer.Resolve<IPage>(scenarioContext.ScenarioInfo.Title.Replace(" ", ""));

            this.TestingContext = testingContext;
            this.UiHelpers = new PlaywrightEstateManagementUiHelpers(page, this.TestingContext.DockerHelper.EstateManagementUiPort);
        }

        [Given(@"I am on the application home page")]
        public async Task GivenIAmOnTheApplicationHomePage()
        {
            await this.UiHelpers.NavigateToHomePage();
        }

        [Given(@"I click on the My Contracts sidebar option")]
        [When(@"I click on the My Contracts sidebar option")]
        public async Task WhenIClickOnTheMyContractsSidebarOption()
        {
            await this.UiHelpers.ClickContractsSidebarOption();
        }

        [Given(@"I click on the My Estate sidebar option")]
        [When(@"I click on the My Estate sidebar option")]
        public async Task WhenIClickOnTheMyEstateSidebarOption()
        {
            await this.UiHelpers.ClickMyEstateSidebarOption();
        }

        [Given(@"I click on the My Merchants sidebar option")]
        [When(@"I click on the My Merchants sidebar option")]
        public async Task WhenIClickOnTheMyMerchantsSidebarOption()
        {
            await this.UiHelpers.ClickMyMerchantsSidebarOption();
        }

        [Given(@"I click on the My Operators sidebar option")]
        [When(@"I click on the My Operators sidebar option")]
        public async Task WhenIClickOnTheMyOperatorsSidebarOption()
        {
            await this.UiHelpers.ClickMyOperatorsSidebarOption();
        }

        [Given(@"I click on the Sign In Button")]
        [When(@"I click on the Sign In Button")]
        public async Task WhenIClickOnTheSignInButton()
        {
            await this.UiHelpers.ClickOnTheSignInButton();
        }

        [Then(@"I am presented with a login screen")]
        public async Task ThenIAmPresentedWithALoginScreen()
        {
            await this.UiHelpers.VerifyOnTheLoginScreen();
        }

        [When(@"I login with the username '(.*)' and password '(.*)'")]
        public async Task WhenILoginWithTheUsernameAndPassword(String username, String password)
        {
            await this.UiHelpers.LoginWithUsernameAndPassword(username, password);
        }

        [Then(@"I am presented with the Estate Administrator Dashboard")]
        public async Task ThenIAmPresentedWithTheEstateAdministratorDashboard()
        {
            await this.UiHelpers.VerifyOnTheDashboard();
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

        [Then(@"I am presented with the Contracts List Screen")]
        public async Task ThenIAmPresentedWithTheContractsListScreen()
        {
            await this.UiHelpers.VerifyOnTheContractsListScreen();
        }

        [Then(@"I am presented with the View Estate Page")]
        public async Task ThenIAmPresentedWithTheViewEstatePage()
        {
            await this.UiHelpers.VerifyOnTheViewEstatePage();
        }

        [Then(@"the dashboard displays the navigation menu")]
        public async Task ThenTheDashboardDisplaysTheNavigationMenu()
        {
            await this.UiHelpers.VerifyDashboardNavigationMenuIsDisplayed();
        }

        [Then(@"the dashboard shows estate information")]
        public async Task ThenTheDashboardShowsEstateInformation()
        {
            await this.UiHelpers.VerifyDashboardEstateInformationIsDisplayed();
        }

        [When(@"I navigate back to the dashboard")]
        public async Task WhenINavigateBackToTheDashboard()
        {
            await this.UiHelpers.NavigateToDashboard();
        }

        // Merchant-specific steps
        [When(@"I click on the New Merchant Button")]
        public async Task WhenIClickOnTheNewMerchantButton()
        {
            await this.UiHelpers.ClickNewMerchantButton();
        }

        [Then(@"the Add New Merchant Screen is displayed")]
        public async Task ThenTheAddNewMerchantScreenIsDisplayed()
        {
            await this.UiHelpers.VerifyAddNewMerchantScreenIsDisplayed();
        }

        [Then(@"the merchant list contains at least (.*) merchants")]
        public async Task ThenTheMerchantListContainsAtLeast(int count)
        {
            await this.UiHelpers.VerifyMerchantListContainsAtLeast(count);
        }

        [Then(@"the merchant list displays merchant names")]
        public async Task ThenTheMerchantListDisplaysMerchantNames()
        {
            await this.UiHelpers.VerifyMerchantListDisplaysMerchantNames();
        }

        [Then(@"the merchant list displays settlement schedules")]
        public async Task ThenTheMerchantListDisplaysSettlementSchedules()
        {
            await this.UiHelpers.VerifyMerchantListDisplaysSettlementSchedules();
        }

        [Then(@"the merchant list displays contact information")]
        public async Task ThenTheMerchantListDisplaysContactInformation()
        {
            await this.UiHelpers.VerifyMerchantListDisplaysContactInformation();
        }

        [When(@"I click on the View Merchant Button for '(.*)'")]
        public async Task WhenIClickOnTheViewMerchantButtonFor(string merchantName)
        {
            await this.UiHelpers.ClickViewMerchantButton(merchantName);
        }

        [Then(@"the View Merchant Screen is displayed")]
        public async Task ThenTheViewMerchantScreenIsDisplayed()
        {
            await this.UiHelpers.VerifyViewMerchantScreenIsDisplayed();
        }

        [Then(@"the merchant details section is visible")]
        public async Task ThenTheMerchantDetailsSectionIsVisible()
        {
            await this.UiHelpers.VerifyMerchantDetailsSectionIsVisible();
        }

        [Then(@"the merchant operators section is visible")]
        public async Task ThenTheMerchantOperatorsSectionIsVisible()
        {
            await this.UiHelpers.VerifyMerchantOperatorsSectionIsVisible();
        }

        [Then(@"the merchant contracts section is visible")]
        public async Task ThenTheMerchantContractsSectionIsVisible()
        {
            await this.UiHelpers.VerifyMerchantContractsSectionIsVisible();
        }

        [Then(@"the merchant devices section is visible")]
        public async Task ThenTheMerchantDevicesSectionIsVisible()
        {
            await this.UiHelpers.VerifyMerchantDevicesSectionIsVisible();
        }

        // Operator-specific steps
        [When(@"I click on the New Operator Button")]
        public async Task WhenIClickOnTheNewOperatorButton()
        {
            await this.UiHelpers.ClickNewOperatorButton();
        }

        [Then(@"the Add New Operator Screen is displayed")]
        public async Task ThenTheAddNewOperatorScreenIsDisplayed()
        {
            await this.UiHelpers.VerifyAddNewOperatorScreenIsDisplayed();
        }

        [Then(@"the operator list contains at least (.*) operators")]
        public async Task ThenTheOperatorListContainsAtLeast(int count)
        {
            await this.UiHelpers.VerifyOperatorListContainsAtLeast(count);
        }

        [Then(@"the operator list displays operator names")]
        public async Task ThenTheOperatorListDisplaysOperatorNames()
        {
            await this.UiHelpers.VerifyOperatorListDisplaysOperatorNames();
        }

        [Then(@"the operator list displays custom merchant number requirements")]
        public async Task ThenTheOperatorListDisplaysCustomMerchantNumberRequirements()
        {
            await this.UiHelpers.VerifyOperatorListDisplaysCustomMerchantNumberRequirements();
        }

        [Then(@"the operator list displays custom terminal number requirements")]
        public async Task ThenTheOperatorListDisplaysCustomTerminalNumberRequirements()
        {
            await this.UiHelpers.VerifyOperatorListDisplaysCustomTerminalNumberRequirements();
        }

        [Then(@"I can see operator configuration details in the list")]
        public async Task ThenICanSeeOperatorConfigurationDetailsInTheList()
        {
            await this.UiHelpers.VerifyOperatorConfigurationDetailsInList();
        }

        // Contract-specific steps
        [When(@"I click on the New Contract Button")]
        public async Task WhenIClickOnTheNewContractButton()
        {
            await this.UiHelpers.ClickNewContractButton();
        }

        [Then(@"the Add New Contract Screen is displayed")]
        public async Task ThenTheAddNewContractScreenIsDisplayed()
        {
            await this.UiHelpers.VerifyAddNewContractScreenIsDisplayed();
        }

        [Then(@"the contract list contains at least (.*) contracts")]
        public async Task ThenTheContractListContainsAtLeast(int count)
        {
            await this.UiHelpers.VerifyContractListContainsAtLeast(count);
        }

        [Then(@"the contract list displays contract descriptions")]
        public async Task ThenTheContractListDisplaysContractDescriptions()
        {
            await this.UiHelpers.VerifyContractListDisplaysDescriptions();
        }

        [Then(@"the contract list displays operator information")]
        public async Task ThenTheContractListDisplaysOperatorInformation()
        {
            await this.UiHelpers.VerifyContractListDisplaysOperatorInformation();
        }

        [When(@"I click on the View Products Button for contract '(.*)'")]
        public async Task WhenIClickOnTheViewProductsButtonForContract(string contractName)
        {
            await this.UiHelpers.ClickViewProductsButtonForContract(contractName);
        }

        [Then(@"the Contract Products Screen is displayed")]
        public async Task ThenTheContractProductsScreenIsDisplayed()
        {
            await this.UiHelpers.VerifyContractProductsScreenIsDisplayed();
        }

        [Then(@"the product list contains at least (.*) products")]
        public async Task ThenTheProductListContainsAtLeast(int count)
        {
            await this.UiHelpers.VerifyProductListContainsAtLeast(count);
        }

        [Then(@"the product list displays product names")]
        public async Task ThenTheProductListDisplaysProductNames()
        {
            await this.UiHelpers.VerifyProductListDisplaysProductNames();
        }

        [Then(@"the product list displays product values")]
        public async Task ThenTheProductListDisplaysProductValues()
        {
            await this.UiHelpers.VerifyProductListDisplaysProductValues();
        }

        // Estate-specific steps
        [Then(@"the estate name is displayed")]
        public async Task ThenTheEstateNameIsDisplayed()
        {
            await this.UiHelpers.VerifyEstateNameIsDisplayed();
        }

        [Then(@"the estate reference is displayed")]
        public async Task ThenTheEstateReferenceIsDisplayed()
        {
            await this.UiHelpers.VerifyEstateReferenceIsDisplayed();
        }

        [Then(@"the estate operators list contains at least (.*) operators")]
        public async Task ThenTheEstateOperatorsListContainsAtLeast(int count)
        {
            await this.UiHelpers.VerifyEstateOperatorsListContainsAtLeast(count);
        }

        [Then(@"the estate operators list displays operator names")]
        public async Task ThenTheEstateOperatorsListDisplaysOperatorNames()
        {
            await this.UiHelpers.VerifyEstateOperatorsListDisplaysOperatorNames();
        }
    }
}
