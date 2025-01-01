using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Pages.Contract.Contracts;
using EstateManagementUI.Pages.Dashboard.Dashboard;
using EstateManagementUI.Pages.Merchant.MerchantDetails;
using EstateManagementUI.Pages.Shared.Components;
using EstateManagementUI.Testing;
using EstateManagementUI.ViewModels;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using Moq;
using Shouldly;
using SimpleResults;
using static EstateManagementUI.Pages.Merchant.MerchantDetails.MerchantPageEvents;

namespace EstateManagementUI.UITests;

public class ContractsListTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IPermissionsService> _permissionsServiceMock;
    private readonly ContractsList _contractsList;

    public ContractsListTests()
    {
        this._mediatorMock = new Mock<IMediator>();
        this._permissionsServiceMock = new Mock<IPermissionsService>();
        this._contractsList = new ContractsList(this._mediatorMock.Object, this._permissionsServiceMock.Object);
    }

    [Fact]
    public async Task MountAsync_PopulatesContracts()
    {
        // Arrange
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetContractsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.ContractsResult);

        // Act
        await this._contractsList.MountAsync();

        // Assert
        this._contractsList.Contracts.ShouldNotBeNull();
        this._contractsList.Contracts.Count.ShouldBe(2);
        this._contractsList.Contracts[0].Description.ShouldBe("Contract1");
        this._contractsList.Contracts[1].Description.ShouldBe("Contract2");
    }

    [Fact]
    public async Task Sort_ShouldSortContractsByDescription()
    {
        // Arrange
        var contracts = TestData.GetContracts();
        _mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetContractsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(contracts);
        
        // Act
        await _contractsList.Sort(ContractSorting.Description);

        // Assert
        _contractsList.Contracts[0].Description.ShouldBe("Description2");
        _contractsList.Contracts[1].Description.ShouldBe("Description1");

        // Act
        await _contractsList.Sort(ContractSorting.Description);

        // Assert
        _contractsList.Contracts[0].Description.ShouldBe("Description1");
        _contractsList.Contracts[1].Description.ShouldBe("Description2");
    }

    [Fact]
    public async Task Sort_ShouldSortContractsByOperator()
    {
        // Arrange
        var contracts = TestData.GetContracts();
        _mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetContractsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(contracts);

        // Act
        await _contractsList.Sort(ContractSorting.Operator);

        // Assert
        _contractsList.Contracts[0].OperatorName.ShouldBe("Operator1");
        _contractsList.Contracts[1].OperatorName.ShouldBe("Operator2");

        // Act
        await _contractsList.Sort(ContractSorting.Operator);

        // Assert
        _contractsList.Contracts[0].OperatorName.ShouldBe("Operator2");
        _contractsList.Contracts[1].OperatorName.ShouldBe("Operator1");
    }

    [Fact]
    public async Task Sort_ShouldSortContractsByNumberOfProducts()
    {
        // Arrange
        var contracts = TestData.GetContracts();
        _mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetContractsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(contracts);

        // Act
        await _contractsList.Sort(ContractSorting.NumberOfProducts);

        // Assert
        _contractsList.Contracts[0].NumberOfProducts.ShouldBe(2);
        _contractsList.Contracts[1].NumberOfProducts.ShouldBe(3);

        // Act
        await _contractsList.Sort(ContractSorting.NumberOfProducts);

        // Assert
        _contractsList.Contracts[0].NumberOfProducts.ShouldBe(3);
        _contractsList.Contracts[1].NumberOfProducts.ShouldBe(2);
    }

    [Fact]
    public async Task ViewProducts_ShouldNavigateToProductPage()
    {
        // Arrange
        var contractId = Guid.NewGuid();

        // Act
        await _contractsList.ViewProducts(contractId);

        // Assert
        _contractsList.LocationUrl.ShouldNotBeNull();
        _contractsList.LocationUrl.ShouldBe("/Contract/ContractProducts");
        var payloadContractId = TestHelpers.GetPropertyValue<Guid>(_contractsList.Payload, "ContractId");
        payloadContractId.ShouldBe(contractId);
    }
}

public class AddOperatorDialogTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IPermissionsService> _permissionsServiceMock;
    private readonly AddOperatorDialog _addOperatorDialog;

    public AddOperatorDialogTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _permissionsServiceMock = new Mock<IPermissionsService>();
        this._addOperatorDialog = new AddOperatorDialog(_mediatorMock.Object, _permissionsServiceMock.Object);
    }

    [Fact]
    public async Task MountAsync_ShouldPopulateOperators()
    {
        // Arrange
        var operatorListModel = TestData.GetOperatorModels();
        _mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetOperatorsQuery>(), It.IsAny<CancellationToken>()))
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
        
        _mediatorMock.Setup(m => m.Send(It.IsAny<Commands.AssignOperatorToMerchantCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        // Act
        await this._addOperatorDialog.Save();

        // Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<Commands.AssignOperatorToMerchantCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        this._addOperatorDialog.Events.ShouldContain(e => e is OperatorAssignedToMerchantEvent);
    }

    [Fact]
    public async Task Save_ShouldShowErrorMessageOnFailure()
    {
        // Arrange
        this._addOperatorDialog.Operator = TestData.GetOperatorListModels().First();
        this._addOperatorDialog.MerchantNumber = "123456";
        this._addOperatorDialog.TerminalNumber = "7890";
        this._addOperatorDialog.MerchantId = Guid.NewGuid();

        _mediatorMock.Setup(m => m.Send(It.IsAny<Commands.AssignOperatorToMerchantCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure("Error"));

        // Act
        await this._addOperatorDialog.Save();

        // Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<Commands.AssignOperatorToMerchantCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        this._addOperatorDialog.Events.ShouldContain(e => e is ShowMessage && ((ShowMessage)e).Message == "Error assigning operator to Merchant");
    }

    [Fact]
    public async Task Close_ShouldDispatchHideAddOperatorDialogEvent()
    {
        // Act
        await this._addOperatorDialog.Close();

        // Assert
        this._addOperatorDialog.Events.ShouldContain(e => e is MerchantPageEvents.HideAddOperatorDialog);
    }
}

public class EditMerchantTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IPermissionsService> _permissionsServiceMock;
    private readonly EditMerchant _editMerchant;

    public EditMerchantTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _permissionsServiceMock = new Mock<IPermissionsService>();
        _editMerchant = new EditMerchant(_mediatorMock.Object, _permissionsServiceMock.Object);
    }

    [Fact]
    public void SetActiveTab_ShouldSetActiveTab()
    {
        // Act
        _editMerchant.SetActiveTab("contracts");

        // Assert
        _editMerchant.ActiveTab.ShouldBe("contracts");
    }

    [Fact]
    public void AddOperator_ShouldDispatchShowAddOperatorDialogEvent()
    {
        // Act
        _editMerchant.AddOperator();

        // Assert
        _editMerchant.Events.ShouldContain(e => e is ShowAddOperatorDialog);
    }

    [Fact]
    public void AddContract_ShouldDispatchShowAddContractDialogEvent()
    {
        // Act
        _editMerchant.AddContract();

        // Assert
        _editMerchant.Events.ShouldContain(e => e is ShowAddContractDialog);
    }
}

public class DashboardTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IPermissionsService> _permissionsServiceMock;
    private readonly Dashboard _dashboard;

    public DashboardTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _permissionsServiceMock = new Mock<IPermissionsService>();
        _dashboard = new Dashboard(_mediatorMock.Object, _permissionsServiceMock.Object);
    }

    [Fact]
    public async Task MountAsync_ShouldPopulateDropdownsAndQueryData()
    {
        // Arrange
        var merchants = TestData.GetMerchants();
        var operators = TestData.GetOperators();
        var comparisonDates = TestData.GetComparisonDates();

        _mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetMerchantsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(merchants);
        _mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetOperatorsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(operators);
        _mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetComparisonDatesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(comparisonDates);
        _mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSalesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.GetTodaysSalesModel()));
        _mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSettlementQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.GetTodaysSettlementModel()));
        _mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSalesCountByHourQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.GetTodaysSalesCountByHourModels()));
        _mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSalesValueByHourQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.GetTodaysSalesValueByHourModels()));

        // Act
        await _dashboard.MountAsync();

        // Assert
        _dashboard.Merchant.ShouldNotBeNull();
        _dashboard.Operator.ShouldNotBeNull();
        _dashboard.ComparisonDate.ShouldNotBeNull();
        _dashboard.TodaysSales.ShouldNotBeNull();
        _dashboard.TodaysSettlement.ShouldNotBeNull();
        _dashboard.TodaysSalesCountByHour.ShouldNotBeNull();
        _dashboard.TodaysSalesValueByHour.ShouldNotBeNull();
        _mediatorMock.Verify(m => m.Send(It.IsAny<Queries.GetMerchantsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        _mediatorMock.Verify(m => m.Send(It.IsAny<Queries.GetOperatorsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        _mediatorMock.Verify(m => m.Send(It.IsAny<Queries.GetComparisonDatesQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Query_ShouldGetTodaysData()
    {
        // Arrange
        var selectedDate = DateTime.Now;
        _dashboard.ComparisonDate = new ComparisonDateListModel { SelectedDate = selectedDate };

        _mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSalesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.GetTodaysSalesModel()));
        _mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSettlementQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.GetTodaysSettlementModel()));
        _mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSalesCountByHourQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.GetTodaysSalesCountByHourModels()));
        _mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSalesValueByHourQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.GetTodaysSalesValueByHourModels()));

        // Act
        await _dashboard.Query();

        // Assert
        _dashboard.TodaysSales.ShouldNotBeNull();
        _dashboard.TodaysSettlement.ShouldNotBeNull();
        _dashboard.TodaysSalesCountByHour.ShouldNotBeNull();
        _dashboard.TodaysSalesValueByHour.ShouldNotBeNull();
        _mediatorMock.Verify(m => m.Send(It.IsAny<Queries.GetTodaysSalesQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        _mediatorMock.Verify(m => m.Send(It.IsAny<Queries.GetTodaysSettlementQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        _mediatorMock.Verify(m => m.Send(It.IsAny<Queries.GetTodaysSalesCountByHourQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        _mediatorMock.Verify(m => m.Send(It.IsAny<Queries.GetTodaysSalesValueByHourQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}