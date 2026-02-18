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

    [Fact(Skip = "Form submission tests require CountrySelector interaction - tracked in separate issue")]
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

    [Fact(Skip = "Form submission tests require CountrySelector interaction - tracked in separate issue")]
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

    [Fact(Skip = "Form submission tests require CountrySelector interaction - tracked in separate issue")]
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

    [Fact(Skip = "Form submission tests require CountrySelector interaction - tracked in separate issue")]
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

    [Fact(Skip = "Form submission tests require CountrySelector interaction - tracked in separate issue")]
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

        // Interact with CountrySelector - find the button that opens the dropdown
        var countryButtons = cut.FindAll("button[aria-label='Select country']");
        if (countryButtons.Any())
        {
            countryButtons.First().Click();
            // Find and click a country option (e.g., United Kingdom)
            var countryOptions = cut.FindAll("button");
            var ukButton = countryOptions.FirstOrDefault(b => b.TextContent.Contains("United Kingdom"));
            ukButton?.Click();
        }

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

    [Fact]
    public async Task HandleSubmit_SuccessfulCreation_SetsSuccessMessage()
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

        // Act - Use reflection to call the private HandleSubmit method
        var handleSubmitMethod = typeof(MerchantsNew).GetMethod("HandleSubmit", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        handleSubmitMethod.ShouldNotBeNull();
        
        var task = (Task)handleSubmitMethod.Invoke(cut.Instance, null);
        await task;

        // Assert - Verify success message was set
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Merchant created successfully"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task HandleSubmit_SuccessfulCreation_NavigatesToMerchantsList()
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

        // Act - Use reflection to call the private HandleSubmit method
        var handleSubmitMethod = typeof(MerchantsNew).GetMethod("HandleSubmit", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        handleSubmitMethod.ShouldNotBeNull();
        
        var task = (Task)handleSubmitMethod.Invoke(cut.Instance, null);
        await task;

        // Assert - Should navigate to /merchants
        _fakeNavigationManager.Uri.ShouldContain("/merchants");
    }

    [Fact]
    public async Task HandleSubmit_FailedCreation_SetsErrorMessage()
    {
        // Arrange
        this.MerchantUIService.Setup(m => m.CreateMerchant(
            It.IsAny<CorrelationId>(), 
            It.IsAny<Guid>(), 
            It.IsAny<Guid>(), 
            It.IsAny<MerchantModels.CreateMerchantModel>()))
            .ReturnsAsync(Result.Failure);

        var cut = RenderComponent<MerchantsNew>();

        // Act - Use reflection to call the private HandleSubmit method
        var handleSubmitMethod = typeof(MerchantsNew).GetMethod("HandleSubmit", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        handleSubmitMethod.ShouldNotBeNull();
        
        var task = (Task)handleSubmitMethod.Invoke(cut.Instance, null);
        await task;

        // Assert - Verify error message was set
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Failed to create merchant"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task HandleSubmit_FailedCreation_DoesNotNavigate()
    {
        // Arrange
        this.MerchantUIService.Setup(m => m.CreateMerchant(
            It.IsAny<CorrelationId>(), 
            It.IsAny<Guid>(), 
            It.IsAny<Guid>(), 
            It.IsAny<MerchantModels.CreateMerchantModel>()))
            .ReturnsAsync(Result.Failure);

        var cut = RenderComponent<MerchantsNew>();
        var initialUri = _fakeNavigationManager.Uri;

        // Act - Use reflection to call the private HandleSubmit method
        var handleSubmitMethod = typeof(MerchantsNew).GetMethod("HandleSubmit", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        handleSubmitMethod.ShouldNotBeNull();
        
        var task = (Task)handleSubmitMethod.Invoke(cut.Instance, null);
        await task;

        // Assert - Should not navigate (should remain on the same page, not go to /merchants list)
        _fakeNavigationManager.Uri.ShouldBe(initialUri);
    }

    [Fact]
    public async Task HandleSubmit_CallsCreateMerchantWithCorrectParameters()
    {
        // Arrange
        Guid capturedEstateId = Guid.Empty;
        Guid capturedMerchantId = Guid.Empty;
        CorrelationId capturedCorrelationId = null;
        MerchantModels.CreateMerchantModel capturedModel = null;

        this.MerchantUIService.Setup(m => m.CreateMerchant(
            It.IsAny<CorrelationId>(), 
            It.IsAny<Guid>(), 
            It.IsAny<Guid>(), 
            It.IsAny<MerchantModels.CreateMerchantModel>()))
            .Callback<CorrelationId, Guid, Guid, MerchantModels.CreateMerchantModel>(
                (correlationId, estateId, merchantId, model) => {
                    capturedCorrelationId = correlationId;
                    capturedEstateId = estateId;
                    capturedMerchantId = merchantId;
                    capturedModel = model;
                })
            .ReturnsAsync(Result.Success);

        var cut = RenderComponent<MerchantsNew>();
        cut.Instance.SetDelayOverride(0);

        // Act - Use reflection to call the private HandleSubmit method
        var handleSubmitMethod = typeof(MerchantsNew).GetMethod("HandleSubmit", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        handleSubmitMethod.ShouldNotBeNull();
        
        var task = (Task)handleSubmitMethod.Invoke(cut.Instance, null);
        await task;

        // Assert - Verify CreateMerchant was called with proper parameters
        this.MerchantUIService.Verify(m => m.CreateMerchant(
            It.IsAny<CorrelationId>(), 
            It.IsAny<Guid>(), 
            It.IsAny<Guid>(), 
            It.IsAny<MerchantModels.CreateMerchantModel>()), Times.Once);
        
        capturedCorrelationId.ShouldNotBeNull();
        capturedEstateId.ShouldNotBe(Guid.Empty);
        capturedMerchantId.ShouldNotBe(Guid.Empty);
        capturedModel.ShouldNotBeNull();
    }

    [Fact]
    public async Task HandleSubmit_ClearsErrorMessageBeforeSubmit()
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

        // Set an error message first using reflection
        var errorMessageField = typeof(MerchantsNew).BaseType.BaseType
            .GetField("errorMessage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        errorMessageField.ShouldNotBeNull();
        errorMessageField.SetValue(cut.Instance, "Previous error");
        cut.Render();

        // Verify error is displayed
        cut.Markup.ShouldContain("Previous error");

        // Act - Use reflection to call the private HandleSubmit method
        var handleSubmitMethod = typeof(MerchantsNew).GetMethod("HandleSubmit", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        handleSubmitMethod.ShouldNotBeNull();
        
        var task = (Task)handleSubmitMethod.Invoke(cut.Instance, null);
        await task;

        // Assert - Error message should be cleared and success message shown
        cut.Markup.ShouldNotContain("Previous error");
        cut.Markup.ShouldContain("Merchant created successfully");
    }
}
