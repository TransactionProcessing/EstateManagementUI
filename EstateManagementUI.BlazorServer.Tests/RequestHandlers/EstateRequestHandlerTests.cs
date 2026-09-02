using EstateManagementUI.BusinessLogic.Client;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.RequestHandlers;
using EstateManagementUI.BusinessLogic.Requests;
using Imposter.Abstractions;
using Shouldly;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.Tests.RequestHandlers;

public class EstateRequestHandlerTests
{
    private readonly IApiClientImposter _mockApiClient;
    private readonly EstateRequestHandler _handler;

    public EstateRequestHandlerTests()
    {
        _mockApiClient = new IApiClientImposter();
        _handler = new EstateRequestHandler(_mockApiClient.Instance());
    }

    [Fact]
    public async Task Handle_GetEstateQuery_ReturnsSuccess_WhenApiClientSucceeds()
    {
        // Arrange
        var estateId = Guid.NewGuid();
        var query = new EstateQueries.GetEstateQuery(CorrelationIdHelper.New(), estateId);
        var estateModel = new EstateModels.EstateModel
        {
            EstateId = estateId,
            EstateName = "Test Estate"
        };

        _mockApiClient
            .GetEstate(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(estateModel));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data!.EstateId.ShouldBe(estateId);
        result.Data.EstateName.ShouldBe("Test Estate");

        _mockApiClient.GetEstate(query, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_GetEstateQuery_ReturnsFailure_WhenApiClientFails()
    {
        // Arrange
        var query = new EstateQueries.GetEstateQuery(CorrelationIdHelper.New(), Guid.NewGuid());

        _mockApiClient
            .GetEstate(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("api error"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailed.ShouldBeTrue();

        _mockApiClient.GetEstate(query, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_AddOperatorToEstateCommand_ReturnsSuccess_WhenApiClientSucceeds()
    {
        // Arrange
        var command = new EstateCommands.AddOperatorToEstateCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid());

        _mockApiClient
            .AddEstateOperator(command, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();

        _mockApiClient.AddEstateOperator(command, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_AddOperatorToEstateCommand_ReturnsFailure_WhenApiClientFails()
    {
        // Arrange
        var command = new EstateCommands.AddOperatorToEstateCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid());

        _mockApiClient
            .AddEstateOperator(command, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("api error"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailed.ShouldBeTrue();

        _mockApiClient.AddEstateOperator(command, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_RemoveOperatorFromEstateCommand_ReturnsSuccess_WhenApiClientSucceeds()
    {
        // Arrange
        var command = new EstateCommands.RemoveOperatorFromEstateCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid());

        _mockApiClient
            .RemoveEstateOperator(command, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();

        _mockApiClient.RemoveEstateOperator(command, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_RemoveOperatorFromEstateCommand_ReturnsFailure_WhenApiClientFails()
    {
        // Arrange
        var command = new EstateCommands.RemoveOperatorFromEstateCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid());

        _mockApiClient
            .RemoveEstateOperator(command, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("api error"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailed.ShouldBeTrue();

        _mockApiClient.RemoveEstateOperator(command, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_GetAssignedOperatorsQuery_ReturnsSuccess_WhenApiClientSucceeds()
    {
        // Arrange
        var estateId = Guid.NewGuid();
        var query = new EstateQueries.GetAssignedOperatorsQuery(CorrelationIdHelper.New(), estateId);
        var operators = new List<OperatorModels.OperatorModel>
        {
            new() { OperatorId = Guid.NewGuid(), Name = "Operator 1" },
            new() { OperatorId = Guid.NewGuid(), Name = "Operator 2" }
        };

        _mockApiClient
            .GetEstateAssignedOperators(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(operators));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data!.Count.ShouldBe(2);
        result.Data[0].Name.ShouldBe("Operator 1");
        result.Data[1].Name.ShouldBe("Operator 2");

        _mockApiClient.GetEstateAssignedOperators(query, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_GetAssignedOperatorsQuery_ReturnsFailure_WhenApiClientFails()
    {
        // Arrange
        var query = new EstateQueries.GetAssignedOperatorsQuery(CorrelationIdHelper.New(), Guid.NewGuid());

        _mockApiClient
            .GetEstateAssignedOperators(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("api error"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailed.ShouldBeTrue();

        _mockApiClient.GetEstateAssignedOperators(query, Arg<CancellationToken>.Any()).Called(Count.Once());
    }
}
