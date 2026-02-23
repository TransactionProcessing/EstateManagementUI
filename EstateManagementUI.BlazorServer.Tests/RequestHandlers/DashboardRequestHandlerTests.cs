using EstateManagementUI.BusinessLogic.Client;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.RequestHandlers;
using EstateManagementUI.BusinessLogic.Requests;
using Moq;
using Shouldly;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.Tests.RequestHandlers;

public class DashboardRequestHandlerTests
{
    private readonly Mock<IApiClient> _mockApiClient;
    private readonly DashboardRequestHandler _handler;

    public DashboardRequestHandlerTests()
    {
        _mockApiClient = new Mock<IApiClient>();
        _handler = new DashboardRequestHandler(_mockApiClient.Object);
    }

    [Fact]
    public async Task Handle_GetTopProductDataQuery_ReturnsSuccess()
    {
        // Arrange
        var query = new Queries.GetTopProductDataQuery(CorrelationIdHelper.New(), "token", Guid.NewGuid(), 5);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data.Count.ShouldBe(StubTestData.GetMockTopProducts().Count);
        result.Data[0].ProductName.ShouldBe(StubTestData.GetMockTopProducts()[0].ProductName);
        result.Data[0].SalesValue.ShouldBe(StubTestData.GetMockTopProducts()[0].SalesValue);
    }

    [Fact]
    public async Task Handle_GetBottomProductDataQuery_ReturnsSuccess()
    {
        // Arrange
        var query = new Queries.GetBottomProductDataQuery(CorrelationIdHelper.New(), "token", Guid.NewGuid(), 5);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data.Count.ShouldBe(StubTestData.GetMockBottomProducts().Count);
        result.Data[0].ProductName.ShouldBe(StubTestData.GetMockBottomProducts()[0].ProductName);
        result.Data[0].SalesValue.ShouldBe(StubTestData.GetMockBottomProducts()[0].SalesValue);
    }

    [Fact]
    public async Task Handle_GetTopMerchantDataQuery_ReturnsSuccess()
    {
        // Arrange
        var query = new Queries.GetTopMerchantDataQuery(CorrelationIdHelper.New(), "token", Guid.NewGuid(), 5);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data.Count.ShouldBe(StubTestData.GetMockTopMerchants().Count);
        result.Data[0].MerchantName.ShouldBe(StubTestData.GetMockTopMerchants()[0].MerchantName);
        result.Data[0].SalesValue.ShouldBe(StubTestData.GetMockTopMerchants()[0].SalesValue);
    }

    [Fact]
    public async Task Handle_GetBottomMerchantDataQuery_ReturnsSuccess()
    {
        // Arrange
        var query = new Queries.GetBottomMerchantDataQuery(CorrelationIdHelper.New(), "token", Guid.NewGuid(), 5);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data.Count.ShouldBe(StubTestData.GetMockBottomMerchants().Count);
        result.Data[0].MerchantName.ShouldBe(StubTestData.GetMockBottomMerchants()[0].MerchantName);
        result.Data[0].SalesValue.ShouldBe(StubTestData.GetMockBottomMerchants()[0].SalesValue);
    }

    [Fact]
    public async Task Handle_GetTopOperatorDataQuery_ReturnsSuccess()
    {
        // Arrange
        var query = new Queries.GetTopOperatorDataQuery(CorrelationIdHelper.New(), "token", Guid.NewGuid(), 5);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data.Count.ShouldBe(StubTestData.GetMockTopOperators().Count);
        result.Data[0].OperatorName.ShouldBe(StubTestData.GetMockTopOperators()[0].OperatorName);
        result.Data[0].SalesValue.ShouldBe(StubTestData.GetMockTopOperators()[0].SalesValue);
    }

    [Fact]
    public async Task Handle_GetBottomOperatorDataQuery_ReturnsSuccess()
    {
        // Arrange
        var query = new Queries.GetBottomOperatorDataQuery(CorrelationIdHelper.New(), "token", Guid.NewGuid(), 5);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data.Count.ShouldBe(StubTestData.GetMockBottomOperators().Count);
        result.Data[0].OperatorName.ShouldBe(StubTestData.GetMockBottomOperators()[0].OperatorName);
        result.Data[0].SalesValue.ShouldBe(StubTestData.GetMockBottomOperators()[0].SalesValue);
    }

    [Fact]
    public async Task Handle_GetLastSettlementQuery_ReturnsSuccess()
    {
        // Arrange
        var query = new Queries.GetLastSettlementQuery(CorrelationIdHelper.New(), "token", Guid.NewGuid());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data.SettlementDate.ShouldBe(StubTestData.GetMockLastSettlement().SettlementDate);
        result.Data.FeesValue.ShouldBe(StubTestData.GetMockLastSettlement().FeesValue);
        result.Data.SalesCount.ShouldBe(StubTestData.GetMockLastSettlement().SalesCount);
        result.Data.SalesValue.ShouldBe(StubTestData.GetMockLastSettlement().SalesValue);
        result.Data.SettlementValue.ShouldBe(StubTestData.GetMockLastSettlement().SettlementValue);
    }
}
