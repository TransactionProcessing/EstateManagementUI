using EstateManagementUI.BlazorServer.UIServices;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using Moq;
using Shouldly;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.Tests.UIServices;

public class TransactionUIServiceTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly TransactionUIService _service;

    public TransactionUIServiceTests()
    {
        this._mockMediator = new Mock<IMediator>();
        this._service = new TransactionUIService(this._mockMediator.Object);
    }

    [Fact]
    public async Task GetTodaysSales_CallsMediatorWithCorrectQuery_AndReturnsSuccess()
    {
        var estateId = Guid.NewGuid();
        var comparisonDate = DateTime.UtcNow.Date;

        var biz = new BusinessLogic.Models.TodaysSalesModel
        {
            TodaysSalesValue = 123.45m,
            TodaysSalesCount = 10
        };

        this._mockMediator
            .Setup(m => m.Send(It.IsAny<TransactionQueries.GetTodaysSalesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(biz));

        var result = await this._service.GetTodaysSales(CorrelationIdHelper.New(), estateId, comparisonDate);

        result.IsSuccess.ShouldBeTrue();
        result.Data!.TodaysSalesCount.ShouldBe(10);

        this._mockMediator.Verify(m =>
            m.Send(It.Is<TransactionQueries.GetTodaysSalesQuery>(q =>
                q.EstateId == estateId && q.ComparisonDate.Date == comparisonDate.Date
            ), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetTodaysSales_ReturnsFailure_WhenMediatorFails()
    {
        this._mockMediator
            .Setup(m => m.Send(It.IsAny<TransactionQueries.GetTodaysSalesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure("err"));

        var result = await this._service.GetTodaysSales(CorrelationIdHelper.New(), Guid.NewGuid(), DateTime.UtcNow);

        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task GetTodaysFailedSales_PassesResponseCodeAndReturnsSuccess()
    {
        var estateId = Guid.NewGuid();
        var comparisonDate = DateTime.UtcNow.Date;
        var responseCode = "RC";

        var biz = new BusinessLogic.Models.TodaysSalesModel
        {
            TodaysSalesValue = 1.23m,
            TodaysSalesCount = 2
        };

        this._mockMediator
            .Setup(m => m.Send(It.IsAny<TransactionQueries.GetTodaysFailedSalesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(biz));

        var result = await this._service.GetTodaysFailedSales(CorrelationIdHelper.New(), estateId, responseCode, comparisonDate);

        result.IsSuccess.ShouldBeTrue();
        this._mockMediator.Verify(m =>
            m.Send(It.Is<TransactionQueries.GetTodaysFailedSalesQuery>(q =>
                q.EstateId == estateId && q.ResponseCode == responseCode && q.ComparisonDate.Date == comparisonDate.Date
            ), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetTodaysSalesByHour_ReturnsListAndVerifiesQuery()
    {
        var estateId = Guid.NewGuid();
        var comparisonDate = DateTime.UtcNow.Date;

        var bizList = new List<BusinessLogic.Models.TransactionModels.TodaysSalesByHourModel>
        {
            new() { Hour = 8, TodaysSalesCount = 1, TodaysSalesValue = 10m }
        };

        this._mockMediator
            .Setup(m => m.Send(It.IsAny<TransactionQueries.GetTodaysSalesByHourQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(bizList));

        var result = await this._service.GetTodaysSalesByHour(CorrelationIdHelper.New(), estateId, comparisonDate);

        result.IsSuccess.ShouldBeTrue();
        result.Data!.Count.ShouldBe(1);

        this._mockMediator.Verify(m =>
            m.Send(It.Is<TransactionQueries.GetTodaysSalesByHourQuery>(q =>
                q.EstateId == estateId && q.ComparisonDate.Date == comparisonDate.Date
            ), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetTodaysSettlement_ReturnsMappedModel()
    {
        var estateId = Guid.NewGuid();
        var comparisonDate = DateTime.UtcNow.Date;

        var biz = new BusinessLogic.Models.TodaysSettlementModel
        {
            TodaysSettlementValue = 50m
        };

        this._mockMediator
            .Setup(m => m.Send(It.IsAny<SettlementQueries.GetTodaysSettlementQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(biz));

        var result = await this._service.GetTodaysSettlement(CorrelationIdHelper.New(), estateId, comparisonDate);

        result.IsSuccess.ShouldBeTrue();
        result.Data!.TodaysSettlementValue.ShouldBe(50m);
    }

    [Fact]
    public async Task GetProductPerformance_PassesDateRange_AndReturnsSuccess()
    {
        var estateId = Guid.NewGuid();
        var start = DateTime.UtcNow.AddDays(-7);
        var end = DateTime.UtcNow;

        var biz = new TransactionModels.ProductPerformanceResponse()
        {
            ProductDetails = new(),
            Summary = new()
        };

        this._mockMediator
            .Setup(m => m.Send(It.IsAny<TransactionQueries.GetProductPerformanceQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(biz));

        var result = await this._service.GetProductPerformance(CorrelationIdHelper.New(), estateId, start, end);

        result.IsSuccess.ShouldBeTrue();
        this._mockMediator.Verify(m =>
            m.Send(It.Is<TransactionQueries.GetProductPerformanceQuery>(q =>
                q.EstateId == estateId && q.StartDate == start && q.EndDate == end
            ), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetTransactionDetail_AllowsNullAndNonNullFilters()
    {
        var estateId = Guid.NewGuid();
        var start = DateTime.UtcNow.AddDays(-7);
        var end = DateTime.UtcNow;

        var biz = new BusinessLogic.Models.TransactionModels.TransactionDetailReportResponse
        {
            Transactions = new(),
            Summary = new()
        };

        this._mockMediator
            .Setup(m => m.Send(It.IsAny<TransactionQueries.GetTransactionDetailQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(biz));

        // Null filters
        var resultNull = await this._service.GetTransactionDetail(CorrelationIdHelper.New(), estateId, start, end, null, null, null);
        resultNull.IsSuccess.ShouldBeTrue();

        // Non-null filters
        var merchantIds = new List<int> { 1, 2 };
        var operatorIds = new List<int> { 3 };
        var productIds = new List<int> { 4 };

        var result = await this._service.GetTransactionDetail(CorrelationIdHelper.New(), estateId, start, end, merchantIds, operatorIds, productIds);
        result.IsSuccess.ShouldBeTrue();

        this._mockMediator.Verify(m =>
            m.Send(It.Is<TransactionQueries.GetTransactionDetailQuery>(q =>
                q.EstateId == estateId && q.MerchantIds == merchantIds && q.OperatorIds == operatorIds && q.ProductIds == productIds
            ), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetMerchantTransactionSummary_And_GetOperatorTransactionSummary_ReturnSuccess()
    {
        var estateId = Guid.NewGuid();
        var start = DateTime.UtcNow.AddDays(-7);
        var end = DateTime.UtcNow;

        var bizMerchant = new BusinessLogic.Models.TransactionModels.TransactionSummaryByMerchantResponse
        {
            Summary = new(),
            Merchants = new()
        };

        var bizOperator = new BusinessLogic.Models.TransactionModels.TransactionSummaryByOperatorResponse
        {
            Summary = new(),
            Operators = new()
        };

        this._mockMediator.Setup(m => m.Send(It.IsAny<TransactionQueries.GetMerchantTransactionSummaryQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(bizMerchant));

        this._mockMediator.Setup(m => m.Send(It.IsAny<TransactionQueries.GetOperatorTransactionSummaryQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(bizOperator));

        var merchantResult = await this._service.GetMerchantTransactionSummary(CorrelationIdHelper.New(), estateId, start, end, 5, 6);
        merchantResult.IsSuccess.ShouldBeTrue();

        var operatorResult = await this._service.GetOperatorTransactionSummary(CorrelationIdHelper.New(), estateId, start, end, 7, 8);
        operatorResult.IsSuccess.ShouldBeTrue();

        this._mockMediator.Verify(m => m.Send(It.Is<TransactionQueries.GetMerchantTransactionSummaryQuery>(q =>
            q.EstateId == estateId && q.MerchantId == 5 && q.OperatorId == 6
        ), It.IsAny<CancellationToken>()), Times.Once);

        this._mockMediator.Verify(m => m.Send(It.Is<TransactionQueries.GetOperatorTransactionSummaryQuery>(q =>
            q.EstateId == estateId && q.MerchantId == 7 && q.OperatorId == 8
        ), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetTodaysFailedSales_ReturnsFailure_WhenMediatorFails()
    {
        _mockMediator
            .Setup(m => m.Send(It.IsAny<TransactionQueries.GetTodaysFailedSalesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure("err"));

        var result = await _service.GetTodaysFailedSales(CorrelationIdHelper.New(), Guid.NewGuid(), "RC", DateTime.UtcNow);

        result.IsFailed.ShouldBeTrue();
        _mockMediator.Verify(m => m.Send(It.IsAny<TransactionQueries.GetTodaysFailedSalesQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetTodaysSalesByHour_ReturnsFailure_WhenMediatorFails()
    {
        _mockMediator
            .Setup(m => m.Send(It.IsAny<TransactionQueries.GetTodaysSalesByHourQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure("err"));

        var result = await _service.GetTodaysSalesByHour(CorrelationIdHelper.New(), Guid.NewGuid(), DateTime.UtcNow);

        result.IsFailed.ShouldBeTrue();
        _mockMediator.Verify(m => m.Send(It.IsAny<TransactionQueries.GetTodaysSalesByHourQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetTodaysSettlement_ReturnsFailure_WhenMediatorFails()
    {
        _mockMediator
            .Setup(m => m.Send(It.IsAny<SettlementQueries.GetTodaysSettlementQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure("err"));

        var result = await _service.GetTodaysSettlement(CorrelationIdHelper.New(), Guid.NewGuid(), DateTime.UtcNow);

        result.IsFailed.ShouldBeTrue();
        _mockMediator.Verify(m => m.Send(It.IsAny<SettlementQueries.GetTodaysSettlementQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetProductPerformance_ReturnsFailure_WhenMediatorFails()
    {
        _mockMediator
            .Setup(m => m.Send(It.IsAny<TransactionQueries.GetProductPerformanceQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure("err"));

        var result = await _service.GetProductPerformance(CorrelationIdHelper.New(), Guid.NewGuid(), DateTime.UtcNow.AddDays(-7), DateTime.UtcNow);

        result.IsFailed.ShouldBeTrue();
        _mockMediator.Verify(m => m.Send(It.IsAny<TransactionQueries.GetProductPerformanceQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetTransactionDetail_ReturnsFailure_WhenMediatorFails()
    {
        _mockMediator
            .Setup(m => m.Send(It.IsAny<TransactionQueries.GetTransactionDetailQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure("err"));

        var result = await _service.GetTransactionDetail(CorrelationIdHelper.New(), Guid.NewGuid(), DateTime.UtcNow.AddDays(-7), DateTime.UtcNow, null, null, null);

        result.IsFailed.ShouldBeTrue();
        _mockMediator.Verify(m => m.Send(It.IsAny<TransactionQueries.GetTransactionDetailQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetMerchantTransactionSummary_ReturnsFailure_WhenMediatorFails()
    {
        _mockMediator
            .Setup(m => m.Send(It.IsAny<TransactionQueries.GetMerchantTransactionSummaryQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure("err"));

        var result = await _service.GetMerchantTransactionSummary(CorrelationIdHelper.New(), Guid.NewGuid(), DateTime.UtcNow.AddDays(-7), DateTime.UtcNow, null, null);

        result.IsFailed.ShouldBeTrue();
        _mockMediator.Verify(m => m.Send(It.IsAny<TransactionQueries.GetMerchantTransactionSummaryQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetOperatorTransactionSummary_ReturnsFailure_WhenMediatorFails()
    {
        _mockMediator
            .Setup(m => m.Send(It.IsAny<TransactionQueries.GetOperatorTransactionSummaryQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure("err"));

        var result = await _service.GetOperatorTransactionSummary(CorrelationIdHelper.New(), Guid.NewGuid(), DateTime.UtcNow.AddDays(-7), DateTime.UtcNow, null, null);

        result.IsFailed.ShouldBeTrue();
        _mockMediator.Verify(m => m.Send(It.IsAny<TransactionQueries.GetOperatorTransactionSummaryQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}