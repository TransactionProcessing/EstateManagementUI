using AngleSharp.Dom;
using Bunit;
using EstateManagementUI.BlazorServer.Components.Permissions;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BlazorServer.Tests.Pages.FileProcessing;
using EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using SimpleResults;
using OperatorsIndex = EstateManagementUI.BlazorServer.Components.Pages.Operators.Index;

namespace EstateManagementUI.BlazorServer.Tests.Pages.Operators;

public class OperatorsIndexPageTests : BaseTest
{
    [Fact]
    public void OperatorsIndex_RendersCorrectly()
    {
        // Arrange
        var operators = new List<OperatorModels.OperatorModel>
        {
            new OperatorModels.OperatorModel
            {
                OperatorId = Guid.NewGuid(),
                Name = "Test Operator",
                RequireCustomMerchantNumber = true,
                RequireCustomTerminalNumber = false
            }
        };
        
        this.OperatorUIService.Setup(o => o.GetOperators(It.IsAny<CorrelationId>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(operators));
        
        // Act
        var cut = RenderComponent<OperatorsIndex>();
        
        // Assert
        cut.Markup.ShouldContain("Operator Management");
    }

    [Fact]
    public void OperatorsIndex_WithNoOperators_ShowsEmptyState()
    {
        // Arrange
        var emptyList = new List<OperatorModels.OperatorModel>();
        this.OperatorUIService.Setup(o => o.GetOperators(It.IsAny<CorrelationId>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(emptyList));

        // Act
        var cut = RenderComponent<OperatorsIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"));
        
        // Assert
        cut.Markup.ShouldContain("No operators found");
    }

    [Fact]
    public void OperatorsIndex_WithOperators_DisplaysOperatorList()
    {
        // Arrange
        var operators = new List<OperatorModels.OperatorModel>
        {
            new OperatorModels.OperatorModel
            {
                OperatorId = Guid.NewGuid(),
                Name = "Operator 1",
                RequireCustomMerchantNumber = true,
                RequireCustomTerminalNumber = false
            },
            new OperatorModels.OperatorModel
            {
                OperatorId = Guid.NewGuid(),
                Name = "Operator 2",
                RequireCustomMerchantNumber = false,
                RequireCustomTerminalNumber = true
            }
        };

        this.OperatorUIService.Setup(o => o.GetOperators(It.IsAny<CorrelationId>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(operators));

        // Act
        var cut = RenderComponent<OperatorsIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("Operator 1");
        cut.Markup.ShouldContain("Operator 2");
    }

    [Fact]
    public void OperatorsIndex_DisplaysCustomNumberRequirements()
    {
        // Arrange
        var operators = new List<OperatorModels.OperatorModel>
        {
            new OperatorModels.OperatorModel
            {
                OperatorId = Guid.NewGuid(),
                Name = "Test Operator",
                RequireCustomMerchantNumber = true,
                RequireCustomTerminalNumber = false
            }
        };

        this.OperatorUIService.Setup(o => o.GetOperators(It.IsAny<CorrelationId>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(operators));

        // Act
        var cut = RenderComponent<OperatorsIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("Custom Merchant Number");
        cut.Markup.ShouldContain("Custom Terminal Number");
    }

    [Fact]
    public void OperatorsIndex_HasCorrectPageTitle()
    {
        // Arrange
        this.OperatorUIService.Setup(o => o.GetOperators(It.IsAny<CorrelationId>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(new List<OperatorModels.OperatorModel>()));

        // Act
        var cut = RenderComponent<OperatorsIndex>();
        
        // Assert
        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }

    [Fact]
    public void OperatorsIndex_AddNewOperatorButton_NavigatesToNewOperatorPage()
    {
        // Arrange
        var operators = new List<OperatorModels.OperatorModel>
        {
            new OperatorModels.OperatorModel
            {
                OperatorId = Guid.NewGuid(),
                Name = "Test Operator",
                RequireCustomMerchantNumber = true,
                RequireCustomTerminalNumber = false
            }
        };

        this.OperatorUIService.Setup(o => o.GetOperators(It.IsAny<CorrelationId>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(operators));

        var cut = RenderComponent<OperatorsIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        // Act - Find and click "Add New Operator" button
        var buttons = cut.FindAll("button");
        var addNewOperatorButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Add New Operator"));
        addNewOperatorButton?.Click();

        // Assert
        _fakeNavigationManager.Uri.ShouldContain("/operators/new");
    }

    [Fact]
    public void OperatorsIndex_ViewButton_NavigatesToOperatorViewPage()
    {
        // Arrange
        var operatorId = Guid.NewGuid();
        var operators = new List<OperatorModels.OperatorModel>
        {
            new OperatorModels.OperatorModel
            {
                OperatorId = operatorId,
                Name = "Test Operator",
                RequireCustomMerchantNumber = true,
                RequireCustomTerminalNumber = false
            }
        };

        this.OperatorUIService.Setup(o => o.GetOperators(It.IsAny<CorrelationId>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(operators));

        var cut = RenderComponent<OperatorsIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        // Act - Find and click View button (button with eye icon/title="View")
        var buttons = cut.FindAll("button");
        var viewButton = buttons.FirstOrDefault(b => b.GetAttribute("title") == "View");
        viewButton?.Click();

        // Assert
        _fakeNavigationManager.Uri.ShouldContain($"/operators/{operatorId}");
    }

    [Fact]
    public void OperatorsIndex_EditButton_NavigatesToOperatorEditPage()
    {
        // Arrange
        var operatorId = Guid.NewGuid();
        var operators = new List<OperatorModels.OperatorModel>
        {
            new OperatorModels.OperatorModel
            {
                OperatorId = operatorId,
                Name = "Test Operator",
                RequireCustomMerchantNumber = true,
                RequireCustomTerminalNumber = false
            }
        };

        this.OperatorUIService.Setup(o => o.GetOperators(It.IsAny<CorrelationId>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(operators));

        var cut = RenderComponent<OperatorsIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        // Act - Find and click Edit button (button with title="Edit")
        var buttons = cut.FindAll("button");
        var editButton = buttons.FirstOrDefault(b => b.GetAttribute("title") == "Edit");
        editButton?.Click();

        // Assert
        _fakeNavigationManager.Uri.ShouldContain($"/operators/{operatorId}/edit");
    }

    [Fact]
    public void OperatorsIndex_TableRowClick_NavigatesToOperatorViewPage()
    {
        // Arrange
        var operatorId = Guid.NewGuid();
        var operators = new List<OperatorModels.OperatorModel>
        {
            new OperatorModels.OperatorModel
            {
                OperatorId = operatorId,
                Name = "Test Operator",
                RequireCustomMerchantNumber = true,
                RequireCustomTerminalNumber = false
            }
        };

        this.OperatorUIService.Setup(o => o.GetOperators(It.IsAny<CorrelationId>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(operators));

        var cut = RenderComponent<OperatorsIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        // Act - Find and click table row
        var tableRows = cut.FindAll("tr");
        var operatorRow = tableRows.FirstOrDefault(tr => tr.TextContent.Contains("Test Operator"));
        operatorRow?.Click();

        // Assert
        _fakeNavigationManager.Uri.ShouldContain($"/operators/{operatorId}");
    }

    [Fact]
    public void OperatorsIndex_LoadOperators_LoadFails_NavigatesToError()
    {
        // Arrange
        this.OperatorUIService.Setup(o => o.GetOperators(It.IsAny<CorrelationId>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Failure());

        // Act
        var cut = RenderComponent<OperatorsIndex>();

        // Assert
        _fakeNavigationManager.Uri.ShouldContain("error");
    }
}
