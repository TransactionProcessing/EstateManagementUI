using EstateManagementUI.BusinessLogic.Client;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.RequestHandlers;

public class DashboardRequestHandler : IRequestHandler<Queries.GetTodaysSalesQuery, Result<TodaysSalesModel>>,
    IRequestHandler<Queries.GetTodaysSettlementQuery, Result<TodaysSettlementModel>>,
    IRequestHandler<Queries.GetTodaysSalesCountByHourQuery, Result<List<TodaysSalesCountByHourModel>>>,
    IRequestHandler<Queries.GetTodaysSalesValueByHourQuery, Result<List<TodaysSalesValueByHourModel>>>,
    IRequestHandler<MerchantQueries.GetMerchantKpiQuery, Result<MerchantModels.MerchantKpiModel>>,
    IRequestHandler<Queries.GetTodaysFailedSalesQuery, Result<TodaysSalesModel>>,
    IRequestHandler<Queries.GetTopProductDataQuery, Result<List<TopBottomProductDataModel>>>,
    IRequestHandler<Queries.GetBottomProductDataQuery, Result<List<TopBottomProductDataModel>>>,
    IRequestHandler<Queries.GetTopMerchantDataQuery, Result<List<TopBottomMerchantDataModel>>>,
    IRequestHandler<Queries.GetBottomMerchantDataQuery, Result<List<TopBottomMerchantDataModel>>>,
    IRequestHandler<Queries.GetTopOperatorDataQuery, Result<List<TopBottomOperatorDataModel>>>,
    IRequestHandler<Queries.GetBottomOperatorDataQuery, Result<List<TopBottomOperatorDataModel>>>,
    IRequestHandler<Queries.GetLastSettlementQuery, Result<LastSettlementModel>>,
    IRequestHandler<Queries.GetTransactionDetailQuery, Result<List<TransactionDetailModel>>>
{
    private readonly IApiClient ApiClient;

    public DashboardRequestHandler(IApiClient apiClient) {
        this.ApiClient = apiClient;
            
    }

    // Implementations similar to above handlers returning stub data
    public async Task<Result<TodaysSalesModel>> Handle(Queries.GetTodaysSalesQuery request,
                                                       CancellationToken cancellationToken) {
        return await this.ApiClient.GetTodaysSales(request, cancellationToken);
    }

    public async Task<Result<TodaysSettlementModel>> Handle(Queries.GetTodaysSettlementQuery request,
                                                            CancellationToken cancellationToken) {
        return Result.Success(StubTestData.GetMockTodaysSettlement());
    }

    public async Task<Result<List<TodaysSalesCountByHourModel>>> Handle(Queries.GetTodaysSalesCountByHourQuery request,
                                                                        CancellationToken cancellationToken) {
        return Result.Success(StubTestData.GetMockSalesCountByHour());
    }

    public async Task<Result<List<TodaysSalesValueByHourModel>>> Handle(Queries.GetTodaysSalesValueByHourQuery request,
                                                                        CancellationToken cancellationToken) {
        return Result.Success(StubTestData.GetMockSalesValueByHour());
    }

    public async Task<Result<MerchantModels.MerchantKpiModel>> Handle(MerchantQueries.GetMerchantKpiQuery request,
                                                                      CancellationToken cancellationToken) {
        return await this.ApiClient.GetMerchantKpi(request, cancellationToken);
    }

    public async Task<Result<TodaysSalesModel>> Handle(Queries.GetTodaysFailedSalesQuery request,
                                                       CancellationToken cancellationToken) {
        return await this.ApiClient.GetTodaysFailedSales(request, cancellationToken);
    }

    public async Task<Result<List<TopBottomProductDataModel>>> Handle(Queries.GetTopProductDataQuery request,
                                                                      CancellationToken cancellationToken) {
        return Result.Success(StubTestData.GetMockTopProducts());
    }

    public async Task<Result<List<TopBottomProductDataModel>>> Handle(Queries.GetBottomProductDataQuery request,
                                                                      CancellationToken cancellationToken) {
        return Result.Success(StubTestData.GetMockBottomProducts());
    }

    public async Task<Result<List<TopBottomMerchantDataModel>>> Handle(Queries.GetTopMerchantDataQuery request,
                                                                       CancellationToken cancellationToken) {
        return Result.Success(StubTestData.GetMockTopMerchants());
    }

    public async Task<Result<List<TopBottomMerchantDataModel>>> Handle(Queries.GetBottomMerchantDataQuery request,
                                                                       CancellationToken cancellationToken) {
        return Result.Success(StubTestData.GetMockBottomMerchants());
    }

    public async Task<Result<List<TopBottomOperatorDataModel>>> Handle(Queries.GetTopOperatorDataQuery request,
                                                                       CancellationToken cancellationToken) {
        return Result.Success(StubTestData.GetMockTopOperators());
    }

    public async Task<Result<List<TopBottomOperatorDataModel>>> Handle(Queries.GetBottomOperatorDataQuery request,
                                                                       CancellationToken cancellationToken) {
        return Result.Success(StubTestData.GetMockBottomOperators());
    }

    public async Task<Result<LastSettlementModel>> Handle(Queries.GetLastSettlementQuery request,
                                                          CancellationToken cancellationToken) {
        return Result.Success(StubTestData.GetMockLastSettlement());
    }

    public async Task<Result<List<TransactionDetailModel>>> Handle(Queries.GetTransactionDetailQuery request,
                                                                   CancellationToken cancellationToken) {
        return Result.Success(StubTestData.GetMockTransactionDetails(request));
    }
}