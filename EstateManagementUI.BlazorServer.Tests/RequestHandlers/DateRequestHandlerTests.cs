using EstateManagementUI.BusinessLogic.Client;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.RequestHandlers;
using EstateManagementUI.BusinessLogic.Requests;
using Imposter.Abstractions;
using Shouldly;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.Tests.RequestHandlers;

public class DateRequestHandlerTests
{
    private readonly IApiClientImposter _mockApiClient;
    private readonly DateRequestHandler _handler;

    public DateRequestHandlerTests()
    {
        _mockApiClient = new IApiClientImposter();
        _handler = new DateRequestHandler(_mockApiClient.Instance());
    }

    [Fact]
    public async Task Handle_ReturnsSuccess_WhenApiClientSucceeds()
    {
        // Arrange
        var estateId = Guid.NewGuid();
        var query = new DateQueries.GetComparisonDatesQuery(CorrelationIdHelper.New(), estateId);
        var dates = new List<ComparisonDateModel>
        {
            new() { Date = DateTime.UtcNow.Date.AddDays(-1), Description = "Yesterday" },
            new() { Date = DateTime.UtcNow.Date.AddDays(-7), Description = "Last Week" }
        };

        _mockApiClient
            .GetComparisonDates(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(dates));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data!.Count.ShouldBe(2);
        result.Data[0].Description.ShouldBe("Yesterday");
        result.Data[1].Description.ShouldBe("Last Week");

        _mockApiClient.GetComparisonDates(query, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenApiClientFails()
    {
        // Arrange
        var query = new DateQueries.GetComparisonDatesQuery(CorrelationIdHelper.New(), Guid.NewGuid());

        _mockApiClient
            .GetComparisonDates(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("api error"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailed.ShouldBeTrue();

        _mockApiClient.GetComparisonDates(query, Arg<CancellationToken>.Any()).Called(Count.Once());
    }
}
