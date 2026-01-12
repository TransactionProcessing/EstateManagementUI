using Microsoft.Playwright;
using Shouldly;

namespace EstateManagementUI.IntegrationTests.Common;

/// <summary>
/// Helper class for interacting with the Contract Management pages using Playwright
/// </summary>
public class ContractManagementPageHelper
{
    private readonly IPage _page;
    private readonly string _baseUrl;

    public ContractManagementPageHelper(IPage page, string baseUrl)
    {
        _page = page;
        _baseUrl = baseUrl;
    }

    #region Navigation

    /// <summary>
    /// Navigate to the Contract Management page (index)
    /// </summary>
    public async Task NavigateToContractManagement()
    {
        await _page.GotoAsync($"{_baseUrl}/contracts");
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    /// <summary>
    /// Navigate to home page to check menu visibility
    /// </summary>
    public async Task NavigateToHome()
    {
        await _page.GotoAsync(_baseUrl);
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    /// <summary>
    /// Navigate to the Create New Contract page
    /// </summary>
    public async Task NavigateToCreateNewContract()
    {
        await _page.GotoAsync($"{_baseUrl}/contracts/new");
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    /// <summary>
    /// Navigate to the Edit Contract page for a specific contract
    /// </summary>
    public async Task NavigateToEditContract(string contractId)
    {
        await _page.GotoAsync($"{_baseUrl}/contracts/{contractId}/edit");
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    /// <summary>
    /// Navigate to the Edit Contract page for the first contract (using hardcoded test data ID)
    /// </summary>
    public async Task NavigateToEditFirstContract()
    {
        // Using the hardcoded test data contract ID from StubbedMediatorService
        await NavigateToEditContract("44444444-4444-4444-4444-444444444444");
    }

    #endregion

    #region Menu Visibility Verification

    /// <summary>
    /// Verify that Contract Management menu is not visible (Administrator role)
    /// </summary>
    public async Task VerifyContractManagementMenuIsNotVisible()
    {
        // Wait for page to be in stable state
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        
        var menuLinkCount = await _page.Locator("text=Contract Management").CountAsync();
        menuLinkCount.ShouldBe(0, "Contract Management menu should not be visible for Administrator role");
    }

    /// <summary>
    /// Verify that Contract Management menu is visible
    /// </summary>
    public async Task VerifyContractManagementMenuIsVisible()
    {
        await _page.Locator("text=Contract Management").WaitForAsync();
        var menuLinkCount = await _page.Locator("text=Contract Management").CountAsync();
        menuLinkCount.ShouldBeGreaterThan(0, "Contract Management menu should be visible");
    }

    #endregion

    #region Page Verification

    /// <summary>
    /// Verify the Contract Management page is displayed
    /// </summary>
    public async Task VerifyContractManagementPageIsDisplayed()
    {
        await _page.Locator("h1:has-text('Contract Management')").WaitForAsync();
        var heading = await _page.Locator("h1:has-text('Contract Management')").TextContentAsync();
        heading.ShouldNotBeNull();
        heading.ShouldContain("Contract Management");
    }

    /// <summary>
    /// Verify the page title
    /// </summary>
    public async Task VerifyPageTitle(string expectedTitle)
    {
        var title = await _page.TitleAsync();
        title.ShouldBe(expectedTitle);
    }

    /// <summary>
    /// Verify the View Contract page is displayed
    /// </summary>
    public async Task VerifyViewContractPageIsDisplayed()
    {
        await _page.Locator("h1:has-text('View Contract')").WaitForAsync();
        var heading = await _page.Locator("h1").TextContentAsync();
        heading.ShouldNotBeNull();
        heading.ShouldContain("View Contract");
    }

    /// <summary>
    /// Verify the Create New Contract page is displayed
    /// </summary>
    public async Task VerifyCreateNewContractPageIsDisplayed()
    {
        await _page.Locator("h1:has-text('Create New Contract')").WaitForAsync();
        var heading = await _page.Locator("h1:has-text('Create New Contract')").TextContentAsync();
        heading.ShouldNotBeNull();
        heading.ShouldContain("Create New Contract");
    }

    /// <summary>
    /// Verify the Edit Contract page is displayed
    /// </summary>
    public async Task VerifyEditContractPageIsDisplayed()
    {
        await _page.Locator("h1:has-text('Edit Contract')").WaitForAsync();
        var heading = await _page.Locator("h1").TextContentAsync();
        heading.ShouldNotBeNull();
        heading.ShouldContain("Edit Contract");
    }

    /// <summary>
    /// Verify access denied message is displayed
    /// </summary>
    public async Task VerifyAccessDeniedMessageIsDisplayed()
    {
        await _page.Locator("h3:has-text('Access Denied')").WaitForAsync();
        var message = await _page.Locator("h3:has-text('Access Denied')").TextContentAsync();
        message.ShouldNotBeNull();
        message.ShouldContain("Access Denied");
    }

    /// <summary>
    /// Verify access denied message for contract creation
    /// </summary>
    public async Task VerifyAccessDeniedForContractCreation()
    {
        await _page.Locator("text=You don't have permission to create contracts").WaitForAsync();
    }

    /// <summary>
    /// Verify access denied message for contract editing
    /// </summary>
    public async Task VerifyAccessDeniedForContractEditing()
    {
        await _page.Locator("text=You don't have permission to edit contracts").WaitForAsync();
    }

    #endregion

    #region Contract List Verification

    /// <summary>
    /// Verify the number of contracts displayed in the list
    /// </summary>
    public async Task VerifyContractCount(int expectedCount)
    {
        // Wait for contracts grid to be present
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        
        var contractCards = _page.Locator(".bg-white.rounded-lg.shadow-md.p-6.hover\\:shadow-lg");
        var count = await contractCards.CountAsync();
        count.ShouldBe(expectedCount, $"Expected {expectedCount} contracts but found {count}");
    }

    /// <summary>
    /// Verify the "Add New Contract" button is visible
    /// </summary>
    public async Task VerifyAddNewContractButtonIsVisible()
    {
        await _page.Locator("#newContractButton").WaitForAsync();
        var button = await _page.Locator("#newContractButton").IsVisibleAsync();
        button.ShouldBeTrue("Add New Contract button should be visible");
    }

    /// <summary>
    /// Verify the "Add New Contract" button is not visible
    /// </summary>
    public async Task VerifyAddNewContractButtonIsNotVisible()
    {
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        var buttonCount = await _page.Locator("#newContractButton").CountAsync();
        buttonCount.ShouldBe(0, "Add New Contract button should not be visible");
    }

    /// <summary>
    /// Verify Edit button is not visible for contracts (Viewer role)
    /// </summary>
    public async Task VerifyEditButtonIsNotVisibleForContracts()
    {
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        var editButtonCount = await _page.Locator("button:has-text('Edit')").CountAsync();
        editButtonCount.ShouldBe(0, "Edit button should not be visible for contracts");
    }

    #endregion

    #region Contract Details Verification

    /// <summary>
    /// Verify contract description on view page
    /// </summary>
    public async Task VerifyContractDescription(string expectedDescription)
    {
        var descriptionElement = _page.Locator("p.text-gray-900.font-medium").First;
        var description = await descriptionElement.TextContentAsync();
        description.ShouldNotBeNull();
        description.Trim().ShouldBe(expectedDescription);
    }

    /// <summary>
    /// Verify contract operator on view page
    /// </summary>
    public async Task VerifyContractOperator(string expectedOperator)
    {
        var operatorElements = _page.Locator("p.text-gray-900.font-medium");
        var operatorText = await operatorElements.Nth(1).TextContentAsync();
        operatorText.ShouldNotBeNull();
        operatorText.Trim().ShouldBe(expectedOperator);
    }

    /// <summary>
    /// Verify the number of products in a contract
    /// </summary>
    public async Task VerifyContractProductCount(int expectedCount)
    {
        var productCards = _page.Locator(".border.border-gray-200.rounded-lg.p-4");
        var count = await productCards.CountAsync();
        count.ShouldBe(expectedCount, $"Expected {expectedCount} products but found {count}");
    }

    /// <summary>
    /// Verify the first product name
    /// </summary>
    public async Task VerifyFirstProductName(string expectedName)
    {
        var productNameElement = _page.Locator("h4.text-md.font-semibold.text-gray-900").First;
        var productName = await productNameElement.TextContentAsync();
        productName.ShouldNotBeNull();
        productName.Trim().ShouldBe(expectedName);
    }

    /// <summary>
    /// Verify the number of transaction fees for the first product
    /// </summary>
    public async Task VerifyFirstProductTransactionFeeCount(int expectedCount)
    {
        var feeCards = _page.Locator(".bg-gray-50.rounded.p-3");
        var count = await feeCards.CountAsync();
        count.ShouldBe(expectedCount, $"Expected {expectedCount} transaction fees but found {count}");
    }

    /// <summary>
    /// Verify a specific transaction fee exists with expected value
    /// </summary>
    public async Task VerifyProductTransactionFee(string productName, string feeDescription, string expectedValue)
    {
        var feeCard = _page.Locator($".bg-gray-50.rounded.p-3:has-text('{feeDescription}')");
        await feeCard.WaitForAsync();
        
        var feeText = await feeCard.TextContentAsync();
        feeText.ShouldNotBeNull();
        feeText.ShouldContain(feeDescription);
        feeText.ShouldContain(expectedValue);
    }

    #endregion

    #region Form Verification

    /// <summary>
    /// Verify the contract form is displayed
    /// </summary>
    public async Task VerifyContractFormIsDisplayed()
    {
        await _page.Locator("form").WaitForAsync();
        var form = await _page.Locator("form").IsVisibleAsync();
        form.ShouldBeTrue("Contract form should be displayed");
    }

    /// <summary>
    /// Verify a specific field is visible
    /// </summary>
    public async Task VerifyFieldIsVisible(string fieldLabel)
    {
        var field = _page.Locator($"label:has-text('{fieldLabel}')");
        await field.WaitForAsync();
        var isVisible = await field.IsVisibleAsync();
        isVisible.ShouldBeTrue($"{fieldLabel} field should be visible");
    }

    /// <summary>
    /// Verify description field contains specific value on edit page
    /// </summary>
    public async Task VerifyDescriptionFieldContains(string expectedValue)
    {
        var descriptionInput = _page.Locator("input[type='text']").First;
        var value = await descriptionInput.InputValueAsync();
        value.ShouldBe(expectedValue);
    }

    /// <summary>
    /// Verify operator name is displayed on edit page
    /// </summary>
    public async Task VerifyOperatorNameDisplayed(string expectedOperator)
    {
        var operatorText = await _page.Locator("p.text-gray-900.font-medium").First.TextContentAsync();
        operatorText.ShouldNotBeNull();
        operatorText.Trim().ShouldBe(expectedOperator);
    }

    /// <summary>
    /// Verify products section is displayed on edit page
    /// </summary>
    public async Task VerifyProductsSectionIsDisplayed()
    {
        await _page.Locator("h3:has-text('Products')").WaitForAsync();
    }

    /// <summary>
    /// Verify the number of products listed on edit page
    /// </summary>
    public async Task VerifyProductCountOnEditPage(int expectedCount)
    {
        var productCards = _page.Locator(".border.border-gray-200.rounded-lg.p-4");
        var count = await productCards.CountAsync();
        count.ShouldBe(expectedCount, $"Expected {expectedCount} products but found {count}");
    }

    /// <summary>
    /// Verify Add Product button is visible
    /// </summary>
    public async Task VerifyAddProductButtonIsVisible()
    {
        var button = _page.Locator("button:has-text('Add Product')");
        await button.WaitForAsync();
        var isVisible = await button.IsVisibleAsync();
        isVisible.ShouldBeTrue("Add Product button should be visible");
    }

    /// <summary>
    /// Verify validation errors are displayed
    /// </summary>
    public async Task VerifyValidationErrorsAreDisplayed()
    {
        var errors = _page.Locator(".text-red-600.text-sm");
        var count = await errors.CountAsync();
        count.ShouldBeGreaterThan(0, "Validation errors should be displayed");
    }

    /// <summary>
    /// Verify specific validation error is shown
    /// </summary>
    public async Task VerifyValidationErrorMessage(string errorMessage)
    {
        await _page.Locator($"text={errorMessage}").WaitForAsync();
    }

    #endregion

    #region Interaction Methods

    /// <summary>
    /// Click on View button for the first contract
    /// </summary>
    public async Task ClickViewButtonForFirstContract()
    {
        var viewButton = _page.Locator("button:has-text('View')").First;
        await viewButton.ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    /// <summary>
    /// Click on Edit button for the first contract
    /// </summary>
    public async Task ClickEditButtonForFirstContract()
    {
        var editButton = _page.Locator("button:has-text('Edit')").First;
        await editButton.ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    /// <summary>
    /// Click on Add New Contract button
    /// </summary>
    public async Task ClickAddNewContractButton()
    {
        await _page.Locator("#newContractButton").ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    /// <summary>
    /// Enter text in a form field
    /// </summary>
    public async Task EnterTextInField(string fieldLabel, string text)
    {
        // Find the input associated with the label
        var input = _page.Locator($"label:has-text('{fieldLabel}')").Locator("..").Locator("input");
        await input.FillAsync(text);
    }

    /// <summary>
    /// Select an option from a dropdown
    /// </summary>
    public async Task SelectFromDropdown(string fieldLabel, string option)
    {
        var select = _page.Locator($"label:has-text('{fieldLabel}')").Locator("..").Locator("select");
        await select.SelectOptionAsync(new[] { option });
    }

    /// <summary>
    /// Click on a button by its text or ID
    /// </summary>
    public async Task ClickButton(string buttonText)
    {
        var button = _page.Locator($"button:has-text('{buttonText}')").First;
        await button.ClickAsync();
        await Task.Delay(500); // Wait for any state changes
    }

    /// <summary>
    /// Click on Create Contract button (with ID)
    /// </summary>
    public async Task ClickCreateContractButton()
    {
        await _page.Locator("#createContractButton").ClickAsync();
        await Task.Delay(500);
    }

    /// <summary>
    /// Verify user is redirected to Contract Management page
    /// </summary>
    public async Task VerifyRedirectedToContractManagementPage()
    {
        await _page.WaitForURLAsync("**/contracts");
        var url = _page.Url;
        url.ShouldEndWith("/contracts");
    }

    /// <summary>
    /// Verify contract is created successfully
    /// </summary>
    public async Task VerifyContractCreatedSuccessfully()
    {
        // After creation, we should be on the contracts list page
        await VerifyRedirectedToContractManagementPage();
    }

    /// <summary>
    /// Click Add Product button
    /// </summary>
    public async Task ClickAddProductButton()
    {
        var button = _page.Locator("button:has-text('Add Product')").First;
        await button.ClickAsync();
        await Task.Delay(300);
    }

    /// <summary>
    /// Verify Add Product modal is displayed
    /// </summary>
    public async Task VerifyAddProductModalIsDisplayed()
    {
        await _page.Locator("h3:has-text('Add New Product')").WaitForAsync();
    }

    /// <summary>
    /// Enter text in product name field
    /// </summary>
    public async Task EnterProductName(string productName)
    {
        var input = _page.Locator("label:has-text('Product Name')").Locator("..").Locator("input");
        await input.FillAsync(productName);
    }

    /// <summary>
    /// Enter text in display text field
    /// </summary>
    public async Task EnterDisplayText(string displayText)
    {
        var input = _page.Locator("label:has-text('Display Text')").Locator("..").Locator("input");
        await input.FillAsync(displayText);
    }

    /// <summary>
    /// Enter value in the value field
    /// </summary>
    public async Task EnterValue(string value)
    {
        var input = _page.Locator("label:has-text('Value')").Locator("..").Locator("input");
        await input.FillAsync(value);
    }

    /// <summary>
    /// Check the Variable Value checkbox
    /// </summary>
    public async Task CheckVariableValueCheckbox()
    {
        var checkbox = _page.Locator("input[type='checkbox']");
        await checkbox.CheckAsync();
    }

    /// <summary>
    /// Click Add Product button in modal
    /// </summary>
    public async Task ClickAddProductButtonInModal()
    {
        var button = _page.Locator(".bg-white.rounded-lg.p-6").Locator("button:has-text('Add Product')");
        await button.ClickAsync();
        await Task.Delay(500);
    }

    /// <summary>
    /// Verify product is added successfully
    /// </summary>
    public async Task VerifyProductAddedSuccessfully()
    {
        // Wait for modal to close
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        var modalCount = await _page.Locator("h3:has-text('Add New Product')").CountAsync();
        modalCount.ShouldBe(0, "Add Product modal should be closed after successful addition");
    }

    /// <summary>
    /// Verify Add Product modal is closed
    /// </summary>
    public async Task VerifyAddProductModalIsClosed()
    {
        var modalCount = await _page.Locator("h3:has-text('Add New Product')").CountAsync();
        modalCount.ShouldBe(0, "Add Product modal should be closed");
    }

    /// <summary>
    /// Click Add Fee button for the first product
    /// </summary>
    public async Task ClickAddFeeButtonForFirstProduct()
    {
        var button = _page.Locator("button:has-text('Add Fee')").First;
        await button.ClickAsync();
        await Task.Delay(300);
    }

    /// <summary>
    /// Verify Add Transaction Fee modal is displayed
    /// </summary>
    public async Task VerifyAddTransactionFeeModalIsDisplayed()
    {
        await _page.Locator("h3:has-text('Add Transaction Fee')").WaitForAsync();
    }

    /// <summary>
    /// Enter text in fee description field
    /// </summary>
    public async Task EnterFeeDescription(string description)
    {
        var input = _page.Locator("label:has-text('Description')").Locator("..").Locator("input");
        await input.FillAsync(description);
    }

    /// <summary>
    /// Select calculation type from dropdown
    /// </summary>
    public async Task SelectCalculationType(string calculationType)
    {
        var select = _page.Locator("label:has-text('Calculation Type')").Locator("..").Locator("select");
        
        // Map text to value with proper error handling
        var value = calculationType.ToLower() switch
        {
            "fixed" => "0",
            "percentage" => "1",
            _ => throw new ArgumentException($"Unknown calculation type: {calculationType}. Expected 'Fixed' or 'Percentage'.")
        };
        await select.SelectOptionAsync(new[] { value });
    }

    /// <summary>
    /// Select fee type from dropdown
    /// </summary>
    public async Task SelectFeeType(string feeType)
    {
        var select = _page.Locator("label:has-text('Fee Type')").Locator("..").Locator("select");
        
        // Map text to value with proper error handling
        var value = feeType.ToLower() switch
        {
            "merchant" => "0",
            "service provider" => "1",
            _ => throw new ArgumentException($"Unknown fee type: {feeType}. Expected 'Merchant' or 'Service Provider'.")
        };
        await select.SelectOptionAsync(new[] { value });
    }

    /// <summary>
    /// Enter fee value
    /// </summary>
    public async Task EnterFeeValue(string value)
    {
        var input = _page.Locator("label:has-text('Fee Value')").Locator("..").Locator("input");
        await input.FillAsync(value);
    }

    /// <summary>
    /// Click Add Fee button in modal
    /// </summary>
    public async Task ClickAddFeeButtonInModal()
    {
        var button = _page.Locator(".bg-white.rounded-lg.p-6").Locator("button:has-text('Add Fee')");
        await button.ClickAsync();
        await Task.Delay(500);
    }

    /// <summary>
    /// Verify transaction fee is added successfully
    /// </summary>
    public async Task VerifyTransactionFeeAddedSuccessfully()
    {
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        var modalCount = await _page.Locator("h3:has-text('Add Transaction Fee')").CountAsync();
        modalCount.ShouldBe(0, "Add Transaction Fee modal should be closed after successful addition");
    }

    /// <summary>
    /// Verify Add Transaction Fee modal is closed
    /// </summary>
    public async Task VerifyAddTransactionFeeModalIsClosed()
    {
        var modalCount = await _page.Locator("h3:has-text('Add Transaction Fee')").CountAsync();
        modalCount.ShouldBe(0, "Add Transaction Fee modal should be closed");
    }

    #endregion
}
