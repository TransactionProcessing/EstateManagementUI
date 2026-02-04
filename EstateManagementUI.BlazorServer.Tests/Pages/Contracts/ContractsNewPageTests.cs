using AngleSharp.Dom;
using Bunit;
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
        _mockMediator.Setup(x => x.Send(It.IsAny<OperatorQueries.GetOperatorsForDropDownQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorDropDownModel>()));

        // Act
        IRenderedComponent<ContractsNew> cut = RenderComponent<ContractsNew>();

        // Assert
        cut.Markup.ShouldContain("Create New Contract");
    }

    [Fact]
    public void ContractsNew_HasCorrectPageTitle()
    {
        // Arrange
        _mockMediator.Setup(x => x.Send(It.IsAny<OperatorQueries.GetOperatorsForDropDownQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorDropDownModel>()));

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
        _mockMediator.Setup(x => x.Send(It.IsAny<OperatorQueries.GetOperatorsForDropDownQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorDropDownModel>()));

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
        _mockMediator.Setup(x => x.Send(It.IsAny<OperatorQueries.GetOperatorsForDropDownQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorDropDownModel>()));

        // Act
        IRenderedComponent<ContractsNew> cut = RenderComponent<ContractsNew>();

        // Assert
        cut.WaitForAssertion(() => {
            IElement createButton = cut.Find("#createContractButton");
            createButton.ShouldNotBeNull();
        }, timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void ContractsNew_LoadsOperatorsSuccessfully()
    {
        // Arrange
        List<OperatorDropDownModel> operators = new List<OperatorDropDownModel>
        {
            new OperatorDropDownModel
            {
                OperatorId = Guid.NewGuid(),
                OperatorName = "Test Operator 1"
            },
            new OperatorDropDownModel
            {
                OperatorId = Guid.NewGuid(),
                OperatorName = "Test Operator 2"
            }
        };

        _mockMediator.Setup(x => x.Send(It.IsAny<OperatorQueries.GetOperatorsForDropDownQuery>(), default))
            .ReturnsAsync(Result.Success(operators));

        // Act
        IRenderedComponent<ContractsNew> cut = RenderComponent<ContractsNew>();

        // Assert
        cut.WaitForAssertion(() => {
            cut.Markup.ShouldContain("Test Operator 1");
            cut.Markup.ShouldContain("Test Operator 2");
        }, timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void ContractsNew_WithNoOperators_ShowsNoOperatorsMessage()
    {
        // Arrange
        _mockMediator.Setup(x => x.Send(It.IsAny<OperatorQueries.GetOperatorsForDropDownQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorDropDownModel>()));

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
        List<OperatorDropDownModel> operators = new List<OperatorDropDownModel>
        {
            new OperatorDropDownModel
            {
                OperatorId = operatorId,
                OperatorName = "Test Operator"
            }
        };

        _mockMediator.Setup(x => x.Send(It.IsAny<OperatorQueries.GetOperatorsForDropDownQuery>(), default))
            .ReturnsAsync(Result.Success(operators));
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractCommands.CreateContractCommand>(), default))
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
            _mockMediator.Verify(x => x.Send(It.IsAny<ContractCommands.CreateContractCommand>(), default), Times.Once());
            _fakeNavigationManager.Uri.ShouldContain("/contracts");
        }, timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void ContractsNew_FailedCreation_ShowsErrorMessage()
    {
        // Arrange
        Guid operatorId = Guid.NewGuid();
        List<OperatorDropDownModel> operators = new List<OperatorDropDownModel>
        {
            new OperatorDropDownModel
            {
                OperatorId = operatorId,
                OperatorName = "Test Operator"
            }
        };

        _mockMediator.Setup(x => x.Send(It.IsAny<OperatorQueries.GetOperatorsForDropDownQuery>(), default))
            .ReturnsAsync(Result.Success(operators));
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractCommands.CreateContractCommand>(), default))
            .ReturnsAsync(Result.Failure("Failed to create contract"));

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
            _mockMediator.Verify(x => x.Send(It.IsAny<ContractCommands.CreateContractCommand>(), default), Times.Once());
            cut.Markup.ShouldContain("Failed to create contract");
        }, timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void ContractsNew_FormValidation_RequiresDescription()
    {
        // Arrange
        Guid operatorId = Guid.NewGuid();
        List<OperatorDropDownModel> operators = new List<OperatorDropDownModel>
        {
            new OperatorDropDownModel
            {
                OperatorId = operatorId,
                OperatorName = "Test Operator"
            }
        };

        _mockMediator.Setup(x => x.Send(It.IsAny<OperatorQueries.GetOperatorsForDropDownQuery>(), default))
            .ReturnsAsync(Result.Success(operators));

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
            _mockMediator.Verify(x => x.Send(It.IsAny<ContractCommands.CreateContractCommand>(), default), Times.Never());
        }, timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void ContractsNew_FormValidation_RequiresOperator()
    {
        // Arrange
        List<OperatorDropDownModel> operators = new List<OperatorDropDownModel>
        {
            new OperatorDropDownModel
            {
                OperatorId = Guid.NewGuid(),
                OperatorName = "Test Operator"
            }
        };

        _mockMediator.Setup(x => x.Send(It.IsAny<OperatorQueries.GetOperatorsForDropDownQuery>(), default))
            .ReturnsAsync(Result.Success(operators));

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
            // Should not call the mediator
            _mockMediator.Verify(x => x.Send(It.IsAny<ContractCommands.CreateContractCommand>(), default), Times.Never());
        }, timeout: TimeSpan.FromSeconds(5));
    }
}
