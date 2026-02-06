using AngleSharp.Dom;
using Bunit;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using Moq;
using Shouldly;
using SimpleResults;
using ContractsNew = EstateManagementUI.BlazorServer.Components.Pages.Contracts.New;

namespace EstateManagementUI.BlazorServer.Tests.Pages.Contracts;

public class ContractsNewPageTests : BaseTest
{
    [Fact]
    public void ContractsNew_RendersCorrectly()
    {
        // Arrange
        this.OperatorUIService.Setup(o => o.GetOperatorsForDropDown(It.IsAny<CorrelationId>(), It.IsAny<Guid>())).ReturnsAsync(Result.Success(new List<OperatorModels.OperatorDropDownModel>()));

        // Act
        IRenderedComponent<ContractsNew> cut = RenderComponent<ContractsNew>();

        // Assert
        cut.Markup.ShouldContain("Create New Contract");
    }

    [Fact]
    public void ContractsNew_HasCorrectPageTitle()
    {
        // Arrange
        this.OperatorUIService.Setup(o => o.GetOperatorsForDropDown(It.IsAny<CorrelationId>(), It.IsAny<Guid>())).ReturnsAsync(Result.Success(new List<OperatorModels.OperatorDropDownModel>()));

        // Act
        IRenderedComponent<ContractsNew> cut = RenderComponent<ContractsNew>();

        // Assert
        IRenderedComponent<Microsoft.AspNetCore.Components.Web.PageTitle> pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }

    [Fact]
    public void ContractsNew_CancelButton_NavigatesToContractsIndex()
    {
        // Arrange
        this.OperatorUIService.Setup(o => o.GetOperatorsForDropDown(It.IsAny<CorrelationId>(), It.IsAny<Guid>())).ReturnsAsync(Result.Success(new List<OperatorModels.OperatorDropDownModel>()));

        IRenderedComponent<ContractsNew> cut = RenderComponent<ContractsNew>();
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Create New Contract"), timeout: TimeSpan.FromSeconds(5));

        // Act - Find and click the Cancel button
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? cancelButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Cancel"));
        cancelButton.ShouldNotBeNull();
        cancelButton.Click();

        // Assert
        _fakeNavigationManager.Uri.ShouldContain("/contracts");
    }

    [Fact]
    public void ContractsNew_CreateContractButton_IsPresent()
    {
        // Arrange
        this.OperatorUIService.Setup(o => o.GetOperatorsForDropDown(It.IsAny<CorrelationId>(), It.IsAny<Guid>())).ReturnsAsync(Result.Success(new List<OperatorModels.OperatorDropDownModel>()));

        IRenderedComponent<ContractsNew> cut = RenderComponent<ContractsNew>();
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Create New Contract"), timeout: TimeSpan.FromSeconds(5));

        // Act & Assert
        IElement createButton = cut.Find("#createContractButton");
        createButton.ShouldNotBeNull();
    }

    [Fact]
    public void ContractsNew_LoadsOperatorsSuccessfully()
    {
        // Arrange
        List<OperatorModels.OperatorDropDownModel> operators = new List<OperatorModels.OperatorDropDownModel>
        {
            new OperatorModels.OperatorDropDownModel
            {
                OperatorId = Guid.NewGuid(),
                OperatorName = "Test Operator 1"
            },
            new OperatorModels.OperatorDropDownModel
            {
                OperatorId = Guid.NewGuid(),
                OperatorName = "Test Operator 2"
            }
        };
        this.OperatorUIService.Setup(o => o.GetOperatorsForDropDown(It.IsAny<CorrelationId>(), It.IsAny<Guid>())).ReturnsAsync(Result.Success(operators));

        // Act
        IRenderedComponent<ContractsNew> cut = RenderComponent<ContractsNew>();

        // Assert - Verify both operators are loaded and displayed in the dropdown
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Test Operator 1"), timeout: TimeSpan.FromSeconds(5));
        cut.Markup.ShouldContain("Test Operator 2");
    }

    [Fact]
    public void ContractsNew_LoadsOperatorsFailed_NavigateToError()
    {
        // Arrange
        this.OperatorUIService.Setup(o => o.GetOperatorsForDropDown(It.IsAny<CorrelationId>(), It.IsAny<Guid>())).ReturnsAsync(Result.Failure());

        // Act
        IRenderedComponent<ContractsNew> cut = RenderComponent<ContractsNew>();

        _fakeNavigationManager.Uri.ShouldContain("error");
    }

    [Fact]
    public void ContractsNew_WithNoOperators_ShowsNoOperatorsMessage()
    {
        // Arrange
        this.OperatorUIService.Setup(o => o.GetOperatorsForDropDown(It.IsAny<CorrelationId>(), It.IsAny<Guid>())).ReturnsAsync(Result.Success(new List<OperatorModels.OperatorDropDownModel>()));

        // Act
        IRenderedComponent<ContractsNew> cut = RenderComponent<ContractsNew>();

        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("No operators available"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void ContractsNew_SuccessfulCreation_NavigatesToContractsIndex()
    {
        // Arrange
        Guid operatorId = Guid.NewGuid();
        List<OperatorModels.OperatorDropDownModel> operators = new List<OperatorModels.OperatorDropDownModel>
        {
            new OperatorModels.OperatorDropDownModel
            {
                OperatorId = operatorId,
                OperatorName = "Test Operator"
            }
        };

        this.OperatorUIService.Setup(o => o.GetOperatorsForDropDown(It.IsAny<CorrelationId>(), It.IsAny<Guid>())).ReturnsAsync(Result.Success(operators));
        this.ContractUIService.Setup(c => c.CreateContract(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<ContractModels.CreateContractFormModel>()))
            .ReturnsAsync(Result.Success());

        IRenderedComponent<ContractsNew> cut = RenderComponent<ContractsNew>();
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Test Operator"), timeout: TimeSpan.FromSeconds(5));

        // Act - Fill in the form
        IElement descriptionInput = cut.Find("input[placeholder='Enter contract description']");
        descriptionInput.Change("Test Contract Description");

        IElement operatorSelect = cut.Find("select");
        operatorSelect.Change(operatorId.ToString());

        // Submit the form
        IElement createButton = cut.Find("#createContractButton");
        createButton.Click();

        // Assert
        cut.WaitForAssertion(() => {
            _fakeNavigationManager.Uri.ShouldContain("/contracts");
        }, timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void ContractsNew_FailedCreation_ShowsErrorMessage()
    {
        // Arrange
        Guid operatorId = Guid.NewGuid();
        List<OperatorModels.OperatorDropDownModel> operators = new List<OperatorModels.OperatorDropDownModel>
        {
            new OperatorModels.OperatorDropDownModel
            {
                OperatorId = operatorId,
                OperatorName = "Test Operator"
            }
        };

        this.OperatorUIService.Setup(o => o.GetOperatorsForDropDown(It.IsAny<CorrelationId>(), It.IsAny<Guid>())).ReturnsAsync(Result.Success(operators));
        this.ContractUIService.Setup(c => c.CreateContract(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<ContractModels.CreateContractFormModel>()))
            .ReturnsAsync(Result.Failure);

        IRenderedComponent<ContractsNew> cut = RenderComponent<ContractsNew>();
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Test Operator"), timeout: TimeSpan.FromSeconds(5));

        // Act - Fill in the form
        IElement descriptionInput = cut.Find("input[placeholder='Enter contract description']");
        descriptionInput.Change("Test Contract Description");

        IElement operatorSelect = cut.Find("select");
        operatorSelect.Change(operatorId.ToString());

        // Submit the form
        IElement createButton = cut.Find("#createContractButton");
        createButton.Click();

        // Assert
        cut.WaitForAssertion(() => {
            cut.Markup.ShouldContain("Failed to create contract");
        }, timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void ContractsNew_FormValidation_RequiresDescription()
    {
        // Arrange
        Guid operatorId = Guid.NewGuid();
        List<OperatorModels.OperatorDropDownModel> operators = new List<OperatorModels.OperatorDropDownModel>
        {
            new OperatorModels.OperatorDropDownModel
            {
                OperatorId = operatorId,
                OperatorName = "Test Operator"
            }
        };

        this.OperatorUIService.Setup(o => o.GetOperatorsForDropDown(It.IsAny<CorrelationId>(), It.IsAny<Guid>())).ReturnsAsync(Result.Success(operators));

        IRenderedComponent<ContractsNew> cut = RenderComponent<ContractsNew>();
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Test Operator"), timeout: TimeSpan.FromSeconds(5));

        // Act - Submit form without description
        IElement operatorSelect = cut.Find("select");
        operatorSelect.Change(operatorId.ToString());

        IElement createButton = cut.Find("#createContractButton");
        createButton.Click();

        // Assert - Validation message should appear
        cut.WaitForAssertion(() => {
            cut.Markup.ShouldContain("Description is required");
            // Should not call the mediator
        }, timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void ContractsNew_FormValidation_RequiresOperator()
    {
        // Arrange
        List<OperatorModels.OperatorDropDownModel> operators = new List<OperatorModels.OperatorDropDownModel>
        {
            new OperatorModels.OperatorDropDownModel
            {
                OperatorId = Guid.NewGuid(),
                OperatorName = "Test Operator"
            }
        };

        this.OperatorUIService.Setup(o => o.GetOperatorsForDropDown(It.IsAny<CorrelationId>(), It.IsAny<Guid>())).ReturnsAsync(Result.Success(operators));

        IRenderedComponent<ContractsNew> cut = RenderComponent<ContractsNew>();
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Test Operator"), timeout: TimeSpan.FromSeconds(5));

        // Act - Submit form without selecting operator
        IElement descriptionInput = cut.Find("input[placeholder='Enter contract description']");
        descriptionInput.Change("Test Contract Description");

        IElement createButton = cut.Find("#createContractButton");
        createButton.Click();

        // Assert - Validation message should appear
        cut.WaitForAssertion(() => {
            cut.Markup.ShouldContain("Operator is required");
        }, timeout: TimeSpan.FromSeconds(5));
    }
}
