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

public class EditMerchantTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IPermissionsService> _permissionsServiceMock;
    private readonly EditMerchant _editMerchant;

    public EditMerchantTests()
    {
        this._mediatorMock = new Mock<IMediator>();
        this._permissionsServiceMock = new Mock<IPermissionsService>();
        this._editMerchant = new EditMerchant(this._mediatorMock.Object, this._permissionsServiceMock.Object);
        this._editMerchant.ViewContext = TestHelper.GetTestViewContext();
    }

    [Fact]
    public void SetActiveTab_ShouldSetActiveTab()
    {
        // Act
        this._editMerchant.SetActiveTab("contracts");

        // Assert
        this._editMerchant.ActiveTab.ShouldBe("contracts");
    }

    [Fact]
    public void AddOperator_ShouldDispatchShowAddOperatorDialogEvent()
    {
        // Act
        this._editMerchant.AddOperator();

        // Assert
        var events = this._editMerchant.GetDispatchedEvents();
        events.ShouldContain(e => e is MerchantPageEvents.ShowAddOperatorDialog);
    }

    [Fact]
    public void AddContract_ShouldDispatchShowAddContractDialogEvent() {
        // Act
        this._editMerchant.AddContract();

        // Assert
        var events = this._editMerchant.GetDispatchedEvents();
        events.ShouldContain(e => e is MerchantPageEvents.ShowAddContractDialog);
    }

    [Fact]
    public void AddDevice_ShouldDispatchShowAddDeviceDialogEvent()
    {
        // Act
        this._editMerchant.AddDevice();

        // Assert
        var events = this._editMerchant.GetDispatchedEvents();
        events.ShouldContain(e => e is MerchantPageEvents.ShowAddDeviceDialog);
    }


    [Fact]
    public async Task GetSettlementSchedules_ReturnsSettlementSchedules()
    {
        // Arrange


        // Act
        var result = this._editMerchant.GetSettlementSchedules();

        // Assert
        result.Count().ShouldBe(4);
    }
}

public class MakeDepositTests {
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IPermissionsService> _permissionsServiceMock;
    private readonly MakeDeposit _makeDeposit;

    public MakeDepositTests()
    {
        this._mediatorMock = new Mock<IMediator>();
        this._permissionsServiceMock = new Mock<IPermissionsService>();
        this._makeDeposit = new MakeDeposit(this._mediatorMock.Object, this._permissionsServiceMock.Object);
        this._makeDeposit.ViewContext = TestHelper.GetTestViewContext();
    }

    [Fact]
    public async Task MountAsync_MerchantIsLoaded() {
        this._makeDeposit.MerchantId = Guid.NewGuid();
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetMerchantQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.MerchantResult);

        await this._makeDeposit.MountAsync();

        this._makeDeposit.Name.ShouldBe(TestData.MerchantResult.Data.MerchantName);
    }

    [Fact]
    public async Task Close()
    {
        this._makeDeposit.Url = TestHelper.GetTestUrlHelper();

        this._makeDeposit.Close();

        this._makeDeposit.LocationUrl.ShouldNotBeNull();
        this._makeDeposit.LocationUrl.ShouldBe("/Merchant/Index");
    }

    [Fact]
    public async Task Save_MerchantDepositIsMade()
    {
        this._makeDeposit.MerchantId = Guid.NewGuid();
        this._makeDeposit.Amount = 100;
        this._makeDeposit.Date = DateTime.Now;
        this._makeDeposit.Reference = "Reference";

        this._mediatorMock.Setup(m => m.Send(It.IsAny<Commands.MakeDepositCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success);

        await this._makeDeposit.Save();

        var events = this._makeDeposit.GetDispatchedEvents();
        events.ShouldContain(e => e is MerchantPageEvents.DepositMadeEvent);
    }

    [Fact]
    public async Task Save_SaveFailed_MerchantDepositIsNotMade()
    {
        this._makeDeposit.MerchantId = Guid.NewGuid();
        this._makeDeposit.Amount = 100;
        this._makeDeposit.Date = DateTime.Now;
        this._makeDeposit.Reference = "Reference";

        this._mediatorMock.Setup(m => m.Send(It.IsAny<Commands.MakeDepositCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure(new List<String>(){"Error Message"}));

        await this._makeDeposit.Save();

        var events = this._makeDeposit.GetDispatchedEvents();
        events.ShouldContain(e => e is ShowMessage);
    }
}