using Microsoft.Playwright;
using Shouldly;

namespace EstateManagementUI.IntegrationTests.Common;

/// <summary>
/// Helper class for interacting with the Estate Management page using Playwright
/// </summary>
public class EstateManagementPageHelper
{
    private readonly IPage _page;
    private readonly string _baseUrl;

    public EstateManagementPageHelper(IPage page, string baseUrl)
    {
        _page = page;
        _baseUrl = baseUrl;
    }

    #region Navigation

    /// <summary>
    /// Navigate to the Estate Management page
    /// </summary>
    public async Task NavigateToEstateManagement()
    {
        await _page.GotoAsync($"{_baseUrl}/estate");
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

    #endregion

    #region Menu Visibility Verification

    /// <summary>
    /// Verify that Estate Management menu is not visible (Administrator role)
    /// </summary>
    public async Task VerifyEstateManagementMenuIsNotVisible()
    {
        // Wait a moment for the page to fully load
        await Task.Delay(1000);
        
        var menuLinkCount = await _page.Locator("#estateDetailsLink").CountAsync();
        menuLinkCount.ShouldBe(0, "Estate Management menu should not be visible for Administrator role");
    }

    /// <summary>
    /// Verify that Estate Management menu is visible
    /// </summary>
    public async Task VerifyEstateManagementMenuIsVisible()
    {
        await _page.Locator("#estateDetailsLink").WaitForAsync();
        var menuLinkCount = await _page.Locator("#estateDetailsLink").CountAsync();
        menuLinkCount.ShouldBeGreaterThan(0, "Estate Management menu should be visible");
    }

    #endregion

    #region Page Verification

    /// <summary>
    /// Verify the Estate Management page is displayed
    /// </summary>
    public async Task VerifyEstateManagementPageIsDisplayed()
    {
        await _page.Locator("h1:has-text('Estate Management')").WaitForAsync();
        var heading = await _page.Locator("h1:has-text('Estate Management')").TextContentAsync();
        heading.ShouldNotBeNull();
        heading.ShouldContain("Estate Management");
    }

    /// <summary>
    /// Verify the page title
    /// </summary>
    public async Task VerifyPageTitle(string expectedTitle)
    {
        var title = await _page.TitleAsync();
        title.ShouldBe(expectedTitle);
    }

    #endregion

    #region Estate Details Verification

    /// <summary>
    /// Verify estate name is displayed correctly
    /// </summary>
    public async Task VerifyEstateName(string expectedName)
    {
        var estateNameInput = _page.Locator("#Estate_Name");
        await estateNameInput.WaitForAsync();
        var actualName = await estateNameInput.InputValueAsync();
        actualName.ShouldBe(expectedName);
    }

    /// <summary>
    /// Verify estate reference is displayed correctly
    /// </summary>
    public async Task VerifyEstateReference(string expectedReference)
    {
        var estateRefInput = _page.Locator("#Estate_Reference");
        await estateRefInput.WaitForAsync();
        var actualReference = await estateRefInput.InputValueAsync();
        actualReference.ShouldBe(expectedReference);
    }

    #endregion

    #region Statistics Verification

    /// <summary>
    /// Verify Total Merchants count
    /// </summary>
    public async Task VerifyTotalMerchantsCount(int expectedCount)
    {
        var merchantsCard = _page.Locator(".bg-gray-50.rounded-lg.p-6").Filter(new LocatorFilterOptions 
        { 
            HasText = "Total Merchants" 
        });
        
        await merchantsCard.WaitForAsync();
        
        var countText = await merchantsCard.Locator(".text-3xl.font-bold").TextContentAsync();
        countText.ShouldNotBeNull();
        int.Parse(countText.Trim()).ShouldBe(expectedCount);
    }

    /// <summary>
    /// Verify Total Operators count
    /// </summary>
    public async Task VerifyTotalOperatorsCount(int expectedCount)
    {
        var operatorsCard = _page.Locator(".bg-gray-50.rounded-lg.p-6").Filter(new LocatorFilterOptions 
        { 
            HasText = "Total Operators" 
        });
        
        await operatorsCard.WaitForAsync();
        
        var countText = await operatorsCard.Locator(".text-3xl.font-bold").TextContentAsync();
        countText.ShouldNotBeNull();
        int.Parse(countText.Trim()).ShouldBe(expectedCount);
    }

    /// <summary>
    /// Verify Total Contracts count
    /// </summary>
    public async Task VerifyTotalContractsCount(int expectedCount)
    {
        var contractsCard = _page.Locator(".bg-gray-50.rounded-lg.p-6").Filter(new LocatorFilterOptions 
        { 
            HasText = "Total Contracts" 
        });
        
        await contractsCard.WaitForAsync();
        
        var countText = await contractsCard.Locator(".text-3xl.font-bold").TextContentAsync();
        countText.ShouldNotBeNull();
        int.Parse(countText.Trim()).ShouldBe(expectedCount);
    }

    /// <summary>
    /// Verify Total Users count
    /// </summary>
    public async Task VerifyTotalUsersCount(int expectedCount)
    {
        var usersCard = _page.Locator(".bg-gray-50.rounded-lg.p-6").Filter(new LocatorFilterOptions 
        { 
            HasText = "Total Users" 
        });
        
        await usersCard.WaitForAsync();
        
        var countText = await usersCard.Locator(".text-3xl.font-bold").TextContentAsync();
        countText.ShouldNotBeNull();
        int.Parse(countText.Trim()).ShouldBe(expectedCount);
    }

    #endregion

    #region Recent Merchants Section

    /// <summary>
    /// Verify Recent Merchants section is displayed
    /// </summary>
    public async Task VerifyRecentMerchantsSection()
    {
        await _page.Locator("h2:has-text('Recent Merchants')").WaitForAsync();
    }

    /// <summary>
    /// Verify at least a certain number of merchants are shown
    /// </summary>
    public async Task VerifyRecentMerchantsCount(int minCount)
    {
        var merchantsSection = _page.Locator(".bg-gray-50.rounded-lg.p-6").Filter(new LocatorFilterOptions 
        { 
            HasText = "Recent Merchants" 
        });
        
        await merchantsSection.WaitForAsync();
        
        var merchantItems = merchantsSection.Locator(".flex.items-center.justify-between.p-3.bg-white.rounded-lg");
        var count = await merchantItems.CountAsync();
        count.ShouldBeGreaterThanOrEqualTo(minCount, $"At least {minCount} merchant(s) should be displayed");
    }

    /// <summary>
    /// Verify a specific merchant is visible by name and reference
    /// </summary>
    public async Task VerifyMerchantIsVisible(string merchantName, string merchantReference)
    {
        var merchantsSection = _page.Locator(".bg-gray-50.rounded-lg.p-6").Filter(new LocatorFilterOptions 
        { 
            HasText = "Recent Merchants" 
        });
        
        await merchantsSection.WaitForAsync();
        
        // Find merchant by name
        var merchantNameLocator = merchantsSection.Locator($"p.font-medium:has-text('{merchantName}')");
        await merchantNameLocator.WaitForAsync();
        
        // Verify reference is present in the same container
        var merchantItem = merchantNameLocator.Locator("../..");
        var referenceText = await merchantItem.Locator($"p.text-sm:has-text('{merchantReference}')").TextContentAsync();
        referenceText.ShouldNotBeNull();
        referenceText.ShouldContain(merchantReference);
    }

    #endregion

    #region Contracts Section

    /// <summary>
    /// Verify Contracts section is displayed
    /// </summary>
    public async Task VerifyContractsSection()
    {
        await _page.Locator("h2:has-text('Contracts')").WaitForAsync();
    }

    /// <summary>
    /// Verify at least a certain number of contracts are shown
    /// </summary>
    public async Task VerifyContractsCount(int minCount)
    {
        var contractsSection = _page.Locator(".bg-gray-50.rounded-lg.p-6").Filter(new LocatorFilterOptions 
        { 
            HasText = "Contracts" 
        });
        
        await contractsSection.WaitForAsync();
        
        var contractItems = contractsSection.Locator(".flex.items-center.justify-between.p-3.bg-white.rounded-lg");
        var count = await contractItems.CountAsync();
        count.ShouldBeGreaterThanOrEqualTo(minCount, $"At least {minCount} contract(s) should be displayed");
    }

    /// <summary>
    /// Verify a specific contract is visible by description and operator
    /// </summary>
    public async Task VerifyContractIsVisible(string contractDescription, string operatorName)
    {
        var contractsSection = _page.Locator(".bg-gray-50.rounded-lg.p-6").Filter(new LocatorFilterOptions 
        { 
            HasText = "Contracts" 
        });
        
        await contractsSection.WaitForAsync();
        
        // Find contract by description
        var contractDescLocator = contractsSection.Locator($"p.font-medium:has-text('{contractDescription}')");
        await contractDescLocator.WaitForAsync();
        
        // Verify operator name is present in the same container
        var contractItem = contractDescLocator.Locator("../..");
        var operatorText = await contractItem.Locator($"p.text-sm:has-text('{operatorName}')").TextContentAsync();
        operatorText.ShouldNotBeNull();
        operatorText.ShouldContain(operatorName);
    }

    #endregion

    #region Tab Navigation

    /// <summary>
    /// Click on a specific tab
    /// </summary>
    public async Task ClickTab(string tabName)
    {
        var tabButton = _page.Locator($"button:has-text('{tabName.Substring(0, 1).ToUpper()}{tabName.Substring(1)}')");
        await tabButton.WaitForAsync();
        await tabButton.ClickAsync();
        
        // Wait for tab content to load
        await Task.Delay(500);
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    /// <summary>
    /// Verify operators tab content is displayed
    /// </summary>
    public async Task VerifyOperatorsTabContent()
    {
        await _page.Locator("h3:has-text('Assigned Operators')").WaitForAsync();
    }

    #endregion

    #region Operators Tab

    /// <summary>
    /// Verify Assigned Operators section is visible
    /// </summary>
    public async Task VerifyAssignedOperatorsSection()
    {
        await _page.Locator("h3:has-text('Assigned Operators')").WaitForAsync();
    }

    /// <summary>
    /// Verify the number of assigned operators
    /// </summary>
    public async Task VerifyAssignedOperatorsCount(int minCount)
    {
        await _page.Locator("h3:has-text('Assigned Operators')").WaitForAsync();
        
        var operatorItems = _page.Locator(".flex.items-center.justify-between.p-3.bg-gray-50.rounded-lg");
        var count = await operatorItems.CountAsync();
        count.ShouldBeGreaterThanOrEqualTo(minCount, $"At least {minCount} operator(s) should be assigned");
    }

    /// <summary>
    /// Verify a specific operator is listed
    /// </summary>
    public async Task VerifyOperatorIsListed(string operatorName)
    {
        var operatorLocator = _page.Locator($".flex.items-center.justify-between.p-3.bg-gray-50.rounded-lg span.font-medium:has-text('{operatorName}')");
        await operatorLocator.WaitForAsync();
        
        var text = await operatorLocator.TextContentAsync();
        text.ShouldNotBeNull();
        text.ShouldContain(operatorName);
    }

    /// <summary>
    /// Verify Add Operator button is visible
    /// </summary>
    public async Task VerifyAddOperatorButtonIsVisible()
    {
        await _page.Locator("#addOperatorButton").WaitForAsync();
        var isVisible = await _page.Locator("#addOperatorButton").IsVisibleAsync();
        isVisible.ShouldBeTrue("Add Operator button should be visible for Estate role");
    }

    /// <summary>
    /// Verify Add Operator button is not visible
    /// </summary>
    public async Task VerifyAddOperatorButtonIsNotVisible()
    {
        // Wait a moment for the page to fully load
        await Task.Delay(1000);
        
        var buttonCount = await _page.Locator("#addOperatorButton").CountAsync();
        buttonCount.ShouldBe(0, "Add Operator button should not be visible for Viewer role");
    }

    #endregion

    #region Add/Remove Operator Actions

    /// <summary>
    /// Remove an operator from the estate
    /// </summary>
    public async Task RemoveOperator(string operatorName)
    {
        // Find the operator item by name
        var operatorItem = _page.Locator(".flex.items-center.justify-between.p-3.bg-gray-50.rounded-lg")
            .Filter(new LocatorFilterOptions { HasText = operatorName });
        
        await operatorItem.WaitForAsync();
        
        // Click the Remove button
        var removeButton = operatorItem.Locator("button:has-text('Remove')");
        await removeButton.ClickAsync();
        
        // Wait for the action to complete
        await Task.Delay(500);
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    /// <summary>
    /// Click the Add Operator button
    /// </summary>
    public async Task ClickAddOperatorButton()
    {
        await _page.Locator("#addOperatorButton").ClickAsync();
        
        // Wait for the form to appear
        await Task.Delay(500);
    }

    /// <summary>
    /// Select an operator from the dropdown
    /// </summary>
    public async Task SelectOperatorFromDropdown(string operatorName)
    {
        var dropdown = _page.Locator("select.input.flex-1");
        await dropdown.WaitForAsync();
        await dropdown.SelectOptionAsync(new[] { operatorName });
        
        // Wait for selection to register
        await Task.Delay(300);
    }

    /// <summary>
    /// Click the Add button in the operator form
    /// </summary>
    public async Task ClickAddButton()
    {
        var addButton = _page.Locator("button.btn.btn-primary:has-text('Add')");
        await addButton.ClickAsync();
        
        // Wait for the action to complete
        await Task.Delay(500);
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    /// <summary>
    /// Verify success message is displayed
    /// </summary>
    public async Task VerifySuccessMessageIsDisplayed()
    {
        var successMessage = _page.Locator(".bg-green-50.border.border-green-200.text-green-800");
        await successMessage.WaitForAsync();
        
        var isVisible = await successMessage.IsVisibleAsync();
        isVisible.ShouldBeTrue("Success message should be displayed");
    }

    /// <summary>
    /// Verify success message contains expected text
    /// </summary>
    public async Task VerifySuccessMessageContains(string expectedMessage)
    {
        var successMessage = _page.Locator(".bg-green-50.border.border-green-200.text-green-800");
        await successMessage.WaitForAsync();
        
        var messageText = await successMessage.TextContentAsync();
        messageText.ShouldNotBeNull();
        messageText.ShouldContain(expectedMessage);
    }

    /// <summary>
    /// Verify an operator is NOT listed (after removal)
    /// </summary>
    public async Task VerifyOperatorIsNotListed(string operatorName)
    {
        // Wait a moment for removal to complete
        await Task.Delay(500);
        
        var operatorCount = await _page.Locator($".flex.items-center.justify-between.p-3.bg-gray-50.rounded-lg span.font-medium:has-text('{operatorName}')").CountAsync();
        operatorCount.ShouldBe(0, $"Operator '{operatorName}' should not be listed after removal");
    }

    /// <summary>
    /// Verify operator selection form is displayed
    /// </summary>
    public async Task VerifyOperatorSelectionFormIsDisplayed()
    {
        await _page.Locator("select.input.flex-1").WaitForAsync();
        var isVisible = await _page.Locator("select.input.flex-1").IsVisibleAsync();
        isVisible.ShouldBeTrue("Operator selection form should be displayed");
    }

    #endregion
}
