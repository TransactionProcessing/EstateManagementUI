using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagmentUI.BusinessLogic.Requests;
using EstateReportingAPI.DataTransferObjects;
using MediatR;
using Microsoft.IdentityModel.Abstractions;
using SimpleResults;
using static EstateManagmentUI.BusinessLogic.Requests.Queries;

namespace EstateManagementUI.BusinessLogic.RequestHandlers;

public class ReportingRequestHandler : IRequestHandler<Queries.GetComparisonDatesQuery, Result<List<ComparisonDateModel>>>,
IRequestHandler<GetTodaysSalesQuery, Result<TodaysSalesModel>>,
    IRequestHandler<GetTodaysSettlementQuery, Result<TodaysSettlementModel>>,
IRequestHandler<GetTodaysSalesCountByHourQuery, Result<List<TodaysSalesCountByHourModel>>>,
IRequestHandler<GetTodaysSalesValueByHourQuery, Result<List<TodaysSalesValueByHourModel>>>,
IRequestHandler<GetMerchantKpiQuery, Result<MerchantKpiModel>>,
IRequestHandler<GetTodaysFailedSalesQuery, Result<TodaysSalesModel>>,
IRequestHandler<GetTopProductDataQuery, Result<List<TopBottomProductDataModel>>>,
IRequestHandler<GetBottomProductDataQuery, Result<List<TopBottomProductDataModel>>>,
IRequestHandler<GetTopMerchantDataQuery, Result<List<TopBottomMerchantDataModel>>>,
IRequestHandler<GetBottomMerchantDataQuery, Result<List<TopBottomMerchantDataModel>>>,
IRequestHandler<GetTopOperatorDataQuery, Result<List<TopBottomOperatorDataModel>>>,
IRequestHandler<GetBottomOperatorDataQuery, Result<List<TopBottomOperatorDataModel>>>,
IRequestHandler<GetLastSettlementQuery, Result<LastSettlementModel>>,
IRequestHandler<GetMerchantTransactionSummaryQuery, Result<List<MerchantTransactionSummaryModel>>> {

    private readonly IApiClient ApiClient;
    public ReportingRequestHandler(IApiClient apiClient)
    {
        this.ApiClient = apiClient;
    }

    public async Task<Result<List<ComparisonDateModel>>> Handle(Queries.GetComparisonDatesQuery request,
                                                                CancellationToken cancellationToken) {
        Result<List<ComparisonDateModel>> model = await this.ApiClient.GetComparisonDates(request.AccessToken, Guid.Empty, request.EstateId, cancellationToken);
        return model;
    }

    public async Task<Result<TodaysSalesModel>> Handle(GetTodaysSalesQuery request,
                                                       CancellationToken cancellationToken) {
        Result<TodaysSalesModel> model = await this.ApiClient.GetTodaysSales(request.AccessToken, request.EstateId, request.EstateId, null,
            null, request.ComparisonDate, cancellationToken);
        return model;
    }

    public async Task<Result<TodaysSettlementModel>> Handle(GetTodaysSettlementQuery request,
                                                            CancellationToken cancellationToken) {
        Result<TodaysSettlementModel> model = await this.ApiClient.GetTodaysSettlement(request.AccessToken, Guid.Empty, request.EstateId, null,
            null, request.ComparisonDate, cancellationToken);
        return model;
    }

    public async Task<Result<List<TodaysSalesCountByHourModel>>> Handle(GetTodaysSalesCountByHourQuery request,
                                                                        CancellationToken cancellationToken) {
        Result<List<TodaysSalesCountByHourModel>> model = await this.ApiClient.GetTodaysSalesCountByHour(request.AccessToken, Guid.Empty, request.EstateId,
            null, null, request.ComparisonDate, cancellationToken);
        return model;
    }

    public async Task<Result<List<TodaysSalesValueByHourModel>>> Handle(GetTodaysSalesValueByHourQuery request,
                                                                        CancellationToken cancellationToken) {
        Result<List<TodaysSalesValueByHourModel>> model = await this.ApiClient.GetTodaysSalesValueByHour(request.AccessToken, Guid.Empty, request.EstateId,
            null, null, request.ComparisonDate, cancellationToken);
        return model;
    }

    public async Task<Result<MerchantKpiModel>> Handle(GetMerchantKpiQuery request,
                                                       CancellationToken cancellationToken) {
        Result<MerchantKpiModel> model = await this.ApiClient.GetMerchantKpi(request.AccessToken, request.EstateId, cancellationToken);
        return model;
    }

    public async Task<Result<TodaysSalesModel>> Handle(GetTodaysFailedSalesQuery request,
                                                       CancellationToken cancellationToken) {
        Result<TodaysSalesModel> model = await this.ApiClient.GetTodaysFailedSales(request.AccessToken, request.EstateId,
            request.ResponseCode, request.ComparisonDate, cancellationToken);
        return model;
    }

    public async Task<Result<List<TopBottomProductDataModel>>> Handle(GetTopProductDataQuery request,
                                                                      CancellationToken cancellationToken) {
        Result<List<TopBottomProductDataModel>> model = await this.ApiClient.GetTopBottomProductData(request.AccessToken, request.EstateId, TopBottom.Top,
            request.ResultCount, cancellationToken);
        return model;
    }

    public async Task<Result<List<TopBottomProductDataModel>>> Handle(GetBottomProductDataQuery request,
                                                                      CancellationToken cancellationToken) {
        Result<List<TopBottomProductDataModel>> model = await this.ApiClient.GetTopBottomProductData(request.AccessToken, request.EstateId, TopBottom.Bottom,
            request.ResultCount, cancellationToken);
        return model;
    }

    public async Task<Result<List<TopBottomMerchantDataModel>>> Handle(GetTopMerchantDataQuery request,
                                                                       CancellationToken cancellationToken) {
        Result<List<TopBottomMerchantDataModel>> model = await this.ApiClient.GetTopBottomMerchantData(request.AccessToken, request.EstateId, TopBottom.Top,
            request.ResultCount, cancellationToken);
        return model;
    }

    public async Task<Result<List<TopBottomMerchantDataModel>>> Handle(GetBottomMerchantDataQuery request,
                                                                       CancellationToken cancellationToken) {
        Result<List<TopBottomMerchantDataModel>> model = await this.ApiClient.GetTopBottomMerchantData(request.AccessToken, request.EstateId, TopBottom.Bottom,
            request.ResultCount, cancellationToken);
        return model;
    }

    public async Task<Result<List<TopBottomOperatorDataModel>>> Handle(GetTopOperatorDataQuery request,
                                                                       CancellationToken cancellationToken) {
        Result<List<TopBottomOperatorDataModel>> model = await this.ApiClient.GetTopBottomOperatorData(request.AccessToken, request.EstateId, TopBottom.Top,
            request.ResultCount, cancellationToken);
        return model;
    }

    public async Task<Result<List<TopBottomOperatorDataModel>>> Handle(GetBottomOperatorDataQuery request,
                                                                       CancellationToken cancellationToken) {
        Result<List<TopBottomOperatorDataModel>> model = await this.ApiClient.GetTopBottomOperatorData(request.AccessToken, request.EstateId, TopBottom.Bottom,
            request.ResultCount, cancellationToken);
        return model;
    }

    public async Task<Result<LastSettlementModel>> Handle(GetLastSettlementQuery request,
                                                          CancellationToken cancellationToken) {
        Result<LastSettlementModel> model = await this.ApiClient.GetLastSettlement(request.AccessToken, request.EstateId, null, null,
            cancellationToken);
        return model;
    }

    public async Task<Result<List<MerchantTransactionSummaryModel>>> Handle(GetMerchantTransactionSummaryQuery request,
                                                                             CancellationToken cancellationToken) {
        // TODO: Replace with actual API call when endpoint is available
        // For now, return mock data for testing
        var merchants = await this.ApiClient.GetMerchants(request.AccessToken, Guid.Empty, request.EstateId, cancellationToken);
        
        if (!merchants.IsSuccess) {
            return Result.Failure<List<MerchantTransactionSummaryModel>>(merchants.Message);
        }

        var summary = new List<MerchantTransactionSummaryModel>();
        var random = new Random(42); // Use seed for consistent test data
        
        foreach (var merchant in merchants.Data) {
            var totalCount = random.Next(100, 1000);
            var successfulCount = (int)(totalCount * 0.85); // 85% success rate
            var failedCount = totalCount - successfulCount;
            var totalValue = (decimal)(random.NextDouble() * 50000 + 10000);
            
            summary.Add(new MerchantTransactionSummaryModel {
                MerchantId = merchant.Id,
                MerchantName = merchant.Name,
                TotalTransactionCount = totalCount,
                TotalTransactionValue = Math.Round(totalValue, 2),
                AverageTransactionValue = Math.Round(totalValue / totalCount, 2),
                SuccessfulTransactionCount = successfulCount,
                FailedTransactionCount = failedCount
            });
        }
        
        // Apply filters if specified
        if (request.MerchantId.HasValue) {
            summary = summary.Where(s => s.MerchantId == request.MerchantId.Value).ToList();
        }
        
        return Result.Success(summary);
    }
}