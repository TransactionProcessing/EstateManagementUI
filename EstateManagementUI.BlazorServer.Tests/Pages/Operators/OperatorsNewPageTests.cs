using AngleSharp.Dom;
using Bunit;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BusinessLogic.Requests;
using Moq;
using Shouldly;
using SimpleResults;
using OperatorsNew = EstateManagementUI.BlazorServer.Components.Pages.Operators.New;

namespace EstateManagementUI.BlazorServer.Tests.Pages.Operators;

public class OperatorsNewPageTests : BaseTest
{
    [Fact]
    public void OperatorsNew_RendersCorrectly()
    {
        // Arrange & Act
        IRenderedComponent<OperatorsNew> cut = RenderComponent<OperatorsNew>();

        // Assert
        cut.Markup.ShouldContain("Create New Operator");
    }

    [Fact]
    public void OperatorsNew_HasCorrectPageTitle()
    {
        // Arrange & Act
        IRenderedComponent<OperatorsNew> cut = RenderComponent<OperatorsNew>();

        // Assert
        IRenderedComponent<Microsoft.AspNetCore.Components.Web.PageTitle> pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }

    [Fact]
    public void OperatorsNew_CancelButton_NavigatesToOperatorsIndex()
    {
        // Arrange
        IRenderedComponent<OperatorsNew> cut = RenderComponent<OperatorsNew>();
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Create New Operator"), timeout: TimeSpan.FromSeconds(5));

        // Act - Find and click the Cancel button
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? cancelButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Cancel"));
        cancelButton.ShouldNotBeNull();
        cancelButton.Click();

        // Assert
        _fakeNavigationManager.Uri.ShouldContain("/operators");
    }

    [Fact]
    public void OperatorsNew_CreateOperatorButton_IsPresent()
    {
        // Arrange
        IRenderedComponent<OperatorsNew> cut = RenderComponent<OperatorsNew>();
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Create New Operator"), timeout: TimeSpan.FromSeconds(5));

        // Act & Assert
        IElement createButton = cut.Find("#createOperatorButton");
        createButton.ShouldNotBeNull();
    }

    [Fact]
    public void OperatorsNew_SuccessfulCreation_NavigatesToOperatorsIndex()
    {
        // Arrange
        this.OperatorUIService.Setup(o => o.CreateOperator(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<OperatorModels.CreateOperatorModel>()))
            .ReturnsAsync(Result.Success());

        IRenderedComponent<OperatorsNew> cut = RenderComponent<OperatorsNew>();
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Create New Operator"), timeout: TimeSpan.FromSeconds(5));

        // Act - Fill in the form
        IElement nameInput = cut.Find("input[placeholder='Enter operator name']");
        nameInput.Change("Test Operator");

        // Submit the form
        IElement createButton = cut.Find("#createOperatorButton");
        createButton.Click();

        // Assert
        cut.WaitForAssertion(() => {
            _fakeNavigationManager.Uri.ShouldContain("/operators");
        }, timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void OperatorsNew_FailedCreation_ShowsErrorMessage()
    {
        // Arrange
        this.OperatorUIService.Setup(o => o.CreateOperator(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<OperatorModels.CreateOperatorModel>()))
            .ReturnsAsync(Result.Failure);

        IRenderedComponent<OperatorsNew> cut = RenderComponent<OperatorsNew>();
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Create New Operator"), timeout: TimeSpan.FromSeconds(5));

        // Act - Fill in the form
        IElement nameInput = cut.Find("input[placeholder='Enter operator name']");
        nameInput.Change("Test Operator");

        // Submit the form
        IElement createButton = cut.Find("#createOperatorButton");
        createButton.Click();

        // Assert
        cut.WaitForAssertion(() => {
            cut.Markup.ShouldContain("Failed to create operator");
        }, timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void OperatorsNew_FormValidation_RequiresOperatorName()
    {
        // Arrange
        IRenderedComponent<OperatorsNew> cut = RenderComponent<OperatorsNew>();
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Create New Operator"), timeout: TimeSpan.FromSeconds(5));

        // Act - Submit form without operator name
        IElement createButton = cut.Find("#createOperatorButton");
        createButton.Click();

        // Assert - Validation message should appear
        cut.WaitForAssertion(() => {
            cut.Markup.ShouldContain("Operator name is required");
        }, timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void OperatorsNew_DisplaysCheckboxOptions()
    {
        // Arrange & Act
        IRenderedComponent<OperatorsNew> cut = RenderComponent<OperatorsNew>();

        // Assert - Verify checkbox options are displayed
        cut.Markup.ShouldContain("Require Custom Merchant Number");
        cut.Markup.ShouldContain("Require Custom Terminal Number");
    }

    [Fact]
    public void OperatorsNew_SuccessfulCreation_ShowsSuccessMessage()
    {
        // Arrange
        this.OperatorUIService.Setup(o => o.CreateOperator(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<OperatorModels.CreateOperatorModel>()))
            .ReturnsAsync(Result.Success());

        IRenderedComponent<OperatorsNew> cut = RenderComponent<OperatorsNew>();
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Create New Operator"), timeout: TimeSpan.FromSeconds(5));

        // Act - Fill in the form
        IElement nameInput = cut.Find("input[placeholder='Enter operator name']");
        nameInput.Change("Test Operator");

        // Submit the form
        IElement createButton = cut.Find("#createOperatorButton");
        createButton.Click();

        // Assert - Success message should appear before navigation
        cut.WaitForAssertion(() => {
            cut.Markup.ShouldContain("Operator created successfully");
        }, timeout: TimeSpan.FromSeconds(5));
    }
}
