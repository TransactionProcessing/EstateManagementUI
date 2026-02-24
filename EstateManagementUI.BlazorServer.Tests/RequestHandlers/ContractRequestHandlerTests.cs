using EstateManagementUI.BusinessLogic.Client;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.RequestHandlers;
using EstateManagementUI.BusinessLogic.Requests;
using Moq;
using Shouldly;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.Tests.RequestHandlers;

public class ContractRequestHandlerTests
{
    private readonly Mock<IApiClient> _mockApiClient;
    private readonly ContractRequestHandler _handler;

    public ContractRequestHandlerTests()
    {
        _mockApiClient = new Mock<IApiClient>();
        _handler = new ContractRequestHandler(_mockApiClient.Object);
    }

    [Fact]
    public async Task Handle_GetContractsQuery_ReturnsSuccess_WhenApiClientSucceeds()
    {
        // Arrange
        var estateId = Guid.NewGuid();
        var query = new ContractQueries.GetContractsQuery(CorrelationIdHelper.New(), estateId);
        var contracts = new List<ContractModels.ContractModel>
        {
            new() { ContractId = Guid.NewGuid(), Description = "Contract 1", OperatorName = "Operator 1" },
            new() { ContractId = Guid.NewGuid(), Description = "Contract 2", OperatorName = "Operator 2" }
        };

        _mockApiClient
            .Setup(c => c.GetContracts(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(contracts));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data!.Count.ShouldBe(2);

        _mockApiClient.Verify(c => c.GetContracts(query, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_GetContractsQuery_ReturnsFailure_WhenApiClientFails()
    {
        // Arrange
        var query = new ContractQueries.GetContractsQuery(CorrelationIdHelper.New(), Guid.NewGuid());

        _mockApiClient
            .Setup(c => c.GetContracts(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure("api error"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailed.ShouldBeTrue();

        _mockApiClient.Verify(c => c.GetContracts(query, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_GetContractQuery_ReturnsSuccess_WhenApiClientSucceeds()
    {
        // Arrange
        var estateId = Guid.NewGuid();
        var contractId = Guid.NewGuid();
        var query = new ContractQueries.GetContractQuery(CorrelationIdHelper.New(), estateId, contractId);
        var contract = new ContractModels.ContractModel
        {
            ContractId = contractId,
            Description = "Contract 1",
            OperatorName = "Operator 1"
        };

        _mockApiClient
            .Setup(c => c.GetContract(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(contract));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data!.ContractId.ShouldBe(contractId);

        _mockApiClient.Verify(c => c.GetContract(query, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_GetContractQuery_ReturnsFailure_WhenApiClientFails()
    {
        // Arrange
        var query = new ContractQueries.GetContractQuery(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid());

        _mockApiClient
            .Setup(c => c.GetContract(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure("api error"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailed.ShouldBeTrue();

        _mockApiClient.Verify(c => c.GetContract(query, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CreateContractCommand_ReturnsSuccess_WhenApiClientSucceeds()
    {
        // Arrange
        var command = new ContractCommands.CreateContractCommand(CorrelationIdHelper.New(), Guid.NewGuid(), "Test Contract", Guid.NewGuid());

        _mockApiClient
            .Setup(c => c.CreateContract(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();

        _mockApiClient.Verify(c => c.CreateContract(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CreateContractCommand_ReturnsFailure_WhenApiClientFails()
    {
        // Arrange
        var command = new ContractCommands.CreateContractCommand(CorrelationIdHelper.New(), Guid.NewGuid(), "Test Contract", Guid.NewGuid());

        _mockApiClient
            .Setup(c => c.CreateContract(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure("api error"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailed.ShouldBeTrue();

        _mockApiClient.Verify(c => c.CreateContract(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_AddProductToContractCommand_ReturnsSuccess_WhenApiClientSucceeds()
    {
        // Arrange
        var command = new ContractCommands.AddProductToContractCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), "Product 1", "Product Display", 9.99m);

        _mockApiClient
            .Setup(c => c.AddProductToContract(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();

        _mockApiClient.Verify(c => c.AddProductToContract(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_AddProductToContractCommand_ReturnsFailure_WhenApiClientFails()
    {
        // Arrange
        var command = new ContractCommands.AddProductToContractCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), "Product 1", "Product Display", 9.99m);

        _mockApiClient
            .Setup(c => c.AddProductToContract(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure("api error"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailed.ShouldBeTrue();

        _mockApiClient.Verify(c => c.AddProductToContract(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_AddTransactionFeeToProductCommand_ReturnsSuccess_WhenApiClientSucceeds()
    {
        // Arrange
        var command = new ContractCommands.AddTransactionFeeToProductCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Fee Description", 1.50m, "Fixed", "Merchant");

        _mockApiClient
            .Setup(c => c.AddTransactionFeeToProduct(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();

        _mockApiClient.Verify(c => c.AddTransactionFeeToProduct(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_AddTransactionFeeToProductCommand_ReturnsFailure_WhenApiClientFails()
    {
        // Arrange
        var command = new ContractCommands.AddTransactionFeeToProductCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Fee Description", 1.50m, "Fixed", "Merchant");

        _mockApiClient
            .Setup(c => c.AddTransactionFeeToProduct(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure("api error"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailed.ShouldBeTrue();

        _mockApiClient.Verify(c => c.AddTransactionFeeToProduct(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_RemoveTransactionFeeFromProductCommand_ReturnsSuccess_WhenApiClientSucceeds()
    {
        // Arrange
        var command = new ContractCommands.RemoveTransactionFeeFromProductCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        _mockApiClient
            .Setup(c => c.RemoveTransactionFeeFromProduct(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();

        _mockApiClient.Verify(c => c.RemoveTransactionFeeFromProduct(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_RemoveTransactionFeeFromProductCommand_ReturnsFailure_WhenApiClientFails()
    {
        // Arrange
        var command = new ContractCommands.RemoveTransactionFeeFromProductCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        _mockApiClient
            .Setup(c => c.RemoveTransactionFeeFromProduct(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure("api error"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailed.ShouldBeTrue();

        _mockApiClient.Verify(c => c.RemoveTransactionFeeFromProduct(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_GetRecentContractsQuery_ReturnsSuccess_WhenApiClientSucceeds()
    {
        // Arrange
        var estateId = Guid.NewGuid();
        var query = new ContractQueries.GetRecentContractsQuery(CorrelationIdHelper.New(), estateId);
        var recentContracts = new List<ContractModels.RecentContractModel>
        {
            new() { ContractId = Guid.NewGuid(), Description = "Contract 1", OperatorName = "Operator 1" }
        };

        _mockApiClient
            .Setup(c => c.GetRecentContracts(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(recentContracts));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data!.Count.ShouldBe(1);

        _mockApiClient.Verify(c => c.GetRecentContracts(query, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_GetRecentContractsQuery_ReturnsFailure_WhenApiClientFails()
    {
        // Arrange
        var query = new ContractQueries.GetRecentContractsQuery(CorrelationIdHelper.New(), Guid.NewGuid());

        _mockApiClient
            .Setup(c => c.GetRecentContracts(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure("api error"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailed.ShouldBeTrue();

        _mockApiClient.Verify(c => c.GetRecentContracts(query, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_GetContractsForDropDownQuery_ReturnsSuccess_WhenApiClientSucceeds()
    {
        // Arrange
        var estateId = Guid.NewGuid();
        var query = new ContractQueries.GetContractsForDropDownQuery(CorrelationIdHelper.New(), estateId);
        var dropDownContracts = new List<ContractModels.ContractDropDownModel>
        {
            new() { ContractId = Guid.NewGuid(), Description = "Contract 1", OperatorName = "Operator 1" },
            new() { ContractId = Guid.NewGuid(), Description = "Contract 2", OperatorName = "Operator 2" }
        };

        _mockApiClient
            .Setup(c => c.GetContracts(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(dropDownContracts));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data!.Count.ShouldBe(2);

        _mockApiClient.Verify(c => c.GetContracts(query, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_GetContractsForDropDownQuery_ReturnsFailure_WhenApiClientFails()
    {
        // Arrange
        var query = new ContractQueries.GetContractsForDropDownQuery(CorrelationIdHelper.New(), Guid.NewGuid());

        _mockApiClient
            .Setup(c => c.GetContracts(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure("api error"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailed.ShouldBeTrue();

        _mockApiClient.Verify(c => c.GetContracts(query, It.IsAny<CancellationToken>()), Times.Once);
    }
}
