using Bunit;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BusinessLogic.Requests;
using Moq;
using Shouldly;
using SimpleResults;
using MerchantsNew = EstateManagementUI.BlazorServer.Components.Pages.Merchants.New;
using AngleSharp.Dom;

namespace EstateManagementUI.BlazorServer.Tests.Pages.Merchants;

public class MerchantsNewPageTests : BaseTest
{
    [Fact]
    public void MerchantsNew_RendersCorrectly()
    {
        // Act
        var cut = RenderComponent<MerchantsNew>();
        
        // Assert
        cut.Markup.ShouldContain("Create New Merchant");
    }

    [Fact]
    public void MerchantsNew_HasCorrectPageTitle()
    {
        // Act
        var cut = RenderComponent<MerchantsNew>();
        
        // Assert
        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }

    [Fact]
    public void MerchantsNew_CancelButton_NavigatesToMerchantsList()
    {
        // Arrange
        var cut = RenderComponent<MerchantsNew>();

        // Act - Find and click the Cancel button
        var buttons = cut.FindAll("button");
        var cancelButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Cancel"));
        cancelButton?.Click();

        // Assert
        _fakeNavigationManager.Uri.ShouldContain("/merchants");
    }

    [Fact]
    public void MerchantsNew_CreateMerchantButton_Exists()
    {
        // Arrange
        var cut = RenderComponent<MerchantsNew>();

        // Act - Find the Create Merchant button by ID
        var createButton = cut.Find("#createMerchantButton");

        // Assert
        createButton.ShouldNotBeNull();
        createButton.TextContent.ShouldContain("Create Merchant");
    }

    [Fact]
    public void MerchantsNew_SuccessfulCreation_ShowsSuccessMessage()
    {
        // Arrange
        this.MerchantUIService.Setup(m => m.CreateMerchant(
            It.IsAny<CorrelationId>(), 
            It.IsAny<Guid>(), 
            It.IsAny<Guid>(), 
            It.IsAny<MerchantModels.CreateMerchantModel>()))
            .ReturnsAsync(Result.Success);

        var cut = RenderComponent<MerchantsNew>();
        cut.Instance.SetDelayOverride(0);
        cut.Render(); // required to trigger re-render

        // Act - Fill in form and submit
        var merchantNameInput = cut.Find("input[name='MerchantName']");
        merchantNameInput.Change("Test Merchant");

        var settlementScheduleSelect = cut.Find("select[name='SettlementSchedule']");
        settlementScheduleSelect.Change("Weekly");

        var addressLine1Input = cut.Find("input[name='AddressLine1']");
        addressLine1Input.Change("123 Test Street");

        var townInput = cut.Find("input[name='Town']");
        townInput.Change("Test Town");

        var regionInput = cut.Find("input[name='Region']");
        regionInput.Change("Test Region");

        var postCodeInput = cut.Find("input[name='PostCode']");
        postCodeInput.Change("12345");

        var countrySelect = cut.Find("select[name='Country']");
        countrySelect.Change("United Kingdom");

        var contactNameInput = cut.Find("input[name='ContactName']");
        contactNameInput.Change("John Doe");

        var emailInput = cut.Find("input[name='EmailAddress']");
        emailInput.Change("john@example.com");

        var phoneInput = cut.Find("input[name='PhoneNumber']");
        phoneInput.Change("1234567890");

        var createButton = cut.Find("#createMerchantButton");
        createButton.Click();

        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Merchant created successfully"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsNew_SuccessfulCreation_NavigatesToMerchantsList()
    {
        // Arrange
        this.MerchantUIService.Setup(m => m.CreateMerchant(
            It.IsAny<CorrelationId>(), 
            It.IsAny<Guid>(), 
            It.IsAny<Guid>(), 
            It.IsAny<MerchantModels.CreateMerchantModel>()))
            .ReturnsAsync(Result.Success);

        var cut = RenderComponent<MerchantsNew>();
        cut.Instance.SetDelayOverride(0);
        cut.Render(); // required to trigger re-render

        // Act - Fill in form and submit
        var merchantNameInput = cut.Find("input[name='MerchantName']");
        merchantNameInput.Change("Test Merchant");

        var settlementScheduleSelect = cut.Find("select[name='SettlementSchedule']");
        settlementScheduleSelect.Change("Weekly");

        var addressLine1Input = cut.Find("input[name='AddressLine1']");
        addressLine1Input.Change("123 Test Street");

        var townInput = cut.Find("input[name='Town']");
        townInput.Change("Test Town");

        var regionInput = cut.Find("input[name='Region']");
        regionInput.Change("Test Region");

        var postCodeInput = cut.Find("input[name='PostCode']");
        postCodeInput.Change("12345");

        var countrySelect = cut.Find("select[name='Country']");
        countrySelect.Change("United Kingdom");

        var contactNameInput = cut.Find("input[name='ContactName']");
        contactNameInput.Change("John Doe");

        var emailInput = cut.Find("input[name='EmailAddress']");
        emailInput.Change("john@example.com");

        var phoneInput = cut.Find("input[name='PhoneNumber']");
        phoneInput.Change("1234567890");

        var createButton = cut.Find("#createMerchantButton");
        createButton.Click();

        // Assert - Wait for navigation
        cut.WaitForAssertion(() => _fakeNavigationManager.Uri.ShouldContain("/merchants"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsNew_FailedCreation_ShowsErrorMessage()
    {
        // Arrange
        this.MerchantUIService.Setup(m => m.CreateMerchant(
            It.IsAny<CorrelationId>(), 
            It.IsAny<Guid>(), 
            It.IsAny<Guid>(), 
            It.IsAny<MerchantModels.CreateMerchantModel>()))
            .ReturnsAsync(Result.Failure);

        var cut = RenderComponent<MerchantsNew>();

        // Act - Fill in form and submit
        var merchantNameInput = cut.Find("input[name='MerchantName']");
        merchantNameInput.Change("Test Merchant");

        var settlementScheduleSelect = cut.Find("select[name='SettlementSchedule']");
        settlementScheduleSelect.Change("Weekly");

        var addressLine1Input = cut.Find("input[name='AddressLine1']");
        addressLine1Input.Change("123 Test Street");

        var townInput = cut.Find("input[name='Town']");
        townInput.Change("Test Town");

        var regionInput = cut.Find("input[name='Region']");
        regionInput.Change("Test Region");

        var postCodeInput = cut.Find("input[name='PostCode']");
        postCodeInput.Change("12345");

        var countrySelect = cut.Find("select[name='Country']");
        countrySelect.Change("United Kingdom");

        var contactNameInput = cut.Find("input[name='ContactName']");
        contactNameInput.Change("John Doe");

        var emailInput = cut.Find("input[name='EmailAddress']");
        emailInput.Change("john@example.com");

        var phoneInput = cut.Find("input[name='PhoneNumber']");
        phoneInput.Change("1234567890");

        var createButton = cut.Find("#createMerchantButton");
        createButton.Click();

        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Failed to create merchant"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsNew_FailedCreation_DoesNotNavigate()
    {
        // Arrange
        this.MerchantUIService.Setup(m => m.CreateMerchant(
            It.IsAny<CorrelationId>(), 
            It.IsAny<Guid>(), 
            It.IsAny<Guid>(), 
            It.IsAny<MerchantModels.CreateMerchantModel>()))
            .ReturnsAsync(Result.Failure);

        var cut = RenderComponent<MerchantsNew>();

        // Act - Fill in form and submit
        var merchantNameInput = cut.Find("input[name='MerchantName']");
        merchantNameInput.Change("Test Merchant");

        var settlementScheduleSelect = cut.Find("select[name='SettlementSchedule']");
        settlementScheduleSelect.Change("Weekly");

        var addressLine1Input = cut.Find("input[name='AddressLine1']");
        addressLine1Input.Change("123 Test Street");

        var townInput = cut.Find("input[name='Town']");
        townInput.Change("Test Town");

        var regionInput = cut.Find("input[name='Region']");
        regionInput.Change("Test Region");

        var postCodeInput = cut.Find("input[name='PostCode']");
        postCodeInput.Change("12345");

        var countrySelect = cut.Find("select[name='Country']");
        countrySelect.Change("United Kingdom");

        var contactNameInput = cut.Find("input[name='ContactName']");
        contactNameInput.Change("John Doe");

        var emailInput = cut.Find("input[name='EmailAddress']");
        emailInput.Change("john@example.com");

        var phoneInput = cut.Find("input[name='PhoneNumber']");
        phoneInput.Change("1234567890");

        var createButton = cut.Find("#createMerchantButton");
        createButton.Click();

        // Assert - Should not navigate to /merchants
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Failed to create merchant"), timeout: TimeSpan.FromSeconds(5));
        _fakeNavigationManager.Uri.ShouldNotContain("/merchants");
    }

    [Fact]
    public void MerchantsNew_SavingState_ShowsLoadingIndicator()
    {
        // Arrange
        var tcs = new TaskCompletionSource<Result>();
        this.MerchantUIService.Setup(m => m.CreateMerchant(
            It.IsAny<CorrelationId>(), 
            It.IsAny<Guid>(), 
            It.IsAny<Guid>(), 
            It.IsAny<MerchantModels.CreateMerchantModel>()))
            .Returns(tcs.Task);

        var cut = RenderComponent<MerchantsNew>();

        // Act - Fill in form and submit
        var merchantNameInput = cut.Find("input[name='MerchantName']");
        merchantNameInput.Change("Test Merchant");

        var settlementScheduleSelect = cut.Find("select[name='SettlementSchedule']");
        settlementScheduleSelect.Change("Weekly");

        var addressLine1Input = cut.Find("input[name='AddressLine1']");
        addressLine1Input.Change("123 Test Street");

        var townInput = cut.Find("input[name='Town']");
        townInput.Change("Test Town");

        var regionInput = cut.Find("input[name='Region']");
        regionInput.Change("Test Region");

        var postCodeInput = cut.Find("input[name='PostCode']");
        postCodeInput.Change("12345");

        var countrySelect = cut.Find("select[name='Country']");
        countrySelect.Change("United Kingdom");

        var contactNameInput = cut.Find("input[name='ContactName']");
        contactNameInput.Change("John Doe");

        var emailInput = cut.Find("input[name='EmailAddress']");
        emailInput.Change("john@example.com");

        var phoneInput = cut.Find("input[name='PhoneNumber']");
        phoneInput.Change("1234567890");

        var createButton = cut.Find("#createMerchantButton");
        createButton.Click();

        // Assert - Should show "Saving..." text
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Saving..."), timeout: TimeSpan.FromSeconds(5));
        
        // Complete the task
        tcs.SetResult(Result.Success());
    }
}
