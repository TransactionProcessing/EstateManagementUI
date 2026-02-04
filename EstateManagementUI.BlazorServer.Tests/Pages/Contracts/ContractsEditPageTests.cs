using AngleSharp.Dom;
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

public class ContractsEditPageTests : BaseTest
{
    [Fact]
    public void ContractsEdit_RendersCorrectly()
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
        var cut = RenderComponent<Edit>(parameters => parameters
            .Add(p => p.ContractId, contractId));
        
        // Assert
        cut.Markup.ShouldContain("Edit Contract");
    }

    [Fact]
    public void ContractsEdit_DisplaysContractDetails()
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
        var cut = RenderComponent<Edit>(parameters => parameters
            .Add(p => p.ContractId, contractId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("Test Contract");
        cut.Markup.ShouldContain("Test Operator");
    }

    [Fact]
    public void ContractsEdit_HasCorrectPageTitle()
    {
        // Arrange
        var contractId = Guid.NewGuid();
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetContractQuery>(), default))
            .ReturnsAsync(Result.Success(new ContractModel 
            { 
                ContractId = contractId,
                Products = new List<ContractProductModel>()
            }));
        
        // Act
        var cut = RenderComponent<Edit>(parameters => parameters
            .Add(p => p.ContractId, contractId));
        
        // Assert
        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }

    [Fact]
    public void ContractsEdit_CancelButton_NavigatesToViewPage()
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
        var cut = RenderComponent<Edit>(parameters => parameters
            .Add(p => p.ContractId, contractId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Find and click Cancel button
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? cancelButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Cancel"));
        cancelButton.ShouldNotBeNull();
        cancelButton.Click();
        
        // Assert
        _fakeNavigationManager.Uri.ShouldContain($"/contracts/{contractId}");
    }

    [Fact]
    public void ContractsEdit_UpdateContractButton_CanBeFound()
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
        var cut = RenderComponent<Edit>(parameters => parameters
            .Add(p => p.ContractId, contractId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Find Update Contract button
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? updateButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Update Contract"));
        
        // Assert
        updateButton.ShouldNotBeNull();
    }

    [Fact]
    public void ContractsEdit_AddProductButton_CanBeFound()
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
        var cut = RenderComponent<Edit>(parameters => parameters
            .Add(p => p.ContractId, contractId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Find Add Product button
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? addProductButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Add Product"));
        
        // Assert
        addProductButton.ShouldNotBeNull();
    }

    [Fact]
    public void ContractsEdit_AddProductButton_OpensModal()
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
        var cut = RenderComponent<Edit>(parameters => parameters
            .Add(p => p.ContractId, contractId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Find and click Add Product button
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? addProductButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Add Product"));
        addProductButton.ShouldNotBeNull();
        addProductButton.Click();
        
        // Assert - Modal should be visible
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Add New Product"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void ContractsEdit_WithProducts_DisplaysProductList()
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
                    ProductName = "Product 1",
                    DisplayText = "Display 1",
                    ProductType = "Mobile",
                    Value = "100",
                    NumberOfFees = 2,
                    TransactionFees = new List<TransactionFeeModel>()
                }
            }
        };
        
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetContractQuery>(), default))
            .ReturnsAsync(Result.Success(contract));
        
        // Act
        var cut = RenderComponent<Edit>(parameters => parameters
            .Add(p => p.ContractId, contractId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("Product 1");
        cut.Markup.ShouldContain("Display 1");
    }

    [Fact]
    public void ContractsEdit_WithProducts_HasRemoveProductButton()
    {
        // Arrange
        var contractId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var contract = new ContractModel
        {
            ContractId = contractId,
            Description = "Test Contract",
            OperatorName = "Test Operator",
            Products = new List<ContractProductModel>
            {
                new ContractProductModel
                {
                    ContractProductId = productId,
                    ProductName = "Product 1",
                    DisplayText = "Display 1",
                    ProductType = "Mobile",
                    Value = "100",
                    NumberOfFees = 0,
                    TransactionFees = new List<TransactionFeeModel>()
                }
            }
        };
        
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetContractQuery>(), default))
            .ReturnsAsync(Result.Success(contract));
        
        // Act
        var cut = RenderComponent<Edit>(parameters => parameters
            .Add(p => p.ContractId, contractId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Find remove button (it has a trash icon svg)
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        var removeButton = buttons.FirstOrDefault(b => 
            b.GetAttribute("title") == "Remove Product");
        
        // Assert
        removeButton.ShouldNotBeNull();
    }

    [Fact]
    public void ContractsEdit_WithProductHavingFees_HasAddFeeButton()
    {
        // Arrange
        var contractId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var contract = new ContractModel
        {
            ContractId = contractId,
            Description = "Test Contract",
            OperatorName = "Test Operator",
            Products = new List<ContractProductModel>
            {
                new ContractProductModel
                {
                    ContractProductId = productId,
                    ProductName = "Product 1",
                    DisplayText = "Display 1",
                    ProductType = "Mobile",
                    Value = "100",
                    NumberOfFees = 1,
                    TransactionFees = new List<TransactionFeeModel>
                    {
                        new TransactionFeeModel
                        {
                            TransactionFeeId = Guid.NewGuid(),
                            Description = "Fee 1",
                            Value = 1.5m,
                            CalculationType = "Fixed",
                            FeeType = "Merchant"
                        }
                    }
                }
            }
        };
        
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetContractQuery>(), default))
            .ReturnsAsync(Result.Success(contract));
        
        // Act
        var cut = RenderComponent<Edit>(parameters => parameters
            .Add(p => p.ContractId, contractId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Find Add Fee button
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? addFeeButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Add Fee"));
        
        // Assert
        addFeeButton.ShouldNotBeNull();
        cut.Markup.ShouldContain("Fee 1");
    }

    [Fact]
    public void ContractsEdit_AddFeeButton_OpensModal()
    {
        // Arrange
        var contractId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var contract = new ContractModel
        {
            ContractId = contractId,
            Description = "Test Contract",
            OperatorName = "Test Operator",
            Products = new List<ContractProductModel>
            {
                new ContractProductModel
                {
                    ContractProductId = productId,
                    ProductName = "Product 1",
                    DisplayText = "Display 1",
                    ProductType = "Mobile",
                    Value = "100",
                    NumberOfFees = 0,
                    TransactionFees = new List<TransactionFeeModel>()
                }
            }
        };
        
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetContractQuery>(), default))
            .ReturnsAsync(Result.Success(contract));
        
        // Act
        var cut = RenderComponent<Edit>(parameters => parameters
            .Add(p => p.ContractId, contractId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Find and click Add Fee button
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? addFeeButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Add Fee"));
        addFeeButton.ShouldNotBeNull();
        addFeeButton.Click();
        
        // Assert - Modal should be visible
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Add Transaction Fee"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void ContractsEdit_WithNoProducts_ShowsEmptyState()
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
        var cut = RenderComponent<Edit>(parameters => parameters
            .Add(p => p.ContractId, contractId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("No products added yet");
    }

    [Fact]
    public void ContractsEdit_ContractNotFound_ShowsErrorMessage()
    {
        // Arrange
        var contractId = Guid.NewGuid();
        
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetContractQuery>(), default))
            .ReturnsAsync(Result.Success<ContractModel>(null!));
        
        // Act
        var cut = RenderComponent<Edit>(parameters => parameters
            .Add(p => p.ContractId, contractId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("Contract not found");
    }

    [Fact]
    public void ContractsEdit_ContractNotFound_HasBackToListButton()
    {
        // Arrange
        var contractId = Guid.NewGuid();
        
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetContractQuery>(), default))
            .ReturnsAsync(Result.Success<ContractModel>(null!));
        
        // Act
        var cut = RenderComponent<Edit>(parameters => parameters
            .Add(p => p.ContractId, contractId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Find Back to List button
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? backButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Back to List"));
        
        // Assert
        backButton.ShouldNotBeNull();
    }

    [Fact]
    public void ContractsEdit_BackToListButton_NavigatesToContractsIndex()
    {
        // Arrange
        var contractId = Guid.NewGuid();
        
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetContractQuery>(), default))
            .ReturnsAsync(Result.Success<ContractModel>(null!));
        
        // Act
        var cut = RenderComponent<Edit>(parameters => parameters
            .Add(p => p.ContractId, contractId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Find and click Back to List button
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? backButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Back to List"));
        backButton.ShouldNotBeNull();
        backButton.Click();
        
        // Assert
        _fakeNavigationManager.Uri.ShouldContain("/contracts");
    }

    [Fact]
    public void ContractsEdit_ProductModal_HasCancelButton()
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
        var cut = RenderComponent<Edit>(parameters => parameters
            .Add(p => p.ContractId, contractId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Open the modal
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? addProductButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Add Product"));
        addProductButton?.Click();
        
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Add New Product"), timeout: TimeSpan.FromSeconds(5));
        
        // Find cancel button in modal (type="button" excludes submit buttons)
        IRefreshableElementCollection<IElement> modalButtons = cut.FindAll("button");
        IElement? cancelButton = modalButtons.FirstOrDefault(b => 
            b.TextContent.Contains("Cancel") && 
            b.GetAttribute("type") == "button");
        
        // Assert
        cancelButton.ShouldNotBeNull();
    }

    [Fact]
    public void ContractsEdit_FeeModal_HasCancelButton()
    {
        // Arrange
        var contractId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var contract = new ContractModel
        {
            ContractId = contractId,
            Description = "Test Contract",
            OperatorName = "Test Operator",
            Products = new List<ContractProductModel>
            {
                new ContractProductModel
                {
                    ContractProductId = productId,
                    ProductName = "Product 1",
                    DisplayText = "Display 1",
                    ProductType = "Mobile",
                    Value = "100",
                    NumberOfFees = 0,
                    TransactionFees = new List<TransactionFeeModel>()
                }
            }
        };
        
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetContractQuery>(), default))
            .ReturnsAsync(Result.Success(contract));
        
        // Act
        var cut = RenderComponent<Edit>(parameters => parameters
            .Add(p => p.ContractId, contractId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Open the modal
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? addFeeButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Add Fee"));
        addFeeButton?.Click();
        
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Add Transaction Fee"), timeout: TimeSpan.FromSeconds(5));
        
        // Find cancel button in modal (type="button" excludes submit buttons)
        IRefreshableElementCollection<IElement> modalButtons = cut.FindAll("button");
        IElement? cancelButton = modalButtons.FirstOrDefault(b => 
            b.TextContent.Contains("Cancel") && 
            b.GetAttribute("type") == "button");
        
        // Assert
        cancelButton.ShouldNotBeNull();
    }

    [Fact]
    public void ContractsEdit_WithTransactionFees_HasRemoveFeeButton()
    {
        // Arrange
        var contractId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var feeId = Guid.NewGuid();
        var contract = new ContractModel
        {
            ContractId = contractId,
            Description = "Test Contract",
            OperatorName = "Test Operator",
            Products = new List<ContractProductModel>
            {
                new ContractProductModel
                {
                    ContractProductId = productId,
                    ProductName = "Product 1",
                    DisplayText = "Display 1",
                    ProductType = "Mobile",
                    Value = "100",
                    NumberOfFees = 1,
                    TransactionFees = new List<TransactionFeeModel>
                    {
                        new TransactionFeeModel
                        {
                            TransactionFeeId = feeId,
                            Description = "Fee 1",
                            Value = 1.5m,
                            CalculationType = "Fixed",
                            FeeType = "Merchant"
                        }
                    }
                }
            }
        };
        
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetContractQuery>(), default))
            .ReturnsAsync(Result.Success(contract));
        
        // Act
        var cut = RenderComponent<Edit>(parameters => parameters
            .Add(p => p.ContractId, contractId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Find remove fee button (it has a trash icon svg and title)
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        var removeFeeButton = buttons.FirstOrDefault(b => 
            b.GetAttribute("title") == "Remove Fee");
        
        // Assert
        removeFeeButton.ShouldNotBeNull();
    }

    [Fact]
    public void ContractsEdit_DisplaysOperatorNameAsReadOnly()
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
        var cut = RenderComponent<Edit>(parameters => parameters
            .Add(p => p.ContractId, contractId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("Test Operator");
        cut.Markup.ShouldContain("Operator cannot be changed after contract creation");
    }
}
