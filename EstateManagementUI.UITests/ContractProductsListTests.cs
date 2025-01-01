using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Pages.Contract.ContractProduct;
using EstateManagementUI.Pages.Contract.ContractProductTransactionFee;
using EstateManagementUI.Testing;
using EstateManagementUI.ViewModels;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using Moq;
using Shouldly;

namespace EstateManagementUI.UITests;

public class ContractProductsListTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IPermissionsService> _permissionsServiceMock;
    private readonly ContractProductsList _contractProductsList;

    public ContractProductsListTests()
    {
        this._mediatorMock = new Mock<IMediator>();
        this._permissionsServiceMock = new Mock<IPermissionsService>();
        this._contractProductsList = new ContractProductsList(this._mediatorMock.Object, this._permissionsServiceMock.Object);
    }

    [Fact]
    public async Task MountAsync_PopulatesContractProducts()
    {
        // Arrange
        this._contractProductsList.ContractId = Guid.NewGuid();
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetContractQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.ContractsResultWithProducts);

        // Act
        await this._contractProductsList.MountAsync();

        // Assert
        this._contractProductsList.ContractName.ShouldBe(TestData.ContractsResultWithProducts.Description);
        this._contractProductsList.ContractProducts.ShouldNotBeNull();
        this._contractProductsList.ContractProducts.Count.ShouldBe(2);
        this._contractProductsList.ContractProducts[0].ProductName.ShouldBe("Product1");
        this._contractProductsList.ContractProducts[1].ProductName.ShouldBe("Product2");
    }
    
    [Fact]
    public async Task Sort_SortsContractProductsByDisplayText()
    {
        // Arrange
        this._contractProductsList.ContractId = Guid.NewGuid();
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetContractQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.ContractsResultWithProducts);

        // Act
        await this._contractProductsList.Sort(ContractProductSorting.DisplayText);

        // Assert
        this._contractProductsList.ContractProducts.ShouldNotBeNull();
        this._contractProductsList.ContractProducts.Count.ShouldBe(2);
        this._contractProductsList.ContractProducts[0].DisplayText.ShouldBe("Product 2");
        this._contractProductsList.ContractProducts[1].DisplayText.ShouldBe("Product 1");

        // Act
        await this._contractProductsList.Sort(ContractProductSorting.DisplayText);

        this._contractProductsList.ContractProducts[0].DisplayText.ShouldBe("Product 1");
        this._contractProductsList.ContractProducts[1].DisplayText.ShouldBe("Product 2");
    }

    [Fact]
    public async Task Sort_SortsContractProductsByNumberOfFees()
    {
        // Arrange
        this._contractProductsList.ContractId = Guid.NewGuid();
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetContractQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.ContractsResultWithProducts);

        // Act
        await this._contractProductsList.Sort(ContractProductSorting.NumberOfFees);

        // Assert
        this._contractProductsList.ContractProducts.ShouldNotBeNull();
        this._contractProductsList.ContractProducts.Count.ShouldBe(2);
        this._contractProductsList.ContractProducts[0].NumberOfFees.ShouldBe(2);
        this._contractProductsList.ContractProducts[1].NumberOfFees.ShouldBe(3);

        // Act
        await this._contractProductsList.Sort(ContractProductSorting.NumberOfFees);

        this._contractProductsList.ContractProducts[0].NumberOfFees.ShouldBe(3);
        this._contractProductsList.ContractProducts[1].NumberOfFees.ShouldBe(2);
    }

    [Fact]
    public async Task Sort_SortsContractProductsByProductName()
    {
        // Arrange
        this._contractProductsList.ContractId = Guid.NewGuid();
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetContractQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.ContractsResultWithProducts);

        // Act
        await this._contractProductsList.Sort(ContractProductSorting.ProductName);

        // Assert
        this._contractProductsList.ContractProducts.ShouldNotBeNull();
        this._contractProductsList.ContractProducts.Count.ShouldBe(2);
        this._contractProductsList.ContractProducts[0].ProductName.ShouldBe("Product1");
        this._contractProductsList.ContractProducts[1].ProductName.ShouldBe("Product2");

        // Act
        await this._contractProductsList.Sort(ContractProductSorting.ProductName);

        this._contractProductsList.ContractProducts[0].ProductName.ShouldBe("Product2");
        this._contractProductsList.ContractProducts[1].ProductName.ShouldBe("Product1");
    }

    [Fact]
    public async Task Sort_SortsContractProductsByProductType()
    {
        // Arrange
        this._contractProductsList.ContractId = Guid.NewGuid();
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetContractQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.ContractsResultWithProducts);

        // Act
        await this._contractProductsList.Sort(ContractProductSorting.ProductType);

        // Assert
        this._contractProductsList.ContractProducts.ShouldNotBeNull();
        this._contractProductsList.ContractProducts.Count.ShouldBe(2);
        this._contractProductsList.ContractProducts[0].ProductType.ShouldBe("Type1");
        this._contractProductsList.ContractProducts[1].ProductType.ShouldBe("Type2");

        // Act
        await this._contractProductsList.Sort(ContractProductSorting.ProductType);

        this._contractProductsList.ContractProducts[0].ProductType.ShouldBe("Type2");
        this._contractProductsList.ContractProducts[1].ProductType.ShouldBe("Type1");
    }

    [Fact]
    public async Task Sort_SortsContractProductsByValue()
    {
        // Arrange
        this._contractProductsList.ContractId = Guid.NewGuid();
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetContractQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.ContractsResultWithProducts);

        // Act
        await this._contractProductsList.Sort(ContractProductSorting.Value);

        // Assert
        this._contractProductsList.ContractProducts.ShouldNotBeNull();
        this._contractProductsList.ContractProducts.Count.ShouldBe(2);
        this._contractProductsList.ContractProducts[0].Value.ShouldBe("100 KES");
        this._contractProductsList.ContractProducts[1].Value.ShouldBe("200 KES");

        // Act
        await this._contractProductsList.Sort(ContractProductSorting.Value);

        this._contractProductsList.ContractProducts[0].Value.ShouldBe("200 KES");
        this._contractProductsList.ContractProducts[1].Value.ShouldBe("100 KES");
    }

    [Fact]
    public async Task ViewProductFees_NavigatesToProductFeesPage()
    {
        // Arrange
        var contractProductId = Guid.NewGuid();
        this._contractProductsList.ContractId = Guid.NewGuid();

        // Act
        await this._contractProductsList.ViewProductFees(contractProductId);

        // Assert
        this._contractProductsList.LocationUrl.ShouldNotBeNull();
        this._contractProductsList.LocationUrl.ShouldBe("/Contract/ContractProductTransactionFees");
        Guid payloadContractProductId = TestHelpers.GetPropertyValue<Guid>(this._contractProductsList.Payload, "ContractProductId");
        payloadContractProductId.ShouldBe(contractProductId);
    }
}

public class ContractProductTransactionFeeListTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IPermissionsService> _permissionsServiceMock;
    private readonly ContractProductTransactionFeeList _component;

    public ContractProductTransactionFeeListTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _permissionsServiceMock = new Mock<IPermissionsService>();
        _component = new ContractProductTransactionFeeList(_mediatorMock.Object, _permissionsServiceMock.Object);
    }

    [Fact]
    public async Task MountAsync_ShouldCallGetContract()
    {
        // Arrange
        _component.ContractId = TestData.ContractId;
        _component.ContractProductId = TestData.ContractProductId;

        var contractModel = TestData.GetContractModel();
        _mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetContractQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(contractModel);

        // Act
        await _component.MountAsync();

        // Assert
        _component.ContractProductTransactionFees.Count.ShouldBe(2);
        _component.ContractProductTransactionFees[0].CalculationType.ShouldBe("Type1");
        _component.ContractProductTransactionFees[1].CalculationType.ShouldBe("Type2");
    }

    [Fact]
    public async Task Sort_ShouldSortContractProductTransactionFeesByValue()
    {
        // Arrange
        _component.ContractId = TestData.ContractId;
        _component.ContractProductId = TestData.ContractProductId;

        var contractModel = TestData.GetContractModel();
        _mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetContractQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(contractModel);

        // Act
        await _component.Sort(ContractProductTransactionFeeSorting.Value);

        // Assert
        _component.ContractProductTransactionFees[0].Value.ShouldBe(10.0m);
        _component.ContractProductTransactionFees[1].Value.ShouldBe(20.0m);

        // Act
        await _component.Sort(ContractProductTransactionFeeSorting.Value);

        // Assert
        _component.ContractProductTransactionFees[0].Value.ShouldBe(20.0m);
        _component.ContractProductTransactionFees[1].Value.ShouldBe(10.0m);
    }

    [Fact]
    public async Task Sort_ShouldSortContractProductTransactionFeesByCalculationType()
    {
        // Arrange
        _component.ContractId = TestData.ContractId;
        _component.ContractProductId = TestData.ContractProductId;

        var contractModel = TestData.GetContractModel();
        _mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetContractQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(contractModel);

        // Act
        await _component.Sort(ContractProductTransactionFeeSorting.CalculationType);

        // Assert
        _component.ContractProductTransactionFees[0].CalculationType.ShouldBe("Type2");
        _component.ContractProductTransactionFees[1].CalculationType.ShouldBe("Type1");

        // Act
        await _component.Sort(ContractProductTransactionFeeSorting.CalculationType);

        // Assert
        _component.ContractProductTransactionFees[0].CalculationType.ShouldBe("Type1");
        _component.ContractProductTransactionFees[1].CalculationType.ShouldBe("Type2");
    }

    [Fact]
    public async Task Sort_ShouldSortContractProductTransactionFeesByDescription()
    {
        // Arrange
        _component.ContractId = TestData.ContractId;
        _component.ContractProductId = TestData.ContractProductId;

        var contractModel = TestData.GetContractModel();
        _mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetContractQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(contractModel);

        // Act
        await _component.Sort(ContractProductTransactionFeeSorting.Description);

        // Assert
        _component.ContractProductTransactionFees[0].Description.ShouldBe("Description1");
        _component.ContractProductTransactionFees[1].Description.ShouldBe("Description2");

        // Act
        await _component.Sort(ContractProductTransactionFeeSorting.Description);

        // Assert
        _component.ContractProductTransactionFees[0].Description.ShouldBe("Description2");
        _component.ContractProductTransactionFees[1].Description.ShouldBe("Description1");
    }

    [Fact]
    public async Task Sort_ShouldSortContractProductTransactionFeesByFeeType()
    {
        // Arrange
        _component.ContractId = TestData.ContractId;
        _component.ContractProductId = TestData.ContractProductId;

        var contractModel = TestData.GetContractModel();
        _mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetContractQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(contractModel);

        // Act
        await _component.Sort(ContractProductTransactionFeeSorting.FeeType);

        // Assert
        _component.ContractProductTransactionFees[0].FeeType.ShouldBe("FeeType1");
        _component.ContractProductTransactionFees[1].FeeType.ShouldBe("FeeType2");

        // Act
        await _component.Sort(ContractProductTransactionFeeSorting.FeeType);

        // Assert
        _component.ContractProductTransactionFees[0].FeeType.ShouldBe("FeeType2");
        _component.ContractProductTransactionFees[1].FeeType.ShouldBe("FeeType1");
    }


}