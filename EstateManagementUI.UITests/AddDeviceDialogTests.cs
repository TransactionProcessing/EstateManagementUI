using System.Text;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Pages.Merchant;
using EstateManagementUI.Pages.Merchant.MerchantDetails;
using EstateManagementUI.Pages.Shared.Components;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using Moq;
using Newtonsoft.Json;
using Shouldly;
using SimpleResults;

namespace EstateManagementUI.UITests;

public class AddDeviceDialogTests {
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IPermissionsService> _permissionsServiceMock;
    private readonly AddDeviceDialog _addDeviceDialog;

    public AddDeviceDialogTests() {
        this._mediatorMock = new Mock<IMediator>();
        this._permissionsServiceMock = new Mock<IPermissionsService>();

        this._addDeviceDialog = new AddDeviceDialog(this._mediatorMock.Object, this._permissionsServiceMock.Object);
        this._addDeviceDialog.ViewContext = TestHelper.GetTestViewContext();
    }

    [Fact]
    public async Task Mount_NoErrors()
    {
        // Arrange
        this._addDeviceDialog.MerchantId = Guid.NewGuid();
        this._addDeviceDialog.MerchantDevice = "ABCD1234";
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Commands.AssignDeviceToMerchantCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        // Act
        await this._addDeviceDialog.MountAsync();

        // Assert
    }

    [Fact]
    public async Task Save_AssignsDeviceToMerchant()
    {
        // Arrange
        this._addDeviceDialog.MerchantId = Guid.NewGuid();
        this._addDeviceDialog.MerchantDevice = "ABCD1234";
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Commands.AssignDeviceToMerchantCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        // Act
        await this._addDeviceDialog.Save();

        // Assert
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Commands.AssignDeviceToMerchantCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        var events = this._addDeviceDialog.GetDispatchedEvents();
        events.Count.ShouldBe(2);
        events[0].ShouldBeOfType<MerchantPageEvents.DeviceAssignedToMerchantEvent>();
        events[1].ShouldBeOfType<MerchantPageEvents.HideAddDeviceDialog>();
    }

    [Fact]
    public async Task Save_AssignsDeviceToMerchant_Fails()
    {
        // Arrange
        this._addDeviceDialog.MerchantId = Guid.NewGuid();
        this._addDeviceDialog.MerchantDevice = "ABCD1234";
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Commands.AssignDeviceToMerchantCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure);

        // Act
        await this._addDeviceDialog.Save();

        // Assert
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Commands.AssignDeviceToMerchantCommand>(), It.IsAny<CancellationToken>()), Times.Once);

        var events = this._addDeviceDialog.GetDispatchedEvents();
        events.Count.ShouldBe(2);
        events[0].ShouldBeOfType<ShowMessage>();
        events[1].ShouldBeOfType<MerchantPageEvents.HideAddDeviceDialog>();
    }

    [Fact]
    public async Task Close_DispatchesHideAddContractDialogEvent()
    {
        // Act
        await this._addDeviceDialog.Close();

        // Assert
        var events = this._addDeviceDialog.GetDispatchedEvents();
        events.Count.ShouldBe(1);
        events[0].ShouldBeOfType<MerchantPageEvents.HideAddDeviceDialog>();
    }
}