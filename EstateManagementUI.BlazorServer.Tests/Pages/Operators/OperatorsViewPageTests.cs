using Bunit;
using EstateManagementUI.BlazorServer.Components.Pages.Operators;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using SimpleResults;
using OperatorModel = EstateManagementUI.BusinessLogic.Models.OperatorModel;

namespace EstateManagementUI.BlazorServer.Tests.Pages.Operators;

public class OperatorsViewPageTests : TestContext
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<NavigationManager> _mockNavigationManager;

    public OperatorsViewPageTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockNavigationManager = new Mock<NavigationManager>();
        
        Services.AddSingleton(_mockMediator.Object);
        Services.AddSingleton(_mockNavigationManager.Object);
    }

    [Fact]
    public void OperatorsView_RendersCorrectly()
    {
        // Arrange
        var operatorId = Guid.NewGuid();
        var operatorModel = new OperatorModel
        {
            OperatorId = operatorId,
            Name = "Test Operator"
        };
        
        _mockMediator.Setup(x => x.Send(It.IsAny<Queries.GetOperatorQuery>(), default))
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
        var operatorModel = new OperatorModel
        {
            OperatorId = operatorId,
            Name = "Test Operator"
        };
        
        _mockMediator.Setup(x => x.Send(It.IsAny<Queries.GetOperatorQuery>(), default))
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
        _mockMediator.Setup(x => x.Send(It.IsAny<Queries.GetOperatorQuery>(), default))
            .ReturnsAsync(Result.Success(new OperatorModel { OperatorId = operatorId }));
        
        // Act
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.OperatorId, operatorId));
        
        // Assert
        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }
}
