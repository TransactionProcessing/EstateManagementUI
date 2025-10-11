using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Pages.Merchant;
using EstateManagementUI.Pages.Merchant.MerchantDetails;
using EstateManagementUI.Pages.Shared.Components;
using EstateManagementUI.Testing;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using Moq;
using Shouldly;
using SimpleResults;

namespace EstateManagementUI.UITests;

public class AddOperatorDialogTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IPermissionsService> _permissionsServiceMock;
    private readonly AddOperatorDialog _addOperatorDialog;

    public AddOperatorDialogTests()
    {
        this._mediatorMock = new Mock<IMediator>();
        this._permissionsServiceMock = new Mock<IPermissionsService>();

        this._addOperatorDialog = new AddOperatorDialog(this._mediatorMock.Object, this._permissionsServiceMock.Object);
        this._addOperatorDialog.ViewContext = TestHelper.GetTestViewContext();
    }

    [Fact]
    public async Task MountAsync_ShouldPopulateOperators()
    {
        // Arrange
        var operatorListModel = TestData.GetOperatorModels();
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetOperatorsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(operatorListModel);

        // Act
        await this._addOperatorDialog.MountAsync();

        // Assert
        this._addOperatorDialog.Operator.ShouldNotBeNull();
        this._addOperatorDialog.Operator.Operators.Count.ShouldBe(3);
    }

    [Fact]
    public async Task Save_ShouldAssignOperatorToMerchant()
    {
        // Arrange
        this._addOperatorDialog.Operator = TestData.GetOperatorListModels().First();
        this._addOperatorDialog.MerchantNumber = "123456";
        this._addOperatorDialog.TerminalNumber = "7890";
        this._addOperatorDialog.MerchantId = Guid.NewGuid();

        var assignOperatorToMerchantModel = TestData.GetAssignOperatorToMerchantModel();
        
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Commands.AssignOperatorToMerchantCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        // Act
        await this._addOperatorDialog.Save();

        // Assert
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Commands.AssignOperatorToMerchantCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        var events = this._addOperatorDialog.GetDispatchedEvents();
        events.ShouldContain(e => e is MerchantPageEvents.OperatorAssignedToMerchantEvent);
    }

    [Fact]
    public async Task Save_ShouldShowErrorMessageOnFailure()
    {
        // Arrange
        this._addOperatorDialog.Operator = TestData.GetOperatorListModels().First();
        this._addOperatorDialog.MerchantNumber = "123456";
        this._addOperatorDialog.TerminalNumber = "7890";
        this._addOperatorDialog.MerchantId = Guid.NewGuid();

        this._mediatorMock.Setup(m => m.Send(It.IsAny<Commands.AssignOperatorToMerchantCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure("Error"));

        // Act
        await this._addOperatorDialog.Save();

        // Assert
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Commands.AssignOperatorToMerchantCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        var events = this._addOperatorDialog.GetDispatchedEvents();
        events.ShouldContain(e => e is ShowMessage && ((ShowMessage)e).Message == "Error assigning operator to Merchant");
    }

    [Fact]
    public async Task Close_ShouldDispatchHideAddOperatorDialogEvent()
    {
        // Act
        await this._addOperatorDialog.Close();

        // Assert
        var events = this._addOperatorDialog.GetDispatchedEvents();
        events.ShouldContain(e => e is MerchantPageEvents.HideAddOperatorDialog);
    }
}