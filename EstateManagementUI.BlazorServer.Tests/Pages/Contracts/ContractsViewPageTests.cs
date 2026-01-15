using Bunit;
using EstateManagementUI.BlazorServer.Components.Pages.Contracts;
using EstateManagementUI.BlazorServer.Components.Permissions;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.Tests.Pages.Contracts;

public class ContractsViewPageTests : TestContext
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<NavigationManager> _mockNavigationManager;
    private readonly Mock<IPermissionKeyProvider> _mockPermissionKeyProvider;

    public ContractsViewPageTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockNavigationManager = new Mock<NavigationManager>();
        _mockPermissionKeyProvider = new Mock<IPermissionKeyProvider>();
        
        _mockPermissionKeyProvider.Setup(x => x.GetKey()).Returns("test-key");
        
        Services.AddSingleton(_mockMediator.Object);
        Services.AddSingleton(_mockNavigationManager.Object);
        Services.AddSingleton(_mockPermissionKeyProvider.Object);
        
        ComponentFactories.AddStub<RequirePermission>();
    }

    [Fact]
    public void ContractsView_RendersCorrectly()
    {
        // Arrange
        var contractId = Guid.NewGuid();
        var contract = new ContractModel
        {
            ContractId = contractId,
            Description = "Test Contract",
            OperatorName = "Test Operator"
        };
        
        _mockMediator.Setup(x => x.Send(It.IsAny<Queries.GetContractQuery>(), default))
            .ReturnsAsync(Result.Success(contract));
        
        // Act
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.ContractId, contractId));
        
        // Assert
        cut.Markup.ShouldContain("View Contract");
    }

    [Fact]
    public void ContractsView_DisplaysContractDetails()
    {
        // Arrange
        var contractId = Guid.NewGuid();
        var contract = new ContractModel
        {
            ContractId = contractId,
            Description = "Test Contract",
            OperatorName = "Test Operator"
        };
        
        _mockMediator.Setup(x => x.Send(It.IsAny<Queries.GetContractQuery>(), default))
            .ReturnsAsync(Result.Success(contract));
        
        // Act
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.ContractId, contractId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("Test Contract");
    }

    [Fact]
    public void ContractsView_HasCorrectPageTitle()
    {
        // Arrange
        var contractId = Guid.NewGuid();
        _mockMediator.Setup(x => x.Send(It.IsAny<Queries.GetContractQuery>(), default))
            .ReturnsAsync(Result.Success(new ContractModel { ContractId = contractId }));
        
        // Act
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.ContractId, contractId));
        
        // Assert
        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }
}
