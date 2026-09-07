using EstateManagementUI.BusinessLogic.Client;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.RequestHandlers;
using EstateManagementUI.BusinessLogic.Requests;
using Imposter.Abstractions;
using Shouldly;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.Tests.RequestHandlers;

public class TransactionRequestHandlerTests
{
    private readonly IApiClientImposter _mockApiClient;
    private readonly TransactionRequestHandler _handler;
    private readonly SettlementRequestHandler _settlementHandler;

    public TransactionRequestHandlerTests()
    {
        _mockApiClient = new IApiClientImposter();
        _handler = new TransactionRequestHandler(_mockApiClient.Instance());
        _settlementHandler = new SettlementRequestHandler(_mockApiClient.Instance());
    }

    [Fact]
    public async Task Handle_GetTodaysSalesQuery_ReturnsSuccess_WhenApiClientSucceeds()
    {
        // Arrange
        var estateId = Guid.NewGuid();
        var comparisonDate = DateTime.UtcNow.Date;
        var query = new TransactionQueries.GetTodaysSalesQuery(CorrelationIdHelper.New(), estateId, comparisonDate);
        var model = new TodaysSalesModel { TodaysSalesCount = 5, TodaysSalesValue = 100m };

        _mockApiClient
            .GetTodaysSales(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(model));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data!.TodaysSalesCount.ShouldBe(5);
        result.Data.TodaysSalesValue.ShouldBe(100m);

        _mockApiClient.GetTodaysSales(query, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_GetTodaysSalesQuery_ReturnsFailure_WhenApiClientFails()
    {
        // Arrange
        var query = new TransactionQueries.GetTodaysSalesQuery(CorrelationIdHelper.New(), Guid.NewGuid(), DateTime.UtcNow.Date);

        _mockApiClient
            .GetTodaysSales(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("api error"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailed.ShouldBeTrue();

        _mockApiClient.GetTodaysSales(query, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_GetTodaysFailedSalesQuery_ReturnsSuccess_WhenApiClientSucceeds()
    {
        // Arrange
        var estateId = Guid.NewGuid();
        var comparisonDate = DateTime.UtcNow.Date;
        var query = new TransactionQueries.GetTodaysFailedSalesQuery(CorrelationIdHelper.New(), estateId, "RC1", comparisonDate);
        var model = new TodaysSalesModel { TodaysSalesCount = 3, TodaysSalesValue = 30m };

        _mockApiClient
            .GetTodaysFailedSales(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(model));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data!.TodaysSalesCount.ShouldBe(3);

        _mockApiClient.GetTodaysFailedSales(query, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_GetTodaysFailedSalesQuery_ReturnsFailure_WhenApiClientFails()
    {
        // Arrange
        var query = new TransactionQueries.GetTodaysFailedSalesQuery(CorrelationIdHelper.New(), Guid.NewGuid(), "RC1", DateTime.UtcNow.Date);

        _mockApiClient
            .GetTodaysFailedSales(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("api error"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailed.ShouldBeTrue();

        _mockApiClient.GetTodaysFailedSales(query, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_GetTransactionDetailQuery_ReturnsSuccess_WhenApiClientSucceeds()
    {
        // Arrange
        var estateId = Guid.NewGuid();
        var startDate = DateTime.UtcNow.AddDays(-7);
        var endDate = DateTime.UtcNow;
        var query = new TransactionQueries.GetTransactionDetailQuery(CorrelationIdHelper.New(), estateId, startDate, endDate);
        var model = new TransactionModels.TransactionDetailReportResponse
        {
            Transactions = new List<TransactionModels.TransactionDetail>(),
            Summary = new TransactionModels.TransactionDetailSummary { TransactionCount = 10 }
        };

        _mockApiClient
            .GetTransactionDetailReport(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(model));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data!.Summary.TransactionCount.ShouldBe(10);

        _mockApiClient.GetTransactionDetailReport(query, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_GetTransactionDetailQuery_ReturnsFailure_WhenApiClientFails()
    {
        // Arrange
        var query = new TransactionQueries.GetTransactionDetailQuery(CorrelationIdHelper.New(), Guid.NewGuid(), DateTime.UtcNow.AddDays(-7), DateTime.UtcNow);

        _mockApiClient
            .GetTransactionDetailReport(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("api error"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailed.ShouldBeTrue();

        _mockApiClient.GetTransactionDetailReport(query, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_GetMerchantTransactionSummaryQuery_ReturnsSuccess_WhenApiClientSucceeds()
    {
        // Arrange
        var estateId = Guid.NewGuid();
        var startDate = DateTime.UtcNow.AddDays(-7);
        var endDate = DateTime.UtcNow;
        var query = new TransactionQueries.GetMerchantTransactionSummaryQuery(CorrelationIdHelper.New(), estateId, startDate, endDate, 1, 2);
        var model = new TransactionModels.TransactionSummaryByMerchantResponse
        {
            Merchants = new List<TransactionModels.MerchantDetail>(),
            Summary = new TransactionModels.MerchantDetailSummary { TotalMerchants = 2 }
        };

        _mockApiClient
            .GetMerchantTransactionSummary(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(model));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data!.Summary.TotalMerchants.ShouldBe(2);

        _mockApiClient.GetMerchantTransactionSummary(query, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_GetMerchantTransactionSummaryQuery_ReturnsFailure_WhenApiClientFails()
    {
        // Arrange
        var query = new TransactionQueries.GetMerchantTransactionSummaryQuery(CorrelationIdHelper.New(), Guid.NewGuid(), DateTime.UtcNow.AddDays(-7), DateTime.UtcNow);

        _mockApiClient
            .GetMerchantTransactionSummary(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("api error"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailed.ShouldBeTrue();

        _mockApiClient.GetMerchantTransactionSummary(query, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_GetOperatorTransactionSummaryQuery_ReturnsSuccess_WhenApiClientSucceeds()
    {
        // Arrange
        var estateId = Guid.NewGuid();
        var startDate = DateTime.UtcNow.AddDays(-7);
        var endDate = DateTime.UtcNow;
        var query = new TransactionQueries.GetOperatorTransactionSummaryQuery(CorrelationIdHelper.New(), estateId, startDate, endDate, 3, 4);
        var model = new TransactionModels.TransactionSummaryByOperatorResponse
        {
            Operators = new List<TransactionModels.OperatorDetail>(),
            Summary = new TransactionModels.OperatorDetailSummary { TotalOperators = 3 }
        };

        _mockApiClient
            .GetOperatorTransactionSummary(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(model));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data!.Summary.TotalOperators.ShouldBe(3);

        _mockApiClient.GetOperatorTransactionSummary(query, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_GetOperatorTransactionSummaryQuery_ReturnsFailure_WhenApiClientFails()
    {
        // Arrange
        var query = new TransactionQueries.GetOperatorTransactionSummaryQuery(CorrelationIdHelper.New(), Guid.NewGuid(), DateTime.UtcNow.AddDays(-7), DateTime.UtcNow);

        _mockApiClient
            .GetOperatorTransactionSummary(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("api error"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailed.ShouldBeTrue();

        _mockApiClient.GetOperatorTransactionSummary(query, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_GetProductPerformanceQuery_ReturnsSuccess_WhenApiClientSucceeds()
    {
        // Arrange
        var estateId = Guid.NewGuid();
        var startDate = DateTime.UtcNow.AddDays(-7);
        var endDate = DateTime.UtcNow;
        var query = new TransactionQueries.GetProductPerformanceQuery(CorrelationIdHelper.New(), estateId, startDate, endDate);
        var model = new TransactionModels.ProductPerformanceResponse
        {
            ProductDetails = new List<TransactionModels.ProductPerformanceDetail>(),
            Summary = new TransactionModels.ProductPerformanceSummary { TotalProducts = 4 }
        };

        _mockApiClient
            .GetProductPerformance(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(model));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data!.Summary.TotalProducts.ShouldBe(4);

        _mockApiClient.GetProductPerformance(query, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_GetProductPerformanceQuery_ReturnsFailure_WhenApiClientFails()
    {
        // Arrange
        var query = new TransactionQueries.GetProductPerformanceQuery(CorrelationIdHelper.New(), Guid.NewGuid(), DateTime.UtcNow.AddDays(-7), DateTime.UtcNow);

        _mockApiClient
            .GetProductPerformance(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("api error"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailed.ShouldBeTrue();

        _mockApiClient.GetProductPerformance(query, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_GetTodaysSalesByHourQuery_ReturnsSuccess_WhenApiClientSucceeds()
    {
        // Arrange
        var estateId = Guid.NewGuid();
        var comparisonDate = DateTime.UtcNow.Date;
        var query = new TransactionQueries.GetTodaysSalesByHourQuery(CorrelationIdHelper.New(), estateId, comparisonDate);
        var model = new List<TransactionModels.TodaysSalesByHourModel>
        {
            new() { Hour = 9, TodaysSalesCount = 7, TodaysSalesValue = 70m }
        };

        _mockApiClient
            .GetTodaysSalesByHour(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(model));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data!.Count.ShouldBe(1);
        result.Data[0].Hour.ShouldBe(9);

        _mockApiClient.GetTodaysSalesByHour(query, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_GetTodaysSalesByHourQuery_ReturnsFailure_WhenApiClientFails()
    {
        // Arrange
        var query = new TransactionQueries.GetTodaysSalesByHourQuery(CorrelationIdHelper.New(), Guid.NewGuid(), DateTime.UtcNow.Date);

        _mockApiClient
            .GetTodaysSalesByHour(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("api error"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailed.ShouldBeTrue();

        _mockApiClient.GetTodaysSalesByHour(query, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_GetTodaysSettlementQuery_ReturnsSuccess_WhenApiClientSucceeds()
    {
        // Arrange
        var estateId = Guid.NewGuid();
        var comparisonDate = DateTime.UtcNow.Date;
        var query = new SettlementQueries.GetTodaysSettlementQuery(CorrelationIdHelper.New(), estateId, comparisonDate);
        var model = new TodaysSettlementModel { TodaysSettlementValue = 500m };

        _mockApiClient
            .GetTodaysSettlement(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(model));

        // Act
        var result = await _settlementHandler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data!.TodaysSettlementValue.ShouldBe(500m);

        _mockApiClient.GetTodaysSettlement(query, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_GetTodaysSettlementQuery_ReturnsFailure_WhenApiClientFails()
    {
        // Arrange
        var query = new SettlementQueries.GetTodaysSettlementQuery(CorrelationIdHelper.New(), Guid.NewGuid(), DateTime.UtcNow.Date);

        _mockApiClient
            .GetTodaysSettlement(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("api error"));

        // Act
        var result = await _settlementHandler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailed.ShouldBeTrue();

        _mockApiClient.GetTodaysSettlement(query, Arg<CancellationToken>.Any()).Called(Count.Once());
    }
}
