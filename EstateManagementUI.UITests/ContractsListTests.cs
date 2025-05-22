using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Pages.Contract.Contracts;
using EstateManagementUI.Testing;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using Shouldly;
using System.Security.Claims;

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
        this._contractsList.ViewContext = TestHelper.GetTestViewContext();
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
        this._contractsList.Url = TestHelper.GetTestUrlHelper();

        // Act
        await _contractsList.ViewProducts(contractId);

        // Assert
        _contractsList.LocationUrl.ShouldNotBeNull();
        _contractsList.LocationUrl.ShouldBe("/Contract/ContractProducts");
        var payloadContractId = TestHelpers.GetPropertyValue<Guid>(_contractsList.Payload, "ContractId");
        payloadContractId.ShouldBe(contractId);
    }

    [Fact]
    public async Task View_ShouldNavigateToContractPage()
    {
        // Arrange
        var contractId = Guid.NewGuid();
        this._contractsList.Url = TestHelper.GetTestUrlHelper();

        // Act
        await _contractsList.View(contractId);

        // Assert
        _contractsList.LocationUrl.ShouldNotBeNull();
        _contractsList.LocationUrl.ShouldBe("/Contract/ViewContract");
        var payloadContractId = TestHelpers.GetPropertyValue<Guid>(_contractsList.Payload, "ContractId");
        payloadContractId.ShouldBe(contractId);
    }

    [Fact]
    public async Task NewContract_ShouldNavigateToNewContractPage()
    {
        // Arrange
        this._contractsList.Url = TestHelper.GetTestUrlHelper();

        // Act
        await _contractsList.NewContract();

        // Assert
        _contractsList.LocationUrl.ShouldNotBeNull();
        _contractsList.LocationUrl.ShouldBe("/Contract/NewContract");
    }
}