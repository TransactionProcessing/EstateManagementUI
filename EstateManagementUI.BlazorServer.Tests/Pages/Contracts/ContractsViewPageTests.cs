using AngleSharp.Dom;
using Bunit;
using EstateManagementUI.BlazorServer.Components.Pages.Contracts;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BusinessLogic.Requests;
using Moq;
using Shouldly;
using SimpleResults;
using TransactionProcessor.DataTransferObjects.Responses.Contract;

namespace EstateManagementUI.BlazorServer.Tests.Pages.Contracts;

public class ContractsViewPageTests : BaseTest
{
    [Fact]
    public void ContractsView_RendersCorrectly()
    {
        // Arrange
        var contractId = Guid.NewGuid();
        var contract = new ContractModels.ContractModel
        {
            ContractId = contractId,
            Description = "Test Contract",
            OperatorName = "Test Operator"
        };
        
        this.ContractUIService.Setup(c => c.GetContract(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
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
        var contract = new ContractModels.ContractModel
        {
            ContractId = contractId,
            Description = "Test Contract",
            OperatorName = "Test Operator"
        };

        this.ContractUIService.Setup(c => c.GetContract(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
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
        var contract = new ContractModels.ContractModel
        {
            ContractId = contractId,
            Description = "Test Contract",
            OperatorName = "Test Operator"
        };
        this.ContractUIService.Setup(c => c.GetContract(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(contract));

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
        var contract = new ContractModels.ContractModel
        {
            ContractId = contractId,
            Description = "Test Contract",
            OperatorName = "Test Operator"
        };

        this.ContractUIService.Setup(c => c.GetContract(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
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
        var contract = new ContractModels.ContractModel
        {
            ContractId = contractId,
            Description = "Test Contract",
            OperatorName = "Test Operator"
        };

        this.ContractUIService.Setup(c => c.GetContract(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(contract));

        // Act
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.ContractId, contractId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Find and click the Back to List button
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? backButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Back to List"));
        backButton.ShouldNotBeNull();
        backButton.Click();
        
        // Assert
        _fakeNavigationManager.Uri.ShouldContain("/contracts");
    }

    [Fact]
    public void ContractsView_DisplaysProducts_WhenPresent()
    {
        // Arrange
        var contractId = Guid.NewGuid();
        var contract = new ContractModels.ContractModel
        {
            ContractId = contractId,
            Description = "Test Contract",
            OperatorName = "Test Operator",
            Products = new List<ContractModels.ContractProductModel>
            {
                new ContractModels.ContractProductModel
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

        this.ContractUIService.Setup(c => c.GetContract(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
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
        var contract = new ContractModels.ContractModel
        {
            ContractId = contractId,
            Description = "Test Contract",
            OperatorName = "Test Operator",
            Products = new List<ContractModels.ContractProductModel>()
        };

        this.ContractUIService.Setup(c => c.GetContract(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
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
        var contract = new ContractModels.ContractModel
        {
            ContractId = contractId,
            Description = "Test Contract",
            OperatorName = "Test Operator",
            Products = new List<ContractModels.ContractProductModel>
            {
                new ContractModels.ContractProductModel
                {
                    ContractProductId = Guid.NewGuid(),
                    ProductName = "Test Product",
                    DisplayText = "Test Display",
                    ProductType = ProductType.MobileTopup,
                    Value = "100",
                    NumberOfFees = 1,
                    TransactionFees = new List<ContractModels.ContractProductTransactionFeeModel>
                    {
                        new ContractModels.ContractProductTransactionFeeModel
                        {
                            TransactionFeeId = Guid.NewGuid(),
                            Description = "Service Fee",
                            CalculationType = 0,
                            FeeType = 0,
                            Value = 10.50m
                        }
                    }
                }
            }
        };

        this.ContractUIService.Setup(c => c.GetContract(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
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
        this.ContractUIService.Setup(c => c.GetContract(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Failure());

        // Act
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.ContractId, contractId));
        //cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        // Assert
        _fakeNavigationManager.Uri.ShouldContain("error");
    }
}
