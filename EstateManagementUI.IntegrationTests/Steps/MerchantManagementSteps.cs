using Microsoft.Playwright;
using Reqnroll;
using EstateManagementUI.IntegrationTests.Common;

namespace EstateManagementUI.IntegrationTests.Steps;

/// <summary>
/// Step definitions for Merchant Management integration tests
/// Links feature file scenarios to browser automation code
/// </summary>
[Binding]
public class MerchantManagementSteps
{
    private readonly IPage _page;
    private readonly MerchantManagementPageHelper _merchantManagementHelper;
    private readonly ScenarioContext _scenarioContext;

    public MerchantManagementSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
        _page = scenarioContext.ScenarioContainer.Resolve<IPage>();
        
        // Get base URL from environment variable or use default
        var baseUrl = Environment.GetEnvironmentVariable("APP_URL") ?? "https://localhost:5001";
        _merchantManagementHelper = new MerchantManagementPageHelper(_page, baseUrl);
    }

    #region Navigation Steps

    [Given(@"the user navigates to Merchant Management")]
    [When(@"the user navigates to Merchant Management")]
    public async Task GivenTheUserNavigatesToMerchantManagement()
    {
        await _merchantManagementHelper.NavigateToMerchantManagement();
    }

    #endregion

    #region Authentication/Role Steps

    [Given(@"the user is authenticated as an ""(.*)"" user")]
    public async Task GivenTheUserIsAuthenticatedAsAUser(string role)
    {
        // Store the role in scenario context for reference
        _scenarioContext["UserRole"] = role;
        
        // Note: This step assumes the application will be started in test mode
        // with the appropriate role already configured. The actual authentication
        // setup will be handled when the application startup is implemented.
        await Task.CompletedTask;
    }

    [Given(@"the user is authenticated as a ""(.*)"" user")]
    public async Task GivenTheUserIsAuthenticatedAsAViewerUser(string role)
    {
        // Store the role in scenario context for reference
        _scenarioContext["UserRole"] = role;
        
        // Note: This step assumes the application will be started in test mode
        // with the appropriate role already configured. The actual authentication
        // setup will be handled when the application startup is implemented.
        await Task.CompletedTask;
    }

    #endregion

    #region Menu Visibility Steps

    [Then(@"the Merchant Management menu is not visible")]
    public async Task ThenTheMerchantManagementMenuIsNotVisible()
    {
        // First navigate to home to check menu
        await _merchantManagementHelper.NavigateToHome();
        await _merchantManagementHelper.VerifyMerchantManagementMenuIsNotVisible();
    }

    [Then(@"the Merchant Management menu is visible")]
    public async Task ThenTheMerchantManagementMenuIsVisible()
    {
        await _merchantManagementHelper.VerifyMerchantManagementMenuIsVisible();
    }

    #endregion

    #region Page Verification Steps

    [Then(@"the Merchant Management page is displayed")]
    public async Task ThenTheMerchantManagementPageIsDisplayed()
    {
        await _merchantManagementHelper.VerifyMerchantManagementPageIsDisplayed();
    }

    [Then(@"the page title is ""(.*)""")]
    public async Task ThenThePageTitleIs(string expectedTitle)
    {
        await _merchantManagementHelper.VerifyPageTitle(expectedTitle);
    }

    [Then(@"the View Merchant page is displayed")]
    public async Task ThenTheViewMerchantPageIsDisplayed()
    {
        await _merchantManagementHelper.VerifyViewMerchantPageIsDisplayed();
    }

    [Then(@"the Create New Merchant page is displayed")]
    public async Task ThenTheCreateNewMerchantPageIsDisplayed()
    {
        await _merchantManagementHelper.VerifyCreateNewMerchantPageIsDisplayed();
    }

    [Then(@"the Edit Merchant page is displayed")]
    public async Task ThenTheEditMerchantPageIsDisplayed()
    {
        await _merchantManagementHelper.VerifyEditMerchantPageIsDisplayed();
    }

    [Then(@"the Make Deposit page is displayed")]
    public async Task ThenTheMakeDepositPageIsDisplayed()
    {
        await _merchantManagementHelper.VerifyMakeDepositPageIsDisplayed();
    }

    #endregion

    #region Merchant List Verification Steps

    [Then(@"the merchant list contains ""(.*)"" merchants")]
    public async Task ThenTheMerchantListContainsMerchants(int expectedCount)
    {
        await _merchantManagementHelper.VerifyMerchantListCount(expectedCount);
    }

    [Then(@"the merchant ""(.*)"" with reference ""(.*)"" is listed")]
    public async Task ThenTheMerchantWithReferenceIsListed(string merchantName, string merchantReference)
    {
        await _merchantManagementHelper.VerifyMerchantIsListed(merchantName, merchantReference);
    }

    [Then(@"the merchant ""(.*)"" shows balance ""(.*)""")]
    public async Task ThenTheMerchantShowsBalance(string merchantName, string expectedBalance)
    {
        await _merchantManagementHelper.VerifyMerchantBalance(merchantName, expectedBalance);
    }

    [Then(@"the merchant ""(.*)"" shows available balance ""(.*)""")]
    public async Task ThenTheMerchantShowsAvailableBalance(string merchantName, string expectedAvailableBalance)
    {
        await _merchantManagementHelper.VerifyMerchantAvailableBalance(merchantName, expectedAvailableBalance);
    }

    [Then(@"the merchant ""(.*)"" shows settlement schedule ""(.*)""")]
    public async Task ThenTheMerchantShowsSettlementSchedule(string merchantName, string expectedSchedule)
    {
        await _merchantManagementHelper.VerifyMerchantSettlementSchedule(merchantName, expectedSchedule);
    }

    #endregion

    #region Button Visibility Steps

    [Then(@"the Add New Merchant button is visible")]
    public async Task ThenTheAddNewMerchantButtonIsVisible()
    {
        await _merchantManagementHelper.VerifyAddNewMerchantButtonIsVisible();
    }

    [Then(@"the Add New Merchant button is not visible")]
    public async Task ThenTheAddNewMerchantButtonIsNotVisible()
    {
        await _merchantManagementHelper.VerifyAddNewMerchantButtonIsNotVisible();
    }

    [Then(@"the Edit button is visible for merchant ""(.*)""")]
    public async Task ThenTheEditButtonIsVisibleForMerchant(string merchantName)
    {
        await _merchantManagementHelper.VerifyEditButtonIsVisibleForMerchant(merchantName);
    }

    [Then(@"the Edit button is not visible for merchant ""(.*)""")]
    public async Task ThenTheEditButtonIsNotVisibleForMerchant(string merchantName)
    {
        await _merchantManagementHelper.VerifyEditButtonIsNotVisibleForMerchant(merchantName);
    }

    [Then(@"the Make Deposit button is visible for merchant ""(.*)""")]
    public async Task ThenTheMakeDepositButtonIsVisibleForMerchant(string merchantName)
    {
        await _merchantManagementHelper.VerifyMakeDepositButtonIsVisibleForMerchant(merchantName);
    }

    [Then(@"the Make Deposit button is not visible for merchant ""(.*)""")]
    public async Task ThenTheMakeDepositButtonIsNotVisibleForMerchant(string merchantName)
    {
        await _merchantManagementHelper.VerifyMakeDepositButtonIsNotVisibleForMerchant(merchantName);
    }

    #endregion

    #region Merchant Detail Verification Steps

    [Then(@"the merchant name is ""(.*)""")]
    public async Task ThenTheMerchantNameIs(string expectedName)
    {
        await _merchantManagementHelper.VerifyMerchantName(expectedName);
    }

    [Then(@"the merchant reference is ""(.*)""")]
    public async Task ThenTheMerchantReferenceIs(string expectedReference)
    {
        await _merchantManagementHelper.VerifyMerchantReference(expectedReference);
    }

    [Then(@"the merchant balance is ""(.*)""")]
    public async Task ThenTheMerchantBalanceIs(string expectedBalance)
    {
        await _merchantManagementHelper.VerifyMerchantBalanceDetail(expectedBalance);
    }

    [Then(@"the merchant available balance is ""(.*)""")]
    public async Task ThenTheMerchantAvailableBalanceIs(string expectedAvailableBalance)
    {
        await _merchantManagementHelper.VerifyMerchantAvailableBalanceDetail(expectedAvailableBalance);
    }

    [Then(@"the merchant settlement schedule is ""(.*)""")]
    public async Task ThenTheMerchantSettlementScheduleIs(string expectedSchedule)
    {
        await _merchantManagementHelper.VerifyMerchantSettlementScheduleDetail(expectedSchedule);
    }

    #endregion

    #region Tab Navigation Steps

    [When(@"the user clicks on the ""(.*)"" tab")]
    public async Task WhenTheUserClicksOnTheTab(string tabName)
    {
        await _merchantManagementHelper.ClickTab(tabName);
    }

    [Then(@"the address tab content is displayed")]
    public async Task ThenTheAddressTabContentIsDisplayed()
    {
        await _merchantManagementHelper.VerifyAddressTabContent();
    }

    [Then(@"the contact tab content is displayed")]
    public async Task ThenTheContactTabContentIsDisplayed()
    {
        await _merchantManagementHelper.VerifyContactTabContent();
    }

    [Then(@"the operators tab content is displayed")]
    public async Task ThenTheOperatorsTabContentIsDisplayed()
    {
        await _merchantManagementHelper.VerifyOperatorsTabContent();
    }

    [Then(@"the Assigned Operators section is visible")]
    public async Task ThenTheAssignedOperatorsSectionIsVisible()
    {
        await _merchantManagementHelper.VerifyAssignedOperatorsSection();
    }

    [Then(@"the contracts tab content is displayed")]
    public async Task ThenTheContractsTabContentIsDisplayed()
    {
        await _merchantManagementHelper.VerifyContractsTabContent();
    }

    [Then(@"the Assigned Contracts section is visible")]
    public async Task ThenTheAssignedContractsSectionIsVisible()
    {
        await _merchantManagementHelper.VerifyAssignedContractsSection();
    }

    [Then(@"the devices tab content is displayed")]
    public async Task ThenTheDevicesTabContentIsDisplayed()
    {
        await _merchantManagementHelper.VerifyDevicesTabContent();
    }

    [Then(@"the Assigned Devices section is visible")]
    public async Task ThenTheAssignedDevicesSectionIsVisible()
    {
        await _merchantManagementHelper.VerifyAssignedDevicesSection();
    }

    #endregion

    #region Address Details Steps

    [Then(@"the address line 1 is ""(.*)""")]
    public async Task ThenTheAddressLine1Is(string expectedAddress)
    {
        await _merchantManagementHelper.VerifyAddressLine1(expectedAddress);
    }

    [Then(@"the town is ""(.*)""")]
    public async Task ThenTheTownIs(string expectedTown)
    {
        await _merchantManagementHelper.VerifyTown(expectedTown);
    }

    [Then(@"the region is ""(.*)""")]
    public async Task ThenTheRegionIs(string expectedRegion)
    {
        await _merchantManagementHelper.VerifyRegion(expectedRegion);
    }

    [Then(@"the postal code is ""(.*)""")]
    public async Task ThenThePostalCodeIs(string expectedPostalCode)
    {
        await _merchantManagementHelper.VerifyPostalCode(expectedPostalCode);
    }

    [Then(@"the country is ""(.*)""")]
    public async Task ThenTheCountryIs(string expectedCountry)
    {
        await _merchantManagementHelper.VerifyCountry(expectedCountry);
    }

    #endregion

    #region Contact Details Steps

    [Then(@"the contact name is ""(.*)""")]
    public async Task ThenTheContactNameIs(string expectedContactName)
    {
        await _merchantManagementHelper.VerifyContactName(expectedContactName);
    }

    [Then(@"the contact email is ""(.*)""")]
    public async Task ThenTheContactEmailIs(string expectedContactEmail)
    {
        await _merchantManagementHelper.VerifyContactEmail(expectedContactEmail);
    }

    [Then(@"the contact phone is ""(.*)""")]
    public async Task ThenTheContactPhoneIs(string expectedContactPhone)
    {
        await _merchantManagementHelper.VerifyContactPhone(expectedContactPhone);
    }

    #endregion

    #region Form Field Verification Steps

    [Then(@"the merchant name field is visible")]
    public async Task ThenTheMerchantNameFieldIsVisible()
    {
        await _merchantManagementHelper.VerifyMerchantNameFieldIsVisible();
    }

    [Then(@"the merchant name field contains ""(.*)""")]
    public async Task ThenTheMerchantNameFieldContains(string expectedValue)
    {
        await _merchantManagementHelper.VerifyMerchantNameFieldContains(expectedValue);
    }

    [Then(@"the settlement schedule field is visible")]
    public async Task ThenTheSettlementScheduleFieldIsVisible()
    {
        await _merchantManagementHelper.VerifySettlementScheduleFieldIsVisible();
    }

    [Then(@"the address line 1 field is visible")]
    public async Task ThenTheAddressLine1FieldIsVisible()
    {
        await _merchantManagementHelper.VerifyAddressLine1FieldIsVisible();
    }

    [Then(@"the contact name field is visible")]
    public async Task ThenTheContactNameFieldIsVisible()
    {
        await _merchantManagementHelper.VerifyContactNameFieldIsVisible();
    }

    [Then(@"the deposit amount field is visible")]
    public async Task ThenTheDepositAmountFieldIsVisible()
    {
        await _merchantManagementHelper.VerifyDepositAmountFieldIsVisible();
    }

    [Then(@"the deposit date field is visible")]
    public async Task ThenTheDepositDateFieldIsVisible()
    {
        await _merchantManagementHelper.VerifyDepositDateFieldIsVisible();
    }

    [Then(@"the deposit reference field is visible")]
    public async Task ThenTheDepositReferenceFieldIsVisible()
    {
        await _merchantManagementHelper.VerifyDepositReferenceFieldIsVisible();
    }

    #endregion

    #region Navigation Action Steps

    [When(@"the user clicks on merchant ""(.*)""")]
    public async Task WhenTheUserClicksOnMerchant(string merchantName)
    {
        await _merchantManagementHelper.ClickOnMerchant(merchantName);
    }

    [When(@"the user clicks the Add New Merchant button")]
    public async Task WhenTheUserClicksTheAddNewMerchantButton()
    {
        await _merchantManagementHelper.ClickAddNewMerchantButton();
    }

    [When(@"the user clicks edit for merchant ""(.*)""")]
    public async Task WhenTheUserClicksEditForMerchant(string merchantName)
    {
        await _merchantManagementHelper.ClickEditForMerchant(merchantName);
    }

    [When(@"the user clicks make deposit for merchant ""(.*)""")]
    public async Task WhenTheUserClicksMakeDepositForMerchant(string merchantName)
    {
        await _merchantManagementHelper.ClickMakeDepositForMerchant(merchantName);
    }

    #endregion
}
