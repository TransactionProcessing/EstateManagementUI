using Microsoft.Playwright;
using Reqnroll;
using EstateManagementUI.IntegrationTests.Common;

namespace EstateManagementUI.IntegrationTests.Steps;

/// <summary>
/// Step definitions for Operator Management integration tests
/// Links feature file scenarios to browser automation code
/// </summary>
[Binding]
public class OperatorManagementSteps
{
    private readonly IPage _page;
    private readonly OperatorManagementPageHelper _operatorManagementHelper;
    private readonly ScenarioContext _scenarioContext;

    public OperatorManagementSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
        _page = scenarioContext.ScenarioContainer.Resolve<IPage>();
        
        // Get base URL from environment variable or use default
        var baseUrl = Environment.GetEnvironmentVariable("APP_URL") ?? "https://localhost:5001";
        _operatorManagementHelper = new OperatorManagementPageHelper(_page, baseUrl);
    }

    #region Navigation Steps

    [Given(@"the user navigates to Operator Management")]
    [When(@"the user navigates to Operator Management")]
    public async Task GivenTheUserNavigatesToOperatorManagement()
    {
        await _operatorManagementHelper.NavigateToOperatorManagement();
    }

    [When(@"the user navigates directly to Edit Operator page for ""(.*)""")]
    public async Task WhenTheUserNavigatesDirectlyToEditOperatorPageFor(string operatorName)
    {
        await _operatorManagementHelper.NavigateToEditOperatorPage(operatorName);
    }

    #endregion

    #region Authentication/Role Steps

    [Given(@"the user is authenticated as an? ""(.*)"" user")]
    public async Task GivenTheUserIsAuthenticatedAsAUser(string role)
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

    [Then(@"the Operator Management menu is not visible")]
    public async Task ThenTheOperatorManagementMenuIsNotVisible()
    {
        // First navigate to home to check menu
        await _operatorManagementHelper.NavigateToHome();
        await _operatorManagementHelper.VerifyOperatorManagementMenuIsNotVisible();
    }

    [Then(@"the Operator Management menu is visible")]
    public async Task ThenTheOperatorManagementMenuIsVisible()
    {
        await _operatorManagementHelper.VerifyOperatorManagementMenuIsVisible();
    }

    #endregion

    #region Page Verification Steps

    [Then(@"the Operator Management page is displayed")]
    public async Task ThenTheOperatorManagementPageIsDisplayed()
    {
        await _operatorManagementHelper.VerifyOperatorManagementPageIsDisplayed();
    }

    [Then(@"the page title is ""(.*)""")]
    public async Task ThenThePageTitleIs(string expectedTitle)
    {
        await _operatorManagementHelper.VerifyPageTitle(expectedTitle);
    }

    [Then(@"the View Operator page is displayed")]
    public async Task ThenTheViewOperatorPageIsDisplayed()
    {
        await _operatorManagementHelper.VerifyViewOperatorPageIsDisplayed();
    }

    [Then(@"the Create New Operator page is displayed")]
    public async Task ThenTheCreateNewOperatorPageIsDisplayed()
    {
        await _operatorManagementHelper.VerifyCreateNewOperatorPageIsDisplayed();
    }

    [Then(@"the Edit Operator page is displayed")]
    public async Task ThenTheEditOperatorPageIsDisplayed()
    {
        await _operatorManagementHelper.VerifyEditOperatorPageIsDisplayed();
    }

    [Then(@"an access denied message is displayed")]
    public async Task ThenAnAccessDeniedMessageIsDisplayed()
    {
        await _operatorManagementHelper.VerifyAccessDeniedMessageIsDisplayed();
    }

    [Then(@"the message indicates no permission to edit operators")]
    public async Task ThenTheMessageIndicatesNoPermissionToEditOperators()
    {
        await _operatorManagementHelper.VerifyAccessDeniedMessageForEditOperators();
    }

    #endregion

    #region Operator List Verification Steps

    [Then(@"the operator list contains ""(.*)"" operators")]
    public async Task ThenTheOperatorListContainsOperators(int expectedCount)
    {
        await _operatorManagementHelper.VerifyOperatorListCount(expectedCount);
    }

    [Then(@"the operator ""(.*)"" is listed")]
    public async Task ThenTheOperatorIsListed(string operatorName)
    {
        await _operatorManagementHelper.VerifyOperatorIsListed(operatorName);
    }

    [Then(@"the operator ""(.*)"" shows custom merchant number as ""(.*)""")]
    public async Task ThenTheOperatorShowsCustomMerchantNumberAs(string operatorName, string expectedStatus)
    {
        await _operatorManagementHelper.VerifyOperatorCustomMerchantNumberRequirement(operatorName, expectedStatus);
    }

    [Then(@"the operator ""(.*)"" shows custom terminal number as ""(.*)""")]
    public async Task ThenTheOperatorShowsCustomTerminalNumberAs(string operatorName, string expectedStatus)
    {
        await _operatorManagementHelper.VerifyOperatorCustomTerminalNumberRequirement(operatorName, expectedStatus);
    }

    #endregion

    #region Button Visibility Steps

    [Then(@"the Add New Operator button is visible")]
    public async Task ThenTheAddNewOperatorButtonIsVisible()
    {
        await _operatorManagementHelper.VerifyAddNewOperatorButtonIsVisible();
    }

    [Then(@"the Add New Operator button is not visible")]
    public async Task ThenTheAddNewOperatorButtonIsNotVisible()
    {
        await _operatorManagementHelper.VerifyAddNewOperatorButtonIsNotVisible();
    }

    [Then(@"the Edit Operator button is visible")]
    public async Task ThenTheEditOperatorButtonIsVisible()
    {
        await _operatorManagementHelper.VerifyEditOperatorButtonIsVisible();
    }

    [Then(@"the Edit button is visible for operator ""(.*)""")]
    public async Task ThenTheEditButtonIsVisibleForOperator(string operatorName)
    {
        await _operatorManagementHelper.VerifyEditButtonIsVisibleForOperator(operatorName);
    }

    [Then(@"the Edit button is not visible for operator ""(.*)""")]
    public async Task ThenTheEditButtonIsNotVisibleForOperator(string operatorName)
    {
        await _operatorManagementHelper.VerifyEditButtonIsNotVisibleForOperator(operatorName);
    }

    #endregion

    #region Operator Detail Verification Steps

    [Then(@"the operator name is ""(.*)""")]
    public async Task ThenTheOperatorNameIs(string expectedName)
    {
        await _operatorManagementHelper.VerifyOperatorName(expectedName);
    }

    [Then(@"the operator custom merchant number requirement is ""(.*)""")]
    public async Task ThenTheOperatorCustomMerchantNumberRequirementIs(string expectedStatus)
    {
        await _operatorManagementHelper.VerifyOperatorCustomMerchantNumberRequirementDetail(expectedStatus);
    }

    [Then(@"the operator custom terminal number requirement is ""(.*)""")]
    public async Task ThenTheOperatorCustomTerminalNumberRequirementIs(string expectedStatus)
    {
        await _operatorManagementHelper.VerifyOperatorCustomTerminalNumberRequirementDetail(expectedStatus);
    }

    #endregion

    #region Form Field Verification Steps

    [Then(@"the operator name field is visible")]
    public async Task ThenTheOperatorNameFieldIsVisible()
    {
        await _operatorManagementHelper.VerifyOperatorNameFieldIsVisible();
    }

    [Then(@"the operator name field contains ""(.*)""")]
    public async Task ThenTheOperatorNameFieldContains(string expectedValue)
    {
        await _operatorManagementHelper.VerifyOperatorNameFieldContains(expectedValue);
    }

    [Then(@"the custom merchant number checkbox is visible")]
    public async Task ThenTheCustomMerchantNumberCheckboxIsVisible()
    {
        await _operatorManagementHelper.VerifyCustomMerchantNumberCheckboxIsVisible();
    }

    [Then(@"the custom terminal number checkbox is visible")]
    public async Task ThenTheCustomTerminalNumberCheckboxIsVisible()
    {
        await _operatorManagementHelper.VerifyCustomTerminalNumberCheckboxIsVisible();
    }

    #endregion

    #region Navigation Action Steps

    [When(@"the user clicks on operator ""(.*)""")]
    public async Task WhenTheUserClicksOnOperator(string operatorName)
    {
        await _operatorManagementHelper.ClickOnOperator(operatorName);
    }

    [When(@"the user clicks the Add New Operator button")]
    public async Task WhenTheUserClicksTheAddNewOperatorButton()
    {
        await _operatorManagementHelper.ClickAddNewOperatorButton();
    }

    [When(@"the user clicks edit for operator ""(.*)""")]
    public async Task WhenTheUserClicksEditForOperator(string operatorName)
    {
        await _operatorManagementHelper.ClickEditForOperator(operatorName);
    }

    [When(@"the user clicks the Edit Operator button")]
    public async Task WhenTheUserClicksTheEditOperatorButton()
    {
        await _operatorManagementHelper.ClickEditOperatorButton();
    }

    #endregion

    #region Edit Operations Steps

    [When(@"the user updates the operator name to ""(.*)""")]
    public async Task WhenTheUserUpdatesTheOperatorNameTo(string newName)
    {
        await _operatorManagementHelper.UpdateOperatorName(newName);
    }

    [When(@"the user updates the custom merchant number requirement to checked")]
    public async Task WhenTheUserUpdatesTheCustomMerchantNumberRequirementToChecked()
    {
        await _operatorManagementHelper.UpdateCustomMerchantNumberRequirement(true);
    }

    [When(@"the user updates the custom terminal number requirement to checked")]
    public async Task WhenTheUserUpdatesTheCustomTerminalNumberRequirementToChecked()
    {
        await _operatorManagementHelper.UpdateCustomTerminalNumberRequirement(true);
    }

    [When(@"the user clicks the Save Changes button")]
    public async Task WhenTheUserClicksTheSaveChangesButton()
    {
        await _operatorManagementHelper.ClickSaveChangesButton();
    }

    [When(@"the user enters ""(.*)"" as the operator name")]
    public async Task WhenTheUserEntersAsTheOperatorName(string operatorName)
    {
        await _operatorManagementHelper.EnterOperatorName(operatorName);
    }

    [When(@"the user clicks the Create Operator button")]
    public async Task WhenTheUserClicksTheCreateOperatorButton()
    {
        await _operatorManagementHelper.ClickCreateOperatorButton();
    }

    #endregion

    #region Success/Error Message Steps

    [Then(@"a success message is displayed")]
    public async Task ThenASuccessMessageIsDisplayed()
    {
        await _operatorManagementHelper.VerifySuccessMessageIsDisplayed();
    }

    [Then(@"a success message is displayed or user is redirected to operator list")]
    public async Task ThenASuccessMessageIsDisplayedOrUserIsRedirectedToOperatorList()
    {
        // Check if either success message is shown or user was redirected
        try
        {
            await _operatorManagementHelper.VerifySuccessMessageIsDisplayed();
        }
        catch
        {
            // If no success message, check if redirected to list
            await _operatorManagementHelper.VerifyRedirectedToOperatorList();
        }
    }

    [Then(@"the success message contains ""(.*)""")]
    public async Task ThenTheSuccessMessageContains(string expectedMessage)
    {
        await _operatorManagementHelper.VerifySuccessMessageContains(expectedMessage);
    }

    [Then(@"a validation error is displayed for operator name")]
    public async Task ThenAValidationErrorIsDisplayedForOperatorName()
    {
        await _operatorManagementHelper.VerifyValidationErrorForOperatorName();
    }

    #endregion
}
