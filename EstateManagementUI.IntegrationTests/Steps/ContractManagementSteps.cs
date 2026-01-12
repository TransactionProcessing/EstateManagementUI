using Microsoft.Playwright;
using Reqnroll;
using EstateManagementUI.IntegrationTests.Common;

namespace EstateManagementUI.IntegrationTests.Steps;

/// <summary>
/// Step definitions for Contract Management integration tests
/// Links feature file scenarios to browser automation code
/// </summary>
[Binding]
public class ContractManagementSteps
{
    private readonly IPage _page;
    private readonly ContractManagementPageHelper _contractHelper;
    private readonly ScenarioContext _scenarioContext;

    public ContractManagementSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
        _page = scenarioContext.ScenarioContainer.Resolve<IPage>();
        
        // Get base URL from environment variable or use default
        var baseUrl = Environment.GetEnvironmentVariable("APP_URL") ?? "https://localhost:5001";
        _contractHelper = new ContractManagementPageHelper(_page, baseUrl);
    }

    #region Navigation Steps

    [Given(@"the user navigates to the Contract Management page")]
    [When(@"the user navigates to the Contract Management page")]
    public async Task GivenTheUserNavigatesToTheContractManagementPage()
    {
        await _contractHelper.NavigateToContractManagement();
    }

    [When(@"the user navigates to the home page")]
    public async Task WhenTheUserNavigatesToTheHomePage()
    {
        await _contractHelper.NavigateToHome();
    }

    [When(@"the user navigates to the Create New Contract page")]
    [When(@"the user navigates to the Create New Contract page directly")]
    [Given(@"the user navigates to the Create New Contract page")]
    public async Task WhenTheUserNavigatesToTheCreateNewContractPage()
    {
        await _contractHelper.NavigateToCreateNewContract();
    }

    [When(@"the user navigates to the Edit Contract page directly")]
    public async Task WhenTheUserNavigatesToTheEditContractPageDirectly()
    {
        await _contractHelper.NavigateToEditFirstContract();
    }

    [When(@"the user navigates to the Edit Contract page for the first contract")]
    [Given(@"the user navigates to the Edit Contract page for the first contract")]
    public async Task WhenTheUserNavigatesToTheEditContractPageForTheFirstContract()
    {
        await _contractHelper.NavigateToEditFirstContract();
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

    [Then(@"the Contract Management menu item is not visible")]
    public async Task ThenTheContractManagementMenuItemIsNotVisible()
    {
        await _contractHelper.VerifyContractManagementMenuIsNotVisible();
    }

    [Then(@"the Contract Management menu item is visible")]
    public async Task ThenTheContractManagementMenuItemIsVisible()
    {
        await _contractHelper.VerifyContractManagementMenuIsVisible();
    }

    #endregion

    #region Page Display Steps

    [Then(@"the Contract Management page is displayed")]
    public async Task ThenTheContractManagementPageIsDisplayed()
    {
        await _contractHelper.VerifyContractManagementPageIsDisplayed();
    }

    [Then(@"the page title is ""(.*)""")]
    public async Task ThenThePageTitleIs(string expectedTitle)
    {
        await _contractHelper.VerifyPageTitle(expectedTitle);
    }

    [Then(@"an access denied message is displayed")]
    public async Task ThenAnAccessDeniedMessageIsDisplayed()
    {
        await _contractHelper.VerifyAccessDeniedMessageIsDisplayed();
    }

    [Then(@"an access denied message is displayed for contract creation")]
    public async Task ThenAnAccessDeniedMessageIsDisplayedForContractCreation()
    {
        await _contractHelper.VerifyAccessDeniedForContractCreation();
    }

    [Then(@"an access denied message is displayed for contract editing")]
    public async Task ThenAnAccessDeniedMessageIsDisplayedForContractEditing()
    {
        await _contractHelper.VerifyAccessDeniedForContractEditing();
    }

    [Then(@"the View Contract page is displayed")]
    public async Task ThenTheViewContractPageIsDisplayed()
    {
        await _contractHelper.VerifyViewContractPageIsDisplayed();
    }

    [Then(@"the Create New Contract page is displayed")]
    public async Task ThenTheCreateNewContractPageIsDisplayed()
    {
        await _contractHelper.VerifyCreateNewContractPageIsDisplayed();
    }

    [Then(@"the Edit Contract page is displayed")]
    public async Task ThenTheEditContractPageIsDisplayed()
    {
        await _contractHelper.VerifyEditContractPageIsDisplayed();
    }

    #endregion

    #region Contract List Verification Steps

    [Then(@"""(.*)"" contracts are displayed in the list")]
    public async Task ThenContractsAreDisplayedInTheList(int count)
    {
        await _contractHelper.VerifyContractCount(count);
    }

    [Then(@"the ""Add New Contract"" button is visible")]
    public async Task ThenTheAddNewContractButtonIsVisible()
    {
        await _contractHelper.VerifyAddNewContractButtonIsVisible();
    }

    [Then(@"the ""Add New Contract"" button is not visible")]
    public async Task ThenTheAddNewContractButtonIsNotVisible()
    {
        await _contractHelper.VerifyAddNewContractButtonIsNotVisible();
    }

    [Then(@"the ""Edit"" button is not visible for contracts")]
    public async Task ThenTheEditButtonIsNotVisibleForContracts()
    {
        await _contractHelper.VerifyEditButtonIsNotVisibleForContracts();
    }

    #endregion

    #region Contract Details Verification Steps

    [Then(@"the contract description is ""(.*)""")]
    public async Task ThenTheContractDescriptionIs(string description)
    {
        await _contractHelper.VerifyContractDescription(description);
    }

    [Then(@"the contract operator is ""(.*)""")]
    public async Task ThenTheContractOperatorIs(string operatorName)
    {
        await _contractHelper.VerifyContractOperator(operatorName);
    }

    [Then(@"the contract has ""(.*)"" products")]
    public async Task ThenTheContractHasProducts(int count)
    {
        await _contractHelper.VerifyContractProductCount(count);
    }

    [Then(@"the first product is ""(.*)""")]
    public async Task ThenTheFirstProductIs(string productName)
    {
        await _contractHelper.VerifyFirstProductName(productName);
    }

    [Then(@"the first product has ""(.*)"" transaction fees")]
    public async Task ThenTheFirstProductHasTransactionFees(int count)
    {
        await _contractHelper.VerifyFirstProductTransactionFeeCount(count);
    }

    [Then(@"the product ""(.*)"" has a transaction fee ""(.*)"" with value ""(.*)""")]
    public async Task ThenTheProductHasATransactionFeeWithValue(string productName, string feeDescription, string value)
    {
        await _contractHelper.VerifyProductTransactionFee(productName, feeDescription, value);
    }

    #endregion

    #region Form Verification Steps

    [Then(@"the contract form is displayed")]
    public async Task ThenTheContractFormIsDisplayed()
    {
        await _contractHelper.VerifyContractFormIsDisplayed();
    }

    [Then(@"the ""(.*)"" field is visible")]
    public async Task ThenTheFieldIsVisible(string fieldLabel)
    {
        await _contractHelper.VerifyFieldIsVisible(fieldLabel);
    }

    [Then(@"the ""(.*)"" dropdown is visible")]
    public async Task ThenTheDropdownIsVisible(string fieldLabel)
    {
        await _contractHelper.VerifyFieldIsVisible(fieldLabel);
    }

    [Then(@"the contract description field contains ""(.*)""")]
    public async Task ThenTheContractDescriptionFieldContains(string value)
    {
        await _contractHelper.VerifyDescriptionFieldContains(value);
    }

    [Then(@"the operator name is displayed as ""(.*)""")]
    public async Task ThenTheOperatorNameIsDisplayedAs(string operatorName)
    {
        await _contractHelper.VerifyOperatorNameDisplayed(operatorName);
    }

    [Then(@"the products section is displayed")]
    public async Task ThenTheProductsSectionIsDisplayed()
    {
        await _contractHelper.VerifyProductsSectionIsDisplayed();
    }

    [Then(@"""(.*)"" products are listed")]
    public async Task ThenProductsAreListed(int count)
    {
        await _contractHelper.VerifyProductCountOnEditPage(count);
    }

    [Then(@"the ""Add Product"" button is visible")]
    public async Task ThenTheAddProductButtonIsVisible()
    {
        await _contractHelper.VerifyAddProductButtonIsVisible();
    }

    [Then(@"validation errors are displayed")]
    public async Task ThenValidationErrorsAreDisplayed()
    {
        await _contractHelper.VerifyValidationErrorsAreDisplayed();
    }

    [Then(@"the ""(.*)"" error is shown")]
    public async Task ThenTheErrorIsShown(string errorMessage)
    {
        await _contractHelper.VerifyValidationErrorMessage(errorMessage);
    }

    #endregion

    #region Interaction Steps

    [When(@"the user clicks on the ""View"" button for the first contract")]
    public async Task WhenTheUserClicksOnTheViewButtonForTheFirstContract()
    {
        await _contractHelper.ClickViewButtonForFirstContract();
    }

    [When(@"the user clicks on the ""Edit"" button for the first contract")]
    public async Task WhenTheUserClicksOnTheEditButtonForTheFirstContract()
    {
        await _contractHelper.ClickEditButtonForFirstContract();
    }

    [When(@"the user clicks on the ""Add New Contract"" button")]
    public async Task WhenTheUserClicksOnTheAddNewContractButton()
    {
        await _contractHelper.ClickAddNewContractButton();
    }

    [When(@"the user enters ""(.*)"" in the ""Description"" field")]
    public async Task WhenTheUserEntersInTheDescriptionField(string text)
    {
        await _contractHelper.EnterTextInField("Description", text);
    }

    [When(@"the user selects ""(.*)"" from the ""Operator"" dropdown")]
    public async Task WhenTheUserSelectsFromTheOperatorDropdown(string option)
    {
        await _contractHelper.SelectFromDropdown("Operator", option);
    }

    [When(@"the user clicks on the ""Create Contract"" button")]
    public async Task WhenTheUserClicksOnTheCreateContractButton()
    {
        await _contractHelper.ClickCreateContractButton();
    }

    [Then(@"the contract is created successfully")]
    public async Task ThenTheContractIsCreatedSuccessfully()
    {
        await _contractHelper.VerifyContractCreatedSuccessfully();
    }

    [Then(@"the user is redirected to the Contract Management page")]
    public async Task ThenTheUserIsRedirectedToTheContractManagementPage()
    {
        await _contractHelper.VerifyRedirectedToContractManagementPage();
    }

    #endregion

    #region Add Product Steps

    [When(@"the user clicks on the ""Add Product"" button")]
    public async Task WhenTheUserClicksOnTheAddProductButton()
    {
        await _contractHelper.ClickAddProductButton();
    }

    [Then(@"the Add Product modal is displayed")]
    public async Task ThenTheAddProductModalIsDisplayed()
    {
        await _contractHelper.VerifyAddProductModalIsDisplayed();
    }

    [When(@"the user enters ""(.*)"" in the product name field")]
    public async Task WhenTheUserEntersInTheProductNameField(string productName)
    {
        await _contractHelper.EnterProductName(productName);
    }

    [When(@"the user enters ""(.*)"" in the display text field")]
    public async Task WhenTheUserEntersInTheDisplayTextField(string displayText)
    {
        await _contractHelper.EnterDisplayText(displayText);
    }

    [When(@"the user enters ""(.*)"" in the value field")]
    public async Task WhenTheUserEntersInTheValueField(string value)
    {
        await _contractHelper.EnterValue(value);
    }

    [When(@"the user checks the ""Variable Value"" checkbox")]
    public async Task WhenTheUserChecksTheVariableValueCheckbox()
    {
        await _contractHelper.CheckVariableValueCheckbox();
    }

    [When(@"the user clicks on the ""Add Product"" button in the modal")]
    public async Task WhenTheUserClicksOnTheAddProductButtonInTheModal()
    {
        await _contractHelper.ClickAddProductButtonInModal();
    }

    [Then(@"the product is added successfully")]
    public async Task ThenTheProductIsAddedSuccessfully()
    {
        await _contractHelper.VerifyProductAddedSuccessfully();
    }

    [Then(@"the Add Product modal is closed")]
    public async Task ThenTheAddProductModalIsClosed()
    {
        await _contractHelper.VerifyAddProductModalIsClosed();
    }

    #endregion

    #region Add Transaction Fee Steps

    [When(@"the user clicks on the ""Add Fee"" button for the first product")]
    public async Task WhenTheUserClicksOnTheAddFeeButtonForTheFirstProduct()
    {
        await _contractHelper.ClickAddFeeButtonForFirstProduct();
    }

    [Then(@"the Add Transaction Fee modal is displayed")]
    public async Task ThenTheAddTransactionFeeModalIsDisplayed()
    {
        await _contractHelper.VerifyAddTransactionFeeModalIsDisplayed();
    }

    [When(@"the user enters ""(.*)"" in the fee description field")]
    public async Task WhenTheUserEntersInTheFeeDescriptionField(string description)
    {
        await _contractHelper.EnterFeeDescription(description);
    }

    [When(@"the user selects ""(.*)"" from the calculation type dropdown")]
    public async Task WhenTheUserSelectsFromTheCalculationTypeDropdown(string calculationType)
    {
        await _contractHelper.SelectCalculationType(calculationType);
    }

    [When(@"the user selects ""(.*)"" from the fee type dropdown")]
    public async Task WhenTheUserSelectsFromTheFeeTypeDropdown(string feeType)
    {
        await _contractHelper.SelectFeeType(feeType);
    }

    [When(@"the user enters ""(.*)"" in the fee value field")]
    public async Task WhenTheUserEntersInTheFeeValueField(string value)
    {
        await _contractHelper.EnterFeeValue(value);
    }

    [When(@"the user clicks on the ""Add Fee"" button in the modal")]
    public async Task WhenTheUserClicksOnTheAddFeeButtonInTheModal()
    {
        await _contractHelper.ClickAddFeeButtonInModal();
    }

    [Then(@"the transaction fee is added successfully")]
    public async Task ThenTheTransactionFeeIsAddedSuccessfully()
    {
        await _contractHelper.VerifyTransactionFeeAddedSuccessfully();
    }

    [Then(@"the Add Transaction Fee modal is closed")]
    public async Task ThenTheAddTransactionFeeModalIsClosed()
    {
        await _contractHelper.VerifyAddTransactionFeeModalIsClosed();
    }

    #endregion
}
