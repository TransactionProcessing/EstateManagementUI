using Bunit;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BusinessLogic.Requests;
using Imposter.Abstractions;
using Shouldly;
using SimpleResults;
using MerchantsNew = EstateManagementUI.BlazorServer.Components.Pages.Merchants.New;

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
        var cut = Render<MerchantsNew>();

        // Render
        cut.Markup.ShouldContain("Create New Merchant");
    }

    [Fact]
    public void MerchantsNew_HasCorrectPageTitle()
    {
        // Act
        var cut = Render<MerchantsNew>();

        // Render
        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }

    [Fact]
    public void MerchantsNew_CancelButton_NavigatesToMerchantsList()
    {
        // Arrange
        var cut = Render<MerchantsNew>();

        // Act - Find and click the Cancel button
        var buttons = cut.FindAll("button");
        var cancelButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Cancel"));
        cancelButton?.Click();

        // Render
        _fakeNavigationManager.Uri.ShouldContain("/merchants");
    }

    [Fact]
    public void MerchantsNew_CreateMerchantButton_Exists()
    {
        // Arrange
        var cut = Render<MerchantsNew>();

        // Act - Find the Create Merchant button by ID
        var createButton = cut.Find("#createMerchantButton");

        // Render
        createButton.ShouldNotBeNull();
        createButton.TextContent.ShouldContain("Create Merchant");
    }

    [Fact]
    public async Task HandleSubmit_SuccessfulCreation_SetsSuccessMessage()
    {
        // Arrange
        this.MerchantUIService.CreateMerchant(
            Arg<CorrelationId>.Any(),
            Arg<Guid>.Any(),
            Arg<Guid>.Any(),
            Arg<MerchantModels.CreateMerchantModel>.Any())
            .ReturnsAsync(Result.Success());

        var cut = Render<MerchantsNew>();
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

        // Render - Verify success message was set
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Merchant created successfully"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task HandleSubmit_SuccessfulCreation_NavigatesToMerchantsList()
    {
        // Arrange
        this.MerchantUIService.CreateMerchant(
            Arg<CorrelationId>.Any(),
            Arg<Guid>.Any(),
            Arg<Guid>.Any(),
            Arg<MerchantModels.CreateMerchantModel>.Any())
            .ReturnsAsync(Result.Success());

        var cut = Render<MerchantsNew>();
        cut.Instance.SetDelayOverride(0);

        // Act - Use reflection to call the private HandleSubmit method on the Dispatcher
        var handleSubmitMethod = GetHandleSubmitMethod();
        handleSubmitMethod.ShouldNotBeNull();

        await cut.InvokeAsync(async () =>
        {
            var task = (Task)handleSubmitMethod.Invoke(cut.Instance, null);
            await task;
        });

        // Render - Should navigate to /merchants
        _fakeNavigationManager.Uri.ShouldContain("/merchants");
    }

    [Fact]
    public async Task HandleSubmit_FailedCreation_SetsErrorMessage()
    {
        // Arrange
        this.MerchantUIService.CreateMerchant(
            Arg<CorrelationId>.Any(),
            Arg<Guid>.Any(),
            Arg<Guid>.Any(),
            Arg<MerchantModels.CreateMerchantModel>.Any())
            .ReturnsAsync(Result.Failure());

        var cut = Render<MerchantsNew>();

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

        // Render - Verify error message was set
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Failed to create merchant"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task HandleSubmit_FailedCreation_DoesNotNavigate()
    {
        // Arrange
        this.MerchantUIService.CreateMerchant(
            Arg<CorrelationId>.Any(),
            Arg<Guid>.Any(),
            Arg<Guid>.Any(),
            Arg<MerchantModels.CreateMerchantModel>.Any())
            .ReturnsAsync(Result.Failure());

        var cut = Render<MerchantsNew>();
        var initialUri = _fakeNavigationManager.Uri;

        // Act - Use reflection to call the private HandleSubmit method on the Dispatcher
        var handleSubmitMethod = GetHandleSubmitMethod();
        handleSubmitMethod.ShouldNotBeNull();

        await cut.InvokeAsync(async () =>
        {
            var task = (Task)handleSubmitMethod.Invoke(cut.Instance, null);
            await task;
        });

        // Render - Should not navigate (should remain on the same page, not go to /merchants list)
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

        this.MerchantUIService.CreateMerchant(
            Arg<CorrelationId>.Any(),
            Arg<Guid>.Any(),
            Arg<Guid>.Any(),
            Arg<MerchantModels.CreateMerchantModel>.Any())
            .ReturnsAsync(Result.Success())
            .Callback(
                (correlationId, estateId, merchantId, model) => {
                    capturedCorrelationId = correlationId;
                    capturedEstateId = estateId;
                    capturedMerchantId = merchantId;
                    capturedModel = model;
                    return Task.CompletedTask;
                });

        var cut = Render<MerchantsNew>();
        cut.Instance.SetDelayOverride(0);

        // Act - Use reflection to call the private HandleSubmit method on the Dispatcher
        var handleSubmitMethod = GetHandleSubmitMethod();
        handleSubmitMethod.ShouldNotBeNull();

        await cut.InvokeAsync(async () =>
        {
            var task = (Task)handleSubmitMethod.Invoke(cut.Instance, null);
            await task;
        });

        // Render - Verify CreateMerchant was called with proper parameters
        this.MerchantUIService.CreateMerchant(
            Arg<CorrelationId>.Any(),
            Arg<Guid>.Any(),
            Arg<Guid>.Any(),
            Arg<MerchantModels.CreateMerchantModel>.Any()).Called(Count.Once());

        capturedCorrelationId.ShouldNotBeNull();
        capturedEstateId.ShouldNotBe(Guid.Empty);
        capturedMerchantId.ShouldNotBe(Guid.Empty);
        capturedModel.ShouldNotBeNull();
    }

    [Fact]
    public async Task HandleSubmit_ClearsErrorMessageBeforeSubmit()
    {
        // Arrange
        this.MerchantUIService.CreateMerchant(
            Arg<CorrelationId>.Any(),
            Arg<Guid>.Any(),
            Arg<Guid>.Any(),
            Arg<MerchantModels.CreateMerchantModel>.Any())
            .ReturnsAsync(Result.Success());

        var cut = Render<MerchantsNew>();
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

        // Render - Error message should be cleared and success message shown
        cut.Markup.ShouldNotContain("Previous error");
        cut.Markup.ShouldContain("Merchant created successfully");
    }
}
