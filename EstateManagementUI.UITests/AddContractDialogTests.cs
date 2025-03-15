using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Pages.Merchant.MerchantDetails;
using EstateManagementUI.Pages.Shared.Components;
using EstateManagementUI.Testing;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using Shouldly;
using SimpleResults;
using System.Security.Claims;

namespace EstateManagementUI.UITests;

public class AddContractDialogTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IPermissionsService> _permissionsServiceMock;
    private readonly AddContractDialog _addContractDialog;

    public AddContractDialogTests()
    {
        this._mediatorMock = new Mock<IMediator>();
        this._permissionsServiceMock = new Mock<IPermissionsService>();

        this._addContractDialog = new AddContractDialog(this._mediatorMock.Object, this._permissionsServiceMock.Object);
        this._addContractDialog.ViewContext = TestHelper.GetTestViewContext();
    }

    [Fact]
    public async Task MountAsync_PopulatesContracts()
    {
        // Arrange
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetContractsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.Contracts);

        // Act
        await this._addContractDialog.MountAsync();

        // Assert
        this._addContractDialog.Contract.ShouldNotBeNull();
        this._addContractDialog.Contract.Contracts.Count.ShouldBe(3);
    }

    [Fact]
    public async Task Save_AssignsContractToMerchant_WhenContractIdIsNotEmpty()
    {
        // Arrange
        this._addContractDialog.MerchantId = Guid.NewGuid();
        this._addContractDialog.Contract = TestData.ContractList;
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Commands.AssignContractToMerchantCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        // Act
        await this._addContractDialog.Save();

        // Assert
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Commands.AssignContractToMerchantCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        this._addContractDialog.Events.Count.ShouldBe(2);
        this._addContractDialog.Events[0].ShouldBeOfType<MerchantPageEvents.ContractAssignedToMerchantEvent>();
        this._addContractDialog.Events[1].ShouldBeOfType<MerchantPageEvents.HideAddContractDialog>();
    }

    [Fact]
    public async Task Save_ShowsErrorMessage_WhenAssignContractFails()
    {
        // Arrange
        this._addContractDialog.MerchantId = Guid.NewGuid();
        this._addContractDialog.Contract = TestData.ContractList;
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Commands.AssignContractToMerchantCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure(new List<string> { "Error" }));

        // Act
        await this._addContractDialog.Save();

        // Assert
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Commands.AssignContractToMerchantCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        this._addContractDialog.Events.Count.ShouldBe(2);
        this._addContractDialog.Events[0].ShouldBeOfType<ShowMessage>();
        this._addContractDialog.Events[1].ShouldBeOfType<MerchantPageEvents.HideAddContractDialog>();
    }

    [Fact]
    public async Task Close_DispatchesHideAddContractDialogEvent()
    {
        // Act
        await this._addContractDialog.Close();

        // Assert
        this._addContractDialog.Events.Count.ShouldBe(1);
        this._addContractDialog.Events[0].ShouldBeOfType<MerchantPageEvents.HideAddContractDialog>();
    }
}