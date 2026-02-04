using Bunit;
using EstateManagementUI.BlazorServer.Components.Pages.Contracts;
using EstateManagementUI.BlazorServer.Components.Permissions;
using EstateManagementUI.BlazorServer.Models;
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

namespace EstateManagementUI.BlazorServer.Tests.Pages.Contracts;

public class ContractsViewPageTests : BaseTest
{
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
        
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetContractQuery>(), default))
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
        
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetContractQuery>(), default))
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
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetContractQuery>(), default))
            .ReturnsAsync(Result.Success(new ContractModel { ContractId = contractId }));
        
        // Act
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.ContractId, contractId));
        
        // Assert
        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }

    [Fact]
    public void ContractsView_HasBackButton()
    {
        // Arrange
        var contractId = Guid.NewGuid();
        var contract = new ContractModel
        {
            ContractId = contractId,
            Description = "Test Contract",
            OperatorName = "Test Operator"
        };
        
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetContractQuery>(), default))
            .ReturnsAsync(Result.Success(contract));
        
        // Act
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.ContractId, contractId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("Back to List");
    }

    [Fact]
    public void ContractsView_BackButton_NavigatesToContractsList()
    {
        // Arrange
        var contractId = Guid.NewGuid();
        var contract = new ContractModel
        {
            ContractId = contractId,
            Description = "Test Contract",
            OperatorName = "Test Operator"
        };
        
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetContractQuery>(), default))
            .ReturnsAsync(Result.Success(contract));
        
        // Act
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.ContractId, contractId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Find and click the Back to List button
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? backButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Back to List"));
        backButton?.Click();
        
        // Assert
        _fakeNavigationManager.Uri.ShouldContain("/contracts");
    }

    [Fact]
    public void ContractsView_DisplaysProducts_WhenPresent()
    {
        // Arrange
        var contractId = Guid.NewGuid();
        var contract = new ContractModel
        {
            ContractId = contractId,
            Description = "Test Contract",
            OperatorName = "Test Operator",
            Products = new List<ContractProductModel>
            {
                new ContractProductModel
                {
                    ContractProductId = Guid.NewGuid(),
                    ProductName = "Test Product",
                    DisplayText = "Test Display",
                    ProductType = ProductType.MobileTopup,
                    Value = "100",
                    NumberOfFees = 2
                }
            }
        };
        
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetContractQuery>(), default))
            .ReturnsAsync(Result.Success(contract));
        
        // Act
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.ContractId, contractId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("Test Product");
        cut.Markup.ShouldContain("Test Display");
        cut.Markup.ShouldContain("100");
    }

    [Fact]
    public void ContractsView_DisplaysNoProducts_WhenEmpty()
    {
        // Arrange
        var contractId = Guid.NewGuid();
        var contract = new ContractModel
        {
            ContractId = contractId,
            Description = "Test Contract",
            OperatorName = "Test Operator",
            Products = new List<ContractProductModel>()
        };
        
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetContractQuery>(), default))
            .ReturnsAsync(Result.Success(contract));
        
        // Act
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.ContractId, contractId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("No products added to this contract yet");
    }

    [Fact]
    public void ContractsView_DisplaysTransactionFees_WhenPresent()
    {
        // Arrange
        var contractId = Guid.NewGuid();
        var contract = new ContractModel
        {
            ContractId = contractId,
            Description = "Test Contract",
            OperatorName = "Test Operator",
            Products = new List<ContractProductModel>
            {
                new ContractProductModel
                {
                    ContractProductId = Guid.NewGuid(),
                    ProductName = "Test Product",
                    DisplayText = "Test Display",
                    ProductType = ProductType.MobileTopup,
                    Value = "100",
                    NumberOfFees = 1,
                    TransactionFees = new List<ContractProductTransactionFeeModel>
                    {
                        new ContractProductTransactionFeeModel
                        {
                            TransactionFeeId = Guid.NewGuid(),
                            Description = "Service Fee",
                            CalculationType = CalculationType.Fixed,
                            FeeType = FeeType.ServiceProvider,
                            Value = 10.50m
                        }
                    }
                }
            }
        };
        
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetContractQuery>(), default))
            .ReturnsAsync(Result.Success(contract));
        
        // Act
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.ContractId, contractId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("Transaction Fees");
        cut.Markup.ShouldContain("Service Fee");
        cut.Markup.ShouldContain("10.50");
    }

    [Fact]
    public void ContractsView_LoadContract_QueryFails_ShowsNotFoundMessage()
    {
        // Arrange
        var contractId = Guid.NewGuid();
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetContractQuery>(), default))
            .ReturnsAsync(Result.Failure("Failed to load contract"));
        
        // Act
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.ContractId, contractId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("Contract not found");
    }

    [Fact]
    public void ContractsView_LoadContract_ReturnsNull_ShowsNotFoundMessage()
    {
        // Arrange
        var contractId = Guid.NewGuid();
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetContractQuery>(), default))
            .ReturnsAsync(Result.Success<ContractModel>(null!));
        
        // Act
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.ContractId, contractId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("Contract not found");
    }

    [Fact]
    public void ContractsView_ContractNotFound_HasBackButton()
    {
        // Arrange
        var contractId = Guid.NewGuid();
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetContractQuery>(), default))
            .ReturnsAsync(Result.Success<ContractModel>(null!));
        
        // Act
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.ContractId, contractId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("Back to List");
    }

    [Fact]
    public void ContractsView_ContractNotFound_BackButton_NavigatesToContractsList()
    {
        // Arrange
        var contractId = Guid.NewGuid();
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetContractQuery>(), default))
            .ReturnsAsync(Result.Success<ContractModel>(null!));
        
        // Act
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.ContractId, contractId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Find and click the Back to List button
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? backButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Back to List"));
        backButton?.Click();
        
        // Assert
        _fakeNavigationManager.Uri.ShouldContain("/contracts");
    }
}
