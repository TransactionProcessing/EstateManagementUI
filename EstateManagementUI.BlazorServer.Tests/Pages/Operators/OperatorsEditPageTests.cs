using AngleSharp.Dom;
using Bunit;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BusinessLogic.Requests;
using Moq;
using Shouldly;
using SimpleResults;
using OperatorsEdit = EstateManagementUI.BlazorServer.Components.Pages.Operators.Edit;

namespace EstateManagementUI.BlazorServer.Tests.Pages.Operators;

public class OperatorsEditPageTests : BaseTest
{
    [Fact]
    public void OperatorsEdit_RendersCorrectly()
    {
        // Arrange
        var operatorId = Guid.NewGuid();
        var operatorModel = new OperatorModels.OperatorModel
        {
            OperatorId = operatorId,
            Name = "Test Operator",
            RequireCustomMerchantNumber = true,
            RequireCustomTerminalNumber = false
        };
        
        this.OperatorUIService.Setup(o => o.GetOperator(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(operatorModel));
        
        // Act
        IRenderedComponent<OperatorsEdit> cut = RenderComponent<OperatorsEdit>(parameters => parameters
            .Add(p => p.OperatorId, operatorId));
        
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("Edit Operator");
    }

    [Fact]
    public void OperatorsEdit_HasCorrectPageTitle()
    {
        // Arrange
        var operatorId = Guid.NewGuid();
        var operatorModel = new OperatorModels.OperatorModel
        {
            OperatorId = operatorId,
            Name = "Test Operator",
            RequireCustomMerchantNumber = true,
            RequireCustomTerminalNumber = false
        };
        
        this.OperatorUIService.Setup(o => o.GetOperator(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(operatorModel));
        
        // Act
        IRenderedComponent<OperatorsEdit> cut = RenderComponent<OperatorsEdit>(parameters => parameters
            .Add(p => p.OperatorId, operatorId));
        
        // Assert
        IRenderedComponent<Microsoft.AspNetCore.Components.Web.PageTitle> pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }

    [Fact]
    public void OperatorsEdit_LoadOperator_DisplaysOperatorDetails()
    {
        // Arrange
        var operatorId = Guid.NewGuid();
        var operatorModel = new OperatorModels.OperatorModel
        {
            OperatorId = operatorId,
            Name = "Test Operator",
            RequireCustomMerchantNumber = true,
            RequireCustomTerminalNumber = false
        };
        
        this.OperatorUIService.Setup(o => o.GetOperator(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(operatorModel));
        
        // Act
        IRenderedComponent<OperatorsEdit> cut = RenderComponent<OperatorsEdit>(parameters => parameters
            .Add(p => p.OperatorId, operatorId));
        
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("Edit Operator: Test Operator");
        cut.Markup.ShouldContain("Operator Name");
        cut.Markup.ShouldContain("Require Custom Merchant Number");
        cut.Markup.ShouldContain("Require Custom Terminal Number");
    }

    [Fact]
    public void OperatorsEdit_BackToListButton_NavigatesToOperatorsList()
    {
        // Arrange
        var operatorId = Guid.NewGuid();
        var operatorModel = new OperatorModels.OperatorModel
        {
            OperatorId = operatorId,
            Name = "Test Operator",
            RequireCustomMerchantNumber = true,
            RequireCustomTerminalNumber = false
        };
        
        this.OperatorUIService.Setup(o => o.GetOperator(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(operatorModel));
        
        IRenderedComponent<OperatorsEdit> cut = RenderComponent<OperatorsEdit>(parameters => parameters
            .Add(p => p.OperatorId, operatorId));
        
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act - Find and click "Back to List" button
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? backButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Back to List"));
        backButton.ShouldNotBeNull();
        backButton.Click();
        
        // Assert
        _fakeNavigationManager.Uri.ShouldContain("/operators");
    }

    [Fact]
    public void OperatorsEdit_CancelButton_NavigatesToOperatorsList()
    {
        // Arrange
        var operatorId = Guid.NewGuid();
        var operatorModel = new OperatorModels.OperatorModel
        {
            OperatorId = operatorId,
            Name = "Test Operator",
            RequireCustomMerchantNumber = true,
            RequireCustomTerminalNumber = false
        };
        
        this.OperatorUIService.Setup(o => o.GetOperator(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(operatorModel));
        
        IRenderedComponent<OperatorsEdit> cut = RenderComponent<OperatorsEdit>(parameters => parameters
            .Add(p => p.OperatorId, operatorId));
        
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act - Find and click "Cancel" button
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? cancelButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Cancel"));
        cancelButton.ShouldNotBeNull();
        cancelButton.Click();
        
        // Assert
        _fakeNavigationManager.Uri.ShouldContain("/operators");
    }

    [Fact]
    public void OperatorsEdit_UpdateOperator_Success_ShowsSuccessMessageAndNavigates()
    {
        // Arrange
        var operatorId = Guid.NewGuid();
        var operatorModel = new OperatorModels.OperatorModel
        {
            OperatorId = operatorId,
            Name = "Test Operator",
            RequireCustomMerchantNumber = true,
            RequireCustomTerminalNumber = false
        };
        
        this.OperatorUIService.Setup(o => o.GetOperator(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(operatorModel));
        
        this.OperatorUIService.Setup(o => o.UpdateOperator(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<OperatorModels.EditOperatorModel>()))
            .ReturnsAsync(Result.Success());
        
        var cut = RenderComponent<OperatorsEdit>(parameters => parameters
            .Add(p => p.OperatorId, operatorId));
        
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act - Find and click "Update Operator" button
        var buttons = cut.FindAll("button");
        var updateButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Update Operator"));
        updateButton.ShouldNotBeNull();
        updateButton.Click();
        
        // Assert - Check for success message
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Operator updated successfully"), timeout: TimeSpan.FromSeconds(5));
        
        // Assert - Check navigation
        cut.WaitForAssertion(() => _fakeNavigationManager.Uri.ShouldContain("/operators"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void OperatorsEdit_UpdateOperator_Failure_ShowsErrorMessage()
    {
        // Arrange
        var operatorId = Guid.NewGuid();
        var operatorModel = new OperatorModels.OperatorModel
        {
            OperatorId = operatorId,
            Name = "Test Operator",
            RequireCustomMerchantNumber = true,
            RequireCustomTerminalNumber = false
        };
        
        this.OperatorUIService.Setup(o => o.GetOperator(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(operatorModel));
        
        this.OperatorUIService.Setup(o => o.UpdateOperator(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<OperatorModels.EditOperatorModel>()))
            .ReturnsAsync(Result.Failure());
        
        IRenderedComponent<OperatorsEdit> cut = RenderComponent<OperatorsEdit>(parameters => parameters
            .Add(p => p.OperatorId, operatorId));
        
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act - Find and click "Update Operator" button
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? updateButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Update Operator"));
        updateButton.ShouldNotBeNull();
        updateButton.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Failed to update operator"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void OperatorsEdit_LoadOperator_LoadFails_NavigatesToError()
    {
        // Arrange
        var operatorId = Guid.NewGuid();
        
        this.OperatorUIService.Setup(o => o.GetOperator(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Failure());
        
        // Act
        IRenderedComponent<OperatorsEdit> cut = RenderComponent<OperatorsEdit>(parameters => parameters
            .Add(p => p.OperatorId, operatorId));
        
        // Assert
        _fakeNavigationManager.Uri.ShouldContain("error");
    }

    [Fact]
    public void OperatorsEdit_OperatorNotFound_DisplaysMessage()
    {
        // Arrange
        var operatorId = Guid.NewGuid();
        
        this.OperatorUIService.Setup(o => o.GetOperator(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success<OperatorModels.OperatorModel>(null));
        
        // Act
        IRenderedComponent<OperatorsEdit> cut = RenderComponent<OperatorsEdit>(parameters => parameters
            .Add(p => p.OperatorId, operatorId));
        
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("Operator not found");
    }

    [Fact]
    public void OperatorsEdit_SubmitButton_DisplaysSavingState()
    {
        // Arrange
        var operatorId = Guid.NewGuid();
        var operatorModel = new OperatorModels.OperatorModel
        {
            OperatorId = operatorId,
            Name = "Test Operator",
            RequireCustomMerchantNumber = true,
            RequireCustomTerminalNumber = false
        };
        
        this.OperatorUIService.Setup(o => o.GetOperator(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(operatorModel));
        
        // Setup a delayed response to capture the saving state
        TaskCompletionSource<Result> tcs = new TaskCompletionSource<Result>();
        this.OperatorUIService.Setup(o => o.UpdateOperator(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<OperatorModels.EditOperatorModel>()))
            .Returns(tcs.Task);
        
        IRenderedComponent<OperatorsEdit> cut = RenderComponent<OperatorsEdit>(parameters => parameters
            .Add(p => p.OperatorId, operatorId));
        
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act - Find and click "Update Operator" button
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? updateButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Update Operator"));
        updateButton.ShouldNotBeNull();
        updateButton.Click();
        
        // Assert - Check for "Saving..." text while operation is in progress
        cut.Markup.ShouldContain("Saving...");
        
        // Complete the operation
        tcs.SetResult(Result.Success());
    }
}
