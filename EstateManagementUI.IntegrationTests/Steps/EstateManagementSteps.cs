using Microsoft.Playwright;
using Reqnroll;
using EstateManagementUI.IntegrationTests.Common;

namespace EstateManagementUI.IntegrationTests.Steps;

/// <summary>
/// Step definitions for Estate Management integration tests
/// Links feature file scenarios to browser automation code
/// </summary>
[Binding]
public class EstateManagementSteps
{
    private readonly IPage _page;
    private readonly EstateManagementPageHelper _estateManagementHelper;
    private readonly ScenarioContext _scenarioContext;

    public EstateManagementSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
        _page = scenarioContext.ScenarioContainer.Resolve<IPage>();
        
        // Get base URL from environment variable or use default
        var baseUrl = Environment.GetEnvironmentVariable("APP_URL") ?? "https://localhost:5001";
        _estateManagementHelper = new EstateManagementPageHelper(_page, baseUrl);
    }

    #region Navigation Steps

    [Given(@"the user navigates to Estate Management")]
    [When(@"the user navigates to Estate Management")]
    public async Task GivenTheUserNavigatesToEstateManagement()
    {
        await _estateManagementHelper.NavigateToEstateManagement();
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

    [Then(@"the Estate Management menu is not visible")]
    public async Task ThenTheEstateManagementMenuIsNotVisible()
    {
        // First navigate to home to check menu
        await _estateManagementHelper.NavigateToHome();
        await _estateManagementHelper.VerifyEstateManagementMenuIsNotVisible();
    }

    [Then(@"the Estate Management menu is visible")]
    public async Task ThenTheEstateManagementMenuIsVisible()
    {
        await _estateManagementHelper.VerifyEstateManagementMenuIsVisible();
    }

    #endregion

    #region Page Verification Steps

    [Then(@"the Estate Management page is displayed")]
    public async Task ThenTheEstateManagementPageIsDisplayed()
    {
        await _estateManagementHelper.VerifyEstateManagementPageIsDisplayed();
    }

    [Then(@"the page title is ""(.*)""")]
    public async Task ThenThePageTitleIs(string expectedTitle)
    {
        await _estateManagementHelper.VerifyPageTitle(expectedTitle);
    }

    #endregion

    #region Estate Details Steps

    [Then(@"the estate name is ""(.*)""")]
    public async Task ThenTheEstateNameIs(string expectedName)
    {
        await _estateManagementHelper.VerifyEstateName(expectedName);
    }

    [Then(@"the estate reference is ""(.*)""")]
    public async Task ThenTheEstateReferenceIs(string expectedReference)
    {
        await _estateManagementHelper.VerifyEstateReference(expectedReference);
    }

    #endregion

    #region Statistics Steps

    [Then(@"the Total Merchants count is ""(.*)""")]
    public async Task ThenTheTotalMerchantsCountIs(int expectedCount)
    {
        await _estateManagementHelper.VerifyTotalMerchantsCount(expectedCount);
    }

    [Then(@"the Total Operators count is ""(.*)""")]
    public async Task ThenTheTotalOperatorsCountIs(int expectedCount)
    {
        await _estateManagementHelper.VerifyTotalOperatorsCount(expectedCount);
    }

    [Then(@"the Total Contracts count is ""(.*)""")]
    public async Task ThenTheTotalContractsCountIs(int expectedCount)
    {
        await _estateManagementHelper.VerifyTotalContractsCount(expectedCount);
    }

    [Then(@"the Total Users count is ""(.*)""")]
    public async Task ThenTheTotalUsersCountIs(int expectedCount)
    {
        await _estateManagementHelper.VerifyTotalUsersCount(expectedCount);
    }

    #endregion

    #region Recent Merchants Steps

    [Then(@"the Recent Merchants section is displayed")]
    public async Task ThenTheRecentMerchantsSectionIsDisplayed()
    {
        await _estateManagementHelper.VerifyRecentMerchantsSection();
    }

    [Then(@"at least ""(.*)"" merchant is shown in Recent Merchants")]
    public async Task ThenAtLeastMerchantIsShownInRecentMerchants(int minCount)
    {
        await _estateManagementHelper.VerifyRecentMerchantsCount(minCount);
    }

    [Then(@"the merchant ""(.*)"" with reference ""(.*)"" is visible")]
    public async Task ThenTheMerchantWithReferenceIsVisible(string merchantName, string merchantReference)
    {
        await _estateManagementHelper.VerifyMerchantIsVisible(merchantName, merchantReference);
    }

    #endregion

    #region Contracts Steps

    [Then(@"the Contracts section is displayed")]
    public async Task ThenTheContractsSectionIsDisplayed()
    {
        await _estateManagementHelper.VerifyContractsSection();
    }

    [Then(@"at least ""(.*)"" contract is shown")]
    public async Task ThenAtLeastContractIsShown(int minCount)
    {
        await _estateManagementHelper.VerifyContractsCount(minCount);
    }

    [Then(@"the contract ""(.*)"" for operator ""(.*)"" is visible")]
    public async Task ThenTheContractForOperatorIsVisible(string contractDescription, string operatorName)
    {
        await _estateManagementHelper.VerifyContractIsVisible(contractDescription, operatorName);
    }

    #endregion

    #region Tab Navigation Steps

    [When(@"the user clicks on the ""(.*)"" tab")]
    public async Task WhenTheUserClicksOnTheTab(string tabName)
    {
        await _estateManagementHelper.ClickTab(tabName);
    }

    [Then(@"the operators tab content is displayed")]
    public async Task ThenTheOperatorsTabContentIsDisplayed()
    {
        await _estateManagementHelper.VerifyOperatorsTabContent();
    }

    #endregion

    #region Operators Tab Steps

    [Then(@"the Assigned Operators section is visible")]
    public async Task ThenTheAssignedOperatorsSectionIsVisible()
    {
        await _estateManagementHelper.VerifyAssignedOperatorsSection();
    }

    [Then(@"at least ""(.*)"" operators are assigned")]
    public async Task ThenAtLeastOperatorsAreAssigned(int minCount)
    {
        await _estateManagementHelper.VerifyAssignedOperatorsCount(minCount);
    }

    [Then(@"the operator ""(.*)"" is listed")]
    public async Task ThenTheOperatorIsListed(string operatorName)
    {
        await _estateManagementHelper.VerifyOperatorIsListed(operatorName);
    }

    [Then(@"the Add Operator button is visible")]
    public async Task ThenTheAddOperatorButtonIsVisible()
    {
        await _estateManagementHelper.VerifyAddOperatorButtonIsVisible();
    }

    [Then(@"the Add Operator button is not visible")]
    public async Task ThenTheAddOperatorButtonIsNotVisible()
    {
        await _estateManagementHelper.VerifyAddOperatorButtonIsNotVisible();
    }

    #endregion

    #region Add/Remove Operator Steps

    [When(@"the user removes the operator ""(.*)""")]
    public async Task WhenTheUserRemovesTheOperator(string operatorName)
    {
        await _estateManagementHelper.RemoveOperator(operatorName);
    }

    [When(@"the user clicks the Add Operator button")]
    public async Task WhenTheUserClicksTheAddOperatorButton()
    {
        await _estateManagementHelper.ClickAddOperatorButton();
    }

    [When(@"the user selects ""(.*)"" from the operator dropdown")]
    public async Task WhenTheUserSelectsFromTheOperatorDropdown(string operatorName)
    {
        await _estateManagementHelper.SelectOperatorFromDropdown(operatorName);
    }

    [When(@"the user clicks the Add button")]
    public async Task WhenTheUserClicksTheAddButton()
    {
        await _estateManagementHelper.ClickAddButton();
    }

    [Then(@"a success message is displayed")]
    public async Task ThenASuccessMessageIsDisplayed()
    {
        await _estateManagementHelper.VerifySuccessMessageIsDisplayed();
    }

    [Then(@"the success message contains ""(.*)""")]
    public async Task ThenTheSuccessMessageContains(string expectedMessage)
    {
        await _estateManagementHelper.VerifySuccessMessageContains(expectedMessage);
    }

    [Then(@"the operator ""(.*)"" is no longer listed")]
    public async Task ThenTheOperatorIsNoLongerListed(string operatorName)
    {
        await _estateManagementHelper.VerifyOperatorIsNotListed(operatorName);
    }

    [Then(@"the operator selection form is displayed")]
    public async Task ThenTheOperatorSelectionFormIsDisplayed()
    {
        await _estateManagementHelper.VerifyOperatorSelectionFormIsDisplayed();
    }

    #endregion
}
