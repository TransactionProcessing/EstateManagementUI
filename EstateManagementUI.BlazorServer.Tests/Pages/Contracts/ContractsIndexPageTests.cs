using AngleSharp.Dom;
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
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("No contracts found"), timeout: TimeSpan.FromSeconds(5));
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
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Contract 1"), timeout: TimeSpan.FromSeconds(5));
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
                    new ContractProductModel { ContractProductId = Guid.NewGuid(), ProductName = "Product 1", ProductType = "NotSet"},
                    new ContractProductModel { ContractProductId = Guid.NewGuid(), ProductName = "Product 2", ProductType = "NotSet" }
                }
            }
        };
        
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetContractsQuery>(), default))
            .ReturnsAsync(Result.Success(contracts));
        
        // Act
        var cut = RenderComponent<ContractsIndex>();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Product(s)"), timeout: TimeSpan.FromSeconds(5));
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

    [Fact]
    public void ContractsIndex_AddNewContractButton_NavigatesToNewContractPage()
    {
        // Arrange
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetContractsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<ContractModel>()));
        
        var cut = RenderComponent<ContractsIndex>();
        cut.WaitForAssertion(() => !cut.Markup.Contains("animate-spin"), timeout: TimeSpan.FromSeconds(5));
        
        // Act - Find and click the "Add New Contract" button
        var addButton = cut.Find("#newContractButton");
        addButton.Click();
        
        // Assert
        _fakeNavigationManager.Uri.ShouldContain("/contracts/new");
    }

    [Fact]
    public void ContractsIndex_ViewButton_NavigatesToContractDetails()
    {
        // Arrange
        var contractId = Guid.NewGuid();
        var contracts = new List<ContractModel>
        {
            new ContractModel
            {
                ContractId = contractId,
                Description = "Test Contract",
                OperatorName = "Test Operator",
                OperatorId = Guid.NewGuid(),
                Products = new List<ContractProductModel>()
            }
        };
        
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetContractsQuery>(), default))
            .ReturnsAsync(Result.Success(contracts));
        
        var cut = RenderComponent<ContractsIndex>();
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Test Contract"), timeout: TimeSpan.FromSeconds(5));
        
        // Act - Find and click the "View" button
        var buttons = cut.FindAll("button");
        var viewButton = buttons.FirstOrDefault(b => b.TextContent.Contains("View"));
        viewButton.ShouldNotBeNull();
        viewButton.Click();
        
        // Assert
        _fakeNavigationManager.Uri.ShouldContain($"/contracts/{contractId}");
    }

    [Fact]
    public void ContractsIndex_EditButton_NavigatesToEditContractPage()
    {
        // Arrange
        var contractId = Guid.NewGuid();
        var contracts = new List<ContractModel>
        {
            new ContractModel
            {
                ContractId = contractId,
                Description = "Test Contract",
                OperatorName = "Test Operator",
                OperatorId = Guid.NewGuid(),
                Products = new List<ContractProductModel>()
            }
        };
        
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetContractsQuery>(), default))
            .ReturnsAsync(Result.Success(contracts));
        
        var cut = RenderComponent<ContractsIndex>();
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Test Contract"), timeout: TimeSpan.FromSeconds(5));
        
        // Act - Find and click the "Edit" button
        var buttons = cut.FindAll("button");
        var editButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Edit"));
        editButton.ShouldNotBeNull();
        editButton.Click();
        
        // Assert
        _fakeNavigationManager.Uri.ShouldContain($"/contracts/{contractId}/edit");
    }
}
