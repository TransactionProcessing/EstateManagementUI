using EstateManagementUI.BlazorServer.UIServices;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using Imposter.Abstractions;
using Shouldly;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.Tests.UIServices;

public class TransactionUIServiceTests
{
    private readonly IMediatorImposter _mockMediator;
    private readonly TransactionUIService _service;

    public TransactionUIServiceTests()
    {
        this._mockMediator = new IMediatorImposter();
        this._service = new TransactionUIService(this._mockMediator.Instance());
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
            .Send<Result<BusinessLogic.Models.TodaysSalesModel>>(Arg<global::MediatR.IRequest<Result<BusinessLogic.Models.TodaysSalesModel>>>.Is(query =>
                query is TransactionQueries.GetTodaysSalesQuery todaysSalesQuery &&
                todaysSalesQuery.EstateId == estateId && todaysSalesQuery.ComparisonDate.Date == comparisonDate.Date
            ), Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(biz));

        var result = await this._service.GetTodaysSales(CorrelationIdHelper.New(), estateId, comparisonDate);

        result.IsSuccess.ShouldBeTrue();
        result.Data!.TodaysSalesCount.ShouldBe(10);

        this._mockMediator.Send<Result<BusinessLogic.Models.TodaysSalesModel>>(Arg<IRequest<Result<BusinessLogic.Models.TodaysSalesModel>>>.Is(query =>
            query is TransactionQueries.GetTodaysSalesQuery todaysSalesQuery &&
            todaysSalesQuery.EstateId == estateId && todaysSalesQuery.ComparisonDate.Date == comparisonDate.Date
        ), Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task GetTodaysSales_ReturnsFailure_WhenMediatorFails()
    {
        this._mockMediator
            .Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TodaysSalesModel>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TodaysSalesModel>>>.Any(), Arg<CancellationToken>.Any())
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
            .Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TodaysSalesModel>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TodaysSalesModel>>>.Any(), Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(biz));

        var result = await this._service.GetTodaysFailedSales(CorrelationIdHelper.New(), estateId, responseCode, comparisonDate);

        result.IsSuccess.ShouldBeTrue();
        this._mockMediator.Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TodaysSalesModel>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TodaysSalesModel>>>.Is(q =>
                ((dynamic)q).EstateId == estateId && ((dynamic)q).ResponseCode == responseCode && ((dynamic)q).ComparisonDate.Date == comparisonDate.Date
            ), Arg<CancellationToken>.Any()).Called(Count.Once());
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
            .Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.TransactionModels.TodaysSalesByHourModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.TransactionModels.TodaysSalesByHourModel>>>>.Any(), Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(bizList));

        var result = await this._service.GetTodaysSalesByHour(CorrelationIdHelper.New(), estateId, comparisonDate);

        result.IsSuccess.ShouldBeTrue();
        result.Data!.Count.ShouldBe(1);

        this._mockMediator.Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.TransactionModels.TodaysSalesByHourModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.TransactionModels.TodaysSalesByHourModel>>>>.Is(q =>
                ((dynamic)q).EstateId == estateId && ((dynamic)q).ComparisonDate.Date == comparisonDate.Date
            ), Arg<CancellationToken>.Any()).Called(Count.Once());
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
            .Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TodaysSettlementModel>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TodaysSettlementModel>>>.Any(), Arg<CancellationToken>.Any())
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
            .Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TransactionModels.ProductPerformanceResponse>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TransactionModels.ProductPerformanceResponse>>>.Any(), Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(biz));

        var result = await this._service.GetProductPerformance(CorrelationIdHelper.New(), estateId, start, end);

        result.IsSuccess.ShouldBeTrue();
        this._mockMediator.Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TransactionModels.ProductPerformanceResponse>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TransactionModels.ProductPerformanceResponse>>>.Is(q =>
                ((dynamic)q).EstateId == estateId && ((dynamic)q).StartDate == start && ((dynamic)q).EndDate == end
            ), Arg<CancellationToken>.Any()).Called(Count.Once());
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
            .Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TransactionModels.TransactionDetailReportResponse>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TransactionModels.TransactionDetailReportResponse>>>.Any(), Arg<CancellationToken>.Any())
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

        this._mockMediator.Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TransactionModels.TransactionDetailReportResponse>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TransactionModels.TransactionDetailReportResponse>>>.Is(q =>
                ((dynamic)q).EstateId == estateId && ((dynamic)q).MerchantIds == merchantIds && ((dynamic)q).OperatorIds == operatorIds && ((dynamic)q).ProductIds == productIds
            ), Arg<CancellationToken>.Any()).Called(Count.Once());
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

        this._mockMediator.Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TransactionModels.TransactionSummaryByMerchantResponse>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TransactionModels.TransactionSummaryByMerchantResponse>>>.Any(), Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(bizMerchant));

        this._mockMediator.Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TransactionModels.TransactionSummaryByOperatorResponse>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TransactionModels.TransactionSummaryByOperatorResponse>>>.Any(), Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(bizOperator));

        var merchantResult = await this._service.GetMerchantTransactionSummary(CorrelationIdHelper.New(), estateId, start, end, 5, 6);
        merchantResult.IsSuccess.ShouldBeTrue();

        var operatorResult = await this._service.GetOperatorTransactionSummary(CorrelationIdHelper.New(), estateId, start, end, 7, 8);
        operatorResult.IsSuccess.ShouldBeTrue();

        this._mockMediator.Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TransactionModels.TransactionSummaryByMerchantResponse>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TransactionModels.TransactionSummaryByMerchantResponse>>>.Is(q =>
            ((dynamic)q).EstateId == estateId && ((dynamic)q).MerchantId == 5 && ((dynamic)q).OperatorId == 6
        ), Arg<CancellationToken>.Any()).Called(Count.Once());

        this._mockMediator.Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TransactionModels.TransactionSummaryByOperatorResponse>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TransactionModels.TransactionSummaryByOperatorResponse>>>.Is(q =>
            ((dynamic)q).EstateId == estateId && ((dynamic)q).MerchantId == 7 && ((dynamic)q).OperatorId == 8
        ), Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task GetTodaysFailedSales_ReturnsFailure_WhenMediatorFails()
    {
        _mockMediator
            .Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TodaysSalesModel>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TodaysSalesModel>>>.Any(), Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("err"));

        var result = await _service.GetTodaysFailedSales(CorrelationIdHelper.New(), Guid.NewGuid(), "RC", DateTime.UtcNow);

        result.IsFailed.ShouldBeTrue();
        _mockMediator.Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TodaysSalesModel>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TodaysSalesModel>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task GetTodaysSalesByHour_ReturnsFailure_WhenMediatorFails()
    {
        _mockMediator
            .Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.TransactionModels.TodaysSalesByHourModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.TransactionModels.TodaysSalesByHourModel>>>>.Any(), Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("err"));

        var result = await _service.GetTodaysSalesByHour(CorrelationIdHelper.New(), Guid.NewGuid(), DateTime.UtcNow);

        result.IsFailed.ShouldBeTrue();
        _mockMediator.Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.TransactionModels.TodaysSalesByHourModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.TransactionModels.TodaysSalesByHourModel>>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task GetTodaysSettlement_ReturnsFailure_WhenMediatorFails()
    {
        _mockMediator
            .Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TodaysSettlementModel>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TodaysSettlementModel>>>.Any(), Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("err"));

        var result = await _service.GetTodaysSettlement(CorrelationIdHelper.New(), Guid.NewGuid(), DateTime.UtcNow);

        result.IsFailed.ShouldBeTrue();
        _mockMediator.Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TodaysSettlementModel>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TodaysSettlementModel>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task GetProductPerformance_ReturnsFailure_WhenMediatorFails()
    {
        _mockMediator
            .Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TransactionModels.ProductPerformanceResponse>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TransactionModels.ProductPerformanceResponse>>>.Any(), Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("err"));

        var result = await _service.GetProductPerformance(CorrelationIdHelper.New(), Guid.NewGuid(), DateTime.UtcNow.AddDays(-7), DateTime.UtcNow);

        result.IsFailed.ShouldBeTrue();
        _mockMediator.Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TransactionModels.ProductPerformanceResponse>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TransactionModels.ProductPerformanceResponse>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task GetTransactionDetail_ReturnsFailure_WhenMediatorFails()
    {
        _mockMediator
            .Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TransactionModels.TransactionDetailReportResponse>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TransactionModels.TransactionDetailReportResponse>>>.Any(), Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("err"));

        var result = await _service.GetTransactionDetail(CorrelationIdHelper.New(), Guid.NewGuid(), DateTime.UtcNow.AddDays(-7), DateTime.UtcNow, null, null, null);

        result.IsFailed.ShouldBeTrue();
        _mockMediator.Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TransactionModels.TransactionDetailReportResponse>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TransactionModels.TransactionDetailReportResponse>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task GetMerchantTransactionSummary_ReturnsFailure_WhenMediatorFails()
    {
        _mockMediator
            .Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TransactionModels.TransactionSummaryByMerchantResponse>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TransactionModels.TransactionSummaryByMerchantResponse>>>.Any(), Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("err"));

        var result = await _service.GetMerchantTransactionSummary(CorrelationIdHelper.New(), Guid.NewGuid(), DateTime.UtcNow.AddDays(-7), DateTime.UtcNow, null, null);

        result.IsFailed.ShouldBeTrue();
        _mockMediator.Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TransactionModels.TransactionSummaryByMerchantResponse>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TransactionModels.TransactionSummaryByMerchantResponse>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task GetOperatorTransactionSummary_ReturnsFailure_WhenMediatorFails()
    {
        _mockMediator
            .Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TransactionModels.TransactionSummaryByOperatorResponse>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TransactionModels.TransactionSummaryByOperatorResponse>>>.Any(), Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("err"));

        var result = await _service.GetOperatorTransactionSummary(CorrelationIdHelper.New(), Guid.NewGuid(), DateTime.UtcNow.AddDays(-7), DateTime.UtcNow, null, null);

        result.IsFailed.ShouldBeTrue();
        _mockMediator.Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TransactionModels.TransactionSummaryByOperatorResponse>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.TransactionModels.TransactionSummaryByOperatorResponse>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
    }
}
