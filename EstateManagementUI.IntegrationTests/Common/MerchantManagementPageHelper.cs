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

    #region Edit Operations

    /// <summary>
    /// Update merchant name in the edit form
    /// </summary>
    public async Task UpdateMerchantName(string newName)
    {
        var nameField = _page.Locator("input[name='MerchantName']");
        await nameField.WaitForAsync();
        await nameField.ClearAsync();
        await nameField.FillAsync(newName);
    }

    /// <summary>
    /// Update settlement schedule in the edit form
    /// </summary>
    public async Task UpdateSettlementSchedule(string newSchedule)
    {
        var scheduleField = _page.Locator("select.input");
        await scheduleField.First.WaitForAsync();
        await scheduleField.First.SelectOptionAsync(new[] { newSchedule });
    }

    /// <summary>
    /// Click the Save Changes button
    /// </summary>
    public async Task ClickSaveChangesButton()
    {
        var saveButton = _page.Locator("button:has-text('Save Changes')");
        await saveButton.ClickAsync();
        await Task.Delay(1000); // Wait for save operation
    }

    /// <summary>
    /// Verify success message is displayed
    /// </summary>
    public async Task VerifySuccessMessageIsDisplayed()
    {
        await _page.Locator(".bg-green-50").WaitForAsync();
        var successMessageCount = await _page.Locator(".bg-green-50").CountAsync();
        successMessageCount.ShouldBeGreaterThan(0, "Success message should be displayed");
    }

    /// <summary>
    /// Verify success message contains specific text
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
    /// Update address line 1
    /// </summary>
    public async Task UpdateAddressLine1(string newAddress)
    {
        var addressField = _page.Locator("input[name='AddressLine1']");
        await addressField.WaitForAsync();
        await addressField.ClearAsync();
        await addressField.FillAsync(newAddress);
    }

    /// <summary>
    /// Update town
    /// </summary>
    public async Task UpdateTown(string newTown)
    {
        var townField = _page.Locator("input[name='Town']");
        await townField.WaitForAsync();
        await townField.ClearAsync();
        await townField.FillAsync(newTown);
    }

    /// <summary>
    /// Update region
    /// </summary>
    public async Task UpdateRegion(string newRegion)
    {
        var regionField = _page.Locator("input[name='Region']");
        await regionField.WaitForAsync();
        await regionField.ClearAsync();
        await regionField.FillAsync(newRegion);
    }

    /// <summary>
    /// Update contact name
    /// </summary>
    public async Task UpdateContactName(string newContactName)
    {
        var contactNameField = _page.Locator("input[name='ContactName']");
        await contactNameField.WaitForAsync();
        await contactNameField.ClearAsync();
        await contactNameField.FillAsync(newContactName);
    }

    /// <summary>
    /// Update contact email
    /// </summary>
    public async Task UpdateContactEmail(string newEmail)
    {
        var emailField = _page.Locator("input[name='EmailAddress']");
        await emailField.WaitForAsync();
        await emailField.ClearAsync();
        await emailField.FillAsync(newEmail);
    }

    /// <summary>
    /// Update contact phone
    /// </summary>
    public async Task UpdateContactPhone(string newPhone)
    {
        var phoneField = _page.Locator("input[name='PhoneNumber']");
        await phoneField.WaitForAsync();
        await phoneField.ClearAsync();
        await phoneField.FillAsync(newPhone);
    }

    #endregion

    #region Operator Management

    /// <summary>
    /// Click the Add Operator button in edit page
    /// </summary>
    public async Task ClickAddOperatorButton()
    {
        var addButton = _page.Locator("#addOperatorButton");
        await addButton.WaitForAsync();
        await addButton.ClickAsync();
        await Task.Delay(500); // Wait for form to appear
    }

    /// <summary>
    /// Select operator from dropdown
    /// </summary>
    public async Task SelectOperatorFromDropdown(string operatorName)
    {
        var operatorDropdown = _page.Locator("select").First;
        await operatorDropdown.WaitForAsync();
        
        // Find the option by text and select it
        var options = await operatorDropdown.Locator("option").AllAsync();
        foreach (var option in options)
        {
            var optionText = await option.TextContentAsync();
            if (optionText != null && optionText.Trim() == operatorName)
            {
                var value = await option.GetAttributeAsync("value");
                if (!string.IsNullOrEmpty(value))
                {
                    await operatorDropdown.SelectOptionAsync(new[] { value });
                    break;
                }
            }
        }
        await Task.Delay(500); // Wait for operator selection to process
    }

    /// <summary>
    /// Enter merchant number
    /// </summary>
    public async Task EnterMerchantNumber(string merchantNumber)
    {
        var merchantNumberField = _page.Locator("input[placeholder='Enter merchant number']");
        await merchantNumberField.WaitForAsync();
        await merchantNumberField.FillAsync(merchantNumber);
    }

    /// <summary>
    /// Click Add button in operator form
    /// </summary>
    public async Task ClickAddButtonInOperatorForm()
    {
        // More specific selector for the Add button in the operator form
        var addButton = _page.Locator("div.bg-gray-50:has-text('Select Operator') button:has-text('Add')");
        await addButton.ClickAsync();
        await Task.Delay(1000); // Wait for operation to complete
    }

    /// <summary>
    /// Verify operator is listed in assigned operators
    /// </summary>
    public async Task VerifyOperatorIsListedInAssignedOperators(string operatorName)
    {
        await Task.Delay(500); // Wait for list to update
        var operatorElement = _page.Locator($".bg-gray-50:has-text('{operatorName}')");
        var operatorCount = await operatorElement.CountAsync();
        operatorCount.ShouldBeGreaterThan(0, $"Operator '{operatorName}' should be listed in assigned operators");
    }

    /// <summary>
    /// Remove operator from merchant
    /// </summary>
    public async Task RemoveOperatorFromMerchant(string operatorName)
    {
        var operatorRow = _page.Locator($".bg-gray-50:has-text('{operatorName}')");
        await operatorRow.WaitForAsync();
        var removeButton = operatorRow.Locator("button:has-text('Remove')");
        await removeButton.ClickAsync();
        await Task.Delay(1000); // Wait for operation to complete
    }

    /// <summary>
    /// Verify operator is not listed in assigned operators
    /// </summary>
    public async Task VerifyOperatorIsNotListedInAssignedOperators(string operatorName)
    {
        await Task.Delay(500); // Wait for list to update
        var operatorElement = _page.Locator($".bg-gray-50 span:has-text('{operatorName}')");
        var operatorCount = await operatorElement.CountAsync();
        operatorCount.ShouldBe(0, $"Operator '{operatorName}' should not be listed in assigned operators");
    }

    #endregion

    #region Contract Management

    /// <summary>
    /// Click the Assign Contract button in edit page
    /// </summary>
    public async Task ClickAssignContractButton()
    {
        var assignButton = _page.Locator("#addContractButton");
        await assignButton.WaitForAsync();
        await assignButton.ClickAsync();
        await Task.Delay(500); // Wait for form to appear
    }

    /// <summary>
    /// Select contract from dropdown
    /// </summary>
    public async Task SelectContractFromDropdown(string contractDescription)
    {
        var contractDropdown = _page.Locator("select").Filter(new LocatorFilterOptions { HasText = "Select a contract" });
        await contractDropdown.First.WaitForAsync();
        
        // Select by visible text that contains the contract description
        var options = await contractDropdown.First.Locator("option").AllAsync();
        foreach (var option in options)
        {
            var optionText = await option.TextContentAsync();
            if (optionText != null && optionText.Contains(contractDescription))
            {
                var value = await option.GetAttributeAsync("value");
                if (!string.IsNullOrEmpty(value))
                {
                    await contractDropdown.First.SelectOptionAsync(new[] { value });
                    break;
                }
            }
        }
        await Task.Delay(500);
    }

    /// <summary>
    /// Click Assign button in contract form
    /// </summary>
    public async Task ClickAssignButtonInContractForm()
    {
        var assignButton = _page.Locator(".bg-gray-50 button:has-text('Assign')").First;
        await assignButton.ClickAsync();
        await Task.Delay(1000); // Wait for operation to complete
    }

    /// <summary>
    /// Verify contract is listed in assigned contracts
    /// </summary>
    public async Task VerifyContractIsListedInAssignedContracts(string contractDescription)
    {
        await Task.Delay(500); // Wait for list to update
        var contractElement = _page.Locator($".bg-gray-50:has-text('{contractDescription}')");
        var contractCount = await contractElement.CountAsync();
        contractCount.ShouldBeGreaterThan(0, $"Contract '{contractDescription}' should be listed in assigned contracts");
    }

    /// <summary>
    /// Remove contract from merchant
    /// </summary>
    public async Task RemoveContractFromMerchant(string contractDescription)
    {
        var contractRow = _page.Locator($".bg-gray-50:has-text('{contractDescription}')");
        await contractRow.WaitForAsync();
        var removeButton = contractRow.Locator("button:has-text('Remove')");
        await removeButton.ClickAsync();
        await Task.Delay(1000); // Wait for operation to complete
    }

    /// <summary>
    /// Verify contract is not listed in assigned contracts
    /// </summary>
    public async Task VerifyContractIsNotListedInAssignedContracts(string contractDescription)
    {
        await Task.Delay(500); // Wait for list to update
        var contractElement = _page.Locator($".bg-gray-50 p:has-text('{contractDescription}')");
        var contractCount = await contractElement.CountAsync();
        contractCount.ShouldBe(0, $"Contract '{contractDescription}' should not be listed in assigned contracts");
    }

    #endregion

    #region Device Management

    /// <summary>
    /// Click the Add Device button in edit page
    /// </summary>
    public async Task ClickAddDeviceButton()
    {
        var addButton = _page.Locator("#addDeviceButton");
        await addButton.WaitForAsync();
        await addButton.ClickAsync();
        await Task.Delay(500); // Wait for form to appear
    }

    /// <summary>
    /// Enter device identifier
    /// </summary>
    public async Task EnterDeviceIdentifier(string deviceIdentifier)
    {
        var deviceField = _page.Locator("input[placeholder='Enter device identifier']");
        await deviceField.WaitForAsync();
        await deviceField.FillAsync(deviceIdentifier);
    }

    /// <summary>
    /// Click Add button in device form
    /// </summary>
    public async Task ClickAddButtonInDeviceForm()
    {
        // More specific selector for the Add button in the device form
        var addButton = _page.Locator("div.bg-gray-50:has-text('Device Identifier') button:has-text('Add')");
        await addButton.ClickAsync();
        await Task.Delay(1000); // Wait for operation to complete
    }

    /// <summary>
    /// Verify device is listed in assigned devices
    /// </summary>
    public async Task VerifyDeviceIsListedInAssignedDevices(string deviceIdentifier)
    {
        await Task.Delay(500); // Wait for list to update
        var deviceElement = _page.Locator($".font-mono:has-text('{deviceIdentifier}')");
        var deviceCount = await deviceElement.CountAsync();
        deviceCount.ShouldBeGreaterThan(0, $"Device '{deviceIdentifier}' should be listed in assigned devices");
    }

    /// <summary>
    /// Remove device from merchant
    /// </summary>
    public async Task RemoveDeviceFromMerchant(string deviceIdentifier)
    {
        var deviceRow = _page.Locator($".bg-gray-50:has-text('{deviceIdentifier}')");
        await deviceRow.WaitForAsync();
        var removeButton = deviceRow.Locator("button:has-text('Remove')");
        await removeButton.ClickAsync();
        await Task.Delay(1000); // Wait for operation to complete
    }

    /// <summary>
    /// Verify device is not listed in assigned devices
    /// </summary>
    public async Task VerifyDeviceIsNotListedInAssignedDevices(string deviceIdentifier)
    {
        await Task.Delay(500); // Wait for list to update
        var deviceElement = _page.Locator($".font-mono:has-text('{deviceIdentifier}')");
        var deviceCount = await deviceElement.CountAsync();
        deviceCount.ShouldBe(0, $"Device '{deviceIdentifier}' should not be listed in assigned devices");
    }

    #endregion

    #region Deposit Operations

    /// <summary>
    /// Enter deposit amount
    /// </summary>
    public async Task EnterDepositAmount(string amount)
    {
        var amountField = _page.Locator("#depositAmount");
        await amountField.WaitForAsync();
        await amountField.FillAsync(amount);
    }

    /// <summary>
    /// Select today as deposit date
    /// </summary>
    public async Task SelectTodayAsDepositDate()
    {
        var dateField = _page.Locator("#depositDate");
        await dateField.WaitForAsync();
        var today = DateTime.Now.ToString("yyyy-MM-dd");
        await dateField.FillAsync(today);
    }

    /// <summary>
    /// Enter deposit reference
    /// </summary>
    public async Task EnterDepositReference(string reference)
    {
        var referenceField = _page.Locator("#depositReference");
        await referenceField.WaitForAsync();
        await referenceField.FillAsync(reference);
    }

    /// <summary>
    /// Click the Make Deposit button on deposit page
    /// </summary>
    public async Task ClickMakeDepositButton()
    {
        var depositButton = _page.Locator("#makeDepositButton");
        await depositButton.WaitForAsync();
        await depositButton.ClickAsync();
        await Task.Delay(1000); // Wait for operation to complete
    }

    #endregion
}
