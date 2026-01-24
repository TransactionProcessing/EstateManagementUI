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
using ContractsIndex = EstateManagementUI.BlazorServer.Components.Pages.Contracts.Index;

namespace EstateManagementUI.BlazorServer.Tests.Pages.Contracts;

public class ContractsIndexPageTests : BaseTest
{
    [Fact]
    public void ContractsIndex_RendersCorrectly()
    {
        // Arrange
        var contracts = new List<ContractModel>
        {
            new ContractModel
            {
                ContractId = Guid.NewGuid(),
                Description = "Test Contract",
                OperatorName = "Test Operator",
                OperatorId = Guid.NewGuid(),
                Products = new List<ContractProductModel>()
            }
        };
        
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetContractsQuery>(), default))
            .ReturnsAsync(Result.Success(contracts));
        
        // Act
        var cut = RenderComponent<ContractsIndex>();
        
        // Assert
        cut.Markup.ShouldContain("Contract Management");
    }

    [Fact]
    public void ContractsIndex_WithNoContracts_ShowsEmptyState()
    {
        // Arrange
        var emptyList = new List<ContractModel>();
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetContractsQuery>(), default))
            .ReturnsAsync(Result.Success(emptyList));
        
        // Act
        var cut = RenderComponent<ContractsIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"));
        
        // Assert
        cut.Markup.ShouldContain("No contracts found");
    }

    [Fact]
    public void ContractsIndex_WithContracts_DisplaysContractList()
    {
        // Arrange
        var contracts = new List<ContractModel>
        {
            new ContractModel
            {
                ContractId = Guid.NewGuid(),
                Description = "Contract 1",
                OperatorName = "Operator A",
                OperatorId = Guid.NewGuid(),
                Products = new List<ContractProductModel>()
            },
            new ContractModel
            {
                ContractId = Guid.NewGuid(),
                Description = "Contract 2",
                OperatorName = "Operator B",
                OperatorId = Guid.NewGuid(),
                Products = new List<ContractProductModel>()
            }
        };
        
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetContractsQuery>(), default))
            .ReturnsAsync(Result.Success(contracts));
        
        // Act
        var cut = RenderComponent<ContractsIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("Contract 1");
        cut.Markup.ShouldContain("Contract 2");
        cut.Markup.ShouldContain("Operator A");
        cut.Markup.ShouldContain("Operator B");
    }

    [Fact]
    public void ContractsIndex_DisplaysProductCount()
    {
        // Arrange
        var contracts = new List<ContractModel>
        {
            new ContractModel
            {
                ContractId = Guid.NewGuid(),
                Description = "Test Contract",
                OperatorName = "Test Operator",
                OperatorId = Guid.NewGuid(),
                Products = new List<ContractProductModel>
                {
                    new ContractProductModel { ContractProductId = Guid.NewGuid(), ProductName = "Product 1" },
                    new ContractProductModel { ContractProductId = Guid.NewGuid(), ProductName = "Product 2" }
                }
            }
        };
        
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetContractsQuery>(), default))
            .ReturnsAsync(Result.Success(contracts));
        
        // Act
        var cut = RenderComponent<ContractsIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("Product(s)");
    }

    [Fact]
    public void ContractsIndex_HasCorrectPageTitle()
    {
        // Arrange
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetContractsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<ContractModel>()));
        
        // Act
        var cut = RenderComponent<ContractsIndex>();
        
        // Assert
        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }
}
