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
        List<ContractModel> contracts = new List<ContractModel>
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
        IRenderedComponent<ContractsIndex> cut = RenderComponent<ContractsIndex>();
        
        // Assert
        cut.Markup.ShouldContain("Contract Management");
    }

    [Fact]
    public void ContractsIndex_WithNoContracts_ShowsEmptyState()
    {
        // Arrange
        List<ContractModel> emptyList = new List<ContractModel>();
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetContractsQuery>(), default))
            .ReturnsAsync(Result.Success(emptyList));
        
        // Act
        IRenderedComponent<ContractsIndex> cut = RenderComponent<ContractsIndex>();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("No contracts found"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void ContractsIndex_WithContracts_DisplaysContractList()
    {
        // Arrange
        List<ContractModel> contracts = new List<ContractModel>
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
        IRenderedComponent<ContractsIndex> cut = RenderComponent<ContractsIndex>();
        
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
        List<ContractModel> contracts = new List<ContractModel>
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
        IRenderedComponent<ContractsIndex> cut = RenderComponent<ContractsIndex>();
        
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
        IRenderedComponent<ContractsIndex> cut = RenderComponent<ContractsIndex>();
        
        // Assert
        IRenderedComponent<Microsoft.AspNetCore.Components.Web.PageTitle> pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }

    [Fact]
    public void ContractsIndex_AddNewContractButton_NavigatesToNewContractPage()
    {
        // Arrange
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetContractsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<ContractModel>()));
        
        IRenderedComponent<ContractsIndex> cut = RenderComponent<ContractsIndex>();
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Contract Management"), timeout: TimeSpan.FromSeconds(5));
        
        // Act - Find and click the "Add New Contract" button
        IElement addButton = cut.Find("#newContractButton");
        addButton.Click();
        
        // Assert
        _fakeNavigationManager.Uri.ShouldContain("/contracts/new");
    }

    [Fact]
    public void ContractsIndex_ViewButton_NavigatesToContractDetails()
    {
        // Arrange
        Guid contractId = Guid.NewGuid();
        List<ContractModel> contracts = new List<ContractModel>
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
        
        IRenderedComponent<ContractsIndex> cut = RenderComponent<ContractsIndex>();
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Test Contract"), timeout: TimeSpan.FromSeconds(5));
        
        // Act - Find and click the "View" button
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? viewButton = buttons.FirstOrDefault(b => b.TextContent.Contains("View"));
        viewButton.ShouldNotBeNull();
        viewButton.Click();
        
        // Assert
        _fakeNavigationManager.Uri.ShouldContain($"/contracts/{contractId}");
    }

    [Fact]
    public void ContractsIndex_EditButton_NavigatesToEditContractPage()
    {
        // Arrange
        Guid contractId = Guid.NewGuid();
        List<ContractModel> contracts = new List<ContractModel>
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
        
        IRenderedComponent<ContractsIndex> cut = RenderComponent<ContractsIndex>();
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Test Contract"), timeout: TimeSpan.FromSeconds(5));
        
        // Act - Find and click the "Edit" button
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? editButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Edit"));
        editButton.ShouldNotBeNull();
        editButton.Click();
        
        // Assert
        _fakeNavigationManager.Uri.ShouldContain($"/contracts/{contractId}/edit");
    }
}
