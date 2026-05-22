using EstateManagementUI.BusinessLogic.Models;
using MediatR;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.Requests;

public static class Queries
{
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

public static class FileProcessingQueries
{
    public record GetFileImportLogsListQuery(CorrelationId CorrelationId, Guid EstateId, Guid? MerchantId, DateTime StartDate, DateTime EndDate)
        : IRequest<Result<List<FileProcessingModels.FileImportLogDetailsModel>>>;
    public record GetFileImportLogQuery(CorrelationId CorrelationId, Guid EstateId, Guid? MerchantId, Guid FileImportLogId)
        : IRequest<Result<FileProcessingModels.FileImportLogDetailsModel>>;
}