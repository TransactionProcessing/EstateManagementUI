using EstateManagementUI.BusinessLogic.Models;
using MediatR;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.Requests;

public static class TransactionQueries {
    public record GetTodaysSalesQuery(CorrelationId CorrelationId, Guid EstateId, DateTime ComparisonDate) : IRequest<Result<TodaysSalesModel>>;
    public record GetTodaysFailedSalesQuery(CorrelationId CorrelationId, Guid EstateId, string ResponseCode, DateTime ComparisonDate) : IRequest<Result<TodaysSalesModel>>;
    public record GetTransactionDetailQuery(CorrelationId CorrelationId, Guid EstateId, DateTime StartDate, DateTime EndDate, List<Int32>? MerchantIds = null, List<Int32>? OperatorIds = null, List<Int32>? ProductIds = null) : IRequest<Result<TransactionModels.TransactionDetailReportResponse>>;
    public record GetMerchantTransactionSummaryQuery(CorrelationId CorrelationId, Guid EstateId, DateTime StartDate, DateTime EndDate, Int32? MerchantId = null, Int32? OperatorId = null) : IRequest<Result<TransactionModels.TransactionSummaryByMerchantResponse>>;
    public record GetOperatorTransactionSummaryQuery(CorrelationId CorrelationId,Guid EstateId, DateTime StartDate, DateTime EndDate, Int32? MerchantId = null, Int32? OperatorId = null) : IRequest<Result<TransactionModels.TransactionSummaryByOperatorResponse>>;
    public record GetProductPerformanceQuery(CorrelationId CorrelationId, Guid EstateId, DateTime StartDate, DateTime EndDate) : IRequest<Result<TransactionModels.ProductPerformanceResponse>>;
    public record GetTodaysSalesByHourQuery(CorrelationId CorrelationId, Guid EstateId, DateTime ComparisonDate) : IRequest<Result<List<TransactionModels.TodaysSalesByHourModel>>>;
}

public static class SettlementQueries {
    public record GetTodaysSettlementQuery(CorrelationId CorrelationId, Guid EstateId, DateTime ComparisonDate) : IRequest<Result<TodaysSettlementModel>>;
}

public static class Queries
{
    public record GetFileImportLogsListQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId, Guid MerchantId, DateTime StartDate, DateTime EndDate)
        : IRequest<Result<List<FileImportLogModel>>>;
    public record GetFileImportLogQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId, Guid MerchantId, Guid FileImportLogId)
        : IRequest<Result<FileImportLogModel>>;
    public record GetFileDetailsQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId, Guid FileId) : IRequest<Result<FileDetailsModel>>;
    public record GetComparisonDatesQuery(CorrelationId CorrelationId, Guid EstateId) : IRequest<Result<List<ComparisonDateModel>>>;
    
    public record GetTopProductDataQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId, int ResultCount) : IRequest<Result<List<TopBottomProductDataModel>>>;
    public record GetBottomProductDataQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId, int ResultCount) : IRequest<Result<List<TopBottomProductDataModel>>>;
    public record GetTopMerchantDataQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId, int ResultCount) : IRequest<Result<List<TopBottomMerchantDataModel>>>;
    public record GetBottomMerchantDataQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId, int ResultCount) : IRequest<Result<List<TopBottomMerchantDataModel>>>;
    public record GetTopOperatorDataQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId, int ResultCount) : IRequest<Result<List<TopBottomOperatorDataModel>>>;
    public record GetBottomOperatorDataQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId, int ResultCount) : IRequest<Result<List<TopBottomOperatorDataModel>>>;
    public record GetLastSettlementQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId) : IRequest<Result<LastSettlementModel>>;
    public record GetMerchantTransactionSummaryQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId, DateTime StartDate, DateTime EndDate, Guid? MerchantId = null, Guid? OperatorId = null, Guid? ProductId = null) : IRequest<Result<List<TransactionModels.TransactionSummaryByMerchantResponse>>>;
    public record GetMerchantSettlementHistoryQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId, Guid? MerchantId, DateTime StartDate, DateTime EndDate) : IRequest<Result<List<MerchantSettlementHistoryModel>>>;
    public record GetSettlementSummaryQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId, DateTime StartDate, DateTime EndDate, Guid? MerchantId = null, string? Status = null) : IRequest<Result<List<SettlementSummaryModel>>>;
}