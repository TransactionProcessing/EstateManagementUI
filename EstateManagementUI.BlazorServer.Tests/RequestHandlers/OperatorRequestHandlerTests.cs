using EstateManagementUI.BusinessLogic.Client;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.RequestHandlers;
using EstateManagementUI.BusinessLogic.Requests;
using Imposter.Abstractions;
using Shouldly;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.Tests.RequestHandlers;

public class OperatorRequestHandlerTests
{
    private readonly IApiClientImposter _mockApiClient;
    private readonly OperatorRequestHandler _handler;

    public OperatorRequestHandlerTests()
    {
        _mockApiClient = new IApiClientImposter();
        _handler = new OperatorRequestHandler(_mockApiClient.Instance());
    }

    [Fact]
    public async Task Handle_GetOperatorsQuery_ReturnsSuccess_WhenApiClientSucceeds()
    {
        // Arrange
        var query = new OperatorQueries.GetOperatorsQuery(CorrelationIdHelper.New(), Guid.NewGuid());
        var operators = new List<OperatorModels.OperatorModel>
        {
            new() { OperatorId = Guid.NewGuid(), Name = "Operator1" },
            new() { OperatorId = Guid.NewGuid(), Name = "Operator2" }
        };

        _mockApiClient
            .GetOperators(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(operators));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data!.Count.ShouldBe(2);
        result.Data[0].Name.ShouldBe("Operator1");
        result.Data[1].Name.ShouldBe("Operator2");

        _mockApiClient.GetOperators(query, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_GetOperatorsQuery_ReturnsFailure_WhenApiClientFails()
    {
        // Arrange
        var query = new OperatorQueries.GetOperatorsQuery(CorrelationIdHelper.New(), Guid.NewGuid());

        _mockApiClient
            .GetOperators(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("api error"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailed.ShouldBeTrue();

        _mockApiClient.GetOperators(query, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_GetOperatorQuery_ReturnsSuccess_WhenApiClientSucceeds()
    {
        // Arrange
        var operatorId = Guid.NewGuid();
        var query = new OperatorQueries.GetOperatorQuery(CorrelationIdHelper.New(), Guid.NewGuid(), operatorId);
        var operatorModel = new OperatorModels.OperatorModel
        {
            OperatorId = operatorId,
            Name = "Operator1",
            RequireCustomMerchantNumber = true,
            RequireCustomTerminalNumber = false
        };

        _mockApiClient
            .GetOperator(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(operatorModel));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data!.OperatorId.ShouldBe(operatorId);
        result.Data.Name.ShouldBe("Operator1");

        _mockApiClient.GetOperator(query, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_GetOperatorQuery_ReturnsFailure_WhenApiClientFails()
    {
        // Arrange
        var query = new OperatorQueries.GetOperatorQuery(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid());

        _mockApiClient
            .GetOperator(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("api error"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailed.ShouldBeTrue();

        _mockApiClient.GetOperator(query, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_CreateOperatorCommand_ReturnsSuccess_WhenApiClientSucceeds()
    {
        // Arrange
        var command = new OperatorCommands.CreateOperatorCommand(CorrelationIdHelper.New(), Guid.NewGuid(), "NewOperator", true, false);

        _mockApiClient
            .CreateOperator(command, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();

        _mockApiClient.CreateOperator(command, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_CreateOperatorCommand_ReturnsFailure_WhenApiClientFails()
    {
        // Arrange
        var command = new OperatorCommands.CreateOperatorCommand(CorrelationIdHelper.New(), Guid.NewGuid(), "NewOperator", true, false);

        _mockApiClient
            .CreateOperator(command, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("api error"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailed.ShouldBeTrue();

        _mockApiClient.CreateOperator(command, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_UpdateOperatorCommand_ReturnsSuccess_WhenApiClientSucceeds()
    {
        // Arrange
        var command = new OperatorCommands.UpdateOperatorCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), "UpdatedOperator", false, true);

        _mockApiClient
            .UpdateOperator(command, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();

        _mockApiClient.UpdateOperator(command, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_UpdateOperatorCommand_ReturnsFailure_WhenApiClientFails()
    {
        // Arrange
        var command = new OperatorCommands.UpdateOperatorCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), "UpdatedOperator", false, true);

        _mockApiClient
            .UpdateOperator(command, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("api error"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailed.ShouldBeTrue();

        _mockApiClient.UpdateOperator(command, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_GetOperatorsForDropDownQuery_ReturnsSuccess_WhenApiClientSucceeds()
    {
        // Arrange
        var query = new OperatorQueries.GetOperatorsForDropDownQuery(CorrelationIdHelper.New(), Guid.NewGuid());
        var dropDownModels = new List<OperatorModels.OperatorDropDownModel>
        {
            new() { OperatorId = Guid.NewGuid(), OperatorName = "Operator1", OperatorReportingId = 1 },
            new() { OperatorId = Guid.NewGuid(), OperatorName = "Operator2", OperatorReportingId = 2 }
        };

        _mockApiClient
            .GetOperators(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(dropDownModels));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data!.Count.ShouldBe(2);
        result.Data[0].OperatorName.ShouldBe("Operator1");
        result.Data[1].OperatorName.ShouldBe("Operator2");

        _mockApiClient.GetOperators(query, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_GetOperatorsForDropDownQuery_ReturnsFailure_WhenApiClientFails()
    {
        // Arrange
        var query = new OperatorQueries.GetOperatorsForDropDownQuery(CorrelationIdHelper.New(), Guid.NewGuid());

        _mockApiClient
            .GetOperators(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("api error"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailed.ShouldBeTrue();

        _mockApiClient.GetOperators(query, Arg<CancellationToken>.Any()).Called(Count.Once());
    }
}
