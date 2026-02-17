using EstateManagementUI.BusinessLogic.Client;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.RequestHandlers;

public class TransactionRequestHandler : IRequestHandler<TransactionQueries.GetTodaysSalesQuery, Result<TodaysSalesModel>>, 
    IRequestHandler<TransactionQueries.GetTransactionDetailQuery, Result<TransactionModels.TransactionDetailReportResponse>>,
    IRequestHandler<TransactionQueries.GetTodaysFailedSalesQuery, Result<TodaysSalesModel>>,
    IRequestHandler<TransactionQueries.GetMerchantTransactionSummaryQuery, Result<TransactionModels.TransactionSummaryByMerchantResponse>>,
    IRequestHandler<TransactionQueries.GetOperatorTransactionSummaryQuery, Result<TransactionModels.TransactionSummaryByOperatorResponse>>,
    IRequestHandler<TransactionQueries.GetProductPerformanceQuery, Result<TransactionModels.ProductPerformanceResponse>>,
    IRequestHandler<TransactionQueries.GetTodaysSalesByHourQuery, Result<List<TransactionModels.TodaysSalesByHourModel>>>
{
    private readonly IApiClient ApiClient;
    public TransactionRequestHandler(IApiClient apiClient) {
        this.ApiClient = apiClient;
    }

    public async Task<Result<TransactionModels.TransactionDetailReportResponse>> Handle(TransactionQueries.GetTransactionDetailQuery request,
                                                                                        CancellationToken cancellationToken) {
        return await this.ApiClient.GetTransactionDetailReport(request, cancellationToken);
    }

    public async Task<Result<TodaysSalesModel>> Handle(TransactionQueries.GetTodaysSalesQuery request,
                                                       CancellationToken cancellationToken) {
        return await this.ApiClient.GetTodaysSales(request, cancellationToken);
    }

    public async Task<Result<TodaysSalesModel>> Handle(TransactionQueries.GetTodaysFailedSalesQuery request,
                                                       CancellationToken cancellationToken) {
        return await this.ApiClient.GetTodaysFailedSales(request, cancellationToken);
    }

    public async Task<Result<TransactionModels.TransactionSummaryByMerchantResponse>> Handle(TransactionQueries.GetMerchantTransactionSummaryQuery request,
                                                                                        CancellationToken cancellationToken) {
        return await this.ApiClient.GetMerchantTransactionSummary(request, cancellationToken);
    }

    public async Task<Result<TransactionModels.TransactionSummaryByOperatorResponse>> Handle(TransactionQueries.GetOperatorTransactionSummaryQuery request,
                                                                                             CancellationToken cancellationToken) {
        return await this.ApiClient.GetOperatorTransactionSummary(request, cancellationToken);
    }

    public async Task<Result<TransactionModels.ProductPerformanceResponse>> Handle(TransactionQueries.GetProductPerformanceQuery request,
                                                                                   CancellationToken cancellationToken) {
        return await this.ApiClient.GetProductPerformance(request, cancellationToken);
    }

    public async Task<Result<List<TransactionModels.TodaysSalesByHourModel>>> Handle(TransactionQueries.GetTodaysSalesByHourQuery request,
                                                                                     CancellationToken cancellationToken) {
        return await this.ApiClient.GetTodaysSalesByHour(request, cancellationToken);
    }
}

public class SettlementRequestHandler : IRequestHandler<SettlementQueries.GetTodaysSettlementQuery, Result<TodaysSettlementModel>> {

    private readonly IApiClient ApiClient;
    public SettlementRequestHandler(IApiClient apiClient)
    {
        this.ApiClient = apiClient;
    }

    public async Task<Result<TodaysSettlementModel>> Handle(SettlementQueries.GetTodaysSettlementQuery request,
                                                            CancellationToken cancellationToken) {
        return await this.ApiClient.GetTodaysSettlement(request, cancellationToken);
    }
}