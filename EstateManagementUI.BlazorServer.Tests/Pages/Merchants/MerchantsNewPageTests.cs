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
    private System.Reflection.MethodInfo GetHandleSubmitMethod()
    {
        return typeof(MerchantsNew).GetMethod("HandleSubmit", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
    }

    private System.Reflection.FieldInfo GetErrorMessageField(object instance)
    {
        // Search through the type hierarchy to find the errorMessage field
        Type currentType = instance.GetType();
        System.Reflection.FieldInfo field = null;
        
        while (currentType != null && field == null)
        {
            field = currentType.GetField("errorMessage", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            currentType = currentType.BaseType;
        }
        
        return field;
    }

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

        // Act - Use reflection to call the private HandleSubmit method on the Dispatcher
        var handleSubmitMethod = GetHandleSubmitMethod();
        handleSubmitMethod.ShouldNotBeNull();
        
        await cut.InvokeAsync(async () =>
        {
            var task = (Task)handleSubmitMethod.Invoke(cut.Instance, null);
            await task;
        });

        // Trigger a render to ensure state changes are reflected
        cut.Render();

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

        // Act - Use reflection to call the private HandleSubmit method on the Dispatcher
        var handleSubmitMethod = GetHandleSubmitMethod();
        handleSubmitMethod.ShouldNotBeNull();
        
        await cut.InvokeAsync(async () =>
        {
            var task = (Task)handleSubmitMethod.Invoke(cut.Instance, null);
            await task;
        });

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

        // Act - Use reflection to call the private HandleSubmit method on the Dispatcher
        var handleSubmitMethod = GetHandleSubmitMethod();
        handleSubmitMethod.ShouldNotBeNull();
        
        await cut.InvokeAsync(async () =>
        {
            var task = (Task)handleSubmitMethod.Invoke(cut.Instance, null);
            await task;
        });

        // Trigger a render to ensure state changes are reflected
        cut.Render();

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

        // Act - Use reflection to call the private HandleSubmit method on the Dispatcher
        var handleSubmitMethod = GetHandleSubmitMethod();
        handleSubmitMethod.ShouldNotBeNull();
        
        await cut.InvokeAsync(async () =>
        {
            var task = (Task)handleSubmitMethod.Invoke(cut.Instance, null);
            await task;
        });

        // Assert - Should not navigate (should remain on the same page, not go to /merchants list)
        _fakeNavigationManager.Uri.ShouldBe(initialUri);
    }

    [Fact]
    public async Task HandleSubmit_CallsCreateMerchantWithCorrectParameters()
    {
        // Arrange
        Guid capturedEstateId = default;
        Guid capturedMerchantId = default;
        CorrelationId capturedCorrelationId = default;
        MerchantModels.CreateMerchantModel capturedModel = default;

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

        // Act - Use reflection to call the private HandleSubmit method on the Dispatcher
        var handleSubmitMethod = GetHandleSubmitMethod();
        handleSubmitMethod.ShouldNotBeNull();
        
        await cut.InvokeAsync(async () =>
        {
            var task = (Task)handleSubmitMethod.Invoke(cut.Instance, null);
            await task;
        });

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
        var errorMessageField = GetErrorMessageField(cut.Instance);
        errorMessageField.ShouldNotBeNull();
        errorMessageField.SetValue(cut.Instance, "Previous error");
        cut.Render();

        // Verify error is displayed
        cut.Markup.ShouldContain("Previous error");

        // Act - Use reflection to call the private HandleSubmit method on the Dispatcher
        var handleSubmitMethod = GetHandleSubmitMethod();
        handleSubmitMethod.ShouldNotBeNull();
        
        await cut.InvokeAsync(async () =>
        {
            var task = (Task)handleSubmitMethod.Invoke(cut.Instance, null);
            await task;
        });

        // Trigger a render to ensure state changes are reflected
        cut.Render();

        // Assert - Error message should be cleared and success message shown
        cut.Markup.ShouldNotContain("Previous error");
        cut.Markup.ShouldContain("Merchant created successfully");
    }
}
