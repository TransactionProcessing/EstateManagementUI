using EstateManagementUI.BusinessLogic.Client;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.RequestHandlers;
using EstateManagementUI.BusinessLogic.Requests;
using Moq;
using Shouldly;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.Tests.RequestHandlers;

public class DateRequestHandlerTests
{
    private readonly Mock<IApiClient> _mockApiClient;
    private readonly DateRequestHandler _handler;

    public DateRequestHandlerTests()
    {
        _mockApiClient = new Mock<IApiClient>();
        _handler = new DateRequestHandler(_mockApiClient.Object);
    }

    [Fact]
    public async Task Handle_ReturnsSuccess_WhenApiClientSucceeds()
    {
        // Arrange
        var estateId = Guid.NewGuid();
        var query = new Queries.GetComparisonDatesQuery(CorrelationIdHelper.New(), estateId);
        var dates = new List<ComparisonDateModel>
        {
            new() { Date = DateTime.UtcNow.Date.AddDays(-1), Description = "Yesterday" },
            new() { Date = DateTime.UtcNow.Date.AddDays(-7), Description = "Last Week" }
        };

        _mockApiClient
            .Setup(c => c.GetComparisonDates(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(dates));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data!.Count.ShouldBe(2);
        result.Data[0].Description.ShouldBe("Yesterday");
        result.Data[1].Description.ShouldBe("Last Week");

        _mockApiClient.Verify(c => c.GetComparisonDates(query, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenApiClientFails()
    {
        // Arrange
        var query = new Queries.GetComparisonDatesQuery(CorrelationIdHelper.New(), Guid.NewGuid());

        _mockApiClient
            .Setup(c => c.GetComparisonDates(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure("api error"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailed.ShouldBeTrue();

        _mockApiClient.Verify(c => c.GetComparisonDates(query, It.IsAny<CancellationToken>()), Times.Once);
    }
}
