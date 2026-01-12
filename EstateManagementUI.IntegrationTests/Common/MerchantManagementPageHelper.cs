using Microsoft.Playwright;
using Shouldly;

namespace EstateManagementUI.IntegrationTests.Common;

/// <summary>
/// Helper class for interacting with the Merchant Management pages using Playwright
/// </summary>
public class MerchantManagementPageHelper
{
    private readonly IPage _page;
    private readonly string _baseUrl;

    public MerchantManagementPageHelper(IPage page, string baseUrl)
    {
        _page = page;
        _baseUrl = baseUrl;
    }

    #region Navigation

    /// <summary>
    /// Navigate to the Merchant Management page (index)
    /// </summary>
    public async Task NavigateToMerchantManagement()
    {
        await _page.GotoAsync($"{_baseUrl}/merchants");
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
    /// Verify that Merchant Management menu is not visible (Administrator role)
    /// </summary>
    public async Task VerifyMerchantManagementMenuIsNotVisible()
    {
        // Wait a moment for the page to fully load
        await Task.Delay(1000);
        
        var menuLinkCount = await _page.Locator("text=Merchant Management").CountAsync();
        menuLinkCount.ShouldBe(0, "Merchant Management menu should not be visible for Administrator role");
    }

    /// <summary>
    /// Verify that Merchant Management menu is visible
    /// </summary>
    public async Task VerifyMerchantManagementMenuIsVisible()
    {
        await _page.Locator("text=Merchant Management").WaitForAsync();
        var menuLinkCount = await _page.Locator("text=Merchant Management").CountAsync();
        menuLinkCount.ShouldBeGreaterThan(0, "Merchant Management menu should be visible");
    }

    #endregion

    #region Page Verification

    /// <summary>
    /// Verify the Merchant Management page is displayed
    /// </summary>
    public async Task VerifyMerchantManagementPageIsDisplayed()
    {
        await _page.Locator("h1:has-text('Merchant Management')").WaitForAsync();
        var heading = await _page.Locator("h1:has-text('Merchant Management')").TextContentAsync();
        heading.ShouldNotBeNull();
        heading.ShouldContain("Merchant Management");
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
    /// Verify the View Merchant page is displayed
    /// </summary>
    public async Task VerifyViewMerchantPageIsDisplayed()
    {
        await _page.Locator("h1:has-text('View Merchant')").WaitForAsync();
        var heading = await _page.Locator("h1").TextContentAsync();
        heading.ShouldNotBeNull();
        heading.ShouldContain("View Merchant");
    }

    /// <summary>
    /// Verify the Create New Merchant page is displayed
    /// </summary>
    public async Task VerifyCreateNewMerchantPageIsDisplayed()
    {
        await _page.Locator("h1:has-text('Create New Merchant')").WaitForAsync();
        var heading = await _page.Locator("h1:has-text('Create New Merchant')").TextContentAsync();
        heading.ShouldNotBeNull();
        heading.ShouldContain("Create New Merchant");
    }

    /// <summary>
    /// Verify the Edit Merchant page is displayed
    /// </summary>
    public async Task VerifyEditMerchantPageIsDisplayed()
    {
        await _page.Locator("h1:has-text('Edit Merchant')").WaitForAsync();
        var heading = await _page.Locator("h1").TextContentAsync();
        heading.ShouldNotBeNull();
        heading.ShouldContain("Edit Merchant");
    }

    /// <summary>
    /// Verify the Make Deposit page is displayed
    /// </summary>
    public async Task VerifyMakeDepositPageIsDisplayed()
    {
        await _page.Locator("h1:has-text('Make Merchant Deposit')").WaitForAsync();
        var heading = await _page.Locator("h1:has-text('Make Merchant Deposit')").TextContentAsync();
        heading.ShouldNotBeNull();
        heading.ShouldContain("Make Merchant Deposit");
    }

    #endregion

    #region Merchant List Verification

    /// <summary>
    /// Verify the number of merchants in the list
    /// </summary>
    public async Task VerifyMerchantListCount(int expectedCount)
    {
        await _page.Locator("#merchantList tbody tr").First.WaitForAsync();
        var merchantRows = await _page.Locator("#merchantList tbody tr").CountAsync();
        merchantRows.ShouldBe(expectedCount, $"Expected {expectedCount} merchants in the list");
    }

    /// <summary>
    /// Verify a specific merchant is listed with name and reference
    /// </summary>
    public async Task VerifyMerchantIsListed(string merchantName, string merchantReference)
    {
        // Wait for the merchant list to load
        await _page.Locator("#merchantList").WaitForAsync();
        
        // Find the row containing the merchant name
        var merchantRow = _page.Locator($"#merchantList tbody tr:has-text('{merchantName}')");
        await merchantRow.WaitForAsync();
        
        var rowCount = await merchantRow.CountAsync();
        rowCount.ShouldBeGreaterThan(0, $"Merchant '{merchantName}' should be visible in the list");
        
        // Verify the reference is in the same row
        var referenceCell = merchantRow.Locator($"text={merchantReference}");
        var refCount = await referenceCell.CountAsync();
        refCount.ShouldBeGreaterThan(0, $"Merchant reference '{merchantReference}' should be visible for '{merchantName}'");
    }

    /// <summary>
    /// Verify merchant balance information
    /// </summary>
    public async Task VerifyMerchantBalance(string merchantName, string expectedBalance)
    {
        var merchantRow = _page.Locator($"#merchantList tbody tr:has-text('{merchantName}')");
        await merchantRow.WaitForAsync();
        
        var balanceCell = merchantRow.Locator($"text={expectedBalance}");
        var balanceCount = await balanceCell.CountAsync();
        balanceCount.ShouldBeGreaterThan(0, $"Merchant '{merchantName}' should show balance '{expectedBalance}'");
    }

    /// <summary>
    /// Verify merchant available balance information
    /// </summary>
    public async Task VerifyMerchantAvailableBalance(string merchantName, string expectedAvailableBalance)
    {
        var merchantRow = _page.Locator($"#merchantList tbody tr:has-text('{merchantName}')");
        await merchantRow.WaitForAsync();
        
        var availableBalanceCell = merchantRow.Locator($"text={expectedAvailableBalance}");
        var availableBalanceCount = await availableBalanceCell.CountAsync();
        availableBalanceCount.ShouldBeGreaterThan(0, $"Merchant '{merchantName}' should show available balance '{expectedAvailableBalance}'");
    }

    /// <summary>
    /// Verify merchant settlement schedule
    /// </summary>
    public async Task VerifyMerchantSettlementSchedule(string merchantName, string expectedSchedule)
    {
        var merchantRow = _page.Locator($"#merchantList tbody tr:has-text('{merchantName}')");
        await merchantRow.WaitForAsync();
        
        var scheduleCell = merchantRow.Locator($"text={expectedSchedule}");
        var scheduleCount = await scheduleCell.CountAsync();
        scheduleCount.ShouldBeGreaterThan(0, $"Merchant '{merchantName}' should show settlement schedule '{expectedSchedule}'");
    }

    #endregion

    #region Button Visibility

    /// <summary>
    /// Verify the Add New Merchant button is visible
    /// </summary>
    public async Task VerifyAddNewMerchantButtonIsVisible()
    {
        var buttonCount = await _page.Locator("#newMerchantButton").CountAsync();
        buttonCount.ShouldBeGreaterThan(0, "Add New Merchant button should be visible for Estate role");
    }

    /// <summary>
    /// Verify the Add New Merchant button is not visible
    /// </summary>
    public async Task VerifyAddNewMerchantButtonIsNotVisible()
    {
        await Task.Delay(500); // Wait a moment for page to render
        var buttonCount = await _page.Locator("#newMerchantButton").CountAsync();
        buttonCount.ShouldBe(0, "Add New Merchant button should not be visible for Viewer role");
    }

    /// <summary>
    /// Verify the Edit button is visible for a specific merchant
    /// </summary>
    public async Task VerifyEditButtonIsVisibleForMerchant(string merchantName)
    {
        var merchantRow = _page.Locator($"#merchantList tbody tr:has-text('{merchantName}')");
        await merchantRow.WaitForAsync();
        
        var editButton = merchantRow.Locator("#editMerchantLink");
        var editButtonCount = await editButton.CountAsync();
        editButtonCount.ShouldBeGreaterThan(0, $"Edit button should be visible for merchant '{merchantName}'");
    }

    /// <summary>
    /// Verify the Edit button is not visible for a specific merchant
    /// </summary>
    public async Task VerifyEditButtonIsNotVisibleForMerchant(string merchantName)
    {
        var merchantRow = _page.Locator($"#merchantList tbody tr:has-text('{merchantName}')");
        await merchantRow.WaitForAsync();
        
        await Task.Delay(500); // Wait a moment for page to render
        var editButton = merchantRow.Locator("#editMerchantLink");
        var editButtonCount = await editButton.CountAsync();
        editButtonCount.ShouldBe(0, $"Edit button should not be visible for merchant '{merchantName}' for Viewer role");
    }

    /// <summary>
    /// Verify the Make Deposit button is visible for a specific merchant
    /// </summary>
    public async Task VerifyMakeDepositButtonIsVisibleForMerchant(string merchantName)
    {
        var merchantRow = _page.Locator($"#merchantList tbody tr:has-text('{merchantName}')");
        await merchantRow.WaitForAsync();
        
        var depositButton = merchantRow.Locator("#makeDepositLink");
        var depositButtonCount = await depositButton.CountAsync();
        depositButtonCount.ShouldBeGreaterThan(0, $"Make Deposit button should be visible for merchant '{merchantName}'");
    }

    /// <summary>
    /// Verify the Make Deposit button is not visible for a specific merchant
    /// </summary>
    public async Task VerifyMakeDepositButtonIsNotVisibleForMerchant(string merchantName)
    {
        var merchantRow = _page.Locator($"#merchantList tbody tr:has-text('{merchantName}')");
        await merchantRow.WaitForAsync();
        
        await Task.Delay(500); // Wait a moment for page to render
        var depositButton = merchantRow.Locator("#makeDepositLink");
        var depositButtonCount = await depositButton.CountAsync();
        depositButtonCount.ShouldBe(0, $"Make Deposit button should not be visible for merchant '{merchantName}' for Viewer role");
    }

    #endregion

    #region Merchant Detail Verification

    /// <summary>
    /// Verify merchant name in the detail view
    /// </summary>
    public async Task VerifyMerchantName(string expectedName)
    {
        var nameElement = _page.Locator("p.font-medium.text-gray-900").Filter(new LocatorFilterOptions 
        { 
            HasText = expectedName 
        });
        await nameElement.WaitForAsync();
        var nameText = await nameElement.TextContentAsync();
        nameText.ShouldNotBeNull();
        nameText.ShouldBe(expectedName);
    }

    /// <summary>
    /// Verify merchant reference in the detail view
    /// </summary>
    public async Task VerifyMerchantReference(string expectedReference)
    {
        var referenceElement = _page.Locator("p.font-mono").Filter(new LocatorFilterOptions 
        { 
            HasText = expectedReference 
        });
        await referenceElement.WaitForAsync();
        var referenceText = await referenceElement.TextContentAsync();
        referenceText.ShouldNotBeNull();
        referenceText.ShouldBe(expectedReference);
    }

    /// <summary>
    /// Verify merchant balance in the detail view
    /// </summary>
    public async Task VerifyMerchantBalanceDetail(string expectedBalance)
    {
        // Wait for the balance to load
        await _page.Locator("text=Balance").WaitForAsync();
        
        var balanceElement = _page.Locator(".text-green-600").Filter(new LocatorFilterOptions 
        { 
            HasText = expectedBalance 
        });
        var balanceCount = await balanceElement.CountAsync();
        balanceCount.ShouldBeGreaterThan(0, $"Merchant should show balance '{expectedBalance}'");
    }

    /// <summary>
    /// Verify merchant available balance in the detail view
    /// </summary>
    public async Task VerifyMerchantAvailableBalanceDetail(string expectedAvailableBalance)
    {
        // Wait for the available balance to load
        await _page.Locator("text=Available Balance").WaitForAsync();
        
        var availableBalanceElement = _page.Locator(".text-blue-600").Filter(new LocatorFilterOptions 
        { 
            HasText = expectedAvailableBalance 
        });
        var availableBalanceCount = await availableBalanceElement.CountAsync();
        availableBalanceCount.ShouldBeGreaterThan(0, $"Merchant should show available balance '{expectedAvailableBalance}'");
    }

    /// <summary>
    /// Verify merchant settlement schedule in the detail view
    /// </summary>
    public async Task VerifyMerchantSettlementScheduleDetail(string expectedSchedule)
    {
        await _page.Locator("text=Settlement Schedule").WaitForAsync();
        
        var scheduleElement = _page.Locator($"text={expectedSchedule}");
        var scheduleCount = await scheduleElement.CountAsync();
        scheduleCount.ShouldBeGreaterThan(0, $"Merchant should show settlement schedule '{expectedSchedule}'");
    }

    #endregion

    #region Tab Navigation and Verification

    /// <summary>
    /// Click on a specific tab (address, contact, operators, contracts, devices, etc.)
    /// </summary>
    public async Task ClickTab(string tabName)
    {
        await _page.Locator($"button:has-text('{tabName}')").ClickAsync();
        await Task.Delay(500); // Wait for tab content to load
    }

    /// <summary>
    /// Verify the address tab content is displayed
    /// </summary>
    public async Task VerifyAddressTabContent()
    {
        await _page.Locator("text=Address Line 1").WaitForAsync();
        var addressHeading = await _page.Locator("text=Address Line 1").CountAsync();
        addressHeading.ShouldBeGreaterThan(0, "Address tab content should be displayed");
    }

    /// <summary>
    /// Verify the contact tab content is displayed
    /// </summary>
    public async Task VerifyContactTabContent()
    {
        await _page.Locator("text=Contact Name").WaitForAsync();
        var contactHeading = await _page.Locator("text=Contact Name").CountAsync();
        contactHeading.ShouldBeGreaterThan(0, "Contact tab content should be displayed");
    }

    /// <summary>
    /// Verify the operators tab content is displayed
    /// </summary>
    public async Task VerifyOperatorsTabContent()
    {
        await _page.Locator("text=Assigned Operators").WaitForAsync();
        var operatorsSection = await _page.Locator("text=Assigned Operators").CountAsync();
        operatorsSection.ShouldBeGreaterThan(0, "Operators tab content should be displayed");
    }

    /// <summary>
    /// Verify the Assigned Operators section is visible
    /// </summary>
    public async Task VerifyAssignedOperatorsSection()
    {
        await _page.Locator("text=Assigned Operators").WaitForAsync();
    }

    /// <summary>
    /// Verify the contracts tab content is displayed
    /// </summary>
    public async Task VerifyContractsTabContent()
    {
        await _page.Locator("text=Assigned Contracts").WaitForAsync();
        var contractsSection = await _page.Locator("text=Assigned Contracts").CountAsync();
        contractsSection.ShouldBeGreaterThan(0, "Contracts tab content should be displayed");
    }

    /// <summary>
    /// Verify the Assigned Contracts section is visible
    /// </summary>
    public async Task VerifyAssignedContractsSection()
    {
        await _page.Locator("text=Assigned Contracts").WaitForAsync();
    }

    /// <summary>
    /// Verify the devices tab content is displayed
    /// </summary>
    public async Task VerifyDevicesTabContent()
    {
        await _page.Locator("text=Assigned Devices").WaitForAsync();
        var devicesSection = await _page.Locator("text=Assigned Devices").CountAsync();
        devicesSection.ShouldBeGreaterThan(0, "Devices tab content should be displayed");
    }

    /// <summary>
    /// Verify the Assigned Devices section is visible
    /// </summary>
    public async Task VerifyAssignedDevicesSection()
    {
        await _page.Locator("text=Assigned Devices").WaitForAsync();
    }

    #endregion

    #region Address Details Verification

    /// <summary>
    /// Verify address line 1
    /// </summary>
    public async Task VerifyAddressLine1(string expectedAddress)
    {
        await _page.Locator("text=Address Line 1").WaitForAsync();
        var addressElement = _page.Locator($"p.text-gray-900:has-text('{expectedAddress}')");
        var addressCount = await addressElement.CountAsync();
        addressCount.ShouldBeGreaterThan(0, $"Address Line 1 should be '{expectedAddress}'");
    }

    /// <summary>
    /// Verify town
    /// </summary>
    public async Task VerifyTown(string expectedTown)
    {
        await _page.Locator("text=Town").WaitForAsync();
        var townElement = _page.Locator($"p.text-gray-900:has-text('{expectedTown}')");
        var townCount = await townElement.CountAsync();
        townCount.ShouldBeGreaterThan(0, $"Town should be '{expectedTown}'");
    }

    /// <summary>
    /// Verify region
    /// </summary>
    public async Task VerifyRegion(string expectedRegion)
    {
        await _page.Locator("text=Region").WaitForAsync();
        var regionElement = _page.Locator($"p.text-gray-900:has-text('{expectedRegion}')");
        var regionCount = await regionElement.CountAsync();
        regionCount.ShouldBeGreaterThan(0, $"Region should be '{expectedRegion}'");
    }

    /// <summary>
    /// Verify postal code
    /// </summary>
    public async Task VerifyPostalCode(string expectedPostalCode)
    {
        await _page.Locator("text=Postal Code").WaitForAsync();
        var postalCodeElement = _page.Locator($"p.text-gray-900:has-text('{expectedPostalCode}')");
        var postalCodeCount = await postalCodeElement.CountAsync();
        postalCodeCount.ShouldBeGreaterThan(0, $"Postal Code should be '{expectedPostalCode}'");
    }

    /// <summary>
    /// Verify country
    /// </summary>
    public async Task VerifyCountry(string expectedCountry)
    {
        await _page.Locator("text=Country").WaitForAsync();
        var countryElement = _page.Locator($"p.text-gray-900:has-text('{expectedCountry}')");
        var countryCount = await countryElement.CountAsync();
        countryCount.ShouldBeGreaterThan(0, $"Country should be '{expectedCountry}'");
    }

    #endregion

    #region Contact Details Verification

    /// <summary>
    /// Verify contact name
    /// </summary>
    public async Task VerifyContactName(string expectedContactName)
    {
        await _page.Locator("text=Contact Name").WaitForAsync();
        var contactNameElement = _page.Locator($"p.text-gray-900:has-text('{expectedContactName}')");
        var contactNameCount = await contactNameElement.CountAsync();
        contactNameCount.ShouldBeGreaterThan(0, $"Contact Name should be '{expectedContactName}'");
    }

    /// <summary>
    /// Verify contact email
    /// </summary>
    public async Task VerifyContactEmail(string expectedContactEmail)
    {
        await _page.Locator("text=Email Address").WaitForAsync();
        var contactEmailElement = _page.Locator($"p.text-gray-900:has-text('{expectedContactEmail}')");
        var contactEmailCount = await contactEmailElement.CountAsync();
        contactEmailCount.ShouldBeGreaterThan(0, $"Contact Email should be '{expectedContactEmail}'");
    }

    /// <summary>
    /// Verify contact phone
    /// </summary>
    public async Task VerifyContactPhone(string expectedContactPhone)
    {
        await _page.Locator("text=Phone Number").WaitForAsync();
        var contactPhoneElement = _page.Locator($"p.text-gray-900:has-text('{expectedContactPhone}')");
        var contactPhoneCount = await contactPhoneElement.CountAsync();
        contactPhoneCount.ShouldBeGreaterThan(0, $"Contact Phone should be '{expectedContactPhone}'");
    }

    #endregion

    #region Form Field Verification

    /// <summary>
    /// Verify merchant name field is visible
    /// </summary>
    public async Task VerifyMerchantNameFieldIsVisible()
    {
        var nameField = _page.Locator("input[name='MerchantName']");
        await nameField.WaitForAsync();
        var isVisible = await nameField.IsVisibleAsync();
        isVisible.ShouldBeTrue("Merchant name field should be visible");
    }

    /// <summary>
    /// Verify merchant name field contains expected value
    /// </summary>
    public async Task VerifyMerchantNameFieldContains(string expectedValue)
    {
        var nameField = _page.Locator("input[name='MerchantName']");
        await nameField.WaitForAsync();
        var fieldValue = await nameField.InputValueAsync();
        fieldValue.ShouldBe(expectedValue, $"Merchant name field should contain '{expectedValue}'");
    }

    /// <summary>
    /// Verify settlement schedule field is visible
    /// </summary>
    public async Task VerifySettlementScheduleFieldIsVisible()
    {
        var scheduleField = _page.Locator("select[name='SettlementSchedule']");
        await scheduleField.WaitForAsync();
        var isVisible = await scheduleField.IsVisibleAsync();
        isVisible.ShouldBeTrue("Settlement schedule field should be visible");
    }

    /// <summary>
    /// Verify address line 1 field is visible
    /// </summary>
    public async Task VerifyAddressLine1FieldIsVisible()
    {
        var addressField = _page.Locator("input[name='AddressLine1']");
        await addressField.WaitForAsync();
        var isVisible = await addressField.IsVisibleAsync();
        isVisible.ShouldBeTrue("Address line 1 field should be visible");
    }

    /// <summary>
    /// Verify contact name field is visible
    /// </summary>
    public async Task VerifyContactNameFieldIsVisible()
    {
        var contactField = _page.Locator("input[name='ContactName']");
        await contactField.WaitForAsync();
        var isVisible = await contactField.IsVisibleAsync();
        isVisible.ShouldBeTrue("Contact name field should be visible");
    }

    /// <summary>
    /// Verify deposit amount field is visible
    /// </summary>
    public async Task VerifyDepositAmountFieldIsVisible()
    {
        var amountField = _page.Locator("#depositAmount");
        await amountField.WaitForAsync();
        var isVisible = await amountField.IsVisibleAsync();
        isVisible.ShouldBeTrue("Deposit amount field should be visible");
    }

    /// <summary>
    /// Verify deposit date field is visible
    /// </summary>
    public async Task VerifyDepositDateFieldIsVisible()
    {
        var dateField = _page.Locator("#depositDate");
        await dateField.WaitForAsync();
        var isVisible = await dateField.IsVisibleAsync();
        isVisible.ShouldBeTrue("Deposit date field should be visible");
    }

    /// <summary>
    /// Verify deposit reference field is visible
    /// </summary>
    public async Task VerifyDepositReferenceFieldIsVisible()
    {
        var referenceField = _page.Locator("#depositReference");
        await referenceField.WaitForAsync();
        var isVisible = await referenceField.IsVisibleAsync();
        isVisible.ShouldBeTrue("Deposit reference field should be visible");
    }

    #endregion

    #region Navigation Actions

    /// <summary>
    /// Click on a merchant to view its details
    /// </summary>
    public async Task ClickOnMerchant(string merchantName)
    {
        var merchantRow = _page.Locator($"#merchantList tbody tr:has-text('{merchantName}')");
        await merchantRow.WaitForAsync();
        
        // Click the view button for the merchant
        var viewButton = merchantRow.Locator("#viewMerchantLink").First;
        await viewButton.ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    /// <summary>
    /// Click the Add New Merchant button
    /// </summary>
    public async Task ClickAddNewMerchantButton()
    {
        var addButton = _page.Locator("#newMerchantButton");
        await addButton.WaitForAsync();
        await addButton.ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    /// <summary>
    /// Click the Edit button for a specific merchant
    /// </summary>
    public async Task ClickEditForMerchant(string merchantName)
    {
        var merchantRow = _page.Locator($"#merchantList tbody tr:has-text('{merchantName}')");
        await merchantRow.WaitForAsync();
        
        var editButton = merchantRow.Locator("#editMerchantLink").First;
        await editButton.ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    /// <summary>
    /// Click the Make Deposit button for a specific merchant
    /// </summary>
    public async Task ClickMakeDepositForMerchant(string merchantName)
    {
        var merchantRow = _page.Locator($"#merchantList tbody tr:has-text('{merchantName}')");
        await merchantRow.WaitForAsync();
        
        var depositButton = merchantRow.Locator("#makeDepositLink").First;
        await depositButton.ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    #endregion
}
