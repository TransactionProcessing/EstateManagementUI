using Bunit;
using EstateManagementUI.BlazorServer.Components.Permissions;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BlazorServer.Tests.Pages.FileProcessing;
using EstateManagementUI.BusinessLogic.Models;
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
        var operators = new List<OperatorModel>
        {
            new OperatorModel
            {
                OperatorId = Guid.NewGuid(),
                Name = "Test Operator",
                RequireCustomMerchantNumber = true,
                RequireCustomTerminalNumber = false
            }
        };
        
        _mockMediator.Setup(x => x.Send(It.IsAny<OperatorQueries.GetOperatorsQuery>(), default))
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
        var emptyList = new List<OperatorModel>();
        _mockMediator.Setup(x => x.Send(It.IsAny<OperatorQueries.GetOperatorsQuery>(), default))
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
        var operators = new List<OperatorModel>
        {
            new OperatorModel
            {
                OperatorId = Guid.NewGuid(),
                Name = "Operator 1",
                RequireCustomMerchantNumber = true,
                RequireCustomTerminalNumber = false
            },
            new OperatorModel
            {
                OperatorId = Guid.NewGuid(),
                Name = "Operator 2",
                RequireCustomMerchantNumber = false,
                RequireCustomTerminalNumber = true
            }
        };
        
        _mockMediator.Setup(x => x.Send(It.IsAny<OperatorQueries.GetOperatorsQuery>(), default))
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
        var operators = new List<OperatorModel>
        {
            new OperatorModel
            {
                OperatorId = Guid.NewGuid(),
                Name = "Test Operator",
                RequireCustomMerchantNumber = true,
                RequireCustomTerminalNumber = false
            }
        };
        
        _mockMediator.Setup(x => x.Send(It.IsAny<OperatorQueries.GetOperatorsQuery>(), default))
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
        _mockMediator.Setup(x => x.Send(It.IsAny<OperatorQueries.GetOperatorsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorModel>()));
        
        // Act
        var cut = RenderComponent<OperatorsIndex>();
        
        // Assert
        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }
}
