using Microsoft.Playwright;
using Shouldly;

namespace EstateManagementUI.IntegrationTests.Common;

/// <summary>
/// Helper class for interacting with the Operator Management pages using Playwright
/// </summary>
public class OperatorManagementPageHelper
{
    private readonly IPage _page;
    private readonly string _baseUrl;

    public OperatorManagementPageHelper(IPage page, string baseUrl)
    {
        _page = page;
        _baseUrl = baseUrl;
    }

    #region Navigation

    /// <summary>
    /// Navigate to the Operator Management page (index)
    /// </summary>
    public async Task NavigateToOperatorManagement()
    {
        await _page.GotoAsync($"{_baseUrl}/operators");
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
    /// Navigate directly to Edit Operator page for a specific operator
    /// </summary>
    public async Task NavigateToEditOperatorPage(string operatorName)
    {
        // Get the operator ID based on the name from hardcoded test data
        var operatorId = operatorName.Equals("Safaricom", StringComparison.OrdinalIgnoreCase)
            ? "33333333-3333-3333-3333-333333333333"
            : "33333333-3333-3333-3333-333333333334"; // Voucher

        await _page.GotoAsync($"{_baseUrl}/operators/{operatorId}/edit");
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    #endregion

    #region Menu Visibility Verification

    /// <summary>
    /// Verify that Operator Management menu is not visible (Administrator role)
    /// </summary>
    public async Task VerifyOperatorManagementMenuIsNotVisible()
    {
        // Wait a moment for the page to fully load
        await Task.Delay(1000);
        
        var menuLinkCount = await _page.Locator("text=Operator Management").CountAsync();
        menuLinkCount.ShouldBe(0, "Operator Management menu should not be visible for Administrator role");
    }

    /// <summary>
    /// Verify that Operator Management menu is visible
    /// </summary>
    public async Task VerifyOperatorManagementMenuIsVisible()
    {
        await _page.Locator("text=Operator Management").WaitForAsync();
        var menuLinkCount = await _page.Locator("text=Operator Management").CountAsync();
        menuLinkCount.ShouldBeGreaterThan(0, "Operator Management menu should be visible");
    }

    #endregion

    #region Page Verification

    /// <summary>
    /// Verify the Operator Management page is displayed
    /// </summary>
    public async Task VerifyOperatorManagementPageIsDisplayed()
    {
        await _page.Locator("h1:has-text('Operator Management')").WaitForAsync();
        var heading = await _page.Locator("h1:has-text('Operator Management')").TextContentAsync();
        heading.ShouldNotBeNull();
        heading.ShouldContain("Operator Management");
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
    /// Verify the View Operator page is displayed
    /// </summary>
    public async Task VerifyViewOperatorPageIsDisplayed()
    {
        await _page.Locator("h1:has-text('View Operator')").WaitForAsync();
        var heading = await _page.Locator("h1").TextContentAsync();
        heading.ShouldNotBeNull();
        heading.ShouldContain("View Operator");
    }

    /// <summary>
    /// Verify the Create New Operator page is displayed
    /// </summary>
    public async Task VerifyCreateNewOperatorPageIsDisplayed()
    {
        await _page.Locator("h1:has-text('Create New Operator')").WaitForAsync();
        var heading = await _page.Locator("h1:has-text('Create New Operator')").TextContentAsync();
        heading.ShouldNotBeNull();
        heading.ShouldContain("Create New Operator");
    }

    /// <summary>
    /// Verify the Edit Operator page is displayed
    /// </summary>
    public async Task VerifyEditOperatorPageIsDisplayed()
    {
        await _page.Locator("h1:has-text('Edit Operator')").WaitForAsync();
        var heading = await _page.Locator("h1").TextContentAsync();
        heading.ShouldNotBeNull();
        heading.ShouldContain("Edit Operator");
    }

    /// <summary>
    /// Verify an access denied message is displayed
    /// </summary>
    public async Task VerifyAccessDeniedMessageIsDisplayed()
    {
        await _page.Locator("text=Access Denied").WaitForAsync();
        var heading = await _page.Locator("h3:has-text('Access Denied')").TextContentAsync();
        heading.ShouldNotBeNull();
        heading.ShouldContain("Access Denied");
    }

    /// <summary>
    /// Verify the access denied message indicates no permission to edit operators
    /// </summary>
    public async Task VerifyAccessDeniedMessageForEditOperators()
    {
        var message = await _page.Locator("text=You don't have permission to edit operators").TextContentAsync();
        message.ShouldNotBeNull();
        message.ShouldContain("edit operators");
    }

    #endregion

    #region Operator List Verification

    /// <summary>
    /// Verify the number of operators in the list
    /// </summary>
    public async Task VerifyOperatorListCount(int expectedCount)
    {
        await _page.Locator("table tbody tr").First.WaitForAsync();
        var operatorRows = await _page.Locator("table tbody tr").CountAsync();
        operatorRows.ShouldBe(expectedCount, $"Expected {expectedCount} operators in the list");
    }

    /// <summary>
    /// Verify a specific operator is listed
    /// </summary>
    public async Task VerifyOperatorIsListed(string operatorName)
    {
        var operatorLocator = _page.Locator($"table tbody tr:has-text('{operatorName}')");
        await operatorLocator.WaitForAsync();
        var count = await operatorLocator.CountAsync();
        count.ShouldBeGreaterThan(0, $"Operator '{operatorName}' should be listed");
    }

    /// <summary>
    /// Verify operator custom merchant number requirement status
    /// </summary>
    public async Task VerifyOperatorCustomMerchantNumberRequirement(string operatorName, string expectedStatus)
    {
        var operatorRow = _page.Locator($"table tbody tr:has-text('{operatorName}')");
        await operatorRow.WaitForAsync();
        
        var statusText = await operatorRow.Locator("td:nth-child(2)").TextContentAsync();
        statusText.ShouldNotBeNull();
        statusText.ShouldContain(expectedStatus);
    }

    /// <summary>
    /// Verify operator custom terminal number requirement status
    /// </summary>
    public async Task VerifyOperatorCustomTerminalNumberRequirement(string operatorName, string expectedStatus)
    {
        var operatorRow = _page.Locator($"table tbody tr:has-text('{operatorName}')");
        await operatorRow.WaitForAsync();
        
        var statusText = await operatorRow.Locator("td:nth-child(3)").TextContentAsync();
        statusText.ShouldNotBeNull();
        statusText.ShouldContain(expectedStatus);
    }

    #endregion

    #region Button Visibility Verification

    /// <summary>
    /// Verify Add New Operator button is visible
    /// </summary>
    public async Task VerifyAddNewOperatorButtonIsVisible()
    {
        var button = _page.Locator("#newOperatorButton");
        await button.WaitForAsync();
        var isVisible = await button.IsVisibleAsync();
        isVisible.ShouldBeTrue("Add New Operator button should be visible");
    }

    /// <summary>
    /// Verify Add New Operator button is not visible
    /// </summary>
    public async Task VerifyAddNewOperatorButtonIsNotVisible()
    {
        await Task.Delay(1000); // Wait for page to fully render
        var buttonCount = await _page.Locator("#newOperatorButton").CountAsync();
        buttonCount.ShouldBe(0, "Add New Operator button should not be visible");
    }

    /// <summary>
    /// Verify Edit Operator button is visible on view page
    /// </summary>
    public async Task VerifyEditOperatorButtonIsVisible()
    {
        var button = _page.Locator("button:has-text('Edit Operator')");
        await button.WaitForAsync();
        var isVisible = await button.IsVisibleAsync();
        isVisible.ShouldBeTrue("Edit Operator button should be visible");
    }

    /// <summary>
    /// Verify Edit button is visible for a specific operator in the list
    /// </summary>
    public async Task VerifyEditButtonIsVisibleForOperator(string operatorName)
    {
        var operatorRow = _page.Locator($"table tbody tr:has-text('{operatorName}')");
        await operatorRow.WaitForAsync();
        
        var editButton = operatorRow.Locator("button[title='Edit']");
        var isVisible = await editButton.IsVisibleAsync();
        isVisible.ShouldBeTrue($"Edit button should be visible for operator '{operatorName}'");
    }

    /// <summary>
    /// Verify Edit button is not visible for a specific operator in the list
    /// </summary>
    public async Task VerifyEditButtonIsNotVisibleForOperator(string operatorName)
    {
        var operatorRow = _page.Locator($"table tbody tr:has-text('{operatorName}')");
        await operatorRow.WaitForAsync();
        
        var editButtonCount = await operatorRow.Locator("button[title='Edit']").CountAsync();
        editButtonCount.ShouldBe(0, $"Edit button should not be visible for operator '{operatorName}'");
    }

    #endregion

    #region Operator Detail Verification

    /// <summary>
    /// Verify the operator name on the view page
    /// </summary>
    public async Task VerifyOperatorName(string expectedName)
    {
        var nameLocator = _page.Locator("h1:has-text('View Operator')");
        await nameLocator.WaitForAsync();
        var heading = await nameLocator.TextContentAsync();
        heading.ShouldNotBeNull();
        heading.ShouldContain(expectedName);
    }

    /// <summary>
    /// Verify operator custom merchant number requirement on detail page
    /// </summary>
    public async Task VerifyOperatorCustomMerchantNumberRequirementDetail(string expectedStatus)
    {
        var labelLocator = _page.Locator("label:has-text('Require Custom Merchant Number')");
        await labelLocator.WaitForAsync();
        
        // Look for the status badge after the label
        var statusBadge = _page.Locator($"span:has-text('{expectedStatus}')").First;
        await statusBadge.WaitForAsync();
        var isVisible = await statusBadge.IsVisibleAsync();
        isVisible.ShouldBeTrue($"Custom merchant number requirement should show '{expectedStatus}'");
    }

    /// <summary>
    /// Verify operator custom terminal number requirement on detail page
    /// </summary>
    public async Task VerifyOperatorCustomTerminalNumberRequirementDetail(string expectedStatus)
    {
        var labelLocator = _page.Locator("label:has-text('Require Custom Terminal Number')");
        await labelLocator.WaitForAsync();
        
        // Look for the status badge after the label
        var statusBadge = _page.Locator($"span:has-text('{expectedStatus}')").Nth(1); // Get second occurrence
        await statusBadge.WaitForAsync();
        var isVisible = await statusBadge.IsVisibleAsync();
        isVisible.ShouldBeTrue($"Custom terminal number requirement should show '{expectedStatus}'");
    }

    #endregion

    #region Form Field Verification

    /// <summary>
    /// Verify operator name field is visible
    /// </summary>
    public async Task VerifyOperatorNameFieldIsVisible()
    {
        var field = _page.Locator("input[placeholder='Enter operator name']");
        await field.WaitForAsync();
        var isVisible = await field.IsVisibleAsync();
        isVisible.ShouldBeTrue("Operator name field should be visible");
    }

    /// <summary>
    /// Verify operator name field contains expected value
    /// </summary>
    public async Task VerifyOperatorNameFieldContains(string expectedValue)
    {
        var field = _page.Locator("input[placeholder='Enter operator name']");
        await field.WaitForAsync();
        var value = await field.InputValueAsync();
        value.ShouldBe(expectedValue, $"Operator name field should contain '{expectedValue}'");
    }

    /// <summary>
    /// Verify custom merchant number checkbox is visible
    /// </summary>
    public async Task VerifyCustomMerchantNumberCheckboxIsVisible()
    {
        var checkbox = _page.Locator("input[type='checkbox']").First;
        await checkbox.WaitForAsync();
        var isVisible = await checkbox.IsVisibleAsync();
        isVisible.ShouldBeTrue("Custom merchant number checkbox should be visible");
    }

    /// <summary>
    /// Verify custom terminal number checkbox is visible
    /// </summary>
    public async Task VerifyCustomTerminalNumberCheckboxIsVisible()
    {
        var checkbox = _page.Locator("input[type='checkbox']").Nth(1);
        await checkbox.WaitForAsync();
        var isVisible = await checkbox.IsVisibleAsync();
        isVisible.ShouldBeTrue("Custom terminal number checkbox should be visible");
    }

    #endregion

    #region Navigation Actions

    /// <summary>
    /// Click on a specific operator in the list
    /// </summary>
    public async Task ClickOnOperator(string operatorName)
    {
        var operatorRow = _page.Locator($"table tbody tr:has-text('{operatorName}')");
        await operatorRow.WaitForAsync();
        await operatorRow.ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    /// <summary>
    /// Click the Add New Operator button
    /// </summary>
    public async Task ClickAddNewOperatorButton()
    {
        var button = _page.Locator("#newOperatorButton");
        await button.WaitForAsync();
        await button.ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    /// <summary>
    /// Click edit button for a specific operator
    /// </summary>
    public async Task ClickEditForOperator(string operatorName)
    {
        var operatorRow = _page.Locator($"table tbody tr:has-text('{operatorName}')");
        await operatorRow.WaitForAsync();
        
        var editButton = operatorRow.Locator("button[title='Edit']");
        await editButton.WaitForAsync();
        await editButton.ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    /// <summary>
    /// Click the Edit Operator button on the view page
    /// </summary>
    public async Task ClickEditOperatorButton()
    {
        var button = _page.Locator("button:has-text('Edit Operator')");
        await button.WaitForAsync();
        await button.ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    #endregion

    #region Edit Operations

    /// <summary>
    /// Update the operator name
    /// </summary>
    public async Task UpdateOperatorName(string newName)
    {
        var field = _page.Locator("input[placeholder='Enter operator name']");
        await field.WaitForAsync();
        await field.ClearAsync();
        await field.FillAsync(newName);
    }

    /// <summary>
    /// Update the custom merchant number requirement checkbox
    /// </summary>
    public async Task UpdateCustomMerchantNumberRequirement(bool isChecked)
    {
        var checkbox = _page.Locator("input[type='checkbox']").First;
        await checkbox.WaitForAsync();
        
        var currentState = await checkbox.IsCheckedAsync();
        if (currentState != isChecked)
        {
            await checkbox.ClickAsync();
        }
    }

    /// <summary>
    /// Update the custom terminal number requirement checkbox
    /// </summary>
    public async Task UpdateCustomTerminalNumberRequirement(bool isChecked)
    {
        var checkbox = _page.Locator("input[type='checkbox']").Nth(1);
        await checkbox.WaitForAsync();
        
        var currentState = await checkbox.IsCheckedAsync();
        if (currentState != isChecked)
        {
            await checkbox.ClickAsync();
        }
    }

    /// <summary>
    /// Click the Save Changes button (Update Operator button)
    /// </summary>
    public async Task ClickSaveChangesButton()
    {
        var button = _page.Locator("button[type='submit']:has-text('Update Operator')");
        await button.WaitForAsync();
        await button.ClickAsync();
        await Task.Delay(2000); // Wait for the operation to complete
    }

    /// <summary>
    /// Click the Create Operator button
    /// </summary>
    public async Task ClickCreateOperatorButton()
    {
        var button = _page.Locator("#createOperatorButton");
        await button.WaitForAsync();
        await button.ClickAsync();
        await Task.Delay(2000); // Wait for the operation to complete
    }

    /// <summary>
    /// Enter operator name
    /// </summary>
    public async Task EnterOperatorName(string operatorName)
    {
        var field = _page.Locator("input[placeholder='Enter operator name']");
        await field.WaitForAsync();
        await field.FillAsync(operatorName);
    }

    #endregion

    #region Success/Error Message Verification

    /// <summary>
    /// Verify a success message is displayed
    /// </summary>
    public async Task VerifySuccessMessageIsDisplayed()
    {
        var successMessage = _page.Locator(".bg-green-50");
        await successMessage.WaitForAsync();
        var isVisible = await successMessage.IsVisibleAsync();
        isVisible.ShouldBeTrue("Success message should be displayed");
    }

    /// <summary>
    /// Verify success message contains expected text
    /// </summary>
    public async Task VerifySuccessMessageContains(string expectedMessage)
    {
        var successMessage = _page.Locator(".bg-green-50");
        await successMessage.WaitForAsync();
        var messageText = await successMessage.TextContentAsync();
        messageText.ShouldNotBeNull();
        messageText.ShouldContain(expectedMessage);
    }

    /// <summary>
    /// Verify user was redirected to operator list (after create operation)
    /// </summary>
    public async Task VerifyRedirectedToOperatorList()
    {
        await Task.Delay(1000);
        var currentUrl = _page.Url;
        currentUrl.ShouldEndWith("/operators");
    }

    /// <summary>
    /// Verify a validation error is displayed
    /// </summary>
    public async Task VerifyValidationErrorIsDisplayed()
    {
        var errorMessage = _page.Locator(".text-red-600");
        await errorMessage.WaitForAsync();
        var isVisible = await errorMessage.IsVisibleAsync();
        isVisible.ShouldBeTrue("Validation error should be displayed");
    }

    /// <summary>
    /// Verify validation error is displayed for operator name
    /// </summary>
    public async Task VerifyValidationErrorForOperatorName()
    {
        var errorMessage = _page.Locator(".text-red-600:has-text('required')");
        await errorMessage.WaitForAsync();
        var isVisible = await errorMessage.IsVisibleAsync();
        isVisible.ShouldBeTrue("Validation error for operator name should be displayed");
    }

    #endregion
}
