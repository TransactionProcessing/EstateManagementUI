using AngleSharp.Dom;
using Bunit;
using EstateManagementUI.BlazorServer.Components.Pages.Operators;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Tests.Pages.FileProcessing;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.Tests.Pages.Operators;

public class OperatorsViewPageTests : BaseTest
{
    [Fact]
    public void OperatorsView_RendersCorrectly()
    {
        // Arrange
        var operatorId = Guid.NewGuid();
        var operatorModel = new OperatorModels.OperatorModel
        {
            OperatorId = operatorId,
            Name = "Test Operator"
        };

        this.OperatorUIService.Setup(o => o.GetOperator(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(operatorModel));

        // Act
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.OperatorId, operatorId));
        
        // Assert
        cut.Markup.ShouldContain("View Operator");
    }

    [Fact]
    public void OperatorsView_DisplaysOperatorName()
    {
        // Arrange
        var operatorId = Guid.NewGuid();
        var operatorModel = new OperatorModels.OperatorModel
        {
            OperatorId = operatorId,
            Name = "Test Operator"
        };

        this.OperatorUIService.Setup(o => o.GetOperator(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(operatorModel));

        // Act
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.OperatorId, operatorId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("Test Operator");
    }

    [Fact]
    public void OperatorsView_HasCorrectPageTitle()
    {
        // Arrange
        var operatorId = Guid.NewGuid();
        this.OperatorUIService.Setup(o => o.GetOperator(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(new OperatorModels.OperatorModel { OperatorId = operatorId }));
        
        // Act
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.OperatorId, operatorId));
        
        // Assert
        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }

    [Fact]
    public void OperatorsView_BackToListButton_NavigatesToOperatorsIndex()
    {
        // Arrange
        var operatorId = Guid.NewGuid();
        var operatorModel = new OperatorModels.OperatorModel
        {
            OperatorId = operatorId,
            Name = "Test Operator"
        };

        this.OperatorUIService.Setup(o => o.GetOperator(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(operatorModel));

        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.OperatorId, operatorId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        // Act - Find and click "Back to List" button
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? backButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Back to List"));
        backButton?.Click();

        // Assert
        _fakeNavigationManager.Uri.ShouldContain("/operators");
    }

    [Fact]
    public void OperatorsView_DisplaysRequireCustomMerchantNumber_WhenRequired()
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
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.OperatorId, operatorId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        // Assert
        cut.Markup.ShouldContain("Required");
        cut.Markup.ShouldContain("Require Custom Merchant Number");
    }

    [Fact]
    public void OperatorsView_DisplaysRequireCustomMerchantNumber_WhenNotRequired()
    {
        // Arrange
        var operatorId = Guid.NewGuid();
        var operatorModel = new OperatorModels.OperatorModel
        {
            OperatorId = operatorId,
            Name = "Test Operator",
            RequireCustomMerchantNumber = false,
            RequireCustomTerminalNumber = false
        };

        this.OperatorUIService.Setup(o => o.GetOperator(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(operatorModel));

        // Act
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.OperatorId, operatorId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        // Assert
        cut.Markup.ShouldContain("Not Required");
        cut.Markup.ShouldContain("Require Custom Merchant Number");
    }

    [Fact]
    public void OperatorsView_DisplaysRequireCustomTerminalNumber_WhenRequired()
    {
        // Arrange
        var operatorId = Guid.NewGuid();
        var operatorModel = new OperatorModels.OperatorModel
        {
            OperatorId = operatorId,
            Name = "Test Operator",
            RequireCustomMerchantNumber = false,
            RequireCustomTerminalNumber = true
        };

        this.OperatorUIService.Setup(o => o.GetOperator(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(operatorModel));

        // Act
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.OperatorId, operatorId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        // Assert
        cut.Markup.ShouldContain("Required");
        cut.Markup.ShouldContain("Require Custom Terminal Number");
    }

    [Fact]
    public void OperatorsView_DisplaysRequireCustomTerminalNumber_WhenNotRequired()
    {
        // Arrange
        var operatorId = Guid.NewGuid();
        var operatorModel = new OperatorModels.OperatorModel
        {
            OperatorId = operatorId,
            Name = "Test Operator",
            RequireCustomMerchantNumber = false,
            RequireCustomTerminalNumber = false
        };

        this.OperatorUIService.Setup(o => o.GetOperator(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(operatorModel));

        // Act
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.OperatorId, operatorId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        // Assert
        cut.Markup.ShouldContain("Not Required");
        cut.Markup.ShouldContain("Require Custom Terminal Number");
    }

    [Fact]
    public void OperatorsView_LoadOperator_LoadFails_NavigatesToError()
    {
        // Arrange
        var operatorId = Guid.NewGuid();
        this.OperatorUIService.Setup(o => o.GetOperator(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Failure());

        // Act
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.OperatorId, operatorId));

        // Assert
        _fakeNavigationManager.Uri.ShouldContain("error");
    }

    [Fact]
    public void OperatorsView_DisplaysOperatorNotFound_WhenOperatorIsNull()
    {
        // Arrange
        var operatorId = Guid.NewGuid();
        this.OperatorUIService.Setup(o => o.GetOperator(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success<OperatorModels.OperatorModel>(null));

        // Act
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.OperatorId, operatorId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        // Assert
        cut.Markup.ShouldContain("Operator not found");
    }

    [Fact]
    public void OperatorsView_OperatorNotFoundBackButton_NavigatesToOperatorsIndex()
    {
        // Arrange
        var operatorId = Guid.NewGuid();
        this.OperatorUIService.Setup(o => o.GetOperator(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success<OperatorModels.OperatorModel>(null));

        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.OperatorId, operatorId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        // Act - Find and click "Back to List" button
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? backButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Back to List"));
        backButton?.Click();

        // Assert
        _fakeNavigationManager.Uri.ShouldContain("/operators");
    }
}
