using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Pages.Contract.ContractProductTransactionFee;
using EstateManagementUI.Testing;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using Moq;
using Shouldly;

namespace EstateManagementUI.UITests;

public class ContractProductTransactionFeeListTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IPermissionsService> _permissionsServiceMock;
    private readonly ContractProductTransactionFeeList _component;

    public ContractProductTransactionFeeListTests()
    {
        this._mediatorMock = new Mock<IMediator>();
        this._permissionsServiceMock = new Mock<IPermissionsService>();
        this._component = new ContractProductTransactionFeeList(this._mediatorMock.Object, this._permissionsServiceMock.Object);
        this._component.ViewContext = TestHelper.GetTestViewContext();
    }

    [Fact]
    public async Task MountAsync_ShouldCallGetContract()
    {
        // Arrange
        this._component.ContractId = TestData.ContractId;
        this._component.ContractProductId = TestData.ContractProductId;

        var contractModel = TestData.GetContractModel();
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetContractQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(contractModel);

        // Act
        await this._component.MountAsync();

        // Assert
        this._component.ContractProductTransactionFees.Count.ShouldBe(2);
        this._component.ContractProductTransactionFees[0].CalculationType.ShouldBe("Type1");
        this._component.ContractProductTransactionFees[1].CalculationType.ShouldBe("Type2");
    }

    [Fact]
    public async Task Sort_ShouldSortContractProductTransactionFeesByValue()
    {
        // Arrange
        this._component.ContractId = TestData.ContractId;
        this._component.ContractProductId = TestData.ContractProductId;

        var contractModel = TestData.GetContractModel();
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetContractQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(contractModel);

        // Act
        await this._component.Sort(ContractProductTransactionFeeSorting.Value);

        // Assert
        this._component.ContractProductTransactionFees[0].Value.ShouldBe(10.0m);
        this._component.ContractProductTransactionFees[1].Value.ShouldBe(20.0m);

        // Act
        await this._component.Sort(ContractProductTransactionFeeSorting.Value);

        // Assert
        this._component.ContractProductTransactionFees[0].Value.ShouldBe(20.0m);
        this._component.ContractProductTransactionFees[1].Value.ShouldBe(10.0m);
    }

    [Fact]
    public async Task Sort_ShouldSortContractProductTransactionFeesByCalculationType()
    {
        // Arrange
        this._component.ContractId = TestData.ContractId;
        this._component.ContractProductId = TestData.ContractProductId;

        var contractModel = TestData.GetContractModel();
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetContractQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(contractModel);

        // Act
        await this._component.Sort(ContractProductTransactionFeeSorting.CalculationType);

        // Assert
        this._component.ContractProductTransactionFees[0].CalculationType.ShouldBe("Type2");
        this._component.ContractProductTransactionFees[1].CalculationType.ShouldBe("Type1");

        // Act
        await this._component.Sort(ContractProductTransactionFeeSorting.CalculationType);

        // Assert
        this._component.ContractProductTransactionFees[0].CalculationType.ShouldBe("Type1");
        this._component.ContractProductTransactionFees[1].CalculationType.ShouldBe("Type2");
    }

    [Fact]
    public async Task Sort_ShouldSortContractProductTransactionFeesByDescription()
    {
        // Arrange
        this._component.ContractId = TestData.ContractId;
        this._component.ContractProductId = TestData.ContractProductId;

        var contractModel = TestData.GetContractModel();
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetContractQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(contractModel);

        // Act
        await this._component.Sort(ContractProductTransactionFeeSorting.Description);

        // Assert
        this._component.ContractProductTransactionFees[0].Description.ShouldBe("Description1");
        this._component.ContractProductTransactionFees[1].Description.ShouldBe("Description2");

        // Act
        await this._component.Sort(ContractProductTransactionFeeSorting.Description);

        // Assert
        this._component.ContractProductTransactionFees[0].Description.ShouldBe("Description2");
        this._component.ContractProductTransactionFees[1].Description.ShouldBe("Description1");
    }

    [Fact]
    public async Task Sort_ShouldSortContractProductTransactionFeesByFeeType()
    {
        // Arrange
        this._component.ContractId = TestData.ContractId;
        this._component.ContractProductId = TestData.ContractProductId;

        var contractModel = TestData.GetContractModel();
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetContractQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(contractModel);

        // Act
        await this._component.Sort(ContractProductTransactionFeeSorting.FeeType);

        // Assert
        this._component.ContractProductTransactionFees[0].FeeType.ShouldBe("FeeType1");
        this._component.ContractProductTransactionFees[1].FeeType.ShouldBe("FeeType2");

        // Act
        await this._component.Sort(ContractProductTransactionFeeSorting.FeeType);

        // Assert
        this._component.ContractProductTransactionFees[0].FeeType.ShouldBe("FeeType2");
        this._component.ContractProductTransactionFees[1].FeeType.ShouldBe("FeeType1");
    }

    [Fact]
    public async Task NewContractProductFees_NavigatesToNewContractProductFeesPage()
    {
        // Arrange
        this._component.Url = TestHelper.GetTestUrlHelper();

        // Act
        await this._component.NewContractProductTransactionFee();

        // Assert
        this._component.LocationUrl.ShouldNotBeNull();
        this._component.LocationUrl.ShouldBe("/Contract/NewContractProductTransactionFee");
    }


}